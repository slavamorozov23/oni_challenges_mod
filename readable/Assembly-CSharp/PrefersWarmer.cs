using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000A67 RID: 2663
[SkipSaveFileSerialization]
public class PrefersWarmer : StateMachineComponent<PrefersWarmer.StatesInstance>
{
	// Token: 0x06004D76 RID: 19830 RVA: 0x001C2A6D File Offset: 0x001C0C6D
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001B89 RID: 7049
	public class StatesInstance : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer, object>.GameInstance
	{
		// Token: 0x0600AA4C RID: 43596 RVA: 0x003C3CC5 File Offset: 0x003C1EC5
		public StatesInstance(PrefersWarmer master) : base(master)
		{
		}
	}

	// Token: 0x02001B8A RID: 7050
	public class States : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer>
	{
		// Token: 0x0600AA4D RID: 43597 RVA: 0x003C3CCE File Offset: 0x003C1ECE
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (PrefersWarmer.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04008540 RID: 34112
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.SKINNY, DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, false, false, true);
	}
}
