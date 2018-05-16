using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Client.Web
{
    public class WebAPIClient : HttpClient
    {
        public WebAPIClient(string apiAddress)
        {
            BaseAddress = new Uri(apiAddress);
        }
        
        public static string BuildUri(string baseUri, params (string key,object value)[] parameters)
        {
            StringBuilder retVal = new StringBuilder();
            retVal.Append(baseUri);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0)
                {
                    retVal.Append("&");
                }
                else
                {
                    retVal.Append("?");
                }
                retVal.Append(parameters[i].key);
                retVal.Append("=");
                retVal.Append(parameters[i].value.ToString());
            }
            return retVal.ToString();
        }

        public T APIPostContent<T>(string uri, HttpContent content, AuthenticationHeaderValue authentication)
        {
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Authorization = authentication;

            return WebAPIClient.ParseResponse<T>(PostAsync(uri, content));
        }
        

        public T APIJSONGet<T>(string uri, AuthenticationHeaderValue authentication)
        {
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Authorization = authentication;

            return WebAPIClient.ParseResponse<T>(GetAsync(uri));
        }

        public T APIJSONGet<T>(string uri)
        {
            return APIJSONGet<T>(uri, null);
        }

        public static T ParseResponse<T>(System.Net.Http.HttpResponseMessage response)
        {
            T retVal = default(T);

            Task<T> deserializeTask = response.Content.ReadAsAsync<T>();
            deserializeTask.Wait();

            retVal = deserializeTask.Result;

            return retVal;
        }

        public static T ParseResponse<T>(Task<System.Net.Http.HttpResponseMessage> response)
        {
            T retVal = default(T);

            response.Wait();
            if (response.Status != TaskStatus.RanToCompletion)
            {
                throw new Exception("Error Retreiving Data", response.Exception);
            }

            Task<T> deserializeTask = response.Result.Content.ReadAsAsync<T>();
            deserializeTask.Wait();

            retVal = deserializeTask.Result;

            return retVal;
        }

        public static Task<T> ParseResponseAsync<T>(System.Net.Http.HttpResponseMessage response)
        {
            return response.Content.ReadAsAsync<T>();
        }


    }
}
