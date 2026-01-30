using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B74 RID: 2932
public class Clustercraft : ClusterGridEntity, IClusterRange, ISim4000ms, ISim1000ms
{
	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x060056E0 RID: 22240 RVA: 0x001F9BC9 File Offset: 0x001F7DC9
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x060056E1 RID: 22241 RVA: 0x001F9BD1 File Offset: 0x001F7DD1
	// (set) Token: 0x060056E2 RID: 22242 RVA: 0x001F9BD9 File Offset: 0x001F7DD9
	public bool Exploding { get; protected set; }

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x060056E3 RID: 22243 RVA: 0x001F9BE2 File Offset: 0x001F7DE2
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x060056E4 RID: 22244 RVA: 0x001F9BE8 File Offset: 0x001F7DE8
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("rocket01_kanim"),
					initialAnim = "idle_loop"
				}
			};
		}
	}

	// Token: 0x060056E5 RID: 22245 RVA: 0x001F9C2C File Offset: 0x001F7E2C
	public override Sprite GetUISprite()
	{
		PassengerRocketModule passengerModule = this.m_moduleInterface.GetPassengerModule();
		if (passengerModule != null)
		{
			return Def.GetUISprite(passengerModule.gameObject, "ui", false).first;
		}
		return Assets.GetSprite("ic_rocket");
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x060056E6 RID: 22246 RVA: 0x001F9C74 File Offset: 0x001F7E74
	public override bool IsVisible
	{
		get
		{
			return !this.Exploding;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x060056E7 RID: 22247 RVA: 0x001F9C7F File Offset: 0x001F7E7F
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x001F9C82 File Offset: 0x001F7E82
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x060056E9 RID: 22249 RVA: 0x001F9C85 File Offset: 0x001F7E85
	public CraftModuleInterface ModuleInterface
	{
		get
		{
			return this.m_moduleInterface;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x060056EA RID: 22250 RVA: 0x001F9C8D File Offset: 0x001F7E8D
	public AxialI Destination
	{
		get
		{
			return this.m_moduleInterface.GetClusterDestinationSelector().GetDestination();
		}
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x060056EB RID: 22251 RVA: 0x001F9CA0 File Offset: 0x001F7EA0
	public float Speed
	{
		get
		{
			float num = this.EnginePower / this.TotalBurden;
			float num2 = num * this.PilotSkillMultiplier;
			bool flag = this.AutoPilotMultiplier > 0.5f;
			bool flag2 = this.ModuleInterface.GetPassengerModule() != null;
			RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
			bool flag3 = robotPilotModule != null && robotPilotModule.GetDataBanksStored() > 1f;
			if (flag3 && flag)
			{
				num2 *= 1.5f;
			}
			else if (!flag && flag2)
			{
				num2 *= 0.5f;
			}
			else if (!flag3 && !flag2)
			{
				num2 = 0f;
			}
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				num2 += num * 0.20000005f;
			}
			return num2;
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x060056EC RID: 22252 RVA: 0x001F9D58 File Offset: 0x001F7F58
	public float EnginePower
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.EnginePower;
			}
			return num;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x060056ED RID: 22253 RVA: 0x001F9DC0 File Offset: 0x001F7FC0
	public float FuelPerDistance
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.FuelKilogramPerDistance;
			}
			return num;
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x060056EE RID: 22254 RVA: 0x001F9E28 File Offset: 0x001F8028
	public float TotalBurden
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.Burden;
			}
			global::Debug.Assert(num > 0f);
			return num;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x060056EF RID: 22255 RVA: 0x001F9E9C File Offset: 0x001F809C
	// (set) Token: 0x060056F0 RID: 22256 RVA: 0x001F9EA4 File Offset: 0x001F80A4
	public bool LaunchRequested
	{
		get
		{
			return this.m_launchRequested;
		}
		private set
		{
			this.m_launchRequested = value;
			this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.RocketRequestLaunch, this);
		}
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x060056F1 RID: 22257 RVA: 0x001F9EBE File Offset: 0x001F80BE
	public Clustercraft.CraftStatus Status
	{
		get
		{
			return this.status;
		}
	}

	// Token: 0x060056F2 RID: 22258 RVA: 0x001F9EC6 File Offset: 0x001F80C6
	public void SetCraftStatus(Clustercraft.CraftStatus craft_status)
	{
		this.status = craft_status;
		this.UpdateGroundTags();
		this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.ClustercraftStateChanged, craft_status);
	}

	// Token: 0x060056F3 RID: 22259 RVA: 0x001F9EEB File Offset: 0x001F80EB
	public void SetExploding()
	{
		this.Exploding = true;
	}

	// Token: 0x060056F4 RID: 22260 RVA: 0x001F9EF4 File Offset: 0x001F80F4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Clustercrafts.Add(this);
	}

	// Token: 0x060056F5 RID: 22261 RVA: 0x001F9F08 File Offset: 0x001F8108
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = new System.Action(this.BurnFuelForTravel);
		this.m_clusterTraveler.validateTravelCB = new Func<AxialI, bool>(this.CanTravelToCell);
		this.UpdateGroundTags();
		base.Subscribe<Clustercraft>(1512695988, Clustercraft.RocketModuleChangedHandler);
		base.Subscribe<Clustercraft>(543433792, Clustercraft.ClusterDestinationChangedHandler);
		base.Subscribe<Clustercraft>(1796608350, Clustercraft.ClusterDestinationReachedHandler);
		base.Subscribe(-688990705, delegate(object o)
		{
			this.UpdateStatusItem();
		});
		base.Subscribe<Clustercraft>(1102426921, Clustercraft.NameChangedHandler);
		this.SetRocketName(this.m_name);
		this.UpdateStatusItem();
		this.RefreshStarBackgroundVariables();
	}

	// Token: 0x060056F6 RID: 22262 RVA: 0x001F9FF4 File Offset: 0x001F81F4
	public void Sim1000ms(float dt)
	{
		this.controlStationBuffTimeRemaining = Mathf.Max(this.controlStationBuffTimeRemaining - dt, 0f);
		if (this.controlStationBuffTimeRemaining > 0f)
		{
			this.missionControlStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, this);
			return;
		}
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, false);
		this.missionControlStatusHandle = Guid.Empty;
	}

	// Token: 0x060056F7 RID: 22263 RVA: 0x001FA070 File Offset: 0x001F8270
	public void Sim4000ms(float dt)
	{
		RocketClusterDestinationSelector clusterDestinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
		if (this.Status == Clustercraft.CraftStatus.InFlight && this.m_location == clusterDestinationSelector.GetDestination())
		{
			this.OnClusterDestinationReached(null);
		}
	}

	// Token: 0x060056F8 RID: 22264 RVA: 0x001FA0AC File Offset: 0x001F82AC
	public void Init(AxialI location, LaunchPad pad)
	{
		this.m_location = location;
		base.GetComponent<RocketClusterDestinationSelector>().SetDestination(this.m_location);
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
		if (pad != null)
		{
			this.Land(pad, true);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x060056F9 RID: 22265 RVA: 0x001FA0E8 File Offset: 0x001F82E8
	protected override void OnCleanUp()
	{
		Components.Clustercrafts.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060056FA RID: 22266 RVA: 0x001FA0FB File Offset: 0x001F82FB
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.RocketInSpace) && (tryingToLand || this.HasResourcesToMove(1, Clustercraft.CombustionResource.All));
	}

	// Token: 0x060056FB RID: 22267 RVA: 0x001FA119 File Offset: 0x001F8319
	private bool CanTravelToCell(AxialI location)
	{
		return !(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null) || this.CanLandAtAsteroid(location, true);
	}

	// Token: 0x060056FC RID: 22268 RVA: 0x001FA139 File Offset: 0x001F8339
	private float GetSpeed()
	{
		return this.Speed;
	}

	// Token: 0x060056FD RID: 22269 RVA: 0x001FA144 File Offset: 0x001F8344
	private void RocketModuleChanged(object data)
	{
		RocketModuleCluster rocketModuleCluster = (RocketModuleCluster)data;
		if (rocketModuleCluster != null)
		{
			this.UpdateGroundTags(rocketModuleCluster.gameObject);
		}
	}

	// Token: 0x060056FE RID: 22270 RVA: 0x001FA16D File Offset: 0x001F836D
	private void OnClusterDestinationChanged(object _)
	{
		this.UpdateStatusItem();
		this.RefreshStarBackgroundVariables();
	}

	// Token: 0x060056FF RID: 22271 RVA: 0x001FA17C File Offset: 0x001F837C
	private void OnClusterDestinationReached(object _)
	{
		RocketClusterDestinationSelector clusterDestinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
		global::Debug.Assert(base.Location == clusterDestinationSelector.GetDestination());
		if (clusterDestinationSelector.HasAsteroidDestination())
		{
			LaunchPad destinationPad = clusterDestinationSelector.GetDestinationPad();
			this.Land(base.Location, destinationPad);
		}
		this.UpdateStatusItem();
		this.RefreshStarBackgroundVariables();
	}

	// Token: 0x06005700 RID: 22272 RVA: 0x001FA1D3 File Offset: 0x001F83D3
	public void SetRocketName(object newName)
	{
		this.SetRocketName((string)newName);
	}

	// Token: 0x06005701 RID: 22273 RVA: 0x001FA1E4 File Offset: 0x001F83E4
	public void RefreshStarBackgroundVariables()
	{
		bool flag = this.IsFlightInProgress() && this.HasResourcesToMove(1, Clustercraft.CombustionResource.All);
		if (this.wasFlying != flag)
		{
			if (flag)
			{
				this.LastTimeFlightBegan = Time.timeSinceLevelLoad;
			}
			else
			{
				this.LastTimeFlightStopped = Time.timeSinceLevelLoad;
			}
			this.wasFlying = flag;
		}
	}

	// Token: 0x06005702 RID: 22274 RVA: 0x001FA230 File Offset: 0x001F8430
	public void SetRocketName(string newName)
	{
		this.m_name = newName;
		base.name = "Clustercraft: " + newName;
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CharacterOverlay component = @ref.Get().GetComponent<CharacterOverlay>();
			if (component != null)
			{
				NameDisplayScreen.Instance.UpdateName(component.gameObject);
				break;
			}
		}
		ClusterManager.Instance.Trigger(1943181844, newName);
	}

	// Token: 0x06005703 RID: 22275 RVA: 0x001FA2C8 File Offset: 0x001F84C8
	public bool CheckPreppedForLaunch()
	{
		return this.m_moduleInterface.CheckPreppedForLaunch();
	}

	// Token: 0x06005704 RID: 22276 RVA: 0x001FA2D5 File Offset: 0x001F84D5
	public bool CheckReadyToLaunch()
	{
		return this.m_moduleInterface.CheckReadyToLaunch();
	}

	// Token: 0x06005705 RID: 22277 RVA: 0x001FA2E2 File Offset: 0x001F84E2
	public bool IsFlightInProgress()
	{
		return this.Status == Clustercraft.CraftStatus.InFlight && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06005706 RID: 22278 RVA: 0x001FA2FC File Offset: 0x001F84FC
	public ClusterGridEntity GetPOIAtCurrentLocation()
	{
		if ((this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress()) && (this.status != Clustercraft.CraftStatus.Launching || !(this.m_location == this.Destination)))
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.POI);
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x001FA349 File Offset: 0x001F8549
	public ClusterGridEntity GetStableOrbitAsteroid()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress())
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x06005708 RID: 22280 RVA: 0x001FA36F File Offset: 0x001F856F
	public ClusterGridEntity GetOrbitAsteroid()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight)
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x06005709 RID: 22281 RVA: 0x001FA38D File Offset: 0x001F858D
	public ClusterGridEntity GetAdjacentAsteroid()
	{
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x001FA3A0 File Offset: 0x001F85A0
	private bool CheckDesinationInRange()
	{
		return this.m_clusterTraveler.CurrentPath != null && this.Speed * this.m_clusterTraveler.TravelETA() <= this.ModuleInterface.Range;
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x001FA3D4 File Offset: 0x001F85D4
	public bool HasResourcesToMove(int hexes = 1, Clustercraft.CombustionResource combustionResource = Clustercraft.CombustionResource.All)
	{
		switch (combustionResource)
		{
		case Clustercraft.CombustionResource.Fuel:
			return this.m_moduleInterface.FuelRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		case Clustercraft.CombustionResource.Oxidizer:
			return this.m_moduleInterface.OxidizerPowerRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		case Clustercraft.CombustionResource.All:
			return this.m_moduleInterface.BurnableMassRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		default:
		{
			bool flag;
			RocketModuleCluster primaryPilotModule = this.m_moduleInterface.GetPrimaryPilotModule(out flag);
			return flag && primaryPilotModule.GetComponent<RoboPilotModule>().HasResourcesToMove(hexes);
		}
		}
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x001FA488 File Offset: 0x001F8688
	private void BurnFuelForTravel()
	{
		float num = 600f;
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			RocketEngineCluster component = rocketModuleCluster.GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				Tag fuelTag = component.fuelTag;
				float num2 = 0f;
				if (component.requireOxidizer)
				{
					num2 = this.ModuleInterface.OxidizerPowerRemaining;
				}
				if (num > 0f)
				{
					foreach (Ref<RocketModuleCluster> ref2 in this.m_moduleInterface.ClusterModules)
					{
						IFuelTank component2 = ref2.Get().GetComponent<IFuelTank>();
						if (!component2.IsNullOrDestroyed())
						{
							num -= this.BurnFromTank(num, component, fuelTag, component2.Storage, ref num2);
						}
						if (num <= 0f)
						{
							break;
						}
					}
				}
			}
			RoboPilotModule component3 = rocketModuleCluster.GetComponent<RoboPilotModule>();
			if (component3 != null)
			{
				component3.ConsumeDataBanksInFlight();
			}
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x001FA5B8 File Offset: 0x001F87B8
	private float BurnFromTank(float attemptTravelAmount, RocketEngineCluster engine, Tag fuelTag, IStorage storage, ref float totalOxidizerRemaining)
	{
		float num = attemptTravelAmount * engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
		num = Mathf.Min(storage.GetAmountAvailable(fuelTag), num);
		if (engine.requireOxidizer)
		{
			num = Mathf.Min(num, totalOxidizerRemaining);
		}
		storage.ConsumeIgnoringDisease(fuelTag, num);
		if (engine.requireOxidizer)
		{
			this.BurnOxidizer(num);
			totalOxidizerRemaining -= num;
		}
		return num / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
	}

	// Token: 0x0600570E RID: 22286 RVA: 0x001FA62C File Offset: 0x001F882C
	private void BurnOxidizer(float fuelEquivalentKGs)
	{
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
			if (component != null)
			{
				foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
				{
					float num = Clustercraft.dlc1OxidizerEfficiencies[keyValuePair.Key];
					float num2 = Mathf.Min(fuelEquivalentKGs / num, keyValuePair.Value);
					if (num2 > 0f)
					{
						component.storage.ConsumeIgnoringDisease(keyValuePair.Key, num2);
						fuelEquivalentKGs -= num2 * num;
					}
				}
			}
			if (fuelEquivalentKGs <= 0f)
			{
				break;
			}
		}
	}

	// Token: 0x0600570F RID: 22287 RVA: 0x001FA720 File Offset: 0x001F8920
	public List<ResourceHarvestModule.StatesInstance> GetAllResourceHarvestModules()
	{
		List<ResourceHarvestModule.StatesInstance> list = new List<ResourceHarvestModule.StatesInstance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			ResourceHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x06005710 RID: 22288 RVA: 0x001FA788 File Offset: 0x001F8988
	public List<ArtifactHarvestModule.StatesInstance> GetAllArtifactHarvestModules()
	{
		List<ArtifactHarvestModule.StatesInstance> list = new List<ArtifactHarvestModule.StatesInstance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			ArtifactHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ArtifactHarvestModule.StatesInstance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x06005711 RID: 22289 RVA: 0x001FA7F0 File Offset: 0x001F89F0
	public List<RocketModuleHexCellCollector.Instance> GetAllHexCellCollectorModules()
	{
		List<RocketModuleHexCellCollector.Instance> list = new List<RocketModuleHexCellCollector.Instance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			RocketModuleHexCellCollector.Instance smi = @ref.Get().GetSMI<RocketModuleHexCellCollector.Instance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x06005712 RID: 22290 RVA: 0x001FA858 File Offset: 0x001F8A58
	public List<CargoBayCluster> GetAllCargoBays()
	{
		List<CargoBayCluster> list = new List<CargoBayCluster>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06005713 RID: 22291 RVA: 0x001FA8C4 File Offset: 0x001F8AC4
	public List<CargoBayCluster> GetCargoBaysOfType(CargoBay.CargoType cargoType)
	{
		List<CargoBayCluster> list = new List<CargoBayCluster>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
			if (component != null && component.storageType == cargoType)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06005714 RID: 22292 RVA: 0x001FA93C File Offset: 0x001F8B3C
	public void DestroyCraftAndModules()
	{
		WorldContainer interiorWorld = this.m_moduleInterface.GetInteriorWorld();
		if (interiorWorld != null)
		{
			NameDisplayScreen.Instance.RemoveWorldEntries(interiorWorld.id);
		}
		List<RocketModuleCluster> list = (from x in this.m_moduleInterface.ClusterModules
		select x.Get()).ToList<RocketModuleCluster>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			RocketModuleCluster rocketModuleCluster = list[i];
			Storage component = rocketModuleCluster.GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = rocketModuleCluster.GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(rocketModuleCluster.gameObject);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06005715 RID: 22293 RVA: 0x001FAA35 File Offset: 0x001F8C35
	public void CancelLaunch()
	{
		if (this.LaunchRequested)
		{
			global::Debug.Log("Cancelling launch!");
			this.LaunchRequested = false;
		}
	}

	// Token: 0x06005716 RID: 22294 RVA: 0x001FAA50 File Offset: 0x001F8C50
	public void RequestLaunch(bool automated = false)
	{
		if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && !automated)
		{
			this.Launch(false);
		}
		if (this.LaunchRequested)
		{
			return;
		}
		if (!this.CheckPreppedForLaunch())
		{
			return;
		}
		global::Debug.Log("Triggering launch!");
		if (this.m_moduleInterface.GetRobotPilotModule() != null)
		{
			this.Launch(automated);
		}
		this.LaunchRequested = true;
	}

	// Token: 0x06005717 RID: 22295 RVA: 0x001FAACC File Offset: 0x001F8CCC
	public void Launch(bool automated = false)
	{
		if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
		{
			this.LaunchRequested = false;
			return;
		}
		if ((!DebugHandler.InstantBuildMode || automated) && !this.CheckReadyToLaunch())
		{
			return;
		}
		if (automated && !this.m_moduleInterface.CheckReadyForAutomatedLaunchCommand())
		{
			this.LaunchRequested = false;
			return;
		}
		this.LaunchRequested = false;
		this.SetCraftStatus(Clustercraft.CraftStatus.Launching);
		this.m_moduleInterface.DoLaunch();
		this.BurnFuelForTravel();
		this.m_clusterTraveler.AdvancePathOneStep();
		this.UpdateStatusItem();
	}

	// Token: 0x06005718 RID: 22296 RVA: 0x001FAB5C File Offset: 0x001F8D5C
	public void LandAtPad(LaunchPad pad)
	{
		this.m_moduleInterface.GetClusterDestinationSelector().SetDestinationPad(pad);
	}

	// Token: 0x06005719 RID: 22297 RVA: 0x001FAB70 File Offset: 0x001F8D70
	public Clustercraft.PadLandingStatus CanLandAtPad(LaunchPad pad, out string failReason)
	{
		if (pad == null)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.NONEAVAILABLE;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		if (pad.HasRocket() && pad.LandedRocket.CraftInterface != this.m_moduleInterface)
		{
			failReason = "<TEMP>The pad already has a rocket on it!<TEMP>";
			return Clustercraft.PadLandingStatus.CanLandEventually;
		}
		if (ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(pad.gameObject) < this.ModuleInterface.RocketHeight)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_TOO_SHORT;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		int num = -1;
		if (!ConditionFlightPathIsClear.CheckFlightPathClear(this.ModuleInterface, pad.gameObject, out num))
		{
			failReason = string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PATH_OBSTRUCTED, pad.GetProperName());
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		if (!pad.GetComponent<Operational>().IsOperational)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		int rocketBottomPosition = pad.RocketBottomPosition;
		foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			int moduleRelativeVerticalPosition = this.ModuleInterface.GetModuleRelativeVerticalPosition(gameObject);
			Building component = gameObject.GetComponent<Building>();
			BuildingUnderConstruction component2 = gameObject.GetComponent<BuildingUnderConstruction>();
			BuildingDef buildingDef = (component != null) ? component.Def : component2.Def;
			for (int i = 0; i < buildingDef.WidthInCells; i++)
			{
				for (int j = 0; j < buildingDef.HeightInCells; j++)
				{
					int num2 = Grid.OffsetCell(rocketBottomPosition, 0, moduleRelativeVerticalPosition);
					num2 = Grid.OffsetCell(num2, -(buildingDef.WidthInCells / 2) + i, j);
					if (Grid.Solid[num2])
					{
						num = num2;
						failReason = string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_SITE_OBSTRUCTED, pad.GetProperName());
						return Clustercraft.PadLandingStatus.CanNeverLand;
					}
				}
			}
		}
		failReason = null;
		return Clustercraft.PadLandingStatus.CanLandImmediately;
	}

	// Token: 0x0600571A RID: 22298 RVA: 0x001FAD40 File Offset: 0x001F8F40
	private LaunchPad FindValidLandingPad(AxialI location, bool mustLandImmediately)
	{
		LaunchPad result = null;
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		LaunchPad preferredLaunchPadForWorld = this.m_moduleInterface.GetPreferredLaunchPadForWorld(asteroidWorldIdAtLocation);
		string text;
		if (preferredLaunchPadForWorld != null && this.CanLandAtPad(preferredLaunchPadForWorld, out text) == Clustercraft.PadLandingStatus.CanLandImmediately)
		{
			return preferredLaunchPadForWorld;
		}
		foreach (object obj in Components.LaunchPads)
		{
			LaunchPad launchPad = (LaunchPad)obj;
			if (launchPad.GetMyWorldLocation() == location)
			{
				string text2;
				Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(launchPad, out text2);
				if (padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately)
				{
					return launchPad;
				}
				if (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually)
				{
					result = launchPad;
				}
			}
		}
		return result;
	}

	// Token: 0x0600571B RID: 22299 RVA: 0x001FADFC File Offset: 0x001F8FFC
	public bool CanLandAtAsteroid(AxialI location, bool mustLandImmediately)
	{
		LaunchPad destinationPad = this.m_moduleInterface.GetClusterDestinationSelector().GetDestinationPad();
		global::Debug.Assert(destinationPad == null || destinationPad.GetMyWorldLocation() == location, "A rocket is trying to travel to an asteroid but has selected a landing pad at a different asteroid!");
		if (destinationPad != null)
		{
			string text;
			Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(destinationPad, out text);
			return padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately || (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually);
		}
		return this.FindValidLandingPad(location, mustLandImmediately) != null;
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x001FAE6C File Offset: 0x001F906C
	private void Land(LaunchPad pad, bool forceGrounded)
	{
		string text;
		if (this.CanLandAtPad(pad, out text) != Clustercraft.PadLandingStatus.CanLandImmediately)
		{
			return;
		}
		this.BurnFuelForTravel();
		this.m_location = pad.GetMyWorldLocation();
		this.SetCraftStatus(forceGrounded ? Clustercraft.CraftStatus.Grounded : Clustercraft.CraftStatus.Landing);
		this.m_moduleInterface.DoLand(pad);
		this.UpdateStatusItem();
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x001FAEB8 File Offset: 0x001F90B8
	private void Land(AxialI destination, LaunchPad chosenPad)
	{
		if (chosenPad == null)
		{
			chosenPad = this.FindValidLandingPad(destination, true);
		}
		global::Debug.Assert(chosenPad == null || chosenPad.GetMyWorldLocation() == this.m_location, "Attempting to land on a pad that isn't at our current position");
		this.Land(chosenPad, false);
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x001FAF08 File Offset: 0x001F9108
	public void UpdateStatusItem()
	{
		if (ClusterGrid.Instance == null)
		{
			return;
		}
		if (this.mainStatusHandle != Guid.Empty)
		{
			this.selectable.RemoveStatusItem(this.mainStatusHandle, false);
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.Asteroid);
		ClusterGridEntity orbitAsteroid = this.GetOrbitAsteroid();
		bool flag = false;
		if (orbitAsteroid != null)
		{
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == orbitAsteroid.Location)
					{
						flag = true;
						break;
					}
				}
			}
		}
		bool set = false;
		if (visibleEntityOfLayerAtCell != null)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, this.m_clusterTraveler);
		}
		else if (!this.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && !flag)
		{
			set = true;
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.RocketStranded, orbitAsteroid);
		}
		else if (!this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination() && !this.CheckDesinationInRange())
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.DestinationOutOfRange, this.m_clusterTraveler);
		}
		else if (orbitAsteroid != null && this.Destination == orbitAsteroid.Location)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.WaitingToLand, orbitAsteroid);
		}
		else if (this.IsFlightInProgress() || this.Status == Clustercraft.CraftStatus.Launching)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, this.m_clusterTraveler);
		}
		else if (orbitAsteroid != null)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InOrbit, orbitAsteroid);
		}
		else
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		}
		base.GetComponent<KPrefabID>().SetTag(GameTags.RocketStranded, set);
		float num = 0f;
		float num2 = 0f;
		foreach (CargoBayCluster cargoBayCluster in this.GetAllCargoBays())
		{
			num += cargoBayCluster.MaxCapacity;
			num2 += cargoBayCluster.RemainingCapacity;
		}
		if (this.Status != Clustercraft.CraftStatus.Grounded && num > 0f)
		{
			if (num2 == 0f)
			{
				this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, null);
				this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, false);
			}
			else
			{
				this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, false);
				if (this.cargoStatusHandle == Guid.Empty)
				{
					this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, num2);
				}
				else
				{
					this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, true);
					this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, num2);
				}
			}
		}
		else
		{
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, false);
		}
		this.UpdatePilotedStatusItems();
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x001FB354 File Offset: 0x001F9554
	private void UpdateGroundTags()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
		{
			if (@ref != null && !(@ref.Get() == null))
			{
				this.UpdateGroundTags(@ref.Get().gameObject);
			}
		}
		this.UpdateGroundTags(base.gameObject);
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x001FB3D0 File Offset: 0x001F95D0
	private void UpdateGroundTags(GameObject go)
	{
		this.SetTagOnGameObject(go, GameTags.RocketOnGround, this.status == Clustercraft.CraftStatus.Grounded);
		this.SetTagOnGameObject(go, GameTags.RocketNotOnGround, this.status > Clustercraft.CraftStatus.Grounded);
		this.SetTagOnGameObject(go, GameTags.RocketInSpace, this.status == Clustercraft.CraftStatus.InFlight);
		this.SetTagOnGameObject(go, GameTags.EntityInSpace, this.status == Clustercraft.CraftStatus.InFlight);
	}

	// Token: 0x06005721 RID: 22305 RVA: 0x001FB434 File Offset: 0x001F9634
	private void UpdatePilotedStatusItems()
	{
		if (this.Status == Clustercraft.CraftStatus.Grounded)
		{
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
			return;
		}
		bool flag = false;
		bool flag2 = false;
		this.GetPilotedStatus(out flag, out flag2);
		if (flag && flag2)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, this);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
			return;
		}
		if (flag || flag2)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, this);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
			return;
		}
		if (this.ModuleInterface.GetPassengerModule() != null)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, this);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
			return;
		}
		this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, this);
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, false);
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
	}

	// Token: 0x06005722 RID: 22306 RVA: 0x001FB690 File Offset: 0x001F9890
	public void GetPilotedStatus(out bool dupe_piloted, out bool robo_piloted)
	{
		dupe_piloted = false;
		robo_piloted = false;
		UnityEngine.Object passengerModule = this.ModuleInterface.GetPassengerModule();
		RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
		if (passengerModule != null)
		{
			dupe_piloted = (this.AutoPilotMultiplier > 0.5f);
		}
		if (robotPilotModule != null)
		{
			robo_piloted = (robotPilotModule.GetDataBanksStored() > 0f);
		}
	}

	// Token: 0x06005723 RID: 22307 RVA: 0x001FB6E9 File Offset: 0x001F98E9
	private void SetTagOnGameObject(GameObject go, Tag tag, bool set)
	{
		if (set)
		{
			go.AddTag(tag);
			return;
		}
		go.RemoveTag(tag);
	}

	// Token: 0x06005724 RID: 22308 RVA: 0x001FB6FD File Offset: 0x001F98FD
	public override bool ShowName()
	{
		return this.status > Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06005725 RID: 22309 RVA: 0x001FB708 File Offset: 0x001F9908
	public override bool ShowPath()
	{
		return this.status > Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06005726 RID: 22310 RVA: 0x001FB713 File Offset: 0x001F9913
	public bool IsTravellingAndFueled()
	{
		return this.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06005727 RID: 22311 RVA: 0x001FB72C File Offset: 0x001F992C
	public override bool ShowProgressBar()
	{
		return this.IsTravellingAndFueled();
	}

	// Token: 0x06005728 RID: 22312 RVA: 0x001FB734 File Offset: 0x001F9934
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x06005729 RID: 22313 RVA: 0x001FB744 File Offset: 0x001F9944
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.Status != Clustercraft.CraftStatus.Grounded && SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 27))
		{
			UIScheduler.Instance.ScheduleNextFrame("Check Fuel Costs", delegate(object o)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					IFuelTank component = rocketModuleCluster.GetComponent<IFuelTank>();
					if (component != null && !component.Storage.IsEmpty())
					{
						component.DEBUG_FillTank();
					}
					OxidizerTank component2 = rocketModuleCluster.GetComponent<OxidizerTank>();
					if (component2 != null)
					{
						Dictionary<Tag, float> oxidizersAvailable = component2.GetOxidizersAvailable();
						if (oxidizersAvailable.Count > 0)
						{
							foreach (KeyValuePair<Tag, float> keyValuePair in oxidizersAvailable)
							{
								if (keyValuePair.Value > 0f)
								{
									component2.DEBUG_FillTank(ElementLoader.GetElementID(keyValuePair.Key));
									break;
								}
							}
						}
					}
				}
			}, null, null);
		}
	}

	// Token: 0x0600572A RID: 22314 RVA: 0x001FB78E File Offset: 0x001F998E
	public float GetRange()
	{
		return this.ModuleInterface.Range;
	}

	// Token: 0x0600572B RID: 22315 RVA: 0x001FB79B File Offset: 0x001F999B
	public int GetRangeInTiles()
	{
		return this.ModuleInterface.RangeInTiles;
	}

	// Token: 0x0600572C RID: 22316 RVA: 0x001FB7A8 File Offset: 0x001F99A8
	public int GetMaxRangeInTiles()
	{
		return this.ModuleInterface.MaxRange;
	}

	// Token: 0x04003A9A RID: 15002
	[Serialize]
	private string m_name;

	// Token: 0x04003A9C RID: 15004
	private bool wasFlying;

	// Token: 0x04003A9D RID: 15005
	public float LastTimeFlightBegan;

	// Token: 0x04003A9E RID: 15006
	public float LastTimeFlightStopped;

	// Token: 0x04003A9F RID: 15007
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x04003AA0 RID: 15008
	[MyCmpReq]
	private CraftModuleInterface m_moduleInterface;

	// Token: 0x04003AA1 RID: 15009
	private Guid mainStatusHandle;

	// Token: 0x04003AA2 RID: 15010
	private Guid cargoStatusHandle;

	// Token: 0x04003AA3 RID: 15011
	private Guid missionControlStatusHandle = Guid.Empty;

	// Token: 0x04003AA4 RID: 15012
	public static Dictionary<Tag, float> dlc1OxidizerEfficiencies = new Dictionary<Tag, float>
	{
		{
			SimHashes.OxyRock.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.LOW
		},
		{
			SimHashes.LiquidOxygen.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.HIGH
		},
		{
			SimHashes.Fertilizer.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.VERY_LOW
		}
	};

	// Token: 0x04003AA5 RID: 15013
	[Serialize]
	[Range(0f, 1f)]
	public float AutoPilotMultiplier = 1f;

	// Token: 0x04003AA6 RID: 15014
	[Serialize]
	[Range(0f, 2f)]
	public float PilotSkillMultiplier = 1f;

	// Token: 0x04003AA7 RID: 15015
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x04003AA8 RID: 15016
	[Serialize]
	private bool m_launchRequested;

	// Token: 0x04003AA9 RID: 15017
	[Serialize]
	private Clustercraft.CraftStatus status;

	// Token: 0x04003AAA RID: 15018
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04003AAB RID: 15019
	private static EventSystem.IntraObjectHandler<Clustercraft> RocketModuleChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.RocketModuleChanged(data);
	});

	// Token: 0x04003AAC RID: 15020
	private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});

	// Token: 0x04003AAD RID: 15021
	private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationReachedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.OnClusterDestinationReached(data);
	});

	// Token: 0x04003AAE RID: 15022
	private static EventSystem.IntraObjectHandler<Clustercraft> NameChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.SetRocketName(data);
	});

	// Token: 0x02001CF2 RID: 7410
	public enum CraftStatus
	{
		// Token: 0x040089CB RID: 35275
		Grounded,
		// Token: 0x040089CC RID: 35276
		Launching,
		// Token: 0x040089CD RID: 35277
		InFlight,
		// Token: 0x040089CE RID: 35278
		Landing
	}

	// Token: 0x02001CF3 RID: 7411
	public enum CombustionResource
	{
		// Token: 0x040089D0 RID: 35280
		Fuel,
		// Token: 0x040089D1 RID: 35281
		Oxidizer,
		// Token: 0x040089D2 RID: 35282
		All
	}

	// Token: 0x02001CF4 RID: 7412
	public enum PadLandingStatus
	{
		// Token: 0x040089D4 RID: 35284
		CanLandImmediately,
		// Token: 0x040089D5 RID: 35285
		CanLandEventually,
		// Token: 0x040089D6 RID: 35286
		CanNeverLand
	}
}
