// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Madamin.Unfollow.Instagram.API
{
    internal class HttpRequestProcessor
    {   
        public HttpRequestProcessor(
            HttpClient httpClient,
            HttpClientHandler httpHandler,
            ApiRequestMessage request_message)
        {
            Client = httpClient;
            HttpHandler = httpHandler;
            RequestMessage = request_message;
        }

        public HttpClientHandler HttpHandler { get; set; }
        public HttpClient Client { get; set; }
        public ApiRequestMessage RequestMessage { get; set; }

        public void SetHttpClientHandler(HttpClientHandler handler)
        {
            HttpHandler = handler;
            Client = new HttpClient(handler);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            Client.DefaultRequestHeaders.ConnectionClose = false;
            requestMessage.Headers.Add("Connection", "Keep-Alive");
            LogHttpRequest(requestMessage);
            await Task.Delay(1000);
            var response = await Client.SendAsync(requestMessage);
            LogHttpResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            Client.DefaultRequestHeaders.ConnectionClose = false;
            //_logger?.LogRequest(requestUri);
            //if (_delay.Exist)
                //await Task.Delay(_delay.Value);
            var response = await Client.GetAsync(requestUri);
            LogHttpResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption)
        {
            Client.DefaultRequestHeaders.ConnectionClose = false;
            requestMessage.Headers.Add("Connection", "Keep-Alive");
            LogHttpRequest(requestMessage);
            //if (_delay.Exist)
                //await Task.Delay(_delay.Value);
            var response = await Client.SendAsync(requestMessage, completionOption);
            LogHttpResponse(response);
            return response;
        }

        public async Task<string> SendAndGetJsonAsync(
            HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption)
        {
            Client.DefaultRequestHeaders.ConnectionClose = false;
            LogHttpRequest(requestMessage);
            //if (_delay.Exist)
                //await Task.Delay(_delay.Value);
            var response = await Client.SendAsync(requestMessage, completionOption);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GeJsonAsync(Uri requestUri)
        {
            Client.DefaultRequestHeaders.ConnectionClose = false;
            //_logger?.LogRequest(requestUri);
            //if (_delay.Exist)
                //await Task.Delay(_delay.Value);
            var response = await Client.GetAsync(requestUri);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        private void LogHttpRequest(HttpRequestMessage request)
        {
            //_logger?.LogRequest(request);
        }

        private void LogHttpResponse(HttpResponseMessage request)
        {
            //_logger?.LogResponse(request);
        }
    }
}