using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Microsoft.WindowsAzure.Storage.File;

namespace AnonymizeUser
{
    public  class BlobUtility : IBlobUtility
    {
        private static string BLOB_CONNECTION_STRING_KEY = "BlobConnection";
        #region Ctor
        public BlobUtility()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <example>https://csaf08a9da082e6x42ddx837.blob.core.windows.net/test-container/test.jpg</example>
        public void DeleteBlob(string url)
        {
            string[] parts= url.Split('/');

            if (parts.Length < 2)
            {
                throw new ArgumentException("Url needs to have at least the container and blob reference separated by '/'");
            }

            string container = parts[parts.Length - 2];
            string blobRef = parts[parts.Length - 1];
            DeleteBlob(container, blobRef);
        }

        public void DeleteBlob(string container, string filename)
        {
            CloudBlobContainer blobcontainer = GetBLOBRef(container);
            CloudBlob blob = blobcontainer.GetBlobReference(filename);
            blob?.DeleteIfExists(
                deleteSnapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);
        }
        public CloudBlob RetrieveBlob(string url)
        {
            string[] parts = url.Split('/');

            if (parts.Length < 2)
            {
                throw new ArgumentException("Url needs to have at least the container and blob reference separated by '/'");
            }

            string container = parts[parts.Length - 2];
            string blobRef = parts[parts.Length - 1];

            return RetrieveBlob(container, blobRef);
        }

        public CloudBlob RetrieveBlob(string container, string filename)
        {
            CloudBlobContainer blobcontainer = GetBLOBRef(container);

            CloudBlob cloudBlob = blobcontainer.GetBlobReference(filename);

            return cloudBlob;
        }

        public void AddBlob(string container, string filePath)
        {
            
            if (filePath.Length > 0)
            {
                string blobName = filePath.Split('\\').Last();
                CloudBlobContainer blobcontainer = GetBLOBRef(container);
                blobcontainer.GetBlockBlobReference(blobName).UploadFromFile(filePath);
            }
        }

        public bool BlobExists(string container, string filename)
        {
            CloudBlob blob =RetrieveBlob(container, filename);
            return blob != null && blob.Exists();
        }

        public void SaveBlobToFile(CloudBlob blob)
        {
            if (blob != null)
            {
                blob.DownloadToFile(blob.Name, FileMode.Create);
            }
        }
        #endregion

        public CloudBlobContainer GetBLOBRef(string container)
        {
            
            CloudStorageAccount storageac = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings.Get(BLOB_CONNECTION_STRING_KEY));
            CloudBlobClient blobclient = storageac.CreateCloudBlobClient();
            CloudBlobContainer blobcontainer = blobclient.GetContainerReference(container);
            if (blobcontainer.CreateIfNotExists())
                blobcontainer.SetPermissions(new BlobContainerPermissions
                { PublicAccess = BlobContainerPublicAccessType.Blob });
            return blobcontainer;
        }
    }//class
}//ns
