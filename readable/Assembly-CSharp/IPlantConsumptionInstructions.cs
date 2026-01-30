using System;

// Token: 0x020005B6 RID: 1462
public interface IPlantConsumptionInstructions
{
	// Token: 0x06002186 RID: 8582
	CellOffset[] GetAllowedOffsets();

	// Token: 0x06002187 RID: 8583
	float ConsumePlant(float desiredUnitsToConsume);

	// Token: 0x06002188 RID: 8584
	float PlantProductGrowthPerCycle();

	// Token: 0x06002189 RID: 8585
	bool CanPlantBeEaten();

	// Token: 0x0600218A RID: 8586
	string GetFormattedConsumptionPerCycle(float consumer_caloriesLossPerCaloriesPerKG);

	// Token: 0x0600218B RID: 8587
	Diet.Info.FoodType GetDietFoodType();
}
