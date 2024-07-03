using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IVnPayRepository
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
