using System;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public class SicknessCuredFX : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance>
{
	// Token: 0x06002A80 RID: 10880 RVA: 0x000F9150 File Offset: 0x000F7350
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).Exit("DestroyFX", delegate(SicknessCuredFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001943 RID: 6467
	public StateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02001586 RID: 5510
	public new class Instance : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060093A0 RID: 37792 RVA: 0x00376508 File Offset: 0x00374708
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("recentlyhealed_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x060093A1 RID: 37793 RVA: 0x0037656A File Offset: 0x0037476A
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
