using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000A0D RID: 2573
public class BionicSuffocationMonitor : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>
{
	// Token: 0x06004B81 RID: 19329 RVA: 0x001B6C6C File Offset: 0x001B4E6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.normal.ToggleAttributeModifier("Breathing", (BionicSuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.noOxygen, (BionicSuffocationMonitor.Instance smi) => !smi.IsBreathing());
		this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.normal, (BionicSuffocationMonitor.Instance smi) => smi.IsBreathing()).TagTransition(GameTags.RecoveringBreath, this.normal, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (BionicSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (BionicSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (BionicSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(BionicSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x04003209 RID: 12809
	public BionicSuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x0400320A RID: 12810
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State normal;

	// Token: 0x0400320B RID: 12811
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State death;

	// Token: 0x0400320C RID: 12812
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State dead;

	// Token: 0x02001A95 RID: 6805
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A96 RID: 6806
	public class NoOxygenState : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State
	{
		// Token: 0x04008223 RID: 33315
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State holdingbreath;

		// Token: 0x04008224 RID: 33316
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State suffocating;
	}

	// Token: 0x02001A97 RID: 6807
	public new class Instance : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x0600A635 RID: 42549 RVA: 0x003B9469 File Offset: 0x003B7669
		// (set) Token: 0x0600A636 RID: 42550 RVA: 0x003B9471 File Offset: 0x003B7671
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x0600A637 RID: 42551 RVA: 0x003B947C File Offset: 0x003B767C
		public Instance(IStateMachineTarget master, BionicSuffocationMonitor.Def def) : base(master, def)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x0600A638 RID: 42552 RVA: 0x003B9521 File Offset: 0x003B7721
		public bool IsBreathing()
		{
			return this.oxygenBreather.HasOxygen || base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x0600A639 RID: 42553 RVA: 0x003B9559 File Offset: 0x003B7759
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x0600A63A RID: 42554 RVA: 0x003B9570 File Offset: 0x003B7770
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x0600A63B RID: 42555 RVA: 0x003B95AA File Offset: 0x003B77AA
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x04008225 RID: 33317
		private AmountInstance breath;

		// Token: 0x04008226 RID: 33318
		public AttributeModifier breathing;

		// Token: 0x04008227 RID: 33319
		public AttributeModifier holdingbreath;
	}
}
