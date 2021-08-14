using Microsoft.Extensions.Hosting;
using System.IO;

namespace AowCore.AppWeb.Helpers
{
    public class CommonFunctions
    {
        private readonly IHostEnvironment _env;

        public CommonFunctions(IHostEnvironment env)
        {
            _env = env;
        }
        public string ReadHtmlTemplate(string FileName)
        {
            string htmlstring = string.Empty;
            if (string.IsNullOrEmpty(FileName) == false)
            {
                if (FileName.ToLower() == "registration")
                    htmlstring = Path.Combine(_env.ContentRootPath, "~/Templates/Registration.html");               
                else if (FileName.ToLower() == "confirmaccount")                   
                htmlstring = Path.Combine(_env.ContentRootPath, "~/Templates/ConfirmAccount.html");
            }
            return htmlstring;
        }
    }
}
