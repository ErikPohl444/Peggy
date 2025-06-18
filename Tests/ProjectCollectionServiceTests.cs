using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Peggy.Data;
using Peggy.Models;
using Peggy.Services;
using Xunit;

namespace Peggy.Tests
{
    public class ProjectCollectionServiceTests
    {
        private readonly Mock<ILogger<ProjectCollectionService>> _loggerMock;
        private readonly AppDbContext _context;
        private readonly ProjectCollectionService _service;

        public ProjectCollectionServiceTests()
        {
            _loggerMock = new Mock<ILogger<ProjectCollectionService>>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _service = new ProjectCollectionService(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateCollectionAsync_ValidCollection_ReturnsCreatedCollection()
        {
            // Arrange
            var collection = new ProjectCollection
            {
                Name = "Test Collection",
                Description = "Test Description",
                OwnerUserId = 1
            };

            // Act
            var result = await _service.CreateCollectionAsync(collection);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collection.Name, result.Name);
            Assert.Equal(collection.Description, result.Description);
            Assert.Equal(collection.OwnerUserId, result.OwnerUserId);
            Assert.True(result.CollectionId > 0);
        }

        [Fact]
        public async Task GetCollectionAsync_ExistingCollection_ReturnsCollection()
        {
            // Arrange
            var collection = new ProjectCollection
            {
                Name = "Test Collection",
                Description = "Test Description",
                OwnerUserId = 1
            };
            _context.ProjectCollections.Add(collection);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetCollectionAsync(collection.CollectionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collection.CollectionId, result.CollectionId);
            Assert.Equal(collection.Name, result.Name);
        }

        [Fact]
        public async Task GetCollectionAsync_NonExistingCollection_ReturnsNull()
        {
            // Act
            var result = await _service.GetCollectionAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCollectionsAsync_ReturnsAllCollections()
        {
            // Arrange
            var collections = new List<ProjectCollection>
            {
                new ProjectCollection { Name = "Collection 1", OwnerUserId = 1 },
                new ProjectCollection { Name = "Collection 2", OwnerUserId = 1 }
            };
            _context.ProjectCollections.AddRange(collections);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllCollectionsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Collection 1");
            Assert.Contains(result, c => c.Name == "Collection 2");
        }

        [Fact]
        public async Task UpdateCollectionAsync_ValidCollection_UpdatesCollection()
        {
            // Arrange
            var collection = new ProjectCollection
            {
                Name = "Original Name",
                Description = "Original Description",
                OwnerUserId = 1
            };
            _context.ProjectCollections.Add(collection);
            await _context.SaveChangesAsync();

            // Act
            collection.Name = "Updated Name";
            collection.Description = "Updated Description";
            var result = await _service.UpdateCollectionAsync(collection);

            // Assert
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("Updated Description", result.Description);
        }

        [Fact]
        public async Task DeleteCollectionAsync_ExistingCollection_DeletesCollection()
        {
            // Arrange
            var collection = new ProjectCollection
            {
                Name = "Test Collection",
                OwnerUserId = 1
            };
            _context.ProjectCollections.Add(collection);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteCollectionAsync(collection.CollectionId);

            // Assert
            Assert.Null(await _context.ProjectCollections.FindAsync(collection.CollectionId));
        }

        [Fact]
        public async Task AddProjectToCollectionAsync_ValidProject_AddsProjectToCollection()
        {
            // Arrange
            var collection = new ProjectCollection
            {
                Name = "Test Collection",
                OwnerUserId = 1
            };
            var project = new Project
            {
                Name = "Test Project",
                Description = "Test Description",
                OwnerUserId = 1
            };
            _context.ProjectCollections.Add(collection);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Act
            await _service.AddProjectToCollectionAsync(collection.CollectionId, project.ProjectId);

            // Assert
            var updatedProject = await _context.Projects.FindAsync(project.ProjectId);
            Assert.Equal(collection.CollectionId, updatedProject.CollectionId);
        }

        [Fact]
        public async Task RemoveProjectFromCollectionAsync_ProjectInCollection_RemovesProjectFromCollection()
        {
            // Arrange
            var collection = new ProjectCollection
            {
                Name = "Test Collection",
                OwnerUserId = 1
            };
            var project = new Project
            {
                Name = "Test Project",
                Description = "Test Description",
                OwnerUserId = 1,
                CollectionId = collection.CollectionId
            };
            _context.ProjectCollections.Add(collection);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Act
            await _service.RemoveProjectFromCollectionAsync(collection.CollectionId, project.ProjectId);

            // Assert
            var updatedProject = await _context.Projects.FindAsync(project.ProjectId);
            Assert.Null(updatedProject.CollectionId);
        }
    }
} 