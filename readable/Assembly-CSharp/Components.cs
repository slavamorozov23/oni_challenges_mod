using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200086B RID: 2155
public class Components
{
	// Token: 0x06003B32 RID: 15154 RVA: 0x0014AE54 File Offset: 0x00149054
	public static Components.Cmps<MinionIdentity> GetMinionIdentitiesByModel(Tag tag)
	{
		Components.Cmps<MinionIdentity> result = null;
		if (Components.MinionIdentitiesByModel.TryGetValue(tag, out result))
		{
			return result;
		}
		return new Components.Cmps<MinionIdentity>();
	}

	// Token: 0x04002424 RID: 9252
	public static Components.Cmps<RobotAi.Instance> LiveRobotsIdentities = new Components.Cmps<RobotAi.Instance>();

	// Token: 0x04002425 RID: 9253
	public static Components.Cmps<MinionIdentity> LiveMinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04002426 RID: 9254
	public static Components.Cmps<MinionIdentity> MinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04002427 RID: 9255
	public static Components.Cmps<StoredMinionIdentity> StoredMinionIdentities = new Components.Cmps<StoredMinionIdentity>();

	// Token: 0x04002428 RID: 9256
	public static Components.Cmps<MinionStorage> MinionStorages = new Components.Cmps<MinionStorage>();

	// Token: 0x04002429 RID: 9257
	public static Components.Cmps<MinionResume> MinionResumes = new Components.Cmps<MinionResume>();

	// Token: 0x0400242A RID: 9258
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> MinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x0400242B RID: 9259
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> LiveMinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x0400242C RID: 9260
	public static Components.CmpsByWorld<Sleepable> NormalBeds = new Components.CmpsByWorld<Sleepable>();

	// Token: 0x0400242D RID: 9261
	public static Components.Cmps<IUsable> Toilets = new Components.Cmps<IUsable>();

	// Token: 0x0400242E RID: 9262
	public static Components.Cmps<GunkEmptierWorkable> GunkExtractors = new Components.Cmps<GunkEmptierWorkable>();

	// Token: 0x0400242F RID: 9263
	public static Components.Cmps<Pickupable> Pickupables = new Components.Cmps<Pickupable>();

	// Token: 0x04002430 RID: 9264
	public static Components.Cmps<Brain> Brains = new Components.Cmps<Brain>();

	// Token: 0x04002431 RID: 9265
	public static Components.Cmps<BuildingComplete> BuildingCompletes = new Components.Cmps<BuildingComplete>();

	// Token: 0x04002432 RID: 9266
	public static Components.Cmps<Notifier> Notifiers = new Components.Cmps<Notifier>();

	// Token: 0x04002433 RID: 9267
	public static Components.Cmps<Fabricator> Fabricators = new Components.Cmps<Fabricator>();

	// Token: 0x04002434 RID: 9268
	public static Components.Cmps<Refinery> Refineries = new Components.Cmps<Refinery>();

	// Token: 0x04002435 RID: 9269
	public static Components.CmpsByWorld<PlantablePlot> PlantablePlots = new Components.CmpsByWorld<PlantablePlot>();

	// Token: 0x04002436 RID: 9270
	public static Components.Cmps<Ladder> Ladders = new Components.Cmps<Ladder>();

	// Token: 0x04002437 RID: 9271
	public static Components.Cmps<NavTeleporter> NavTeleporters = new Components.Cmps<NavTeleporter>();

	// Token: 0x04002438 RID: 9272
	public static Components.Cmps<ITravelTubePiece> ITravelTubePieces = new Components.Cmps<ITravelTubePiece>();

	// Token: 0x04002439 RID: 9273
	public static Components.CmpsByWorld<CreatureFeeder> CreatureFeeders = new Components.CmpsByWorld<CreatureFeeder>();

	// Token: 0x0400243A RID: 9274
	public static Components.CmpsByWorld<MilkFeeder.Instance> MilkFeeders = new Components.CmpsByWorld<MilkFeeder.Instance>();

