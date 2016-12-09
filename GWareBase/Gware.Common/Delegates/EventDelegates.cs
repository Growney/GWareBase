using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Delegates
{
    public delegate void SingleResult<T, K>(T sender, K result);
    public delegate void SingleResult<T>(object sender,T result);
    public delegate void SingleResultWithReturn<T,returnType>(object sender,T result,ref returnType retVal);
    public delegate bool UserQueryContinue();
    public delegate string UserQueryString(params string[] parameters);
    public delegate LoginResult RequestLogin(string username,bool failed);
}
