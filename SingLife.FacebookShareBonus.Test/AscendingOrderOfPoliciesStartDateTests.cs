using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    internal class AscendingOrderOfPoliciesStartDateTests
    {
        //Format matched dd/MM/yyyy
        [Test]
        public void SortsPoliciesStartDateInAscendingOrder_FirstPolicyDate08122017SecondPolicyDate12052016_PoliciesIsInAscendingOrder()
        {
            // Arrange
            Policy firstPolicy = new Policy();
            firstPolicy.StartDate = DateTime.ParseExact("08/12/2017", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Policy secondPolicy = new Policy();
            secondPolicy.StartDate = DateTime.ParseExact("12/05/2016", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            IEnumerable<Policy> policies = new Policy[2];
            Policy[] testPolicies = new Policy[2] { firstPolicy, secondPolicy };
            IEnumerable<Policy> expectedPolicies = new Policy[2] { secondPolicy, firstPolicy };

            // Act
            AscendingOrderOfPoliciesStartDate ascendingOrderOfPolicies = new AscendingOrderOfPoliciesStartDate();
            policies = ascendingOrderOfPolicies.Sort(testPolicies);

            // Assert
            Assert.IsTrue(policies.SequenceEqual(expectedPolicies));
        }
    }
}