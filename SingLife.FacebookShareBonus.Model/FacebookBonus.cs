namespace SingLife.FacebookShareBonus.Model
{
    //  <Summary>
    //  Resents the bonus rewarded to a customer for sharing to Facebook
    //  </summary>>
    public class FacebookBonus
    {
        public PolicyBonus[] PolicyBonuses { get; set; }

        public int Total
        {
            get
            {
                int totalBonusInPoints = 0;
                foreach (PolicyBonus element in PolicyBonuses)
                {
                    if (element == null)
                    {
                        return totalBonusInPoints;
                    }
                    else
                        totalBonusInPoints += element.BonusInPoints;
                }
                return totalBonusInPoints;
            }
        }
    }
}