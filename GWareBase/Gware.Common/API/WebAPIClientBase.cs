using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Gware.Common.API
{
    public abstract class WebAPIClientBase : APIClientBase
    {
        private string m_keyParameterName;
        private string m_apiAddress;
        public string APIAddress
        {
            get
            {
                return m_apiAddress;
            }

            set
            {
                m_apiAddress = value;
            }
        }

        protected string KeyParameterName
        {
            get
            {
                return m_keyParameterName;
            }

            set
            {
                m_keyParameterName = value;
            }
        }

        public WebAPIClientBase(string apiAddress,string username,string password)
            :base(username,password)
        {
            m_apiAddress = apiAddress;
        }
        public WebAPIClientBase(string apiAddress)
            : base()
        {
            m_apiAddress = apiAddress;
        }
        protected T AuthenticatedGet<T>(string uri)
        {
            T retVal = default(T);

            if (CheckAuthenticationKey())
            {
                retVal = APIHttpGet<T>(uri);
            }
            else
            {
                throw new Exception("Unauthorized Request");
            }

            return retVal;
        }

        protected T GetAuthenticatedResult<T>(string baseUri)
        {
            return AuthenticatedGet<T>(GetAuthenticatedUri(baseUri));
        }
        protected bool PostAuthenticatedData<T>(string baseUri,T obj)
        {
            bool retVal = false;
            if (CheckAuthenticationKey())
            {
                retVal = APIHttpPost<T>(GetAuthenticatedUri(baseUri), obj);
            }
            else
            {
                throw new Exception("Unauthorised Request");
            }
            return retVal;
        }

        protected T APIHttpGet<T>(string uri)
        {
            T retVal = default(T);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                Task<HttpResponseMessage> responseTask = client.GetAsync(string.Format("api/{0}",uri));
                responseTask.Wait();

                if (responseTask.Result.IsSuccessStatusCode)
                {
                    Task<T> deserializeTask = responseTask.Result.Content.ReadAsAsync<T>();
                    deserializeTask.Wait();

                    retVal = deserializeTask.Result;
                }
            }
            return retVal;
        }
        protected T APIHttpPost<T,K>(string uri,K postObj)
        {
            T retVal = default(T);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
                // New code:
                Task<HttpResponseMessage> responseTask = client.PostAsync(string.Format("api/{0}", uri), postObj, formatter);
                responseTask.Wait();

                if (responseTask.Result.IsSuccessStatusCode)
                {
                    Task<T> deserializeTask = responseTask.Result.Content.ReadAsAsync<T>();
                    deserializeTask.Wait();

                    retVal = deserializeTask.Result;
                }
            }
            return retVal;
        }
        protected bool APIHttpPost<K>(string uri,K postObj)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
                // New code:
                Task<HttpResponseMessage> responseTask = client.PostAsync(string.Format("api/{0}", uri), postObj, formatter);
                responseTask.Wait();

                return responseTask.Result.IsSuccessStatusCode;
            }
        }
        public static string BuildUri(string baseUri, params KeyValuePair<string, string>[] parameters)
        {
            StringBuilder retVal = new StringBuilder();
            retVal.Append(baseUri);
            retVal.Append("?");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0)
                {
                    retVal.Append("&");
                }
                retVal.Append(parameters[i].Key);
                retVal.Append("=");
                retVal.Append(parameters[i].Value);
            }
            return retVal.ToString();
        }
        private string GetAuthenticatedUri(string baseUri)
        {
            return BuildUri(baseUri,new KeyValuePair<string,string>(KeyParameterName, AuthenticationKey.Key));
        }
        
        
    }
}
