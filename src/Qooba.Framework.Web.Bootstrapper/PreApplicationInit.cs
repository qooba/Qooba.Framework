using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(Qooba.Framework.Web.Bootstrapper.PreApplicationInit), "InitializeModules")]
namespace Qooba.Framework.Web.Bootstrapper
{
    public class PreApplicationInit
    {
        public void InitializeModules()
        {
            var namePatterns = new List<string>();
            var configuration = ConfigurationManager.AppSettings["Qooba::Framework::InitializeModules::Patterns"];
            if (configuration != null)
            {
                namePatterns = configuration.Split(';').ToList();
            }

            Framework.Bootstrapper.Bootstrapp(namePatterns.ToArray());
        }
    }
}
