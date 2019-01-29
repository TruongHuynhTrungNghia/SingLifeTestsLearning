﻿using System;
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

        private FacebookBonus CalculateFacebookBonus(Policy[] Policies, FacebookBonusSettings Settings)
        {
            var resultFacebookBonus = new FacebookBonus();
            int inputPoliciesLength = Policies.Length;
            resultFacebookBonus.PolicyBonuses = new PolicyBonus[inputPoliciesLength];
            resultFacebookBonus.PolicyBonuses = ImplementPolicyBonus(Policies, Settings);

            return resultFacebookBonus;
        }

        private PolicyBonus[] ImplementPolicyBonus(Policy[] Policies, FacebookBonusSettings Settings)
        {
            var resultPolicyBonus = new PolicyBonus[Policies.Length];
            int count = 0;
            int total = 0;
            int temp = 0;
            var sortedPolicies = SortPoliciesWithCondition(Settings, Policies);

            foreach (Policy policy in sortedPolicies)
            {
                temp = CalculatePoints(policy.Premium, Settings.BonusPercentage);
                var elementPolicyBonus = new PolicyBonus()
                {
                    PolicyNumber = policy.PolicyNumber,
                    BonusInPoints = temp
                };

                if (IsTotalBiggerThanMaxiumPointAndIsMaxiumBiggerThan0(total + temp, Settings.MaximumBonus))
                {
                    temp = Convert.ToInt32(Settings.MaximumBonus) - total;
                    total = Convert.ToInt32(Settings.MaximumBonus);
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

        private bool IsTotalBiggerThanMaxiumPointAndIsMaxiumBiggerThan0(int total, decimal maximumBonus)
        {
            if (total >= maximumBonus && maximumBonus > 0)
                return true;
            return false;
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