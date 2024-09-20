using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using Xunit;
using Moq;

namespace HMS_UnitTesting
{
    internal class UserAdministration
    {
    }
    public class UserAdministrationTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserAdministration _userAdministration;

        public UserAdministrationTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockUserRepository = new Mock<IUserRepository>();
            _userAdministration = new UserAdministration(_mockLogger.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task UpdateUserInformationAsync_SuccessfulUpdate_ReturnsSuccessMessage()
        {
            // Arrange
            var userInfo = new UserInfo { UserName = "testuser", Email = "testuser@example.com" };
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(userInfo)).ReturnsAsync(true);

            // Act
            var result = await _userAdministration.UpdateUserInformationAsync(userInfo);

            // Assert
            Assert.Equal("User information updated successfully.", result);
            _mockLogger.Verify(logger => logger.LogMessage(It.Is<string>(s => s.Contains("updated successfully"))), Times.Once);
        }

        [Fact]
        public async Task UpdateUserInformationAsync_FailedUpdate_ReturnsFailureMessage()
        {
            // Arrange
            var userInfo = new UserInfo { UserName = "testuser", Email = "testuser@example.com" };
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(userInfo)).ReturnsAsync(false);

            // Act
            var result = await _userAdministration.UpdateUserInformationAsync(userInfo);

            // Assert
            Assert.Equal("Failed to update user information.", result);
            _mockLogger.Verify(logger => logger.LogMessage(It.Is<string>(s => s.Contains("Failed to update"))), Times.Once);
        }

        [Fact]
        public async Task UpdateUserInformationAsync_ExceptionThrown_ReturnsErrorMessage()
        {
            // Arrange
            var userInfo = new UserInfo { UserName = "testuser", Email = "testuser@example.com" };
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(userInfo)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userAdministration.UpdateUserInformationAsync(userInfo);

            // Assert
            Assert.Equal("An error occurred while updating user information.", result);
            _mockLogger.Verify(logger => logger.LogMessage(It.Is<string>(s => s.Contains("Exception occurred"))), Times.Once);
        }
    }
}
  
}
