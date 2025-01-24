//using MiniTest;
using MiniTests.Attributes;
using MiniTests.Assertions;

namespace AuthenticationService.Tests;

[TestClass]
public class PasswordHasherTests
{
    // format data, description
    [DataRow("!Nic3Password*", "!Nic3Password*")]
    [DataRow("(modnaRlat0t", "(modnaRlat0t")]
    [DataRow("PJDs6a!q", "PJDs6a!q")]
    [TestMethod]
    public void PasswordHasher_SamePassword_ShouldHaveSameHash(string s)
    {
        var passHash1 = PasswordHasher.HashPassword(s);
        var passHash2 = PasswordHasher.HashPassword(s);
        Assert.AreEqual(passHash1, passHash2);
    }

    [TestMethod]
    public void PasswordHasher_DifferentPasswords_ShouldHaveDifferentHashes()
    {
        var passHash1 = PasswordHasher.HashPassword("KaWaRaNo_FTW!");
        var passHash2 = PasswordHasher.HashPassword("KaWaRaNo_FTW?");
        Assert.AreNotEqual(passHash1, passHash2);
    }
}