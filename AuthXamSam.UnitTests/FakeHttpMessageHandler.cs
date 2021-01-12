using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWineDb.Mobile.UnitTests
{
    // Extracted from the GitHub Repository https://gist.github.com/benhysell/499581ed48299cd442f1
    // Thank You Benjamin Hysell!
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        readonly HttpResponseMessage response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            this.response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            tcs.SetResult(response);

            return tcs.Task;
        }

        protected Task<HttpResponseMessage> GetAsync(string Uri)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(response);
            return tcs.Task;
        }
    }
}
