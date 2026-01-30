using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class RemoteWorker : StandardWorker
{
	// Token: 0x0600509E RID: 20638 RVA: 0x001D38FC File Offset: 0x001D1AFC
	public override Attributes GetAttributes()
	{
		RemoteWorkerDock homeDepot = this.remoteWorkerSM.HomeDepot;
		WorkerBase workerBase = ((homeDepot != null) ? homeDepot.GetActiveTerminalWorker() : null) ?? null;
		if (workerBase != null)
		{
			return workerBase.GetAttributes();
		}
		return null;
	}

	// Token: 0x0600509F RID: 20639 RVA: 0x001D3938 File Offset: 0x001D1B38
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		RemoteWorkerDock homeDepot = this.remoteWorkerSM.HomeDepot;
		WorkerBase workerBase = ((homeDepot != null) ? homeDepot.GetActiveTerminalWorker() : null) ?? null;
		if (workerBase != null)
		{
			return workerBase.GetAttributeConverter(id);
		}
		return null;
	}

	// Token: 0x060050A0 RID: 20640 RVA: 0x001D3974 File Offset: 0x001D1B74
	protected override void TryPlayingIdle()
	{
		if (this.remoteWorkerSM.Docked)
		{
			base.GetComponent<KAnimControllerBase>().Play("in_dock_idle", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		base.TryPlayingIdle();
	}

	// Token: 0x060050A1 RID: 20641 RVA: 0x001D39AC File Offset: 0x001D1BAC
	protected override void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		base.InternalStopWork(target_workable, is_aborted);
		Vector3 position = base.transform.GetPosition();
		RemoteWorkerSM remoteWorkerSM = this.remoteWorkerSM;
		position.z = Grid.GetLayerZ((remoteWorkerSM != null && remoteWorkerSM.Docked) ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
	}

	// Token: 0x040035C9 RID: 13769
	[MyCmpGet]
	private RemoteWorkerSM remoteWorkerSM;
}
