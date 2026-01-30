using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007FC RID: 2044
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitDropper : StateMachineComponent<SolidConduitDropper.SMInstance>
{
	// Token: 0x060036D8 RID: 14040 RVA: 0x00135279 File Offset: 0x00133479
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x00135281 File Offset: 0x00133481
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x00135294 File Offset: 0x00133494
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x0013529C File Offset: 0x0013349C
	private void Update()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
		base.smi.sm.isclosed.Set(!this.operational.IsOperational, base.smi, false);
		this.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x04002149 RID: 8521
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400214A RID: 8522
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x0400214B RID: 8523
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x02001776 RID: 6006
	public class SMInstance : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.GameInstance
	{
		// Token: 0x06009B49 RID: 39753 RVA: 0x0039438E File Offset: 0x0039258E
		public SMInstance(SolidConduitDropper master) : base(master)
		{
		}
	}

	// Token: 0x02001777 RID: 6007
	public class States : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper>
	{
		// Token: 0x06009B4A RID: 39754 RVA: 0x00394398 File Offset: 0x00392598
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("Update", delegate(SolidConduitDropper.SMInstance smi, float dt)
			{
				smi.master.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.closed, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.idle, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
		}

		// Token: 0x040077CA RID: 30666
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter consuming;

		// Token: 0x040077CB RID: 30667
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter isclosed;

		// Token: 0x040077CC RID: 30668
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State idle;

		// Token: 0x040077CD RID: 30669
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State working;

		// Token: 0x040077CE RID: 30670
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State post;

		// Token: 0x040077CF RID: 30671
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State closed;
	}
}
