using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000605 RID: 1541
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/MinimumOperatingTemperature")]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
{
	// Token: 0x060023D7 RID: 9175 RVA: 0x000CF567 File Offset: 0x000CD767
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.TestTemperature(true);
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000CF576 File Offset: 0x000CD776
	public void Sim200ms(float dt)
	{
		this.TestTemperature(false);
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000CF580 File Offset: 0x000CD780
	private void TestTemperature(bool force)
	{
		bool flag;
		if (this.primaryElement.Temperature < this.minimumTemperature)
		{
			flag = false;
		}
		else
		{
			flag = true;
			for (int i = 0; i < this.building.PlacementCells.Length; i++)
			{
				int i2 = this.building.PlacementCells[i];
				float num = Grid.Temperature[i2];
				float num2 = Grid.Mass[i2];
				if ((num != 0f || num2 != 0f) && num < this.minimumTemperature)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			this.lastOffTime = Time.time;
		}
		if ((flag != this.isWarm && !flag) || (flag != this.isWarm && flag && Time.time > this.lastOffTime + 5f) || force)
		{
			this.isWarm = flag;
			this.operational.SetFlag(MinimumOperatingTemperature.warmEnoughFlag, this.isWarm);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !this.isWarm, this);
		}
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000CF68A File Offset: 0x000CD88A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000CF6A4 File Offset: 0x000CD8A4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x040014D8 RID: 5336
	[MyCmpReq]
	private Building building;

	// Token: 0x040014D9 RID: 5337
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040014DA RID: 5338
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040014DB RID: 5339
	public float minimumTemperature = 275.15f;

	// Token: 0x040014DC RID: 5340
	private const float TURN_ON_DELAY = 5f;

	// Token: 0x040014DD RID: 5341
	private float lastOffTime;

	// Token: 0x040014DE RID: 5342
	public static readonly Operational.Flag warmEnoughFlag = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);

	// Token: 0x040014DF RID: 5343
	private bool isWarm;

	// Token: 0x040014E0 RID: 5344
	private HandleVector<int>.Handle partitionerEntry;
}
