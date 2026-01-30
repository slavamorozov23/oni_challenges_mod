using System;
using UnityEngine;

// Token: 0x02000BB4 RID: 2996
public class ClusterMapBallisticAnimator : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x060059F3 RID: 23027 RVA: 0x0020A2E0 File Offset: 0x002084E0
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.moving;
		this.root.Target(this.entityTarget).TagTransition(GameTags.BallisticEntityLaunching, this.launching, false).TagTransition(GameTags.BallisticEntityLanding, this.landing, false).TagTransition(GameTags.BallisticEntityMoving, this.moving, false);
		this.moving.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.landing.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
		});
		this.launching.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
		});
	}

	// Token: 0x04003C2A RID: 15402
	public StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003C2B RID: 15403
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State launching;

	// Token: 0x04003C2C RID: 15404
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;

	// Token: 0x04003C2D RID: 15405
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;

	// Token: 0x02001D4D RID: 7501
	public class StatesInstance : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600B0C1 RID: 45249 RVA: 0x003DB7D3 File Offset: 0x003D99D3
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600B0C2 RID: 45250 RVA: 0x003DB7FC File Offset: 0x003D99FC
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x0600B0C3 RID: 45251 RVA: 0x003DB80C File Offset: 0x003D9A0C
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600B0C4 RID: 45252 RVA: 0x003DB844 File Offset: 0x003D9A44
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x0600B0C5 RID: 45253 RVA: 0x003DB886 File Offset: 0x003D9A86
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClustermapBallisticAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600B0C6 RID: 45254 RVA: 0x003DB8C0 File Offset: 0x003D9AC0
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x04008AEC RID: 35564
		public ClusterGridEntity entity;

		// Token: 0x04008AED RID: 35565
		private int animCompleteHandle = -1;

		// Token: 0x04008AEE RID: 35566
		private GameObject animCompleteSubscriber;
	}
}
