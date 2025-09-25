using Azure.Storage.Files.Shares;
using MongoDB.Driver.Core.Configuration;
using RulesEngine.Application.InputSources;
using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.InputSourcesEntities;
using static Azure.Core.HttpHeader;

namespace RulesEngine.Application.InputSources.FromFile
{
    public class InvoiceNumberFromFile : IInputSource<InvoiceNumberFile>
    {
        private const int InvoiceNumber = 0;
        private readonly SharedResource _resource;
        private readonly string _connectionString;
        public InvoiceNumberFromFile(string connectionString, SharedResource resource)
        {
            _resource = resource;
            _connectionString = connectionString;
        }
        public async Task<List<InvoiceNumberFile>> Create()
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
                                    return new InvoiceNumberFile
                                    {
                                        InvoiceNumber = values[InvoiceNumber],
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
