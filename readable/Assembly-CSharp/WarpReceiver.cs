using System;
using System.Linq;
using KSerialization;

// Token: 0x0200082A RID: 2090
public class WarpReceiver : Workable
{
	// Token: 0x06003901 RID: 14593 RVA: 0x0013EDBC File Offset: 0x0013CFBC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0013EDC4 File Offset: 0x0013CFC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpReceiverSMI = new WarpReceiver.WarpReceiverSM.Instance(this);
		this.warpReceiverSMI.StartSM();
		Components.WarpReceivers.Add(this);
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x0013EDF0 File Offset: 0x0013CFF0
	public void ReceiveWarpedDuplicant(WorkerBase dupe)
	{
		dupe.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this), CellAlignment.Bottom, Grid.SceneLayer.Move));
		Debug.Assert(this.chore == null);
		KAnimFile anim = Assets.GetAnim("anim_interacts_warp_portal_receiver_kanim");
		ChoreType migrate = Db.Get().ChoreTypes.Migrate;
		KAnimFile override_anims = anim;
		this.chore = new WorkChore<Workable>(migrate, this, dupe.GetComponent<ChoreProvider>(), true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, true, true, override_anims, false, true, false, PriorityScreen.PriorityClass.compulsory, 5, false, true);
		Workable component = base.GetComponent<Workable>();
		component.workLayer = Grid.SceneLayer.Building;
		component.workAnims = new HashedString[]
		{
			"printing_pre",
			"printing_loop"
		};
		component.workingPstComplete = new HashedString[]
		{
			"printing_pst"
		};
		component.workingPstFailed = new HashedString[]
		{
			"printing_pst"
		};
		component.synchronizeAnims = true;
		float num = 0f;
		KAnimFileData data = anim.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim2 = data.GetAnim(i);
			if (component.workAnims.Contains(anim2.hash))
			{
				num += anim2.totalTime;
			}
		}
		component.SetWorkTime(num);
		this.Used = true;
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x0013EF4B File Offset: 0x0013D14B
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
		this.warpReceiverSMI.GoTo(this.warpReceiverSMI.sm.idle);
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0013EF7A File Offset: 0x0013D17A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.WarpReceivers.Remove(this);
	}

	// Token: 0x040022D0 RID: 8912
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040022D1 RID: 8913
	private WarpReceiver.WarpReceiverSM.Instance warpReceiverSMI;

	// Token: 0x040022D2 RID: 8914
	private Notification notification;

	// Token: 0x040022D3 RID: 8915
	[Serialize]
	public bool IsConsumed;

	// Token: 0x040022D4 RID: 8916
	private Chore chore;

	// Token: 0x040022D5 RID: 8917
	[Serialize]
	public bool Used;

	// Token: 0x020017D4 RID: 6100
	public class WarpReceiverSM : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver>
	{
		// Token: 0x06009CB3 RID: 40115 RVA: 0x0039ADDD File Offset: 0x00398FDD
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("idle");
		}

		// Token: 0x040078E7 RID: 30951
		public GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.State idle;

		// Token: 0x02002969 RID: 10601
		public new class Instance : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.GameInstance
		{
			// Token: 0x0600D0E4 RID: 53476 RVA: 0x00435E3E File Offset: 0x0043403E
			public Instance(WarpReceiver master) : base(master)
			{
			}
		}
	}
}
