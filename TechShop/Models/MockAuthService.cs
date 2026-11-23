using System.Text.Json;

namespace TechShop.Models
{
    public class MockAuthService : IAuthService
    {
        private const string UsersKey = "MockUsers";
        private const string CurrentUserKey = "CurrentUser";
        private const string CurrentUserEmailKey = "CurrentUserEmail";
    private const string AdminEmail = "admin@techshop.local";
    private const string AdminPassword = "Admin@123";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MockAuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return !string.IsNullOrWhiteSpace(session?.GetString(CurrentUserKey));
        }

        public bool IsAdmin()
        {
            var email = GetCurrentUserEmail();
            return string.Equals(email, AdminEmail, StringComparison.OrdinalIgnoreCase);
        }

        public string? GetCurrentUserName()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString(CurrentUserKey);
        }

        public string? GetCurrentUserEmail()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString(CurrentUserEmailKey);
        }

        public bool Login(string email, string password)
        {
            if (string.Equals(email, AdminEmail, StringComparison.OrdinalIgnoreCase)
                && password == AdminPassword)
            {
                var adminSession = _httpContextAccessor.HttpContext?.Session;
                adminSession?.SetString(CurrentUserKey, "Admin");
                adminSession?.SetString(CurrentUserEmailKey, AdminEmail);
                return true;
            }

            var users = GetUsers();
            var user = users.FirstOrDefault(u =>
                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);

            if (user is null)
            {
                return false;
            }

            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString(CurrentUserKey, user.FullName);
            session?.SetString(CurrentUserEmailKey, user.Email);
            return true;
        }

        public bool Register(string fullName, string email, string password)
        {
            var users = GetUsers();
            var exists = users.Any(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                return false;
            }

            users.Add(new MockUser
            {
                FullName = fullName,
                Email = email,
                Password = password
            });

            SaveUsers(users);

            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString(CurrentUserKey, fullName);
            session?.SetString(CurrentUserEmailKey, email);
            return true;
        }

        public void Logout()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Remove(CurrentUserKey);
            session?.Remove(CurrentUserEmailKey);
        }

        public IReadOnlyList<CustomerInfo> GetRegisteredUsers()
        {
            return GetUsers()
                .Select(u => new CustomerInfo(u.FullName, u.Email))
                .ToList();
        }

        private List<MockUser> GetUsers()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString(UsersKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<MockUser>();
            }

            return JsonSerializer.Deserialize<List<MockUser>>(json) ?? new List<MockUser>();
        }

        private void SaveUsers(List<MockUser> users)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session is null)
            {
                return;
            }

            var json = JsonSerializer.Serialize(users);
            session.SetString(UsersKey, json);
        }

        private class MockUser
        {
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
