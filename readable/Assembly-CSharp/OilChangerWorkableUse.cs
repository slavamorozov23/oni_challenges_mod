using System;
using Klei;
using Klei.AI;
using UnityEngine;

// Token: 0x020007CF RID: 1999
public class OilChangerWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06003500 RID: 13568 RVA: 0x0012C2A8 File Offset: 0x0012A4A8
	private OilChangerWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x0012C2B8 File Offset: 0x0012A4B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.operational = base.GetComponent<Operational>();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x0012C308 File Offset: 0x0012A508
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			worker.transform.SetPosition(position);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
		this.operational.SetActive(true, false);
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x0012C38C File Offset: 0x0012A58C
	protected override void OnStopWork(WorkerBase worker)
	{
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			worker.transform.SetPosition(position);
		}
		this.operational.SetActive(false, false);
		base.OnStopWork(worker);
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x0012C3E0 File Offset: 0x0012A5E0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		BionicOilMonitor.Instance smi = worker.GetSMI<BionicOilMonitor.Instance>();
		if (smi != null)
		{
			float b = 200f - smi.CurrentOilMass;
			float num = Mathf.Min(component.GetMassAvailable(GameTags.LubricatingOil), b);
			float num2 = num;
			float num3 = 0f;
			Storage component2 = base.GetComponent<Storage>();
			SimHashes lubricant = SimHashes.CrudeOil;
			foreach (SimHashes simHashes in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo;
				float num5;
				component2.ConsumeAndGetDisease(simHashes.CreateTag(), num2, out num4, out diseaseInfo, out num5);
				if (num4 > num3)
				{
					lubricant = simHashes;
					num3 = num4;
				}
				num2 -= num4;
			}
			base.GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, num2);
			smi.RefillOil(num);
			BionicOilMonitor.ApplyLubricationEffects(worker.GetComponent<Effects>(), lubricant);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x04002010 RID: 8208
	private Operational operational;
}
