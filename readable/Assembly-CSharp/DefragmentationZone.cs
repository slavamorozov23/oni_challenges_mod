using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x020008D0 RID: 2256
public class DefragmentationZone : Workable
{
	// Token: 0x06003E8A RID: 16010 RVA: 0x0015E354 File Offset: 0x0015C554
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
		this.approachable = base.GetComponent<IApproachable>();
		this.workAnims = new HashedString[]
		{
			"microchip_bed_pre",
			"microchip_bed_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"microchip_bed_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"microchip_bed_pst"
		};
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0015E406 File Offset: 0x0015C606
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0015E43B File Offset: 0x0015C63B
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddRoomEffects();
		}
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0015E448 File Offset: 0x0015C648
	private void AddRoomEffects()
	{
		if (base.worker == null)
		{
			return;
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
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

	// Token: 0x06003E8E RID: 16014 RVA: 0x0015E4E4 File Offset: 0x0015C6E4
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x0400269C RID: 9884
	private const float BEDROOM_EFFECTS_DURATION_OVERRIDE = 1800f;

	// Token: 0x0400269D RID: 9885
	[MyCmpGet]
	public Assignable assignable;

	// Token: 0x0400269E RID: 9886
	public IApproachable approachable;
}
