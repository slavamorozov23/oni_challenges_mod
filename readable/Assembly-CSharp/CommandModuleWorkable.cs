using System;
using UnityEngine;

// Token: 0x02000721 RID: 1825
[AddComponentMenu("KMonoBehaviour/Workable/CommandModuleWorkable")]
public class CommandModuleWorkable : Workable
{
	// Token: 0x06002D95 RID: 11669 RVA: 0x00108894 File Offset: 0x00106A94
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsets(CommandModuleWorkable.entryOffsets);
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
		this.showProgressBar = false;
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x001088E9 File Offset: 0x00106AE9
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x001088F4 File Offset: 0x00106AF4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!(worker != null))
		{
			return base.OnWorkTick(worker, dt);
		}
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = worker.gameObject;
			base.CompleteWork(worker);
			base.GetComponent<ClustercraftExteriorDoor>().FerryMinion(gameObject);
			return true;
		}
		GameObject gameObject2 = worker.gameObject;
		base.CompleteWork(worker);
		base.GetComponent<MinionStorage>().SerializeMinion(gameObject2);
		return true;
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x00108951 File Offset: 0x00106B51
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x0010895A File Offset: 0x00106B5A
	protected override void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x04001B1B RID: 6939
	private static CellOffset[] entryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(0, 2),
		new CellOffset(0, 3),
		new CellOffset(0, 4)
	};
}
