namespace Localyzer.Context
{
    public static class DataBaseQuerries
    {
        public static string showUsers = string.Format("Select * FROM users");
        public static string showDevices = string.Format("Select * FROM devices");
        public static string addDevice = string.Format("INSERT INTO [dbo].[devices] VALUES ('{0}','{1}','{2}')");
        public static string addUser = string.Format("INSERT INTO [dbo].[users] VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')");
    }
}
