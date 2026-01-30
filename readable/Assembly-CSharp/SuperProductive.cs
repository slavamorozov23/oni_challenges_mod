using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class SuperProductive : GameStateMachine<SuperProductive, SuperProductive.Instance>
{
	// Token: 0x06001B2D RID: 6957 RVA: 0x000956AC File Offset: 0x000938AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleStatusItem(Db.Get().DuplicantStatusItems.BeingProductive, null).Enter(delegate(SuperProductive.Instance smi)
		{
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, DUPLICANTS.TRAITS.SUPERPRODUCTIVE.NAME, smi.master.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			smi.fx = new SuperProductiveFX.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
		}).Exit(delegate(SuperProductive.Instance smi)
		{
			smi.fx.sm.destroyFX.Trigger(smi.fx);
		}).DefaultState(this.overjoyed.idle);
		this.overjoyed.idle.EventTransition(GameHashes.StartWork, this.overjoyed.working, null);
		this.overjoyed.working.ScheduleGoTo(0.33f, this.overjoyed.superProductive);
		this.overjoyed.superProductive.Enter(delegate(SuperProductive.Instance smi)
		{
			WorkerBase component = smi.GetComponent<WorkerBase>();
			if (component != null && component.GetState() == WorkerBase.State.Working)
			{
				Workable workable = component.GetWorkable();
				if (workable != null)
				{
					float num = workable.WorkTimeRemaining;
					if (workable.GetComponent<Diggable>() != null)
					{
						num = Diggable.GetApproximateDigTime(Grid.PosToCell(workable));
					}
					if (num > 1f && smi.ShouldSkipWork() && component.InstantlyFinish())
					{
						smi.ReactSuperProductive();
						smi.fx.sm.wasProductive.Trigger(smi.fx);
					}
				}
			}
			smi.GoTo(this.overjoyed.idle);
		});
	}

	// Token: 0x04000FAB RID: 4011
	public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000FAC RID: 4012
	public SuperProductive.OverjoyedStates overjoyed;

	// Token: 0x0200137F RID: 4991
	public class OverjoyedStates : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B59 RID: 27481
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B5A RID: 27482
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x04006B5B RID: 27483
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State superProductive;
	}

	// Token: 0x02001380 RID: 4992
	public new class Instance : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C1D RID: 35869 RVA: 0x0036067B File Offset: 0x0035E87B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008C1E RID: 35870 RVA: 0x00360684 File Offset: 0x0035E884
		public bool ShouldSkipWork()
		{
			return UnityEngine.Random.Range(0f, 100f) <= TRAITS.JOY_REACTIONS.SUPER_PRODUCTIVE.INSTANT_SUCCESS_CHANCE;
		}

		// Token: 0x06008C1F RID: 35871 RVA: 0x003606A0 File Offset: 0x0035E8A0
		public void ReactSuperProductive()
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi != null)
			{
				smi.AddSelfEmoteReactable(base.gameObject, "SuperProductive", Db.Get().Emotes.Minion.ProductiveCheer, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 1f, 1f, 0f, null);
			}
		}

		// Token: 0x04006B5C RID: 27484
		public SuperProductiveFX.Instance fx;
	}
}
