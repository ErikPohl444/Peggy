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
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;
        private static readonly ActivitySource ActivitySource = new("Peggy.UserService");

        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            using var activity = ActivitySource.StartActivity("CreateUser");
            activity?.SetTag("user.email", user.Email);

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                Metrics.UserCreationCounter.Inc();
                _logger.LogInformation("Created user with ID {UserId}", user.Id);
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<User?> GetUserAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("GetUser");
            activity?.SetTag("user.id", id);

            try
            {
                var user = await _context.Users.FindAsync(id);
                
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "User not found");
                }
                else
                {
                    _logger.LogInformation("Retrieved user with ID {UserId}", id);
                }
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var activity = ActivitySource.StartActivity("GetAllUsers");

            try
            {
                var users = await _context.Users.ToListAsync();
                _logger.LogInformation("Retrieved {Count} users", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            using var activity = ActivitySource.StartActivity("UpdateUser");
            activity?.SetTag("user.id", user.Id);

            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                Metrics.UserUpdateCounter.Inc();
                _logger.LogInformation("Updated user with ID {UserId}", user.Id);
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", user.Id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("DeleteUser");
            activity?.SetTag("user.id", id);

            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "User not found");
                    return;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                Metrics.UserDeletionCounter.Inc();
                _logger.LogInformation("Deleted user with ID {UserId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }
    }
} 