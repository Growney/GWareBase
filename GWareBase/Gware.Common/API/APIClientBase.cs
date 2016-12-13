﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public abstract class APIClientBase : IAPIClient
    {
        private string m_password;
        private string m_username;

        private UserAuthenticationKey m_authenticationKey;

        public string Password
        {
            get
            {
                return m_password;
            }

            set
            {
                m_password = value;
            }
        }
        public string Username
        {
            get
            {
                return m_username;
            }

            set
            {
                m_username = value;
            }
        }
        public int UserID
        {
            get
            {
                if(IsAuthenticated)
                {
                    return AuthenticationKey.UserID;
                }
                else
                {
                    throw new UnauthorizedAccessException("Must be authenticated to access user ID");
                }
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return AuthenticationKey != null && AuthenticationKey.IsAuthenticated;
            }
        }

        public UserAuthenticationKey AuthenticationKey
        {
            get
            {
                return m_authenticationKey;
            }
        }

        public APIClientBase(string username,string password)
        {
            m_username = username;
            m_password = password;
        }
        
        public bool CheckAuthenticationKey(int retry = 3)
        {
            bool retVal = false;
            if (retry > 0)
            {
                if (AuthenticationKey != null)
                {
                    if (AuthenticationKey.IsAuthenticated)
                    {
                        retVal = true;
                    }
                    else
                    {
                        Authenticate();
                        retVal = CheckAuthenticationKey(retry - 1);
                    }
                }
                else
                {
                    Authenticate();
                    retVal = CheckAuthenticationKey(retry - 1);
                }
            }
            return false;
        }
        public int Authenticate()
        {
            UserAuthenticationResult result = GetAuthenticationKey();
            if (result.Success)
            {
                m_authenticationKey = result.Key;
            }
            
            return result.ErrorCode;
        }
        public bool UnAuthenticate()
        {
            if (RemoveAuthenticationKey())
            {
                m_authenticationKey = null;
            }
            return !IsAuthenticated;
        }
        protected abstract UserAuthenticationResult GetAuthenticationKey();
        protected abstract bool RemoveAuthenticationKey();

        public abstract bool CanConnect();
    }
}