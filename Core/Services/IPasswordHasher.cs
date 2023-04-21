namespace TimeX.Core.Services
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);

        public bool PasswordMatches(string providedPassword, string passwordHash);

    }
}
