using System;
using UnityEngine;

// Token: 0x02000BB7 RID: 2999
public class ClusterMapLongRangeMissileAnimator : GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x060059F9 RID: 23033 RVA: 0x0020A484 File Offset: 0x00208684
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.moving;
		this.root.OnTargetLost(this.entityTarget, null).Target(this.entityTarget).TagTransition(GameTags.LongRangeMissileMoving, this.moving, false).TagTransition(GameTags.LongRangeMissileIdle, this.idle, false).TagTransition(GameTags.LongRangeMissileExploding, this.exploding, false);
		this.moving.Enter(delegate(ClusterMapLongRangeMissileAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.idle.Enter(delegate(ClusterMapLongRangeMissileAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop);
		});
		this.exploding.DefaultState(this.exploding.pre);
		this.exploding.pre.ScheduleGoTo(10f, this.exploding.animating).EventTransition(GameHashes.ClusterMapTravelAnimatorMoveComplete, this.exploding.animating, null);
		this.exploding.animating.Enter(delegate(ClusterMapLongRangeMissileAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object _)
			{
				smi.GoTo(this.exploding.post);
			});
		});
		this.exploding.post.Enter(delegate(ClusterMapLongRangeMissileAnimator.StatesInstance smi)
		{
			if (smi.entity != null)
			{
				smi.entity.Trigger(-1311384361, null);
			}
			smi.GoTo(null);
		});
	}

	// Token: 0x04003C35 RID: 15413
	public StateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003C36 RID: 15414
	public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;

	// Token: 0x04003C37 RID: 15415
	public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x04003C38 RID: 15416
	public ClusterMapLongRangeMissileAnimator.ExplodingStates exploding;

	// Token: 0x02001D51 RID: 7505
	public class ExplodingStates : GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x04008AF5 RID: 35573
		public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;

		// Token: 0x04008AF6 RID: 35574
		public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State animating;

		// Token: 0x04008AF7 RID: 35575
		public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State post;
	}

	// Token: 0x02001D52 RID: 7506
	public class StatesInstance : GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600B0D3 RID: 45267 RVA: 0x003DB998 File Offset: 0x003D9B98
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600B0D4 RID: 45268 RVA: 0x003DB9C1 File Offset: 0x003D9BC1
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x0600B0D5 RID: 45269 RVA: 0x003DB9D0 File Offset: 0x003D9BD0
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600B0D6 RID: 45270 RVA: 0x003DBA08 File Offset: 0x003D9C08
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x0600B0D7 RID: 45271 RVA: 0x003DBA4A File Offset: 0x003D9C4A
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClustermapBallisticAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600B0D8 RID: 45272 RVA: 0x003DBA84 File Offset: 0x003D9C84
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x04008AF8 RID: 35576
		public ClusterGridEntity entity;

		// Token: 0x04008AF9 RID: 35577
		private int animCompleteHandle = -1;

		// Token: 0x04008AFA RID: 35578
		private GameObject animCompleteSubscriber;
	}
}
