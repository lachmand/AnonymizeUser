using System;
using AnonymizeUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnonymizeUserTest
{
    [TestClass]
    public class GdprUtilityTest
    {
        [TestMethod]
        public void Retrieve()
        {
            IBlobUtility blobUtility=new BlobUtility();
            IMySqlUtility mySqlUtility=new MySqlUtility();

            GdprUtility gdprUtility=new GdprUtility(blobUtility,mySqlUtility);

            gdprUtility.Retrieve(new AnonymizeToken(156001,"Want to be anonymized",true)); 
        }

        //commented out to stop deletion
        //[TestMethod]
        //public void Anonymize()
        //{

        //    IBlobUtility blobUtility = new BlobUtility();
        //    IMySqlUtility mySqlUtility = new MySqlUtility();

        //    GdprUtility gdprUtility = new GdprUtility(blobUtility, mySqlUtility);

        //    gdprUtility.Anonymize(new AnonymizeToken(156001, "Want to be anonymized", true)); 
        //}
    }
}
