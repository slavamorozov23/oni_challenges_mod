using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
[SkipSaveFileSerialization]
public class PressureVulnerable : StateMachineComponent<PressureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06003D61 RID: 15713 RVA: 0x00156B50 File Offset: 0x00154D50
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x06003D62 RID: 15714 RVA: 0x00156B72 File Offset: 0x00154D72
	public bool IsSafeElement(Element element)
	{
		return this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_atmospheres.Contains(element);
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06003D63 RID: 15715 RVA: 0x00156B9A File Offset: 0x00154D9A
	public PressureVulnerable.PressureState ExternalPressureState
	{
		get
		{
			return this.pressureState;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06003D64 RID: 15716 RVA: 0x00156BA2 File Offset: 0x00154DA2
	public bool IsLethal
	{
		get
		{
			return this.pressureState == PressureVulnerable.PressureState.LethalHigh || this.pressureState == PressureVulnerable.PressureState.LethalLow || !this.testAreaElementSafe;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06003D65 RID: 15717 RVA: 0x00156BC0 File Offset: 0x00154DC0
	public bool IsNormal
	{
		get
		{
			return this.testAreaElementSafe && this.pressureState == PressureVulnerable.PressureState.Normal;
		}
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x00156BD8 File Offset: 0x00154DD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayPressureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.AirPressure, base.gameObject));
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x00156C20 File Offset: 0x00154E20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.RegisterUpdate1000ms(this);
		this.cell = Grid.PosToCell(this);
		base.smi.sm.pressure.Set(1f, base.smi, false);
		base.smi.sm.safe_element.Set(this.testAreaElementSafe, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x00156C9A File Offset: 0x00154E9A
	protected override void OnCleanUp()
	{
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x00156CB0 File Offset: 0x00154EB0
	public void Configure(SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = false;
		this.pressureWarning_Low = float.MinValue;
		this.pressureLethal_Low = float.MinValue;
		this.pressureLethal_High = float.MaxValue;
		this.pressureWarning_High = float.MaxValue;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x00156D24 File Offset: 0x00154F24
	public void Configure(float pressureWarningLow = 0.25f, float pressureLethalLow = 0.01f, float pressureWarningHigh = 10f, float pressureLethalHigh = 30f, SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = true;
		this.pressureWarning_Low = pressureWarningLow;
		this.pressureLethal_Low = pressureLethalLow;
		this.pressureLethal_High = pressureLethalHigh;
		this.pressureWarning_High = pressureWarningHigh;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06003D6B RID: 15723 RVA: 0x00156D8B File Offset: 0x00154F8B
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Pressure,
				WiltCondition.Condition.AtmosphereElement
			};
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06003D6C RID: 15724 RVA: 0x00156D9C File Offset: 0x00154F9C
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.warningLow) || base.smi.IsInsideState(base.smi.sm.lethalLow))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooLow.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOLOW.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.warningHigh) || base.smi.IsInsideState(base.smi.sm.lethalHigh))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooHigh.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOHIGH.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.unsafeElement))
			{
				text += Db.Get().CreatureStatusItems.WrongAtmosphere.resolveStringCallback(CREATURES.STATUSITEMS.WRONGATMOSPHERE.NAME, this);
			}
			return text;
		}
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x00156EC9 File Offset: 0x001550C9
	public bool IsSafePressure(float pressure)
	{
		return !this.pressure_sensitive || (pressure > this.pressureLethal_Low && pressure < this.pressureLethal_High);
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x00156EEC File Offset: 0x001550EC
	public void SlicedSim1000ms(float dt)
	{
		float value = base.smi.sm.pressure.Get(base.smi) * 0.7f + this.GetPressureOverArea(this.cell) * 0.3f;
		this.safe_element *= 0.7f;
		if (this.testAreaElementSafe)
		{
			this.safe_element += 0.3f;
		}
		this.displayPressureAmount.value = value;
		bool value2 = this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_element >= 0.06f;
		base.smi.sm.safe_element.Set(value2, base.smi, false);
		base.smi.sm.pressure.Set(value, base.smi, false);
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00156FCB File Offset: 0x001551CB
	public float GetExternalPressure()
	{
		return this.GetPressureOverArea(this.cell);
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x00156FDC File Offset: 0x001551DC
	private float GetPressureOverArea(int cell)
	{
		bool flag = this.testAreaElementSafe;
		PressureVulnerable.testAreaPressure = 0f;
		PressureVulnerable.testAreaCount = 0;
		PressureVulnerable.testAreaSafeElementCount = 0;
		this.testAreaElementSafe = false;
		this.currentAtmoElement = null;
		this.occupyArea.TestArea(cell, this, PressureVulnerable.testAreaCB);
		this.testAreaElementSafe = (this.allCellsMustBeSafe ? (PressureVulnerable.testAreaSafeElementCount == this.occupyArea.OccupiedCellsOffsets.Length) : (PressureVulnerable.testAreaSafeElementCount > 0));
		PressureVulnerable.testAreaPressure = ((PressureVulnerable.testAreaCount > 0) ? (PressureVulnerable.testAreaPressure / (float)PressureVulnerable.testAreaCount) : 0f);
		if (this.testAreaElementSafe != flag)
		{
			base.Trigger(-2023773544, null);
		}
		return PressureVulnerable.testAreaPressure;
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x00157090 File Offset: 0x00155290
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.pressure_sensitive)
		{
			list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		}
		if (this.safe_atmospheres != null && this.safe_atmospheres.Count > 0)
		{
			string text = "";
			bool flag = false;
			bool flag2 = false;
			foreach (Element element in this.safe_atmospheres)
			{
				flag |= element.IsGas;
				flag2 |= element.IsLiquid;
				text = text + "\n        • " + element.name;
			}
			if (flag && flag2)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_MIXED, text), Descriptor.DescriptorType.Requirement, false));
			}
			if (flag)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE, text), Descriptor.DescriptorType.Requirement, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_LIQUID, text), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x040025E1 RID: 9697
	private const float kTrailingWeight = 0.7f;

	// Token: 0x040025E2 RID: 9698
	private const float kLeadingWeight = 0.3f;

	// Token: 0x040025E3 RID: 9699
	private const float kSafeElementThreshold = 0.06f;

	// Token: 0x040025E4 RID: 9700
	private float safe_element = 1f;

	// Token: 0x040025E5 RID: 9701
	private OccupyArea _occupyArea;

	// Token: 0x040025E6 RID: 9702
	public float pressureLethal_Low;

	// Token: 0x040025E7 RID: 9703
	public float pressureWarning_Low;

	// Token: 0x040025E8 RID: 9704
	public float pressureWarning_High;

	// Token: 0x040025E9 RID: 9705
	public float pressureLethal_High;

	// Token: 0x040025EA RID: 9706
	private static float testAreaPressure;

	// Token: 0x040025EB RID: 9707
	private static int testAreaCount;

	// Token: 0x040025EC RID: 9708
	private static int testAreaSafeElementCount;

	// Token: 0x040025ED RID: 9709
	public bool testAreaElementSafe = true;

	// Token: 0x040025EE RID: 9710
	public Element currentAtmoElement;

	// Token: 0x040025EF RID: 9711
	private static Func<int, object, bool> testAreaCB = delegate(int test_cell, object data)
	{
		PressureVulnerable pressureVulnerable = (PressureVulnerable)data;
		if (!Grid.IsSolidCell(test_cell))
		{
			Element element = Grid.Element[test_cell];
			if (pressureVulnerable.IsSafeElement(element))
			{
				PressureVulnerable.testAreaPressure += Grid.Mass[test_cell];
				PressureVulnerable.testAreaCount++;
				PressureVulnerable.testAreaSafeElementCount++;
				pressureVulnerable.currentAtmoElement = element;
			}
			if (pressureVulnerable.currentAtmoElement == null)
			{
				pressureVulnerable.currentAtmoElement = element;
			}
		}
		return true;
	};

	// Token: 0x040025F0 RID: 9712
	private AmountInstance displayPressureAmount;

	// Token: 0x040025F1 RID: 9713
	public bool pressure_sensitive = true;

	// Token: 0x040025F2 RID: 9714
	public bool allCellsMustBeSafe;

	// Token: 0x040025F3 RID: 9715
	public HashSet<Element> safe_atmospheres = new HashSet<Element>();

	// Token: 0x040025F4 RID: 9716
	private int cell;

	// Token: 0x040025F5 RID: 9717
	private PressureVulnerable.PressureState pressureState = PressureVulnerable.PressureState.Normal;

	// Token: 0x020018B7 RID: 6327
	public class StatesInstance : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.GameInstance
	{
		// Token: 0x0600A022 RID: 40994 RVA: 0x003A8CE9 File Offset: 0x003A6EE9
		public StatesInstance(PressureVulnerable master) : base(master)
		{
			if (Db.Get().Amounts.Maturity.Lookup(base.gameObject) != null)
			{
				this.hasMaturity = true;
			}
		}

		// Token: 0x04007BD0 RID: 31696
		public bool hasMaturity;
	}

	// Token: 0x020018B8 RID: 6328
	public class States : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable>
	{
		// Token: 0x0600A023 RID: 40995 RVA: 0x003A8D18 File Offset: 0x003A6F18
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalLow.ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalLow;
			}).TriggerOnEnter(GameHashes.LowPressureFatal, null);
			this.lethalHigh.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalHigh;
			}).TriggerOnEnter(GameHashes.HighPressureFatal, null);
			this.warningLow.ParamTransition<float>(this.pressure, this.lethalLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_Low).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningLow;
			}).TriggerOnEnter(GameHashes.LowPressureWarning, null);
			this.unsafeElement.ParamTransition<bool>(this.safe_element, this.normal, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsTrue).TriggerOnExit(GameHashes.CorrectAtmosphere, null).TriggerOnEnter(GameHashes.WrongAtmosphere, null);
			this.warningHigh.ParamTransition<float>(this.pressure, this.lethalHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_High).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningHigh;
			}).TriggerOnEnter(GameHashes.HighPressureWarning, null);
			this.normal.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_High).ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalPressureAchieved, null);
		}

		// Token: 0x04007BD1 RID: 31697
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.FloatParameter pressure;

		// Token: 0x04007BD2 RID: 31698
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.BoolParameter safe_element;

		// Token: 0x04007BD3 RID: 31699
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State unsafeElement;

		// Token: 0x04007BD4 RID: 31700
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalLow;

		// Token: 0x04007BD5 RID: 31701
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalHigh;

		// Token: 0x04007BD6 RID: 31702
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningLow;

		// Token: 0x04007BD7 RID: 31703
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningHigh;

		// Token: 0x04007BD8 RID: 31704
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State normal;
	}

	// Token: 0x020018B9 RID: 6329
	public enum PressureState
	{
		// Token: 0x04007BDA RID: 31706
		LethalLow,
		// Token: 0x04007BDB RID: 31707
		WarningLow,
		// Token: 0x04007BDC RID: 31708
		Normal,
		// Token: 0x04007BDD RID: 31709
		WarningHigh,
		// Token: 0x04007BDE RID: 31710
		LethalHigh
	}
}
