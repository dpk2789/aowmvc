using Microsoft.AspNetCore.Http;
using System;

namespace AowCore.AppWeb.Helpers
{
    public class CookieHelper : ICookieHelper
    {
        private IHttpContextAccessor _httpContextAccessor;
        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void Set(string key, string value, int? expireTime)
        {
            var option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                //option.Domain = ".accountingonweb.com";
                //option.IsEssential = true;              
                //option.Path = "/users/";
            }
            else
                //option.Expires = DateTime.Now.AddMilliseconds(10);
                option.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
            //  Response.Cookies.Append("rudeCookie", "I don't need no user to tell me it's ok.", option);
            //  _httpContextAccessor.HttpContext.Response.Cookies.Append("user_id", "1");
        }

        public string Get(string key)
        {            
            var cValue = _httpContextAccessor.HttpContext.Request.Cookies[key];       
            return cValue;
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }
    }
}
