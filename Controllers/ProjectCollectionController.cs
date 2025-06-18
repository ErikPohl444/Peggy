using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Peggy.Models;
using Peggy.Services;

namespace Peggy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectCollectionController : ControllerBase
    {
        private readonly IProjectCollectionService _collectionService;
        private readonly ILogger<ProjectCollectionController> _logger;
        private static readonly ActivitySource ActivitySource = new("Peggy.ProjectCollectionController");

        public ProjectCollectionController(
            IProjectCollectionService collectionService,
            ILogger<ProjectCollectionController> logger)
        {
            _collectionService = collectionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectCollection>> CreateCollection(ProjectCollection collection)
        {
            using var activity = ActivitySource.StartActivity("CreateCollection");
            activity?.SetTag("collection.name", collection.Name);

            try
            {
                var result = await _collectionService.CreateCollectionAsync(collection);
                _logger.LogInformation("Created collection with ID {CollectionId}", result.CollectionId);
                return CreatedAtAction(nameof(GetCollection), new { id = result.CollectionId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating collection");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while creating the collection");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectCollection>> GetCollection(int id)
        {
            using var activity = ActivitySource.StartActivity("GetCollection");
            activity?.SetTag("collection.id", id);

            try
            {
                var collection = await _collectionService.GetCollectionAsync(id);
                if (collection == null)
                {
                    _logger.LogWarning("Collection with ID {CollectionId} not found", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Collection not found");
                    return NotFound();
                }

                _logger.LogInformation("Retrieved collection with ID {CollectionId}", id);
                return Ok(collection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collection with ID {CollectionId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while retrieving the collection");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectCollection>>> GetAllCollections()
        {
            using var activity = ActivitySource.StartActivity("GetAllCollections");

            try
            {
                var collections = await _collectionService.GetAllCollectionsAsync();
                _logger.LogInformation("Retrieved {Count} collections", collections.Count());
                return Ok(collections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all collections");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while retrieving collections");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollection(int id, ProjectCollection collection)
        {
            using var activity = ActivitySource.StartActivity("UpdateCollection");
            activity?.SetTag("collection.id", id);

            if (id != collection.CollectionId)
            {
                _logger.LogWarning("Collection ID mismatch: {PathId} vs {CollectionId}", id, collection.CollectionId);
                return BadRequest();
            }

            try
            {
                var result = await _collectionService.UpdateCollectionAsync(collection);
                _logger.LogInformation("Updated collection with ID {CollectionId}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating collection with ID {CollectionId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while updating the collection");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            using var activity = ActivitySource.StartActivity("DeleteCollection");
            activity?.SetTag("collection.id", id);

            try
            {
                await _collectionService.DeleteCollectionAsync(id);
                _logger.LogInformation("Deleted collection with ID {CollectionId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting collection with ID {CollectionId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while deleting the collection");
            }
        }

        [HttpPost("{collectionId}/projects/{projectId}")]
        public async Task<IActionResult> AddProjectToCollection(int collectionId, int projectId)
        {
            using var activity = ActivitySource.StartActivity("AddProjectToCollection");
            activity?.SetTag("collection.id", collectionId);
            activity?.SetTag("project.id", projectId);

            try
            {
                await _collectionService.AddProjectToCollectionAsync(collectionId, projectId);
                _logger.LogInformation("Added project {ProjectId} to collection {CollectionId}", projectId, collectionId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project {ProjectId} to collection {CollectionId}", projectId, collectionId);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while adding the project to the collection");
            }
        }

        [HttpDelete("{collectionId}/projects/{projectId}")]
        public async Task<IActionResult> RemoveProjectFromCollection(int collectionId, int projectId)
        {
            using var activity = ActivitySource.StartActivity("RemoveProjectFromCollection");
            activity?.SetTag("collection.id", collectionId);
            activity?.SetTag("project.id", projectId);

            try
            {
                await _collectionService.RemoveProjectFromCollectionAsync(collectionId, projectId);
                _logger.LogInformation("Removed project {ProjectId} from collection {CollectionId}", projectId, collectionId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing project {ProjectId} from collection {CollectionId}", projectId, collectionId);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "An error occurred while removing the project from the collection");
            }
        }
    }
} 