using System.Collections.Generic;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool database, bool textFiles)
        {
            if (database)
            {
                //TODO - Set up the SQL connection properly
                var sql = new SqlConnection();
                Connections.Add(sql);
            }

            if (textFiles)
            {
                //TODO - Create the text connection
                var text = new TextConnection();
                Connections.Add(text);
            }
        }
    }
}