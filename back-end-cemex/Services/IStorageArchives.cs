using System.Threading.Tasks;

namespace back_end_cemex.Services
{
    public interface IStorageArchives
    {
        Task<string> SaveArchive(byte[] contenido, string name, string extension, string contenedor,
            string contentType);
        Task<string> EditArchive(byte[] contenido, string name, string extension, string contenedor,
            string ruta, string contentType);
        Task<string> DeleteArchive(string ruta, string contenedor);
    }
}
