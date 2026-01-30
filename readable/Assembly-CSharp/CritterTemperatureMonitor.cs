using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class CritterTemperatureMonitor : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>
{
	// Token: 0x06003C61 RID: 15457 RVA: 0x00151C60 File Offset: 0x0014FE60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.uncomfortableEffect = new Effect("EffectCritterTemperatureUncomfortable", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.uncomfortableEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, false, false, true));
		this.deadlyEffect = new Effect("EffectCritterTemperatureDeadly", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.deadlyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -2f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, false, false, true));
		this.root.Enter(new StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback(CritterTemperatureMonitor.RefreshInternalTemperature)).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			StateMachine.BaseState targetState = smi.GetTargetState();
			if (smi.GetCurrentState() != targetState)
			{
				smi.GoTo(targetState);
			}
		}, UpdateRate.SIM_200ms, false).Update(new Action<CritterTemperatureMonitor.Instance, float>(CritterTemperatureMonitor.UpdateInternalTemperature), UpdateRate.SIM_1000ms, false);
		this.hot.TagTransition(GameTags.Dead, this.dead, false).ToggleCritterEmotion(Db.Get().CritterEmotions.Hot, null);
		this.cold.TagTransition(GameTags.Dead, this.dead, false).ToggleCritterEmotion(Db.Get().CritterEmotions.Cold, null);
		this.hot.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.hot.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.cold.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.cold.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.dead.DoNothing();
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00151F6E File Offset: 0x0015016E
	public static void UpdateInternalTemperature(CritterTemperatureMonitor.Instance smi, float dt)
	{
		CritterTemperatureMonitor.RefreshInternalTemperature(smi);
		if (smi.OnUpdate_GetTemperatureInternal != null)
		{
			smi.OnUpdate_GetTemperatureInternal(dt, smi.GetTemperatureInternal());
		}
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00151F90 File Offset: 0x00150190
	public static void RefreshInternalTemperature(CritterTemperatureMonitor.Instance smi)
	{
		if (smi.temperature != null)
		{
			smi.temperature.SetValue(smi.GetTemperatureInternal());
		}
	}

	// Token: 0x0400253C RID: 9532
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State comfortable;

	// Token: 0x0400253D RID: 9533
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State dead;

	// Token: 0x0400253E RID: 9534
	public CritterTemperatureMonitor.TemperatureStates hot;

	// Token: 0x0400253F RID: 9535
	public CritterTemperatureMonitor.TemperatureStates cold;

	// Token: 0x04002540 RID: 9536
	public Effect uncomfortableEffect;

	// Token: 0x04002541 RID: 9537
	public Effect deadlyEffect;

	// Token: 0x02001869 RID: 6249
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009ED0 RID: 40656 RVA: 0x003A41D1 File Offset: 0x003A23D1
		public float GetIdealTemperature()
		{
			return (this.temperatureHotUncomfortable + this.temperatureColdUncomfortable) / 2f;
		}

		// Token: 0x04007AD1 RID: 31441
		public float temperatureHotDeadly = float.MaxValue;

		// Token: 0x04007AD2 RID: 31442
		public float temperatureHotUncomfortable = float.MaxValue;

		// Token: 0x04007AD3 RID: 31443
		public float temperatureColdDeadly = float.MinValue;

		// Token: 0x04007AD4 RID: 31444
		public float temperatureColdUncomfortable = float.MinValue;

		// Token: 0x04007AD5 RID: 31445
		public float secondsUntilDamageStarts = 1f;

		// Token: 0x04007AD6 RID: 31446
		public float damagePerSecond = 0.25f;
	}

	// Token: 0x0200186A RID: 6250
	public class TemperatureStates : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State
	{
		// Token: 0x04007AD7 RID: 31447
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State uncomfortable;

		// Token: 0x04007AD8 RID: 31448
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State deadly;
	}

	// Token: 0x0200186B RID: 6251
	public new class Instance : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.GameInstance
	{
		// Token: 0x06009ED3 RID: 40659 RVA: 0x003A4248 File Offset: 0x003A2448
		public Instance(IStateMachineTarget master, CritterTemperatureMonitor.Def def) : base(master, def)
		{
			this.health = master.GetComponent<Health>();
			this.occupyArea = master.GetComponent<OccupyArea>();
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.CritterTemperature.Lookup(base.gameObject);
			this.pickupable = master.GetComponent<Pickupable>();
		}

		// Token: 0x06009ED4 RID: 40660 RVA: 0x003A42AD File Offset: 0x003A24AD
		public void ResetDamageCooldown()
		{
			this.secondsUntilDamage = base.def.secondsUntilDamageStarts;
		}

		// Token: 0x06009ED5 RID: 40661 RVA: 0x003A42C0 File Offset: 0x003A24C0
		public void TryDamage(float deltaSeconds)
		{
			if (this.secondsUntilDamage <= 0f)
			{
				this.health.Damage(base.def.damagePerSecond);
				this.secondsUntilDamage = 1f;
				return;
			}
			this.secondsUntilDamage -= deltaSeconds;
		}

		// Token: 0x06009ED6 RID: 40662 RVA: 0x003A4300 File Offset: 0x003A2500
		public StateMachine.BaseState GetTargetState()
		{
			bool flag = this.IsEntirelyInVaccum();
			float temperatureExternal = this.GetTemperatureExternal();
			float temperatureInternal = this.GetTemperatureInternal();
			StateMachine.BaseState result;
			if (this.pickupable.KPrefabID.HasTag(GameTags.Dead))
			{
				result = base.sm.dead;
			}
			else if (!flag && temperatureExternal > base.def.temperatureHotDeadly)
			{
				result = base.sm.hot.deadly;
			}
			else if (!flag && temperatureExternal < base.def.temperatureColdDeadly)
			{
				result = base.sm.cold.deadly;
			}
			else if (temperatureInternal > base.def.temperatureHotUncomfortable)
			{
				result = base.sm.hot.uncomfortable;
			}
			else if (temperatureInternal < base.def.temperatureColdUncomfortable)
			{
				result = base.sm.cold.uncomfortable;
			}
			else
			{
				result = base.sm.comfortable;
			}
			return result;
		}

		// Token: 0x06009ED7 RID: 40663 RVA: 0x003A43E4 File Offset: 0x003A25E4
		public bool IsEntirelyInVaccum()
		{
			int cachedCell = this.pickupable.cachedCell;
			bool result;
			if (this.occupyArea != null)
			{
				result = true;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
					if (!Grid.IsValidCell(num) || !Grid.Element[num].IsVacuum)
					{
						result = false;
						break;
					}
				}
			}
			else
			{
				result = (!Grid.IsValidCell(cachedCell) || Grid.Element[cachedCell].IsVacuum);
			}
			return result;
		}

		// Token: 0x06009ED8 RID: 40664 RVA: 0x003A4472 File Offset: 0x003A2672
		public float GetTemperatureInternal()
		{
			return this.primaryElement.Temperature;
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x003A4480 File Offset: 0x003A2680
		public float GetTemperatureExternal()
		{
			int cachedCell = this.pickupable.cachedCell;
			if (this.occupyArea != null)
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num3 = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
					if (Grid.IsValidCell(num3))
					{
						bool flag = Grid.Element[num3].id == SimHashes.Vacuum || Grid.Element[num3].id == SimHashes.Void;
						num2++;
						num += (flag ? this.GetTemperatureInternal() : Grid.Temperature[num3]);
					}
				}
				return num / (float)Mathf.Max(1, num2);
			}
			if (Grid.Element[cachedCell].id != SimHashes.Vacuum && Grid.Element[cachedCell].id != SimHashes.Void)
			{
				return Grid.Temperature[cachedCell];
			}
			return this.GetTemperatureInternal();
		}

		// Token: 0x04007AD9 RID: 31449
		public AmountInstance temperature;

		// Token: 0x04007ADA RID: 31450
		public Health health;

		// Token: 0x04007ADB RID: 31451
		public OccupyArea occupyArea;

		// Token: 0x04007ADC RID: 31452
		public PrimaryElement primaryElement;

		// Token: 0x04007ADD RID: 31453
		public Pickupable pickupable;

		// Token: 0x04007ADE RID: 31454
		public float secondsUntilDamage;

		// Token: 0x04007ADF RID: 31455
		public Action<float, float> OnUpdate_GetTemperatureInternal;
	}
}
