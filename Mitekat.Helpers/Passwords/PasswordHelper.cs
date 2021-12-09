namespace Mitekat.Helpers.Passwords;

using BCrypt.Net;
using Mitekat.Core.Abstractions.Helpers;

internal class PasswordHelper : IPasswordHelper
{
    public string Hash(string plainTextPassword) =>
        BCrypt.HashPassword(plainTextPassword);

    public bool Match(string plainTextPassword, string hashedPassword) =>
        BCrypt.Verify(plainTextPassword, hashedPassword);
}
