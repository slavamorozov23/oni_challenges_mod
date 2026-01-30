using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A3C RID: 2620
public class RadiationMonitor : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance>
{
	// Token: 0x06004C7A RID: 19578 RVA: 0x001BC1DC File Offset: 0x001BA3DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.init;
		this.init.Transition(null, (RadiationMonitor.Instance smi) => !Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms).Transition(this.active, (RadiationMonitor.Instance smi) => Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms);
		this.active.Update(new Action<RadiationMonitor.Instance, float>(RadiationMonitor.CheckRadiationLevel), UpdateRate.SIM_1000ms, false).DefaultState(this.active.idle);
		this.active.idle.DoNothing().ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ParamTransition<float>(this.radiationExposure, this.active.sick.minor, RadiationMonitor.COMPARE_GTE_MINOR);
		this.active.sick.ParamTransition<float>(this.radiationExposure, this.active.idle, RadiationMonitor.COMPARE_LT_MINOR).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(true, smi, false);
		}).Exit(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(false, smi, false);
		});
		this.active.sick.minor.ToggleEffect(delegate(RadiationMonitor.Instance smi)
		{
			if (!smi.master.gameObject.HasTag(GameTags.Minions.Models.Bionic))
			{
				return RadiationMonitor.minorSicknessEffect;
			}
			return RadiationMonitor.bionic_minorSicknessEffect;
		}).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ToggleAnims("anim_loco_radiation1_kanim", 4f).ToggleAnims("anim_idle_radiation1_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation1, null).DefaultState(this.active.sick.minor.waiting);
		this.active.sick.minor.reacting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.minor.waiting);
		this.active.sick.major.ToggleEffect(delegate(RadiationMonitor.Instance smi)
		{
			if (!smi.master.gameObject.HasTag(GameTags.Minions.Models.Bionic))
			{
				return RadiationMonitor.majorSicknessEffect;
			}
			return RadiationMonitor.bionic_majorSicknessEffect;
		}).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ToggleAnims("anim_loco_radiation2_kanim", 4f).ToggleAnims("anim_idle_radiation2_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation2, null).DefaultState(this.active.sick.major.waiting);
		this.active.sick.major.waiting.ScheduleGoTo(120f, this.active.sick.major.vomiting);
		this.active.sick.major.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.major.waiting);
		this.active.sick.extreme.ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ToggleEffect(delegate(RadiationMonitor.Instance smi)
		{
			if (!smi.master.gameObject.HasTag(GameTags.Minions.Models.Bionic))
			{
				return RadiationMonitor.extremeSicknessEffect;
			}
			return RadiationMonitor.bionic_extremeSicknessEffect;
		}).ToggleAnims("anim_loco_radiation3_kanim", 4f).ToggleAnims("anim_idle_radiation3_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation3, null).DefaultState(this.active.sick.extreme.waiting);
		this.active.sick.extreme.waiting.ScheduleGoTo(60f, this.active.sick.extreme.vomiting);
		this.active.sick.extreme.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.extreme.waiting);
		this.active.sick.deadly.ToggleAnims("anim_loco_radiation4_kanim", 4f).ToggleAnims("anim_idle_radiation4_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation4, null).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_NO_LONGER_DEADLY).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.GetComponent<Health>().Incapacitate(GameTags.RadiationSicknessIncapacitation);
		});
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x001BC768 File Offset: 0x001BA968
	private Chore CreateVomitChore(RadiationMonitor.Instance smi)
	{
		Notification notification = new Notification(DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		return new VomitChore(Db.Get().ChoreTypes.Vomit, smi.master, Db.Get().DuplicantStatusItems.Vomiting, notification, null);
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x001BC7E0 File Offset: 0x001BA9E0
	private static void RadiationRecovery(RadiationMonitor.Instance smi, float dt)
	{
		float num = Db.Get().Attributes.RadiationRecovery.Lookup(smi.gameObject).GetTotalValue() * dt;
		smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num);
		smi.master.Trigger(1556680150, num);
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x001BC850 File Offset: 0x001BAA50
	private static void CheckRadiationLevel(RadiationMonitor.Instance smi, float dt)
	{
		RadiationMonitor.RadiationRecovery(smi, dt);
		smi.sm.timeUntilNextExposureReact.Delta(-dt, smi);
		smi.sm.timeUntilNextSickReact.Delta(-dt, smi);
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(smi.gameObject).GetTotalValue());
			float num3 = Grid.Radiation[num] * 1f * num2 / 600f * dt;
			smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num3);
			float num4 = num3 / dt * 600f;
			smi.sm.currentExposurePerCycle.Set(num4, smi, false);
			if (smi.sm.timeUntilNextExposureReact.Get(smi) <= 0f && !smi.HasTag(GameTags.InTransitTube) && RadiationMonitor.COMPARE_REACT(smi, num4))
			{
				smi.sm.timeUntilNextExposureReact.Set(120f, smi, false);
				Emote radiation_Glare = Db.Get().Emotes.Minion.Radiation_Glare;
				smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Glare, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 20f, float.NegativeInfinity, 0f, null);
			}
		}
		if (smi.sm.timeUntilNextSickReact.Get(smi) <= 0f && smi.sm.isSick.Get(smi) && !smi.HasTag(GameTags.InTransitTube))
		{
			smi.sm.timeUntilNextSickReact.Set(60f, smi, false);
			Emote radiation_Itch = Db.Get().Emotes.Minion.Radiation_Itch;
			smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Itch, true, Db.Get().ChoreTypes.RadiationPain, 0f, 20f, float.NegativeInfinity, 0f, null);
		}
		smi.sm.radiationExposure.Set(smi.master.gameObject.GetComponent<KSelectable>().GetAmounts().GetValue("RadiationBalance"), smi, false);
	}

	// Token: 0x040032C4 RID: 12996
	public const float BASE_ABSORBTION_RATE = 1f;

	// Token: 0x040032C5 RID: 12997
	public const float MIN_TIME_BETWEEN_EXPOSURE_REACTS = 120f;

	// Token: 0x040032C6 RID: 12998
	public const float MIN_TIME_BETWEEN_SICK_REACTS = 60f;

	// Token: 0x040032C7 RID: 12999
	public const int VOMITS_PER_CYCLE_MAJOR = 5;

	// Token: 0x040032C8 RID: 13000
	public const int VOMITS_PER_CYCLE_EXTREME = 10;

	// Token: 0x040032C9 RID: 13001
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter radiationExposure;

	// Token: 0x040032CA RID: 13002
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter currentExposurePerCycle;

	// Token: 0x040032CB RID: 13003
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSick;

	// Token: 0x040032CC RID: 13004
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextExposureReact;

	// Token: 0x040032CD RID: 13005
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextSickReact;

	// Token: 0x040032CE RID: 13006
	public static string minorSicknessEffect = "RadiationExposureMinor";

	// Token: 0x040032CF RID: 13007
	public static string majorSicknessEffect = "RadiationExposureMajor";

	// Token: 0x040032D0 RID: 13008
	public static string extremeSicknessEffect = "RadiationExposureExtreme";

	// Token: 0x040032D1 RID: 13009
	public static string bionic_minorSicknessEffect = "BionicRadiationExposureMinor";

	// Token: 0x040032D2 RID: 13010
	public static string bionic_majorSicknessEffect = "BionicRadiationExposureMajor";

	// Token: 0x040032D3 RID: 13011
	public static string bionic_extremeSicknessEffect = "BionicRadiationExposureExtreme";

	// Token: 0x040032D4 RID: 13012
	public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State init;

	// Token: 0x040032D5 RID: 13013
	public RadiationMonitor.ActiveStates active;

	// Token: 0x040032D6 RID: 13014
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_RECOVERY_IMMEDIATE = (RadiationMonitor.Instance smi, float p) => p > 100f * smi.difficultySettingMod / 2f;

	// Token: 0x040032D7 RID: 13015
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_REACT = (RadiationMonitor.Instance smi, float p) => p >= 133f * smi.difficultySettingMod;

	// Token: 0x040032D8 RID: 13016
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_LT_MINOR = (RadiationMonitor.Instance smi, float p) => p < 100f * smi.difficultySettingMod;

	// Token: 0x040032D9 RID: 13017
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MINOR = (RadiationMonitor.Instance smi, float p) => p >= 100f * smi.difficultySettingMod;

	// Token: 0x040032DA RID: 13018
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MAJOR = (RadiationMonitor.Instance smi, float p) => p >= 300f * smi.difficultySettingMod;

	// Token: 0x040032DB RID: 13019
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_EXTREME = (RadiationMonitor.Instance smi, float p) => p >= 600f * smi.difficultySettingMod;

	// Token: 0x040032DC RID: 13020
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_DEADLY = (RadiationMonitor.Instance smi, float p) => p >= 900f * smi.difficultySettingMod;

	// Token: 0x040032DD RID: 13021
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_NO_LONGER_DEADLY = (RadiationMonitor.Instance smi, float p) => p < 900f * smi.difficultySettingMod;

	// Token: 0x02001B18 RID: 6936
	public class ActiveStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040083B1 RID: 33713
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040083B2 RID: 33714
		public RadiationMonitor.SickStates sick;
	}

	// Token: 0x02001B19 RID: 6937
	public class SickStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040083B3 RID: 33715
		public RadiationMonitor.SickStates.MinorStates minor;

		// Token: 0x040083B4 RID: 33716
		public RadiationMonitor.SickStates.MajorStates major;

		// Token: 0x040083B5 RID: 33717
		public RadiationMonitor.SickStates.ExtremeStates extreme;

		// Token: 0x040083B6 RID: 33718
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State deadly;

		// Token: 0x020029DE RID: 10718
		public class MinorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400B919 RID: 47385
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x0400B91A RID: 47386
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State reacting;
		}

		// Token: 0x020029DF RID: 10719
		public class MajorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400B91B RID: 47387
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x0400B91C RID: 47388
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}

		// Token: 0x020029E0 RID: 10720
		public class ExtremeStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400B91D RID: 47389
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x0400B91E RID: 47390
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}
	}

	// Token: 0x02001B1A RID: 6938
	public new class Instance : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A85A RID: 43098 RVA: 0x003BEC54 File Offset: 0x003BCE54
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			if (Sim.IsRadiationEnabled())
			{
				SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Radiation);
				if (currentQualitySetting != null)
				{
					string id = currentQualitySetting.id;
					if (id == "Easiest")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIEST;
						return;
					}
					if (id == "Easier")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIER;
						return;
					}
					if (id == "Harder")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDER;
						return;
					}
					if (!(id == "Hardest"))
					{
						return;
					}
					this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDEST;
				}
			}
		}

		// Token: 0x0600A85B RID: 43099 RVA: 0x003BED04 File Offset: 0x003BCF04
		public float SicknessSecondsRemaining()
		{
			return 600f * (Mathf.Max(0f, base.sm.radiationExposure.Get(base.smi) - 100f * this.difficultySettingMod) / 100f);
		}

		// Token: 0x0600A85C RID: 43100 RVA: 0x003BED40 File Offset: 0x003BCF40
		public string GetEffectStatusTooltip()
		{
			if (this.effects.HasEffect(RadiationMonitor.minorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.minorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.minorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.majorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.majorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.majorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.extremeSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.extremeSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.extremeSicknessEffect));
			}
			return DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.TOOLTIP;
		}

		// Token: 0x040083B7 RID: 33719
		public Effects effects;

		// Token: 0x040083B8 RID: 33720
		public float difficultySettingMod = 1f;
	}
}
