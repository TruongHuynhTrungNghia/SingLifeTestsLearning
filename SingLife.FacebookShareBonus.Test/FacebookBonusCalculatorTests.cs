using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;

namespace SingLife.FacebookShareBonus.Test
{
    [TestFixture]
    internal class FacebookBonusCalculatorTests
    {
        [Test]
        public void Calculate_PremimumEquals200AndPercentageEquals2_PointEquals4()
        {
            // Arrange
            Policy firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 100 };
            FacebookBonusSettings settings = new FacebookBonusSettings() { BonusPercentage = 3, policySorter = new FakeSortOrder() };
            FacebookBonusCalculationInput input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[1] { firstPolicy },
                Setting = settings
            };
            const int expectedPoints = 3;

            // Act
            var calculator = new FacebookBonusCalculator();
            var facebookBonus = calculator.Calculate(input);

            // Assert
            int actualBonusPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Assert.That(actualBonusPoint, Is.EqualTo(expectedPoints));
        }

        [Test]
        public void Calculate_PonitsEquals3Point99_PointRoundDown3()
        {
            // Arrange
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            Policy firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 133 };
            FacebookBonusSettings settings = new FacebookBonusSettings() { BonusPercentage = 3, policySorter = new FakeSortOrder() };
            FacebookBonusCalculationInput input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[1] { firstPolicy },
                Setting = settings
            };
            const int expectedBonusPoint = 3;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            var actualBonusPoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Assert.That(actualBonusPoint, Is.EqualTo(expectedBonusPoint));
        }

        [Test]
        public void Calculate_FirstPoilicyPremiumEquals100AndSecondPoilicyPremiumEquals150_PointsCalculateForEachPolicy()
        {
            // Arrange
            Policy firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 100 };
            Policy secondPolicy = new Policy() { PolicyNumber = "P002", Premium = 150 };
            FacebookBonusSettings settings = new FacebookBonusSettings() { BonusPercentage = 3, policySorter = new FakeSortOrder() };
            FacebookBonusCalculationInput input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[2] { firstPolicy, secondPolicy },
                Setting = settings
            };
            var facebookBonusCalculator = new FacebookBonusCalculator();
            const int expectedFirstPolicyPoint = 3;
            const int expectedSecondPolicyPoint = 4;

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            int actualFirstPolicypoint = facebookBonus.PolicyBonuses[0].BonusInPoints;
            int actualSecondPolicypoint = facebookBonus.PolicyBonuses[1].BonusInPoints;
            Assert.That(actualFirstPolicypoint, Is.EqualTo(expectedFirstPolicyPoint));
            Assert.That(actualSecondPolicypoint, Is.EqualTo(expectedSecondPolicyPoint));
        }

        [Test]
        public void Calculate_FirstPolicyNumberEqualsP001SecondPolicyNumberEqualsP002_DescentPolicyNumber()
        {
            // Arrange
            Policy firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 500 };
            Policy secondPolicy = new Policy() { PolicyNumber = "P002", Premium = 300 };

            var settings = new FacebookBonusSettings()
            {
                BonusPercentage = 3,
                MaximumBonus = 10,
                policySorter = new DescendingOrderOfPoliciesNumber()
            };

            var input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[2] { firstPolicy, secondPolicy },
                Setting = settings
            };

            var facebookBonusCalculator = new FacebookBonusCalculator();
            const string expectedFirstPolicyNumber = "P002";
            const string expectedSecondPolicyNumber = "P001";

            // Act
            var facebookBonus = facebookBonusCalculator.Calculate(input);

            // Assert
            string actualFirstPolicyNumber = facebookBonus.PolicyBonuses[0].PolicyNumber;
            string actualSecondPolicyNumber = facebookBonus.PolicyBonuses[1].PolicyNumber;
            Assert.That(actualFirstPolicyNumber, Is.EqualTo(expectedFirstPolicyNumber));
            Assert.That(actualSecondPolicyNumber, Is.EqualTo(expectedSecondPolicyNumber));
        }

        [Test]
        public void Calculate_MaxiumPointEquals10_TotalPointIsSmallerOrEqualMaxiumPoint()
        {
            // Arrange
            const int maxiumBonus = 10;
            Policy secondPolicy = new Policy() { PolicyNumber = "P001", Premium = 500, StartDate = new System.DateTime(12 / 05 / 2016) };
            Policy firstPolicy = new Policy() { PolicyNumber = "P002", Premium = 300, StartDate = new System.DateTime(08 / 11 / 2017) };
            Policy thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100, StartDate = new System.DateTime(01 / 12 / 2017) };

            FacebookBonusSettings settings = new FacebookBonusSettings()
            {
                BonusPercentage = 3,
                MaximumBonus = maxiumBonus,
                policySorter = new AscendingOrderOfPoliciesStartDate()
            };

            FacebookBonusCalculationInput input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[3] { firstPolicy, secondPolicy, thirdPolicy },
                Setting = settings
            };
            var calculator = new FacebookBonusCalculator();

            // Act
            FacebookBonus facebookBonus = calculator.Calculate(input);

            // Assert
            int totalPoint = facebookBonus.Total;
            Assert.That(totalPoint, Is.LessThanOrEqualTo(maxiumBonus));
        }
    }
}