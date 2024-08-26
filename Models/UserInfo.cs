namespace Localyzer.Models;
public class UserInfo
{
    public String? Id;
    public String? name;
    public String? lastname;
    public String? email;
    public String? phone;
    public String? address;
    public String? created_at;
    public Guid deviceId;
}

public class UserCredentials
{
    public String? Id;
    public String? login;
    public String? password;
}
