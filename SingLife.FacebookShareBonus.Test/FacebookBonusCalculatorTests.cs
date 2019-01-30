using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System;
using System.Linq;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    internal class FacebookBonusCalculatorTests
    {
        private FacebookBonusCalculationInput calculationInput;
        private FacebookBonusCalculator facebookBonusCalculator;

        [SetUp]
        public void SetUp()
        {
            var secondPolicy = new Policy() { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 05, 06) };
            var firstPolicy = new Policy() { PolicyNumber = "P002", Premium = 100, StartDate = new DateTime(2017, 08, 11) };
            var thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100, StartDate = new DateTime(2017, 09, 12) };

            var settings = new FacebookBonusSettings()
            {
                BonusPercentage = 3,
                MaximumBonus = 10,
                policySorter = new AscendingOrderOfPoliciesStartDate()
            };

            calculationInput = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[] { firstPolicy, secondPolicy, thirdPolicy },
                Setting = settings
            };

            facebookBonusCalculator = new FacebookBonusCalculator();
        }

        [Test]
        public void Calculate_GivenPremiumAndPercentage_IncentivePointsArePercentageOfPolicyPremium()
        {
            // Arrange
            const int expectedIncentivePoint = 6;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(calculationInput);

            // Assert
            int actualBonusPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Assert.That(actualBonusPoint, Is.EqualTo(expectedIncentivePoint));
        }

        [Test]
        public void Calculate_GivenIncentivePoints_IncentivePointsAreRoundedDownToTheNearestInteger()
        {
            // Arrange
            const int expectedBonusPoint = 3;
            calculationInput.PoliciesOfCustomer[0] = new Policy() { Premium = 133 };

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(calculationInput);

            // Assert
            var actualBonusPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Assert.That(actualBonusPoint, Is.EqualTo(expectedBonusPoint));
        }

        [Test]
        public void Calculate_GivenCustomerPolicies_IncePointsAreCalculatedForEachPolicy()
        {
            // Arrange
            var expectedPolicyBonus = new PolicyBonus() { PolicyNumber = "P001", BonusInPoints = 6 };

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(calculationInput);

            // Assert
            //int actualFirstPolicyPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            //int actualSecondPolicyPoint = facebookBonus.PolicyBonuses[1].BonusInPoints;
            //Assert.That(actualFirstPolicyPoint, Is.EqualTo(expectedFirstPolicyPoint));
            //Assert.That(actualSecondPolicyPoint, Is.EqualTo(expectedSecondPolicyPoint));
            // Assert.That(facebookBonus.PolicyBonuses.Any(p => p == expectedPolicyBonus));
            //Assert.That(facebookBonus.PolicyBonuses, Is.Co)
            //Assert.Contains(expectedPolicyBonus, facebookBonus.PolicyBonuses);
            Assert.That(facebookBonus.PolicyBonuses.Any(p => p.PolicyNumber == expectedPolicyBonus.PolicyNumber && p.BonusInPoints == expectedPolicyBonus.BonusInPoints));
        }

        [Test]
        public void Calculate_DescendingSortByPolicyNumbersIsSpecifiedInSettings_PoliciesAreSortedByDecendingOrderOfPolicyNumbers()
        {
            var input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[]
                {
                    new Policy { PolicyNumber = "P001", Premium = 500 },
                    new Policy { PolicyNumber = "P002", Premium = 300 }
                },
                Setting = new FacebookBonusSettings
                {
                    policySorter = new DescendingOrderOfPoliciesNumber(),
                    BonusPercentage = 3,
                    MaximumBonus = 10
                }
            };

            var facebookBonusCalculator = new FacebookBonusCalculator();

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            Assert.That(facebookBonus.PolicyBonuses, Is.Ordered.Descending.By(nameof(PolicyBonus.PolicyNumber)));
        }

        [Test]
        public void Calculate_MaxiumIncentivePointInSettings_TotalPointIsSmallerOrEqualMaxiumPoint()
        {
            // Arrange
            const int maxiumBonus = 10;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(calculationInput);

            // Assert
            int totalPoint = facebookBonus.Total;
            Assert.That(totalPoint, Is.LessThanOrEqualTo(maxiumBonus));
        }
    }
}