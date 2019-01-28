using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Test
{
    internal class DescendingOrderOfPoliciesNumberTests
    {
        [Test]
        public void SortPoliciesBaseOnPolicyNumberInDescendingOrder_FirstPolicyNumberEqualsP001AndSecondPolicyNumberEqualsP002_IsInDescendingOrder()
        {
            // Arrange
            Policy firstPolicy = new Policy();
            firstPolicy.PolicyNumber = "P001";
            Policy secondPolicy = new Policy();
            secondPolicy.PolicyNumber = "P002";
            Policy thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100 };
            IEnumerable<Policy> policies = new Policy[2] { firstPolicy, secondPolicy };
            IEnumerable<Policy> expectedPolicies = new Policy[2] { secondPolicy, firstPolicy };

            // Act
            DescendingOrderOfPoliciesNumber policySorterConditions = new DescendingOrderOfPoliciesNumber();
            policies = policySorterConditions.Sort(policies);

            // Assert
            Assert.IsTrue(policies.SequenceEqual(expectedPolicies));
        }
    }
}