using System.Collections.Generic;

namespace SingLife.FacebookShareBonus.Model
{
    public interface IPolicySortService
    {
        // <summary>
        // Sorts the specified policies.
        // </Summary>
        // <param name = "policies"> An enumrable of polices to be sorted. </param>
        // <returns> An ordered enumrable of policies. </returns>
        IEnumerable<Policy> Sort(IEnumerable<Policy> policies);
    }
}