using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008AD RID: 2221
public class IrrigationMonitor : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>
{
	// Token: 0x06003D41 RID: 15681 RVA: 0x00155D10 File Offset: 0x00153F10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.resourceStorage, this.unfertilizable, (IrrigationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(IrrigationMonitor.Instance smi)
		{
			if (smi.AcceptsLiquid())
			{
				smi.GoTo(this.replanted.irrigated);
			}
		});
		this.replanted.Enter(delegate(IrrigationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateIrrigation(0.033333335f);
		}).Target(this.resourceStorage).EventHandler(GameHashes.OnStorageChange, delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateIrrigation(0.2f);
		}).Target(this.masterTarget);
		this.replanted.irrigated.DefaultState(this.replanted.irrigated.absorbing).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.irrigated.absorbing.DefaultState(this.replanted.irrigated.absorbing.normal).ParamTransition<bool>(this.hasCorrectLiquid, this.replanted.starved, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (IrrigationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(true);
		}).EventHandler(GameHashes.TagsChanged, delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(true);
		}).Exit(delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(false);
		});
		this.replanted.irrigated.absorbing.normal.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
		this.replanted.irrigated.absorbing.wrongLiquid.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.enoughCorrectLiquidToRecover, this.replanted.irrigated.absorbing, (IrrigationMonitor.Instance smi, bool p) => p && this.hasCorrectLiquid.Get(smi)).ParamTransition<bool>(this.hasCorrectLiquid, this.replanted.irrigated.absorbing, (IrrigationMonitor.Instance smi, bool p) => p && this.enoughCorrectLiquidToRecover.Get(smi));
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.starved.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongLiquid.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.starved.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
	}

	// Token: 0x040025CA RID: 9674
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.TargetParameter resourceStorage;

	// Token: 0x040025CB RID: 9675
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasCorrectLiquid;

	// Token: 0x040025CC RID: 9676
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasIncorrectLiquid;

	// Token: 0x040025CD RID: 9677
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter enoughCorrectLiquidToRecover;

	// Token: 0x040025CE RID: 9678
	public GameHashes ResourceRecievedEvent = GameHashes.LiquidResourceRecieved;

	// Token: 0x040025CF RID: 9679
	public GameHashes ResourceDepletedEvent = GameHashes.LiquidResourceEmpty;

	// Token: 0x040025D0 RID: 9680
	public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wild;

	// Token: 0x040025D1 RID: 9681
	public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State unfertilizable;

	// Token: 0x040025D2 RID: 9682
	public IrrigationMonitor.ReplantedStates replanted;

	// Token: 0x020018A6 RID: 6310
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009FB1 RID: 40881 RVA: 0x003A749C File Offset: 0x003A569C
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

		// Token: 0x04007B8D RID: 31629
		public Tag wrongIrrigationTestTag;

		// Token: 0x04007B8E RID: 31630
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x020018A7 RID: 6311
	public class VariableIrrigationStates : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x04007B8F RID: 31631
		public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State normal;

		// Token: 0x04007B90 RID: 31632
		public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wrongLiquid;
	}

	// Token: 0x020018A8 RID: 6312
	public class Irrigated : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x04007B91 RID: 31633
		public IrrigationMonitor.VariableIrrigationStates absorbing;
	}

	// Token: 0x020018A9 RID: 6313
	public class ReplantedStates : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x04007B92 RID: 31634
		public IrrigationMonitor.Irrigated irrigated;

		// Token: 0x04007B93 RID: 31635
		public IrrigationMonitor.VariableIrrigationStates starved;
	}

	// Token: 0x020018AA RID: 6314
	public new class Instance : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06009FB6 RID: 40886 RVA: 0x003A7584 File Offset: 0x003A5784
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x06009FB7 RID: 40887 RVA: 0x003A758C File Offset: 0x003A578C
		public Instance(IStateMachineTarget master, IrrigationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x06009FB8 RID: 40888 RVA: 0x003A75CB File Offset: 0x003A57CB
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsIrrigation;
		}

		// Token: 0x06009FB9 RID: 40889 RVA: 0x003A75DC File Offset: 0x003A57DC
		public virtual StatusItem GetIncorrectLiquidStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongIrrigation;
		}

		// Token: 0x06009FBA RID: 40890 RVA: 0x003A75ED File Offset: 0x003A57ED
		public virtual StatusItem GetIncorrectLiquidStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongIrrigationMajor;
		}

		// Token: 0x06009FBB RID: 40891 RVA: 0x003A7600 File Offset: 0x003A5800
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.irrigation = amounts.Add(new AmountInstance(Db.Get().Amounts.Irrigation, gameObject));
		}

		// Token: 0x06009FBC RID: 40892 RVA: 0x003A7638 File Offset: 0x003A5838
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.IRRIGATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.IRRIGATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x06009FBD RID: 40893 RVA: 0x003A76B4 File Offset: 0x003A58B4
		public static void DumpIncorrectFertilizers(Storage storage, GameObject go)
		{
			if (storage == null)
			{
				return;
			}
			if (go == null)
			{
				return;
			}
			IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
			PlantElementAbsorber.ConsumeInfo[] consumed_infos = null;
			if (smi != null)
			{
				consumed_infos = smi.def.consumedElements;
			}
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos, false);
			FertilizationMonitor.Instance smi2 = go.GetSMI<FertilizationMonitor.Instance>();
			PlantElementAbsorber.ConsumeInfo[] consumed_infos2 = null;
			if (smi2 != null)
			{
				consumed_infos2 = smi2.def.consumedElements;
			}
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos2, true);
		}

		// Token: 0x06009FBE RID: 40894 RVA: 0x003A7718 File Offset: 0x003A5918
		private static void DumpIncorrectFertilizers(Storage storage, PlantElementAbsorber.ConsumeInfo[] consumed_infos, bool validate_solids)
		{
			if (storage == null)
			{
				return;
			}
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component == null) && !(gameObject.GetComponent<ElementChunk>() == null))
					{
						if (validate_solids)
						{
							if (!component.Element.IsSolid)
							{
								goto IL_C1;
							}
						}
						else if (!component.Element.IsLiquid)
						{
							goto IL_C1;
						}
						bool flag = false;
						KPrefabID component2 = component.GetComponent<KPrefabID>();
						if (consumed_infos != null)
						{
							foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in consumed_infos)
							{
								if (component2.HasTag(consumeInfo.tag))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							storage.Drop(gameObject, true);
						}
					}
				}
				IL_C1:;
			}
		}

		// Token: 0x06009FBF RID: 40895 RVA: 0x003A77F4 File Offset: 0x003A59F4
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.resourceStorage.Set(this.storage, base.smi);
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
					manualDeliveryKG.enabled = !this.storage.gameObject.GetComponent<PlantablePlot>().has_liquid_pipe_input;
				}
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06009FC0 RID: 40896 RVA: 0x003A78D4 File Offset: 0x003A5AD4
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Irrigation
				};
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06009FC1 RID: 40897 RVA: 0x003A78E0 File Offset: 0x003A5AE0
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.irrigated.absorbing.wrongLiquid))
				{
					result = this.GetIncorrectLiquidStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGIRRIGATION.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongLiquid))
				{
					result = this.GetIncorrectLiquidStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGIRRIGATIONMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSIRRIGATION.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x06009FC2 RID: 40898 RVA: 0x003A79C4 File Offset: 0x003A5BC4
		public virtual bool AcceptsLiquid()
		{
			PlantablePlot component = base.sm.resourceStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsIrrigation;
		}

		// Token: 0x06009FC3 RID: 40899 RVA: 0x003A79F9 File Offset: 0x003A5BF9
		public bool Starved()
		{
			return this.irrigation.value == 0f;
		}

		// Token: 0x06009FC4 RID: 40900 RVA: 0x003A7A10 File Offset: 0x003A5C10
		public void UpdateIrrigation(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			Storage storage = base.sm.resourceStorage.Get<Storage>(base.smi);
			bool flag = true;
			bool value = false;
			bool flag2 = true;
			if (storage != null)
			{
				List<GameObject> items = storage.items;
				for (int i = 0; i < base.def.consumedElements.Length; i++)
				{
					float num = 0f;
					PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
					for (int j = 0; j < items.Count; j++)
					{
						GameObject gameObject = items[j];
						if (gameObject.HasTag(consumeInfo.tag))
						{
							num += gameObject.GetComponent<PrimaryElement>().Mass;
						}
						else if (gameObject.HasTag(base.def.wrongIrrigationTestTag))
						{
							value = true;
						}
					}
					this.total_available_mass = num;
					float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
					if (num < consumeInfo.massConsumptionRate * totalValue * dt)
					{
						flag = false;
						break;
					}
					if (num < consumeInfo.massConsumptionRate * totalValue * (dt * 30f))
					{
						flag2 = false;
						break;
					}
				}
			}
			else
			{
				flag = false;
				flag2 = false;
				value = false;
			}
			base.sm.hasCorrectLiquid.Set(flag, base.smi, false);
			base.sm.hasIncorrectLiquid.Set(value, base.smi, false);
			base.sm.enoughCorrectLiquidToRecover.Set(flag2 && flag, base.smi, false);
		}

		// Token: 0x06009FC5 RID: 40901 RVA: 0x003A7BA4 File Offset: 0x003A5DA4
		public void UpdateAbsorbing(bool allow)
		{
			bool flag = allow && !base.smi.gameObject.HasTag(GameTags.Wilting);
			if (flag != this.absorberHandle.IsValid())
			{
				if (flag)
				{
					if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
					{
						return;
					}
					float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
					PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
					for (int i = 0; i < base.def.consumedElements.Length; i++)
					{
						PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
						consumeInfo.massConsumptionRate *= totalValue;
						array[i] = consumeInfo;
					}
					this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
					return;
				}
				else
				{
					this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
				}
			}
		}

		// Token: 0x04007B94 RID: 31636
		public AttributeModifier consumptionRate;

		// Token: 0x04007B95 RID: 31637
		public AttributeModifier absorptionRate;

		// Token: 0x04007B96 RID: 31638
		protected AmountInstance irrigation;

		// Token: 0x04007B97 RID: 31639
		private float total_available_mass;

		// Token: 0x04007B98 RID: 31640
		private Storage storage;

		// Token: 0x04007B99 RID: 31641
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;
	}
}
