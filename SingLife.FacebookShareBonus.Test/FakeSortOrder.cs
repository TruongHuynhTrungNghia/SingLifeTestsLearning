using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System.Collections.Generic;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    internal class FakeSortOrder : IPolicySortService
    {
        public IEnumerable<Policy> Sort(IEnumerable<Policy> policies)
        {
            return policies;
        }
    }
}