using System;
using UnityEngine;

// Token: 0x020006BF RID: 1727
public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
	// Token: 0x06002A70 RID: 10864 RVA: 0x000F859C File Offset: 0x000F679C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		base.Target(this.fx);
		this.loop.Enter(delegate(FXAnim.Instance smi)
		{
			smi.Enter();
		}).EventTransition(GameHashes.AnimQueueComplete, this.restart, null).Exit("Post", delegate(FXAnim.Instance smi)
		{
			smi.Exit();
		});
		this.restart.GoTo(this.loop);
	}

	// Token: 0x04001934 RID: 6452
	public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001935 RID: 6453
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

	// Token: 0x04001936 RID: 6454
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State restart;

	// Token: 0x0200157C RID: 5500
	public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009375 RID: 37749 RVA: 0x00375DF8 File Offset: 0x00373FF8
		public Instance(IStateMachineTarget master, string kanim_file, string anim, KAnim.PlayMode mode, Vector3 offset, Color32 tint_colour) : base(master)
		{
			this.animController = FXHelpers.CreateEffect(kanim_file, base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			this.animController.gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
			this.animController.TintColour = tint_colour;
			base.sm.fx.Set(this.animController.gameObject, base.smi, false);
			this.anim = anim;
			this.mode = mode;
		}

		// Token: 0x06009376 RID: 37750 RVA: 0x00375EA9 File Offset: 0x003740A9
		public void Enter()
		{
			this.animController.Play(this.anim, this.mode, 1f, 0f);
		}

		// Token: 0x06009377 RID: 37751 RVA: 0x00375ED1 File Offset: 0x003740D1
		public void Exit()
		{
			this.DestroyFX();
		}

		// Token: 0x06009378 RID: 37752 RVA: 0x00375ED9 File Offset: 0x003740D9
		private void OnAnimQueueComplete(object data)
		{
			this.DestroyFX();
		}

		// Token: 0x06009379 RID: 37753 RVA: 0x00375EE1 File Offset: 0x003740E1
		private void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040071E2 RID: 29154
		private string anim;

		// Token: 0x040071E3 RID: 29155
		private KAnim.PlayMode mode;

		// Token: 0x040071E4 RID: 29156
		private KBatchedAnimController animController;
	}
}
