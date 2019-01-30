using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var secondPolicy = new Policy() { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 05, 06) };
            var firstPolicy = new Policy() { PolicyNumber = "P002", Premium = 100, StartDate = new DateTime(2017, 08, 11) };
            var thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100, StartDate = new DateTime(2017, 09, 12) };

            var settings = new FacebookBonusSettings()
            {
                BonusPercentage = 3,
                MaximumBonus = 10,
                PolicySorter = new AscendingOrderOfPoliciesStartDate()
            };

            var calculationInput = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[] { firstPolicy, secondPolicy, thirdPolicy },
                Settings = settings
            };

            var facebookBonusCalculator = new FacebookBonusCalculator();

            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(calculationInput);
            PolicyBonus policyBonus = facebookBonus.PolicyBonuses[0];
            Console.WriteLine(policyBonus.PolicyNumber+" "+policyBonus.BonusInPoints);
            var expectedPolicyBonus = new PolicyBonus() { PolicyNumber = "P001", BonusInPoints = 6 };

            Console.WriteLine("\n" + facebookBonus.PolicyBonuses.Contains(expectedPolicyBonus));

            Console.ReadLine();
        }

    }
}