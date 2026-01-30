using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A34 RID: 2612
public class MilkProductionMonitor : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>
{
	// Token: 0x06004C40 RID: 19520 RVA: 0x001BB4AC File Offset: 0x001B96AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.producing;
		this.producing.DefaultState(this.producing.paused).EventHandler(GameHashes.CaloriesConsumed, delegate(MilkProductionMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.producing.paused.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.producing, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing), UpdateRate.SIM_1000ms);
		this.producing.producing.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing)), UpdateRate.SIM_1000ms).ToggleCritterEmotion(Db.Get().CritterEmotions.WellFed, null);
		this.producing.full.ToggleStatusItem(Db.Get().CreatureStatusItems.MilkFull, null).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull)), UpdateRate.SIM_1000ms).Enter(delegate(MilkProductionMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Creatures.RequiresMilking);
		});
	}

	// Token: 0x06004C41 RID: 19521 RVA: 0x001BB615 File Offset: 0x001B9815
	private static bool IsProducing(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull && smi.IsUnderProductionEffect;
	}

	// Token: 0x06004C42 RID: 19522 RVA: 0x001BB627 File Offset: 0x001B9827
	private static bool IsFull(MilkProductionMonitor.Instance smi)
	{
		return smi.IsFull;
	}

	// Token: 0x06004C43 RID: 19523 RVA: 0x001BB62F File Offset: 0x001B982F
	private static bool HasCapacity(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull;
	}

	// Token: 0x040032AA RID: 12970
	public MilkProductionMonitor.ProducingStates producing;

	// Token: 0x02001B05 RID: 6917
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600A81F RID: 43039 RVA: 0x003BE57D File Offset: 0x003BC77D
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.MilkProduction.Id);
		}

		// Token: 0x0400838F RID: 33679
		public SimHashes element = SimHashes.Milk;

		// Token: 0x04008390 RID: 33680
		public Func<CreatureCalorieMonitor.CaloriesConsumedEvent, bool> dietConditionForMilkProduction;

		// Token: 0x04008391 RID: 33681
		public string effectId;

		// Token: 0x04008392 RID: 33682
		public float Capacity = 200f;

		// Token: 0x04008393 RID: 33683
		public float CaloriesPerCycle = 1000f;

		// Token: 0x04008394 RID: 33684
		public float HappinessRequired;
	}

	// Token: 0x02001B06 RID: 6918
	public class ProducingStates : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State
	{
		// Token: 0x04008395 RID: 33685
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State paused;

		// Token: 0x04008396 RID: 33686
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State producing;

		// Token: 0x04008397 RID: 33687
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State full;
	}

	// Token: 0x02001B07 RID: 6919
	public new class Instance : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.GameInstance
	{
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x0600A822 RID: 43042 RVA: 0x003BE5D4 File Offset: 0x003BC7D4
		public float MilkAmount
		{
			get
			{
				return this.MilkPercentage / 100f * base.def.Capacity;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600A823 RID: 43043 RVA: 0x003BE5EE File Offset: 0x003BC7EE
		public float MilkPercentage
		{
			get
			{
				return this.milkAmountInstance.value;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x0600A824 RID: 43044 RVA: 0x003BE5FB File Offset: 0x003BC7FB
		public bool IsFull
		{
			get
			{
				return this.MilkPercentage >= this.milkAmountInstance.GetMax();
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x0600A825 RID: 43045 RVA: 0x003BE613 File Offset: 0x003BC813
		public bool IsUnderProductionEffect
		{
			get
			{
				return this.milkAmountInstance.GetDelta() > 0f;
			}
		}

		// Token: 0x0600A826 RID: 43046 RVA: 0x003BE627 File Offset: 0x003BC827
		public Instance(IStateMachineTarget master, MilkProductionMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A827 RID: 43047 RVA: 0x003BE634 File Offset: 0x003BC834
		public override void StartSM()
		{
			this.milkAmountInstance = Db.Get().Amounts.MilkProduction.Lookup(base.gameObject);
			if (base.def.effectId != null)
			{
				this.effectInstance = this.effects.Get(base.smi.def.effectId);
			}
			base.StartSM();
		}

		// Token: 0x0600A828 RID: 43048 RVA: 0x003BE698 File Offset: 0x003BC898
		public void OnCaloriesConsumed(object data)
		{
			if (base.def.effectId == null)
			{
				return;
			}
			CreatureCalorieMonitor.CaloriesConsumedEvent value = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
			if (base.def.dietConditionForMilkProduction != null && !base.def.dietConditionForMilkProduction(value))
			{
				return;
			}
			this.effectInstance = this.effects.Get(base.smi.def.effectId);
			if (this.effectInstance == null)
			{
				this.effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			this.effectInstance.timeRemaining += value.calories / base.smi.def.CaloriesPerCycle * 600f;
		}

		// Token: 0x0600A829 RID: 43049 RVA: 0x003BE75C File Offset: 0x003BC95C
		private void RemoveMilk(float amount)
		{
			if (this.milkAmountInstance != null)
			{
				float value = Mathf.Min(this.milkAmountInstance.GetMin(), this.MilkPercentage - amount);
				this.milkAmountInstance.SetValue(value);
			}
		}

		// Token: 0x0600A82A RID: 43050 RVA: 0x003BE798 File Offset: 0x003BC998
		public PrimaryElement ExtractMilk(float desiredAmount)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (num <= 0f)
			{
				return null;
			}
			this.RemoveMilk(num);
			PrimaryElement component = LiquidSourceManager.Instance.CreateChunk(base.def.element, num, temperature, 0, 0, base.transform.GetPosition()).GetComponent<PrimaryElement>();
			component.KeepZeroMassObject = false;
			return component;
		}

		// Token: 0x0600A82B RID: 43051 RVA: 0x003BE800 File Offset: 0x003BCA00
		public PrimaryElement ExtractMilkIntoElementChunk(float desiredAmount, PrimaryElement elementChunk)
		{
			if (elementChunk == null || elementChunk.ElementID != base.def.element)
			{
				return null;
			}
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			float mass = elementChunk.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(elementChunk.Temperature, mass, temperature, num);
			elementChunk.SetMassTemperature(mass + num, finalTemperature);
			return elementChunk;
		}

		// Token: 0x0600A82C RID: 43052 RVA: 0x003BE86C File Offset: 0x003BCA6C
		public PrimaryElement ExtractMilkIntoStorage(float desiredAmount, Storage storage)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			return storage.AddLiquid(base.def.element, num, temperature, 0, 0, false, true);
		}

		// Token: 0x04008398 RID: 33688
		public Action<float> OnMilkAmountChanged;

		// Token: 0x04008399 RID: 33689
		public AmountInstance milkAmountInstance;

		// Token: 0x0400839A RID: 33690
		public EffectInstance effectInstance;

		// Token: 0x0400839B RID: 33691
		[MyCmpGet]
		private Effects effects;
	}
}
