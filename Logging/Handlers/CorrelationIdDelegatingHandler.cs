using System.Diagnostics;

namespace GlobalPay.Logging.Handlers
{
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("X-Correlation-ID") && Activity.Current?.TraceId != null)
            {
                request.Headers.Add("X-Correlation-ID", Activity.Current.TraceId.ToString());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
