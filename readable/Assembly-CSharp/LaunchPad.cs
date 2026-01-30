using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
public class LaunchPad : KMonoBehaviour, ISim1000ms, IListableOption, IProcessConditionSet
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06005852 RID: 22610 RVA: 0x002011B8 File Offset: 0x001FF3B8
	public RocketModuleCluster LandedRocket
	{
		get
		{
			GameObject gameObject = null;
			Grid.ObjectLayers[1].TryGetValue(this.RocketBottomPosition, out gameObject);
			if (gameObject != null)
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				Clustercraft clustercraft = (component != null && component.CraftInterface != null) ? component.CraftInterface.GetComponent<Clustercraft>() : null;
				if (clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Grounded || clustercraft.Status == Clustercraft.CraftStatus.Landing))
				{
					return component;
				}
			}
			return null;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06005853 RID: 22611 RVA: 0x0020122F File Offset: 0x001FF42F
	public int RocketBottomPosition
	{
		get
		{
			return Grid.OffsetCell(Grid.PosToCell(this), this.baseModulePosition);
		}
	}

	// Token: 0x06005854 RID: 22612 RVA: 0x00201244 File Offset: 0x001FF444
	[OnDeserialized]
	private void OnDeserialzed()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 24))
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				component.RunOnArea(delegate(int cell)
				{
					if (Grid.IsValidCell(cell) && Grid.Solid[cell])
					{
						SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.LaunchpadDesolidify, 0f, -1f, byte.MaxValue, 0, -1);
					}
				});
			}
		}
	}

	// Token: 0x06005855 RID: 22613 RVA: 0x002012A0 File Offset: 0x001FF4A0
	protected override void OnPrefabInit()
	{
		UserNameable component = base.GetComponent<UserNameable>();
		if (component != null)
		{
			component.SetName(GameUtil.GenerateRandomLaunchPadName());
		}
	}

	// Token: 0x06005856 RID: 22614 RVA: 0x002012C8 File Offset: 0x001FF4C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tower = new LaunchPad.LaunchPadTower(this, this.maxTowerHeight);
		this.OnRocketBuildingChanged(this.GetRocketBaseModule());
		this.partitionerEntry = GameScenePartitioner.Instance.Add("LaunchPad.OnSpawn", base.gameObject, Extents.OneCell(this.RocketBottomPosition), GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnRocketBuildingChanged));
		Components.LaunchPads.Add(this);
		this.CheckLandedRocketPassengerModuleStatus();
		int num = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(base.gameObject);
		if (num < 35)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.RocketPlatformCloseToCeiling, num);
		}
	}

	// Token: 0x06005857 RID: 22615 RVA: 0x0020137C File Offset: 0x001FF57C
	protected override void OnCleanUp()
	{
		Components.LaunchPads.Remove(this);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.lastBaseAttachable != null)
		{
			AttachableBuilding attachableBuilding = this.lastBaseAttachable;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			this.lastBaseAttachable = null;
		}
		this.RebuildLaunchTowerHeightHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005858 RID: 22616 RVA: 0x002013F4 File Offset: 0x001FF5F4
	private void CheckLandedRocketPassengerModuleStatus()
	{
		if (this.LandedRocket == null)
		{
			this.selectable.RemoveStatusItem(this.landedRocketPassengerModuleStatusItem, false);
			this.landedRocketPassengerModuleStatusItem = Guid.Empty;
			return;
		}
		if (this.LandedRocket.CraftInterface.GetPassengerModule() == null)
		{
			if (this.landedRocketPassengerModuleStatusItem == Guid.Empty)
			{
				this.landedRocketPassengerModuleStatusItem = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.LandedRocketLacksPassengerModule, null);
				return;
			}
		}
		else if (this.landedRocketPassengerModuleStatusItem != Guid.Empty)
		{
			this.selectable.RemoveStatusItem(this.landedRocketPassengerModuleStatusItem, false);
			this.landedRocketPassengerModuleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06005859 RID: 22617 RVA: 0x002014AC File Offset: 0x001FF6AC
	public bool IsLogicInputConnected()
	{
		int portCell = this.ports.GetPortCell(this.triggerPort);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x0600585A RID: 22618 RVA: 0x002014E0 File Offset: 0x001FF6E0
	public void Sim1000ms(float dt)
	{
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null && this.IsLogicInputConnected())
		{
			if (this.ports.GetInputValue(this.triggerPort) == 1)
			{
				if (landedRocket.CraftInterface.CheckReadyForAutomatedLaunchCommand())
				{
					landedRocket.CraftInterface.TriggerLaunch(true);
				}
				else
				{
					landedRocket.CraftInterface.CancelLaunch();
				}
			}
			else
			{
				landedRocket.CraftInterface.CancelLaunch();
			}
		}
		this.CheckLandedRocketPassengerModuleStatus();
		this.ports.SendSignal(this.landedRocketPort, (landedRocket != null) ? 1 : 0);
		if (landedRocket != null)
		{
			this.ports.SendSignal(this.statusPort, (landedRocket.CraftInterface.CheckReadyForAutomatedLaunch() || landedRocket.CraftInterface.HasTag(GameTags.RocketNotOnGround)) ? 1 : 0);
			return;
		}
		this.ports.SendSignal(this.statusPort, 0);
	}

	// Token: 0x0600585B RID: 22619 RVA: 0x002015C0 File Offset: 0x001FF7C0
	public GameObject AddBaseModule(BuildingDef moduleDefID, IList<Tag> elements)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.baseModulePosition);
		GameObject gameObject;
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			gameObject = moduleDefID.Build(cell, Orientation.Neutral, null, elements, 293.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject = moduleDefID.TryPlace(null, Grid.CellToPosCBC(cell, moduleDefID.SceneLayer), Orientation.Neutral, elements, 0);
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("Clustercraft"), null, null);
		gameObject2.SetActive(true);
		Clustercraft component = gameObject2.GetComponent<Clustercraft>();
		component.GetComponent<CraftModuleInterface>().AddModule(gameObject.GetComponent<RocketModuleCluster>());
		component.Init(this.GetMyWorldLocation(), this);
		if (gameObject.GetComponent<BuildingUnderConstruction>() != null)
		{
			this.OnRocketBuildingChanged(gameObject);
		}
		base.Trigger(374403796, null);
		return gameObject;
	}

	// Token: 0x0600585C RID: 22620 RVA: 0x00201690 File Offset: 0x001FF890
	private void OnRocketBuildingChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		RocketModuleCluster landedRocket = this.LandedRocket;
		global::Debug.Assert(gameObject == null || landedRocket == null || landedRocket.gameObject == gameObject, "Launch Pad had a rocket land or take off on it twice??");
		Clustercraft clustercraft = (landedRocket != null && landedRocket.CraftInterface != null) ? landedRocket.CraftInterface.GetComponent<Clustercraft>() : null;
		if (clustercraft != null)
		{
			if (clustercraft.Status == Clustercraft.CraftStatus.Landing)
			{
				base.Trigger(-887025858, landedRocket);
			}
			else if (clustercraft.Status == Clustercraft.CraftStatus.Launching)
			{
				base.Trigger(-1277991738, landedRocket);
				AttachableBuilding component = landedRocket.CraftInterface.ClusterModules[0].Get().GetComponent<AttachableBuilding>();
				component.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(component.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			}
		}
		this.OnRocketLayoutChanged(null);
	}

	// Token: 0x0600585D RID: 22621 RVA: 0x00201774 File Offset: 0x001FF974
	private void OnRocketLayoutChanged(object data)
	{
		if (this.lastBaseAttachable != null)
		{
			AttachableBuilding attachableBuilding = this.lastBaseAttachable;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			this.lastBaseAttachable = null;
		}
		GameObject rocketBaseModule = this.GetRocketBaseModule();
		if (rocketBaseModule != null)
		{
			this.lastBaseAttachable = rocketBaseModule.GetComponent<AttachableBuilding>();
			AttachableBuilding attachableBuilding2 = this.lastBaseAttachable;
			attachableBuilding2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachableBuilding2.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
		}
		this.DirtyTowerHeight();
	}

	// Token: 0x0600585E RID: 22622 RVA: 0x00201806 File Offset: 0x001FFA06
	public bool HasRocket()
	{
		return this.LandedRocket != null;
	}

	// Token: 0x0600585F RID: 22623 RVA: 0x00201814 File Offset: 0x001FFA14
	public bool HasRocketWithCommandModule()
	{
		return this.HasRocket() && this.LandedRocket.CraftInterface.FindLaunchableRocket() != null;
	}

	// Token: 0x06005860 RID: 22624 RVA: 0x00201838 File Offset: 0x001FFA38
	private GameObject GetRocketBaseModule()
	{
		GameObject gameObject = Grid.Objects[Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.baseModulePosition), 1];
		if (!(gameObject != null) || !(gameObject.GetComponent<RocketModule>() != null))
		{
			return null;
		}
		return gameObject;
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x00201884 File Offset: 0x001FFA84
	public void DirtyTowerHeight()
	{
		if (!this.dirtyTowerHeight)
		{
			this.dirtyTowerHeight = true;
			if (!this.RebuildLaunchTowerHeightHandler.IsValid)
			{
				this.RebuildLaunchTowerHeightHandler = GameScheduler.Instance.ScheduleNextFrame("RebuildLaunchTowerHeight", new Action<object>(this.RebuildLaunchTowerHeight), null, null);
			}
		}
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x002018D0 File Offset: 0x001FFAD0
	private void RebuildLaunchTowerHeight(object obj)
	{
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			this.tower.SetTowerHeight(landedRocket.CraftInterface.MaxHeight);
		}
		this.dirtyTowerHeight = false;
		this.RebuildLaunchTowerHeightHandler.ClearScheduler();
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x00201915 File Offset: 0x001FFB15
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x06005864 RID: 22628 RVA: 0x00201924 File Offset: 0x001FFB24
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		RocketProcessConditionDisplayTarget rocketProcessConditionDisplayTarget = null;
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			for (int i = 0; i < landedRocket.CraftInterface.ClusterModules.Count; i++)
			{
				RocketProcessConditionDisplayTarget component = landedRocket.CraftInterface.ClusterModules[i].Get().GetComponent<RocketProcessConditionDisplayTarget>();
				if (component != null)
				{
					rocketProcessConditionDisplayTarget = component;
					break;
				}
			}
		}
		if (rocketProcessConditionDisplayTarget != null)
		{
			return ((IProcessConditionSet)rocketProcessConditionDisplayTarget).GetConditionSet(conditionType);
		}
		return new List<ProcessCondition>();
	}

	// Token: 0x06005865 RID: 22629 RVA: 0x002019A0 File Offset: 0x001FFBA0
	public int PopulateConditionSet(ProcessCondition.ProcessConditionType conditionType, List<ProcessCondition> conditions)
	{
		RocketProcessConditionDisplayTarget rocketProcessConditionDisplayTarget = null;
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			for (int i = 0; i < landedRocket.CraftInterface.ClusterModules.Count; i++)
			{
				RocketProcessConditionDisplayTarget component = landedRocket.CraftInterface.ClusterModules[i].Get().GetComponent<RocketProcessConditionDisplayTarget>();
				if (component != null)
				{
					rocketProcessConditionDisplayTarget = component;
					break;
				}
			}
		}
		if (rocketProcessConditionDisplayTarget != null)
		{
			return ((IProcessConditionSet)rocketProcessConditionDisplayTarget).PopulateConditionSet(conditionType, conditions);
		}
		return 0;
	}

	// Token: 0x06005866 RID: 22630 RVA: 0x00201A18 File Offset: 0x001FFC18
	public static List<LaunchPad> GetLaunchPadsForDestination(AxialI destination)
	{
		List<LaunchPad> list = new List<LaunchPad>();
		foreach (object obj in Components.LaunchPads)
		{
			LaunchPad launchPad = (LaunchPad)obj;
			if (launchPad.GetMyWorldLocation() == destination)
			{
				list.Add(launchPad);
			}
		}
		return list;
	}

	// Token: 0x04003B4E RID: 15182
	public HashedString triggerPort;

	// Token: 0x04003B4F RID: 15183
	public HashedString statusPort;

	// Token: 0x04003B50 RID: 15184
	public HashedString landedRocketPort;

	// Token: 0x04003B51 RID: 15185
	private CellOffset baseModulePosition = new CellOffset(0, 2);

	// Token: 0x04003B52 RID: 15186
	[MyCmpReq]
	private LogicPorts ports;

	// Token: 0x04003B53 RID: 15187
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003B54 RID: 15188
	private SchedulerHandle RebuildLaunchTowerHeightHandler;

	// Token: 0x04003B55 RID: 15189
	private AttachableBuilding lastBaseAttachable;

	// Token: 0x04003B56 RID: 15190
	private LaunchPad.LaunchPadTower tower;

	// Token: 0x04003B57 RID: 15191
	[Serialize]
	public int maxTowerHeight;

	// Token: 0x04003B58 RID: 15192
	private bool dirtyTowerHeight;

	// Token: 0x04003B59 RID: 15193
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003B5A RID: 15194
	private Guid landedRocketPassengerModuleStatusItem = Guid.Empty;

	// Token: 0x02001D15 RID: 7445
	public class LaunchPadTower
	{
		// Token: 0x0600AFF3 RID: 45043 RVA: 0x003D8004 File Offset: 0x003D6204
		public LaunchPadTower(LaunchPad pad, int startHeight)
		{
			this.pad = pad;
			this.SetTowerHeight(startHeight);
		}

		// Token: 0x0600AFF4 RID: 45044 RVA: 0x003D80BC File Offset: 0x003D62BC
		public void AddTowerRow()
		{
			GameObject gameObject = new GameObject("LaunchPadTowerRow");
			gameObject.SetActive(false);
			gameObject.transform.SetParent(this.pad.transform);
			gameObject.transform.SetLocalPosition(Grid.CellSizeInMeters * Vector3.up * (float)(this.towerAnimControllers.Count + this.pad.baseModulePosition.y));
			gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Backwall)));
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("rocket_launchpad_tower_kanim")
			};
			gameObject.SetActive(true);
			this.towerAnimControllers.Add(kbatchedAnimController);
			kbatchedAnimController.initialAnim = this.towerBGAnimNames[this.towerAnimControllers.Count % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off;
			this.animLink = new KAnimLink(this.pad.GetComponent<KAnimControllerBase>(), kbatchedAnimController);
		}

		// Token: 0x0600AFF5 RID: 45045 RVA: 0x003D81E0 File Offset: 0x003D63E0
		public void RemoveTowerRow()
		{
		}

		// Token: 0x0600AFF6 RID: 45046 RVA: 0x003D81E4 File Offset: 0x003D63E4
		public void SetTowerHeight(int height)
		{
			if (height < 8)
			{
				height = 0;
			}
			this.targetHeight = height;
			this.pad.maxTowerHeight = height;
			while (this.targetHeight > this.towerAnimControllers.Count)
			{
				this.AddTowerRow();
			}
			if (this.activeAnimationRoutine != null)
			{
				this.pad.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = this.pad.StartCoroutine(this.TowerRoutine());
		}

		// Token: 0x0600AFF7 RID: 45047 RVA: 0x003D8256 File Offset: 0x003D6456
		private IEnumerator TowerRoutine()
		{
			while (this.currentHeight < this.targetHeight)
			{
				LaunchPad.LaunchPadTower.<>c__DisplayClass15_0 CS$<>8__locals1 = new LaunchPad.LaunchPadTower.<>c__DisplayClass15_0();
				CS$<>8__locals1.animComplete = false;
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on_pre, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].onAnimComplete += delegate(HashedString arg)
				{
					CS$<>8__locals1.animComplete = true;
				};
				float delay = 0.25f;
				while (!CS$<>8__locals1.animComplete && delay > 0f)
				{
					delay -= Time.deltaTime;
					yield return 0;
				}
				this.currentHeight++;
				CS$<>8__locals1 = null;
			}
			while (this.currentHeight > this.targetHeight)
			{
				LaunchPad.LaunchPadTower.<>c__DisplayClass15_1 CS$<>8__locals2 = new LaunchPad.LaunchPadTower.<>c__DisplayClass15_1();
				this.currentHeight--;
				CS$<>8__locals2.animComplete = false;
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off_pre, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].onAnimComplete += delegate(HashedString arg)
				{
					CS$<>8__locals2.animComplete = true;
				};
				float delay = 0.25f;
				while (!CS$<>8__locals2.animComplete && delay > 0f)
				{
					delay -= Time.deltaTime;
					yield return 0;
				}
				CS$<>8__locals2 = null;
			}
			yield return 0;
			yield break;
		}

		// Token: 0x04008A44 RID: 35396
		private LaunchPad pad;

		// Token: 0x04008A45 RID: 35397
		private KAnimLink animLink;

		// Token: 0x04008A46 RID: 35398
		private Coroutine activeAnimationRoutine;

		// Token: 0x04008A47 RID: 35399
		private string[] towerBGAnimNames = new string[]
		{
			"A1",
			"A2",
			"A3",
			"B",
			"C",
			"D",
			"E1",
			"E2",
			"F1",
			"F2"
		};

		// Token: 0x04008A48 RID: 35400
		private string towerBGAnimSuffix_on = "_on";

		// Token: 0x04008A49 RID: 35401
		private string towerBGAnimSuffix_on_pre = "_on_pre";

		// Token: 0x04008A4A RID: 35402
		private string towerBGAnimSuffix_off_pre = "_off_pre";

		// Token: 0x04008A4B RID: 35403
		private string towerBGAnimSuffix_off = "_off";

		// Token: 0x04008A4C RID: 35404
		private List<KBatchedAnimController> towerAnimControllers = new List<KBatchedAnimController>();

		// Token: 0x04008A4D RID: 35405
		private int targetHeight;

		// Token: 0x04008A4E RID: 35406
		private int currentHeight;
	}
}
