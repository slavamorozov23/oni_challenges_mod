using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
public class MercuryLight : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>
{
	// Token: 0x06003431 RID: 13361 RVA: 0x00128028 File Offset: 0x00126228
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<float>(this.Charge, this.noOperational.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.noOperational.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.noOperational.depleating.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.noOperational.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.noOperational.depleated.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.idle.TagTransition(GameTags.Operational, this.noOperational.exit, false).PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null);
		this.noOperational.exit.PlayAnim("on_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.darkness).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ConsumeFuelUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<bool>(this.HasEnoughFuel, this.operational.light, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsTrue).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.operational.darkness.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.operational.darkness.depleating.PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.depleated.PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.darkness.idle);
		this.operational.darkness.idle.PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero);
		this.operational.light.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOn)).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.HasEnoughFuel, this.operational.darkness, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).DefaultState(this.operational.light.charging);
		this.operational.light.charging.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charging, null).ParamTransition<float>(this.Charge, this.operational.light.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTEOne).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ChargeUpdate), UpdateRate.SIM_200ms, false);
		this.operational.light.idle.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charged, null).ParamTransition<float>(this.Charge, this.operational.light.charging, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTOne);
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x00128468 File Offset: 0x00126668
	public static void SetOperationalActiveFlagOn(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x00128477 File Offset: 0x00126677
	public static void SetOperationalActiveFlagOff(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x00128486 File Offset: 0x00126686
	public static void DepleteUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.DepleteUpdate(dt);
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x0012848F File Offset: 0x0012668F
	public static void ChargeUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ChargeUpdate(dt);
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x00128498 File Offset: 0x00126698
	public static void ConsumeFuelUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ConsumeFuelUpdate(dt);
	}

	// Token: 0x04001F80 RID: 8064
	private static Tag ELEMENT_TAG = SimHashes.Mercury.CreateTag();

	// Token: 0x04001F81 RID: 8065
	private const string ON_ANIM_NAME = "on";

	// Token: 0x04001F82 RID: 8066
	private const string ON_PRE_ANIM_NAME = "on_pre";

	// Token: 0x04001F83 RID: 8067
	private const string TRANSITION_TO_OFF_ANIM_NAME = "on_pst";

	// Token: 0x04001F84 RID: 8068
	private const string DEPLEATING_ANIM_NAME = "depleating";

	// Token: 0x04001F85 RID: 8069
	private const string OFF_ANIM_NAME = "off";

	// Token: 0x04001F86 RID: 8070
	private const string LIGHT_LEVEL_METER_TARGET_NAME = "meter_target";

	// Token: 0x04001F87 RID: 8071
	private const string LIGHT_LEVEL_METER_ANIM_NAME = "meter";

	// Token: 0x04001F88 RID: 8072
	public MercuryLight.Darknesstates noOperational;

	// Token: 0x04001F89 RID: 8073
	public MercuryLight.OperationalStates operational;

	// Token: 0x04001F8A RID: 8074
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.FloatParameter Charge;

	// Token: 0x04001F8B RID: 8075
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.BoolParameter HasEnoughFuel;

	// Token: 0x020016EA RID: 5866
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009925 RID: 39205 RVA: 0x0038BDD0 File Offset: 0x00389FD0
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			string arg = MercuryLight.ELEMENT_TAG.ProperName();
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
			return list;
		}

		// Token: 0x04007629 RID: 30249
		public float MAX_LUX;

		// Token: 0x0400762A RID: 30250
		public float TURN_ON_DELAY;

		// Token: 0x0400762B RID: 30251
		public float FUEL_MASS_PER_SECOND;
	}

	// Token: 0x020016EB RID: 5867
	public class LightStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x0400762C RID: 30252
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State charging;

		// Token: 0x0400762D RID: 30253
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;
	}

	// Token: 0x020016EC RID: 5868
	public class Darknesstates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x0400762E RID: 30254
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleating;

		// Token: 0x0400762F RID: 30255
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleated;

		// Token: 0x04007630 RID: 30256
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;

		// Token: 0x04007631 RID: 30257
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State exit;
	}

	// Token: 0x020016ED RID: 5869
	public class OperationalStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04007632 RID: 30258
		public MercuryLight.LightStates light;

		// Token: 0x04007633 RID: 30259
		public MercuryLight.Darknesstates darkness;
	}

	// Token: 0x020016EE RID: 5870
	public new class Instance : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.GameInstance
	{
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x0600992A RID: 39210 RVA: 0x0038BE63 File Offset: 0x0038A063
		public bool HasEnoughFuel
		{
			get
			{
				return base.sm.HasEnoughFuel.Get(this);
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x0600992B RID: 39211 RVA: 0x0038BE76 File Offset: 0x0038A076
		public int LuxLevel
		{
			get
			{
				return Mathf.FloorToInt(base.smi.ChargeLevel * base.def.MAX_LUX);
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x0600992C RID: 39212 RVA: 0x0038BE95 File Offset: 0x0038A095
		public float ChargeLevel
		{
			get
			{
				return base.smi.sm.Charge.Get(this);
			}
		}

		// Token: 0x0600992D RID: 39213 RVA: 0x0038BEB0 File Offset: 0x0038A0B0
		public Instance(IStateMachineTarget master, MercuryLight.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.lightIntensityMeterController = new MeterController(component, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
		}

		// Token: 0x0600992E RID: 39214 RVA: 0x0038BEEA File Offset: 0x0038A0EA
		public override void StartSM()
		{
			base.StartSM();
			this.SetChargeLevel(this.ChargeLevel);
		}

		// Token: 0x0600992F RID: 39215 RVA: 0x0038BF00 File Offset: 0x0038A100
		public void DepleteUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel - dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06009930 RID: 39216 RVA: 0x0038BF38 File Offset: 0x0038A138
		public void ChargeUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel + dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06009931 RID: 39217 RVA: 0x0038BF70 File Offset: 0x0038A170
		public void SetChargeLevel(float value)
		{
			base.sm.Charge.Set(value, this, false);
			this.light.Lux = this.LuxLevel;
			this.light.FullRefresh();
			bool flag = this.ChargeLevel > 0f;
			if (this.light.enabled != flag)
			{
				this.light.enabled = flag;
			}
			this.lightIntensityMeterController.SetPositionPercent(value);
		}

		// Token: 0x06009932 RID: 39218 RVA: 0x0038BFE4 File Offset: 0x0038A1E4
		public void ConsumeFuelUpdate(float dt)
		{
			float num = base.def.FUEL_MASS_PER_SECOND * dt;
			if (this.storage.MassStored() < num)
			{
				base.sm.HasEnoughFuel.Set(false, this, false);
				return;
			}
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float num3;
			this.storage.ConsumeAndGetDisease(MercuryLight.ELEMENT_TAG, num, out num2, out diseaseInfo, out num3);
			base.sm.HasEnoughFuel.Set(true, this, false);
		}

		// Token: 0x06009933 RID: 39219 RVA: 0x0038C04D File Offset: 0x0038A24D
		public bool CanRun()
		{
			return true;
		}

		// Token: 0x04007634 RID: 30260
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04007635 RID: 30261
		[MyCmpGet]
		private Light2D light;

		// Token: 0x04007636 RID: 30262
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04007637 RID: 30263
		[MyCmpGet]
		private ConduitConsumer conduitConsumer;

		// Token: 0x04007638 RID: 30264
		private MeterController lightIntensityMeterController;
	}
}
