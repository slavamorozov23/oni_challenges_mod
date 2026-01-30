using System;
using UnityEngine;

// Token: 0x020006C2 RID: 1730
public class GameplayEventFX : GameStateMachine<GameplayEventFX, GameplayEventFX.Instance>
{
	// Token: 0x06002A77 RID: 10871 RVA: 0x000F8974 File Offset: 0x000F6B74
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("event_pre").OnAnimQueueComplete(this.single).Exit("DestroyFX", delegate(GameplayEventFX.Instance smi)
		{
			smi.DestroyFX();
		});
		this.single.PlayAnim("event_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.notificationCount, this.multiple, (GameplayEventFX.Instance smi, int p) => p > 1);
		this.multiple.PlayAnim("event_loop_multiple", KAnim.PlayMode.Loop).ParamTransition<int>(this.notificationCount, this.single, (GameplayEventFX.Instance smi, int p) => p == 1);
	}

	// Token: 0x04001938 RID: 6456
	public StateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001939 RID: 6457
	public StateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.IntParameter notificationCount;

	// Token: 0x0400193A RID: 6458
	public GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.State single;

	// Token: 0x0400193B RID: 6459
	public GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.State multiple;

	// Token: 0x02001581 RID: 5505
	public new class Instance : GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600938B RID: 37771 RVA: 0x00376064 File Offset: 0x00374264
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("event_alert_fx_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x0600938C RID: 37772 RVA: 0x003760D0 File Offset: 0x003742D0
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040071F1 RID: 29169
		public int previousCount;
	}
}
