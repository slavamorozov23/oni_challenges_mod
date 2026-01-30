using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000B9E RID: 2974
public class RocketClusterDestinationSelector : ClusterDestinationSelector
{
	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x060058DD RID: 22749 RVA: 0x00203E79 File Offset: 0x00202079
	// (set) Token: 0x060058DE RID: 22750 RVA: 0x00203E81 File Offset: 0x00202081
	public bool Repeat
	{
		get
		{
			return this.m_repeat;
		}
		set
		{
			this.m_repeat = value;
		}
	}

	// Token: 0x060058DF RID: 22751 RVA: 0x00203E8A File Offset: 0x0020208A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketClusterDestinationSelector>(-1277991738, this.OnLaunchDelegate);
	}

	// Token: 0x060058E0 RID: 22752 RVA: 0x00203EA4 File Offset: 0x002020A4
	protected override void OnSpawn()
	{
		if (this.isHarvesting)
		{
			this.WaitForPOIHarvest();
		}
	}

	// Token: 0x060058E1 RID: 22753 RVA: 0x00203EB4 File Offset: 0x002020B4
	public LaunchPad GetDestinationPad(AxialI destination)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
		if (this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			return this.m_launchPad[asteroidWorldIdAtLocation].Get();
		}
		return null;
	}

	// Token: 0x060058E2 RID: 22754 RVA: 0x00203EE9 File Offset: 0x002020E9
	public LaunchPad GetDestinationPad()
	{
		return this.GetDestinationPad(this.m_destination);
	}

	// Token: 0x060058E3 RID: 22755 RVA: 0x00203EF7 File Offset: 0x002020F7
	public override void SetDestination(AxialI location)
	{
		base.SetDestination(location);
	}

	// Token: 0x060058E4 RID: 22756 RVA: 0x00203F00 File Offset: 0x00202100
	public void SetDestinationPad(LaunchPad pad)
	{
		Debug.Assert(pad == null || ClusterGrid.Instance.IsInRange(pad.GetMyWorldLocation(), this.m_destination, 1), "Tried sending a rocket to a launchpad that wasn't its destination world.");
		if (pad != null)
		{
			this.AddDestinationPad(pad.GetMyWorldLocation(), pad);
			base.SetDestination(pad.GetMyWorldLocation());
		}
		base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationChanged, null);
	}

	// Token: 0x060058E5 RID: 22757 RVA: 0x00203F70 File Offset: 0x00202170
	private void AddDestinationPad(AxialI location, LaunchPad pad)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		if (asteroidWorldIdAtLocation < 0)
		{
			return;
		}
		if (!this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			this.m_launchPad.Add(asteroidWorldIdAtLocation, new Ref<LaunchPad>());
		}
		this.m_launchPad[asteroidWorldIdAtLocation].Set(pad);
	}

	// Token: 0x060058E6 RID: 22758 RVA: 0x00203FBC File Offset: 0x002021BC
	protected override void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		if (clusterLocationChangedEvent.newLocation == this.m_destination)
		{
			base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationReached, null);
			if (this.m_repeat)
			{
				if ((ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(clusterLocationChangedEvent.newLocation, EntityLayer.POI) != null && this.CanRocketDrill()) || this.CanCollectFromHexCellInventory())
				{
					this.WaitForPOIHarvest();
					return;
				}
				this.SetUpReturnTrip();
			}
		}
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x00204038 File Offset: 0x00202238
	private void SetUpReturnTrip()
	{
		this.AddDestinationPad(this.m_prevDestination, this.m_prevLaunchPad.Get());
		this.m_destination = this.m_prevDestination;
		this.m_prevDestination = base.GetComponent<Clustercraft>().Location;
		this.m_prevLaunchPad.Set(base.GetComponent<CraftModuleInterface>().CurrentPad);
	}

	// Token: 0x060058E8 RID: 22760 RVA: 0x00204090 File Offset: 0x00202290
	private bool CanCollectFromHexCellInventory()
	{
		bool flag = false;
		foreach (RocketModuleHexCellCollector.Instance instance in base.GetComponent<Clustercraft>().GetAllHexCellCollectorModules())
		{
			flag = (flag || (instance != null && RocketModuleHexCellCollector.CanCollect(instance)));
		}
		return flag;
	}

	// Token: 0x060058E9 RID: 22761 RVA: 0x002040F8 File Offset: 0x002022F8
	private bool CanRocketDrill()
	{
		bool flag = false;
		List<ResourceHarvestModule.StatesInstance> allResourceHarvestModules = base.GetComponent<Clustercraft>().GetAllResourceHarvestModules();
		if (allResourceHarvestModules.Count > 0)
		{
			using (List<ResourceHarvestModule.StatesInstance>.Enumerator enumerator = allResourceHarvestModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckIfCanDrill())
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			List<ArtifactHarvestModule.StatesInstance> allArtifactHarvestModules = base.GetComponent<Clustercraft>().GetAllArtifactHarvestModules();
			if (allArtifactHarvestModules.Count > 0)
			{
				using (List<ArtifactHarvestModule.StatesInstance>.Enumerator enumerator2 = allArtifactHarvestModules.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.CheckIfCanHarvest())
						{
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060058EA RID: 22762 RVA: 0x002041B8 File Offset: 0x002023B8
	private void OnTagsChanged(object data)
	{
		if (((Boxed<TagChangedEventData>)data).value.tag == GameTags.RocketDrilling)
		{
			this.CheckAndReturnRocketIfHarvestingEnded();
		}
	}

	// Token: 0x060058EB RID: 22763 RVA: 0x002041DC File Offset: 0x002023DC
	private void OnStorageChange(object data)
	{
		this.CheckAndReturnRocketIfHarvestingEnded();
	}

	// Token: 0x060058EC RID: 22764 RVA: 0x002041E4 File Offset: 0x002023E4
	private void CheckAndReturnRocketIfHarvestingEnded()
	{
		if (!this.CanRocketDrill() && !this.CanCollectFromHexCellInventory())
		{
			if (!this.isHarvesting)
			{
				return;
			}
			this.isHarvesting = false;
			Clustercraft component = base.GetComponent<Clustercraft>();
			foreach (Ref<RocketModuleCluster> @ref in component.ModuleInterface.ClusterModules)
			{
				if (@ref.Get().GetComponent<Storage>())
				{
					base.Unsubscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
				}
			}
			component.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
			this.SetUpReturnTrip();
		}
	}

	// Token: 0x060058ED RID: 22765 RVA: 0x002042B0 File Offset: 0x002024B0
	private void WaitForPOIHarvest()
	{
		this.isHarvesting = true;
		Clustercraft component = base.GetComponent<Clustercraft>();
		foreach (Ref<RocketModuleCluster> @ref in component.ModuleInterface.ClusterModules)
		{
			if (@ref.Get().GetComponent<Storage>())
			{
				base.Subscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
			}
		}
		component.gameObject.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x060058EE RID: 22766 RVA: 0x0020435C File Offset: 0x0020255C
	private void OnLaunch(object data)
	{
		CraftModuleInterface component = base.GetComponent<CraftModuleInterface>();
		this.m_prevLaunchPad.Set(component.CurrentPad);
		Clustercraft component2 = base.GetComponent<Clustercraft>();
		this.m_prevDestination = component2.Location;
	}

	// Token: 0x04003B97 RID: 15255
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> m_launchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04003B98 RID: 15256
	[Serialize]
	private bool m_repeat;

	// Token: 0x04003B99 RID: 15257
	[Serialize]
	private AxialI m_prevDestination;

	// Token: 0x04003B9A RID: 15258
	[Serialize]
	private Ref<LaunchPad> m_prevLaunchPad = new Ref<LaunchPad>();

	// Token: 0x04003B9B RID: 15259
	[Serialize]
	private bool isHarvesting;

	// Token: 0x04003B9C RID: 15260
	private EventSystem.IntraObjectHandler<RocketClusterDestinationSelector> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketClusterDestinationSelector>(delegate(RocketClusterDestinationSelector cmp, object data)
	{
		cmp.OnLaunch(data);
	});
}
