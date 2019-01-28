using System;
using System.Collections.Generic;
using System.Linq;
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
            int countPolicies = 0;
            FacebookBonus facebookBonus = new FacebookBonus();
            IEnumerable<Policy> policiesOfCustomer = input.PoliciesOfCustomer;
            var bonusPercentage = input.Setting.BonusPercentage;
            var maxiumBonus = input.Setting.MaximumBonus;
            IPolicySortService policySortService = new DescendingOrderOfPoliciesNumber();
            if (input.Setting.policySorter != null)
            {
                policySortService = input.Setting.policySorter;
            }
            policiesOfCustomer = policySortService.Sort(policiesOfCustomer);
            int numberOfPolicyBonus = policiesOfCustomer.Count();
            facebookBonus.PolicyBonuses = new PolicyBonus[numberOfPolicyBonus];
            foreach (var policy in policiesOfCustomer)
            {
                PolicyBonus tempPolicyBonus = new PolicyBonus();
                int temp = CalculatePoints(policy.Premium, bonusPercentage);
                if (maxiumBonus <= facebookBonus.Total + temp && maxiumBonus > 0)
                {
                    temp = Convert.ToInt32(maxiumBonus) - facebookBonus.Total;
                }
                tempPolicyBonus.BonusInPoints = temp;
                tempPolicyBonus.PolicyNumber = policy.PolicyNumber;
                facebookBonus.PolicyBonuses[countPolicies] = tempPolicyBonus;
                countPolicies++;
            }
            return facebookBonus;
        }

        internal bool IsTotalBiggerThanMaxiumBonus(int total, decimal maximumBonus)
        {
            if (total > maximumBonus)
                return true;
            return false;
        }

        internal int CalculatePoints(decimal premium, float bonusPercentage)
        {
            return ConverseFloatToInt(RoundDown(decimal.ToSingle(premium) * bonusPercentage / 100));
        }

        internal int ConverseFloatToInt(float v)
        {
            return (int)v;
        }

        internal int RoundDown(float v)
        {
            return (int)Math.Floor(v);
        }

        public IEnumerable<Policy> SortsPoliciesStartDate(Policy[] policies)
        {
            IEnumerable<Policy> sortedPolicies = new Policy[policies.Length];
            FacebookBonusSettings facebook = new FacebookBonusSettings();
            facebook.policySorter = new AscendingOrderOfPoliciesStartDate();
            sortedPolicies = facebook.policySorter.Sort(policies);
            return sortedPolicies;
        }
    }
}