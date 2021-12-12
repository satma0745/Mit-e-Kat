namespace Mitekat.Auth.Application.Helpers.Passwords;

internal interface IPasswordHelper
{
    string Hash(string plainTextPassword);

    bool Match(string plainTextPassword, string hashedPassword);
}
