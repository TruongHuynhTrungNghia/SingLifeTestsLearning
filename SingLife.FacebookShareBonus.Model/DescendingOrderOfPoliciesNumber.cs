using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
    internal class DescendingOrderOfPoliciesNumber : IPolicySortService
    {
        public IEnumerable<Policy> Sort(IEnumerable<Policy> policies)
        {
            IEnumerable<Policy> sortedPolicies = policies.OrderByDescending(s => s.PolicyNumber, StringComparer.CurrentCultureIgnoreCase);
            return sortedPolicies;
        }
    }
}