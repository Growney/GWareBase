using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public enum eLoginResult
    {
        None = 0,
        UsernameNotFound = 1,
        PasswordDoesNotMatch = 2,
        SubscriptionExpired = 3,
        SystemError = 4,
    }

    public enum eKeyStatus
    {
        Approved = 0,
        Unknown = 1,
        Expired = 2
    }

    public enum eUserCreationResult
    {
        Success = 0,
        InvalidUsername = 1,
        Existing = 2,
        InvalidPassword = 3,
    }

    public enum eRequestStatus
    {
        Success,
        AuthenticationFailure,
        AuthenticationTimeOut,
        ExceptionOccured,
    }
}
