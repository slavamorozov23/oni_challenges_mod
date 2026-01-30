using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
[AddComponentMenu("KMonoBehaviour/Workable/ArcadeMachineWorkable")]
public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06002AC6 RID: 10950 RVA: 0x000FA9CA File Offset: 0x000F8BCA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x000FA9FA File Offset: 0x000F8BFA
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("ArcadePlaying", false);
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x000FAA15 File Offset: 0x000F8C15
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("ArcadePlaying");
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x000FAA30 File Offset: 0x000F8C30
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
		{
			component.Add(ArcadeMachineWorkable.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
		{
			component.Add(ArcadeMachineWorkable.specificEffect, true);
		}
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x000FAA78 File Offset: 0x000F8C78
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04001986 RID: 6534
	public ArcadeMachine owner;

	// Token: 0x04001987 RID: 6535
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04001988 RID: 6536
	private static string specificEffect = "PlayedArcade";

	// Token: 0x04001989 RID: 6537
	private static string trackingEffect = "RecentlyPlayedArcade";
}
