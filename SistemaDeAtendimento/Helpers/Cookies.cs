using System;
using System.Configuration;
using System.Web;


namespace SistemaDeAtendimento.Helpers
{
    public static class Cookies
    {
        private const string CookieSetting = "Cookie.Duration";
        private const string CookieIsHttp = "Cookie.IsHttp";
        private static HttpContext _context { get { return HttpContext.Current; } }
        private static int _cookieDuration { get; set; }
        private static bool _cookieIsHttp { get; set; }

        static Cookies()
        {
            _cookieDuration = GetCookieDuration();
            _cookieIsHttp = GetCookieType();
        }

        public static void Set(string key, string value)
        {
            var c = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddDays(_cookieDuration),
                HttpOnly = _cookieIsHttp
            };
            _context.Response.Cookies.Add(c);
        }

        public static string Get(string key)
        {
            var value = string.Empty;

            var c = _context.Request.Cookies[key];
            return c != null
                    ? _context.Server.HtmlEncode(c.Value).Trim()
                    : value;
        }

        public static bool Exists(string key)
        {
            return _context.Request.Cookies[key] != null;
        }

        public static void Delete(string key)
        {
            if (Exists(key))
            {
                var c = new HttpCookie(key) { Expires = DateTime.Now.AddDays(-1) };
                _context.Response.Cookies.Add(c);
            }
        }

        public static void DeleteAll()
        {
            for (int i = 0; i <= _context.Request.Cookies.Count - 1; i++)
            {
                if (_context.Request.Cookies[i] != null)
                    Delete(_context.Request.Cookies[i].Name);
            }
        }

        private static int GetCookieDuration()
        {
            //default
            int duration = 360;
            var setting = ConfigurationManager.AppSettings[CookieSetting];

            if (!string.IsNullOrEmpty(setting))
                int.TryParse(setting, out duration);

            return duration;
        }

        private static bool GetCookieType()
        {
            //default
            var isHttp = true;
            var setting = ConfigurationManager.AppSettings[CookieIsHttp];

            if (!string.IsNullOrEmpty(setting))
                bool.TryParse(setting, out isHttp);

            return isHttp;
        }
    }
}