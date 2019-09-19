namespace DaGetV2.Service
{
    public class AppConfiguration
    {
        public string IntrospectionEndPoint { get; set; }

        public string RessourceServerName { get; set; }

        public string Password { get; set; }

        public string[] DefaultsOperationTypes { get; set; }

        public DataBaseType DataBaseType { get; set; }
    }

    public enum DataBaseType
    {
        SqlServer = 1,
        CosmosDb = 2
    }
}
