using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Peggy.Data;
using Peggy.Models;

namespace Peggy.Services
{
    public interface IProjectCollectionService
    {
        Task<ProjectCollection> CreateCollectionAsync(ProjectCollection collection);
        Task<ProjectCollection?> GetCollectionAsync(int id);
        Task<IEnumerable<ProjectCollection>> GetAllCollectionsAsync();
        Task<ProjectCollection> UpdateCollectionAsync(ProjectCollection collection);
        Task DeleteCollectionAsync(int id);
        Task AddProjectToCollectionAsync(int collectionId, int projectId);
        Task RemoveProjectFromCollectionAsync(int collectionId, int projectId);
    }

    public class ProjectCollectionService : IProjectCollectionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectCollectionService> _logger;
        private static readonly ActivitySource ActivitySource = new("Peggy.ProjectCollectionService");

        public ProjectCollectionService(AppDbContext context, ILogger<ProjectCollectionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProjectCollection> CreateCollectionAsync(ProjectCollection collection)
        {
            using var activity = ActivitySource.StartActivity("CreateCollection");
            activity?.SetTag("collection.name", collection.Name);

            try
            {
                _context.ProjectCollections.Add(collection);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Created collection with ID {CollectionId}", collection.CollectionId);
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating collection");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<ProjectCollection?> GetCollectionAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("GetCollection");
            activity?.SetTag("collection.id", id);

            try
            {
                var collection = await _context.ProjectCollections
                    .Include(c => c.Projects)
                    .FirstOrDefaultAsync(c => c.CollectionId == id);
                
                if (collection == null)
                {
                    _logger.LogWarning("Collection with ID {CollectionId} not found", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Collection not found");
                }
                else
                {
                    _logger.LogInformation("Retrieved collection with ID {CollectionId}", id);
                }
                
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collection with ID {CollectionId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectCollection>> GetAllCollectionsAsync()
        {
            using var activity = ActivitySource.StartActivity("GetAllCollections");

            try
            {
                var collections = await _context.ProjectCollections
                    .Include(c => c.Projects)
                    .ToListAsync();
                _logger.LogInformation("Retrieved {Count} collections", collections.Count);
                return collections;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all collections");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<ProjectCollection> UpdateCollectionAsync(ProjectCollection collection)
        {
            using var activity = ActivitySource.StartActivity("UpdateCollection");
            activity?.SetTag("collection.id", collection.CollectionId);

            try
            {
                _context.Entry(collection).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Updated collection with ID {CollectionId}", collection.CollectionId);
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating collection with ID {CollectionId}", collection.CollectionId);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task DeleteCollectionAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("DeleteCollection");
            activity?.SetTag("collection.id", id);

            try
            {
                var collection = await _context.ProjectCollections.FindAsync(id);
                if (collection == null)
                {
                    _logger.LogWarning("Collection with ID {CollectionId} not found for deletion", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Collection not found");
                    return;
                }

                _context.ProjectCollections.Remove(collection);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Deleted collection with ID {CollectionId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting collection with ID {CollectionId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task AddProjectToCollectionAsync(int collectionId, int projectId)
        {
            using var activity = ActivitySource.StartActivity("AddProjectToCollection");
            activity?.SetTag("collection.id", collectionId);
            activity?.SetTag("project.id", projectId);

            try
            {
                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found", projectId);
                    activity?.SetStatus(ActivityStatusCode.Error, "Project not found");
                    return;
                }

                project.CollectionId = collectionId;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Added project {ProjectId} to collection {CollectionId}", projectId, collectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project {ProjectId} to collection {CollectionId}", projectId, collectionId);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task RemoveProjectFromCollectionAsync(int collectionId, int projectId)
        {
            using var activity = ActivitySource.StartActivity("RemoveProjectFromCollection");
            activity?.SetTag("collection.id", collectionId);
            activity?.SetTag("project.id", projectId);

            try
            {
                var project = await _context.Projects.FindAsync(projectId);
                if (project == null || project.CollectionId != collectionId)
                {
                    _logger.LogWarning("Project {ProjectId} not found in collection {CollectionId}", projectId, collectionId);
                    activity?.SetStatus(ActivityStatusCode.Error, "Project not found in collection");
                    return;
                }

                project.CollectionId = null;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Removed project {ProjectId} from collection {CollectionId}", projectId, collectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing project {ProjectId} from collection {CollectionId}", projectId, collectionId);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }
    }
} 