using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000853 RID: 2131
public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x06003A93 RID: 14995 RVA: 0x0014715C File Offset: 0x0014535C
	protected override void OnSpawn()
	{
		this.trunk = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x00147175 File Offset: 0x00145375
	public bool CanPlantBeEaten()
	{
		return this.GetMaxBranchMaturity() >= this.MinimumEdibleMaturity;
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x00147188 File Offset: 0x00145388
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		float maxBranchMaturity = this.GetMaxBranchMaturity();
		float num = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
		GameObject mostMatureBranch = this.GetMostMatureBranch();
		if (!mostMatureBranch)
		{
			return 0f;
		}
		Growing component = mostMatureBranch.GetComponent<Growing>();
		if (component)
		{
			Harvestable component2 = mostMatureBranch.GetComponent<Harvestable>();
			if (component2 != null)
			{
				component2.Trigger(2127324410, BoxedBools.True);
			}
			component.ConsumeMass(num);
			return num;
		}
		mostMatureBranch.GetAmounts().Get(Db.Get().Amounts.Maturity.Id).ApplyDelta(-desiredUnitsToConsume);
		base.gameObject.Trigger(-1793167409, null);
		mostMatureBranch.Trigger(-1793167409, null);
		return desiredUnitsToConsume;
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x0014723C File Offset: 0x0014543C
	public float PlantProductGrowthPerCycle()
	{
		Crop component = base.GetComponent<Crop>();
		string cropID = component.cropId;
		if (this.overrideCropID != null)
		{
			cropID = this.overrideCropID;
		}
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == cropID).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x001472A0 File Offset: 0x001454A0
	public float GetMaxBranchMaturity()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_maturity;
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x001472E4 File Offset: 0x001454E4
	private GameObject GetMostMatureBranch()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_branch;
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x00147328 File Offset: 0x00145528
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x0014734B File Offset: 0x0014554B
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x0014734E File Offset: 0x0014554E
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04002399 RID: 9113
	private PlantBranchGrower.Instance trunk;

	// Token: 0x0400239A RID: 9114
	public float MinimumEdibleMaturity = 0.25f;

	// Token: 0x0400239B RID: 9115
	public string overrideCropID;
}
