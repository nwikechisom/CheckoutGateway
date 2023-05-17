using CheckoutGateway.Api.Controllers;
using CheckoutGateway.BusinessLogic.Commands.PostPayment;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Queries.PaymentDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CheckoutGateway.Tests.ControllerTests;
    public class PaymentControllerTests
    {
        private readonly Mock<ISender> _mediatorMock;
        private readonly PaymentController _controller;

        public PaymentControllerTests()
        {
            _mediatorMock = new Mock<ISender>();
            _controller = new PaymentController(_mediatorMock.Object);
        }

        [Fact]
        public async Task RequestPaymentAsync_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new RequestPaymentCommand();
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(new RequestPaymentResponse { Message = ""});

            // Act
            var result = await _controller.RequestPaymentAsync(command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mediatorMock.Verify(m => m.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task PostPaymentAsync_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new PostPaymentCommand();
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(new PostPaymentResponse { });

            // Act
            var result = await _controller.PostPaymentAsync(command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mediatorMock.Verify(m => m.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task GetPaymentDetail_ValidReference_ReturnsOkResult()
        {
            // Arrange
            var paymentReference = "payment123";
            var query = new PaymentDetailQuery { TransactionReference = paymentReference };
            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(new PaymentDetailQueryResponse { });

            // Act
            var result = await _controller.PostPaymentAsync(paymentReference);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mediatorMock.Verify(m => m.Send(query, default), Times.Once);
        }
    }
