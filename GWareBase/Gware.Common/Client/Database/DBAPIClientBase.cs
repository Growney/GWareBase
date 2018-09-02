namespace Gware.Common.Client.Database
{
    public abstract class DBAPIClientBase : IClient
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
        public bool Trusted { get; set; }

        public DBAPIClientBase(string serverName, string databaseName, string databaseUsername, string databasePassword)
        {
            SetDetails(serverName, databaseName, databaseUsername, databasePassword);
        }

        public DBAPIClientBase(string serverName, string databaseName)
        {
            SetDetails(serverName, databaseName);
        }
        public void SetDetails(string serverName, string databaseName, string databaseUsername, string databasePassword)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            DatabaseUsername = databaseUsername;
            DatabasePassword = databasePassword;
        }

        public void SetDetails(string serverName,string databaseName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            Trusted = true;
        }

        public abstract bool CanConnect();
        public abstract ClientConnectionStatus GetConnectionStatus();
    }
}
