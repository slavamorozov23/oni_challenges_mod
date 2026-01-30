using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200081B RID: 2075
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableUse")]
public class ToiletWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06003852 RID: 14418 RVA: 0x0013B517 File Offset: 0x00139717
	private ToiletWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x0013B548 File Offset: 0x00139748
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x0013B580 File Offset: 0x00139780
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
		if (worker != null)
		{
			this.last_user_id = worker.gameObject.PrefabID();
		}
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x0013B62C File Offset: 0x0013982C
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		HashedString[] array = null;
		if (this.workerTypePstAnims.TryGetValue(worker.PrefabID(), out array))
		{
			this.workingPstComplete = array;
			this.workingPstFailed = array;
		}
		return base.GetWorkPstAnims(worker, successfully_completed);
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x0013B668 File Offset: 0x00139868
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x0013B69A File Offset: 0x0013989A
	protected override void OnStopWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x0013B6CB File Offset: 0x001398CB
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnAbortWork(worker);
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x0013B6FC File Offset: 0x001398FC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(worker);
		if (amountInstance != null)
		{
			this.lastAmountOfWasteMassRemovedFromDupe = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
			this.lastElementRemovedFromDupe = SimHashes.DirtyWater;
			amountInstance.SetValue(0f);
		}
		else
		{
			GunkMonitor.Instance smi = worker.GetSMI<GunkMonitor.Instance>();
			if (smi != null)
			{
				this.lastAmountOfWasteMassRemovedFromDupe = smi.CurrentGunkMass;
				this.lastElementRemovedFromDupe = GunkMonitor.GunkElement;
				smi.SetGunkMassValue(0f);
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_GunkedToilet, true);
			}
		}
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
			AmountInstance amountInstance2 = Db.Get().Amounts.RadiationBalance.Lookup(worker);
			RadiationMonitor.Instance smi2 = worker.GetSMI<RadiationMonitor.Instance>();
			float num = Math.Min(amountInstance2.value, 100f * smi2.difficultySettingMod);
			if (num >= 1f)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, worker.transform, Vector3.up * 2f, 1.5f, false, false);
			}
			amountInstance2.ApplyDelta(-num);
		}
		this.timesUsed++;
		if (amountInstance != null)
		{
			base.Trigger(-350347868, worker);
		}
		else
		{
			base.Trigger(1234642927, worker);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x0013B87B File Offset: 0x00139A7B
	public override StatusItem GetWorkerStatusItem()
	{
		if (base.worker != null && base.worker.gameObject.HasTag(GameTags.Minions.Models.Bionic))
		{
			return Db.Get().DuplicantStatusItems.CloggingToilet;
		}
		return base.GetWorkerStatusItem();
	}

	// Token: 0x04002239 RID: 8761
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x0400223A RID: 8762
	public Dictionary<Tag, HashedString[]> workerTypePstAnims = new Dictionary<Tag, HashedString[]>();

	// Token: 0x0400223B RID: 8763
	[Serialize]
	public int timesUsed;

	// Token: 0x0400223C RID: 8764
	[Serialize]
	public Tag last_user_id;

	// Token: 0x0400223D RID: 8765
	[Serialize]
	public SimHashes lastElementRemovedFromDupe = SimHashes.DirtyWater;

	// Token: 0x0400223E RID: 8766
	[Serialize]
	public float lastAmountOfWasteMassRemovedFromDupe;
}
