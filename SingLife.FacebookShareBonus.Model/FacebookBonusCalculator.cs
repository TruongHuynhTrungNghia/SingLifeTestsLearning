using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SingLife.FacebookShareBonus.Test")]

namespace SingLife.FacebookShareBonus.Model
{
    ///<summary>
    /// A domain service that calculates the bonus rewarded to a customer who has shared to FaceBook.
    ///</summary>
    public class FacebookBonusCalculator
    {
        // <summary>
        // Calculates the bonus
        // </summary>
        // <param name = "input">A parameter object representing inputs to the calculation.</param>
        // <returns> A <see cref="FacebookBonus"/> object.</returns>
        public FacebookBonus Calculate(FacebookBonusCalculationInput input)
        {
            var inputPolicies = input.PoliciesOfCustomer;
            var inputSettings = input.Setting;

            var facebookBonus = new FacebookBonus();
            facebookBonus = CalculateFacebookBonus(inputPolicies, inputSettings);

            return facebookBonus;
        }

        private FacebookBonus CalculateFacebookBonus(Policy[] inputPolicies, FacebookBonusSettings inputSettings)
        {
            var resultFacebookBonus = new FacebookBonus();
            int inputPoliciesLength = inputPolicies.Length;
            resultFacebookBonus.PolicyBonuses = new PolicyBonus[inputPoliciesLength];
            resultFacebookBonus.PolicyBonuses = SetUpPolicyBonus(inputPolicies, inputSettings);

            return resultFacebookBonus;
        }

        private PolicyBonus[] SetUpPolicyBonus(Policy[] inputPolicies, FacebookBonusSettings inputSettings)
        {
            var resultPolicyBonus = new PolicyBonus[inputPolicies.Length];
            int count = 0;
            int total = 0;
            int temp = 0;
            var sortedPolicies = SortPoliciesWithCondition(inputSettings, inputPolicies);

            foreach (Policy policy in sortedPolicies)
            {
                temp = CalculatePoints(policy.Premium, inputSettings.BonusPercentage);

                var elementPolicyBonus = new PolicyBonus()
                {
                    PolicyNumber = policy.PolicyNumber,
                    BonusInPoints = temp
                };

                if (IsTotalBiggerThanMaxiumPointAndMaxiumBiggerThan0(total + temp, inputSettings.MaximumBonus))
                {
                    temp = Convert.ToInt32(inputSettings.MaximumBonus) - total;
                    total = Convert.ToInt32(inputSettings.MaximumBonus);
                    elementPolicyBonus.BonusInPoints = temp;
                }
                else
                {
                    total += temp;
                }
                resultPolicyBonus[count] = elementPolicyBonus;
                count++;
            }

            return resultPolicyBonus;
        }

        private bool IsTotalBiggerThanMaxiumPointAndMaxiumBiggerThan0(int total, decimal maximumBonus)
        {
            if (total >= maximumBonus && maximumBonus > 0)
                return true;
            return false;
        }

        private static IEnumerable<Policy> SortPoliciesWithCondition(FacebookBonusSettings settings, IEnumerable<Policy> policiesOfCustomer)
        {
            IPolicySortService policySortService = new DescendingOrderOfPoliciesNumber();
            if (settings.policySorter != null)
            {
                policySortService = settings.policySorter;
            }
            policiesOfCustomer = policySortService.Sort(policiesOfCustomer);
            return policiesOfCustomer;
        }

        private int CalculatePoints(decimal premium, float bonusPercentage)
        {
            return ConverseFloatToInt(RoundDown(decimal.ToSingle(premium) * bonusPercentage / 100));
        }

        private int ConverseFloatToInt(float v)
        {
            return (int)v;
        }

        private int RoundDown(float v)
        {
            return (int)Math.Floor(v);
        }
    }
}