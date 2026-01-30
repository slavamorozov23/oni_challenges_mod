using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B9B RID: 2971
public class ResourceHarvestModule : GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>
{
	// Token: 0x060058C7 RID: 22727 RVA: 0x00203460 File Offset: 0x00201660
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanDrill();
		});
		this.grounded.TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false).Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.UpdateMeter(null);
		});
		this.not_grounded.DefaultState(this.not_grounded.not_drilling).EventHandler(GameHashes.ClusterLocationChanged, (ResourceHarvestModule.StatesInstance smi) => Game.Instance, delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanDrill();
		}).EventHandler(GameHashes.OnStorageChange, delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanDrill();
		}).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.not_drilling.PlayAnim("loaded").ParamTransition<bool>(this.canHarvest, this.not_grounded.drilling, GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.IsTrue).Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			ResourceHarvestModule.StatesInstance.RemoveHarvestStatusItems(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.gameObject);
		}).Update(delegate(ResourceHarvestModule.StatesInstance smi, float dt)
		{
			smi.CheckIfCanDrill();
		}, UpdateRate.SIM_4000ms, false);
		this.not_grounded.drilling.PlayAnim("deploying").Exit(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().Trigger(939543986, null);
			ResourceHarvestModule.StatesInstance.RemoveHarvestStatusItems(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.gameObject);
		}).Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			Clustercraft component = smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			component.AddTag(GameTags.RocketDrilling);
			component.Trigger(-1762453998, null);
			ResourceHarvestModule.StatesInstance.AddHarvestStatusItems(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.gameObject, smi);
		}).Exit(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().RemoveTag(GameTags.RocketDrilling);
		}).Update(delegate(ResourceHarvestModule.StatesInstance smi, float dt)
		{
			smi.HarvestFromPOI(dt);
			this.lastHarvestTime.Set(Time.time, smi, false);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<bool>(this.canHarvest, this.not_grounded.not_drilling, GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.IsFalse);
	}

	// Token: 0x04003B8C RID: 15244
	public StateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.BoolParameter canHarvest;

	// Token: 0x04003B8D RID: 15245
	public StateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.FloatParameter lastHarvestTime;

	// Token: 0x04003B8E RID: 15246
	public GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State grounded;

	// Token: 0x04003B8F RID: 15247
	public ResourceHarvestModule.NotGroundedStates not_grounded;

	// Token: 0x02001D27 RID: 7463
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008A76 RID: 35446
		public float harvestSpeed;
	}

	// Token: 0x02001D28 RID: 7464
	public class NotGroundedStates : GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State
	{
		// Token: 0x04008A77 RID: 35447
		public GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State not_drilling;

		// Token: 0x04008A78 RID: 35448
		public GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State drilling;
	}

	// Token: 0x02001D29 RID: 7465
	public class StatesInstance : GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.GameInstance
	{
		// Token: 0x0600B04A RID: 45130 RVA: 0x003DA2A0 File Offset: 0x003D84A0
		public StatesInstance(IStateMachineTarget master, ResourceHarvestModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasResource(this.storage, SimHashes.Diamond, 1000f));
			this.onStorageChangeHandle = base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_fill",
				"meter_frame",
				"meter_OL"
			});
			KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
			component.matchParentOffset = true;
			component.forceAlwaysAlive = true;
			this.UpdateMeter(null);
		}

		// Token: 0x0600B04B RID: 45131 RVA: 0x003DA36F File Offset: 0x003D856F
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.Unsubscribe(ref this.onStorageChangeHandle);
		}

		// Token: 0x0600B04C RID: 45132 RVA: 0x003DA383 File Offset: 0x003D8583
		public void UpdateMeter(object data = null)
		{
			this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		}

		// Token: 0x0600B04D RID: 45133 RVA: 0x003DA3A8 File Offset: 0x003D85A8
		public void HarvestFromPOI(float dt)
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (!this.CheckIfCanDrill())
			{
				return;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			if (poiatCurrentLocation == null || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>() == null)
			{
				return;
			}
			StarmapHexCellInventory starmapHexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
			HarvestablePOIStates.Instance smi = poiatCurrentLocation.GetSMI<HarvestablePOIStates.Instance>();
			Dictionary<SimHashes, float> elementsWithWeights = smi.configuration.GetElementsWithWeights();
			float num = 0f;
			foreach (KeyValuePair<SimHashes, float> keyValuePair in elementsWithWeights)
			{
				num += keyValuePair.Value;
			}
			foreach (KeyValuePair<SimHashes, float> keyValuePair2 in elementsWithWeights)
			{
				Element element = ElementLoader.FindElementByHash(keyValuePair2.Key);
				if (!DiscoveredResources.Instance.IsDiscovered(element.tag))
				{
					DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
				}
			}
			float num2 = Mathf.Min(this.GetMaxExtractKGFromDiamondAvailable(), base.def.harvestSpeed * dt);
			float num3 = 0f;
			foreach (KeyValuePair<SimHashes, float> keyValuePair3 in elementsWithWeights)
			{
				if (num3 >= num2)
				{
					break;
				}
				SimHashes key = keyValuePair3.Key;
				float num4 = keyValuePair3.Value / num;
				float num5 = base.def.harvestSpeed * dt * num4;
				Element element2 = ElementLoader.FindElementByHash(key);
				starmapHexCellInventory.AddItem(element2, num5);
				num3 += num5;
			}
			smi.DeltaPOICapacity(-num3);
			this.ConsumeDiamond(num3 * 0.05f);
			SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI += num3;
		}

		// Token: 0x0600B04E RID: 45134 RVA: 0x003DA5A4 File Offset: 0x003D87A4
		public void ConsumeDiamond(float amount)
		{
			base.GetComponent<Storage>().ConsumeIgnoringDisease(SimHashes.Diamond.CreateTag(), amount);
		}

		// Token: 0x0600B04F RID: 45135 RVA: 0x003DA5BC File Offset: 0x003D87BC
		public bool HasAnyAmountOfDiamond()
		{
			return base.GetComponent<Storage>().GetAmountAvailable(SimHashes.Diamond.CreateTag()) > 0f;
		}

		// Token: 0x0600B050 RID: 45136 RVA: 0x003DA5DA File Offset: 0x003D87DA
		public float GetMaxExtractKGFromDiamondAvailable()
		{
			return base.GetComponent<Storage>().GetAmountAvailable(SimHashes.Diamond.CreateTag()) / 0.05f;
		}

		// Token: 0x0600B051 RID: 45137 RVA: 0x003DA5F8 File Offset: 0x003D87F8
		public bool CheckIfCanDrill()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component == null)
			{
				base.sm.canHarvest.Set(false, this, false);
				return false;
			}
			if (!this.HasAnyAmountOfDiamond())
			{
				base.sm.canHarvest.Set(false, this, false);
				return false;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			bool flag = false;
			if (poiatCurrentLocation != null && poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>())
			{
				flag = poiatCurrentLocation.GetSMI<HarvestablePOIStates.Instance>().POICanBeHarvested();
			}
			base.sm.canHarvest.Set(flag, this, false);
			return flag;
		}

		// Token: 0x0600B052 RID: 45138 RVA: 0x003DA693 File Offset: 0x003D8893
		public static void AddHarvestStatusItems(GameObject statusTarget, ResourceHarvestModule.StatesInstance smi)
		{
			statusTarget.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SpacePOIHarvesting, smi);
		}

		// Token: 0x0600B053 RID: 45139 RVA: 0x003DA6B1 File Offset: 0x003D88B1
		public static void RemoveHarvestStatusItems(GameObject statusTarget)
		{
			statusTarget.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SpacePOIHarvesting, false);
		}

		// Token: 0x04008A79 RID: 35449
		private MeterController meter;

		// Token: 0x04008A7A RID: 35450
		private Storage storage;

		// Token: 0x04008A7B RID: 35451
		private int onStorageChangeHandle = -1;
	}
}
