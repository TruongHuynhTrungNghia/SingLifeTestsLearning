using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    public class PolicyTests
    {
        [Test]
        public void CheckPolicyNumberTypeIsSting()
        {
            //Arrange
            Policy policy = new Policy();
            const string expectedType = "string";

            //Act
            var getPolicyNumberType = policy.PolicyNumber.GetType();

            //Assert
            var actualPolicyNumberType = getPolicyNumberType.ToString();
            Assert.That(actualPolicyNumberType, Is.EqualTo(expectedType));
        }

        [Test]
        public void CheckStartDateIsNotTomorrow()
        {
            //Arrange
            Policy policy = new Policy();
            DateTime date = DateTime.Today;

            //Act
            var getStartDate = policy.StartDate.ge
        }
    }
}