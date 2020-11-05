using GraphQL.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels
{
    public class GraphQLUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
