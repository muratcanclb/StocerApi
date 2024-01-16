using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Thy.Api.Demo.Controllers.ARR
{
    [Route("api/[controller]")]
    public class ArrController : Controller
    {
        /// <summary>
        /// ARR Application ayakta olup olmadıgını kontrol etmek için bu servise ping atar.
        /// Log dışında tutuldugundan Controller inherit alır.
        /// Auth dışında olmalıdır.
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            var value = new Dictionary<string, string>
            {
                { "ARR", "OK" }
            };
            return value;
        }
    }
}