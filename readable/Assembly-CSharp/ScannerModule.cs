using System;

// Token: 0x020007F6 RID: 2038
public class ScannerModule : GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>
{
	// Token: 0x0600368E RID: 13966 RVA: 0x00133BA4 File Offset: 0x00131DA4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		}).EventHandler(GameHashes.RocketLaunched, delegate(ScannerModule.Instance smi)
		{
			smi.Scan();
		}).EventHandler(GameHashes.ClusterLocationChanged, (ScannerModule.Instance smi) => smi.GetComponent<RocketModuleCluster>().CraftInterface, delegate(ScannerModule.Instance smi)
		{
			smi.Scan();
		}).EventHandler(GameHashes.RocketModuleChanged, (ScannerModule.Instance smi) => smi.GetComponent<RocketModuleCluster>().CraftInterface, delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		}).Exit(delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		});
	}

	// Token: 0x0200176B RID: 5995
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040077B3 RID: 30643
		public int scanRadius = 1;
	}

	// Token: 0x0200176C RID: 5996
	public new class Instance : GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.GameInstance
	{
		// Token: 0x06009B29 RID: 39721 RVA: 0x00393ED9 File Offset: 0x003920D9
		public Instance(IStateMachineTarget master, ScannerModule.Def def) : base(master, def)
		{
		}

		// Token: 0x06009B2A RID: 39722 RVA: 0x00393EE4 File Offset: 0x003920E4
		public void Scan()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component.Status == Clustercraft.CraftStatus.InFlight)
			{
				ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
				AxialI location = component.Location;
				smi.RevealLocation(location, base.def.scanRadius, 2);
				foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetNotVisibleEntitiesAtAdjacentCell(location))
				{
					smi.RevealLocation(clusterGridEntity.Location, 0, 2);
				}
			}
		}

		// Token: 0x06009B2B RID: 39723 RVA: 0x00393F84 File Offset: 0x00392184
		public void SetFogOfWarAllowed()
		{
			CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
			if (craftInterface.HasClusterDestinationSelector())
			{
				bool flag = false;
				ClusterDestinationSelector clusterDestinationSelector = craftInterface.GetClusterDestinationSelector();
				bool canNavigateFogOfWar = clusterDestinationSelector.canNavigateFogOfWar;
				foreach (Ref<RocketModuleCluster> @ref in craftInterface.ClusterModules)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					if (((rocketModuleCluster != null) ? rocketModuleCluster.GetSMI<ScannerModule.Instance>() : null) != null)
					{
						flag = true;
						break;
					}
				}
				clusterDestinationSelector.canNavigateFogOfWar = flag;
				if (canNavigateFogOfWar && !flag)
				{
					ClusterTraveler component = craftInterface.GetComponent<ClusterTraveler>();
					if (component != null)
					{
						component.RevalidatePath(true);
					}
				}
				craftInterface.GetComponent<Clustercraft>().Trigger(-688990705, null);
			}
		}
	}
}
