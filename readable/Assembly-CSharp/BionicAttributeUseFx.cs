using System;
using UnityEngine;

// Token: 0x020006BE RID: 1726
public class BionicAttributeUseFx : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance>
{
	// Token: 0x06002A6E RID: 10862 RVA: 0x000F84A4 File Offset: 0x000F66A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (BionicAttributeUseFx.Instance smi, StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.SignalParameter param) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("bionic_upgrade_active_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("bionic_upgrade_active_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("bionic_upgrade_active_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("bionic_upgrade_active_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BionicAttributeUseFx.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x0400192D RID: 6445
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x0400192E RID: 6446
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x0400192F RID: 6447
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001930 RID: 6448
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x04001931 RID: 6449
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04001932 RID: 6450
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x04001933 RID: 6451
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x0200157A RID: 5498
	public new class Instance : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600936F RID: 37743 RVA: 0x00375D34 File Offset: 0x00373F34
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("bionic_upgrade_active_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06009370 RID: 37744 RVA: 0x00375D96 File Offset: 0x00373F96
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
