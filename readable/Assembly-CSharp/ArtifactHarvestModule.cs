using System;

// Token: 0x02000B61 RID: 2913
public class ArtifactHarvestModule : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>
{
	// Token: 0x06005630 RID: 22064 RVA: 0x001F6AA4 File Offset: 0x001F4CA4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		});
		this.grounded.TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.not_grounded.DefaultState(this.not_grounded.not_harvesting).EventHandler(GameHashes.ClusterLocationChanged, (ArtifactHarvestModule.StatesInstance smi) => Game.Instance, new GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.GameEvent.Callback(ArtifactHarvestModule.OnAnythingChangingLocationsInSpace)).EventHandler(GameHashes.OnStorageChange, delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.not_harvesting.PlayAnim("loaded").ParamTransition<bool>(this.canHarvest, this.not_grounded.harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsTrue);
		this.not_grounded.harvesting.PlayAnim("deploying").Update(delegate(ArtifactHarvestModule.StatesInstance smi, float dt)
		{
			smi.HarvestFromHexCell(dt);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<bool>(this.canHarvest, this.not_grounded.not_harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsFalse);
	}

	// Token: 0x06005631 RID: 22065 RVA: 0x001F6C0C File Offset: 0x001F4E0C
	private static void OnAnythingChangingLocationsInSpace(ArtifactHarvestModule.StatesInstance smi, object obj)
	{
		if (obj == null)
		{
			return;
		}
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)obj;
		Clustercraft component = smi.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
		if (clusterLocationChangedEvent.entity == component)
		{
			smi.CheckIfCanHarvest();
		}
	}

	// Token: 0x04003A37 RID: 14903
	public StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.BoolParameter canHarvest;

	// Token: 0x04003A38 RID: 14904
	public StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.TargetParameter entityTarget;

	// Token: 0x04003A39 RID: 14905
	public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State grounded;

	// Token: 0x04003A3A RID: 14906
	public ArtifactHarvestModule.NotGroundedStates not_grounded;

	// Token: 0x02001CCE RID: 7374
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001CCF RID: 7375
	public class NotGroundedStates : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State
	{
		// Token: 0x04008940 RID: 35136
		public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State not_harvesting;

		// Token: 0x04008941 RID: 35137
		public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State harvesting;
	}

	// Token: 0x02001CD0 RID: 7376
	public class StatesInstance : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.GameInstance
	{
		// Token: 0x0600AEB8 RID: 44728 RVA: 0x003D4674 File Offset: 0x003D2874
		public StatesInstance(IStateMachineTarget master, ArtifactHarvestModule.Def def) : base(master, def)
		{
		}

		// Token: 0x0600AEB9 RID: 44729 RVA: 0x003D4680 File Offset: 0x003D2880
		public void HarvestFromHexCell(float dt)
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			StarmapHexCellInventory starmapHexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
			StarmapHexCellInventory.SerializedItem serializedItem = starmapHexCellInventory.Items.Find((StarmapHexCellInventory.SerializedItem item) => item.IsEntity && Assets.GetPrefab(item.ID).HasTag(GameTags.Artifact));
			if (serializedItem != null)
			{
				PrimaryElement primaryElement = starmapHexCellInventory.ExtractAndSpawnItem(serializedItem.ID);
				this.receptacle.ForceDeposit(primaryElement.gameObject);
				this.storage.Store(primaryElement.gameObject, false, false, true, false);
				return;
			}
		}

		// Token: 0x0600AEBA RID: 44730 RVA: 0x003D4714 File Offset: 0x003D2914
		public bool CheckIfCanHarvest()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component == null)
			{
				return false;
			}
			if (this.receptacle.Occupant != null)
			{
				base.sm.canHarvest.Set(false, this, false);
				return false;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			if (ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location).Items.Find((StarmapHexCellInventory.SerializedItem item) => item.IsEntity && Assets.GetPrefab(item.ID).HasTag(GameTags.Artifact)) != null)
			{
				base.sm.canHarvest.Set(true, this, false);
				return true;
			}
			if (poiatCurrentLocation != null && (poiatCurrentLocation.GetComponent<ArtifactPOIClusterGridEntity>() || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>()))
			{
				ArtifactPOIStates.Instance smi = poiatCurrentLocation.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.HasArtifactAvailableInHexCell())
				{
					base.sm.canHarvest.Set(true, this, false);
					return true;
				}
			}
			base.sm.canHarvest.Set(false, this, false);
			return false;
		}

		// Token: 0x04008942 RID: 35138
		[MyCmpReq]
		private Storage storage;

		// Token: 0x04008943 RID: 35139
		[MyCmpReq]
		private SingleEntityReceptacle receptacle;
	}
}
