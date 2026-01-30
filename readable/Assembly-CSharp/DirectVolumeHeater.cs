using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000746 RID: 1862
public class DirectVolumeHeater : KMonoBehaviour, ISim33ms, ISim200ms, ISim1000ms, ISim4000ms, IGameObjectEffectDescriptor
{
	// Token: 0x06002EEE RID: 12014 RVA: 0x0010ECA4 File Offset: 0x0010CEA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x0010ECD0 File Offset: 0x0010CED0
	public void Sim33ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms33)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x0010ED0C File Offset: 0x0010CF0C
	public void Sim200ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms200)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x0010ED48 File Offset: 0x0010CF48
	public void Sim1000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms1000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x0010ED84 File Offset: 0x0010CF84
	public void Sim4000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms4000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x0010EDC0 File Offset: 0x0010CFC0
	private float CalculateCellWeight(int dx, int dy, int maxDistance)
	{
		return 1f + (float)(maxDistance - Math.Abs(dx) - Math.Abs(dy));
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x0010EDD8 File Offset: 0x0010CFD8
	private bool TestLineOfSight(int offsetCell)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int x;
		int y;
		Grid.CellToXY(offsetCell, out x, out y);
		int x2;
		int y2;
		Grid.CellToXY(cell, out x2, out y2);
		return Grid.FastTestLineOfSightSolid(x2, y2, x, y);
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x0010EE0C File Offset: 0x0010D00C
	private float AddSelfHeat(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		if (this.primaryElement.Temperature > this.maximumInternalTemperature)
		{
			return 0f;
		}
		float result = 8f;
		GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 8f * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return result;
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x0010EE68 File Offset: 0x0010D068
	private float AddHeatToVolume(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		int num2 = this.width / 2;
		int num3 = this.width % 2;
		int maxDistance = num2 + this.height;
		float num4 = 0f;
		float num5 = this.DTUs * dt / 1000f;
		for (int i = -num2; i < num2 + num3; i++)
		{
			for (int j = 0; j < this.height; j++)
			{
				if (Grid.IsCellOffsetValid(num, i, j))
				{
					int num6 = Grid.OffsetCell(num, i, j);
					if (!Grid.Solid[num6] && Grid.Mass[num6] != 0f && Grid.WorldIdx[num6] == Grid.WorldIdx[num] && this.TestLineOfSight(num6) && Grid.Temperature[num6] < this.maximumExternalTemperature)
					{
						num4 += this.CalculateCellWeight(i, j, maxDistance);
					}
				}
			}
		}
		float num7 = num5;
		if (num4 > 0f)
		{
			num7 /= num4;
		}
		float num8 = 0f;
		for (int k = -num2; k < num2 + num3; k++)
		{
			for (int l = 0; l < this.height; l++)
			{
				if (Grid.IsCellOffsetValid(num, k, l))
				{
					int num9 = Grid.OffsetCell(num, k, l);
					if (!Grid.Solid[num9] && Grid.Mass[num9] != 0f && Grid.WorldIdx[num9] == Grid.WorldIdx[num] && this.TestLineOfSight(num9) && Grid.Temperature[num9] < this.maximumExternalTemperature)
					{
						float num10 = num7 * this.CalculateCellWeight(k, l, maxDistance);
						num8 += num10;
						SimMessages.ModifyEnergy(num9, num10, 10000f, SimMessages.EnergySourceID.HeatBulb);
					}
				}
			}
		}
		return num8;
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x0010F050 File Offset: 0x0010D250
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.DTUs, GameUtil.HeatEnergyFormatterUnit.Automatic);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001BC5 RID: 7109
	[SerializeField]
	public int width = 12;

	// Token: 0x04001BC6 RID: 7110
	[SerializeField]
	public int height = 4;

	// Token: 0x04001BC7 RID: 7111
	[SerializeField]
	public float DTUs = 100000f;

	// Token: 0x04001BC8 RID: 7112
	[SerializeField]
	public float maximumInternalTemperature = 773.15f;

	// Token: 0x04001BC9 RID: 7113
	[SerializeField]
	public float maximumExternalTemperature = 340f;

	// Token: 0x04001BCA RID: 7114
	[SerializeField]
	public Operational operational;

	// Token: 0x04001BCB RID: 7115
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04001BCC RID: 7116
	public bool EnableEmission;

	// Token: 0x04001BCD RID: 7117
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001BCE RID: 7118
	private PrimaryElement primaryElement;

	// Token: 0x04001BCF RID: 7119
	[SerializeField]
	private DirectVolumeHeater.TimeMode impulseFrequency = DirectVolumeHeater.TimeMode.ms1000;

	// Token: 0x02001623 RID: 5667
	private enum TimeMode
	{
		// Token: 0x040073D7 RID: 29655
		ms33,
		// Token: 0x040073D8 RID: 29656
		ms200,
		// Token: 0x040073D9 RID: 29657
		ms1000,
		// Token: 0x040073DA RID: 29658
		ms4000
	}
}
