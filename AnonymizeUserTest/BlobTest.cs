using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AnonymizeUser;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AnonymizeUserTest
{
    [TestClass]
    public class BlobTest
    {
        private const string CONTAINER = "test-container";
        private const string FILENAME = "test.jpg";

        [TestMethod]
        public void AddBlob()
        {
            string path = string.Concat(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"\data\test.jpg");

            AnonymizeUser.BlobUtility blobUtility = new BlobUtility();

            blobUtility.DeleteBlob(CONTAINER, FILENAME);
            Assert.IsFalse(blobUtility.BlobExists(CONTAINER, FILENAME));

            blobUtility.AddBlob(CONTAINER, path);
            Assert.IsTrue(blobUtility.BlobExists(CONTAINER, FILENAME));
        }

        [TestMethod]
        public void RemoveBlob()
        {
            AnonymizeUser.BlobUtility blobUtility = new BlobUtility();

            if(blobUtility.BlobExists(CONTAINER,FILENAME))
            {
                blobUtility.DeleteBlob(CONTAINER, FILENAME);
            }

            Assert.IsFalse(blobUtility.BlobExists(CONTAINER, FILENAME));
        }

        [TestMethod]
        public void RetrieveBlob()
        {
            AnonymizeUser.BlobUtility blobUtility = new BlobUtility();

            if (!blobUtility.BlobExists(CONTAINER, FILENAME))
            {
                string path = string.Concat(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"\data\test.jpg");

                blobUtility.AddBlob(CONTAINER, path);
            }

            CloudBlob cloudBlob = blobUtility.RetrieveBlob(CONTAINER, FILENAME);
            Assert.IsTrue(blobUtility.BlobExists(CONTAINER, FILENAME));
        }

    }//class
}//ns
