using System;

// Token: 0x0200076C RID: 1900
public class ToggleGeothermalVentConnection : Toggleable
{
	// Token: 0x0600303C RID: 12348 RVA: 0x001168C0 File Offset: 0x00114AC0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim(GeothermalVentConfig.TOGGLE_ANIM_OVERRIDE)
		};
		this.workAnims = new HashedString[]
		{
			GeothermalVentConfig.TOGGLE_ANIMATION
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.workLayer = Grid.SceneLayer.Front;
		this.synchronizeAnims = false;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		base.SetOffsets(new CellOffset[]
		{
			CellOffset.none
		});
	}

	// Token: 0x0600303D RID: 12349 RVA: 0x00116950 File Offset: 0x00114B50
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.buildingAnimController.Play(GeothermalVentConfig.TOGGLE_ANIMATION, KAnim.PlayMode.Once, 1f, 0f);
		if (this.workerFacing == null || this.workerFacing.gameObject != worker.gameObject)
		{
			this.workerFacing = worker.GetComponent<Facing>();
		}
	}

	// Token: 0x0600303E RID: 12350 RVA: 0x001169B6 File Offset: 0x00114BB6
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.workerFacing != null)
		{
			this.workerFacing.Face(this.workerFacing.transform.GetLocalPosition().x + 0.5f);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x04001CB6 RID: 7350
	[MyCmpGet]
	private KBatchedAnimController buildingAnimController;

	// Token: 0x04001CB7 RID: 7351
	private Facing workerFacing;
}