	// Token: 0x0400243B RID: 9275
	public static Components.Cmps<Light2D> Light2Ds = new Components.Cmps<Light2D>();

	// Token: 0x0400243C RID: 9276
	public static Components.Cmps<Radiator> Radiators = new Components.Cmps<Radiator>();

	// Token: 0x0400243D RID: 9277
	public static Components.Cmps<Edible> Edibles = new Components.Cmps<Edible>();

	// Token: 0x0400243E RID: 9278
	public static Components.Cmps<Diggable> Diggables = new Components.Cmps<Diggable>();

	// Token: 0x0400243F RID: 9279
	public static Components.Cmps<IResearchCenter> ResearchCenters = new Components.Cmps<IResearchCenter>();

	// Token: 0x04002440 RID: 9280
	public static Components.Cmps<Harvestable> Harvestables = new Components.Cmps<Harvestable>();

	// Token: 0x04002441 RID: 9281
	public static Components.Cmps<HarvestDesignatable> HarvestDesignatables = new Components.Cmps<HarvestDesignatable>();

	// Token: 0x04002442 RID: 9282
	public static Components.Cmps<Uprootable> Uprootables = new Components.Cmps<Uprootable>();

	// Token: 0x04002443 RID: 9283
	public static Components.Cmps<Health> Health = new Components.Cmps<Health>();

	// Token: 0x04002444 RID: 9284
	public static Components.Cmps<Equipment> Equipment = new Components.Cmps<Equipment>();

	// Token: 0x04002445 RID: 9285
	public static Components.Cmps<FactionAlignment> FactionAlignments = new Components.Cmps<FactionAlignment>();

	// Token: 0x04002446 RID: 9286
	public static Components.Cmps<FactionAlignment> PlayerTargeted = new Components.Cmps<FactionAlignment>();

	// Token: 0x04002447 RID: 9287
	public static Components.Cmps<Telepad> Telepads = new Components.Cmps<Telepad>();

	// Token: 0x04002448 RID: 9288
	public static Components.Cmps<Generator> Generators = new Components.Cmps<Generator>();

	// Token: 0x04002449 RID: 9289
	public static Components.Cmps<EnergyConsumer> EnergyConsumers = new Components.Cmps<EnergyConsumer>();

	// Token: 0x0400244A RID: 9290
	public static Components.Cmps<Battery> Batteries = new Components.Cmps<Battery>();

	// Token: 0x0400244B RID: 9291
	public static Components.Cmps<Breakable> Breakables = new Components.Cmps<Breakable>();

	// Token: 0x0400244C RID: 9292
	public static Components.Cmps<Crop> Crops = new Components.Cmps<Crop>();

	// Token: 0x0400244D RID: 9293
	public static Components.Cmps<Prioritizable> Prioritizables = new Components.Cmps<Prioritizable>();

	// Token: 0x0400244E RID: 9294
	public static Components.Cmps<Clinic> Clinics = new Components.Cmps<Clinic>();

	// Token: 0x0400244F RID: 9295
	public static Components.Cmps<HandSanitizer> HandSanitizers = new Components.Cmps<HandSanitizer>();

	// Token: 0x04002450 RID: 9296
	public static Components.Cmps<EntityCellVisualizer> EntityCellVisualizers = new Components.Cmps<EntityCellVisualizer>();

	// Token: 0x04002451 RID: 9297
	public static Components.Cmps<RoleStation> RoleStations = new Components.Cmps<RoleStation>();

	// Token: 0x04002452 RID: 9298
	public static Components.Cmps<Telescope> Telescopes = new Components.Cmps<Telescope>();

	// Token: 0x04002453 RID: 9299
	public static Components.Cmps<Capturable> Capturables = new Components.Cmps<Capturable>();

	// Token: 0x04002454 RID: 9300
	public static Components.Cmps<NotCapturable> NotCapturables = new Components.Cmps<NotCapturable>();

