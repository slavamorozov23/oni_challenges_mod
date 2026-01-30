using System;

// Token: 0x02000A69 RID: 2665
[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
	// Token: 0x06004D7B RID: 19835 RVA: 0x001C2AE3 File Offset: 0x001C0CE3
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x001C2AF0 File Offset: 0x001C0CF0
	protected bool IsUncomfortable()
	{
		if (!base.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
		{
			return false;
		}
		int num = 5;
		bool flag = true;
		bool flag2 = true;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 1; i < num; i++)
		{
			int num2 = Grid.OffsetCell(cell, i, 0);
			int num3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Solid[num3])
			{
				flag = false;
			}
			if (Grid.Solid[num2])
			{
				flag2 = false;
			}
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				if (flag && Grid.PosToCell(minionIdentity.gameObject) == num3)
				{
					return true;
				}
				if (flag2 && Grid.PosToCell(minionIdentity.gameObject) == num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x02001B8D RID: 7053
	public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
	{
		// Token: 0x0600AA54 RID: 43604 RVA: 0x003C3DDE File Offset: 0x003C1FDE
		public StatesInstance(SolitarySleeper master) : base(master)
		{
		}
	}

	// Token: 0x02001B8E RID: 7054
	public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
	{
		// Token: 0x0600AA55 RID: 43605 RVA: 0x003C3DE8 File Offset: 0x003C1FE8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.TagTransition(GameTags.Dead, null, false).EventTransition(GameHashes.NewDay, this.satisfied, null).Update("SolitarySleeperCheck", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					if (smi.GetCurrentState() != this.suffering)
					{
						smi.GoTo(this.suffering);
						return;
					}
				}
				else if (smi.GetCurrentState() != this.satisfied)
				{
					smi.GoTo(this.satisfied);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable, null).Update("PeopleTooCloseSleepFail", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				smi.master.gameObject.Trigger(1338475637, this);
			}, UpdateRate.SIM_1000ms, false);
			this.satisfied.DoNothing();
		}

		// Token: 0x04008543 RID: 34115
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;

		// Token: 0x04008544 RID: 34116
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;
	}
}
