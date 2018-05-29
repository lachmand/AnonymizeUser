using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonymizeUser
{
    /// <summary>
    /// Token for Anonymize MySql stored proc
    /// </summary>
    public class AnonymizeToken
    {
        #region Ctor
        protected AnonymizeToken()
        {

        }

        public AnonymizeToken(Int64 userId, string reason, bool canCascade)
        {
            this.UserId = userId;
            this.Reason = reason;
            this.CanCascade = canCascade;
        }

        public AnonymizeToken(string userId, string reason, string canCascade)
        {
            this.UserId = Int64.Parse(userId);
            this.Reason = reason;
            this.CanCascade = bool.Parse(canCascade);
        } 
        #endregion
        public Int64 UserId { get; set; }
        public string Reason { get; set; }
        public bool CanCascade { get; set; }
    }
}
