using System;
using UnityEngine;

// Token: 0x020006C5 RID: 1733
public class SuperProductiveFX : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance>
{
	// Token: 0x06002A82 RID: 10882 RVA: 0x000F91BC File Offset: 0x000F73BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (SuperProductiveFX.Instance smi, StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.SignalParameter param) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("productive_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("productive_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("productive_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("productive_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(SuperProductiveFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001944 RID: 6468
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x04001945 RID: 6469
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x04001946 RID: 6470
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001947 RID: 6471
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x04001948 RID: 6472
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04001949 RID: 6473
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x0400194A RID: 6474
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x02001588 RID: 5512
	public new class Instance : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060093A5 RID: 37797 RVA: 0x003765A4 File Offset: 0x003747A4
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("productive_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x060093A6 RID: 37798 RVA: 0x00376606 File Offset: 0x00374806
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