	// Token: 0x04002455 RID: 9301
	public static Components.Cmps<DiseaseSourceVisualizer> DiseaseSourceVisualizers = new Components.Cmps<DiseaseSourceVisualizer>();

	// Token: 0x04002456 RID: 9302
	public static Components.Cmps<Grave> Graves = new Components.Cmps<Grave>();

	// Token: 0x04002457 RID: 9303
	public static Components.Cmps<AttachableBuilding> AttachableBuildings = new Components.Cmps<AttachableBuilding>();

	// Token: 0x04002458 RID: 9304
	public static Components.Cmps<BuildingAttachPoint> BuildingAttachPoints = new Components.Cmps<BuildingAttachPoint>();

	// Token: 0x04002459 RID: 9305
	public static Components.Cmps<MinionAssignablesProxy> MinionAssignablesProxy = new Components.Cmps<MinionAssignablesProxy>();

	// Token: 0x0400245A RID: 9306
	public static Components.Cmps<ComplexFabricator> ComplexFabricators = new Components.Cmps<ComplexFabricator>();

	// Token: 0x0400245B RID: 9307
	public static Components.Cmps<MonumentPart> MonumentParts = new Components.Cmps<MonumentPart>();

	// Token: 0x0400245C RID: 9308
	public static Components.Cmps<PlantableSeed> PlantableSeeds = new Components.Cmps<PlantableSeed>();

	// Token: 0x0400245D RID: 9309
	public static Components.Cmps<IBasicBuilding> BasicBuildings = new Components.Cmps<IBasicBuilding>();

	// Token: 0x0400245E RID: 9310
	public static Components.Cmps<Painting> Paintings = new Components.Cmps<Painting>();

	// Token: 0x0400245F RID: 9311
	public static Components.Cmps<BuildingComplete> TemplateBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04002460 RID: 9312
	public static Components.Cmps<Teleporter> Teleporters = new Components.Cmps<Teleporter>();

	// Token: 0x04002461 RID: 9313
	public static Components.Cmps<MutantPlant> MutantPlants = new Components.Cmps<MutantPlant>();

	// Token: 0x04002462 RID: 9314
	public static Components.Cmps<LandingBeacon.Instance> LandingBeacons = new Components.Cmps<LandingBeacon.Instance>();

	// Token: 0x04002463 RID: 9315
	public static Components.Cmps<HighEnergyParticle> HighEnergyParticles = new Components.Cmps<HighEnergyParticle>();

	// Token: 0x04002464 RID: 9316
	public static Components.Cmps<HighEnergyParticlePort> HighEnergyParticlePorts = new Components.Cmps<HighEnergyParticlePort>();

	// Token: 0x04002465 RID: 9317
	public static Components.Cmps<Clustercraft> Clustercrafts = new Components.Cmps<Clustercraft>();

	// Token: 0x04002466 RID: 9318
	public static Components.Cmps<ClustercraftInteriorDoor> ClusterCraftInteriorDoors = new Components.Cmps<ClustercraftInteriorDoor>();

	// Token: 0x04002467 RID: 9319
	public static Components.Cmps<PassengerRocketModule> PassengerRocketModules = new Components.Cmps<PassengerRocketModule>();

	// Token: 0x04002468 RID: 9320
	public static Components.Cmps<ClusterTraveler> ClusterTravelers = new Components.Cmps<ClusterTraveler>();

	// Token: 0x04002469 RID: 9321
	public static Components.Cmps<LaunchPad> LaunchPads = new Components.Cmps<LaunchPad>();

	// Token: 0x0400246A RID: 9322
	public static Components.Cmps<WarpReceiver> WarpReceivers = new Components.Cmps<WarpReceiver>();

	// Token: 0x0400246B RID: 9323
	public static Components.Cmps<RocketControlStation> RocketControlStations = new Components.Cmps<RocketControlStation>();

