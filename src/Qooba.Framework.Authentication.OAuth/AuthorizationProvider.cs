using AspNet.Security.OpenIdConnect.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Authentication.OAuth
{
    public class AuthorizationProvider : OpenIdConnectServerProvider
    {
        //TODO: RC2
        //public override Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
        //{
            //if (string.IsNullOrEmpty(context.ClientId) || string.IsNullOrEmpty(context.ClientSecret))
            //{
                //context.Rejected(
                    //error: "invalid_request",
                    //description: "missing credentials");
            //}

            //return base.ValidateClientAuthentication(context);
        //}
    }
}
