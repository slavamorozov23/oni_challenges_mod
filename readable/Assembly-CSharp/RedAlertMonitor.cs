using System;

// Token: 0x02000A3F RID: 2623
public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
	// Token: 0x06004C8A RID: 19594 RVA: 0x001BD0A0 File Offset: 0x001BB2A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.on, delegate(RedAlertMonitor.Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && myWorld.AlertManager.IsRedAlert();
		});
		this.on.EventTransition(GameHashes.ExitedRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.off, delegate(RedAlertMonitor.Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && !myWorld.AlertManager.IsRedAlert();
		}).Enter("EnableRedAlert", delegate(RedAlertMonitor.Instance smi)
		{
			smi.EnableRedAlert();
		}).ToggleEffect("RedAlert").ToggleExpression(Db.Get().Expressions.RedAlert, null);
	}

	// Token: 0x040032E6 RID: 13030
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040032E7 RID: 13031
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02001B23 RID: 6947
	public new class Instance : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A891 RID: 43153 RVA: 0x003BF56A File Offset: 0x003BD76A
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A892 RID: 43154 RVA: 0x003BF574 File Offset: 0x003BD774
		public void EnableRedAlert()
		{
			ChoreDriver component = base.GetComponent<ChoreDriver>();
			if (component != null)
			{
				Chore currentChore = component.GetCurrentChore();
				if (currentChore != null)
				{
					bool flag = false;
					for (int i = 0; i < currentChore.GetPreconditions().Count; i++)
					{
						if (currentChore.GetPreconditions()[i].condition.id == ChorePreconditions.instance.IsNotRedAlert.id)
						{
							flag = true;
						}
					}
					if (flag)
					{
						component.StopChore();
					}
				}
			}
		}
	}
}
