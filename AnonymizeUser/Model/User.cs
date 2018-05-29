using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnonymizeUser.Model;

namespace AnonymizeUser.Model
{
    public class User
    {
        #region Ctor
        public User()
        {

        }
        #endregion

        #region Public Methods
        /** Unique id for a user. Used when update/get/delete information. */
        public long ID { get; set; }

        /** Id of the Parent (if has one). */
        public long Parent { get; set; }

        /** Full name of the Parent (if has one). */
        public string ParentName { get; set; }

        /** Full name of a user. Minimum 2 words departed by ' '. */
        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        /** Name of the price plan for a user. */
        public string PricePlanName { get; set; }

        public string Country { get; set; }

        public string Gender { get; set; }

        public Enums.Gender? GenderId { get; set; }

        /** Photo Url. When adding a photo this should be a BASE64 string. Maximum 200x200px. */
        public string Photo { get; set; }

        public Enums.UserLevel Level { get; set; } = Enums.UserLevel.Unknown;

        public int Status { get; set; }

        /** Date when the user was created. */
        public string CreatedDate { get; set; }

        /** Date of the user's last login. */
        public string LoginDate { get; set; }

        /** Timezone offset. */
        public Int16 TimeZoneOffset { get; set; }

        /** If the Child is grounded - doesn't have access to social media. */
        public bool IsGrounded { get; set; }

        /// <summary>
        /// Shows that child received notification to block/unlock device since the last block action
        /// </summary>
        public bool UserReceivedBlockNotification { get; set; }

        /** Date of birth of a user. */
        public string DOB { get; set; }

        /** Age of a user. Calculated from DOB. */
        public int Age { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("Age:").Append(this.Age).Append(Environment.NewLine);
                sb.Append("Country:").Append(this.Country ?? "").Append(Environment.NewLine);
                sb.Append("CreatedDate:").Append(this.CreatedDate ?? "").Append(Environment.NewLine);
                sb.Append("DOB:").Append(this.DOB ?? "").Append(Environment.NewLine);
                sb.Append("Email:").Append(this.Email ?? "").Append(Environment.NewLine);
                sb.Append("First Name:").Append(this.FirstName ?? "").Append(Environment.NewLine);
                sb.Append("Last Name:").Append(this.LastName ?? "").Append(Environment.NewLine);
                sb.Append("FullName:").Append(this.FullName ?? "").Append(Environment.NewLine);
                sb.Append("Gender:").Append(this.Gender ?? "").Append(Environment.NewLine);
                sb.Append("IsGrounded:").Append(this.IsGrounded).Append(Environment.NewLine);
                sb.Append("Level:").Append(this.Level).Append(Environment.NewLine);
                sb.Append("Parent Name:").Append(this.ParentName ?? "").Append(Environment.NewLine);

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region Derived Data
        public string StatusName
        {
            get
            {
                return (Status == 1) ? "Active" : "Disabled";//??$$
            }
        }
        #endregion

        #endregion
        
    }
}
