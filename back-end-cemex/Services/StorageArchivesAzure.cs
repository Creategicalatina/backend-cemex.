using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace back_end_cemex.Services
{
    public class StorageArchivesAzure : IStorageArchives
    {
        private readonly string connectionString;
        public StorageArchivesAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }
        public async Task<string> DeleteArchive(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return ("No existe el archivo").ToString();
            }


            var client = new BlobContainerClient(connectionString, contenedor);
            await client.CreateIfNotExistsAsync();
            var archive = Path.GetFileName(ruta);
            var blob = client.GetBlobClient(archive);
            await blob.DeleteIfExistsAsync();
            return ("El archivo actualizó correctamente").ToString();
        }

        public async Task<string> EditArchive(byte[] contenido, string name, string extension, string contenedor, string ruta, string contentType)
        {
            await DeleteArchive(ruta, contenedor);
            return await SaveArchive(contenido, name, extension, contenedor, contentType);
        }

        public async Task<string> SaveArchive(byte[] contenido, string name, string extension, string contenedor, string contentType)
        {
            var client = new BlobContainerClient(connectionString, contenedor);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicyAsync(PublicAccessType.Blob);

            var archiveName = $"{name}{extension}";
            var blob = client.GetBlobClient(archiveName);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeader;

            await blob.UploadAsync(new BinaryData(contenido), blobUploadOptions);
            return blob.Uri.ToString();
        }
    }
}
