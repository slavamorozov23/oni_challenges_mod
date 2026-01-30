using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02000A0B RID: 2571
public class BionicOilMonitor : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>
{
	// Token: 0x06004B58 RID: 19288 RVA: 0x001B5E5C File Offset: 0x001B405C
	private static Effect CreateFreshOilEffectVariation(string id, float stressBonus, float moralBonus)
	{
		Effect effect = new Effect("FreshOil_" + id, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, DUPLICANTS.MODIFIERS.FRESHOIL.TOOLTIP, 4800f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, moralBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, stressBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		return effect;
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x001B5F08 File Offset: 0x001B4108
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.offline;
		this.root.Update(new Action<BionicOilMonitor.Instance, float>(BionicOilMonitor.OilAmountInstanceWatcherUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.offline.EventTransition(GameHashes.BionicOnline, this.online, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline)).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.online.EventTransition(GameHashes.BionicOffline, this.offline, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline))).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.AddBaseOilDeltaModifier)).DefaultState(this.online.idle).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.EnableSolidLubricationSensor)).Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.DisableSolidLubricationSensor));
		this.online.idle.EnterTransition(this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.WantsOilChange)).OnSignal(this.OilValueChanged, this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Parameter<StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter>.Callback(BionicOilMonitor.WantsOilChange));
		this.online.seeking.OnSignal(this.OilFilledSignal, this.online.idle).OnSignal(this.OilValueChanged, this.online.idle, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Parameter<StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter>.Callback(BionicOilMonitor.HasDecentAmountOfOil)).DefaultState(this.online.seeking.hasOil).ToggleThought(Db.Get().Thoughts.RefillOilDesire, null).ToggleUrge(Db.Get().Urges.OilRefill).ToggleChore((BionicOilMonitor.Instance smi) => new UseSolidLubricantChore(smi.master), this.online.idle);
		this.online.seeking.hasOil.EnterTransition(this.online.seeking.noOil, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.HasAnyAmountOfOil))).OnSignal(this.OilRanOutSignal, this.online.seeking.noOil).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWantsOilChange, null);
		this.online.seeking.noOil.Enter(delegate(BionicOilMonitor.Instance smi)
		{
			smi.currentNoLubricationEffectApplied = smi.effects.Add(smi.GetEffect(), false).effect.IdHash;
		}).Exit(delegate(BionicOilMonitor.Instance smi)
		{
			smi.effects.Remove(smi.currentNoLubricationEffectApplied);
		}).ToggleReactable(new Func<BionicOilMonitor.Instance, Reactable>(BionicOilMonitor.GrindingGearsReactable)).EventTransition(GameHashes.AssignedRoleChanged, this.online.seeking.hasOil, null);
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x001B61D5 File Offset: 0x001B43D5
	public static bool IsBionicOnline(BionicOilMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06004B5B RID: 19291 RVA: 0x001B61DD File Offset: 0x001B43DD
	public static bool HasAnyAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilMass > 0f;
	}

	// Token: 0x06004B5C RID: 19292 RVA: 0x001B61EC File Offset: 0x001B43EC
	public static bool HasDecentAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return BionicOilMonitor.HasDecentAmountOfOil(smi, null);
	}

	// Token: 0x06004B5D RID: 19293 RVA: 0x001B61F5 File Offset: 0x001B43F5
	public static bool HasDecentAmountOfOil(BionicOilMonitor.Instance smi, StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter param)
	{
		return smi.CurrentOilPercentage > 0.2f;
	}

	// Token: 0x06004B5E RID: 19294 RVA: 0x001B6204 File Offset: 0x001B4404
	public static bool WantsOilChange(BionicOilMonitor.Instance smi)
	{
		return BionicOilMonitor.WantsOilChange(smi, null);
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x001B620D File Offset: 0x001B440D
	public static bool WantsOilChange(BionicOilMonitor.Instance smi, StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter param)
	{
		return smi.CurrentOilPercentage <= 0.2f;
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x001B621F File Offset: 0x001B441F
	public static void AddBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(true);
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x001B6228 File Offset: 0x001B4428
	public static void RemoveBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(false);
	}

	// Token: 0x06004B62 RID: 19298 RVA: 0x001B6234 File Offset: 0x001B4434
	public static void OilAmountInstanceWatcherUpdate(BionicOilMonitor.Instance smi, float dt)
	{
		float lastOilAmountMassRecorded = smi.LastOilAmountMassRecorded;
		float num = smi.CurrentOilMass - lastOilAmountMassRecorded;
		if (num != 0f)
		{
			smi.LastOilAmountMassRecorded = smi.CurrentOilMass;
			if (!smi.HasOil)
			{
				smi.ReportOilRanOut();
			}
			smi.ReportOilValueChanged(num);
		}
	}

	// Token: 0x06004B63 RID: 19299 RVA: 0x001B627A File Offset: 0x001B447A
	public static void EnableSolidLubricationSensor(BionicOilMonitor.Instance smi)
	{
		smi.SetSolidLubricationSensorActiveState(true);
	}

	// Token: 0x06004B64 RID: 19300 RVA: 0x001B6283 File Offset: 0x001B4483
	public static void DisableSolidLubricationSensor(BionicOilMonitor.Instance smi)
	{
		smi.SetSolidLubricationSensorActiveState(false);
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x001B628C File Offset: 0x001B448C
	private static Reactable GrindingGearsReactable(BionicOilMonitor.Instance smi)
	{
		return smi.GetGrindingGearReactable();
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x001B6294 File Offset: 0x001B4494
	public static void ApplyLubricationEffects(Effects targetBionicEffects, SimHashes lubricant)
	{
		foreach (SimHashes simHashes in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
		{
			if (BionicOilMonitor.LUBRICANT_TYPE_EFFECT.ContainsKey(simHashes))
			{
				Effect effect = BionicOilMonitor.LUBRICANT_TYPE_EFFECT[simHashes];
				if (lubricant == simHashes)
				{
					targetBionicEffects.Add(effect, true);
				}
				else
				{
					targetBionicEffects.Remove(effect);
				}
			}
		}
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x001B631C File Offset: 0x001B451C
	// Note: this type is marked as 'beforefieldinit'.
	static BionicOilMonitor()
	{
		Dictionary<SimHashes, Effect> dictionary = new Dictionary<SimHashes, Effect>();
		dictionary[SimHashes.Tallow] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.Tallow.ToString(), -0.016666668f, 3f);
		dictionary[SimHashes.CrudeOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.CrudeOil.ToString(), -0.016666668f, 3f);
		dictionary[SimHashes.PhytoOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.PhytoOil.ToString(), -0.008333334f, 2f);
		BionicOilMonitor.LUBRICANT_TYPE_EFFECT = dictionary;
	}

	// Token: 0x040031ED RID: 12781
	public static Dictionary<SimHashes, Effect> LUBRICANT_TYPE_EFFECT;

	// Token: 0x040031EE RID: 12782
	public const float OIL_CAPACITY = 200f;

	// Token: 0x040031EF RID: 12783
	public const float OIL_TANK_DURATION = 6000f;

	// Token: 0x040031F0 RID: 12784
	public const float OIL_REFILL_TRESHOLD = 0.2f;

	// Token: 0x040031F1 RID: 12785
	public const string NO_OIL_EFFECT_NAME_MINOR = "NoLubricationMinor";

	// Token: 0x040031F2 RID: 12786
	public const string NO_OIL_EFFECT_NAME_MAJOR = "NoLubricationMajor";

	// Token: 0x040031F3 RID: 12787
	public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State offline;

	// Token: 0x040031F4 RID: 12788
	public BionicOilMonitor.OnlineStates online;

	// Token: 0x040031F5 RID: 12789
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilFilledSignal;

	// Token: 0x040031F6 RID: 12790
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilRanOutSignal;

	// Token: 0x040031F7 RID: 12791
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilValueChanged;

	// Token: 0x040031F8 RID: 12792
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OnClosestSolidLubricantChangedSignal;

	// Token: 0x02001A89 RID: 6793
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A8A RID: 6794
	public class WantsOilChangeState : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x040081F2 RID: 33266
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State hasOil;

		// Token: 0x040081F3 RID: 33267
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State noOil;
	}

	// Token: 0x02001A8B RID: 6795
	public class OnlineStates : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x040081F4 RID: 33268
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State idle;

		// Token: 0x040081F5 RID: 33269
		public BionicOilMonitor.WantsOilChangeState seeking;
	}

	// Token: 0x02001A8C RID: 6796
	public new class Instance : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.GameInstance
	{
		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x0600A5E9 RID: 42473 RVA: 0x003B884C File Offset: 0x003B6A4C
		public bool IsOnline
		{
			get
			{
				return this.batterySMI != null && this.batterySMI.IsOnline;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x0600A5EA RID: 42474 RVA: 0x003B8863 File Offset: 0x003B6A63
		public bool HasOil
		{
			get
			{
				return this.CurrentOilMass > 0f;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x0600A5EB RID: 42475 RVA: 0x003B8872 File Offset: 0x003B6A72
		public float CurrentOilPercentage
		{
			get
			{
				return this.CurrentOilMass / this.oilAmount.GetMax();
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x0600A5EC RID: 42476 RVA: 0x003B8886 File Offset: 0x003B6A86
		public float CurrentOilMass
		{
			get
			{
				if (this.oilAmount != null)
				{
					return this.oilAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600A5EE RID: 42478 RVA: 0x003B88AA File Offset: 0x003B6AAA
		// (set) Token: 0x0600A5ED RID: 42477 RVA: 0x003B88A1 File Offset: 0x003B6AA1
		public AmountInstance oilAmount { get; private set; }

		// Token: 0x0600A5EF RID: 42479 RVA: 0x003B88B4 File Offset: 0x003B6AB4
		public Instance(IStateMachineTarget master, BionicOilMonitor.Def def) : base(master, def)
		{
			this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(base.gameObject);
			this.batterySMI = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
		}

		// Token: 0x0600A5F0 RID: 42480 RVA: 0x003B8938 File Offset: 0x003B6B38
		public override void StartSM()
		{
			this.closestSolidLubricantSensor = base.GetComponent<Sensors>().GetSensor<ClosestLubricantSensor>();
			ClosestLubricantSensor closestLubricantSensor = this.closestSolidLubricantSensor;
			closestLubricantSensor.OnItemChanged = (Action<Pickupable>)Delegate.Combine(closestLubricantSensor.OnItemChanged, new Action<Pickupable>(this.OnClosestSolidLubricantChanged));
			this.LastOilAmountMassRecorded = this.CurrentOilMass;
			base.StartSM();
		}

		// Token: 0x0600A5F1 RID: 42481 RVA: 0x003B898F File Offset: 0x003B6B8F
		public string GetEffect()
		{
			if (!this.resume.HasPerk(Db.Get().SkillPerks.EfficientBionicGears))
			{
				return "NoLubricationMajor";
			}
			return "NoLubricationMinor";
		}

		// Token: 0x0600A5F2 RID: 42482 RVA: 0x003B89B8 File Offset: 0x003B6BB8
		private void ReportOilTankFilled()
		{
			base.sm.OilFilledSignal.Trigger(this);
		}

		// Token: 0x0600A5F3 RID: 42483 RVA: 0x003B89CB File Offset: 0x003B6BCB
		public void ReportOilRanOut()
		{
			base.sm.OilRanOutSignal.Trigger(this);
		}

		// Token: 0x0600A5F4 RID: 42484 RVA: 0x003B89DE File Offset: 0x003B6BDE
		public void ReportOilValueChanged(float delta)
		{
			base.sm.OilValueChanged.Trigger(this);
			Action<float> onOilValueChanged = this.OnOilValueChanged;
			if (onOilValueChanged == null)
			{
				return;
			}
			onOilValueChanged(delta);
		}

		// Token: 0x0600A5F5 RID: 42485 RVA: 0x003B8A02 File Offset: 0x003B6C02
		public void SetOilMassValue(float value)
		{
			this.oilAmount.SetValue(value);
		}

		// Token: 0x0600A5F6 RID: 42486 RVA: 0x003B8A14 File Offset: 0x003B6C14
		public void SetBaseDeltaModifierActiveState(bool isActive)
		{
			MinionModifiers component = base.GetComponent<MinionModifiers>();
			if (isActive)
			{
				bool flag = false;
				int count = component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers.Count;
				for (int i = 0; i < count; i++)
				{
					if (component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers[i] == this.BaseOilDeltaModifier)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					component.attributes.Add(this.BaseOilDeltaModifier);
					return;
				}
			}
			else
			{
				component.attributes.Remove(this.BaseOilDeltaModifier);
			}
		}

		// Token: 0x0600A5F7 RID: 42487 RVA: 0x003B8AAD File Offset: 0x003B6CAD
		public void RefillOil(float amount)
		{
			this.oilAmount.SetValue(this.CurrentOilMass + amount);
			this.ReportOilTankFilled();
		}

		// Token: 0x0600A5F8 RID: 42488 RVA: 0x003B8AC9 File Offset: 0x003B6CC9
		private void OnClosestSolidLubricantChanged(Pickupable newItem)
		{
			base.sm.OnClosestSolidLubricantChangedSignal.Trigger(this);
		}

		// Token: 0x0600A5F9 RID: 42489 RVA: 0x003B8ADC File Offset: 0x003B6CDC
		public Pickupable GetClosestSolidLubricant()
		{
			return this.closestSolidLubricantSensor.GetItem();
		}

		// Token: 0x0600A5FA RID: 42490 RVA: 0x003B8AE9 File Offset: 0x003B6CE9
		public void SetSolidLubricationSensorActiveState(bool shouldItBeActive)
		{
			this.closestSolidLubricantSensor.SetActive(shouldItBeActive);
			if (shouldItBeActive)
			{
				this.closestSolidLubricantSensor.Update();
			}
		}

		// Token: 0x0600A5FB RID: 42491 RVA: 0x003B8B08 File Offset: 0x003B6D08
		public Reactable GetGrindingGearReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, Db.Get().Emotes.Minion.GrindingGears.Id, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 10f, float.PositiveInfinity, 0f);
			Emote grindingGears = Db.Get().Emotes.Minion.GrindingGears;
			selfEmoteReactable.SetEmote(grindingGears);
			selfEmoteReactable.SetThought(Db.Get().Thoughts.RefillOilDesire);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x040081F6 RID: 33270
		public float LastOilAmountMassRecorded = -1f;

		// Token: 0x040081F7 RID: 33271
		public Action<float> OnOilValueChanged;

		// Token: 0x040081F8 RID: 33272
		private BionicBatteryMonitor.Instance batterySMI;

		// Token: 0x040081F9 RID: 33273
		[MyCmpGet]
		private MinionResume resume;

		// Token: 0x040081FA RID: 33274
		[MyCmpGet]
		public Effects effects;

		// Token: 0x040081FB RID: 33275
		public HashedString currentNoLubricationEffectApplied;

		// Token: 0x040081FC RID: 33276
		private AttributeModifier BaseOilDeltaModifier = new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, -0.033333335f, BionicMinionConfig.NAME, false, false, true);

		// Token: 0x040081FE RID: 33278
		private ClosestLubricantSensor closestSolidLubricantSensor;
	}
}
