namespace Common.Scripts
{
    public class FoodWasteImpactCalculator
    {
        // Conversion factor for food waste to CO2 emissions (kg to kg)
        private const double FoodWasteToCO2Factor = 2.3;

        // Conversion factor for CO2 emissions to kilometers driven (kg CO2 to km)
        private const double CO2ToKilometersFactor = 0.120;
        
        private const int FoodWasteToWaterFactor = 2500;

        // Calculates the amount of CO2 released from a given amount of food waste (in kg)
        public static double CalculateCO2EmissionsFromFoodWaste(int kgOfFoodWaste)
        {
            return kgOfFoodWaste * FoodWasteToCO2Factor;
        }

        // Converts CO2 emissions to the equivalent amount of kilometers driven by a car
        public static double ConvertCO2EmissionsToKilometersDriven(double kgOfCO2)
        {
            return kgOfCO2 / CO2ToKilometersFactor;
        }
        
        public static string GetCO2FunFactString(int amountDestroyed)
        {
            double co2 = CalculateCO2EmissionsFromFoodWaste(amountDestroyed);
            double kmDriven = ConvertCO2EmissionsToKilometersDriven(co2);

            return
                $"that's the equivalent of releasing {co2.ToString("F0")}kg of CO2 into the atmosphere, the same as driving a car for {kmDriven.ToString("F0")} kilometers.";
        }
        
        public static string GetWaterWasteFunFactString(int kgOfFoodWaste)
        {
            int waterWasted = kgOfFoodWaste * FoodWasteToWaterFactor;
            return $"By wasting {kgOfFoodWaste} kg of food, you've also wasted approximately {waterWasted.ToString("F0")} liters of water, " +
                   $"enough to fill over {(waterWasted / 1000).ToString("F0")} standard-sized bathtubs!";
        }
        
        public static string GetLandfillContributionFunFactString(int kgOfFoodWaste)
        {
            double landfillContribution = kgOfFoodWaste * 1.5;
            return $"You've thrown away {kgOfFoodWaste} kg of food. That's like adding {landfillContribution.ToString("F0")} kg to our already overflowing trash mountains due to additional packaging and non-compostable waste.";
        }
    }
}