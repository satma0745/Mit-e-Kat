namespace Mitekat.Core.Abstractions.Helpers;

public interface IPasswordHelper
{
    string Hash(string plainTextPassword);

    bool Match(string plainTextPassword, string hashedPassword);
}
