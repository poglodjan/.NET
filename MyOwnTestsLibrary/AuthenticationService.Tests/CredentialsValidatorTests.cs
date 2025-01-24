//using MiniTest;
using MiniTests.Attributes;
using MiniTests.Assertions;

namespace AuthenticationService.Tests;

[TestClass]
public class CredentialsValidatorTests
{
    private CredentialsValidator CredentialsValidator { get; set; } = null!;

    [BeforeEach]
    public void Setup()
    {
        CredentialsValidator = new CredentialsValidator();
    }
    
    [TestMethod]
    public void CredentialsValidator_UsernameStartsWithNumber_ShouldFail()
    {
        Assert.IsFalse(CredentialsValidator.ValidateUsername("123abc456"));
    }

    [TestMethod]
    public void CredentialsValidator_UsernameStartsWithUnderscore_ShouldFail()
    {
        Assert.IsFalse(CredentialsValidator.ValidateUsername("_username"));
    }

    // format data, description
    [DataRow("123abc456", "Starts with a number")]
    [DataRow("_username", "Starts with underscore")]
    [DataRow("user", "To short (needs at least 8 characters)")]
    [DataRow("\\(^o^)/", "Contains (mostly) illegal characters")]
    [DataRow("User@name", "Contains illegal character")]
    [DataRow("SuperLongUsernameNobodysGoingToRepeat", "Exceeds Username Length Limit")]
    [TestMethod]
    public void CredentialsValidator_InvalidUsername_ShouldFail(string s)
    {
        Assert.IsFalse(CredentialsValidator.ValidateUsername(s));
    }
    // format data, description
    [DataRow("UserName", "UserName")]
    [DataRow("MarioTheStrong_01", "MarioTheStrong_01")]
    [DataRow("hello_kitty_", "hello_kitty_")]
    [DataRow("smiley2137", "smiley2137")]
    [DataRow("qwertyqwerty", "qwertyqwerty")]
    [DataRow("ONLY_CAPSLOCK", "ONLY_CAPSLOCK")]
    [TestMethod]
    [Description("These username contain only legal characters ([a-zA-Z0-9_])\n" +
                 "  and have correct length (8-32)")]
    public void CredentialsValidator_ValidUsername_ShouldSucceed(string s)
    {
        Assert.IsTrue(CredentialsValidator.ValidateUsername(s));
    }
    // format data, description
    [DataRow("QWErtyASDfgh_123!@#", "Password too long")]
    [DataRow("pass", "Password too short")]
    [DataRow("password", "No upper case letter, number or special character")]
    [DataRow("1234567Aa", "No special character")]
    [DataRow("PaSsWoRd!",  "No number")]
    [DataRow("STRONG_PASS1!", "No lowercase letter")]
    [DataRow("\\(^o^)/", "No number, no upper case letter, too short")]
    [TestMethod]
    public void CredentialsValidator_InvalidPassword_ShouldFail(string s)
    {
        Assert.IsFalse(CredentialsValidator.ValidatePassword(s));
    }

    // format data, description
    [DataRow("o\\(O_0)/o",  "o\\(O_0)/o")]
    [DataRow("!Nic3Password*", "!Nic3Password*")]
    [DataRow("(modnaRlat0t", "(modnaRlat0t")]
    [DataRow("PJDs6a!q", "PJDs6a!q")]
    [TestMethod]
    [Description("These passwords contain between 8 and 16 characters,\n" +
                 "  lower/uppercase char, number and a special character")]
    public void CredentialsValidator_ValidPassword_ShouldSucceed(string s)
    {
        Assert.IsTrue(CredentialsValidator.ValidatePassword(s));
    }
}