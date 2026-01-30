using System;
using UnityEngine;

// Token: 0x020006C1 RID: 1729
public class FliesFX : GameStateMachine<FliesFX, FliesFX.Instance>
{
	// Token: 0x06002A75 RID: 10869 RVA: 0x000F8904 File Offset: 0x000F6B04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("swarm_pre").QueueAnim("swarm_loop", true, null).Exit("DestroyFX", delegate(FliesFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001937 RID: 6455
	public StateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x0200157F RID: 5503
	public new class Instance : GameStateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009386 RID: 37766 RVA: 0x00375FBC File Offset: 0x003741BC
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("fly_swarm_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06009387 RID: 37767 RVA: 0x00376028 File Offset: 0x00374228
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
