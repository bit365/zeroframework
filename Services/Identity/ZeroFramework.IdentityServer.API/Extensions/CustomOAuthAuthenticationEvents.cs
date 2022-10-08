using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ZeroFramework.IdentityServer.API.Extensions
{
    public class CustomOAuthAuthenticationEvents : OAuthEvents
    {
        public override async Task RemoteFailure(RemoteFailureContext context)
        {
            //.Net Core Identity 2 Provider login Cancel leads to unhandled exception
            await base.RemoteFailure(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            return base.TicketReceived(context);
        }

        public override async Task CreatingTicket(OAuthCreatingTicketContext context)
        {
            await base.CreatingTicket(context);
        }
    }
}
