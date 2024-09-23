using System;
using System.IO;
using Xunit;
using Moq;
using Xunit.Abstractions;

public class UserAdminTest
{
    // This is a mockable user information class (for testing).
    public class UserInfo
    {
        public string UserName { get; set; }
    }

    // Test class for the UserAdministrations class.
    public class UserAdministrations
    {
        private readonly string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app-log.txt");

        public virtual bool UpdateUserInDatabase(UserInfo userInfo)
        {
            // Mockable method to simulate database update.
            return true;
        }

        public virtual void LogMessage(string message)
        {
            // Mockable method to simulate logging.
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        public string UpdateUserInformation(UserInfo userInfo)
        {
            try
            {
                bool updateSuccessful = UpdateUserInDatabase(userInfo);

                if (updateSuccessful)
                {
                    LogMessage($"User information for {userInfo.UserName} updated successfully.");
                    return "User information updated successfully.";
                }
                else
                {
                    LogMessage($"Failed to update user information for {userInfo.UserName}.");
                    return "Failed to update user information.";
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Exception occurred while updating user information: {ex.Message}");
                return "An error occurred while updating user information.";
            }
        }
    }

    // Inject the output helper to capture test output.
    private readonly ITestOutputHelper _output;

    public UserAdminTest(ITestOutputHelper output)
    {
        _output = output;
    }

    // xUnit test for successful user information update.
    [Fact]
    public void UpdateUserInformation_Success_ReturnsSuccessMessage()
    {
        // Arrange
        var userInfo = new UserInfo { UserName = "JohnDoe" };
        var mockUserAdmin = new Mock<UserAdministrations>();
        mockUserAdmin.Setup(x => x.UpdateUserInDatabase(It.IsAny<UserInfo>())).Returns(true);

        // Act
        var result = mockUserAdmin.Object.UpdateUserInformation(userInfo);

        // Output test details
        _output.WriteLine($"Test UpdateUserInformation_Success: Result = {result}");

        // Assert
        Assert.Equal("User information updated successfully.", result);
        mockUserAdmin.Verify(x => x.LogMessage("User information for JohnDoe updated successfully."), Times.Once);
    }

    // xUnit test for failure in updating user information.
    [Fact]
    public void UpdateUserInformation_Failure_ReturnsFailureMessage()
    {
        // Arrange
        var userInfo = new UserInfo { UserName = "JohnDoe" };
        var mockUserAdmin = new Mock<UserAdministrations>();
        mockUserAdmin.Setup(x => x.UpdateUserInDatabase(It.IsAny<UserInfo>())).Returns(false);

        // Act
        var result = mockUserAdmin.Object.UpdateUserInformation(userInfo);

        // Output test details
        _output.WriteLine($"Test UpdateUserInformation_Failure: Result = {result}");

        // Assert
        Assert.Equal("Failed to update user information.", result);
        mockUserAdmin.Verify(x => x.LogMessage("Failed to update user information for JohnDoe."), Times.Once);
    }

    // xUnit test for an exception during the update.
    [Fact]
    public void UpdateUserInformation_Exception_ReturnsErrorMessage()
    {
        // Arrange
        var userInfo = new UserInfo { UserName = "JohnDoe" };
        var mockUserAdmin = new Mock<UserAdministrations>();
        mockUserAdmin.Setup(x => x.UpdateUserInDatabase(It.IsAny<UserInfo>())).Throws(new Exception("Database error"));

        // Act
        var result = mockUserAdmin.Object.UpdateUserInformation(userInfo);

        // Output test details
        _output.WriteLine($"Test UpdateUserInformation_Exception: Result = {result}");

        // Assert
        Assert.Equal("An error occurred while updating user information.", result);
        mockUserAdmin.Verify(x => x.LogMessage("Exception occurred while updating user information: Database error"), Times.Once);
    }
}
