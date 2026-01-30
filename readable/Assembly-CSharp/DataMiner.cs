using System;
using KSerialization;
using UnityEngine;

// Token: 0x020008C8 RID: 2248
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class DataMiner : ComplexFabricator
{
	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06003E4F RID: 15951 RVA: 0x0015C567 File Offset: 0x0015A767
	public float OperatingTemp
	{
		get
		{
			return this.pe.Temperature;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06003E50 RID: 15952 RVA: 0x0015C574 File Offset: 0x0015A774
	public float TemperatureScaleFactor
	{
		get
		{
			return 1f - DataMinerConfig.TEMPERATURE_SCALING_RANGE.LerpFactorClamped(this.OperatingTemp);
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06003E51 RID: 15953 RVA: 0x0015C58C File Offset: 0x0015A78C
	public float EfficiencyRate
	{
		get
		{
			return DataMinerConfig.PRODUCTION_RATE_SCALE.Lerp(this.TemperatureScaleFactor);
		}
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x0015C5A0 File Offset: 0x0015A7A0
	protected override float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		float efficiencyRate = this.EfficiencyRate;
		this.minEfficiency = Mathf.Min(this.minEfficiency, efficiencyRate);
		return base.ComputeWorkProgress(dt, recipe) * efficiencyRate;
	}

	// Token: 0x06003E53 RID: 15955 RVA: 0x0015C5D0 File Offset: 0x0015A7D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DataMinerEfficiency, this);
	}

	// Token: 0x06003E54 RID: 15956 RVA: 0x0015C608 File Offset: 0x0015A808
	public override void CompleteWorkingOrder()
	{
		if (this.minEfficiency == DataMinerConfig.PRODUCTION_RATE_SCALE.max)
		{
			SaveGame.Instance.ColonyAchievementTracker.efficientlyGatheredData = true;
		}
		this.minEfficiency = DataMinerConfig.PRODUCTION_RATE_SCALE.max;
		base.CompleteWorkingOrder();
	}

	// Token: 0x06003E55 RID: 15957 RVA: 0x0015C642 File Offset: 0x0015A842
	public override void Sim1000ms(float dt)
	{
		base.Sim1000ms(dt);
		this.meter.SetPositionPercent(this.TemperatureScaleFactor);
	}

	// Token: 0x0400266A RID: 9834
	[MyCmpReq]
	private PrimaryElement pe;

	// Token: 0x0400266B RID: 9835
	[Serialize]
	private float minEfficiency = DataMinerConfig.PRODUCTION_RATE_SCALE.max;

	// Token: 0x0400266C RID: 9836
	private MeterController meter;
}
