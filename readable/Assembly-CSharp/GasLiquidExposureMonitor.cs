using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000A28 RID: 2600
public class GasLiquidExposureMonitor : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>
{
	// Token: 0x06004C03 RID: 19459 RVA: 0x001B9C68 File Offset: 0x001B7E68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		this.root.Update(new Action<GasLiquidExposureMonitor.Instance, float>(this.UpdateExposure), UpdateRate.SIM_33ms, false);
		this.normal.ParamTransition<bool>(this.isIrritated, this.irritated, (GasLiquidExposureMonitor.Instance smi, bool p) => this.isIrritated.Get(smi));
		this.irritated.ParamTransition<bool>(this.isIrritated, this.normal, (GasLiquidExposureMonitor.Instance smi, bool p) => !this.isIrritated.Get(smi)).ToggleStatusItem(Db.Get().DuplicantStatusItems.GasLiquidIrritation, (GasLiquidExposureMonitor.Instance smi) => smi).DefaultState(this.irritated.irritated);
		this.irritated.irritated.Transition(this.irritated.rubbingEyes, new StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Transition.ConditionCallback(GasLiquidExposureMonitor.CanReact), UpdateRate.SIM_200ms);
		this.irritated.rubbingEyes.Exit(delegate(GasLiquidExposureMonitor.Instance smi)
		{
			smi.lastReactTime = GameClock.Instance.GetTime();
		}).ToggleReactable((GasLiquidExposureMonitor.Instance smi) => smi.GetReactable()).OnSignal(this.reactFinished, this.irritated.irritated);
	}

	// Token: 0x06004C04 RID: 19460 RVA: 0x001B9DB5 File Offset: 0x001B7FB5
	private static bool CanReact(GasLiquidExposureMonitor.Instance smi)
	{
		return GameClock.Instance.GetTime() > smi.lastReactTime + 60f;
	}

	// Token: 0x06004C05 RID: 19461 RVA: 0x001B9DD0 File Offset: 0x001B7FD0
	private static void InitializeCustomRates()
	{
		if (GasLiquidExposureMonitor.customExposureRates != null)
		{
			return;
		}
		GasLiquidExposureMonitor.minorIrritationEffect = Db.Get().effects.Get("MinorIrritation");
		GasLiquidExposureMonitor.majorIrritationEffect = Db.Get().effects.Get("MajorIrritation");
		GasLiquidExposureMonitor.customExposureRates = new Dictionary<SimHashes, float>();
		float value = -1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Water] = value;
		float value2 = -0.25f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CarbonDioxide] = value2;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen] = value2;
		float value3 = 0f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ContaminatedOxygen] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.DirtyWater] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ViscoGel] = value3;
		float value4 = 0.5f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Hydrogen] = value4;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SaltWater] = value4;
		float value5 = 1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ChlorineGas] = value5;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.EthanolGas] = value5;
		float value6 = 3f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Chlorine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SourGas] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Brine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Ethanol] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SuperCoolant] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CrudeOil] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Naphtha] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Petroleum] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Mercury] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.MercuryGas] = value6;
	}

	// Token: 0x06004C06 RID: 19462 RVA: 0x001B9F94 File Offset: 0x001B8194
	public float GetCurrentExposure(GasLiquidExposureMonitor.Instance smi)
	{
		float result;
		if (GasLiquidExposureMonitor.customExposureRates.TryGetValue(smi.CurrentlyExposedToElement().id, out result))
		{
			return result;
		}
		return 0f;
	}

	// Token: 0x06004C07 RID: 19463 RVA: 0x001B9FC4 File Offset: 0x001B81C4
	private void UpdateExposure(GasLiquidExposureMonitor.Instance smi, float dt)
	{
		GasLiquidExposureMonitor.InitializeCustomRates();
		float exposureRate = 0f;
		smi.isInAirtightEnvironment = false;
		smi.isImmuneToIrritability = false;
		int num = Grid.CellAbove(Grid.PosToCell(smi.gameObject));
		if (Grid.IsValidCell(num))
		{
			Element element = Grid.Element[num];
			float num2;
			if (!GasLiquidExposureMonitor.customExposureRates.TryGetValue(element.id, out num2))
			{
				if (Grid.Temperature[num] >= -13657.5f && Grid.Temperature[num] <= 27315f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 2f;
				}
			}
			if (smi.effects.HasImmunityTo(GasLiquidExposureMonitor.minorIrritationEffect) || smi.effects.HasImmunityTo(GasLiquidExposureMonitor.majorIrritationEffect))
			{
				smi.isImmuneToIrritability = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if ((smi.master.gameObject.HasTag(GameTags.HasSuitTank) && smi.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit()) || smi.master.gameObject.HasTag(GameTags.InTransitTube))
			{
				smi.isInAirtightEnvironment = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if (!smi.isInAirtightEnvironment && !smi.isImmuneToIrritability)
			{
				if (element.IsGas)
				{
					exposureRate = num2 * Grid.Mass[num] / 1f;
				}
				else if (element.IsLiquid)
				{
					exposureRate = num2 * Grid.Mass[num] / 1000f;
				}
			}
		}
		smi.exposureRate = exposureRate;
		smi.exposure += smi.exposureRate * dt;
		smi.exposure = MathUtil.Clamp(0f, 30f, smi.exposure);
		this.ApplyEffects(smi);
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x001BA174 File Offset: 0x001B8374
	private void ApplyEffects(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.minorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else if (smi.IsMajorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.majorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else
		{
			smi.effects.Remove(GasLiquidExposureMonitor.minorIrritationEffect);
			smi.effects.Remove(GasLiquidExposureMonitor.majorIrritationEffect);
			this.isIrritated.Set(false, smi, false);
		}
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001BA206 File Offset: 0x001B8406
	public Effect GetAppliedEffect(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			return GasLiquidExposureMonitor.minorIrritationEffect;
		}
		if (smi.IsMajorIrritation())
		{
			return GasLiquidExposureMonitor.majorIrritationEffect;
		}
		return null;
	}

	// Token: 0x0400326B RID: 12907
	public const float MIN_REACT_INTERVAL = 60f;

	// Token: 0x0400326C RID: 12908
	public const string MINOR_EFFECT_NAME = "MinorIrritation";

	// Token: 0x0400326D RID: 12909
	public const string MAJOR_EFFECT_NAME = "MajorIrritation";

	// Token: 0x0400326E RID: 12910
	private static Dictionary<SimHashes, float> customExposureRates;

	// Token: 0x0400326F RID: 12911
	private static Effect minorIrritationEffect;

	// Token: 0x04003270 RID: 12912
	private static Effect majorIrritationEffect;

	// Token: 0x04003271 RID: 12913
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.BoolParameter isIrritated;

	// Token: 0x04003272 RID: 12914
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Signal reactFinished;

	// Token: 0x04003273 RID: 12915
	public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State normal;

	// Token: 0x04003274 RID: 12916
	public GasLiquidExposureMonitor.IrritatedStates irritated;

	// Token: 0x02001AE4 RID: 6884
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AE5 RID: 6885
	public class TUNING
	{
		// Token: 0x0400830F RID: 33551
		public const float MINOR_IRRITATION_THRESHOLD = 8f;

		// Token: 0x04008310 RID: 33552
		public const float MAJOR_IRRITATION_THRESHOLD = 15f;

		// Token: 0x04008311 RID: 33553
		public const float MAX_EXPOSURE = 30f;

		// Token: 0x04008312 RID: 33554
		public const float GAS_UNITS = 1f;

		// Token: 0x04008313 RID: 33555
		public const float LIQUID_UNITS = 1000f;

		// Token: 0x04008314 RID: 33556
		public const float REDUCE_EXPOSURE_RATE_FAST = -1f;

		// Token: 0x04008315 RID: 33557
		public const float REDUCE_EXPOSURE_RATE_SLOW = -0.25f;

		// Token: 0x04008316 RID: 33558
		public const float NO_CHANGE = 0f;

		// Token: 0x04008317 RID: 33559
		public const float SLOW_EXPOSURE_RATE = 0.5f;

		// Token: 0x04008318 RID: 33560
		public const float NORMAL_EXPOSURE_RATE = 1f;

		// Token: 0x04008319 RID: 33561
		public const float QUICK_EXPOSURE_RATE = 3f;

		// Token: 0x0400831A RID: 33562
		public const float DEFAULT_MIN_TEMPERATURE = -13657.5f;

		// Token: 0x0400831B RID: 33563
		public const float DEFAULT_MAX_TEMPERATURE = 27315f;

		// Token: 0x0400831C RID: 33564
		public const float DEFAULT_LOW_RATE = 1f;

		// Token: 0x0400831D RID: 33565
		public const float DEFAULT_HIGH_RATE = 2f;
	}

	// Token: 0x02001AE6 RID: 6886
	public class IrritatedStates : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State
	{
		// Token: 0x0400831E RID: 33566
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State irritated;

		// Token: 0x0400831F RID: 33567
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State rubbingEyes;
	}

	// Token: 0x02001AE7 RID: 6887
	public new class Instance : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.GameInstance
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600A785 RID: 42885 RVA: 0x003BC7F3 File Offset: 0x003BA9F3
		public float minorIrritationThreshold
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x0600A786 RID: 42886 RVA: 0x003BC7FA File Offset: 0x003BA9FA
		public Instance(IStateMachineTarget master, GasLiquidExposureMonitor.Def def) : base(master, def)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x0600A787 RID: 42887 RVA: 0x003BC810 File Offset: 0x003BAA10
		public Reactable GetReactable()
		{
			Emote iritatedEyes = Db.Get().Emotes.Minion.IritatedEyes;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "IrritatedEyes", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(iritatedEyes);
			selfEmoteReactable.preventChoreInterruption = true;
			selfEmoteReactable.RegisterEmoteStepCallbacks("irritated_eyes", null, delegate(GameObject go)
			{
				base.sm.reactFinished.Trigger(this);
			});
			return selfEmoteReactable;
		}

		// Token: 0x0600A788 RID: 42888 RVA: 0x003BC89C File Offset: 0x003BAA9C
		public bool IsMinorIrritation()
		{
			return this.exposure >= 8f && this.exposure < 15f;
		}

		// Token: 0x0600A789 RID: 42889 RVA: 0x003BC8BA File Offset: 0x003BAABA
		public bool IsMajorIrritation()
		{
			return this.exposure >= 15f;
		}

		// Token: 0x0600A78A RID: 42890 RVA: 0x003BC8CC File Offset: 0x003BAACC
		public Element CurrentlyExposedToElement()
		{
			if (this.isInAirtightEnvironment)
			{
				return ElementLoader.GetElement(SimHashes.Oxygen.CreateTag());
			}
			int num = Grid.CellAbove(Grid.PosToCell(base.smi.gameObject));
			return Grid.Element[num];
		}

		// Token: 0x0600A78B RID: 42891 RVA: 0x003BC90E File Offset: 0x003BAB0E
		public void ResetExposure()
		{
			this.exposure = 0f;
		}

		// Token: 0x04008320 RID: 33568
		[Serialize]
		public float exposure;

		// Token: 0x04008321 RID: 33569
		[Serialize]
		public float lastReactTime;

		// Token: 0x04008322 RID: 33570
		[Serialize]
		public float exposureRate;

		// Token: 0x04008323 RID: 33571
		public Effects effects;

		// Token: 0x04008324 RID: 33572
		public bool isInAirtightEnvironment;

		// Token: 0x04008325 RID: 33573
		public bool isImmuneToIrritability;
	}
}
