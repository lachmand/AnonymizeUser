using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace AnonymizeUserTest
{
    [TestClass]
    public class MySqlTest
    {
        [TestMethod]
        public void RebrieveUser()
        {
            AnonymizeUser.MySqlUtility mySqlUtility = new AnonymizeUser.MySqlUtility();

            var userIds = mySqlUtility.GetUserIds(new AnonymizeUser.AnonymizeToken(156001, null,true));

            Assert.IsTrue(userIds.Count() > 0);
         }
    }
}
