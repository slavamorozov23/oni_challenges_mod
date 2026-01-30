using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005AE RID: 1454
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Constructable")]
public class Constructable : Workable, ISaveLoadable
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06002148 RID: 8520 RVA: 0x000C0736 File Offset: 0x000BE936
	public Recipe Recipe
	{
		get
		{
			return this.building.Def.CraftRecipe;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06002149 RID: 8521 RVA: 0x000C0748 File Offset: 0x000BE948
	// (set) Token: 0x0600214A RID: 8522 RVA: 0x000C0750 File Offset: 0x000BE950
	public IList<Tag> SelectedElementsTags
	{
		get
		{
			return this.selectedElementsTags;
		}
		set
		{
			if (this.selectedElementsTags == null || this.selectedElementsTags.Length != value.Count)
			{
				this.selectedElementsTags = new Tag[value.Count];
			}
			value.CopyTo(this.selectedElementsTags, 0);
		}
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000C0788 File Offset: 0x000BE988
	public override string GetConversationTopic()
	{
		return this.building.Def.PrefabID;
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000C079C File Offset: 0x000BE99C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		float num = 0f;
		float num2 = 0f;
		bool flag = true;
		foreach (GameObject gameObject in this.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null))
				{
					num += component.Mass;
					num2 += component.Temperature * component.Mass;
					flag = (flag && component.HasTag(GameTags.Liquifiable));
				}
			}
		}
		if (num <= 0f)
		{
			DebugUtil.LogWarningArgs(base.gameObject, new object[]
			{
				"uhhh this constructable is about to generate a nan",
				"Item Count: ",
				this.storage.items.Count
			});
			return;
		}
		if (flag)
		{
			this.initialTemperature = Mathf.Min(num2 / num, 318.15f);
		}
		else
		{
			this.initialTemperature = Mathf.Clamp(num2 / num, 0f, 318.15f);
		}
		KAnimGraphTileVisualizer component2 = base.GetComponent<KAnimGraphTileVisualizer>();
		UtilityConnections connections = (component2 == null) ? ((UtilityConnections)0) : component2.Connections;
		bool flag2 = true;
		if (this.IsReplacementTile)
		{
			int cell = Grid.PosToCell(base.transform.GetLocalPosition());
			GameObject replacementCandidate = this.building.Def.GetReplacementCandidate(cell);
			if (replacementCandidate != null)
			{
				flag2 = false;
				SimCellOccupier component3 = replacementCandidate.GetComponent<SimCellOccupier>();
				if (component3 != null)
				{
					component3.DestroySelf(delegate
					{
						if (this != null && this.gameObject != null)
						{
							this.FinishConstruction(connections, worker);
						}
					});
				}
				else
				{
					Conduit component4 = replacementCandidate.GetComponent<Conduit>();
					if (component4 != null)
					{
						component4.GetFlowManager().MarkForReplacement(cell);
					}
					BuildingComplete component5 = replacementCandidate.GetComponent<BuildingComplete>();
					if (component5 != null)
					{
						component5.Subscribe(-21016276, delegate(object data)
						{
							this.FinishConstruction(connections, worker);
						});
					}
					else
					{
						global::Debug.LogWarning("Why am I trying to replace a: " + replacementCandidate.name);
						this.FinishConstruction(connections, worker);
					}
				}
				KAnimGraphTileVisualizer component6 = replacementCandidate.GetComponent<KAnimGraphTileVisualizer>();
				if (component6 != null)
				{
					component6.skipCleanup = true;
				}
				Deconstructable component7 = replacementCandidate.GetComponent<Deconstructable>();
				if (component7 != null)
				{
					component7.SpawnItemsFromConstruction(worker);
				}
				Boxed<Constructable.ReplaceCallbackParameters> boxed = Boxed<Constructable.ReplaceCallbackParameters>.Get(new Constructable.ReplaceCallbackParameters
				{
					TileLayer = this.building.Def.TileLayer,
					Worker = worker
				});
				replacementCandidate.Trigger(1606648047, boxed);
				Boxed<Constructable.ReplaceCallbackParameters>.Release(boxed);
				replacementCandidate.DeleteObject();
			}
		}
		if (flag2)
		{
			this.FinishConstruction(connections, worker);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, base.GetComponent<KSelectable>().GetName(), base.transform, 1.5f, false);
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000C0AA4 File Offset: 0x000BECA4
	private void FinishConstruction(UtilityConnections connections, WorkerBase workerForGameplayEvent)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		Orientation orientation = (component != null) ? component.GetOrientation() : Orientation.Neutral;
		int cell = Grid.PosToCell(base.transform.GetLocalPosition());
		this.UnmarkArea();
		GameObject gameObject = this.building.Def.Build(cell, orientation, this.storage, this.selectedElementsTags, this.initialTemperature, base.GetComponent<BuildingFacade>().CurrentFacade, true, GameClock.Instance.GetTime());
		BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
		gameplayEventData.building = gameObject.GetComponent<BuildingComplete>();
		gameplayEventData.workable = this;
		gameplayEventData.worker = workerForGameplayEvent;
		gameplayEventData.eventTrigger = GameHashes.NewBuilding;
		GameplayEventManager.Instance.Trigger(-1661515756, gameplayEventData);
		gameObject.transform.rotation = base.transform.rotation;
		Rotatable component2 = gameObject.GetComponent<Rotatable>();
		if (component2 != null)
		{
			component2.SetOrientation(orientation);
		}
		KAnimGraphTileVisualizer component3 = base.GetComponent<KAnimGraphTileVisualizer>();
		if (component3 != null)
		{
			gameObject.GetComponent<KAnimGraphTileVisualizer>().Connections = connections;
			component3.skipCleanup = true;
		}
		KSelectable component4 = base.GetComponent<KSelectable>();
		if (component4 != null && component4.IsSelected && gameObject.GetComponent<KSelectable>() != null)
		{
			component4.Unselect();
			if (PlayerController.Instance.ActiveTool.name == "SelectTool")
			{
				((SelectTool)PlayerController.Instance.ActiveTool).SelectNextFrame(gameObject.GetComponent<KSelectable>(), false);
			}
		}
		gameObject.Trigger(2121280625, this);
		this.storage.ConsumeAllIgnoringDisease();
		this.finished = true;
		this.DeleteObject();
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x000C0C40 File Offset: 0x000BEE40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.invalidLocation = new Notification(MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		this.faceTargetWhenWorking = true;
		base.Subscribe<Constructable>(-1432940121, Constructable.OnReachableChangedDelegate);
		if (this.rotatable == null)
		{
			this.MarkArea();
		}
		if (Db.Get().TechItems.GetTechTierForItem(this.building.Def.PrefabID) > 2)
		{
			this.requireMinionToWork = true;
		}
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Building;
		this.workingStatusItem = null;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		Prioritizable.AddRef(base.gameObject);
		this.synchronizeAnims = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000C0D98 File Offset: 0x000BEF98
	protected override void OnSpawn()
	{
		base.OnSpawn();
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		if (this.building.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
		}
		CellOffset[] array = this.building.Def.PlacementOffsets;
		if (this.rotatable != null)
		{
			array = new CellOffset[this.building.Def.PlacementOffsets.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.rotatable.GetRotatedCellOffset(this.building.Def.PlacementOffsets[i]);
			}
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(array, table, this.building.Def.ConstructionOffsetFilter);
		base.SetOffsetTable(offsetTable);
		this.storage.SetOffsetTable(offsetTable);
		base.Subscribe<Constructable>(2127324410, Constructable.OnCancelDelegate);
		if (this.rotatable != null)
		{
			this.MarkArea();
		}
		this.fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.BuildFetch);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Element element = ElementLoader.GetElement(this.SelectedElementsTags[0]);
		global::Debug.Assert(element != null, "Missing primary element for Constructable");
		component.ElementID = element.id;
		float b = component.Element.highTemp - 10f;
		component.Temperature = (component.Temperature = Mathf.Min(this.building.Def.Temperature, b));
		foreach (Recipe.Ingredient ingredient in this.Recipe.GetAllIngredients(this.selectedElementsTags))
		{
			this.fetchList.Add(ingredient.tag, null, ingredient.amount, Operational.State.None);
			MaterialNeeds.UpdateNeed(ingredient.tag, ingredient.amount, base.gameObject.GetMyWorldId());
		}
		if (!this.building.Def.IsTilePiece)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Construction");
		}
		this.building.RunOnArea(delegate(int offset_cell)
		{
			if (base.gameObject.GetComponent<ConduitBridge>() == null)
			{
				GameObject gameObject2 = Grid.Objects[offset_cell, 7];
				if (gameObject2 != null)
				{
					gameObject2.DeleteObject();
				}
			}
		});
		if (this.IsReplacementTile && this.building.Def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			GameObject x = Grid.Objects[cell, (int)this.building.Def.ReplacementLayer];
			if (x == null || x == base.gameObject)
			{
				Grid.Objects[cell, (int)this.building.Def.ReplacementLayer] = base.gameObject;
				if (base.gameObject.GetComponent<SimCellOccupier>() != null)
				{
					int renderLayer = LayerMask.NameToLayer("Overlay");
					World.Instance.blockTileRenderer.AddBlock(renderLayer, this.building.Def, this.IsReplacementTile, SimHashes.Void, cell);
				}
				TileVisualizer.RefreshCell(cell, this.building.Def.TileLayer, this.building.Def.ReplacementLayer);
			}
			else
			{
				global::Debug.LogError("multiple replacement tiles on the same cell!");
				Util.KDestroyGameObject(base.gameObject);
			}
			GameObject gameObject = Grid.Objects[cell, (int)this.building.Def.ObjectLayer];
			if (gameObject != null)
			{
				Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
				if (component2 != null)
				{
					component2.CancelDeconstruction();
				}
			}
		}
		bool flag = this.building.Def.BuildingComplete.GetComponent<Ladder>();
		this.waitForFetchesBeforeDigging = (flag || this.building.Def.BuildingComplete.GetComponent<SimCellOccupier>() || this.building.Def.BuildingComplete.GetComponent<Door>() || this.building.Def.BuildingComplete.GetComponent<LiquidPumpingStation>());
		if (flag)
		{
			int x2 = 0;
			int num = 0;
			Grid.CellToXY(Grid.PosToCell(this), out x2, out num);
			int y = num - 3;
			this.ladderDetectionExtents = new Extents(x2, y, 1, 5);
			this.ladderPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnNearbyBuildingLayerChanged", base.gameObject, this.ladderDetectionExtents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnNearbyBuildingLayerChanged));
			this.OnNearbyBuildingLayerChanged(null);
		}
		this.fetchList.Submit(new System.Action(this.OnFetchListComplete), true);
		this.PlaceDiggables();
		new ReachabilityMonitor.Instance(this).StartSM();
		base.Subscribe<Constructable>(493375141, Constructable.OnRefreshUserMenuDelegate);
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		Prioritizable prioritizable = component3;
		prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		this.OnPriorityChanged(component3.GetMasterPriority());
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000C1274 File Offset: 0x000BF474
	private void OnPriorityChanged(PrioritySetting priority)
	{
		this.building.RunOnArea(delegate(int cell)
		{
			Diggable diggable = Diggable.GetDiggable(cell);
			if (diggable != null)
			{
				diggable.GetComponent<Prioritizable>().SetMasterPriority(priority);
			}
		});
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x000C12A8 File Offset: 0x000BF4A8
	private void MarkArea()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		BuildingDef def = this.building.Def;
		Orientation orientation = this.building.Orientation;
		ObjectLayer layer = this.IsReplacementTile ? def.ReplacementLayer : def.ObjectLayer;
		def.MarkArea(num, orientation, layer, base.gameObject);
		if (def.IsTilePiece)
		{
			if (Grid.Objects[num, (int)def.TileLayer] == null)
			{
				def.MarkArea(num, orientation, def.TileLayer, base.gameObject);
				def.RunOnArea(num, orientation, delegate(int c)
				{
					TileVisualizer.RefreshCell(c, def.TileLayer, def.ReplacementLayer);
				});
			}
			Grid.IsTileUnderConstruction[num] = true;
		}
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x000C138C File Offset: 0x000BF58C
	private void UnmarkArea()
	{
		if (this.unmarked)
		{
			return;
		}
		this.unmarked = true;
		int num = Grid.PosToCell(base.transform.GetPosition());
		BuildingDef def = this.building.Def;
		ObjectLayer layer = this.IsReplacementTile ? this.building.Def.ReplacementLayer : this.building.Def.ObjectLayer;
		def.UnmarkArea(num, this.building.Orientation, layer, base.gameObject);
		if (def.IsTilePiece)
		{
			Grid.IsTileUnderConstruction[num] = false;
		}
		this.ClearPendingUproots();
	}

	// Token: 0x06002153 RID: 8531 RVA: 0x000C1424 File Offset: 0x000BF624
	private void ClearPendingUproots()
	{
		foreach (Uprootable uprootable in this.pendingUproots)
		{
			if (!uprootable.IsNullOrDestroyed())
			{
				uprootable.Unsubscribe(-216549700, new Action<object>(this.OnSolidChangedOrDigDestroyed));
				uprootable.Unsubscribe(1198393204, new Action<object>(this.OnSolidChangedOrDigDestroyed));
				uprootable.ForceCancelUproot(null);
			}
		}
		this.pendingUproots.Clear();
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000C14B8 File Offset: 0x000BF6B8
	private void OnNearbyBuildingLayerChanged(object data)
	{
		this.hasLadderNearby = false;
		for (int i = this.ladderDetectionExtents.y; i < this.ladderDetectionExtents.y + this.ladderDetectionExtents.height; i++)
		{
			int num = Grid.OffsetCell(0, this.ladderDetectionExtents.x, i);
			if (Grid.IsValidCell(num))
			{
				GameObject gameObject = null;
				Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
				if (gameObject != null && gameObject.GetComponent<Ladder>() != null)
				{
					this.hasLadderNearby = true;
					return;
				}
			}
		}
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000C1544 File Offset: 0x000BF744
	private bool IsWire()
	{
		return this.building.Def.name.Contains("Wire");
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x000C1560 File Offset: 0x000BF760
	public bool IconConnectionAnimation(float delay, int connectionCount, string defName, string soundName)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (this.building.Def.Name.Contains(defName))
		{
			Building building = null;
			GameObject gameObject = Grid.Objects[num, 1];
			if (gameObject != null)
			{
				building = gameObject.GetComponent<Building>();
			}
			if (building != null)
			{
				bool flag = this.IsWire();
				int num2 = flag ? building.GetPowerInputCell() : building.GetUtilityInputCell();
				int num3 = flag ? num2 : building.GetUtilityOutputCell();
				if (num == num2 || num == num3)
				{
					BuildingCellVisualizer component = building.gameObject.GetComponent<BuildingCellVisualizer>();
					if (component != null && (flag ? ((component.addedPorts & (EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut)) > (EntityCellVisualizer.Ports)0) : ((component.addedPorts & (EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut | EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut | EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut)) > (EntityCellVisualizer.Ports)0)))
					{
						component.ConnectedEventWithDelay(delay, connectionCount, num, soundName);
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000C1640 File Offset: 0x000BF840
	protected override void OnCleanUp()
	{
		if (this.IsReplacementTile && this.building.Def.isKAnimTile)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			GameObject gameObject = Grid.Objects[cell, (int)this.building.Def.ReplacementLayer];
			if (gameObject == base.gameObject && gameObject.GetComponent<SimCellOccupier>() != null)
			{
				World.Instance.blockTileRenderer.RemoveBlock(this.building.Def, this.IsReplacementTile, SimHashes.Void, cell);
			}
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.digPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.ladderPartitionerEntry);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (component != null)
		{
			SaveLoader.Instance.saveManager.Unregister(component);
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Constructable destroyed");
		}
		this.UnmarkArea();
		HashSetPool<Uprootable, Constructable>.PooledHashSet pooledHashSet = HashSetPool<Uprootable, Constructable>.Allocate();
		foreach (int cell2 in this.building.PlacementCells)
		{
			Diggable diggable = Diggable.GetDiggable(cell2);
			if (diggable != null)
			{
				diggable.gameObject.DeleteObject();
			}
			Constructable.<OnCleanUp>g__TryAddUprootable|48_0(Grid.Objects[cell2, 1], pooledHashSet);
			Constructable.<OnCleanUp>g__TryAddUprootable|48_0(Grid.Objects[cell2, 5], pooledHashSet);
		}
		foreach (Uprootable uprootable in pooledHashSet)
		{
			uprootable.Unsubscribe(-216549700, new Action<object>(this.OnSolidChangedOrDigDestroyed));
			uprootable.Unsubscribe(1198393204, new Action<object>(this.OnSolidChangedOrDigDestroyed));
			uprootable.ForceCancelUproot(null);
		}
		pooledHashSet.Recycle();
		base.OnCleanUp();
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000C1834 File Offset: 0x000BFA34
	private void OnDiggableReachabilityChanged(object _)
	{
		if (!this.IsReplacementTile)
		{
			int diggable_count = 0;
			int unreachable_count = 0;
			this.building.RunOnArea(delegate(int offset_cell)
			{
				Diggable diggable = Diggable.GetDiggable(offset_cell);
				if (diggable != null && diggable.isActiveAndEnabled)
				{
					int num = diggable_count + 1;
					diggable_count = num;
					if (!diggable.GetComponent<KPrefabID>().HasTag(GameTags.Reachable))
					{
						num = unreachable_count + 1;
						unreachable_count = num;
					}
				}
			});
			bool flag = unreachable_count > 0 && unreachable_count == diggable_count;
			if (flag != this.hasUnreachableDigs)
			{
				if (flag)
				{
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable, null);
				}
				else
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable, false);
				}
				this.hasUnreachableDigs = flag;
			}
		}
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000C18E0 File Offset: 0x000BFAE0
	private void PlaceDiggables()
	{
		if (this.waitForFetchesBeforeDigging && this.fetchList != null && !this.hasLadderNearby)
		{
			this.OnDiggableReachabilityChanged(null);
			return;
		}
		if (!this.solidPartitionerEntry.IsValid())
		{
			Extents validPlacementExtents = this.building.GetValidPlacementExtents();
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.PlaceDiggables", base.gameObject, validPlacementExtents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChangedOrDigDestroyed));
			this.digPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.PlaceDiggables", base.gameObject, validPlacementExtents, GameScenePartitioner.Instance.digDestroyedLayer, new Action<object>(this.OnSolidChangedOrDigDestroyed));
		}
		bool digs_complete = true;
		if (!this.IsReplacementTile)
		{
			PrioritySetting master_priority = base.GetComponent<Prioritizable>().GetMasterPriority();
			HashSetPool<Uprootable, Constructable>.PooledHashSet uprootables = HashSetPool<Uprootable, Constructable>.Allocate();
			this.building.RunOnArea(delegate(int offset_cell)
			{
				Uprootable item2;
				if (Diggable.IsDiggable(offset_cell))
				{
					digs_complete = false;
					Diggable diggable = Diggable.GetDiggable(offset_cell);
					if (diggable != null && !diggable.isActiveAndEnabled)
					{
						diggable.Unsubscribe(-1432940121, new Action<object>(this.OnDiggableReachabilityChanged));
						diggable = null;
					}
					if (diggable == null)
					{
						diggable = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), Grid.SceneLayer.Move, null, 0).GetComponent<Diggable>();
						diggable.choreTypeIdHash = Db.Get().ChoreTypes.BuildDig.IdHash;
						diggable.gameObject.SetActive(true);
						diggable.transform.SetPosition(Grid.CellToPosCBC(offset_cell, Grid.SceneLayer.Move));
						Grid.Objects[offset_cell, 7] = diggable.gameObject;
						diggable.Subscribe(-1432940121, new Action<object>(this.OnDiggableReachabilityChanged));
					}
					diggable.GetComponent<Prioritizable>().SetMasterPriority(master_priority);
					RenderUtil.EnableRenderer(diggable.transform, false);
					SaveLoadRoot component = diggable.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
						return;
					}
				}
				else if (this.building.Def.ObjectLayer == ObjectLayer.Building && Uprootable.CanUproot(Grid.Objects[offset_cell, 5], out item2))
				{
					digs_complete = false;
					uprootables.Add(item2);
				}
			});
			ListPool<Uprootable, Constructable>.PooledList pooledList = ListPool<Uprootable, Constructable>.Allocate();
			foreach (Uprootable uprootable in this.pendingUproots)
			{
				if (uprootable.IsNullOrDestroyed())
				{
					pooledList.Add(uprootable);
				}
				else if (!uprootables.Contains(uprootable))
				{
					uprootable.Unsubscribe(-216549700, new Action<object>(this.OnSolidChangedOrDigDestroyed));
					uprootable.Unsubscribe(1198393204, new Action<object>(this.OnSolidChangedOrDigDestroyed));
					pooledList.Add(uprootable);
				}
			}
			foreach (Uprootable item in pooledList)
			{
				this.pendingUproots.Remove(item);
			}
			pooledList.Recycle();
			foreach (Uprootable uprootable2 in uprootables)
			{
				bool flag = this.pendingUproots.Add(uprootable2);
				uprootable2.choreTypeIdHash = Db.Get().ChoreTypes.BuildUproot.IdHash;
				uprootable2.MarkForUproot(true);
				if (flag)
				{
					uprootable2.Subscribe(-216549700, new Action<object>(this.OnSolidChangedOrDigDestroyed));
					uprootable2.Subscribe(1198393204, new Action<object>(this.OnSolidChangedOrDigDestroyed));
				}
			}
			uprootables.Recycle();
			this.OnDiggableReachabilityChanged(null);
		}
		bool flag2 = this.building.Def.IsValidBuildLocation(base.gameObject, base.transform.GetPosition(), this.building.Orientation, this.IsReplacementTile);
		if (flag2)
		{
			this.notifier.Remove(this.invalidLocation);
		}
		else
		{
			this.notifier.Add(this.invalidLocation, "");
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidBuildingLocation, !flag2, this);
		bool flag3 = digs_complete && flag2 && this.fetchList == null;
		if (flag3 && this.buildChore == null)
		{
			this.buildChore = new WorkChore<Constructable>(Db.Get().ChoreTypes.Build, this, null, true, new Action<Chore>(this.UpdateBuildState), new Action<Chore>(this.UpdateBuildState), new Action<Chore>(this.UpdateBuildState), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.UpdateBuildState(this.buildChore);
			return;
		}
		if (!flag3 && this.buildChore != null)
		{
			this.buildChore.Cancel("Need to dig");
			this.buildChore = null;
		}
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x000C1CA0 File Offset: 0x000BFEA0
	private void OnFetchListComplete()
	{
		this.fetchList = null;
		this.PlaceDiggables();
		this.ClearMaterialNeeds();
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000C1CB8 File Offset: 0x000BFEB8
	private void ClearMaterialNeeds()
	{
		if (this.materialNeedsCleared)
		{
			return;
		}
		foreach (Recipe.Ingredient ingredient in this.Recipe.GetAllIngredients(this.SelectedElementsTags))
		{
			MaterialNeeds.UpdateNeed(ingredient.tag, -ingredient.amount, base.gameObject.GetMyWorldId());
		}
		this.materialNeedsCleared = true;
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000C1D16 File Offset: 0x000BFF16
	private void OnSolidChangedOrDigDestroyed(object data)
	{
		if (this == null || this.finished)
		{
			return;
		}
		this.PlaceDiggables();
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x000C1D30 File Offset: 0x000BFF30
	private void UpdateBuildState(Chore chore)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		StatusItem status_item = chore.InProgress() ? Db.Get().BuildingStatusItems.UnderConstruction : Db.Get().BuildingStatusItems.UnderConstructionNoWorker;
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, status_item, null);
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x000C1D84 File Offset: 0x000BFF84
	[OnDeserialized]
	internal void OnDeserialized()
	{
		if (this.ids != null)
		{
			this.selectedElements = new Element[this.ids.Length];
			for (int i = 0; i < this.ids.Length; i++)
			{
				this.selectedElements[i] = ElementLoader.FindElementByHash((SimHashes)this.ids[i]);
			}
			if (this.selectedElementsTags == null)
			{
				this.selectedElementsTags = new Tag[this.ids.Length];
				for (int j = 0; j < this.ids.Length; j++)
				{
					this.selectedElementsTags[j] = ElementLoader.FindElementByHash((SimHashes)this.ids[j]).tag;
				}
			}
			global::Debug.Assert(this.selectedElements.Length == this.selectedElementsTags.Length);
			for (int k = 0; k < this.selectedElements.Length; k++)
			{
				global::Debug.Assert(this.selectedElements[k].tag == this.SelectedElementsTags[k]);
			}
		}
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x000C1E70 File Offset: 0x000C0070
	private void OnReachableChanged(object data)
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (((Boxed<bool>)data).value)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable, false);
			if (component != null)
			{
				component.TintColour = Game.Instance.uiColours.Build.validLocation;
				return;
			}
		}
		else
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable, this);
			if (component != null)
			{
				component.TintColour = Game.Instance.uiColours.Build.unreachable;
			}
		}
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000C1F1C File Offset: 0x000C011C
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_cancel", UI.USERMENUACTIONS.CANCELCONSTRUCTION.NAME, new System.Action(this.OnPressCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCONSTRUCTION.TOOLTIP, true), 1f);
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x000C1F76 File Offset: 0x000C0176
	private void OnPressCancel()
	{
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000C1F89 File Offset: 0x000C0189
	private void OnCancel(object _ = null)
	{
		DetailsScreen.Instance.Show(false);
		this.ClearMaterialNeeds();
		this.ClearPendingUproots();
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000C2074 File Offset: 0x000C0274
	[CompilerGenerated]
	internal static void <OnCleanUp>g__TryAddUprootable|48_0(GameObject plant, HashSet<Uprootable> _uprootables)
	{
		if (plant == null)
		{
			return;
		}
		Uprootable component = plant.GetComponent<Uprootable>();
		if (component == null)
		{
			return;
		}
		_uprootables.Add(component);
	}

	// Token: 0x04001359 RID: 4953
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400135A RID: 4954
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x0400135B RID: 4955
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x0400135C RID: 4956
	[MyCmpReq]
	private Building building;

	// Token: 0x0400135D RID: 4957
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400135E RID: 4958
	private Notification invalidLocation;

	// Token: 0x0400135F RID: 4959
	private float initialTemperature = -1f;

	// Token: 0x04001360 RID: 4960
	[Serialize]
	private bool isPrioritized;

	// Token: 0x04001361 RID: 4961
	private FetchList2 fetchList;

	// Token: 0x04001362 RID: 4962
	private Chore buildChore;

	// Token: 0x04001363 RID: 4963
	private bool materialNeedsCleared;

	// Token: 0x04001364 RID: 4964
	private bool hasUnreachableDigs;

	// Token: 0x04001365 RID: 4965
	private bool finished;

	// Token: 0x04001366 RID: 4966
	private bool unmarked;

	// Token: 0x04001367 RID: 4967
	public bool isDiggingRequired = true;

	// Token: 0x04001368 RID: 4968
	private bool waitForFetchesBeforeDigging;

	// Token: 0x04001369 RID: 4969
	private bool hasLadderNearby;

	// Token: 0x0400136A RID: 4970
	private Extents ladderDetectionExtents;

	// Token: 0x0400136B RID: 4971
	[Serialize]
	public bool IsReplacementTile;

	// Token: 0x0400136C RID: 4972
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x0400136D RID: 4973
	private HandleVector<int>.Handle digPartitionerEntry;

	// Token: 0x0400136E RID: 4974
	private HandleVector<int>.Handle ladderPartitionerEntry;

	// Token: 0x0400136F RID: 4975
	private readonly HashSet<Uprootable> pendingUproots = new HashSet<Uprootable>();

	// Token: 0x04001370 RID: 4976
	private LoggerFSS log = new LoggerFSS("Constructable", 35);

	// Token: 0x04001371 RID: 4977
	[Serialize]
	private Tag[] selectedElementsTags;

	// Token: 0x04001372 RID: 4978
	private Element[] selectedElements;

	// Token: 0x04001373 RID: 4979
	[Serialize]
	private int[] ids;

	// Token: 0x04001374 RID: 4980
	private static readonly EventSystem.IntraObjectHandler<Constructable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04001375 RID: 4981
	private static readonly EventSystem.IntraObjectHandler<Constructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04001376 RID: 4982
	private static readonly EventSystem.IntraObjectHandler<Constructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02001433 RID: 5171
	public struct ReplaceCallbackParameters
	{
		// Token: 0x04006DD4 RID: 28116
		public ObjectLayer TileLayer;

		// Token: 0x04006DD5 RID: 28117
		public WorkerBase Worker;
	}
}
