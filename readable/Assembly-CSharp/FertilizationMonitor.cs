using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
public class FertilizationMonitor : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>
{
	// Token: 0x06003CC7 RID: 15559 RVA: 0x00153C24 File Offset: 0x00151E24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.fertilizerStorage, this.unfertilizable, (FertilizationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			if (smi.AcceptsFertilizer())
			{
				smi.GoTo(this.replanted.fertilized);
			}
		});
		this.replanted.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateFertilization(0.033333335f);
		}).Target(this.fertilizerStorage).EventHandler(GameHashes.OnStorageChange, delegate(FertilizationMonitor.Instance smi)
		{
			smi.UpdateFertilization(0.2f);
		}).Target(this.masterTarget);
		this.replanted.fertilized.DefaultState(this.replanted.fertilized.decaying).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.fertilized.decaying.DefaultState(this.replanted.fertilized.decaying.normal).ToggleAttributeModifier("Consuming", (FertilizationMonitor.Instance smi) => smi.consumptionRate, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.absorbing, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue).Update("Decaying", delegate(FertilizationMonitor.Instance smi, float dt)
		{
			if (smi.Starved())
			{
				smi.GoTo(this.replanted.starved);
			}
		}, UpdateRate.SIM_200ms, false);
		this.replanted.fertilized.decaying.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.decaying.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.fertilized.absorbing.DefaultState(this.replanted.fertilized.absorbing.normal).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.decaying, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (FertilizationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).EventHandler(GameHashes.Wilt, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		}).EventHandler(GameHashes.WiltRecover, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).Exit(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		});
		this.replanted.fertilized.absorbing.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.absorbing.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
	}

	// Token: 0x0400258A RID: 9610
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.TargetParameter fertilizerStorage;

	// Token: 0x0400258B RID: 9611
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasCorrectFertilizer;

	// Token: 0x0400258C RID: 9612
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasIncorrectFertilizer;

	// Token: 0x0400258D RID: 9613
	public GameHashes ResourceRecievedEvent = GameHashes.Fertilized;

	// Token: 0x0400258E RID: 9614
	public GameHashes ResourceDepletedEvent = GameHashes.Unfertilized;

	// Token: 0x0400258F RID: 9615
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wild;

	// Token: 0x04002590 RID: 9616
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State unfertilizable;

	// Token: 0x04002591 RID: 9617
	public FertilizationMonitor.ReplantedStates replanted;

	// Token: 0x0200188D RID: 6285
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009F57 RID: 40791 RVA: 0x003A5E04 File Offset: 0x003A4004
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.consumedElements.Length != 0)
			{
				List<Descriptor> list = new List<Descriptor>();
				float preModifiedAttributeValue = obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.FertilizerUsageMod);
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in this.consumedElements)
				{
					float num = consumeInfo.massConsumptionRate * preModifiedAttributeValue;
					list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(-num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
				}
				return list;
			}
			return null;
		}

		// Token: 0x04007B42 RID: 31554
		public Tag wrongFertilizerTestTag;

		// Token: 0x04007B43 RID: 31555
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x0200188E RID: 6286
	public class VariableFertilizerStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x04007B44 RID: 31556
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State normal;

		// Token: 0x04007B45 RID: 31557
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wrongFert;
	}

	// Token: 0x0200188F RID: 6287
	public class FertilizedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x04007B46 RID: 31558
		public FertilizationMonitor.VariableFertilizerStates decaying;

		// Token: 0x04007B47 RID: 31559
		public FertilizationMonitor.VariableFertilizerStates absorbing;

		// Token: 0x04007B48 RID: 31560
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wilting;
	}

	// Token: 0x02001890 RID: 6288
	public class ReplantedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x04007B49 RID: 31561
		public FertilizationMonitor.FertilizedStates fertilized;

		// Token: 0x04007B4A RID: 31562
		public FertilizationMonitor.VariableFertilizerStates starved;
	}

	// Token: 0x02001891 RID: 6289
	public new class Instance : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06009F5C RID: 40796 RVA: 0x003A5EEC File Offset: 0x003A40EC
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x06009F5D RID: 40797 RVA: 0x003A5EF4 File Offset: 0x003A40F4
		public Instance(IStateMachineTarget master, FertilizationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x06009F5E RID: 40798 RVA: 0x003A5F33 File Offset: 0x003A4133
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsFertilizer;
		}

		// Token: 0x06009F5F RID: 40799 RVA: 0x003A5F44 File Offset: 0x003A4144
		public virtual StatusItem GetIncorrectFertStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizer;
		}

		// Token: 0x06009F60 RID: 40800 RVA: 0x003A5F55 File Offset: 0x003A4155
		public virtual StatusItem GetIncorrectFertStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizerMajor;
		}

		// Token: 0x06009F61 RID: 40801 RVA: 0x003A5F68 File Offset: 0x003A4168
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.fertilization = amounts.Add(new AmountInstance(Db.Get().Amounts.Fertilization, gameObject));
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06009F62 RID: 40802 RVA: 0x003A5F9D File Offset: 0x003A419D
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Fertilized
				};
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06009F63 RID: 40803 RVA: 0x003A5FAC File Offset: 0x003A41AC
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.decaying.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.absorbing.wrongFert))
				{
					result = this.GetIncorrectFertStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x06009F64 RID: 40804 RVA: 0x003A60E0 File Offset: 0x003A42E0
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.FERTILIZATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.FERTILIZATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x06009F65 RID: 40805 RVA: 0x003A615C File Offset: 0x003A435C
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.fertilizerStorage.Set(this.storage, base.smi);
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(this.storage, base.smi.gameObject);
			foreach (ManualDeliveryKG manualDeliveryKG in base.smi.gameObject.GetComponents<ManualDeliveryKG>())
			{
				bool flag = false;
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in base.def.consumedElements)
				{
					if (manualDeliveryKG.RequestedItemTag == consumeInfo.tag)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					manualDeliveryKG.SetStorage(this.storage);
					manualDeliveryKG.enabled = true;
				}
			}
		}

		// Token: 0x06009F66 RID: 40806 RVA: 0x003A6228 File Offset: 0x003A4428
		public virtual bool AcceptsFertilizer()
		{
			PlantablePlot component = base.sm.fertilizerStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsFertilizer;
		}

		// Token: 0x06009F67 RID: 40807 RVA: 0x003A625D File Offset: 0x003A445D
		public bool Starved()
		{
			return this.fertilization.value == 0f;
		}

		// Token: 0x06009F68 RID: 40808 RVA: 0x003A6274 File Offset: 0x003A4474
		public void UpdateFertilization(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			if (this.storage == null)
			{
				return;
			}
			bool value = true;
			bool value2 = false;
			List<GameObject> items = this.storage.items;
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (gameObject.HasTag(consumeInfo.tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
					else if (gameObject.HasTag(base.def.wrongFertilizerTestTag))
					{
						value2 = true;
					}
				}
				this.total_available_mass = num;
				float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
				if (num < consumeInfo.massConsumptionRate * totalValue * dt)
				{
					value = false;
					break;
				}
			}
			base.sm.hasCorrectFertilizer.Set(value, base.smi, false);
			base.sm.hasIncorrectFertilizer.Set(value2, base.smi, false);
		}

		// Token: 0x06009F69 RID: 40809 RVA: 0x003A63B4 File Offset: 0x003A45B4
		public void StartAbsorbing()
		{
			if (this.absorberHandle.IsValid())
			{
				return;
			}
			if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
			{
				return;
			}
			GameObject gameObject = base.smi.gameObject;
			float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				consumeInfo.massConsumptionRate *= totalValue;
				array[i] = consumeInfo;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
		}

		// Token: 0x06009F6A RID: 40810 RVA: 0x003A6489 File Offset: 0x003A4689
		public void StopAbsorbing()
		{
			if (!this.absorberHandle.IsValid())
			{
				return;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
		}

		// Token: 0x04007B4B RID: 31563
		public AttributeModifier consumptionRate;

		// Token: 0x04007B4C RID: 31564
		public AttributeModifier absorptionRate;

		// Token: 0x04007B4D RID: 31565
		protected AmountInstance fertilization;

		// Token: 0x04007B4E RID: 31566
		private Storage storage;

		// Token: 0x04007B4F RID: 31567
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;

		// Token: 0x04007B50 RID: 31568
		private float total_available_mass;
	}
}
