using System;
using Klei.AI;

// Token: 0x02000A0F RID: 2575
public class BionicWaterDamageMonitor : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>
{
	// Token: 0x06004B90 RID: 19344 RVA: 0x001B7268 File Offset: 0x001B5468
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.suffering, new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsSuffering), UpdateRate.SIM_200ms);
		this.suffering.Transition(this.safe, GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Not(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsSuffering)), UpdateRate.SIM_200ms).ToggleEffect("BionicWaterStress").ToggleReactable(new Func<BionicWaterDamageMonitor.Instance, Reactable>(BionicWaterDamageMonitor.ZapReactable));
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x001B72E2 File Offset: 0x001B54E2
	private static Reactable ZapReactable(BionicWaterDamageMonitor.Instance smi)
	{
		return smi.GetZapReactable();
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x001B72EA File Offset: 0x001B54EA
	private static bool IsSuffering(BionicWaterDamageMonitor.Instance smi)
	{
		return BionicWaterDamageMonitor.IsFloorWetWithIntolerantSubstance(smi);
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x001B72F4 File Offset: 0x001B54F4
	private static bool IsFloorWetWithIntolerantSubstance(BionicWaterDamageMonitor.Instance smi)
	{
		if (smi.master.gameObject.HasTag(GameTags.InTransitTube))
		{
			return false;
		}
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid && !smi.kpid.HasTag(GameTags.HasAirtightSuit) && smi.def.IsElementIntolerable(Grid.Element[num].id);
	}

	// Token: 0x04003214 RID: 12820
	public const string EFFECT_NAME = "BionicWaterStress";

	// Token: 0x04003215 RID: 12821
	public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State safe;

	// Token: 0x04003216 RID: 12822
	public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State suffering;

	// Token: 0x02001AA0 RID: 6816
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600A683 RID: 42627 RVA: 0x003BA418 File Offset: 0x003B8618
		public bool IsElementIntolerable(SimHashes element)
		{
			for (int i = 0; i < this.IntolerantToElements.Length; i++)
			{
				if (this.IntolerantToElements[i] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400824B RID: 33355
		public readonly SimHashes[] IntolerantToElements = new SimHashes[]
		{
			SimHashes.Water,
			SimHashes.DirtyWater,
			SimHashes.SaltWater,
			SimHashes.Brine
		};

		// Token: 0x0400824C RID: 33356
		public static float ZapInterval = 10f;
	}

	// Token: 0x02001AA1 RID: 6817
	public new class Instance : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.GameInstance
	{
		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600A686 RID: 42630 RVA: 0x003BA471 File Offset: 0x003B8671
		public bool IsAffectedByWaterDamage
		{
			get
			{
				return this.effects.HasEffect("BionicWaterStress");
			}
		}

		// Token: 0x0600A687 RID: 42631 RVA: 0x003BA483 File Offset: 0x003B8683
		public Instance(IStateMachineTarget master, BionicWaterDamageMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x0600A688 RID: 42632 RVA: 0x003BA49C File Offset: 0x003B869C
		public Reactable GetZapReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, Db.Get().Emotes.Minion.WaterDamage.Id, Db.Get().ChoreTypes.WaterDamageZap, 0f, BionicWaterDamageMonitor.Def.ZapInterval, float.PositiveInfinity, 0f);
			Emote waterDamage = Db.Get().Emotes.Minion.WaterDamage;
			selfEmoteReactable.SetEmote(waterDamage);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x0400824D RID: 33357
		public Effects effects;

		// Token: 0x0400824E RID: 33358
		[MyCmpGet]
		public KPrefabID kpid;
	}
}
