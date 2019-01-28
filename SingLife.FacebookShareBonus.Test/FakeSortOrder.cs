using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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