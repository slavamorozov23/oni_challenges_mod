using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A43 RID: 2627
public class ScaldingMonitor : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>
{
	// Token: 0x06004C99 RID: 19609 RVA: 0x001BD550 File Offset: 0x001BB750
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State.Callback(ScaldingMonitor.SetInitialAverageExternalTemperature)).EventHandler(GameHashes.OnUnequip, new GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameEvent.Callback(ScaldingMonitor.OnSuitUnequipped)).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.AverageExternalTemperatureUpdate), UpdateRate.SIM_200ms, false);
		this.idle.Transition(this.transitionToScalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding), UpdateRate.SIM_200ms).Transition(this.transitionToScolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding), UpdateRate.SIM_200ms);
		this.transitionToScalding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding)), UpdateRate.SIM_200ms).Transition(this.scalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScaldingTimed), UpdateRate.SIM_200ms);
		this.transitionToScolding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding)), UpdateRate.SIM_200ms).Transition(this.scolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScoldingTimed), UpdateRate.SIM_200ms);
		this.scalding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScalding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeScaldDamage), UpdateRate.SIM_1000ms, false);
		this.scolding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScolding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scolding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeColdDamage), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x001BD77A File Offset: 0x001BB97A
	public static void OnSuitUnequipped(ScaldingMonitor.Instance smi, object obj)
	{
		if (obj != null && ((Equippable)obj).HasTag(GameTags.AirtightSuit))
		{
			smi.ResetExternalTemperatureAverage();
		}
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x001BD797 File Offset: 0x001BB997
	public static void SetInitialAverageExternalTemperature(ScaldingMonitor.Instance smi)
	{
		smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature();
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x001BD7A5 File Offset: 0x001BB9A5
	public static bool CanEscapeScalding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x001BD7BE File Offset: 0x001BB9BE
	public static bool CanEscapeScolding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x001BD7D7 File Offset: 0x001BB9D7
	public static bool IsScaldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x001BD7F0 File Offset: 0x001BB9F0
	public static bool IsScalding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding();
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x001BD7F8 File Offset: 0x001BB9F8
	public static bool IsScolding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding();
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x001BD800 File Offset: 0x001BBA00
	public static bool IsScoldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x06004CA2 RID: 19618 RVA: 0x001BD819 File Offset: 0x001BBA19
	public static void TakeScaldDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x06004CA3 RID: 19619 RVA: 0x001BD822 File Offset: 0x001BBA22
	public static void TakeColdDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x06004CA4 RID: 19620 RVA: 0x001BD82C File Offset: 0x001BBA2C
	public static void AverageExternalTemperatureUpdate(ScaldingMonitor.Instance smi, float dt)
	{
		smi.AverageExternalTemperature *= Mathf.Max(0f, 1f - dt / 6f);
		smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature() * (dt / 6f);
	}

	// Token: 0x040032F0 RID: 13040
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x040032F1 RID: 13041
	private const float TEMPERATURE_AVERAGING_RANGE = 6f;

	// Token: 0x040032F2 RID: 13042
	private const float MIN_SCALD_INTERVAL = 5f;

	// Token: 0x040032F3 RID: 13043
	private const float SCALDING_DAMAGE_AMOUNT = 10f;

	// Token: 0x040032F4 RID: 13044
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State idle;

	// Token: 0x040032F5 RID: 13045
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScalding;

	// Token: 0x040032F6 RID: 13046
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScolding;

	// Token: 0x040032F7 RID: 13047
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scalding;

	// Token: 0x040032F8 RID: 13048
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scolding;

	// Token: 0x02001B2C RID: 6956
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040083F3 RID: 33779
		public float defaultScaldingTreshold = 345f;

		// Token: 0x040083F4 RID: 33780
		public float defaultScoldingTreshold = 183f;
	}

	// Token: 0x02001B2D RID: 6957
	public new class Instance : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameInstance
	{
		// Token: 0x0600A8B1 RID: 43185 RVA: 0x003BF8C0 File Offset: 0x003BDAC0
		public Instance(IStateMachineTarget master, ScaldingMonitor.Def def) : base(master, def)
		{
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", def.defaultScaldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.baseScoldingThreshold = new AttributeModifier("ScoldingThreshold", def.defaultScoldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.attributes = base.gameObject.GetAttributes();
		}

		// Token: 0x0600A8B2 RID: 43186 RVA: 0x003BF94C File Offset: 0x003BDB4C
		public override void StartSM()
		{
			base.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(this.baseScalindingThreshold);
			base.smi.attributes.Get(Db.Get().Attributes.ScoldingThreshold).Add(this.baseScoldingThreshold);
			base.StartSM();
		}

		// Token: 0x0600A8B3 RID: 43187 RVA: 0x003BF9B4 File Offset: 0x003BDBB4
		public bool IsScalding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature > this.GetScaldingThreshold();
		}

		// Token: 0x0600A8B4 RID: 43188 RVA: 0x003BFA0B File Offset: 0x003BDC0B
		public float GetScaldingThreshold()
		{
			return base.smi.attributes.GetValue("ScaldingThreshold");
		}

		// Token: 0x0600A8B5 RID: 43189 RVA: 0x003BFA24 File Offset: 0x003BDC24
		public bool IsScolding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature < this.GetScoldingThreshold();
		}

		// Token: 0x0600A8B6 RID: 43190 RVA: 0x003BFA7B File Offset: 0x003BDC7B
		public float GetScoldingThreshold()
		{
			return base.smi.attributes.GetValue("ScoldingThreshold");
		}

		// Token: 0x0600A8B7 RID: 43191 RVA: 0x003BFA92 File Offset: 0x003BDC92
		public void TemperatureDamage(float dt)
		{
			if (this.health != null && Time.time - this.lastScaldTime > 5f)
			{
				this.lastScaldTime = Time.time;
				this.health.Damage(dt * 10f);
			}
		}

		// Token: 0x0600A8B8 RID: 43192 RVA: 0x003BFAD2 File Offset: 0x003BDCD2
		public void ResetExternalTemperatureAverage()
		{
			base.smi.AverageExternalTemperature = this.internalTemperature.value;
		}

		// Token: 0x0600A8B9 RID: 43193 RVA: 0x003BFAEC File Offset: 0x003BDCEC
		public float GetCurrentExternalTemperature()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (this.occupyArea != null)
			{
				float num2 = 0f;
				int num3 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num4 = Grid.OffsetCell(num, this.occupyArea.OccupiedCellsOffsets[i]);
					if (Grid.IsValidCell(num4))
					{
						bool flag = Grid.Element[num4].id == SimHashes.Vacuum || Grid.Element[num4].id == SimHashes.Void;
						num3++;
						num2 += (flag ? this.internalTemperature.value : Grid.Temperature[num4]);
					}
				}
				return num2 / (float)Mathf.Max(1, num3);
			}
			if (Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void)
			{
				return Grid.Temperature[num];
			}
			return this.internalTemperature.value;
		}

		// Token: 0x040083F5 RID: 33781
		public float AverageExternalTemperature;

		// Token: 0x040083F6 RID: 33782
		private float lastScaldTime;

		// Token: 0x040083F7 RID: 33783
		private Attributes attributes;

		// Token: 0x040083F8 RID: 33784
		[MyCmpGet]
		private Health health;

		// Token: 0x040083F9 RID: 33785
		[MyCmpGet]
		private OccupyArea occupyArea;

		// Token: 0x040083FA RID: 33786
		private AttributeModifier baseScalindingThreshold;

		// Token: 0x040083FB RID: 33787
		private AttributeModifier baseScoldingThreshold;

		// Token: 0x040083FC RID: 33788
		public AmountInstance internalTemperature;
	}
}
