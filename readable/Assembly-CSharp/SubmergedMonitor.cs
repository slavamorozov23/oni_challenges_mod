using System;

// Token: 0x020008BC RID: 2236
public class SubmergedMonitor : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>
{
	// Token: 0x06003D9F RID: 15775 RVA: 0x00157F00 File Offset: 0x00156100
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Enter("SetNavType", delegate(SubmergedMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover);
		}).Update("SetNavType", delegate(SubmergedMonitor.Instance smi, float dt)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover);
		}, UpdateRate.SIM_1000ms, false).Transition(this.submerged, (SubmergedMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_1000ms);
		this.submerged.Enter("SetNavType", delegate(SubmergedMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim);
		}).Update("SetNavType", delegate(SubmergedMonitor.Instance smi, float dt)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim);
		}, UpdateRate.SIM_1000ms, false).Transition(this.satisfied, (SubmergedMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Submerged);
	}

	// Token: 0x04002609 RID: 9737
	public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State satisfied;

	// Token: 0x0400260A RID: 9738
	public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State submerged;

	// Token: 0x020018CF RID: 6351
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018D0 RID: 6352
	public new class Instance : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.GameInstance
	{
		// Token: 0x0600A058 RID: 41048 RVA: 0x003A9920 File Offset: 0x003A7B20
		public Instance(IStateMachineTarget master, SubmergedMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A059 RID: 41049 RVA: 0x003A992A File Offset: 0x003A7B2A
		public bool IsSubmerged()
		{
			return Grid.IsSubstantialLiquid(Grid.PosToCell(base.transform.GetPosition()), 0.35f);
		}
	}
}
