using Raven.Client.Document;
using System;
using System.Threading.Tasks;

namespace RavenLibrary
{
    public class RavenUtils
    {
        public int DbPort { get; set; }
        public string DbAddress { get; set; }

        public RavenUtils(){}

        public RavenUtils(string address, int port)
        {
            DbAddress = address;
            DbPort = port;
        }

        public bool CheckConnection()
        {
            if (!string.IsNullOrEmpty(DbAddress) && DbPort > 0)
            {
                DocumentStore documentStore = null;
                try
                {
                    documentStore = new DocumentStore { Url = string.Format("http://{0}:{1}", DbAddress, DbPort) };
                    return true;
                }
                catch(Exception ex) { throw ex; }
                finally
                {
                    if (documentStore != null)
                        documentStore.Dispose();
                }
            }
            else
                return false;
        }
    }
}
