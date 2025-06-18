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
    public class PatronageServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly PatronageService _patronageService;

        public PatronageServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _patronageService = new PatronageService(_mockContext.Object);
        }

        [Fact]
        public async Task CreatePatronageAsync_ShouldAddPatronageToContext()
        {
            var patronage = new Patronage { PatronageId = 1, PatronUserId = 1, ProjectId = 1 };
            var mockDbSet = new Mock<DbSet<Patronage>>();
            _mockContext.Setup(c => c.Patronages).Returns(mockDbSet.Object);

            await _patronageService.CreatePatronageAsync(patronage);

            mockDbSet.Verify(db => db.Add(patronage), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdatePatronageAsync_ShouldUpdatePatronageInContext()
        {
            var patronage = new Patronage { PatronageId = 1, PatronUserId = 1, ProjectId = 1 };
            var mockDbSet = new Mock<DbSet<Patronage>>();
            _mockContext.Setup(c => c.Patronages).Returns(mockDbSet.Object);

            await _patronageService.UpdatePatronageAsync(patronage);

            _mockContext.Verify(c => c.Entry(patronage).State = EntityState.Modified, Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetPatronageByIdAsync_ShouldReturnPatronage()
        {
            var patronage = new Patronage { PatronageId = 1, PatronUserId = 1, ProjectId = 1 };
            var mockDbSet = new Mock<DbSet<Patronage>>();
            _mockContext.Setup(c => c.Patronages).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(patronage);

            var result = await _patronageService.GetPatronageByIdAsync(1);

            Assert.Equal(patronage, result);
        }

        [Fact]
        public async Task GetAllPatronagesAsync_ShouldReturnAllPatronages()
        {
            var patronages = new List<Patronage> { new Patronage { PatronageId = 1, PatronUserId = 1, ProjectId = 1 }, new Patronage { PatronageId = 2, PatronUserId = 2, ProjectId = 2 } };
            var mockDbSet = new Mock<DbSet<Patronage>>();
            _mockContext.Setup(c => c.Patronages).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.ToListAsync()).ReturnsAsync(patronages);

            var result = await _patronageService.GetAllPatronagesAsync();

            Assert.Equal(patronages, result);
        }

        [Fact]
        public async Task DeletePatronageAsync_ShouldRemovePatronageFromContext()
        {
            var patronage = new Patronage { PatronageId = 1, PatronUserId = 1, ProjectId = 1 };
            var mockDbSet = new Mock<DbSet<Patronage>>();
            _mockContext.Setup(c => c.Patronages).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(patronage);

            await _patronageService.DeletePatronageAsync(1);

            mockDbSet.Verify(db => db.Remove(patronage), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
} 