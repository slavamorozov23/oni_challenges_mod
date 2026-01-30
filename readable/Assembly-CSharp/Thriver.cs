using System;

// Token: 0x02000BF5 RID: 3061
[SkipSaveFileSerialization]
public class Thriver : StateMachineComponent<Thriver.StatesInstance>
{
	// Token: 0x06005BE1 RID: 23521 RVA: 0x002141A7 File Offset: 0x002123A7
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001D8C RID: 7564
	public class StatesInstance : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.GameInstance
	{
		// Token: 0x0600B16B RID: 45419 RVA: 0x003DD5A4 File Offset: 0x003DB7A4
		public StatesInstance(Thriver master) : base(master)
		{
		}

		// Token: 0x0600B16C RID: 45420 RVA: 0x003DD5B0 File Offset: 0x003DB7B0
		public bool IsStressed()
		{
			StressMonitor.Instance smi = base.master.GetSMI<StressMonitor.Instance>();
			return smi != null && smi.IsStressed();
		}
	}

	// Token: 0x02001D8D RID: 7565
	public class States : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver>
	{
		// Token: 0x0600B16D RID: 45421 RVA: 0x003DD5D4 File Offset: 0x003DB7D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.EventTransition(GameHashes.NotStressed, this.idle, null).EventTransition(GameHashes.Stressed, this.stressed, null).EventTransition(GameHashes.StressedHadEnough, this.stressed, null).Enter(delegate(Thriver.StatesInstance smi)
			{
				StressMonitor.Instance smi2 = smi.master.GetSMI<StressMonitor.Instance>();
				if (smi2 != null && smi2.IsStressed())
				{
					smi.GoTo(this.stressed);
				}
			});
			this.idle.DoNothing();
			this.stressed.ToggleEffect("Thriver");
			this.toostressed.DoNothing();
		}

		// Token: 0x04008B95 RID: 35733
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State idle;

		// Token: 0x04008B96 RID: 35734
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State stressed;

		// Token: 0x04008B97 RID: 35735
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State toostressed;
	}
}
