using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BE5 RID: 3045
public struct StructureTemperaturePayload
{
	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06005B33 RID: 23347 RVA: 0x002101A5 File Offset: 0x0020E3A5
	// (set) Token: 0x06005B34 RID: 23348 RVA: 0x002101AD File Offset: 0x0020E3AD
	public PrimaryElement primaryElement
	{
		get
		{
			return this.primaryElementBacking;
		}
		set
		{
			if (this.primaryElementBacking != value)
			{
				this.primaryElementBacking = value;
				this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
			}
		}
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x002101D8 File Offset: 0x0020E3D8
	public StructureTemperaturePayload(GameObject go)
	{
		this.simHandleCopy = -1;
		this.enabled = true;
		this.bypass = false;
		this.overrideExtents = false;
		this.overriddenExtents = default(Extents);
		this.primaryElementBacking = go.GetComponent<PrimaryElement>();
		this.overheatable = ((this.primaryElementBacking != null) ? this.primaryElementBacking.GetComponent<Overheatable>() : null);
		this.building = go.GetComponent<Building>();
		this.operational = go.GetComponent<Operational>();
		this.heatEffect = go.GetComponent<KBatchedAnimHeatPostProcessingEffect>();
		this.pendingEnergyModifications = 0f;
		this.maxTemperature = 10000f;
		this.energySourcesKW = null;
		this.isActiveStatusItemSet = false;
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06005B36 RID: 23350 RVA: 0x00210284 File Offset: 0x0020E484
	public float TotalEnergyProducedKW
	{
		get
		{
			if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.energySourcesKW.Count; i++)
			{
				num += this.energySourcesKW[i].value;
			}
			return num;
		}
	}

	// Token: 0x06005B37 RID: 23351 RVA: 0x002102DD File Offset: 0x0020E4DD
	public void OverrideExtents(Extents newExtents)
	{
		this.overrideExtents = true;
		this.overriddenExtents = newExtents;
	}

	// Token: 0x06005B38 RID: 23352 RVA: 0x002102ED File Offset: 0x0020E4ED
	public Extents GetExtents()
	{
		if (!this.overrideExtents)
		{
			return this.building.GetExtents();
		}
		return this.overriddenExtents;
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06005B39 RID: 23353 RVA: 0x00210309 File Offset: 0x0020E509
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06005B3A RID: 23354 RVA: 0x00210316 File Offset: 0x0020E516
	public float ExhaustKilowatts
	{
		get
		{
			return this.building.Def.ExhaustKilowattsWhenActive;
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06005B3B RID: 23355 RVA: 0x00210328 File Offset: 0x0020E528
	public float OperatingKilowatts
	{
		get
		{
			if (!(this.operational != null) || !this.operational.IsActive)
			{
				return 0f;
			}
			return this.building.Def.SelfHeatKilowattsWhenActive;
		}
	}

	// Token: 0x04003CCA RID: 15562
	public int simHandleCopy;

	// Token: 0x04003CCB RID: 15563
	public bool enabled;

	// Token: 0x04003CCC RID: 15564
	public bool bypass;

	// Token: 0x04003CCD RID: 15565
	public bool isActiveStatusItemSet;

	// Token: 0x04003CCE RID: 15566
	public bool overrideExtents;

	// Token: 0x04003CCF RID: 15567
	private PrimaryElement primaryElementBacking;

	// Token: 0x04003CD0 RID: 15568
	public Overheatable overheatable;

	// Token: 0x04003CD1 RID: 15569
	public Building building;

	// Token: 0x04003CD2 RID: 15570
	public Operational operational;

	// Token: 0x04003CD3 RID: 15571
	public KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04003CD4 RID: 15572
	public List<StructureTemperaturePayload.EnergySource> energySourcesKW;

	// Token: 0x04003CD5 RID: 15573
	public float pendingEnergyModifications;

	// Token: 0x04003CD6 RID: 15574
	public float maxTemperature;

	// Token: 0x04003CD7 RID: 15575
	public Extents overriddenExtents;

	// Token: 0x02001D78 RID: 7544
	public class EnergySource
	{
		// Token: 0x0600B138 RID: 45368 RVA: 0x003DCAE5 File Offset: 0x003DACE5
		public EnergySource(float kj, string source)
		{
			this.source = source;
			this.kw_accumulator = new RunningAverage(float.MinValue, float.MaxValue, Mathf.RoundToInt(186f), true);
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x0600B139 RID: 45369 RVA: 0x003DCB14 File Offset: 0x003DAD14
		public float value
		{
			get
			{
				float averageValue = this.kw_accumulator.AverageValue;
				if (averageValue == float.NaN)
				{
					return 0f;
				}
				return averageValue;
			}
		}

		// Token: 0x0600B13A RID: 45370 RVA: 0x003DCB3C File Offset: 0x003DAD3C
		public void Accumulate(float value)
		{
			this.kw_accumulator.AddSample(value);
		}

		// Token: 0x04008B6D RID: 35693
		public string source;

		// Token: 0x04008B6E RID: 35694
		public RunningAverage kw_accumulator;
	}
}
