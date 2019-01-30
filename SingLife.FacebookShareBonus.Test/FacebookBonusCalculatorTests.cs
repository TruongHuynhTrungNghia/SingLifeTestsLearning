using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    internal class FacebookBonusCalculatorTests
    {
        [Test]
        public void Calculate_GivenPremiumAndPercentage_IncentivePointsArePercentageOfPolicyPremium()
        {
            // Arrange
            var input = CreateCalculationInputWithSinglePolicy(premium: 200, percentage: 3);
            var facebookBonusCalculator = new FacebookBonusCalculator();
            const int expectedIncentivePoint = 6;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            int actualBonusPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Assert.That(actualBonusPoint, Is.EqualTo(expectedIncentivePoint));
        }

        [Test]
        public void Calculate_GivenIncentivePoints_IncentivePointsAreRoundedDownToTheNearestInteger()
        {
            // Arrange
            var input = CreateCalculationInputWithSinglePolicy(premium: 133, percentage: 3);
            var facebookBonusCalculator = new FacebookBonusCalculator();
            const int expectedBonusPoint = 3;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            var actualBonusPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Assert.That(actualBonusPoint, Is.EqualTo(expectedBonusPoint));
        }

        [Test]
        public void Calculate_GivenCustomerPolicies_InctivePointsAreCalculatedForEachPolicy()
        {
            // Arrange
            var input = CreateCalculationInputWith3Policies();
            var facebookBonusCalculator = new FacebookBonusCalculator();
            int expectedFirstPolicyBonus = 6;
            int expectedSecondPolicyBonus = 3;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            int actualFirstPolicyPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            int actualSecondPolicyPoint = facebookBonus.PolicyBonuses[1].BonusInPoints;
            Assert.That(actualFirstPolicyPoint, Is.EqualTo(expectedFirstPolicyBonus));
            Assert.That(actualSecondPolicyPoint, Is.EqualTo(expectedSecondPolicyBonus));
        }

        [Test]
        public void Calculate_DescendingSortByPolicyNumbersIsSpecifiedInSettings_PoliciesAreSortedByDecendingOrderOfPolicyNumbers()
        {
            // Arrange
            var input = CreateCalculationInputWith3PoliciesAndDescendingPoliciesSpecifiedInSettings();
            var facebookBonusCalculator = new FacebookBonusCalculator();

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            Assert.That(facebookBonus.PolicyBonuses, Is.Ordered.Descending.By(nameof(PolicyBonus.PolicyNumber)));
        }

        [Test]
        public void Calculate_MaxiumIncentivePointInSettings_TotalPointIsSmallerOrEqualToMaxiumPoint()
        {
            // Arrange
            var input = CreateCalculationInputWith3Policies();
            var facebookBonusCalculator = new FacebookBonusCalculator();
            const int maxiumBonus = 10;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            int totalPoint = facebookBonus.Total;
            Assert.That(totalPoint, Is.LessThanOrEqualTo(maxiumBonus));
        }

        [Test]
        public void Calculate_MaxiumIncentivePointInSettings_BonusPointsAreDistributedCorrectly()
        {
            // Arrange
            var input = CreateCalculationInputWith3Policies();
            var facebookBonusCalculator = new FacebookBonusCalculator();
            const int expecctedFirstBonusPoints = 6;
            const int expecctedSecondBonusPoints = 3;
            const int expecctedThirdBonusPoints = 1;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            var actualFirstBonusPoints = facebookBonus.PolicyBonuses[0].BonusInPoints;
            var actualSecondBonusPoints = facebookBonus.PolicyBonuses[1].BonusInPoints;
            var actualThirdBonusPoints = facebookBonus.PolicyBonuses[2].BonusInPoints;

            Assert.That(actualFirstBonusPoints, Is.EqualTo(expecctedFirstBonusPoints));
            Assert.That(actualSecondBonusPoints, Is.EqualTo(expecctedSecondBonusPoints));
            Assert.That(actualThirdBonusPoints, Is.EqualTo(expecctedThirdBonusPoints));
        }

        private FacebookBonusCalculationInput CreateCalculationInputWithSinglePolicy(decimal premium, float percentage)
        {
            var policy = new Policy() { PolicyNumber = "P001", Premium = premium, StartDate = new DateTime(2016, 05, 06) };
            var settings = new FacebookBonusSettings()
            {
                BonusPercentage = percentage,
            };

            var calculationInput = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[] { policy },
                Setting = settings
            };

            return calculationInput;
        }

        private FacebookBonusCalculationInput CreateCalculationInputWith3Policies()
        {
            var firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 05, 06) };
            var secondPolicy = new Policy() { PolicyNumber = "P002", Premium = 100, StartDate = new DateTime(2017, 08, 11) };
            var thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100, StartDate = new DateTime(2017, 09, 12) };

            var settings = new FacebookBonusSettings()
            {
                BonusPercentage = 3,
                MaximumBonus = 10,
                policySorter = new FakeSortOrder()
            };

            var calculationInput = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[] { firstPolicy, secondPolicy, thirdPolicy },
                Setting = settings
            };

            return calculationInput;
        }

        private FacebookBonusCalculationInput CreateCalculationInputWith3PoliciesAndDescendingPoliciesSpecifiedInSettings()
        {
            var firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 05, 06) };
            var secondPolicy = new Policy() { PolicyNumber = "P002", Premium = 100, StartDate = new DateTime(2017, 08, 11) };
            var thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100, StartDate = new DateTime(2017, 09, 12) };

            var settings = new FacebookBonusSettings()
            {
                BonusPercentage = 3,
                MaximumBonus = 10,
                policySorter = new DescendingOrderOfPoliciesNumber()
            };

            var calculationInput = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[] { firstPolicy, secondPolicy, thirdPolicy },
                Setting = settings
            };

            return calculationInput;
        }
    }
}