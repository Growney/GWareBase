using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API.Web
{
    
    public class WebAPIRequestResult<T>
    {
        private T m_result;
        private eRequestStatus m_status;
        private string m_exceptionMessage;

        public T Result
        {
            get
            {
                return m_result;
            }

            set
            {
                m_result = value;
            }
        }

        public eRequestStatus Status
        {
            get
            {
                return m_status;
            }

            set
            {
                m_status = value;
            }
        }

        public string ExceptionMessage
        {
            get
            {
                return m_exceptionMessage;
            }

            set
            {
                m_exceptionMessage = value;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return m_status == eRequestStatus.Success;
            }
        }

        public WebAPIRequestResult(T result)
        {
            m_result = result;
            m_status = eRequestStatus.Success;
        }

        public WebAPIRequestResult(string exceptionText)
        {
            m_status = eRequestStatus.ExceptionOccured;
            m_exceptionMessage = exceptionText;
        }

        public WebAPIRequestResult(eRequestStatus status)
        {
            m_status = status;
        }

        public WebAPIRequestResult(eKeyStatus status)
        {
            switch (status)
            {
                case eKeyStatus.Approved:
                    m_status = eRequestStatus.Success;
                    break;
                case eKeyStatus.Expired:
                    m_status = eRequestStatus.AuthenticationTimeOut;
                    break;
                case eKeyStatus.Unknown:
                default:
                    m_status = eRequestStatus.AuthenticationFailure;
                    break;
            }
        }
    }
}
