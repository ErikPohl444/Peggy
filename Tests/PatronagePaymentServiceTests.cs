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
    public class PatronagePaymentServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly PatronagePaymentService _patronagePaymentService;

        public PatronagePaymentServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _patronagePaymentService = new PatronagePaymentService(_mockContext.Object);
        }

        [Fact]
        public async Task CreatePatronagePaymentAsync_ShouldAddPatronagePaymentToContext()
        {
            var payment = new PatronagePayment { PaymentId = 1, PatronageId = 1, PaymentAmount = 100 };
            var mockDbSet = new Mock<DbSet<PatronagePayment>>();
            _mockContext.Setup(c => c.PatronagePayments).Returns(mockDbSet.Object);

            await _patronagePaymentService.CreatePatronagePaymentAsync(payment);

            mockDbSet.Verify(db => db.Add(payment), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdatePatronagePaymentAsync_ShouldUpdatePatronagePaymentInContext()
        {
            var payment = new PatronagePayment { PaymentId = 1, PatronageId = 1, PaymentAmount = 200 };
            var mockDbSet = new Mock<DbSet<PatronagePayment>>();
            _mockContext.Setup(c => c.PatronagePayments).Returns(mockDbSet.Object);

            await _patronagePaymentService.UpdatePatronagePaymentAsync(payment);

            _mockContext.Verify(c => c.Entry(payment).State = EntityState.Modified, Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetPatronagePaymentByIdAsync_ShouldReturnPatronagePayment()
        {
            var payment = new PatronagePayment { PaymentId = 1, PatronageId = 1, PaymentAmount = 100 };
            var mockDbSet = new Mock<DbSet<PatronagePayment>>();
            _mockContext.Setup(c => c.PatronagePayments).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(payment);

            var result = await _patronagePaymentService.GetPatronagePaymentByIdAsync(1);

            Assert.Equal(payment, result);
        }

        [Fact]
        public async Task GetAllPatronagePaymentsAsync_ShouldReturnAllPatronagePayments()
        {
            var payments = new List<PatronagePayment> { new PatronagePayment { PaymentId = 1, PatronageId = 1, PaymentAmount = 100 }, new PatronagePayment { PaymentId = 2, PatronageId = 2, PaymentAmount = 200 } };
            var mockDbSet = new Mock<DbSet<PatronagePayment>>();
            _mockContext.Setup(c => c.PatronagePayments).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.ToListAsync()).ReturnsAsync(payments);

            var result = await _patronagePaymentService.GetAllPatronagePaymentsAsync();

            Assert.Equal(payments, result);
        }

        [Fact]
        public async Task DeletePatronagePaymentAsync_ShouldRemovePatronagePaymentFromContext()
        {
            var payment = new PatronagePayment { PaymentId = 1, PatronageId = 1, PaymentAmount = 100 };
            var mockDbSet = new Mock<DbSet<PatronagePayment>>();
            _mockContext.Setup(c => c.PatronagePayments).Returns(mockDbSet.Object);
            mockDbSet.Setup(db => db.FindAsync(1)).ReturnsAsync(payment);

            await _patronagePaymentService.DeletePatronagePaymentAsync(1);

            mockDbSet.Verify(db => db.Remove(payment), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
} 