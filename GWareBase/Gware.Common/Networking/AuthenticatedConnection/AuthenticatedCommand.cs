using Gware.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.AuthenticatedConnection
{
    public enum eAuthenticatedCommand
    {
        None = 0,
        AuthenticationRequest = 1,
        AuthenticationResult = 2,
        DataTransfer = 3,
        DataTransferResponse = 4,
        TokenRefresh = 5,
    }

    public enum eErrors
    {
        None = 0,
        AuthenticationFailed = 1,
        UnknownCommand = 2,
        ValidExpiredToken = 3,
        InvalidToken = 4,
        InvalidEndPoint = 5,
        
        InvalidUsername = 6,
        InvalidPassword = 7,
        InvalidSubscription = 8,

    }
    public class AuthenticatedCommand
    {
        private static int c_currentClientCommandID;

        private static readonly Encoding c_stringEncoding = Encoding.Unicode;
        private int m_commandID;
        private int m_errorCode;

        private string m_authenticationToken;
        private string m_username;
        private string m_password;
        private byte[] m_data = new byte[0];
        private int m_clientCommandID;

        public int ClientCommandID
        {
            get { return m_clientCommandID; }
            set { m_clientCommandID = value; }
        }

        public byte[] Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public int DataLength
        {
            get { return m_data.Length; }
        }

        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        public string AuthenticationToken
        {
            get { return m_authenticationToken; }
            set { m_authenticationToken = value; }
        }

        public int ErrorCode
        {
            get { return m_errorCode; }
            set { m_errorCode = value; }
        }
        public int CommandID
        {
            get { return m_commandID; }
            protected set { m_commandID = value; }
        }
        public AuthenticatedCommand()
        {
            m_clientCommandID = c_currentClientCommandID++;
        }
        public AuthenticatedCommand(eAuthenticatedCommand command, eErrors error)
        {
            m_clientCommandID = c_currentClientCommandID++;
            CommandID = (int)command;
            ErrorCode = (int)error;
        }

        public void FromBytes(byte[] bytes)
        {
            BufferReader reader = new BufferReader(bytes);

            m_clientCommandID = reader.ReadInt32();
            m_commandID = reader.ReadInt32();
            m_errorCode = reader.ReadInt32();
            m_authenticationToken = reader.ReadString();
            m_username = reader.ReadString();
            m_password = reader.ReadString();
            int dataLength = reader.ReadInt32();
            m_data = reader.ReadBytes(dataLength);
        }

        public byte[] ToBytes()
        {
            BufferWriter writer = new BufferWriter();

            writer.WriteInt32(m_clientCommandID);
            writer.WriteInt32(m_commandID);//The header length and the data length must remain at the start of the packet header
            writer.WriteInt32(m_errorCode);
            writer.WriteString(m_authenticationToken);
            writer.WriteString(m_username);
            writer.WriteString(m_password);
            writer.WriteInt32(m_data.Length);
            writer.WriteBytes(m_data);

            return writer.GetBuffer();
        }
    }
}
