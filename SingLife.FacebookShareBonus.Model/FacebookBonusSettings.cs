namespace SingLife.FacebookShareBonus.Model
{
    public class FacebookBonusSettings
    {
        public float BonusPercentage { get; set; }

        public decimal MaximumBonus { get; set; }

        public IPolicySortService PolicySorter { get; set; }
    }
}