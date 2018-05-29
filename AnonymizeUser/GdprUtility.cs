using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AnonymizeUser
{
    public class GdprUtility
    {
        private IBlobUtility _blobUtility;
        private IMySqlUtility _mySqlUtility;
        protected GdprUtility()
        {
                
        }

        public GdprUtility(IBlobUtility blobUtility, IMySqlUtility mySqlUtility)
        {
            _blobUtility = blobUtility;
            _mySqlUtility = mySqlUtility;
        }

        public void Anonymize(AnonymizeToken token)
        {
            IEnumerable<string> blobs = _mySqlUtility.GetBlobUrls(token);

            foreach (var blob in blobs) { _blobUtility.DeleteBlob(blob); };
            _mySqlUtility.AnonymizeUser(token);

        }

        public void Retrieve(AnonymizeToken token)
        {
            IEnumerable<string> blobUrlss = _mySqlUtility.GetBlobUrls(token);

            foreach (var blobUrl in blobUrlss)
            {
                CloudBlob blob=_blobUtility.RetrieveBlob(blobUrl);
                blob.DownloadToFile(blob.Name,FileMode.Create);
            };

            FileStream fileStream = null;
            try
            {
                fileStream = File.OpenWrite(String.Concat(Guid.NewGuid().ToString(), ".txt"));

                using (TextWriter textWriter = new StreamWriter(fileStream))
                {
                    var users = _mySqlUtility.RetrieveUser(token);

                    foreach (var user in users)
                    {
                        textWriter.WriteLine(user.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                fileStream?.Close();
            }

        }
    }
}
