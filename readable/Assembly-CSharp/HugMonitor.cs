using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class HugMonitor : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
	// Token: 0x060021C7 RID: 8647 RVA: 0x000C45DC File Offset: 0x000C27DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<HugMonitor.Instance, float>(this.UpdateHugEggCooldownTimer), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (HugMonitor.Instance smi) => smi.UpdateHasTarget(), delegate(HugMonitor.Instance smi)
		{
			smi.hugTarget = null;
		});
		this.normal.DefaultState(this.normal.idle).ParamTransition<float>(this.hugFrenzyTimer, this.hugFrenzy, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsGTZero);
		this.normal.idle.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		this.normal.hugReady.ToggleReactable(new Func<HugMonitor.Instance, Reactable>(this.GetHugReactable));
		GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State state = this.normal.hugReady.passiveHug.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (HugMonitor.Instance smi) => true, delegate(HugMonitor.Instance smi)
		{
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi, false);
			smi.GoTo(this.normal.hugReady.passiveHug);
		});
		this.hugFrenzy.ParamTransition<float>(this.hugFrenzyTimer, this.normal, (HugMonitor.Instance smi, float p) => p <= 0f && !smi.IsHugging()).Update(new Action<HugMonitor.Instance, float>(this.UpdateHugFrenzyTimer), UpdateRate.SIM_1000ms, false).ToggleEffect((HugMonitor.Instance smi) => smi.frenzyEffect).ToggleLoopingSound(HugMonitor.soundPath, null, true, true, true).Enter(delegate(HugMonitor.Instance smi)
		{
			smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
			smi.hugParticleFx.transform.SetParent(smi.master.transform);
			smi.hugParticleFx.SetActive(true);
		}).Exit(delegate(HugMonitor.Instance smi)
		{
			Util.KDestroyGameObject(smi.hugParticleFx);
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi, false);
		});
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x000C4860 File Offset: 0x000C2A60
	private Reactable GetHugReactable(HugMonitor.Instance smi)
	{
		return new HugMinionReactable(smi.gameObject);
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000C486D File Offset: 0x000C2A6D
	private void UpdateWantsHugCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.wantsHugCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000C4888 File Offset: 0x000C2A88
	private void UpdateHugEggCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugEggCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x000C48A3 File Offset: 0x000C2AA3
	private void UpdateHugFrenzyTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugFrenzyTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x040013AE RID: 5038
	private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX", false);

	// Token: 0x040013AF RID: 5039
	private static Effect hugEffect;

	// Token: 0x040013B0 RID: 5040
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugFrenzyTimer;

	// Token: 0x040013B1 RID: 5041
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter wantsHugCooldownTimer;

	// Token: 0x040013B2 RID: 5042
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugEggCooldownTimer;

	// Token: 0x040013B3 RID: 5043
	public HugMonitor.NormalStates normal;

	// Token: 0x040013B4 RID: 5044
	public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State hugFrenzy;

	// Token: 0x02001477 RID: 5239
	public class HUGTUNING
	{
		// Token: 0x04006EB0 RID: 28336
		public const float HUG_EGG_TIME = 15f;

		// Token: 0x04006EB1 RID: 28337
		public const float HUG_DUPE_WAIT = 60f;

		// Token: 0x04006EB2 RID: 28338
		public const float FRENZY_EGGS_PER_CYCLE = 6f;

		// Token: 0x04006EB3 RID: 28339
		public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;

		// Token: 0x04006EB4 RID: 28340
		public const float HUG_FRENZY_DURATION = 120f;
	}

	// Token: 0x02001478 RID: 5240
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006EB5 RID: 28341
		public float hugsPerCycle = 2f;

		// Token: 0x04006EB6 RID: 28342
		public float scanningInterval = 30f;

		// Token: 0x04006EB7 RID: 28343
		public float hugFrenzyDuration = 120f;

		// Token: 0x04006EB8 RID: 28344
		public float hugFrenzyCooldown = 480f;

		// Token: 0x04006EB9 RID: 28345
		public float hugFrenzyCooldownFailed = 120f;

		// Token: 0x04006EBA RID: 28346
		public float scanningIntervalFrenzy = 15f;

		// Token: 0x04006EBB RID: 28347
		public int maxSearchCost = 30;
	}

	// Token: 0x02001479 RID: 5241
	public class HugReadyStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04006EBC RID: 28348
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State passiveHug;

		// Token: 0x04006EBD RID: 28349
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State seekingHug;
	}

	// Token: 0x0200147A RID: 5242
	public class NormalStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04006EBE RID: 28350
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State idle;

		// Token: 0x04006EBF RID: 28351
		public HugMonitor.HugReadyStates hugReady;
	}

	// Token: 0x0200147B RID: 5243
	public new class Instance : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.GameInstance
	{
		// Token: 0x06008FEB RID: 36843 RVA: 0x0036D028 File Offset: 0x0036B228
		public Instance(IStateMachineTarget master, HugMonitor.Def def) : base(master, def)
		{
			this.frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
			this.RefreshSearchTime();
			if (HugMonitor.hugEffect == null)
			{
				HugMonitor.hugEffect = Db.Get().effects.Get("EggHug");
			}
			base.smi.sm.wantsHugCooldownTimer.Set(UnityEngine.Random.Range(base.smi.def.hugFrenzyCooldownFailed, base.smi.def.hugFrenzyCooldown), base.smi, false);
		}

		// Token: 0x06008FEC RID: 36844 RVA: 0x0036D0C0 File Offset: 0x0036B2C0
		private void RefreshSearchTime()
		{
			if (this.hugTarget == null)
			{
				base.smi.sm.hugEggCooldownTimer.Set(this.GetScanningInterval(), base.smi, false);
				return;
			}
			base.smi.sm.hugEggCooldownTimer.Set(this.GetHugInterval(), base.smi, false);
		}

		// Token: 0x06008FED RID: 36845 RVA: 0x0036D122 File Offset: 0x0036B322
		private float GetScanningInterval()
		{
			if (!this.IsHuggingFrenzy())
			{
				return base.def.scanningInterval;
			}
			return base.def.scanningIntervalFrenzy;
		}

		// Token: 0x06008FEE RID: 36846 RVA: 0x0036D143 File Offset: 0x0036B343
		private float GetHugInterval()
		{
			if (this.IsHuggingFrenzy())
			{
				return 0f;
			}
			return 600f / base.def.hugsPerCycle;
		}

		// Token: 0x06008FEF RID: 36847 RVA: 0x0036D164 File Offset: 0x0036B364
		public bool IsHuggingFrenzy()
		{
			return base.smi.GetCurrentState() == base.smi.sm.hugFrenzy;
		}

		// Token: 0x06008FF0 RID: 36848 RVA: 0x0036D183 File Offset: 0x0036B383
		public bool IsHugging()
		{
			return base.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;
		}

		// Token: 0x06008FF1 RID: 36849 RVA: 0x0036D198 File Offset: 0x0036B398
		public bool UpdateHasTarget()
		{
			if (this.hugTarget == null)
			{
				if (base.smi.sm.hugEggCooldownTimer.Get(base.smi) > 0f)
				{
					return false;
				}
				this.FindEgg();
				this.RefreshSearchTime();
			}
			return this.hugTarget != null;
		}

		// Token: 0x06008FF2 RID: 36850 RVA: 0x0036D1F0 File Offset: 0x0036B3F0
		public void EnterHuggingFrenzy()
		{
			base.smi.sm.hugFrenzyTimer.Set(base.smi.def.hugFrenzyDuration, base.smi, false);
			base.smi.sm.hugEggCooldownTimer.Set(0f, base.smi, false);
		}

		// Token: 0x06008FF3 RID: 36851 RVA: 0x0036D24C File Offset: 0x0036B44C
		private void FindEgg()
		{
			int cell = Grid.PosToCell(base.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			int num = base.def.maxSearchCost;
			this.hugTarget = null;
			if (cavityForCell != null)
			{
				foreach (KPrefabID kprefabID in cavityForCell.eggs)
				{
					if (!kprefabID.HasTag(GameTags.Creatures.ReservedByCreature) && !kprefabID.GetComponent<Effects>().HasEffect(HugMonitor.hugEffect))
					{
						int num2 = Grid.PosToCell(kprefabID);
						if (kprefabID.HasTag(GameTags.Stored))
						{
							GameObject gameObject;
							KPrefabID kprefabID2;
							if (!Grid.ObjectLayers[1].TryGetValue(num2, out gameObject) || !gameObject.TryGetComponent<KPrefabID>(out kprefabID2) || !kprefabID2.IsPrefabID("EggIncubator"))
							{
								continue;
							}
							num2 = Grid.PosToCell(gameObject);
							kprefabID = kprefabID2;
						}
						int navigationCost = this.navigator.GetNavigationCost(num2);
						if (navigationCost != -1 && navigationCost < num)
						{
							this.hugTarget = kprefabID;
							num = navigationCost;
						}
					}
				}
			}
		}

		// Token: 0x04006EC0 RID: 28352
		public GameObject hugParticleFx;

		// Token: 0x04006EC1 RID: 28353
		public Vector3 hugParticleOffset;

		// Token: 0x04006EC2 RID: 28354
		public Effect frenzyEffect;

		// Token: 0x04006EC3 RID: 28355
		public KPrefabID hugTarget;

		// Token: 0x04006EC4 RID: 28356
		[MyCmpGet]
		private Navigator navigator;
	}
}
