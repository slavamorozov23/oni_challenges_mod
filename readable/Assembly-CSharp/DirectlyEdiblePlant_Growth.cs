using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000851 RID: 2129
public class DirectlyEdiblePlant_Growth : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x06003A81 RID: 14977 RVA: 0x00146E70 File Offset: 0x00145070
	public bool CanPlantBeEaten()
	{
		float num = 0.25f;
		float num2 = 0f;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(base.gameObject);
		if (amountInstance != null)
		{
			num2 = amountInstance.value / amountInstance.GetMax();
		}
		return num2 >= num;
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x00146EBC File Offset: 0x001450BC
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(this.growing.gameObject);
		float growthUnitToMaturityRatio = this.GetGrowthUnitToMaturityRatio(amountInstance.GetMax(), base.GetComponent<KPrefabID>());
		float b = amountInstance.value * growthUnitToMaturityRatio;
		float num = Mathf.Min(desiredUnitsToConsume, b);
		this.growing.ConsumeGrowthUnits(num, growthUnitToMaturityRatio);
		return num;
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x00146F24 File Offset: 0x00145124
	public float PlantProductGrowthPerCycle()
	{
		Crop crop = base.GetComponent<Crop>();
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x06003A84 RID: 14980 RVA: 0x00146F6C File Offset: 0x0014516C
	private float GetGrowthUnitToMaturityRatio(float maturityMax, KPrefabID prefab_id)
	{
		ResourceSet<Trait> traits = Db.Get().traits;
		Tag prefabTag = prefab_id.PrefabTag;
		Trait trait = traits.Get(prefabTag.ToString() + "Original");
		if (trait != null)
		{
			AttributeModifier attributeModifier = trait.SelfModifiers.Find((AttributeModifier match) => match.AttributeId == "MaturityMax");
			if (attributeModifier != null)
			{
				return attributeModifier.Value / maturityMax;
			}
		}
		return 1f;
	}

	// Token: 0x06003A85 RID: 14981 RVA: 0x00146FE8 File Offset: 0x001451E8
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x0014700B File Offset: 0x0014520B
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0014700E File Offset: 0x0014520E
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04002391 RID: 9105
	[MyCmpGet]
	private Growing growing;
}
