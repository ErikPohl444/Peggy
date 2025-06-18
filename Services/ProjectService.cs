using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Peggy.Data;
using Peggy.Models;

namespace Peggy.Services
{
    public interface IProjectService
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<Project?> GetProjectAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
    }

    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectService> _logger;
        private static readonly ActivitySource ActivitySource = new("Peggy.ProjectService");

        public ProjectService(AppDbContext context, ILogger<ProjectService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            using var activity = ActivitySource.StartActivity("CreateProject");
            activity?.SetTag("project.title", project.Title);

            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                
                Metrics.ProjectCreationCounter.Inc();
                _logger.LogInformation("Created project with ID {ProjectId}", project.Id);
                
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<Project?> GetProjectAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("GetProject");
            activity?.SetTag("project.id", id);

            try
            {
                var project = await _context.Projects.FindAsync(id);
                
                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Project not found");
                }
                else
                {
                    _logger.LogInformation("Retrieved project with ID {ProjectId}", id);
                }
                
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project with ID {ProjectId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            using var activity = ActivitySource.StartActivity("GetAllProjects");

            try
            {
                var projects = await _context.Projects.ToListAsync();
                _logger.LogInformation("Retrieved {Count} projects", projects.Count);
                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all projects");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            using var activity = ActivitySource.StartActivity("UpdateProject");
            activity?.SetTag("project.id", project.Id);

            try
            {
                _context.Entry(project).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                Metrics.ProjectUpdateCounter.Inc();
                _logger.LogInformation("Updated project with ID {ProjectId}", project.Id);
                
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project with ID {ProjectId}", project.Id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("DeleteProject");
            activity?.SetTag("project.id", id);

            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found for deletion", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Project not found");
                    return;
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                
                Metrics.ProjectDeletionCounter.Inc();
                _logger.LogInformation("Deleted project with ID {ProjectId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project with ID {ProjectId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }
    }
} 