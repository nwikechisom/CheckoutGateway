using CheckoutGateway.Api.Controllers;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CheckoutGateway.Tests.ControllerTests
{
    internal class PaymentControllerTest
    {
        private PaymentController _controller;
        private Mock<ISender> _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<ISender>();
            _controller = new PaymentController(_mediator.Object);
        }

        [Test]
        public async Task RequestPayment_WhenValid_ReturnsToken()
        {

            var request = new RequestPaymentCommand
            {

            };

            var expected = BaseResponse<PaymentResponseModel>
            .Success(new PaymentResponseModel { },
            "An authorization token has been sent to the customer",
            OperationResultStatus.Success,
            Guid.NewGuid().ToString().Replace("-", ""));

            _mediator.Setup(x => x.Send(It.IsAny<RequestPaymentCommand>(), default))
            .ReturnsAsync(expected);

            // Act
            var result = await _controller.RequestPaymentAsync(request);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkResult)result;
            Assert.AreEqual(expected, okResult.ExecuteResult);
        }
    }
}
