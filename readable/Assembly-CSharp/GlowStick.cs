using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000975 RID: 2421
[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
	// Token: 0x06004516 RID: 17686 RVA: 0x00190619 File Offset: 0x0018E819
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020019B2 RID: 6578
	public class StatesInstance : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick, object>.GameInstance
	{
		// Token: 0x0600A2E7 RID: 41703 RVA: 0x003B0B68 File Offset: 0x003AED68
		public StatesInstance(GlowStick master) : base(master)
		{
			this._radiationEmitter.emitRads = 100f;
			this._radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			this._radiationEmitter.emitRate = 0.5f;
			this._radiationEmitter.emitRadiusX = 3;
			this._radiationEmitter.emitRadiusY = 3;
			this.radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TRAITS.GLOWSTICK_RADIATION_RESISTANCE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
			this.luminescenceModifier = new AttributeModifier(Db.Get().Attributes.Luminescence.Id, TRAITS.GLOWSTICK_LUX_VALUE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
		}

		// Token: 0x04007F1C RID: 32540
		[MyCmpAdd]
		private RadiationEmitter _radiationEmitter;

		// Token: 0x04007F1D RID: 32541
		public AttributeModifier radiationResistance;

		// Token: 0x04007F1E RID: 32542
		public AttributeModifier luminescenceModifier;
	}

	// Token: 0x020019B3 RID: 6579
	public class States : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick>
	{
		// Token: 0x0600A2E8 RID: 41704 RVA: 0x003B0C24 File Offset: 0x003AEE24
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleComponent<RadiationEmitter>(false).ToggleAttributeModifier("Radiation Resistance", (GlowStick.StatesInstance smi) => smi.radiationResistance, null).ToggleAttributeModifier("Luminescence Modifier", (GlowStick.StatesInstance smi) => smi.luminescenceModifier, null);
		}
	}
}
