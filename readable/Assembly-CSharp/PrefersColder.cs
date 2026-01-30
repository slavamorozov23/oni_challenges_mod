using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000A66 RID: 2662
[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
	// Token: 0x06004D74 RID: 19828 RVA: 0x001C2A58 File Offset: 0x001C0C58
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001B87 RID: 7047
	public class StatesInstance : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance
	{
		// Token: 0x0600AA48 RID: 43592 RVA: 0x003C3C4E File Offset: 0x003C1E4E
		public StatesInstance(PrefersColder master) : base(master)
		{
		}
	}

	// Token: 0x02001B88 RID: 7048
	public class States : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
	{
		// Token: 0x0600AA49 RID: 43593 RVA: 0x003C3C57 File Offset: 0x003C1E57
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (PrefersColder.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x0400853F RID: 34111
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.PUDGY, DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, false, false, true);
	}
}
