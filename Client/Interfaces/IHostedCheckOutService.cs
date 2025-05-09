using Client.Common.Models;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Client.Interfaces
{
    public interface IHostedCheckOutService
    {
        //Task<ClientResponse> CreateSessiontest(sessionRequest request);
        Task<HostedCheckoutResult> CreateSession(sessionRequest request);

        Task<HostedCheckoutResponse> CreateSessionv1(sessionRequest request);
        Task<VerifyTransactionResult> Verify(string orderid);
        Task<VerifyTransactionResultV2> VerifyV2(string orderid);
    }
}
