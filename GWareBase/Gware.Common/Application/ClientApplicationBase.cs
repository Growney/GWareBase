using Gware.Common.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Application
{
    public abstract class ClientApplicationBase<T> : ApplicationBase where T : IAPIClient
    {
        private T m_applicationApiClient;
        private int m_defaultKeyExpiry = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
        private int m_defaultKeySize = 64;

        public T ApplicationApiClient
        {
            get
            {
                return m_applicationApiClient;
            }
        }

        public int DefaultKeyExpiry
        {
            get
            {
                return m_defaultKeyExpiry;
            }

            set
            {
                m_defaultKeyExpiry = value;
            }
        }

        public int DefaultKeySize
        {
            get
            {
                return m_defaultKeySize;
            }

            set
            {
                m_defaultKeySize = value;
            }
        }

        public ClientApplicationBase(T apiClient,bool useNetworkOrder)
            : base(useNetworkOrder)
        {
            m_applicationApiClient = apiClient;
        }
        public ClientApplicationBase(T apiClient,Encoding applicationEncoding)
            : base(applicationEncoding)
        {
            m_applicationApiClient = apiClient;
        }
        public ClientApplicationBase(T apiClient,Encoding applicationEncoding, bool useNetworkOrder)
            :base(applicationEncoding, useNetworkOrder)
        {
            m_applicationApiClient = apiClient;
        }
        public ClientApplicationBase(T apiClient) : base()
        {
            m_applicationApiClient = apiClient;
        }
        
        public UserAuthenticationKey CreateKey(int userID)
        {
            return UserAuthenticationKey.CreateKey(userID, TimeSpan.FromMilliseconds(m_defaultKeyExpiry), ApplicationEncoding, ApplicationRandom, DefaultKeySize);
        }
    }
}
