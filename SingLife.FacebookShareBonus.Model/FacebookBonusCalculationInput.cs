namespace SingLife.FacebookShareBonus.Model
{
    // <summary>
    // parameter class of the <see cref="FacebookBonusCalculator.Calculate(FacebookBonusCalculationInput)"/> method..
    // </summary>
    public class FacebookBonusCalculationInput
    {
        // <summary>
        // A collection of polices owned by the customer.
        // </summary>
        public Policy[] PoliciesOfCustomer { get; set; }

        // <summary>
        // FaceBook bonus Calculation Settings.
        // </summary>
        public FacebookBonusSettings Setting { get; set; }
    }
}