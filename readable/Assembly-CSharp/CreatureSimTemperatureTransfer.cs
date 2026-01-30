using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000890 RID: 2192
public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
	// Token: 0x06003C53 RID: 15443 RVA: 0x00151870 File Offset: 0x0014FA70
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f, 20, true);
		this.averageTemperatureTransferPerSecond = new AttributeModifier(this.temperatureAttributeName + "Delta", 0f, DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, false, true, false);
		this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
		base.OnPrefabInit();
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x001518E8 File Offset: 0x0014FAE8
	protected override void OnSpawn()
	{
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier);
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.skinThickness, this.skinThicknessAttributeModifierName, false, false, true);
		attributeInstance.Add(modifier);
		base.OnSpawn();
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06003C55 RID: 15445 RVA: 0x00151949 File Offset: 0x0014FB49
	public bool LastTemperatureRecordIsReliable
	{
		get
		{
			return Time.time - this.lastTemperatureRecordTime < 2f && this.average_kilowatts_exchanged.ValidRecordsInLastSeconds(4f) > 5;
		}
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x00151974 File Offset: 0x0014FB74
	protected unsafe void unsafeUpdateAverageKiloWattsExchanged(float dt)
	{
		if (Time.time < this.lastTemperatureRecordTime + 0.2f)
		{
			return;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(this.simHandle);
			if (Game.Instance.simData.elementChunks[handleIndex].deltaKJ == 0f)
			{
				return;
			}
			this.average_kilowatts_exchanged.AddSample(Game.Instance.simData.elementChunks[handleIndex].deltaKJ, Time.time);
			this.lastTemperatureRecordTime = Time.time;
		}
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x00151A0D File Offset: 0x0014FC0D
	private void Update()
	{
		this.unsafeUpdateAverageKiloWattsExchanged(Time.deltaTime);
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x00151A1C File Offset: 0x0014FC1C
	public void Sim200ms(float dt)
	{
		this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetUnweightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
		float num = 0f;
		foreach (AttributeModifier attributeModifier in this.NonSimTemperatureModifiers)
		{
			num += attributeModifier.Value;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			float num2 = num * (this.primaryElement.Mass * 1000f) * this.primaryElement.Element.specificHeatCapacity * 0.001f;
			float delta_kj = num2 * dt;
			SimMessages.ModifyElementChunkEnergy(this.simHandle, delta_kj);
			this.heatEffect.SetHeatBeingProducedValue(num2);
			return;
		}
		this.heatEffect.SetHeatBeingProducedValue(0f);
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00151B14 File Offset: 0x0014FD14
	public void RefreshRegistration()
	{
		base.SimUnregister();
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get(Db.Get().Attributes.ThermalConductivityBarrier);
		this.thickness = attributeInstance.GetTotalValue();
		this.simHandle = -1;
		base.SimRegister();
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00151B60 File Offset: 0x0014FD60
	public static float PotentialEnergyFlowToCreature(int cell, PrimaryElement transfererPrimaryElement, SimTemperatureTransfer temperatureTransferer, float deltaTime = 1f)
	{
		return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
	}

	// Token: 0x04002533 RID: 9523
	public string temperatureAttributeName = "Temperature";

	// Token: 0x04002534 RID: 9524
	public float skinThickness = DUPLICANTSTATS.STANDARD.Temperature.SKIN_THICKNESS;

	// Token: 0x04002535 RID: 9525
	public string skinThicknessAttributeModifierName = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04002536 RID: 9526
	public AttributeModifier averageTemperatureTransferPerSecond;

	// Token: 0x04002537 RID: 9527
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04002538 RID: 9528
	private PrimaryElement primaryElement;

	// Token: 0x04002539 RID: 9529
	public RunningWeightedAverage average_kilowatts_exchanged;

	// Token: 0x0400253A RID: 9530
	public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();

	// Token: 0x0400253B RID: 9531
	private float lastTemperatureRecordTime;
}
