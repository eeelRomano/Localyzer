using Localyzer.Models;
using Serilog;
using System.Data.SqlClient;

namespace Localyzer.Context
{
    public class DataBaseHandler
    {
        #region Definicje Zmiennych
        public List<UserInfo> userlist = [];

        private String connectionString = "Data Source=APOLLO-11\\U18372;Initial Catalog=LocalyzerDB;Integrated Security=True;";// Trust Server Certificate=True;";
        #endregion

        public void GetUserList() 
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                String sql = "Select * FROM users";
                using SqlCommand command = new(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo userinfo = new();
                    userinfo.Id = "" + reader.GetInt32(0);
                    userinfo.name = reader.GetString(1);
                    userinfo.lastname = reader.GetString(2);
                    userinfo.email = reader.GetString(3);
                    userinfo.phone = reader.GetString(4);
                    userinfo.address = reader.GetString(5);

                    userlist.Add(userinfo);
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }
        }

        private List<UserCredentials> GetUserCredentials()
        {
            List<UserCredentials> upass = [];
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                String sql = "Select * FROM users";
                using SqlCommand command = new(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserCredentials ucreds = new();
                    ucreds.Id = "" + reader.GetInt32(0);
                    ucreds.login = reader.GetString(8);
                    ucreds.password = reader.GetString(9);

                    upass.Add(ucreds);
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }

            return upass;
        }

        public String Login(string login, string password)
        {
            List<UserCredentials> userpass = GetUserCredentials();

            foreach (var item in userpass)
            {
                if (login == item.login)
                {
                    foreach (var item1 in userpass)
                    {
                        if (password == item1.password)
                            return item1.Id;
                    }
                }
            }

            return "";
        }
    }
}
