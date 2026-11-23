namespace TechShop.Models
{
    public interface IAuthService
    {
        bool IsAuthenticated();
        bool IsAdmin();
        string? GetCurrentUserName();
        string? GetCurrentUserEmail();
        bool Login(string email, string password);
        bool Register(string fullName, string email, string password);
        void Logout();
        IReadOnlyList<CustomerInfo> GetRegisteredUsers();
    }
}
