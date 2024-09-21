using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using System.Threading.Tasks;
using KWLCodes_HMSProject.Maui.Pages;

namespace HMS_UnitTesting
{
    public class UserAdministrationTest
    {
        [Fact]
        public void UpdateUserInformation_SuccessfulUpdate_ReturnsSuccessMessage()
        {
            // Arrange
            var userInfo = new UserAdministration.UserInfo { UserName = "testuser", Email = "testuser@example.com" };
            var userAdmin = new UserAdministration.UserAdministrations();

            // Act
            var result = userAdmin.UpdateUserInformation(userInfo);

            // Assert
            Assert.Equal("User information updated successfully.", result);
        }

        [Fact]
        public void UpdateUserInformation_FailedUpdate_ReturnsFailureMessage()
        {
            // Arrange
            var userInfo = new UserAdministration.UserInfo { UserName = "testuser", Email = "testuser@example.com" };
            var userAdminMock = new Mock<UserAdministration.UserAdministrations>();
            userAdminMock.Setup(u => u.UpdateUserInDatabase(It.IsAny<UserAdministration.UserInfo>())).Returns(false);

            // Act
            var result = userAdminMock.Object.UpdateUserInformation(userInfo);

            // Assert
            Assert.Equal("Failed to update user information.", result);
        }

        [Fact]
        public void UpdateUserInformation_ExceptionThrown_ReturnsErrorMessage()
        {
            // Arrange
            var userInfo = new UserAdministration.UserInfo { UserName = "testuser", Email = "testuser@example.com" };
            var userAdminMock = new Mock<UserAdministration.UserAdministrations>();
            userAdminMock.Setup(u => u.UpdateUserInDatabase(It.IsAny<UserAdministration.UserInfo>())).Throws(new Exception("Database error"));

            // Act
            var result = userAdminMock.Object.UpdateUserInformation(userInfo);

            // Assert
            Assert.Equal("An error occurred while updating user information.", result);
        }
    }
}
