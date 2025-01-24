Created library for unit testing. Output:
Loading assembly: C:\Users\janpoglod\Desktop\C#\p1\MiniTest\AuthenticationService.Tests\bin\Debug\net6.0\AuthenticationService.Tests.dll
Running tests from class AuthenticationService.Tests.AuthServiceTests...
GetRegisteredUserData_ExistingUsername_ShouldThrowError      : PASSED
GetRegisteredUserData_NonExistingUsername_ShouldThrowError   : PASSED
Login_InvalidPassword_ShouldFail                             : PASSED
Login_NonExistingUsername_ShouldFail                         : PASSED
Login_ValidPassword_ShouldSucceed                            : PASSED
Register_ExistingUsername_ShouldRejectRegisteringUser        : PASSED
Register_InvalidPassword_ShouldRejectNewUser                 : PASSED
Register_InvalidUsername_ShouldRejectNewUser                 : PASSED
Register_NewUsername_ShouldAddNewUser                        : PASSED
Register_TwoDifferentUsernames_ShouldAddBothUsers            : PASSED
FAILED: ChangePassword_InvalidNewPassword_ShouldFail - Assertion failed: User should not be able to change password to something invalid.
ChangePassword_InvalidNewPassword_ShouldFail                 : FAILED
  This test is supposed to fail, just for testing purposes.
FAILED: ChangePassword_NonExistingUsername_ShouldThrowError - Expected exception: AuthenticationService.UserNotFoundException, but got: System.NotImplementedException.
ChangePassword_NonExistingUsername_ShouldThrowError          : FAILED
  This test is supposed to fail, just for testing purposes.
FAILED: ChangePassword_ValidUserAndPassword_ShouldSucceed - Assertion failed: Existing user should be able to change password to something valid.
ChangePassword_ValidUserAndPassword_ShouldSucceed            : FAILED
  This test is supposed to fail, just for testing purposes.
******************************
* Test passed:    10 / 13    *
* Failed:          3         *
******************************
################################################################################
Running tests from class AuthenticationService.Tests.CredentialsValidatorTests...
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidPassword_ShouldFail              : PASSED
CredentialsValidator_InvalidUsername_ShouldFail              : PASSED
CredentialsValidator_InvalidUsername_ShouldFail              : PASSED
CredentialsValidator_InvalidUsername_ShouldFail              : PASSED
CredentialsValidator_InvalidUsername_ShouldFail              : PASSED
CredentialsValidator_InvalidUsername_ShouldFail              : PASSED
CredentialsValidator_InvalidUsername_ShouldFail              : PASSED
CredentialsValidator_UsernameStartsWithNumber_ShouldFail     : PASSED
CredentialsValidator_UsernameStartsWithUnderscore_ShouldFail : PASSED
CredentialsValidator_ValidPassword_ShouldSucceed             : PASSED
CredentialsValidator_ValidPassword_ShouldSucceed             : PASSED
CredentialsValidator_ValidPassword_ShouldSucceed             : PASSED
CredentialsValidator_ValidPassword_ShouldSucceed             : PASSED
CredentialsValidator_ValidUsername_ShouldSucceed             : PASSED
CredentialsValidator_ValidUsername_ShouldSucceed             : PASSED
CredentialsValidator_ValidUsername_ShouldSucceed             : PASSED
CredentialsValidator_ValidUsername_ShouldSucceed             : PASSED
CredentialsValidator_ValidUsername_ShouldSucceed             : PASSED
CredentialsValidator_ValidUsername_ShouldSucceed             : PASSED
******************************
* Test passed:    25 / 25    *
* Failed:          0         *
******************************
################################################################################
Running tests from class AuthenticationService.Tests.PasswordHasherTests...
PasswordHasher_DifferentPasswords_ShouldHaveDifferentHashes  : PASSED
PasswordHasher_SamePassword_ShouldHaveSameHash               : PASSED
PasswordHasher_SamePassword_ShouldHaveSameHash               : PASSED
PasswordHasher_SamePassword_ShouldHaveSameHash               : PASSED
******************************
* Test passed:    4 / 4    *
* Failed:          0         *
******************************
################################################################################
Summary of running tests:
******************************
* Test passed:    39 / 42    *
* Failed:          3         *
******************************

C:\Users\janpoglod\Desktop\C#\p1\MiniTest\MiniTestRunner\bin\Debug\net6.0\MiniTestRunner.exe (process 14464) exited with code 0.
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .
