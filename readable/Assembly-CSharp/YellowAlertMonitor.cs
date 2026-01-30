using System;

// Token: 0x02000A5A RID: 2650
public class YellowAlertMonitor : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance>
{
	// Token: 0x06004D07 RID: 19719 RVA: 0x001C062C File Offset: 0x001BE82C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.on, (YellowAlertMonitor.Instance smi) => YellowAlertManager.Instance.Get().IsOn());
		this.on.EventTransition(GameHashes.ExitedYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.off, (YellowAlertMonitor.Instance smi) => !YellowAlertManager.Instance.Get().IsOn()).Enter("EnableYellowAlert", delegate(YellowAlertMonitor.Instance smi)
		{
			smi.EnableYellowAlert();
		});
	}

	// Token: 0x04003357 RID: 13143
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04003358 RID: 13144
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02001B6D RID: 7021
	public new class Instance : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9FC RID: 43516 RVA: 0x003C3094 File Offset: 0x003C1294
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A9FD RID: 43517 RVA: 0x003C309D File Offset: 0x003C129D
		public void EnableYellowAlert()
		{
		}
	}
}
