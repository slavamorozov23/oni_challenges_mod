using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class GunkEmptierWorkable : Workable
{
	// Token: 0x06000C33 RID: 3123 RVA: 0x000494BE File Offset: 0x000476BE
	private GunkEmptierWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x000494D0 File Offset: 0x000476D0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gunkdump_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		this.storage = base.GetComponent<Storage>();
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x0004953C File Offset: 0x0004773C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float mass = Mathf.Min(new float[]
		{
			dt / this.workTime * GunkMonitor.GUNK_CAPACITY,
			this.gunkMonitor.CurrentGunkMass,
			this.storage.RemainingCapacity()
		});
		this.gunkMonitor.ExpellGunk(mass, this.storage);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0004959C File Offset: 0x0004779C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		this.TriggerRoomEffects();
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00049610 File Offset: 0x00047810
	private void TriggerRoomEffects()
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			List<EffectInstance> list = null;
			roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), base.worker.GetComponent<Effects>(), out list);
			if (list != null)
			{
				foreach (EffectInstance effectInstance in list)
				{
					effectInstance.timeRemaining = 1800f;
				}
			}
		}
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0004969C File Offset: 0x0004789C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.gunkMonitor != null)
		{
			this.gunkMonitor.ExpellAllGunk(this.storage);
		}
		this.gunkMonitor = null;
		base.OnCompleteWork(worker);
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x000496C5 File Offset: 0x000478C5
	protected override void OnStopWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnStopWork(worker);
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x000496D4 File Offset: 0x000478D4
	protected override void OnAbortWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnAbortWork(worker);
		this.gunkMonitor = null;
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000496EA File Offset: 0x000478EA
	private void RemoveExpellingRadStatusItem()
	{
		if (Sim.IsRadiationEnabled())
		{
			base.worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
	}

	// Token: 0x04000862 RID: 2146
	private const float BATHROOM_EFFECTS_DURATION_OVERRIDE = 1800f;

	// Token: 0x04000863 RID: 2147
	private Storage storage;

	// Token: 0x04000864 RID: 2148
	private GunkMonitor.Instance gunkMonitor;
}
