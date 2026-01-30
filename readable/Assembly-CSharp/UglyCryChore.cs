using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
public class UglyCryChore : Chore<UglyCryChore.StatesInstance>
{
	// Token: 0x060019BC RID: 6588 RVA: 0x0008FE7C File Offset: 0x0008E07C
	public UglyCryChore(ChoreType chore_type, IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.UglyCry, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new UglyCryChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001330 RID: 4912
	public class StatesInstance : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.GameInstance
	{
		// Token: 0x06008B2F RID: 35631 RVA: 0x0035DF3B File Offset: 0x0035C13B
		public StatesInstance(UglyCryChore master, GameObject crier) : base(master)
		{
			base.sm.crier.Set(crier, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(crier);
		}

		// Token: 0x06008B30 RID: 35632 RVA: 0x0035DF78 File Offset: 0x0035C178
		public void ProduceTears(float dt)
		{
			if (dt <= 0f)
			{
				return;
			}
			int gameCell = Grid.PosToCell(base.smi.master.gameObject);
			Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
			if (equippable != null)
			{
				equippable.GetComponent<Storage>().AddLiquid(SimHashes.Water, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, false, true);
				return;
			}
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.Water, CellEventLogger.Instance.Tears, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x04006A8B RID: 27275
		private AmountInstance bodyTemperature;
	}

	// Token: 0x02001331 RID: 4913
	public class States : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore>
	{
		// Token: 0x06008B31 RID: 35633 RVA: 0x0035E020 File Offset: 0x0035C220
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.cry;
			base.Target(this.crier);
			this.uglyCryingEffect = new Effect("UglyCrying", DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, DUPLICANTS.MODIFIERS.UGLY_CRYING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.uglyCryingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, false, false, true));
			Db.Get().effects.Add(this.uglyCryingEffect);
			this.cry.defaultState = this.cry.cry_pre.RemoveEffect("CryFace").ToggleAnims("anim_cry_kanim", 0f);
			this.cry.cry_pre.PlayAnim("working_pre").ScheduleGoTo(2f, this.cry.cry_loop);
			this.cry.cry_loop.ToggleAnims("anim_cry_kanim", 0f).Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.Play("working_loop", KAnim.PlayMode.Loop);
			}).ScheduleGoTo(18f, this.cry.cry_pst).ToggleEffect((UglyCryChore.StatesInstance smi) => this.uglyCryingEffect).Update(delegate(UglyCryChore.StatesInstance smi, float dt)
			{
				smi.ProduceTears(dt);
			}, UpdateRate.SIM_200ms, false);
			this.cry.cry_pst.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.AddEffect("CryFace").Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x04006A8C RID: 27276
		public StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.TargetParameter crier;

		// Token: 0x04006A8D RID: 27277
		public UglyCryChore.States.Cry cry;

		// Token: 0x04006A8E RID: 27278
		public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State complete;

		// Token: 0x04006A8F RID: 27279
		private Effect uglyCryingEffect;

		// Token: 0x020027EC RID: 10220
		public class Cry : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State
		{
			// Token: 0x0400B0FE RID: 45310
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pre;

			// Token: 0x0400B0FF RID: 45311
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_loop;

			// Token: 0x0400B100 RID: 45312
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pst;
		}
	}
}
