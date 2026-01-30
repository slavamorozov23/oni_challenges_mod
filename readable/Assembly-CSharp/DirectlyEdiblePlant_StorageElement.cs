using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06003A89 RID: 14985 RVA: 0x00147019 File Offset: 0x00145219
	public float MassGeneratedPerCycle
	{
		get
		{
			return this.rateProducedPerCycle * this.storageCapacity;
		}
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x00147028 File Offset: 0x00145228
	protected override void OnPrefabInit()
	{
		this.storageCapacity = this.storage.capacityKg;
		base.OnPrefabInit();
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x00147044 File Offset: 0x00145244
	public bool CanPlantBeEaten()
	{
		Tag tag = this.GetTagToConsume();
		return this.storage.GetMassAvailable(tag) / this.storage.capacityKg >= this.minimum_mass_percentageRequiredToEat;
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x0014707C File Offset: 0x0014527C
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		if (this.storage.MassStored() <= 0f)
		{
			return 0f;
		}
		Tag tag = this.GetTagToConsume();
		float massAvailable = this.storage.GetMassAvailable(tag);
		float num = Mathf.Min(desiredUnitsToConsume, massAvailable);
		this.storage.ConsumeIgnoringDisease(tag, num);
		return num;
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x001470CB File Offset: 0x001452CB
	public float PlantProductGrowthPerCycle()
	{
		return this.MassGeneratedPerCycle;
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x001470D3 File Offset: 0x001452D3
	private Tag GetTagToConsume()
	{
		if (!(this.tagToConsume != Tag.Invalid))
		{
			return this.storage.items[0].GetComponent<KPrefabID>().PrefabTag;
		}
		return this.tagToConsume;
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00147109 File Offset: 0x00145309
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		return string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE, GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), this.tagToConsume.ProperName());
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00147133 File Offset: 0x00145333
	public CellOffset[] GetAllowedOffsets()
	{
		return this.edibleCellOffsets;
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0014713B File Offset: 0x0014533B
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantStorage;
	}

	// Token: 0x04002392 RID: 9106
	public CellOffset[] edibleCellOffsets;

	// Token: 0x04002393 RID: 9107
	public Tag tagToConsume = Tag.Invalid;

	// Token: 0x04002394 RID: 9108
	public float rateProducedPerCycle;

	// Token: 0x04002395 RID: 9109
	public float storageCapacity;

	// Token: 0x04002396 RID: 9110
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002397 RID: 9111
	[MyCmpGet]
	private KPrefabID prefabID;

	// Token: 0x04002398 RID: 9112
	public float minimum_mass_percentageRequiredToEat = 0.25f;
}
