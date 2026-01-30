using System;
using KSerialization;

// Token: 0x020007FD RID: 2045
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitInbox : StateMachineComponent<SolidConduitInbox.SMInstance>, ISim1000ms
{
	// Token: 0x060036DD RID: 14045 RVA: 0x0013531C File Offset: 0x0013351C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.StorageFetch);
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x00135342 File Offset: 0x00133542
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filteredStorage.FilterChanged();
		base.smi.StartSM();
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x00135360 File Offset: 0x00133560
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x00135368 File Offset: 0x00133568
	public void Sim1000ms(float dt)
	{
		if (this.operational.IsOperational && this.dispenser.IsDispensing)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x0400214C RID: 8524
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400214D RID: 8525
	[MyCmpReq]
	private SolidConduitDispenser dispenser;

	// Token: 0x0400214E RID: 8526
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400214F RID: 8527
	private FilteredStorage filteredStorage;

	// Token: 0x02001778 RID: 6008
	public class SMInstance : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.GameInstance
	{
		// Token: 0x06009B4C RID: 39756 RVA: 0x003944B0 File Offset: 0x003926B0
		public SMInstance(SolidConduitInbox master) : base(master)
		{
		}
	}

	// Token: 0x02001779 RID: 6009
	public class States : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox>
	{
		// Token: 0x06009B4D RID: 39757 RVA: 0x003944BC File Offset: 0x003926BC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).EventTransition(GameHashes.ActiveChanged, this.on.post, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x040077D0 RID: 30672
		public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State off;

		// Token: 0x040077D1 RID: 30673
		public SolidConduitInbox.States.ReadyStates on;

		// Token: 0x0200293F RID: 10559
		public class ReadyStates : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State
		{
			// Token: 0x0400B678 RID: 46712
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State idle;

			// Token: 0x0400B679 RID: 46713
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State working;

			// Token: 0x0400B67A RID: 46714
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State post;
		}
	}
}