	// Token: 0x0400246C RID: 9324
	public static Components.Cmps<Reactor> NuclearReactors = new Components.Cmps<Reactor>();

	// Token: 0x0400246D RID: 9325
	public static Components.Cmps<BuildingComplete> EntombedBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x0400246E RID: 9326
	public static Components.Cmps<SpaceArtifact> SpaceArtifacts = new Components.Cmps<SpaceArtifact>();

	// Token: 0x0400246F RID: 9327
	public static Components.Cmps<ArtifactAnalysisStationWorkable> ArtifactAnalysisStations = new Components.Cmps<ArtifactAnalysisStationWorkable>();

	// Token: 0x04002470 RID: 9328
	public static Components.Cmps<RocketConduitReceiver> RocketConduitReceivers = new Components.Cmps<RocketConduitReceiver>();

	// Token: 0x04002471 RID: 9329
	public static Components.Cmps<RocketConduitSender> RocketConduitSenders = new Components.Cmps<RocketConduitSender>();

	// Token: 0x04002472 RID: 9330
	public static Components.Cmps<LogicBroadcaster> LogicBroadcasters = new Components.Cmps<LogicBroadcaster>();

	// Token: 0x04002473 RID: 9331
	public static Components.Cmps<Telephone> Telephones = new Components.Cmps<Telephone>();

	// Token: 0x04002474 RID: 9332
	public static Components.Cmps<MissionControlWorkable> MissionControlWorkables = new Components.Cmps<MissionControlWorkable>();

	// Token: 0x04002475 RID: 9333
	public static Components.Cmps<MissionControlClusterWorkable> MissionControlClusterWorkables = new Components.Cmps<MissionControlClusterWorkable>();

	// Token: 0x04002476 RID: 9334
	public static Components.Cmps<MinorFossilDigSite.Instance> MinorFossilDigSites = new Components.Cmps<MinorFossilDigSite.Instance>();

	// Token: 0x04002477 RID: 9335
	public static Components.Cmps<MajorFossilDigSite.Instance> MajorFossilDigSites = new Components.Cmps<MajorFossilDigSite.Instance>();

	// Token: 0x04002478 RID: 9336
	public static Components.Cmps<GameObject> FoodRehydrators = new Components.Cmps<GameObject>();

	// Token: 0x04002479 RID: 9337
	public static Components.CmpsByWorld<SocialGatheringPoint> SocialGatheringPoints = new Components.CmpsByWorld<SocialGatheringPoint>();

	// Token: 0x0400247A RID: 9338
	public static Components.CmpsByWorld<Geyser> Geysers = new Components.CmpsByWorld<Geyser>();

	// Token: 0x0400247B RID: 9339
	public static Components.CmpsByWorld<GeoTuner.Instance> GeoTuners = new Components.CmpsByWorld<GeoTuner.Instance>();

	// Token: 0x0400247C RID: 9340
	public static Components.CmpsByWorld<CritterCondo.Instance> CritterCondos = new Components.CmpsByWorld<CritterCondo.Instance>();

	// Token: 0x0400247D RID: 9341
	public static Components.CmpsByWorld<GeothermalController> GeothermalControllers = new Components.CmpsByWorld<GeothermalController>();

	// Token: 0x0400247E RID: 9342
	public static Components.CmpsByWorld<GeothermalVent> GeothermalVents = new Components.CmpsByWorld<GeothermalVent>();

	// Token: 0x0400247F RID: 9343
	public static Components.CmpsByWorld<RemoteWorkerDock> RemoteWorkerDocks = new Components.CmpsByWorld<RemoteWorkerDock>();

	// Token: 0x04002480 RID: 9344
	public static Components.CmpsByWorld<IRemoteDockWorkTarget> RemoteDockWorkTargets = new Components.CmpsByWorld<IRemoteDockWorkTarget>();

	// Token: 0x04002481 RID: 9345
	public static Components.Cmps<Assignable> AssignableItems = new Components.Cmps<Assignable>();

	// Token: 0x04002482 RID: 9346
	public static Components.CmpsByWorld<Comet> Meteors = new Components.CmpsByWorld<Comet>();

	// Token: 0x04002483 RID: 9347
	public static Components.CmpsByWorld<DetectorNetwork.Instance> DetectorNetworks = new Components.CmpsByWorld<DetectorNetwork.Instance>();

	// Token: 0x04002484 RID: 9348
	public static Components.CmpsByWorld<ScannerNetworkVisualizer> ScannerVisualizers = new Components.CmpsByWorld<ScannerNetworkVisualizer>();

	// Token: 0x04002485 RID: 9349
	public static Components.CmpsByWorld<Electrobank> Electrobanks = new Components.CmpsByWorld<Electrobank>();

	// Token: 0x04002486 RID: 9350
	public static Components.CmpsByWorld<SelfChargingElectrobank> SelfChargingElectrobanks = new Components.CmpsByWorld<SelfChargingElectrobank>();

	// Token: 0x04002487 RID: 9351
	public static Components.Cmps<ClusterGridEntity> LongRangeMissileTargetables = new Components.Cmps<ClusterGridEntity>();

	// Token: 0x04002488 RID: 9352
	public static Components.Cmps<IncubationMonitor.Instance> IncubationMonitors = new Components.Cmps<IncubationMonitor.Instance>();

	// Token: 0x04002489 RID: 9353
	public static Components.Cmps<FixedCapturableMonitor.Instance> FixedCapturableMonitors = new Components.Cmps<FixedCapturableMonitor.Instance>();

	// Token: 0x0400248A RID: 9354
	public static Components.Cmps<BeeHive.StatesInstance> BeeHives = new Components.Cmps<BeeHive.StatesInstance>();

	// Token: 0x0400248B RID: 9355
	public static Components.Cmps<StateMachine.Instance> EffectImmunityProviderStations = new Components.Cmps<StateMachine.Instance>();

	// Token: 0x0400248C RID: 9356
	public static Components.Cmps<PeeChoreMonitor.Instance> CriticalBladders = new Components.Cmps<PeeChoreMonitor.Instance>();

	// Token: 0x0400248D RID: 9357
	public static Components.Cmps<MissileLauncher.Instance> MissileLaunchers = new Components.Cmps<MissileLauncher.Instance>();

