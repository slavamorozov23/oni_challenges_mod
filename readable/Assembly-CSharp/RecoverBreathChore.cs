using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class RecoverBreathChore : Chore<RecoverBreathChore.StatesInstance>
{
	// Token: 0x06001988 RID: 6536 RVA: 0x0008EB50 File Offset: 0x0008CD50
	public RecoverBreathChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverBreath, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverBreathChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotABionic, null);
	}

	// Token: 0x02001308 RID: 4872
	public class StatesInstance : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.GameInstance
	{
		// Token: 0x06008A79 RID: 35449 RVA: 0x00359414 File Offset: 0x00357614
		public StatesInstance(RecoverBreathChore master, GameObject recoverer) : base(master)
		{
			base.sm.recoverer.Set(recoverer, base.smi, false);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float recover_BREATH_DELTA = DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA;
			this.recoveringbreath = new AttributeModifier(deltaAttribute.Id, recover_BREATH_DELTA, DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME, false, false, true);
		}

		// Token: 0x06008A7A RID: 35450 RVA: 0x00359488 File Offset: 0x00357688
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverBreathLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x003594C0 File Offset: 0x003576C0
		public void UpdateLocator()
		{
			int num = base.sm.recoverer.GetSMI<BreathMonitor.Instance>(base.smi).GetRecoverCell();
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.recoverer.Get<Transform>(base.smi).GetPosition());
			}
			Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
			base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
		}

		// Token: 0x06008A7C RID: 35452 RVA: 0x00359538 File Offset: 0x00357738
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x06008A7D RID: 35453 RVA: 0x00359564 File Offset: 0x00357764
		public void RemoveSuitIfNecessary()
		{
			Equipment equipment = base.sm.recoverer.Get<Equipment>(base.smi);
			if (equipment == null)
			{
				return;
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable == null)
			{
				return;
			}
			assignable.Unassign();
		}

		// Token: 0x040069F4 RID: 27124
		public AttributeModifier recoveringbreath;
	}

	// Token: 0x02001309 RID: 4873
	public class States : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore>
	{
		// Token: 0x06008A7E RID: 35454 RVA: 0x003595B8 File Offset: 0x003577B8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.recoverer);
			this.root.Enter("CreateLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.CreateLocator();
			}).Exit("DestroyLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverBreathChore.StatesInstance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.recoverer, this.locator, this.remove_suit, null, null, null);
			this.remove_suit.GoTo(this.recover);
			this.recover.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.recover.pre).ToggleAttributeModifier("Recovering Breath", (RecoverBreathChore.StatesInstance smi) => smi.recoveringbreath, null).ToggleTag(GameTags.RecoveringBreath).TriggerOnEnter(GameHashes.BeginBreathRecovery, null).TriggerOnExit(GameHashes.EndBreathRecovery, null);
			this.recover.pre.PlayAnim("breathe_pre").OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop);
			this.recover.pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(null);
		}

		// Token: 0x040069F5 RID: 27125
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040069F6 RID: 27126
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.PreLoopPostState recover;

		// Token: 0x040069F7 RID: 27127
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State remove_suit;

		// Token: 0x040069F8 RID: 27128
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter recoverer;

		// Token: 0x040069F9 RID: 27129
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter locator;
	}
}
