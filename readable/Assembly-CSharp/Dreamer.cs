using System;

// Token: 0x020005D5 RID: 1493
public class Dreamer : GameStateMachine<Dreamer, Dreamer.Instance>
{
	// Token: 0x06002273 RID: 8819 RVA: 0x000C84E0 File Offset: 0x000C66E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notDreaming;
		this.notDreaming.OnSignal(this.startDreaming, this.dreaming, (Dreamer.Instance smi, StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.SignalParameter param) => smi.currentDream != null);
		this.dreaming.Enter(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(Dreamer.PrepareDream)).OnSignal(this.stopDreaming, this.notDreaming).Update(new Action<Dreamer.Instance, float>(this.UpdateDream), UpdateRate.SIM_EVERY_TICK, false).Exit(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(this.RemoveDream));
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x000C8579 File Offset: 0x000C6779
	private void RemoveDream(Dreamer.Instance smi)
	{
		smi.SetDream(null);
		NameDisplayScreen.Instance.StopDreaming(smi.gameObject);
	}

	// Token: 0x06002275 RID: 8821 RVA: 0x000C8592 File Offset: 0x000C6792
	private void UpdateDream(Dreamer.Instance smi, float dt)
	{
		NameDisplayScreen.Instance.DreamTick(smi.gameObject, dt);
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000C85A5 File Offset: 0x000C67A5
	private static void PrepareDream(Dreamer.Instance smi)
	{
		NameDisplayScreen.Instance.SetDream(smi.gameObject, smi.currentDream);
	}

	// Token: 0x04001418 RID: 5144
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal stopDreaming;

	// Token: 0x04001419 RID: 5145
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal startDreaming;

	// Token: 0x0400141A RID: 5146
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State notDreaming;

	// Token: 0x0400141B RID: 5147
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State dreaming;

	// Token: 0x020014AA RID: 5290
	public class DreamingState : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006F27 RID: 28455
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State hidden;

		// Token: 0x04006F28 RID: 28456
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State visible;
	}

	// Token: 0x020014AB RID: 5291
	public new class Instance : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009093 RID: 37011 RVA: 0x0036ED5F File Offset: 0x0036CF5F
		public Instance(IStateMachineTarget master) : base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06009094 RID: 37012 RVA: 0x0036ED7A File Offset: 0x0036CF7A
		public void SetDream(Dream dream)
		{
			this.currentDream = dream;
		}

		// Token: 0x06009095 RID: 37013 RVA: 0x0036ED83 File Offset: 0x0036CF83
		public void StartDreaming()
		{
			base.sm.startDreaming.Trigger(base.smi);
		}

		// Token: 0x06009096 RID: 37014 RVA: 0x0036ED9B File Offset: 0x0036CF9B
		public void StopDreaming()
		{
			this.SetDream(null);
			base.sm.stopDreaming.Trigger(base.smi);
		}

		// Token: 0x04006F29 RID: 28457
		public Dream currentDream;
	}
}
