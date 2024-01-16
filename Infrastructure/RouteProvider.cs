using Microsoft.AspNetCore.Routing;
using Meb.Api.Framework.Localization;
using Meb.Api.Framework.Mvc.Routing;

namespace Intra.Api.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //routeBuilder.MapLocalizedRoute("ARR", "GetARR", new { controller = "ARR", action = "Get" });
            //routeBuilder.MapLocalizedRoute("environment", "environment", new { controller = "DbCheck", action = "Get" });
            //routeBuilder.MapLocalizedRoute("Postman", "Postman", new { controller = "Postman", action = "Postman" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return 0; }
        }

        #endregion
    }
}