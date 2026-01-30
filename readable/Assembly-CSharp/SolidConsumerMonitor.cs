using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005CC RID: 1484
public class SolidConsumerMonitor : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>
{
	// Token: 0x06002207 RID: 8711 RVA: 0x000C5594 File Offset: 0x000C3794
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.EatSolidComplete, delegate(SolidConsumerMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToEat, (SolidConsumerMonitor.Instance smi) => smi.targetEdible != null && !smi.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature), null);
		this.satisfied.TagTransition(GameTags.Creatures.Hungry, this.lookingforfood, false);
		this.lookingforfood.TagTransition(GameTags.Creatures.Hungry, this.satisfied, true).PreBrainUpdate(new Action<SolidConsumerMonitor.Instance>(SolidConsumerMonitor.FindFood));
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000C5644 File Offset: 0x000C3844
	private static void FindFood(SolidConsumerMonitor.Instance smi)
	{
		if (smi.IsTargetEdibleValid())
		{
			return;
		}
		smi.ClearTargetEdible();
		Diet diet = smi.diet;
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(smi.gameObject.transform.GetPosition(), out num, out num2);
		num -= 8;
		num2 -= 8;
		bool flag = false;
		if (diet.CanEatPreyCritter)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
			KPrefabID kprefabID = null;
			int num3 = int.MaxValue;
			if (cavityForCell != null)
			{
				foreach (KPrefabID kprefabID2 in cavityForCell.creatures)
				{
					if (!kprefabID2.HasTag(GameTags.Creatures.ReservedByCreature) && diet.GetDietInfo(kprefabID2.PrefabTag) != null)
					{
						int cost = smi.GetCost(kprefabID2.gameObject);
						if (cost != -1 && (cost < num3 || num3 == -1))
						{
							kprefabID = kprefabID2;
							num3 = cost;
						}
					}
					if (kprefabID != null && num3 < 3)
					{
						break;
					}
				}
			}
			if (kprefabID != null)
			{
				smi.SetTargetEdible(kprefabID.gameObject, num3);
				smi.targetEdibleOffset = smi.GetBestEdibleOffset(kprefabID.gameObject);
				flag = true;
			}
		}
		bool flag2 = false;
		if (!flag && diet.CanEatAnySolid)
		{
			ListPool<Storage, SolidConsumerMonitor>.PooledList pooledList = ListPool<Storage, SolidConsumerMonitor>.Allocate();
			int num4 = 32;
			foreach (CreatureFeeder creatureFeeder in Components.CreatureFeeders.GetItems(smi.GetMyWorldId()))
			{
				Vector2I targetFeederCell = creatureFeeder.GetTargetFeederCell();
				if (targetFeederCell.x >= num && targetFeederCell.x <= num + num4 && targetFeederCell.y >= num2 && targetFeederCell.y <= num2 + num4 && !creatureFeeder.StoragesAreEmpty())
				{
					int cost2 = smi.GetCost(Grid.XYToCell(targetFeederCell.x, targetFeederCell.y));
					if (smi.IsCloserThanTargetEdible(cost2))
					{
						foreach (Storage storage in creatureFeeder.storages)
						{
							if (!(storage == null) && !storage.IsEmpty() && smi.GetCost(Grid.PosToCell(storage.items[0])) != -1)
							{
								foreach (GameObject gameObject in storage.items)
								{
									if (!(gameObject == null))
									{
										KPrefabID component = gameObject.GetComponent<KPrefabID>();
										if (!component.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(component.PrefabTag) != null)
										{
											smi.SetTargetEdible(gameObject, cost2);
											smi.targetEdibleOffset = Vector3.zero;
											flag2 = true;
											break;
										}
									}
								}
								if (flag2)
								{
									break;
								}
							}
						}
					}
				}
			}
			pooledList.Recycle();
		}
		bool flag3 = false;
		if (!flag && !flag2 && diet.CanEatAnyPlantDirectly)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList2 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num, num2, 16, 16, GameScenePartitioner.Instance.plants, pooledList2);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList2)
			{
				KPrefabID kprefabID3 = (KPrefabID)scenePartitionerEntry.obj;
				Diet.Info dietInfo = diet.GetDietInfo(kprefabID3.PrefabTag);
				Vector3 vector = kprefabID3.transform.GetPosition();
				bool flag4 = kprefabID3.HasTag(GameTags.PlantedOnFloorVessel);
				if (flag4)
				{
					vector += SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET;
				}
				int num5 = smi.GetCost(Grid.PosToCell(vector));
				Vector3 a = Vector3.zero;
				if (smi.IsCloserThanTargetEdible(num5) && !kprefabID3.HasAnyTags(SolidConsumerMonitor.creatureTags) && dietInfo != null)
				{
					if (kprefabID3.HasTag(GameTags.Plant))
					{
						IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(kprefabID3.gameObject);
						if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
						{
							continue;
						}
						bool flag5 = false;
						foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
						{
							if (plantConsumptionInstructions2.CanPlantBeEaten() && dietInfo.foodType == plantConsumptionInstructions2.GetDietFoodType())
							{
								CellOffset[] allowedOffsets = plantConsumptionInstructions2.GetAllowedOffsets();
								if (allowedOffsets != null)
								{
									num5 = -1;
									foreach (CellOffset offset in allowedOffsets)
									{
										int cost3 = smi.GetCost(Grid.OffsetCell(Grid.PosToCell(vector), offset));
										if (cost3 != -1 && (num5 == -1 || cost3 < num5))
										{
											num5 = cost3;
											a = offset.ToVector3();
										}
									}
									if (num5 != -1)
									{
										flag5 = true;
										break;
									}
								}
								else
								{
									flag5 = true;
								}
							}
						}
						if (!flag5)
						{
							continue;
						}
					}
					smi.SetTargetEdible(kprefabID3.gameObject, num5);
					smi.targetEdibleOffset = a + (flag4 ? SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET : Vector3.zero);
					flag3 = true;
				}
			}
			pooledList2.Recycle();
		}
		if (!flag && !flag2 && !flag3 && diet.CanEatAnySolid)
		{
			bool flag6 = false;
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num, num2, 16, 16, GameScenePartitioner.Instance.pickupablesLayer, pooledList3);
			foreach (ScenePartitionerEntry scenePartitionerEntry2 in pooledList3)
			{
				Pickupable pickupable = (Pickupable)scenePartitionerEntry2.obj;
				KPrefabID kprefabID4 = pickupable.KPrefabID;
				if (!kprefabID4.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(kprefabID4.PrefabTag) != null)
				{
					bool flag7;
					smi.ProcessEdible(pickupable.gameObject, out flag7);
					smi.targetEdibleOffset = Vector3.zero;
					flag6 = (flag6 || flag7);
				}
			}
			pooledList3.Recycle();
		}
	}

	// Token: 0x040013D7 RID: 5079
	public static Vector3 PLANT_ON_FLOOR_VESSEL_OFFSET = Vector3.down;

	// Token: 0x040013D8 RID: 5080
	private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State satisfied;

	// Token: 0x040013D9 RID: 5081
	private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State lookingforfood;

	// Token: 0x040013DA RID: 5082
	private static Tag[] creatureTags = new Tag[]
	{
		GameTags.Creatures.ReservedByCreature,
		GameTags.CreatureBrain
	};

	// Token: 0x02001497 RID: 5271
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006EFC RID: 28412
		public Diet diet;

		// Token: 0x04006EFD RID: 28413
		public Vector3[] possibleEatPositionOffsets = new Vector3[]
		{
			Vector3.zero
		};

		// Token: 0x04006EFE RID: 28414
		public Vector2 navigatorSize = Vector2.one;
	}

	// Token: 0x02001498 RID: 5272
	public new class Instance : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x0600904D RID: 36941 RVA: 0x0036E1EB File Offset: 0x0036C3EB
		public Instance(IStateMachineTarget master, SolidConsumerMonitor.Def def) : base(master, def)
		{
			this.diet = DietManager.Instance.GetPrefabDiet(base.gameObject);
		}

		// Token: 0x0600904E RID: 36942 RVA: 0x0036E20B File Offset: 0x0036C40B
		public bool CanSearchForPickupables(bool foodAtFeeder)
		{
			return !foodAtFeeder;
		}

		// Token: 0x0600904F RID: 36943 RVA: 0x0036E211 File Offset: 0x0036C411
		public bool IsCloserThanTargetEdible(int cost)
		{
			return cost != -1 && (cost < this.targetEdibleCost || this.targetEdibleCost == -1);
		}

		// Token: 0x06009050 RID: 36944 RVA: 0x0036E230 File Offset: 0x0036C430
		public bool IsTargetEdibleValid()
		{
			if (this.targetEdible == null)
			{
				return false;
			}
			int cost = this.GetCost(Grid.PosToCell(this.targetEdible.transform.GetPosition() + this.targetEdibleOffset));
			return cost != -1 && cost <= this.targetEdibleCost + 4;
		}

		// Token: 0x06009051 RID: 36945 RVA: 0x0036E286 File Offset: 0x0036C486
		public void ClearTargetEdible()
		{
			this.targetEdibleCost = -1;
			this.targetEdible = null;
			this.targetEdibleOffset = Vector3.zero;
		}

		// Token: 0x06009052 RID: 36946 RVA: 0x0036E2A4 File Offset: 0x0036C4A4
		public bool ProcessEdible(GameObject edible, out bool isReachable)
		{
			int cost = this.GetCost(edible);
			isReachable = (cost != -1);
			if (cost != -1 && (cost < this.targetEdibleCost || this.targetEdibleCost == -1))
			{
				this.SetTargetEdible(edible, cost);
				return true;
			}
			return false;
		}

		// Token: 0x06009053 RID: 36947 RVA: 0x0036E2E3 File Offset: 0x0036C4E3
		public void SetTargetEdible(GameObject gameObject, int cost)
		{
			if (this.targetEdible == gameObject)
			{
				return;
			}
			this.targetEdibleCost = cost;
			this.targetEdible = gameObject;
		}

		// Token: 0x06009054 RID: 36948 RVA: 0x0036E302 File Offset: 0x0036C502
		public int GetCost(GameObject edible)
		{
			return this.GetCost(Grid.PosToCell(edible.transform.GetPosition() + base.smi.GetBestEdibleOffset(edible)));
		}

		// Token: 0x06009055 RID: 36949 RVA: 0x0036E32C File Offset: 0x0036C52C
		public int GetCost(int cell)
		{
			if (this.drowningMonitor != null && this.drowningMonitor.canDrownToDeath && !this.drowningMonitor.livesUnderWater && !this.drowningMonitor.IsCellSafe(cell))
			{
				return -1;
			}
			return this.navigator.GetNavigationCost(cell);
		}

		// Token: 0x06009056 RID: 36950 RVA: 0x0036E384 File Offset: 0x0036C584
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = data as KPrefabID;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			if (component == null)
			{
				return;
			}
			Diet.Info dietInfo = this.diet.GetDietInfo(kprefabID.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(base.smi.gameObject);
			string properName = kprefabID.GetProperName();
			PopFXManager.Instance.SpawnFX(global::Def.GetUISprite(kprefabID.gameObject, "ui", false).first, PopFXManager.Instance.sprite_Negative, properName, kprefabID.transform, Vector3.zero, 1.5f, true, false, false);
			float num = amountInstance.GetMax() - amountInstance.value;
			float num2 = dietInfo.ConvertCaloriesToConsumptionMass(num);
			IPlantConsumptionInstructions plantConsumptionInstructions = null;
			foreach (IPlantConsumptionInstructions plantConsumptionInstructions3 in GameUtil.GetPlantConsumptionInstructions(kprefabID.gameObject))
			{
				if (dietInfo.foodType == plantConsumptionInstructions3.GetDietFoodType())
				{
					plantConsumptionInstructions = plantConsumptionInstructions3;
				}
			}
			float calories;
			if (plantConsumptionInstructions != null)
			{
				num2 = plantConsumptionInstructions.ConsumePlant(num2);
				calories = dietInfo.ConvertConsumptionMassToCalories(num2);
			}
			else if (dietInfo.foodType == Diet.Info.FoodType.EatPrey || dietInfo.foodType == Diet.Info.FoodType.EatButcheredPrey)
			{
				float num3 = this.diet.AvailableCaloriesInPrey(kprefabID.PrefabTag);
				float num4 = Mathf.Clamp(1f - num / num3, 0f, 1f);
				if (num4 > 0f)
				{
					Butcherable component2 = kprefabID.GetComponent<Butcherable>();
					if (component2 != null)
					{
						component2.CreateDrops(num4);
					}
				}
				component.Mass = 0f;
				calories = Mathf.Min(num, num3);
			}
			else
			{
				num2 = Mathf.Min(num2, component.Mass);
				component.Mass -= num2;
				Pickupable component3 = component.GetComponent<Pickupable>();
				if (component3.storage != null)
				{
					component3.storage.Trigger(-1452790913, base.gameObject);
					component3.storage.Trigger(-1697596308, base.gameObject);
				}
				calories = dietInfo.ConvertConsumptionMassToCalories(num2);
			}
			Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent> boxed = Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>.Get(new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = kprefabID.PrefabTag,
				calories = calories
			});
			base.Trigger(-2038961714, boxed);
			boxed.Release();
			this.targetEdible = null;
		}

		// Token: 0x06009057 RID: 36951 RVA: 0x0036E5D9 File Offset: 0x0036C7D9
		public string[] GetTargetEdibleEatAnims()
		{
			return this.diet.GetDietInfo(this.targetEdible.PrefabID()).eatAnims;
		}

		// Token: 0x06009058 RID: 36952 RVA: 0x0036E5F8 File Offset: 0x0036C7F8
		public Vector3 GetBestEdibleOffset(GameObject edible)
		{
			int num = int.MaxValue;
			Vector3 result = Vector3.zero;
			foreach (Vector3 vector in base.def.possibleEatPositionOffsets)
			{
				Vector3 vector2 = edible.transform.position + vector;
				if (vector.x > 0f)
				{
					vector2 += new Vector3(base.def.navigatorSize.x / 2f, 0f, 0f);
				}
				else if (vector.x < 0f)
				{
					vector2 -= new Vector3(base.def.navigatorSize.x / 2f, 0f, 0f);
				}
				if (vector.y > 0f)
				{
					vector2 += new Vector3(0f, base.def.navigatorSize.y / 2f, 0f);
				}
				else if (vector.y < 0f)
				{
					vector2 -= new Vector3(0f, base.def.navigatorSize.y / 2f, 0f);
				}
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(vector2));
				if (navigationCost != -1 && navigationCost < num)
				{
					num = navigationCost;
					result = vector;
				}
			}
			return result;
		}

		// Token: 0x04006EFF RID: 28415
		private const int RECALC_THRESHOLD = 4;

		// Token: 0x04006F00 RID: 28416
		public GameObject targetEdible;

		// Token: 0x04006F01 RID: 28417
		public Vector3 targetEdibleOffset;

		// Token: 0x04006F02 RID: 28418
		private int targetEdibleCost;

		// Token: 0x04006F03 RID: 28419
		[MyCmpGet]
		private Navigator navigator;

		// Token: 0x04006F04 RID: 28420
		[MyCmpGet]
		private DrowningMonitor drowningMonitor;

		// Token: 0x04006F05 RID: 28421
		public Diet diet;
	}
}
