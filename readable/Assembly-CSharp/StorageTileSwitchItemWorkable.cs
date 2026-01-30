using System;

// Token: 0x0200080A RID: 2058
public class StorageTileSwitchItemWorkable : Workable
{
	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06003775 RID: 14197 RVA: 0x00137A86 File Offset: 0x00135C86
	// (set) Token: 0x06003774 RID: 14196 RVA: 0x00137A7D File Offset: 0x00135C7D
	public int LastCellWorkerUsed { get; private set; } = -1;

	// Token: 0x06003776 RID: 14198 RVA: 0x00137A8E File Offset: 0x00135C8E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x00137ACD File Offset: 0x00135CCD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x00137AE0 File Offset: 0x00135CE0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (worker != null)
		{
			this.LastCellWorkerUsed = Grid.PosToCell(worker.transform.GetPosition());
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x040021CE RID: 8654
	private const string animName = "anim_use_remote_kanim";
}
