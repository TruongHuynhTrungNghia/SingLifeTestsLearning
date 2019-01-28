using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
    internal class AscendingOrderOfPoliciesStartDate : IPolicySortService
    {
        public IEnumerable<Policy> Sort(IEnumerable<Policy> policies)
        {
            policies = policies.OrderBy(p => p.StartDate);
            return policies;
        }
    }
}