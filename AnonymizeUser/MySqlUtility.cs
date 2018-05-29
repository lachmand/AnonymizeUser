using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using AnonymizeUser.Model;

namespace AnonymizeUser
{
    public interface IMySqlUtility
    {
        IEnumerable<Int64> GetUserIds(AnonymizeToken token);
        IEnumerable<string> GetBlobUrls(AnonymizeToken token);
        void AnonymizeUser(AnonymizeToken token);
        IEnumerable<User> RetrieveUser(AnonymizeToken token);
    }

    public class MySqlUtility : IMySqlUtility
    {
        private static string MYSQL_CONNECTION_STRING_KEY ="MySqlConnection";
        #region Ctor
        public MySqlUtility()
        {
        }
        #endregion

        #region Public Methods
        public IEnumerable<Int64> GetUserIds(AnonymizeToken token)
        {
            return Connect<long>((connection) =>
            {
                List<Int64> parentAndChildIds = new List<Int64>();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    const string PARENT_ID_PARAM = "@IN_PARENT_ID";
                    const string USER_ID_ATTRIBUTE= "USER_ID";
             
                    cmd.Connection = connection;

                    cmd.CommandText = "WEB_SPS_USER_USERID_AND_CHILDRENID";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(PARENT_ID_PARAM, (Int64)token.UserId);
                    cmd.Parameters[PARENT_ID_PARAM].Direction = ParameterDirection.Input;

                    using (MySqlDataReader myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            if (!Convert.IsDBNull(myReader[USER_ID_ATTRIBUTE]))
                            {
                                parentAndChildIds.Add(myReader.GetInt64(USER_ID_ATTRIBUTE));
                            }
                        }
                    }//using myReader
                }//using cmd
                return parentAndChildIds.ToArray<Int64>();
            });
        }

        public IEnumerable<string> GetBlobUrls(AnonymizeToken token)
        {
            return Connect<string>((connection) =>
            {
                List<string> blobs = new List<string>();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    const string PARENT_ID_PARAM = "@IN_PARENT_ID";
                    const string CASCADE_PARAM = "@IN_CASCADE";
                    const string USER_PHOTO_ATTRIBUTE = "USER_PHOTO";

                    cmd.Connection = connection;

                    cmd.CommandText = "WEB_SPS_USER_BLOBS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(PARENT_ID_PARAM, (Int64)token.UserId);
                    cmd.Parameters[PARENT_ID_PARAM].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue(CASCADE_PARAM, (bool)token.CanCascade);
                    cmd.Parameters[CASCADE_PARAM].Direction = ParameterDirection.Input;

                    using (MySqlDataReader myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            if (!myReader.IsDBNull(myReader.GetOrdinal(USER_PHOTO_ATTRIBUTE)))
                            {
                                blobs.Add(myReader.GetString(USER_PHOTO_ATTRIBUTE));
                            }
                        }
                    }//using myReader
                }//using cmd
                return blobs.ToArray<string>();
            });
        }

        public void AnonymizeUser(AnonymizeToken token)
        {
            Connect((connection) =>
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    const string PARENT_ID_PARAM = "@IN_PARENT_ID";
                    const string REASON_PARAM = "@IN_REASON";
                    const string CASCADE_PARAM = "@IN_CASCADE";

                    cmd.Connection = connection;

                    cmd.CommandText = "WEB_SPU_USER_ANONYMIZE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(PARENT_ID_PARAM, (Int64)token.UserId);
                    cmd.Parameters[PARENT_ID_PARAM].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue(REASON_PARAM, token.CanCascade);
                    cmd.Parameters[REASON_PARAM].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue(CASCADE_PARAM, (bool)token.CanCascade);
                    cmd.Parameters[CASCADE_PARAM].Direction = ParameterDirection.Input;

                    cmd.ExecuteNonQuery();
                }//using cmd
            });
        }

        public IEnumerable<User> RetrieveUser(AnonymizeToken token)
        {
            List<User> users= new List<User>();

            Connect((connection) =>
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    const string USER_ID_PARAM = "@IN_USER_ID";

                    cmd.Connection = connection;

                    cmd.CommandText = "WEB_SPS_USER_RETRIEVE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(USER_ID_PARAM, (Int64)token.UserId);
                    cmd.Parameters[USER_ID_PARAM].Direction = ParameterDirection.Input;
                    using (MySqlDataReader myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            //TODO
                            users.Add(new User()
                            {
                                ID = (long)myReader["USER_ID"],
                                Parent = (long)myReader["USER_PARENT"],
                                ParentName = (string)myReader["USER_PARENT_NAME"],
                                FullName = (string)myReader["USER_FULL_NAME"],
                                FirstName = (string)myReader["USER_FIRST_NAME"],
                                LastName = (string)myReader["USER_LAST_NAME"],
                                Username = (string)myReader["USER_USERNAME"],
                                Email = (string)myReader["USER_EMAIL"],
                                Country=(string)myReader["USER_COUNTRY"],
                                Gender = (string)myReader["USER_GENDER"],
                                GenderId = (Enums.Gender)Enums.Gender.Parse(typeof(Enums.Gender), ((string)myReader["USER_GENDER"])),
                                Photo = (string)myReader["USER_PHOTO"],
                                Level = (Enums.UserLevel)Enums.UserLevel.Parse(typeof(Enums.UserLevel), ((string)myReader["USER_LEVEL"])),
                                Status = (int)myReader[0],
                                CreatedDate = ((DateTime)myReader["USER_CREATED_DATE"]).ToString(),
                                LoginDate = (string)myReader["USER_LOGIN_DATE"],
                                TimeZoneOffset = (short)myReader[0],
                                IsGrounded = (bool)myReader["USER_GROUNDED"],
                                DOB = System.Math.Abs(DateTime.UtcNow.Year - ((DateTime)myReader["USER_DOB"]).Year).ToString(),
                                Age = (int)myReader["YEAR"]
                            });
                        }//while
                    }//using myReader
                }//using cmd
            });

            return users;
        }
        #endregion

        #region Private Members
        private IEnumerable<T> Connect<T>(Func<MySqlConnection, IEnumerable<T>> function)
        {

            //Format: Server=localhost; database={0}; UID=UserName; password=your password
            string connectionString = ConfigurationManager.ConnectionStrings[MYSQL_CONNECTION_STRING_KEY].ConnectionString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return function(connection);
            }
        }

        private void Connect(Action<MySqlConnection> action)
        {
            //Format: Server=localhost; database={0}; UID=UserName; password=your password
            string connectionString = ConfigurationManager.ConnectionStrings[MYSQL_CONNECTION_STRING_KEY].ConnectionString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                action(connection);
            }
        }

        /// <summary>
        /// get user details
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        #endregion
    }//class
}//ns
