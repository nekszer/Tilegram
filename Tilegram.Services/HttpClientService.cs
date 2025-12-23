using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Tilegram.Feature
{
    public class HttpClientService
    {
        public string AccessToken { get; set; }

        public HttpClientService (string accessToken)
        {
            AccessToken = accessToken;
        }

        public async Task<Either<Exception,T>> ExecuteGetAsync<T>(string uri)
        {
            try
            {
                var httpClient = new HttpClient();

                if (!string.IsNullOrEmpty(AccessToken))
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accessToken", AccessToken);
                
                var response = await httpClient.GetAsync(uri);

                var contentResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException(contentResponse);
                
                var data = JsonConvert.DeserializeObject<T>(contentResponse);
                return Either<Exception, T>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, T>.Left(ex);
            }
        }

        public async Task<Either<Exception, T>> ExecutePostAsync<T>(string uri)
        {
            try
            {
                var httpClient = new HttpClient();

                if (!string.IsNullOrEmpty(AccessToken))
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accessToken", AccessToken);

                var response = await httpClient.PostAsync(uri, CurrentFormUrlEncodedContent);
                var contentResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException(contentResponse);

                var data = JsonConvert.DeserializeObject<T>(contentResponse);
                return Either<Exception, T>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, T>.Left(ex);
            }
        }

        private FormUrlEncodedContent CurrentFormUrlEncodedContent { get; set; }
        public HttpClientService FormUrlEncodedContent<T>(T data)
        {
            var dataList = new List<KeyValuePair<string, string>>();
            var properties = data.GetType().GetRuntimeProperties();
            foreach (var propertyInfo in properties)
            {
                var attributes = propertyInfo.GetCustomAttributes();
                var requestAttribute = attributes.FirstOrDefault(a => a is RequestAttribute) as RequestAttribute;
                if (requestAttribute == null)
                    continue;

                var propertyValue = propertyInfo.GetValue(data)?.ToString() ?? string.Empty;
                dataList.Add(new KeyValuePair<string, string>(requestAttribute.FieldName, propertyValue));
            }

            CurrentFormUrlEncodedContent = new FormUrlEncodedContent(dataList);
            return this;
        }


    }
}
