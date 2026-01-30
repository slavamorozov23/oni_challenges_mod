using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008BF RID: 2239
[SkipSaveFileSerialization]
public class TemperatureVulnerable : StateMachineComponent<TemperatureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00158364 File Offset: 0x00156564
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

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x00158386 File Offset: 0x00156586
	public float TemperatureLethalLow
	{
		get
		{
			return this.internalTemperatureLethal_Low;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x0015838E File Offset: 0x0015658E
	public float TemperatureLethalHigh
	{
		get
		{
			return this.internalTemperatureLethal_High;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x00158396 File Offset: 0x00156596
	public float TemperatureWarningLow
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_Low + (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_Low;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x001583C6 File Offset: 0x001565C6
	public float TemperatureWarningHigh
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_High - (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_High;
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x001583F6 File Offset: 0x001565F6
	public float InternalTemperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06003DBA RID: 15802 RVA: 0x00158403 File Offset: 0x00156603
	public TemperatureVulnerable.TemperatureState GetInternalTemperatureState
	{
		get
		{
			return this.internalTemperatureState;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06003DBB RID: 15803 RVA: 0x0015840B File Offset: 0x0015660B
	public bool IsLethal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot || this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold;
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06003DBC RID: 15804 RVA: 0x00158421 File Offset: 0x00156621
	public bool IsNormal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06003DBD RID: 15805 RVA: 0x0015842C File Offset: 0x0015662C
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[1];
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06003DBE RID: 15806 RVA: 0x00158434 File Offset: 0x00156634
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.warningCold))
			{
				return Db.Get().CreatureStatusItems.Cold_Crop.resolveStringCallback(CREATURES.STATUSITEMS.COLD_CROP.NAME, this);
			}
			if (base.smi.IsInsideState(base.smi.sm.warningHot))
			{
				return Db.Get().CreatureStatusItems.Hot_Crop.resolveStringCallback(CREATURES.STATUSITEMS.HOT_CROP.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x001584CC File Offset: 0x001566CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayTemperatureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.Temperature, base.gameObject));
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x00158514 File Offset: 0x00156714
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wiltTempRangeModAttribute = this.GetAttributes().Get(Db.Get().PlantAttributes.WiltTempRangeMod);
		this.temperatureRangeModScalar = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.RegisterUpdate1000ms(this);
		base.smi.sm.internalTemp.Set(this.primaryElement.Temperature, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x06003DC1 RID: 15809 RVA: 0x0015859E File Offset: 0x0015679E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.UnregisterUpdate1000ms(this);
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x001585B1 File Offset: 0x001567B1
	public void Configure(float tempWarningLow, float tempLethalLow, float tempWarningHigh, float tempLethalHigh)
	{
		this.internalTemperatureWarning_Low = tempWarningLow;
		this.internalTemperatureLethal_Low = tempLethalLow;
		this.internalTemperatureLethal_High = tempLethalHigh;
		this.internalTemperatureWarning_High = tempWarningHigh;
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x001585D0 File Offset: 0x001567D0
	public bool IsCellSafe(int cell)
	{
		float averageTemperature = this.GetAverageTemperature(cell);
		return averageTemperature > -1f && averageTemperature > this.TemperatureLethalLow && averageTemperature < this.internalTemperatureLethal_High;
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x00158604 File Offset: 0x00156804
	public void SlicedSim1000ms(float dt)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)))
		{
			return;
		}
		base.smi.sm.internalTemp.Set(this.InternalTemperature, base.smi, false);
		this.displayTemperatureAmount.value = this.InternalTemperature;
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x00158658 File Offset: 0x00156858
	private static bool GetAverageTemperatureCb(int cell, object data)
	{
		TemperatureVulnerable temperatureVulnerable = data as TemperatureVulnerable;
		if (Grid.Mass[cell] > 0.1f)
		{
			temperatureVulnerable.averageTemp += Grid.Temperature[cell];
			temperatureVulnerable.cellCount++;
		}
		return true;
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x001586A8 File Offset: 0x001568A8
	private float GetAverageTemperature(int cell)
	{
		this.averageTemp = 0f;
		this.cellCount = 0;
		this.occupyArea.TestArea(cell, this, TemperatureVulnerable.GetAverageTemperatureCbDelegate);
		if (this.cellCount > 0)
		{
			return this.averageTemp / (float)this.cellCount;
		}
		return -1f;
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x001586F8 File Offset: 0x001568F8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		float num = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		float temp = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningLow : (this.internalTemperatureWarning_Low + (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		float temp2 = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningHigh : (this.internalTemperatureWarning_High - (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04002615 RID: 9749
	private OccupyArea _occupyArea;

	// Token: 0x04002616 RID: 9750
	[SerializeField]
	private float internalTemperatureLethal_Low;

	// Token: 0x04002617 RID: 9751
	[SerializeField]
	private float internalTemperatureWarning_Low;

	// Token: 0x04002618 RID: 9752
	[SerializeField]
	private float internalTemperatureWarning_High;

	// Token: 0x04002619 RID: 9753
	[SerializeField]
	private float internalTemperatureLethal_High;

	// Token: 0x0400261A RID: 9754
	private AttributeInstance wiltTempRangeModAttribute;

	// Token: 0x0400261B RID: 9755
	private float temperatureRangeModScalar;

	// Token: 0x0400261C RID: 9756
	private const float minimumMassForReading = 0.1f;

	// Token: 0x0400261D RID: 9757
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400261E RID: 9758
	[MyCmpReq]
	private SimTemperatureTransfer temperatureTransfer;

	// Token: 0x0400261F RID: 9759
	private AmountInstance displayTemperatureAmount;

	// Token: 0x04002620 RID: 9760
	private TemperatureVulnerable.TemperatureState internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;

	// Token: 0x04002621 RID: 9761
	private float averageTemp;

	// Token: 0x04002622 RID: 9762
	private int cellCount;

	// Token: 0x04002623 RID: 9763
	private static readonly Func<int, object, bool> GetAverageTemperatureCbDelegate = (int cell, object data) => TemperatureVulnerable.GetAverageTemperatureCb(cell, data);

	// Token: 0x020018D3 RID: 6355
	public class StatesInstance : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.GameInstance
	{
		// Token: 0x0600A065 RID: 41061 RVA: 0x003A99C6 File Offset: 0x003A7BC6
		public StatesInstance(TemperatureVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x020018D4 RID: 6356
	public class States : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable>
	{
		// Token: 0x0600A066 RID: 41062 RVA: 0x003A99D0 File Offset: 0x003A7BD0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalCold;
			}).TriggerOnEnter(GameHashes.TooColdFatal, null).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalLow).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.lethalHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalHot;
			}).TriggerOnEnter(GameHashes.TooHotFatal, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalHigh).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.warningCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningCold;
			}).TriggerOnEnter(GameHashes.TooColdWarning, null).ParamTransition<float>(this.internalTemp, this.lethalCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalLow).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningLow);
			this.warningHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningHot;
			}).TriggerOnEnter(GameHashes.TooHotWarning, null).ParamTransition<float>(this.internalTemp, this.lethalHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalHigh).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningHigh);
			this.normal.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningHigh).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningLow);
		}

		// Token: 0x0600A067 RID: 41063 RVA: 0x003A9C98 File Offset: 0x003A7E98
		private static void Kill(StateMachine.Instance smi)
		{
			DeathMonitor.Instance smi2 = smi.GetSMI<DeathMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.Kill(Db.Get().Deaths.Generic);
			}
		}

		// Token: 0x04007C15 RID: 31765
		public StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.FloatParameter internalTemp;

		// Token: 0x04007C16 RID: 31766
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalCold;

		// Token: 0x04007C17 RID: 31767
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalHot;

		// Token: 0x04007C18 RID: 31768
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningCold;

		// Token: 0x04007C19 RID: 31769
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningHot;

		// Token: 0x04007C1A RID: 31770
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State normal;
	}

	// Token: 0x020018D5 RID: 6357
	public enum TemperatureState
	{
		// Token: 0x04007C1C RID: 31772
		LethalCold,
		// Token: 0x04007C1D RID: 31773
		WarningCold,
		// Token: 0x04007C1E RID: 31774
		Normal,
		// Token: 0x04007C1F RID: 31775
		WarningHot,
		// Token: 0x04007C20 RID: 31776
		LethalHot
	}
}
