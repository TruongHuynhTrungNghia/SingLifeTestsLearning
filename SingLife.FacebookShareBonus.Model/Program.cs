using System;

namespace SingLife.FacebookShareBonus.Model
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            //Policy firstPolicy = new Policy();
            //firstPolicy.PolicyNumber = "P001";
            //firstPolicy.Premium = 200m;
            //Policy secondPolicy = new Policy();
            //secondPolicy.PolicyNumber = "P002";
            //secondPolicy.Premium = 300m;
            //float settingPercentage = 3f;
            //decimal Maxium = 10;
            //Policy thirdPolicy = new Policy() { PolicyNumber = "P003", Premium = 100 };

            //FacebookBonusCalculationInput input = SetupFaceBookCalculationInput(
            //        new Policy[] { firstPolicy, secondPolicy, thirdPolicy }, settingPercentage,Maxium);
            Policy firstPolicy = new Policy() { PolicyNumber = "P001", Premium = 500 };
            FacebookBonusSettings settings = new FacebookBonusSettings() { BonusPercentage = 3 };
            FacebookBonusCalculationInput input = new FacebookBonusCalculationInput()
            {
                PoliciesOfCustomer = new Policy[1] { firstPolicy },
                Setting = settings
            };

            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(input);
            int actualFirstPoints = facebookBonus.PolicyBonuses[0].BonusInPoints;
            Console.WriteLine(actualFirstPoints);
        }

        private static FacebookBonusCalculationInput SetupFaceBookCalculationInput(Policy[] policies, float settings = 0f, decimal Maxium = 0)
        {
            FacebookBonusCalculationInput input = new FacebookBonusCalculationInput();
            input.PoliciesOfCustomer = policies;
            input.Setting = new FacebookBonusSettings();
            input.Setting.BonusPercentage = settings;
            input.Setting.MaximumBonus = Maxium;
            return input;
        }
    }
}