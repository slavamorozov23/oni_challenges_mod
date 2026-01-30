using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A47 RID: 2631
public class SlipperyMonitor : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>
{
	// Token: 0x06004CB2 RID: 19634 RVA: 0x001BDF28 File Offset: 0x001BC128
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.EventTransition(GameHashes.NavigationCellChanged, this.unsafeCell, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell));
		this.unsafeCell.EventTransition(GameHashes.NavigationCellChanged, this.safe, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell))).DefaultState(this.unsafeCell.atRisk);
		this.unsafeCell.atRisk.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).EventTransition(GameHashes.EffectAdded, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).DefaultState(this.unsafeCell.atRisk.idle);
		this.unsafeCell.atRisk.idle.EventHandlerTransition(GameHashes.NavigationCellChanged, this.unsafeCell.atRisk.slip, new Func<SlipperyMonitor.Instance, object, bool>(SlipperyMonitor.RollDTwenty));
		this.unsafeCell.atRisk.slip.ToggleReactable(new Func<SlipperyMonitor.Instance, Reactable>(this.GetReactable)).ScheduleGoTo(8f, this.unsafeCell.atRisk.idle);
		this.unsafeCell.immune.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces))).EventTransition(GameHashes.EffectRemoved, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)));
	}

	// Token: 0x06004CB3 RID: 19635 RVA: 0x001BE0CD File Offset: 0x001BC2CD
	public bool IsImmuneToSlipperySurfaces(SlipperyMonitor.Instance smi)
	{
		return smi.IsImmune;
	}

	// Token: 0x06004CB4 RID: 19636 RVA: 0x001BE0D5 File Offset: 0x001BC2D5
	public Reactable GetReactable(SlipperyMonitor.Instance smi)
	{
		return smi.CreateReactable();
	}

	// Token: 0x06004CB5 RID: 19637 RVA: 0x001BE0E0 File Offset: 0x001BC2E0
	private static bool IsStandingOnASlipperyCell(SlipperyMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		int num2 = Grid.OffsetCell(num, 0, -1);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsSlippery) || (Grid.IsValidCell(num2) && Grid.Element[num2].IsSolid && Grid.Element[num2].IsSlippery);
	}

	// Token: 0x06004CB6 RID: 19638 RVA: 0x001BE142 File Offset: 0x001BC342
	private static bool RollDTwenty(SlipperyMonitor.Instance smi, object o)
	{
		return UnityEngine.Random.value <= 0.05f;
	}

	// Token: 0x04003308 RID: 13064
	public const string EFFECT_NAME = "RecentlySlippedTracker";

	// Token: 0x04003309 RID: 13065
	public const float SLIP_FAIL_TIMEOUT = 8f;

	// Token: 0x0400330A RID: 13066
	public const float PROBABILITY_OF_SLIP = 0.05f;

	// Token: 0x0400330B RID: 13067
	public const float STRESS_DAMAGE = 3f;

	// Token: 0x0400330C RID: 13068
	public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State safe;

	// Token: 0x0400330D RID: 13069
	public SlipperyMonitor.UnsafeCellState unsafeCell;

	// Token: 0x02001B34 RID: 6964
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B35 RID: 6965
	public class UnsafeCellState : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x04008416 RID: 33814
		public SlipperyMonitor.RiskStates atRisk;

		// Token: 0x04008417 RID: 33815
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State immune;
	}

	// Token: 0x02001B36 RID: 6966
	public class RiskStates : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x04008418 RID: 33816
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State idle;

		// Token: 0x04008419 RID: 33817
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State slip;
	}

	// Token: 0x02001B37 RID: 6967
	public new class Instance : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.GameInstance
	{
		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600A8E2 RID: 43234 RVA: 0x003C0081 File Offset: 0x003BE281
		public bool IsImmune
		{
			get
			{
				return this.effects.HasEffect("RecentlySlippedTracker") || this.effects.HasImmunityTo(this.effect);
			}
		}

		// Token: 0x0600A8E3 RID: 43235 RVA: 0x003C00A8 File Offset: 0x003BE2A8
		public Instance(IStateMachineTarget master, SlipperyMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
			this.effect = Db.Get().effects.Get("RecentlySlippedTracker");
		}

		// Token: 0x0600A8E4 RID: 43236 RVA: 0x003C00D8 File Offset: 0x003BE2D8
		public SlipperyMonitor.SlipReactable CreateReactable()
		{
			return new SlipperyMonitor.SlipReactable(this);
		}

		// Token: 0x0400841A RID: 33818
		private Effect effect;

		// Token: 0x0400841B RID: 33819
		public Effects effects;
	}

	// Token: 0x02001B38 RID: 6968
	public class SlipReactable : Reactable
	{
		// Token: 0x0600A8E5 RID: 43237 RVA: 0x003C00E0 File Offset: 0x003BE2E0
		public SlipReactable(SlipperyMonitor.Instance _smi) : base(_smi.gameObject, "Slip", Db.Get().ChoreTypes.Slip, 1, 1, false, 0f, 0f, 8f, 0f, ObjectLayer.NumLayers)
		{
			this.smi = _smi;
		}

		// Token: 0x0600A8E6 RID: 43238 RVA: 0x003C0134 File Offset: 0x003BE334
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (new_reactor == null)
			{
				return false;
			}
			if (this.gameObject != new_reactor)
			{
				return false;
			}
			if (this.smi == null)
			{
				return false;
			}
			Navigator component = new_reactor.GetComponent<Navigator>();
			return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
		}

		// Token: 0x0600A8E7 RID: 43239 RVA: 0x003C01A8 File Offset: 0x003BE3A8
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, DUPLICANTS.MODIFIERS.SLIPPED.NAME, this.gameObject.transform, 1.5f, false);
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_slip_kanim"), 1f);
			component.Play("slip_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.reactor.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.Slippering, null);
		}

		// Token: 0x0600A8E8 RID: 43240 RVA: 0x003C0286 File Offset: 0x003BE486
		public override void Update(float dt)
		{
			if (Time.time - this.startTime > 4.3f)
			{
				base.Cleanup();
				this.ApplyStress();
				this.ApplyTrackerEffect();
			}
		}

		// Token: 0x0600A8E9 RID: 43241 RVA: 0x003C02AD File Offset: 0x003BE4AD
		public void ApplyTrackerEffect()
		{
			this.smi.effects.Add("RecentlySlippedTracker", true);
		}

		// Token: 0x0600A8EA RID: 43242 RVA: 0x003C02C8 File Offset: 0x003BE4C8
		private void ApplyStress()
		{
			this.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Stress.Id).ApplyDelta(3f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, 3f.ToString() + "% " + Db.Get().Amounts.Stress.Name, this.gameObject.transform, 1.5f, false);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, 3f, DUPLICANTS.MODIFIERS.SLIPPED.NAME, this.gameObject.GetProperName());
		}

		// Token: 0x0600A8EB RID: 43243 RVA: 0x003C0384 File Offset: 0x003BE584
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					this.reactor.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.Slippering, false);
					component.RemoveAnimOverrides(Assets.GetAnim("anim_slip_kanim"));
				}
			}
		}

		// Token: 0x0600A8EC RID: 43244 RVA: 0x003C03EA File Offset: 0x003BE5EA
		protected override void InternalCleanup()
		{
		}

		// Token: 0x0400841C RID: 33820
		private SlipperyMonitor.Instance smi;

		// Token: 0x0400841D RID: 33821
		private float startTime;

		// Token: 0x0400841E RID: 33822
		private const string ANIM_FILE_NAME = "anim_slip_kanim";

		// Token: 0x0400841F RID: 33823
		private const float DURATION = 4.3f;
	}
}
