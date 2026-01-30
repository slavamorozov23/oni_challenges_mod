using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000A33 RID: 2611
public class LightMonitor : GameStateMachine<LightMonitor, LightMonitor.Instance>
{
	// Token: 0x06004C3D RID: 19517 RVA: 0x001BB114 File Offset: 0x001B9314
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unburnt;
		this.root.EventTransition(GameHashes.SicknessAdded, this.burnt, (LightMonitor.Instance smi) => smi.gameObject.GetSicknesses().Has(Db.Get().Sicknesses.Sunburn)).Update(new Action<LightMonitor.Instance, float>(LightMonitor.CheckLightLevel), UpdateRate.SIM_1000ms, false);
		this.unburnt.DefaultState(this.unburnt.safe).ParamTransition<float>(this.burnResistance, this.get_burnt, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsLTEZero);
		this.unburnt.safe.DefaultState(this.unburnt.safe.unlit).Update(delegate(LightMonitor.Instance smi, float dt)
		{
			smi.sm.burnResistance.DeltaClamp(dt * 0.25f, 0f, DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME, smi);
		}, UpdateRate.SIM_200ms, false);
		this.unburnt.safe.unlit.ParamTransition<float>(this.lightLevel, this.unburnt.safe.normal_light, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.unburnt.safe.normal_light.ParamTransition<float>(this.lightLevel, this.unburnt.safe.unlit, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ParamTransition<float>(this.lightLevel, this.unburnt.safe.sunlight, (LightMonitor.Instance smi, float p) => p >= (float)DUPLICANTSTATS.STANDARD.Light.LUX_PLEASANT_LIGHT);
		this.unburnt.safe.sunlight.ParamTransition<float>(this.lightLevel, this.unburnt.safe.normal_light, (LightMonitor.Instance smi, float p) => p < (float)DUPLICANTSTATS.STANDARD.Light.LUX_PLEASANT_LIGHT).ParamTransition<float>(this.lightLevel, this.unburnt.burning, (LightMonitor.Instance smi, float p) => p >= (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN).ToggleEffect("Sunlight_Pleasant");
		this.unburnt.burning.ParamTransition<float>(this.lightLevel, this.unburnt.safe.sunlight, (LightMonitor.Instance smi, float p) => p < (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN).Update(delegate(LightMonitor.Instance smi, float dt)
		{
			smi.sm.burnResistance.DeltaClamp(-dt, 0f, DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME, smi);
		}, UpdateRate.SIM_200ms, false).ToggleEffect("Sunlight_Burning");
		this.get_burnt.Enter(delegate(LightMonitor.Instance smi)
		{
			smi.gameObject.GetSicknesses().Infect(new SicknessExposureInfo(Db.Get().Sicknesses.Sunburn.Id, DUPLICANTS.DISEASES.SUNBURNSICKNESS.SUNEXPOSURE));
		}).GoTo(this.burnt);
		this.burnt.EventTransition(GameHashes.SicknessCured, this.unburnt, (LightMonitor.Instance smi) => !smi.gameObject.GetSicknesses().Has(Db.Get().Sicknesses.Sunburn)).Exit(delegate(LightMonitor.Instance smi)
		{
			smi.sm.burnResistance.Set(DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME, smi, false);
		});
	}

	// Token: 0x06004C3E RID: 19518 RVA: 0x001BB414 File Offset: 0x001B9614
	private static void CheckLightLevel(LightMonitor.Instance smi, float dt)
	{
		KPrefabID component = smi.GetComponent<KPrefabID>();
		if (component != null && component.HasTag(GameTags.Shaded))
		{
			smi.sm.lightLevel.Set(0f, smi, false);
			return;
		}
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			smi.sm.lightLevel.Set((float)Grid.LightIntensity[num], smi, false);
		}
	}

	// Token: 0x040032A4 RID: 12964
	public const float BURN_RESIST_RECOVERY_FACTOR = 0.25f;

	// Token: 0x040032A5 RID: 12965
	public StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter lightLevel;

	// Token: 0x040032A6 RID: 12966
	public StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter burnResistance = new StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter(DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME);

	// Token: 0x040032A7 RID: 12967
	public LightMonitor.UnburntStates unburnt;

	// Token: 0x040032A8 RID: 12968
	public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State get_burnt;

	// Token: 0x040032A9 RID: 12969
	public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State burnt;

	// Token: 0x02001B01 RID: 6913
	public class UnburntStates : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400837E RID: 33662
		public LightMonitor.SafeStates safe;

		// Token: 0x0400837F RID: 33663
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State burning;
	}

	// Token: 0x02001B02 RID: 6914
	public class SafeStates : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008380 RID: 33664
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State unlit;

		// Token: 0x04008381 RID: 33665
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State normal_light;

		// Token: 0x04008382 RID: 33666
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State sunlight;
	}

	// Token: 0x02001B03 RID: 6915
	public new class Instance : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A812 RID: 43026 RVA: 0x003BE403 File Offset: 0x003BC603
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x04008383 RID: 33667
		public Effects effects;
	}
}
