using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009FA RID: 2554
[AddComponentMenu("KMonoBehaviour/Workable/MechanicalSurfboardWorkable")]
public class MechanicalSurfboardWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004A9E RID: 19102 RVA: 0x001AFEED File Offset: 0x001AE0ED
	private MechanicalSurfboardWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004A9F RID: 19103 RVA: 0x001AFEFD File Offset: 0x001AE0FD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(30f);
		this.surfboard = base.GetComponent<MechanicalSurfboard>();
	}

	// Token: 0x06004AA0 RID: 19104 RVA: 0x001AFF31 File Offset: 0x001AE131
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("MechanicalSurfing", false);
	}

	// Token: 0x06004AA1 RID: 19105 RVA: 0x001AFF54 File Offset: 0x001AE154
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		AttributeInstance attributeInstance = worker.GetAttributes().Get(Db.Get().Attributes.Athletics);
		if (attributeInstance.GetTotalValue() <= 7f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[0])
			};
		}
		else if (attributeInstance.GetTotalValue() <= 15f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[1])
			};
		}
		else
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[2])
			};
		}
		return result;
	}

	// Token: 0x06004AA2 RID: 19106 RVA: 0x001B0018 File Offset: 0x001AE218
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		Building component = base.GetComponent<Building>();
		MechanicalSurfboard component2 = base.GetComponent<MechanicalSurfboard>();
		int widthInCells = component.Def.WidthInCells;
		int minInclusive = -(widthInCells - 1) / 2;
		int maxExclusive = widthInCells / 2;
		int x = UnityEngine.Random.Range(minInclusive, maxExclusive);
		float amount = component2.waterSpillRateKG * dt;
		float base_mass;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		base.GetComponent<Storage>().ConsumeAndGetDisease(SimHashes.Water.CreateTag(), amount, out base_mass, out diseaseInfo, out temperature);
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), new CellOffset(x, 0));
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Water);
		FallingWater.instance.AddParticle(cell, elementIndex, base_mass, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
		return false;
	}

	// Token: 0x06004AA3 RID: 19107 RVA: 0x001B00C0 File Offset: 0x001AE2C0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect))
		{
			component.Add(this.surfboard.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect))
		{
			component.Add(this.surfboard.trackingEffect, true);
		}
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x001B011E File Offset: 0x001AE31E
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("MechanicalSurfing");
	}

	// Token: 0x06004AA5 RID: 19109 RVA: 0x001B0140 File Offset: 0x001AE340
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect) && component.HasEffect(this.surfboard.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect) && component.HasEffect(this.surfboard.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003173 RID: 12659
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003174 RID: 12660
	public int basePriority;

	// Token: 0x04003175 RID: 12661
	private MechanicalSurfboard surfboard;
}
