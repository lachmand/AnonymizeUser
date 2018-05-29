using Microsoft.WindowsAzure.Storage.Blob;

namespace AnonymizeUser
{
    public interface IBlobUtility
    {
        void DeleteBlob(string url);
        void DeleteBlob(string container, string filename);

        CloudBlob RetrieveBlob(string url);


        CloudBlob RetrieveBlob(string container, string filename);


        void AddBlob(string container, string filePath);


        bool BlobExists(string container, string filename);


        void SaveBlobToFile(CloudBlob blob);

    }
}