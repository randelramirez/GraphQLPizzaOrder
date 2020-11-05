using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Models
{
    public class TokenModel
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpireOn { get; set; }
    }
}
