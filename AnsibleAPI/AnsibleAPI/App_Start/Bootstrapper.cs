using System.Web.Http;

namespace AnsibleAPI.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            AutoFacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
        }
    }
}