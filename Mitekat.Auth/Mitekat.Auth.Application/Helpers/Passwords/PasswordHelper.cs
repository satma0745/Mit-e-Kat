namespace Mitekat.Auth.Application.Helpers.Passwords;

using BCrypt.Net;

internal class PasswordHelper : IPasswordHelper
{
    public string Hash(string plainTextPassword) =>
        BCrypt.HashPassword(plainTextPassword);

    public bool Match(string plainTextPassword, string hashedPassword) =>
        BCrypt.Verify(plainTextPassword, hashedPassword);
}
