namespace OracleUserManagementApp.Models
{
    public class UserRoleModel
    {
        public string Name { get; set; } // Username hoặc Role name
        public string Type { get; set; } // "USER" hoặc "ROLE"
        public string Status { get; set; } // Trạng thái tài khoản (cho user)
        public string Password { get; set; } // Mật khẩu (cho user)
    }
}