using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    internal class FacebookBonusTests
    {
        [Test]
        public void CalculateTotal_FirstPointEquals6AndSecondPointEquals4_TotalEquals10()
        {
            // Arrange
            FacebookBonus facebookBonus = new FacebookBonus();
            PolicyBonus firstPoilicyBonus = new PolicyBonus() { PolicyNumber = "P001", BonusInPoints = 6 };
            PolicyBonus secondPoilicyBonus = new PolicyBonus() { PolicyNumber = "P002", BonusInPoints = 4 };
            facebookBonus.PolicyBonuses = new PolicyBonus[] { firstPoilicyBonus, secondPoilicyBonus };
            int expectedTotal = 10;

            // Act
            int actualTotal = facebookBonus.Total;

            // Assert
            Assert.That(actualTotal, Is.EqualTo(expectedTotal));
        }
    }
}