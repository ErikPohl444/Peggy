using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Peggy.Data;
using Peggy.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace Peggy.Services
{
    public interface IPatronagePaymentService
    {
        Task<PatronagePayment> CreatePaymentAsync(PatronagePayment payment);
        Task<PatronagePayment?> GetPaymentAsync(int id);
        Task<IEnumerable<PatronagePayment>> GetAllPaymentsAsync();
        Task<PatronagePayment> UpdatePaymentAsync(PatronagePayment payment);
        Task DeletePaymentAsync(int id);
    }

    public class PatronagePaymentService : IPatronagePaymentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PatronagePaymentService> _logger;
        private static readonly ActivitySource ActivitySource = new("Peggy.PatronagePaymentService");

        public PatronagePaymentService(AppDbContext context, ILogger<PatronagePaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PatronagePayment> CreatePaymentAsync(PatronagePayment payment)
        {
            using var activity = ActivitySource.StartActivity("CreatePayment");
            activity?.SetTag("payment.patronage_id", payment.PatronageId);
            activity?.SetTag("payment.amount", payment.Amount);

            try
            {
                _context.PatronagePayments.Add(payment);
                await _context.SaveChangesAsync();
                
                Metrics.PaymentCreationCounter.Inc();
                _logger.LogInformation("Created payment with ID {PaymentId}", payment.Id);
                
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<PatronagePayment?> GetPaymentAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("GetPayment");
            activity?.SetTag("payment.id", id);

            try
            {
                var payment = await _context.PatronagePayments.FindAsync(id);
                
                if (payment == null)
                {
                    _logger.LogWarning("Payment with ID {PaymentId} not found", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Payment not found");
                }
                else
                {
                    _logger.LogInformation("Retrieved payment with ID {PaymentId}", id);
                }
                
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment with ID {PaymentId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<PatronagePayment>> GetAllPaymentsAsync()
        {
            using var activity = ActivitySource.StartActivity("GetAllPayments");

            try
            {
                var payments = await _context.PatronagePayments.ToListAsync();
                _logger.LogInformation("Retrieved {Count} payments", payments.Count);
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payments");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<PatronagePayment> UpdatePaymentAsync(PatronagePayment payment)
        {
            using var activity = ActivitySource.StartActivity("UpdatePayment");
            activity?.SetTag("payment.id", payment.Id);

            try
            {
                _context.Entry(payment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                Metrics.PaymentUpdateCounter.Inc();
                _logger.LogInformation("Updated payment with ID {PaymentId}", payment.Id);
                
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment with ID {PaymentId}", payment.Id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task DeletePaymentAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("DeletePayment");
            activity?.SetTag("payment.id", id);

            try
            {
                var payment = await _context.PatronagePayments.FindAsync(id);
                if (payment == null)
                {
                    _logger.LogWarning("Payment with ID {PaymentId} not found for deletion", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Payment not found");
                    return;
                }

                _context.PatronagePayments.Remove(payment);
                await _context.SaveChangesAsync();
                
                Metrics.PaymentDeletionCounter.Inc();
                _logger.LogInformation("Deleted payment with ID {PaymentId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment with ID {PaymentId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }
    }
} 