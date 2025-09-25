using Azure.Storage.Files.Shares;
using RulesEngine.Application.InputSources;
using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.InputSourcesEntities;

namespace RulesEngine.Application.InputSources.FromFile
{
    public class IpsPhoneVerificationFromFIle : IInputSource<IpsPhoneVerification>
    {
        private readonly string _connectionString;
        private readonly SharedResource _resource;
        
        private const int NitIps = 0;
        private const int NameIps = 1;
        private const int Department = 2;
        private const int City = 3;
        private const int StartDate = 4;
        private const int EndDate = 5;
        private const int ObservationCrossing = 6;
        private const int ResearchFirm = 7;
        private const int Type = 8;
        public IpsPhoneVerificationFromFIle(string connectionString, SharedResource resource)
        {
            _connectionString = connectionString;
            _resource = resource;
        }
        public async Task<List<IpsPhoneVerification>> Create()
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
                                    return new IpsPhoneVerification
                                    {
                                        NitIps = values[NitIps],
                                        NameIps = values[NameIps],
                                        Department = values[Department],
                                        City = values[City],
                                        StartDate = values[StartDate],
                                        EndDate = values[EndDate],
                                        ObservationCrossing = values[ObservationCrossing],
                                        ResearchFirm = values[ResearchFirm],
                                        Type = values[Type]
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
