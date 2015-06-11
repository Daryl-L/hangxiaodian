using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace hangxiaodian.Helper
{
    class HttpRequestHelper
    {
        private string _url;
        private List<KeyValuePair<string, string>> _param;
        private HttpMethod _method;

        public HttpRequestHelper(string url, HttpMethod method, List<KeyValuePair<string, string>> param = null)
        {
            _url = url;
            _method = method;
            _param = param;
        }

        public async Task<string> Request()
        {
            var request = new HttpRequestMessage(_method, new Uri(_url));
            var httpClient = new HttpClient();
            request.Content = new HttpFormUrlEncodedContent(_param);
            string responseString = string.Empty;
            try
            {
                var response = await httpClient.SendRequestAsync(request);
                responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseString;
        }
    }
}
