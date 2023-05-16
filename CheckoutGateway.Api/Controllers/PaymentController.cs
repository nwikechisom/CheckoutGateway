using CheckoutGateway.BusinessLogic.Commands.PostPayment;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Queries.PaymentDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutGateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISender _mediator;

        public PaymentController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RequestPayment")]
        public async Task<ActionResult> RequestPaymentAsync(RequestPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("PostPayment")]
        public async Task<ActionResult> PostPaymentAsync(PostPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetPaymentDetail/{paymentreference}")]
        public async Task<ActionResult> PostPaymentAsync([FromRoute] string paymentreference)
        {
            var result = await _mediator.Send(new PaymentDetailQuery { TransactionReference = paymentreference});
            return Ok(result);
        }
    }
}
