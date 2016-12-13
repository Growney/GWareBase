using Gware.Common.Networking.FramedConnection;
using Gware.Common.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.AuthenticatedConnection
{
    public class AuthenticatedClient : FramedClient
    {
        private string m_token;
        private bool m_useNetworkOrder;
        private Encoding m_encoding;
        public AuthenticatedClient(ClientServerConnectionType type,bool useNetworkOrder,Encoding encoding)
            : base(type,useNetworkOrder)
        {
            m_useNetworkOrder = useNetworkOrder;
            m_encoding = encoding;
        }

        public void RequestAuthenticationAsync(string username, string password)
        {
            AuthenticatedCommand cmd = new AuthenticatedCommand(eAuthenticatedCommand.AuthenticationRequest, eErrors.None,m_useNetworkOrder,m_encoding);
            cmd.Username = username;
            cmd.Password = password;
            Send(cmd);
        }

        protected override void OnDataCompleted(System.Net.IPEndPoint sender, byte[] result)
        {
            AuthenticatedCommand rxCmd = new AuthenticatedCommand();
            rxCmd.FromBytes(result);
            
            switch ((eAuthenticatedCommand)rxCmd.CommandID)
            {
                case eAuthenticatedCommand.AuthenticationResult:
                    HandleAuthenticationResult(rxCmd);
                    break;
                case eAuthenticatedCommand.TokenRefresh:
                    m_token = rxCmd.AuthenticationToken;
                    break;
                case eAuthenticatedCommand.DataTransfer:
                    
                    break;
                case eAuthenticatedCommand.None:
                default:
                    break;
            }
        }
        private void HandleIncomingDataCommand(AuthenticatedCommand cmd)
        {
            if ((eAuthenticatedCommand)cmd.CommandID == eAuthenticatedCommand.DataTransfer)
            {
                OnSessionDataRx(cmd.Data);
            }
        }
        private void HandleAuthenticationResult(AuthenticatedCommand cmd)
        {

            if ((eAuthenticatedCommand)cmd.CommandID == eAuthenticatedCommand.AuthenticationResult)
            {
                if ((eErrors)cmd.ErrorCode == eErrors.None)
                {
                    m_token = cmd.AuthenticationToken;
                }
                OnAuthenticationResult((eErrors)cmd.ErrorCode);
            }

        }
        protected virtual void OnSessionDataRx(byte[] result)
        {

        }

        protected virtual void OnAuthenticationResult(eErrors error)
        {

        }

        public void Send(AuthenticatedCommand cmd,bool waitForResponse = false,int timeout = 2000)
        {
            cmd.AuthenticationToken = m_token;
            base.Send(cmd.ToBytes());
            if (waitForResponse)
            {
            }
        }

        public override void Send(byte[] data)
        {
            AuthenticatedCommand cmd = new AuthenticatedCommand(eAuthenticatedCommand.DataTransfer, eErrors.None,m_useNetworkOrder,m_encoding);
            Send(cmd);
        }

        
    }
}
