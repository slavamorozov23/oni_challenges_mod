using System;

// Token: 0x02000892 RID: 2194
public class CritterElementMonitor : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06003C5F RID: 15455 RVA: 0x00151C1A File Offset: 0x0014FE1A
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateInElement", delegate(CritterElementMonitor.Instance smi, float dt)
		{
			smi.UpdateCurrentElement(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001867 RID: 6247
	public new class Instance : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06009EC9 RID: 40649 RVA: 0x003A4130 File Offset: 0x003A2330
		// (remove) Token: 0x06009ECA RID: 40650 RVA: 0x003A4168 File Offset: 0x003A2368
		public event Action<float> OnUpdateEggChances;

		// Token: 0x06009ECB RID: 40651 RVA: 0x003A419D File Offset: 0x003A239D
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009ECC RID: 40652 RVA: 0x003A41A6 File Offset: 0x003A23A6
		public void UpdateCurrentElement(float dt)
		{
			this.OnUpdateEggChances(dt);
		}
	}
}
