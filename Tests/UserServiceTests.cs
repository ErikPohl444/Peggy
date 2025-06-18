using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Peggy.Data;
using Peggy.Models;
using Peggy.Services;
using Xunit;

namespace Peggy.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _userService = new UserService(_mockContext.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToContext()
        {
            var user = new User { UserId = 1, Username = "TestUser" };
            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            await _userService.CreateUserAsync(user);

            mockDbSet.Verify(db => db.Add(user), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUserInContext()
        {
            var user = new User { UserId = 1, Username = "UpdatedUser" };
            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            await _userService.UpdateUserAsync(user);

            _mockContext.Verify(c => c.Entry(user).State = EntityState.Modified, Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            var user = new User { UserId = 1, Username = "TestUser" };
            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(1);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            var users = new List<User> { new User { UserId = 1, Username = "User1" }, new User { UserId = 2, Username = "User2" } };
            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.ToListAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();

            Assert.Equal(users, result);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUserFromContext()
        {
            var user = new User { UserId = 1, Username = "TestUser" };
            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(user);

            await _userService.DeleteUserAsync(1);

            mockDbSet.Verify(db => db.Remove(user), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
} 