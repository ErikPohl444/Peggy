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
    public class ProjectServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _projectService = new ProjectService(_mockContext.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldAddProjectToContext()
        {
            var project = new Project { ProjectId = 1, Description = "TestProject" };
            var mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            await _projectService.CreateProjectAsync(project);

            mockDbSet.Verify(db => db.Add(project), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldUpdateProjectInContext()
        {
            var project = new Project { ProjectId = 1, Description = "UpdatedProject" };
            var mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            await _projectService.UpdateProjectAsync(project);

            _mockContext.Verify(c => c.Entry(project).State = EntityState.Modified, Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnProject()
        {
            var project = new Project { ProjectId = 1, Description = "TestProject" };
            var mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(project);

            var result = await _projectService.GetProjectByIdAsync(1);

            Assert.Equal(project, result);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
        {
            var projects = new List<Project> { new Project { ProjectId = 1, Description = "Project1" }, new Project { ProjectId = 2, Description = "Project2" } };
            var mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.ToListAsync()).ReturnsAsync(projects);

            var result = await _projectService.GetAllProjectsAsync();

            Assert.Equal(projects, result);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldRemoveProjectFromContext()
        {
            var project = new Project { ProjectId = 1, Description = "TestProject" };
            var mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(project);

            await _projectService.DeleteProjectAsync(1);

            mockDbSet.Verify(db => db.Remove(project), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
} 