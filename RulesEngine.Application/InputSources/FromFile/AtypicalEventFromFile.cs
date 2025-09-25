using Azure.Storage.Files.Shares;
using RulesEngine.Application.InputSources;
using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.InputSourcesEntities;

namespace RulesEngine.Application.InputSources.FromFile
{
    public class AtypicalEventFromFile : IInputSource<AtypicalEvent>
    {
        private readonly string _connectionString;
        private readonly SharedResource _resource;

        private const int soatNumber = 0;
        private const int plate = 1;
        private const int victimId = 4;
        private const int alertDescription = 8;

        public AtypicalEventFromFile(string connectionString, SharedResource resource)
        {
            _connectionString = connectionString;
            _resource = resource;
        }

        public async Task<List<AtypicalEvent>> Create()
        {
            // Se genera una instancia de la clase "ShareClient"
            ShareClient share = new(_connectionString, _resource.Share);

            // Se comprueba si el recurso compartido asociado existe en la cuenta de almacenamiento
            if (await share.ExistsAsync())
            {
                // Se genera una instancia de la clase "ShareDirectoryClient"
                ShareDirectoryClient directory = share.GetDirectoryClient(_resource.Directory);

                // Se comprueba si el directorio asociado existe en el recurso compartido
                if (await directory.ExistsAsync())
                {
                    // Se genera una instancia de la clase "ShareFileClient"
                    ShareFileClient file = directory.GetFileClient(_resource.FileName);

                    // Se comprueba si el archivo asociado existe en el recurso compartido
                    if (await file.ExistsAsync())
                    {
                        // Se abre un flujo para leer del archivo y obtener los datos
                        using (var st = await file.OpenReadAsync())
                        {
                            using (StreamReader streamReader = new StreamReader(st))
                            {
                                var text = await streamReader.ReadToEndAsync();

                                var lines = text.Replace("\n", "").Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                lines = lines.Skip(1).ToArray();

                                var entities = lines.Select(line =>
                                {
                                    var values = line.Split(';');
                                    return new AtypicalEvent
                                    {
                                        SoatNumber = values[soatNumber],
                                        LicensePlate = values[plate],
                                        VictimId = values[victimId],
                                        AlertDescription = values[alertDescription]
                                    };
                                }).ToList();

                                return await Task.FromResult(entities);
                            }
                        }
                    }
                }
            }
            return new();
        }
    }
}