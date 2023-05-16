using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutGateway.BusinessLogic.Services.Caching;

public interface ICacheService
{
    T Get<T>(string key);
    T Set<T>(string key, T value);
}
