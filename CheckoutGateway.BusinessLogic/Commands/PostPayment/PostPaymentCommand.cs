using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutGateway.BusinessLogic.Commands.PostPayment;

public class PostPaymentCommand : IRequest<PostPaymentResponse>
{
    public string OneTimePassword { get; set; }
    public string TransactionReference { get; set; }
    public string MerchantId { get; set; }
}
