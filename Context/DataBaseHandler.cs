using Localyzer.Models;
using Localyzer.Models.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace Localyzer.Context
{
    public class DataBaseHandler
    {
        #region Definicje Zmiennych
        public List<UserInfo> userlist = [];
        public List<DeviceLocation> deviceslist = [];

        private String connectionString = "Data Source=APOLLO-11\\U18372;Initial Catalog=LocalyzerDB;Integrated Security=True;";// Trust Server Certificate=True;";

        private readonly ILoggedDevice _logged;
        #endregion

        public DataBaseHandler(ILoggedDevice logged)
        {
            _logged = logged;
        }

        public void GetUserList() 
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                using SqlCommand command = new("Select * FROM users", connection);
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
                    
                    userinfo.deviceId = reader.GetGuid(10);

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
                using SqlCommand command = new("Select * FROM users", connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserCredentials ucreds = new();
                    DeviceLocation device = new();
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

        public List<DeviceLocation> GetDeviceList()
        {
            List<DeviceLocation> devices = [];
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                using SqlCommand command = new("Select * FROM devices", connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DeviceLocation device = new()
                    {
                        DeviceId = reader.GetGuid(0),
                        Latitude = reader.GetDecimal(1),
                        Longtitude = reader.GetDecimal(2),
                        DeviceNr = reader.GetInt32(3),
                        Active = reader.GetBoolean(4),
                    };

                    devices.Add(device);
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }

            return devices;
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
                        {
                            AssignLoggedUser(item1.Id);
                            return item1.Id;
                        }
                    }
                }
            }

            return "";
        }

        public void CreateUser(int id, string name, string lastname,
                                string email, string phone, string adres, 
                                int depart, string created, string login, 
                                string pass, Guid deviceid)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();

                string adddevice = string.Format("INSERT INTO [dbo].[devices] VALUES ('{0}','{1}','{2}','{3}','{4}')",
                                            deviceid, 0, 0, id, 0);
                string adduser = string.Format("INSERT INTO [dbo].[users] VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                            id, name, lastname,
                                            email, phone, adres,
                                            depart, created, login,
                                            pass, deviceid);

                using SqlCommand command = new(adddevice, connection);
                using SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                reader.Close();

                using SqlCommand command1 = new(adduser, connection);
                using SqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                reader1.Close();

                connection.Close();
            }
            catch(Exception e) 
            {
                Debug.WriteLine(e.Message);  
            }
        }

        private void AssignLoggedUser(string uid)
        {
            GetUserList();
            UserInfo logeduser = new();
            foreach(var item in userlist)
            {
                if(item.Id == uid)
                {
                    logeduser = item;
                    _logged.DeviceID = logeduser.deviceId;
                    _logged.DeviceName = logeduser.name;
                }
            }
        }

        public void ActivateDeviceLocating(string userid)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                bool status = CheckDeviceStatus(userid);
                string adddevice = string.Format("UPDATE [dbo].[devices] SET active = '{0}' WHERE deviceNr = '{1}'",
                                                    !status, userid);

                using SqlCommand command = new(adddevice, connection);
                using SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                connection.Close();
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public bool CheckDeviceStatus(string userid)
        {
            using SqlConnection connection = new(connectionString);
            connection.Open();
            string checkstatus = string.Format("SELECT active FROM [dbo].[devices] WHERE deviceNr = '{0}'", userid);
            using SqlCommand command = new(checkstatus, connection);
            using SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            bool status = reader.GetBoolean(0);
            reader.Close();
            connection.Close();
            return status;
        }

        public void UpdateDeviceLocations(double latitude, double longitude, string userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string cmd_updatelocation = "UPDATE [dbo].[devices] SET Latitude = @Latitude, Longtitude = @Longtitude WHERE DeviceNr = @UserId";

                using SqlCommand command = new SqlCommand(cmd_updatelocation, connection);
                command.Parameters.AddWithValue("@Latitude", latitude);
                command.Parameters.AddWithValue("@Longtitude", longitude);
                command.Parameters.AddWithValue("@UserId", userId);

                int rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
