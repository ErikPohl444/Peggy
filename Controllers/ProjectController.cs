using Microsoft.AspNetCore.Mvc;
using Peggy.Models;
using Peggy.Services;

namespace Peggy.Controllers;

/// <summary>
/// Controller for managing projects in the Peggy system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectController> _logger;

    /// <summary>
    /// Initializes a new instance of the ProjectController.
    /// </summary>
    /// <param name="projectService">The project service for handling project operations.</param>
    /// <param name="logger">The logger for recording controller activities.</param>
    public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="project">The project data to create.</param>
    /// <returns>The created project with its ID.</returns>
    /// <response code="201">Returns the newly created project.</response>
    /// <response code="400">If the project data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Project), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        try
        {
            var createdProject = await _projectService.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.ProjectId }, createdProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a specific project by its ID.
    /// </summary>
    /// <param name="id">The ID of the project to retrieve.</param>
    /// <returns>The requested project.</returns>
    /// <response code="200">Returns the requested project.</response>
    /// <response code="404">If the project is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        try
        {
            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving project {ProjectId}", id);
            return StatusCode(500, "An error occurred while retrieving the project.");
        }
    }

    /// <summary>
    /// Retrieves all projects.
    /// </summary>
    /// <returns>A list of all projects.</returns>
    /// <response code="200">Returns the list of projects.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all projects");
            return StatusCode(500, "An error occurred while retrieving the projects.");
        }
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="id">The ID of the project to update.</param>
    /// <param name="project">The updated project data.</param>
    /// <returns>The updated project.</returns>
    /// <response code="200">Returns the updated project.</response>
    /// <response code="404">If the project is not found.</response>
    /// <response code="400">If the project data is invalid.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Project>> UpdateProject(int id, Project project)
    {
        if (id != project.ProjectId)
        {
            return BadRequest("Project ID mismatch");
        }

        try
        {
            var updatedProject = await _projectService.UpdateProjectAsync(project);
            if (updatedProject == null)
            {
                return NotFound();
            }
            return Ok(updatedProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {ProjectId}", id);
            return StatusCode(500, "An error occurred while updating the project.");
        }
    }

    /// <summary>
    /// Deletes a project.
    /// </summary>
    /// <param name="id">The ID of the project to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the project was successfully deleted.</response>
    /// <response code="404">If the project is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {ProjectId}", id);
            return StatusCode(500, "An error occurred while deleting the project.");
        }
    }
} 