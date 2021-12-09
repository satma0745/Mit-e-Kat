namespace Mitekat.Auth.Core.Abstractions.Helpers;

public interface IPasswordHelper
{
    string Hash(string plainTextPassword);

    bool Match(string plainTextPassword, string hashedPassword);
}
