using System;

// Token: 0x02000BB5 RID: 2997
public class ClusterMapFXAnimator : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x060059F5 RID: 23029 RVA: 0x0020A3C4 File Offset: 0x002085C4
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.play;
		this.play.OnSignal(this.onAnimComplete, this.finished);
		this.finished.Enter(delegate(ClusterMapFXAnimator.StatesInstance smi)
		{
			smi.DestroyEntity();
		});
	}

	// Token: 0x04003C2E RID: 15406
	private KBatchedAnimController animController;

	// Token: 0x04003C2F RID: 15407
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003C30 RID: 15408
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State play;

	// Token: 0x04003C31 RID: 15409
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State finished;

	// Token: 0x04003C32 RID: 15410
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.Signal onAnimComplete;

	// Token: 0x02001D4F RID: 7503
	public class StatesInstance : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600B0CC RID: 45260 RVA: 0x003DB90C File Offset: 0x003D9B0C
		public StatesInstance(ClusterMapVisualizer visualizer, ClusterGridEntity entity) : base(visualizer)
		{
			base.sm.entityTarget.Set(entity, this);
			visualizer.GetFirstAnimController().gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
		}

		// Token: 0x0600B0CD RID: 45261 RVA: 0x003DB949 File Offset: 0x003D9B49
		private void OnAnimQueueComplete(object data)
		{
			base.sm.onAnimComplete.Trigger(this);
		}

		// Token: 0x0600B0CE RID: 45262 RVA: 0x003DB95C File Offset: 0x003D9B5C
		public void DestroyEntity()
		{
			base.sm.entityTarget.Get(this).DeleteObject();
		}
	}
}
