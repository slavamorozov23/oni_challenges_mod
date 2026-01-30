using System;
using KSerialization;

// Token: 0x020007FF RID: 2047
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidLogicValve : StateMachineComponent<SolidLogicValve.StatesInstance>
{
	// Token: 0x060036EA RID: 14058 RVA: 0x00135484 File Offset: 0x00133684
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x0013548C File Offset: 0x0013368C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x0013549F File Offset: 0x0013369F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04002155 RID: 8533
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002156 RID: 8534
	[MyCmpReq]
	private SolidConduitBridge bridge;

	// Token: 0x0200177D RID: 6013
	public class States : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve>
	{
		// Token: 0x06009B55 RID: 39765 RVA: 0x0039471C File Offset: 0x0039291C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidLogicValve.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidLogicValve.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
			this.on.idle.PlayAnim("on").Transition(this.on.working, (SolidLogicValve.StatesInstance smi) => smi.IsDispensing(), UpdateRate.SIM_200ms);
			this.on.working.PlayAnim("on_flow", KAnim.PlayMode.Loop).Transition(this.on.idle, (SolidLogicValve.StatesInstance smi) => !smi.IsDispensing(), UpdateRate.SIM_200ms);
		}

		// Token: 0x040077D7 RID: 30679
		public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State off;

		// Token: 0x040077D8 RID: 30680
		public SolidLogicValve.States.ReadyStates on;

		// Token: 0x02002942 RID: 10562
		public class ReadyStates : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State
		{
			// Token: 0x0400B682 RID: 46722
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State idle;

			// Token: 0x0400B683 RID: 46723
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State working;
		}
	}

	// Token: 0x0200177E RID: 6014
	public class StatesInstance : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.GameInstance
	{
		// Token: 0x06009B57 RID: 39767 RVA: 0x003948A0 File Offset: 0x00392AA0
		public StatesInstance(SolidLogicValve master) : base(master)
		{
		}

		// Token: 0x06009B58 RID: 39768 RVA: 0x003948A9 File Offset: 0x00392AA9
		public bool IsDispensing()
		{
			return base.master.bridge.IsDispensing;
		}
	}
}