	// Token: 0x02001826 RID: 6182
	public class Cmps<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06009DE5 RID: 40421 RVA: 0x003A12D7 File Offset: 0x0039F4D7
		public List<T> Items
		{
			get
			{
				return this.items.GetDataList();
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06009DE6 RID: 40422 RVA: 0x003A12E4 File Offset: 0x0039F4E4
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x06009DE7 RID: 40423 RVA: 0x003A12F1 File Offset: 0x0039F4F1
		public Cmps()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.items = new KCompactedVector<T>(0);
			this.table = new Dictionary<T, HandleVector<int>.Handle>();
		}

		// Token: 0x17000AC0 RID: 2752
		public T this[int idx]
		{
			get
			{
				return this.Items[idx];
			}
		}

		// Token: 0x06009DE9 RID: 40425 RVA: 0x003A133E File Offset: 0x0039F53E
		private void Clear()
		{
			this.items.Clear();
			this.table.Clear();
			this.OnAdd = null;
			this.OnRemove = null;
		}

		// Token: 0x06009DEA RID: 40426 RVA: 0x003A1364 File Offset: 0x0039F564
		public void Add(T cmp)
		{
			HandleVector<int>.Handle value = this.items.Allocate(cmp);
			this.table[cmp] = value;
			if (this.OnAdd != null)
			{
				this.OnAdd(cmp);
			}
		}

		// Token: 0x06009DEB RID: 40427 RVA: 0x003A13A0 File Offset: 0x0039F5A0
		public void Remove(T cmp)
		{
			HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
			if (this.table.TryGetValue(cmp, out invalidHandle))
			{
				this.table.Remove(cmp);
				this.items.Free(invalidHandle);
				if (this.OnRemove != null)
				{
					this.OnRemove(cmp);
				}
			}
		}

		// Token: 0x06009DEC RID: 40428 RVA: 0x003A13F4 File Offset: 0x0039F5F4
		public void Register(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd += on_add;
			this.OnRemove += on_remove;
			foreach (T obj in this.Items)
			{
				this.OnAdd(obj);
			}
		}

		// Token: 0x06009DED RID: 40429 RVA: 0x003A145C File Offset: 0x0039F65C
		public void Unregister(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd -= on_add;
			this.OnRemove -= on_remove;
		}

		// Token: 0x06009DEE RID: 40430 RVA: 0x003A146C File Offset: 0x0039F66C
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds = false)
		{
			if (ClusterManager.Instance.worldCount == 1)
			{
				return this.Items;
			}
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
				if (world != null)
				{
					otherWorldIds = world.GetChildWorldIds();
				}
			}
			return this.GetWorldItems(worldId, otherWorldIds, null);
		}

		// Token: 0x06009DEF RID: 40431 RVA: 0x003A14B8 File Offset: 0x0039F6B8
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds, Func<T, bool> filter)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
				if (world != null)
				{
					otherWorldIds = world.GetChildWorldIds();
				}
			}
			return this.GetWorldItems(worldId, otherWorldIds, filter);
		}

		// Token: 0x06009DF0 RID: 40432 RVA: 0x003A14F0 File Offset: 0x0039F6F0
		public List<T> GetWorldItems(int worldId, ICollection<int> otherWorldIds, Func<T, bool> filter)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.Items.Count; i++)
			{
				T t = this.Items[i];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				bool flag = worldId == myWorldId;
				if (!flag && otherWorldIds != null && otherWorldIds.Contains(myWorldId))
				{
					flag = true;
				}
				if (flag && filter != null)
				{
					flag = filter(t);
				}
				if (flag)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x06009DF1 RID: 40433 RVA: 0x003A156C File Offset: 0x0039F76C
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06009DF2 RID: 40434 RVA: 0x003A1597 File Offset: 0x0039F797
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			int num;
			for (int index = 0; index < this.Items.Count; index = num + 1)
			{
				T t = this.Items[index];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				if (myWorldId == worldId || (otherWorldIds != null && otherWorldIds.Contains(myWorldId)))
				{
					yield return t;
				}
				num = index;
			}
			yield break;
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06009DF3 RID: 40435 RVA: 0x003A15B8 File Offset: 0x0039F7B8
		// (remove) Token: 0x06009DF4 RID: 40436 RVA: 0x003A15F0 File Offset: 0x0039F7F0
		public event Action<T> OnAdd;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06009DF5 RID: 40437 RVA: 0x003A1628 File Offset: 0x0039F828
		// (remove) Token: 0x06009DF6 RID: 40438 RVA: 0x003A1660 File Offset: 0x0039F860
		public event Action<T> OnRemove;

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06009DF7 RID: 40439 RVA: 0x003A1695 File Offset: 0x0039F895
		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06009DF8 RID: 40440 RVA: 0x003A169C File Offset: 0x0039F89C
		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06009DF9 RID: 40441 RVA: 0x003A16A3 File Offset: 0x0039F8A3
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009DFA RID: 40442 RVA: 0x003A16AA File Offset: 0x0039F8AA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06009DFB RID: 40443 RVA: 0x003A16BC File Offset: 0x0039F8BC
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06009DFC RID: 40444 RVA: 0x003A16CE File Offset: 0x0039F8CE
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x040079EF RID: 31215
		private Dictionary<T, HandleVector<int>.Handle> table;

		// Token: 0x040079F0 RID: 31216
		private KCompactedVector<T> items;
	}

	// Token: 0x02001827 RID: 6183
	public class CmpsByWorld<T>
	{
		// Token: 0x06009DFD RID: 40445 RVA: 0x003A16E0 File Offset: 0x0039F8E0
		public CmpsByWorld()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.m_CmpsByWorld = new Dictionary<int, Components.Cmps<T>>();
		}

		// Token: 0x06009DFE RID: 40446 RVA: 0x003A1713 File Offset: 0x0039F913
		public void Clear()
		{
			this.m_CmpsByWorld.Clear();
		}

		// Token: 0x06009DFF RID: 40447 RVA: 0x003A1720 File Offset: 0x0039F920
		public Components.Cmps<T> CreateOrGetCmps(int worldId)
		{
			Components.Cmps<T> cmps;
			if (!this.m_CmpsByWorld.TryGetValue(worldId, out cmps))
			{
				cmps = new Components.Cmps<T>();
				this.m_CmpsByWorld[worldId] = cmps;
			}
			return cmps;
		}

		// Token: 0x06009E00 RID: 40448 RVA: 0x003A1751 File Offset: 0x0039F951
		public void Add(int worldId, T cmp)
		{
			DebugUtil.DevAssertArgs(worldId != -1, new object[]
			{
				"CmpsByWorld tried to add a component to an invalid world. Did you call this during a state machine's constructor instead of StartSM? ",
				cmp
			});
			this.CreateOrGetCmps(worldId).Add(cmp);
		}

		// Token: 0x06009E01 RID: 40449 RVA: 0x003A1783 File Offset: 0x0039F983
		public void Remove(int worldId, T cmp)
		{
			this.CreateOrGetCmps(worldId).Remove(cmp);
		}

		// Token: 0x06009E02 RID: 40450 RVA: 0x003A1792 File Offset: 0x0039F992
		public void Register(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Register(on_add, on_remove);
		}

		// Token: 0x06009E03 RID: 40451 RVA: 0x003A17A2 File Offset: 0x0039F9A2
		public void Unregister(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Unregister(on_add, on_remove);
		}

		// Token: 0x06009E04 RID: 40452 RVA: 0x003A17B2 File Offset: 0x0039F9B2
		public List<T> GetItems(int worldId)
		{
			return this.CreateOrGetCmps(worldId).Items;
		}

		// Token: 0x06009E05 RID: 40453 RVA: 0x003A17C0 File Offset: 0x0039F9C0
		public Dictionary<int, Components.Cmps<T>>.KeyCollection GetWorldsIds()
		{
			return this.m_CmpsByWorld.Keys;
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06009E06 RID: 40454 RVA: 0x003A17D0 File Offset: 0x0039F9D0
		public int GlobalCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<int, Components.Cmps<T>> keyValuePair in this.m_CmpsByWorld)
				{
					num += keyValuePair.Value.Count;
				}
				return num;
			}
		}

		// Token: 0x06009E07 RID: 40455 RVA: 0x003A1830 File Offset: 0x0039FA30
		public int CountWorldItems(int worldId, bool includeChildren = false)
		{
			int num = this.GetItems(worldId).Count;
			if (includeChildren)
			{
				foreach (int worldId2 in ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds())
				{
					num += this.GetItems(worldId2).Count;
				}
			}
			return num;
		}

		// Token: 0x06009E08 RID: 40456 RVA: 0x003A18A0 File Offset: 0x0039FAA0
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06009E09 RID: 40457 RVA: 0x003A18CB File Offset: 0x0039FACB
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			List<T> items = this.GetItems(worldId);
			int num;
			for (int index = 0; index < items.Count; index = num + 1)
			{
				yield return items[index];
				num = index;
			}
			if (otherWorldIds != null)
			{
				foreach (int worldId2 in otherWorldIds)
				{
					items = this.GetItems(worldId2);
					for (int index = 0; index < items.Count; index = num + 1)
					{
						yield return items[index];
						num = index;
					}
				}
				IEnumerator<int> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x040079F3 RID: 31219
		private Dictionary<int, Components.Cmps<T>> m_CmpsByWorld;
	}
}
