using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008A4 RID: 2212
public class FlytrapConsumptionMonitor : StateMachineComponent<FlytrapConsumptionMonitor.Instance>, IGameObjectEffectDescriptor, IPlantConsumeEntities
{
	// Token: 0x06003CD6 RID: 15574 RVA: 0x00154161 File Offset: 0x00152361
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x00154174 File Offset: 0x00152374
	public string GetConsumableEntitiesCategoryName()
	{
		return CREATURES.SPECIES.FLYTRAPPLANT.VICTIM_IDENTIFIER;
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00154180 File Offset: 0x00152380
	public bool AreEntitiesConsumptionRequirementsSatisfied()
	{
		return base.smi != null && base.smi.HasEaten;
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x00154197 File Offset: 0x00152397
	public string GetRequirementText()
	{
		return CREATURES.SPECIES.FLYTRAPPLANT.PLANT_HUNGER_REQUIREMENT;
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x001541A3 File Offset: 0x001523A3
	public string GetConsumedEntityName()
	{
		if (base.smi != null)
		{
			return base.smi.LastConsumedEntityName;
		}
		return "Unknown Critter";
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x001541C0 File Offset: 0x001523C0
	public List<KPrefabID> GetPrefabsOfPossiblePrey()
	{
		List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(FlytrapConsumptionMonitor.CONSUMABLE_TAG);
		List<KPrefabID> list = new List<KPrefabID>();
		for (int i = 0; i < prefabsWithTag.Count; i++)
		{
			KPrefabID component = prefabsWithTag[i].GetComponent<KPrefabID>();
			if (this.IsEntityEdible(component) && !list.Contains(component) && Game.IsCorrectDlcActiveForCurrentSave(component))
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x00154220 File Offset: 0x00152420
	public string[] GetFormattedPossiblePreyList()
	{
		List<string> list = new List<string>();
		foreach (KPrefabID kprefabID in this.GetPrefabsOfPossiblePrey())
		{
			CreatureBrain component = kprefabID.GetComponent<CreatureBrain>();
			if (component != null)
			{
				string item = component.species.ProperName();
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x001542A4 File Offset: 0x001524A4
	public bool IsEntityEdible(GameObject entity)
	{
		return !(entity == null) && this.IsEntityEdible(entity.GetComponent<KPrefabID>());
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x001542BD File Offset: 0x001524BD
	public bool IsEntityEdible(KPrefabID entity)
	{
		return !(entity == null) && entity.HasTag(FlytrapConsumptionMonitor.CONSUMABLE_TAG) && entity.GetComponent<CreatureBrain>() != null && entity.GetComponent<OccupyArea>().OccupiedCellsOffsets.Length <= 1;
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x001542FC File Offset: 0x001524FC
	public List<Descriptor> GetDescriptors(GameObject obj)
	{
		return new List<Descriptor>
		{
			new Descriptor(this.GetRequirementText(), "", Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x0015431B File Offset: 0x0015251B
	public static bool IsWilted(FlytrapConsumptionMonitor.Instance smi)
	{
		return smi.IsWilted;
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00154323 File Offset: 0x00152523
	public static void CompleteEat(FlytrapConsumptionMonitor.Instance smi)
	{
		smi.sm.HasEaten.Set(true, smi, false);
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x0015433C File Offset: 0x0015253C
	public static void RetriggerGrowAnimationIfInGrowState(FlytrapConsumptionMonitor.Instance smi)
	{
		StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
		if (component == null || component.smi == null)
		{
			return;
		}
		if (component.smi.IsInsideState(component.smi.sm.alive.idle))
		{
			KBatchedAnimController component2 = smi.GetComponent<KBatchedAnimController>();
			if (component2 != null)
			{
				component2.Play(component.anims.grow, component.anims.grow_playmode, 1f, 0f);
			}
		}
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x001543BF File Offset: 0x001525BF
	public static void BecomeHungry(FlytrapConsumptionMonitor.Instance smi)
	{
		smi.sm.HasEaten.Set(false, smi, false);
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x001543D5 File Offset: 0x001525D5
	public static void RegisterVictimProximityMonitor(FlytrapConsumptionMonitor.Instance smi)
	{
		smi.RegisterVictimProximityMonitor();
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x001543DD File Offset: 0x001525DD
	public static void UnregisterVictimProximityMonitor(FlytrapConsumptionMonitor.Instance smi)
	{
		smi.UnregisterVictimProximityMonitor();
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x001543E8 File Offset: 0x001525E8
	public static void SetAndPlayConsumeCropPlantAnimations(FlytrapConsumptionMonitor.Instance smi)
	{
		StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
		if (component == null || component.smi == null)
		{
			return;
		}
		component.anims = FlytrapConsumptionMonitor.EATING_STATE_ANIM_SET;
		component.smi.GoTo(component.smi.sm.alive.pre_idle);
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x0015443C File Offset: 0x0015263C
	public static void SetCropPlantAnimationsToAwaitPrey(FlytrapConsumptionMonitor.Instance smi)
	{
		FlytrapConsumptionMonitor.SetCropPlantAnimationSet(smi, FlytrapConsumptionMonitor.HUNGRY_STATE_ANIM_SET);
		FlytrapConsumptionMonitor.RetriggerGrowAnimationIfInGrowState(smi);
		StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
		if (component == null || component.smi == null)
		{
			return;
		}
		component.preventGrowPositionUpdate = true;
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x0015447C File Offset: 0x0015267C
	public static void RestoreDefaultCropPlantAnimations(FlytrapConsumptionMonitor.Instance smi)
	{
		FlytrapConsumptionMonitor.SetCropPlantAnimationSet(smi, FlyTrapPlantConfig.Default_StandardCropAnimSet);
		StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
		if (component == null || component.smi == null)
		{
			return;
		}
		component.preventGrowPositionUpdate = false;
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x001544B4 File Offset: 0x001526B4
	private static void SetCropPlantAnimationSet(FlytrapConsumptionMonitor.Instance smi, StandardCropPlant.AnimSet set)
	{
		StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
		if (component == null || component.smi == null)
		{
			return;
		}
		component.anims = set;
	}

	// Token: 0x04002594 RID: 9620
	public const string AWAIT_PREY_ANIM_NAME = "awaiting_prey";

	// Token: 0x04002595 RID: 9621
	public const string EAT_ANIM_NAME = "consume";

	// Token: 0x04002596 RID: 9622
	private const string CONSUMED_ENTITY_NAME_FALLBACK = "Unknown Critter";

	// Token: 0x04002597 RID: 9623
	private static Tag CONSUMABLE_TAG = GameTags.Creatures.Flyer;

	// Token: 0x04002598 RID: 9624
	public static readonly StandardCropPlant.AnimSet HUNGRY_STATE_ANIM_SET = new StandardCropPlant.AnimSet(FlyTrapPlantConfig.Default_StandardCropAnimSet)
	{
		grow = "awaiting_prey",
		wilt_base = "flower_wilt",
		grow_playmode = KAnim.PlayMode.Loop
	};

	// Token: 0x04002599 RID: 9625
	public static readonly StandardCropPlant.AnimSet EATING_STATE_ANIM_SET = new StandardCropPlant.AnimSet(FlyTrapPlantConfig.Default_StandardCropAnimSet)
	{
		pre_grow = "consume",
		grow_playmode = KAnim.PlayMode.Paused
	};

	// Token: 0x02001895 RID: 6293
	public class States : GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor>
	{
		// Token: 0x06009F79 RID: 40825 RVA: 0x003A656C File Offset: 0x003A476C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.hungry;
			this.hungry.ParamTransition<bool>(this.HasEaten, this.satisfied, GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.IsTrue).Toggle("Toggle Standard Crop Plant Animations", new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.SetCropPlantAnimationsToAwaitPrey), new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.RestoreDefaultCropPlantAnimations)).ToggleAttributeModifier("Pause Growing", (FlytrapConsumptionMonitor.Instance smi) => smi.pauseGrowing, null).DefaultState(this.hungry.idle);
			this.hungry.idle.EventTransition(GameHashes.Wilt, this.hungry.wilt, new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Transition.ConditionCallback(FlytrapConsumptionMonitor.IsWilted)).ToggleStatusItem(Db.Get().CreatureStatusItems.CarnivorousPlantAwaitingVictim, (FlytrapConsumptionMonitor.Instance smi) => smi.master.GetComponent<IPlantConsumeEntities>()).Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.RegisterVictimProximityMonitor)).TriggerOnEnter(GameHashes.CropSleep, null).OnSignal(this.EatSignal, this.hungry.complete).Exit(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.UnregisterVictimProximityMonitor));
			this.hungry.complete.Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.SetAndPlayConsumeCropPlantAnimations)).Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.CompleteEat));
			this.hungry.wilt.EventTransition(GameHashes.WiltRecover, this.hungry.idle, GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Not(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Transition.ConditionCallback(FlytrapConsumptionMonitor.IsWilted)));
			this.satisfied.Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.RetriggerGrowAnimationIfInGrowState)).TriggerOnEnter(GameHashes.CropWakeUp, null).ParamTransition<bool>(this.HasEaten, this.hungry, GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.IsFalse).EventHandler(GameHashes.Harvest, new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.BecomeHungry));
		}

		// Token: 0x04007B5C RID: 31580
		public FlytrapConsumptionMonitor.States.HungryStates hungry;

		// Token: 0x04007B5D RID: 31581
		public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State satisfied;

		// Token: 0x04007B5E RID: 31582
		public StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.BoolParameter HasEaten;

		// Token: 0x04007B5F RID: 31583
		public StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Signal EatSignal;

		// Token: 0x02002989 RID: 10633
		public class HungryStates : GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State
		{
			// Token: 0x0400B7A8 RID: 47016
			public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State wilt;

			// Token: 0x0400B7A9 RID: 47017
			public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State idle;

			// Token: 0x0400B7AA RID: 47018
			public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State complete;
		}
	}

	// Token: 0x02001896 RID: 6294
	public class Instance : GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.GameInstance
	{
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06009F7B RID: 40827 RVA: 0x003A675B File Offset: 0x003A495B
		public bool HasEaten
		{
			get
			{
				return base.sm.HasEaten.Get(this);
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06009F7C RID: 40828 RVA: 0x003A676E File Offset: 0x003A496E
		public bool IsWilted
		{
			get
			{
				return this.wiltCondition.IsWilting();
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06009F7D RID: 40829 RVA: 0x003A677B File Offset: 0x003A497B
		public string LastConsumedEntityName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.lastConsumedEntityPrefabID))
				{
					return Assets.GetPrefab(this.lastConsumedEntityPrefabID).GetProperName();
				}
				return "Unknown Critter";
			}
		}

		// Token: 0x06009F7E RID: 40830 RVA: 0x003A67A8 File Offset: 0x003A49A8
		public Instance(FlytrapConsumptionMonitor master) : base(master)
		{
			Amounts amounts = base.gameObject.GetAmounts();
			this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
			this.pauseGrowing = new AttributeModifier(this.maturity.deltaAttribute.Id, -1f, CREATURES.SPECIES.FLYTRAPPLANT.HUNGRY, true, false, true);
			this.wiltCondition = base.GetComponent<WiltCondition>();
			this.growing = base.GetComponent<Growing>();
			this.growing.CustomGrowStallCondition_IsStalled = new Func<GameObject, bool>(this.ShouldStallGrowingComponent);
		}

		// Token: 0x06009F7F RID: 40831 RVA: 0x003A684A File Offset: 0x003A4A4A
		private bool ShouldStallGrowingComponent(GameObject plantGameObject)
		{
			return !this.HasEaten;
		}

		// Token: 0x06009F80 RID: 40832 RVA: 0x003A6858 File Offset: 0x003A4A58
		public void RegisterVictimProximityMonitor()
		{
			OccupyArea component = base.GetComponent<OccupyArea>();
			this.partitionerEntry = GameScenePartitioner.Instance.Add("FlytrapConsumptionMonitor.hungry.idle", base.gameObject, component.GetExtents(), GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupableLayerObjectDetected));
		}

		// Token: 0x06009F81 RID: 40833 RVA: 0x003A68A3 File Offset: 0x003A4AA3
		public void UnregisterVictimProximityMonitor()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			this.partitionerEntry = HandleVector<int>.InvalidHandle;
		}

		// Token: 0x06009F82 RID: 40834 RVA: 0x003A68C0 File Offset: 0x003A4AC0
		public void OnPickupableLayerObjectDetected(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (base.master.IsEntityEdible(pickupable.gameObject))
			{
				this.lastConsumedEntityPrefabID = pickupable.PrefabID().ToString();
				pickupable.gameObject.DeleteObject();
				base.sm.EatSignal.Trigger(this);
			}
		}

		// Token: 0x04007B60 RID: 31584
		public AttributeModifier pauseGrowing;

		// Token: 0x04007B61 RID: 31585
		[Serialize]
		private string lastConsumedEntityPrefabID;

		// Token: 0x04007B62 RID: 31586
		private Growing growing;

		// Token: 0x04007B63 RID: 31587
		private WiltCondition wiltCondition;

		// Token: 0x04007B64 RID: 31588
		private AmountInstance maturity;

		// Token: 0x04007B65 RID: 31589
		private HandleVector<int>.Handle partitionerEntry = HandleVector<int>.InvalidHandle;
	}
}
