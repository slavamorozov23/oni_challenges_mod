using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B2E RID: 2862
[AddComponentMenu("KMonoBehaviour/scripts/GameScenePartitioner")]
public class GameScenePartitioner : KMonoBehaviour
{
	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06005421 RID: 21537 RVA: 0x001EB691 File Offset: 0x001E9891
	public static GameScenePartitioner Instance
	{
		get
		{
			global::Debug.Assert(GameScenePartitioner.instance != null);
			return GameScenePartitioner.instance;
		}
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x001EB6A8 File Offset: 0x001E98A8
	public bool Lookup(HandleVector<int>.Handle handle, out ScenePartitionerEntry entry)
	{
		if (!this.scenePartitionerEntries.IsValid(handle) || !this.scenePartitionerEntries.IsVersionValid(handle))
		{
			entry = null;
			return false;
		}
		entry = this.scenePartitionerEntries.GetData(handle);
		return true;
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x001EB6DC File Offset: 0x001E98DC
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GameScenePartitioner.instance == null);
		GameScenePartitioner.instance = this;
		this.partitioner = new ScenePartitioner(16, 67, Grid.WidthInCells, Grid.HeightInCells);
		this.solidChangedLayer = this.partitioner.CreateMask("SolidChanged");
		this.liquidChangedLayer = this.partitioner.CreateMask("LiquidChanged");
		this.digDestroyedLayer = this.partitioner.CreateMask("DigDestroyed");
		this.fogOfWarChangedLayer = this.partitioner.CreateMask("FogOfWarChanged");
		this.decorProviderLayer = this.partitioner.CreateMask("DecorProviders");
		this.attackableEntitiesLayer = this.partitioner.CreateMask("FactionedEntities");
		this.fetchChoreLayer = this.partitioner.CreateMask("FetchChores");
		this.pickupablesLayer = this.partitioner.CreateMask("Pickupables");
		this.storedPickupablesLayer = this.partitioner.CreateMask("StoredPickupables");
		this.pickupablesChangedLayer = this.partitioner.CreateMask("PickupablesChanged");
		this.plantsChangedLayer = this.partitioner.CreateMask("PlantsChanged");
		this.gasConduitsLayer = this.partitioner.CreateMask("GasConduit");
		this.liquidConduitsLayer = this.partitioner.CreateMask("LiquidConduit");
		this.solidConduitsLayer = this.partitioner.CreateMask("SolidConduit");
		this.noisePolluterLayer = this.partitioner.CreateMask("NoisePolluters");
		this.validNavCellChangedLayer = this.partitioner.CreateMask("validNavCellChangedLayer");
		this.dirtyNavCellUpdateLayer = this.partitioner.CreateMask("dirtyNavCellUpdateLayer");
		this.trapsLayer = this.partitioner.CreateMask("trapsLayer");
		this.floorSwitchActivatorLayer = this.partitioner.CreateMask("FloorSwitchActivatorLayer");
		this.floorSwitchActivatorChangedLayer = this.partitioner.CreateMask("FloorSwitchActivatorChangedLayer");
		this.collisionLayer = this.partitioner.CreateMask("Collision");
		this.lure = this.partitioner.CreateMask("Lure");
		this.plants = this.partitioner.CreateMask("Plants");
		this.industrialBuildings = this.partitioner.CreateMask("IndustrialBuildings");
		this.completeBuildings = this.partitioner.CreateMask("CompleteBuildings");
		this.prioritizableObjects = this.partitioner.CreateMask("PrioritizableObjects");
		this.contactConductiveLayer = this.partitioner.CreateMask("ContactConductiveLayer");
		this.objectLayers = new ScenePartitionerLayer[45];
		for (int i = 0; i < 45; i++)
		{
			ObjectLayer objectLayer = (ObjectLayer)i;
			this.objectLayers[i] = this.partitioner.CreateMask(objectLayer.ToString());
		}
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001EB9A8 File Offset: 0x001E9BA8
	protected override void OnForcedCleanUp()
	{
		GameScenePartitioner.instance = null;
		this.partitioner.FreeResources();
		this.partitioner = null;
		this.solidChangedLayer = null;
		this.liquidChangedLayer = null;
		this.digDestroyedLayer = null;
		this.fogOfWarChangedLayer = null;
		this.decorProviderLayer = null;
		this.attackableEntitiesLayer = null;
		this.fetchChoreLayer = null;
		this.pickupablesLayer = null;
		this.storedPickupablesLayer = null;
		this.plantsChangedLayer = null;
		this.pickupablesChangedLayer = null;
		this.gasConduitsLayer = null;
		this.liquidConduitsLayer = null;
		this.solidConduitsLayer = null;
		this.noisePolluterLayer = null;
		this.validNavCellChangedLayer = null;
		this.dirtyNavCellUpdateLayer = null;
		this.trapsLayer = null;
		this.floorSwitchActivatorLayer = null;
		this.floorSwitchActivatorChangedLayer = null;
		this.contactConductiveLayer = null;
		this.objectLayers = null;
		this.scenePartitionerEntries.Clear();
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x001EBA74 File Offset: 0x001E9C74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		navGrid.OnNavGridUpdateComplete = (Action<List<int>>)Delegate.Combine(navGrid.OnNavGridUpdateComplete, new Action<List<int>>(this.OnNavGridUpdateComplete));
		NavTable navTable = navGrid.NavTable;
		navTable.OnValidCellChanged = (Action<int, NavType>)Delegate.Combine(navTable.OnValidCellChanged, new Action<int, NavType>(this.OnValidNavCellChanged));
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x001EBAE0 File Offset: 0x001E9CE0
	public HandleVector<int>.Handle Add(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		ScenePartitionerEntry scenePartitionerEntry = ScenePartitionerEntry.EntryPool.Get();
		scenePartitionerEntry.Init(name, obj, x, y, width, height, layer, this.partitioner, event_callback);
		HandleVector<int>.Handle handle = this.scenePartitionerEntries.Allocate(scenePartitionerEntry);
		this.partitioner.Add(handle);
		return handle;
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x001EBB2C File Offset: 0x001E9D2C
	public HandleVector<int>.Handle Add(string name, object obj, Extents extents, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		return this.Add(name, obj, extents.x, extents.y, extents.width, extents.height, layer, event_callback);
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x001EBB60 File Offset: 0x001E9D60
	public HandleVector<int>.Handle Add(string name, object obj, int cell, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return this.Add(name, obj, x, y, 1, 1, layer, event_callback);
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x001EBB8B File Offset: 0x001E9D8B
	public void AddGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Combine(layer.OnEvent, action);
	}

	// Token: 0x0600542A RID: 21546 RVA: 0x001EBBA4 File Offset: 0x001E9DA4
	public void RemoveGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Remove(layer.OnEvent, action);
	}

	// Token: 0x0600542B RID: 21547 RVA: 0x001EBBBD File Offset: 0x001E9DBD
	public void TriggerEvent(List<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(cells, layer, event_data);
	}

	// Token: 0x0600542C RID: 21548 RVA: 0x001EBBCD File Offset: 0x001E9DCD
	public void TriggerEvent(Extents extents, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(extents.x, extents.y, extents.width, extents.height, layer, event_data);
	}

	// Token: 0x0600542D RID: 21549 RVA: 0x001EBBF4 File Offset: 0x001E9DF4
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(x, y, width, height, layer, event_data);
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x001EBC0C File Offset: 0x001E9E0C
	public void TriggerEvent(int cell, ScenePartitionerLayer layer, object event_data)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		this.TriggerEvent(x, y, 1, 1, layer, event_data);
	}

	// Token: 0x0600542F RID: 21551 RVA: 0x001EBC33 File Offset: 0x001E9E33
	[Obsolete("use Visit pattern instead")]
	public void GatherEntries(Extents extents, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.GatherEntries(extents.x, extents.y, extents.width, extents.height, layer, gathered_entries);
	}

	// Token: 0x06005430 RID: 21552 RVA: 0x001EBC55 File Offset: 0x001E9E55
	public void GatherEntries(int x_bottomLeft, int y_bottomLeft, int width, int height, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.partitioner.GatherEntries(x_bottomLeft, y_bottomLeft, width, height, layer, null, gathered_entries);
	}

	// Token: 0x06005431 RID: 21553 RVA: 0x001EBC6C File Offset: 0x001E9E6C
	public void VisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, Func<object, ContextType, Util.IterationInstruction> visitor, ContextType context) where ContextType : class
	{
		this.partitioner.VisitEntries<ContextType>(x, y, width, height, layer, visitor, context);
	}

	// Token: 0x06005432 RID: 21554 RVA: 0x001EBC84 File Offset: 0x001E9E84
	public void VisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, GameScenePartitioner.VisitorRef<ContextType> visitor, ref ContextType context) where ContextType : struct
	{
		this.partitioner.VisitEntries<ContextType>(x, y, width, height, layer, visitor, ref context);
	}

	// Token: 0x06005433 RID: 21555 RVA: 0x001EBC9C File Offset: 0x001E9E9C
	public void ReadonlyVisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, Func<object, ContextType, Util.IterationInstruction> visitor, ContextType context) where ContextType : class
	{
		this.partitioner.ReadonlyVisitEntries<ContextType>(x, y, width, height, layer, visitor, context);
	}

	// Token: 0x06005434 RID: 21556 RVA: 0x001EBCB4 File Offset: 0x001E9EB4
	public void ReadonlyVisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, GameScenePartitioner.VisitorRef<ContextType> visitor, ref ContextType context) where ContextType : struct
	{
		this.partitioner.ReadonlyVisitEntries<ContextType>(x, y, width, height, layer, visitor, ref context);
	}

	// Token: 0x06005435 RID: 21557 RVA: 0x001EBCCC File Offset: 0x001E9ECC
	private void OnValidNavCellChanged(int cell, NavType nav_type)
	{
		this.changedCells.Add(cell);
	}

	// Token: 0x06005436 RID: 21558 RVA: 0x001EBCDC File Offset: 0x001E9EDC
	private void OnNavGridUpdateComplete(List<int> dirty_nav_cells)
	{
		GameScenePartitioner.Instance.TriggerEvent(dirty_nav_cells, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, null);
		if (this.changedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.validNavCellChangedLayer, null);
			this.changedCells.Clear();
		}
	}

	// Token: 0x06005437 RID: 21559 RVA: 0x001EBD34 File Offset: 0x001E9F34
	public void UpdatePosition(HandleVector<int>.Handle handle, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.UpdatePosition(handle, vector2I.x, vector2I.y);
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x001EBD5B File Offset: 0x001E9F5B
	public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(handle, x, y);
	}

	// Token: 0x06005439 RID: 21561 RVA: 0x001EBD7B File Offset: 0x001E9F7B
	public void UpdatePosition(HandleVector<int>.Handle handle, Extents ext)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(handle, ext);
	}

	// Token: 0x0600543A RID: 21562 RVA: 0x001EBD9C File Offset: 0x001E9F9C
	public void Free(ref HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		ScenePartitionerEntry data = this.scenePartitionerEntries.GetData(handle);
		data.Release(handle);
		this.scenePartitionerEntries.Free(handle);
		handle.Clear();
		ScenePartitionerEntry.EntryPool.Release(data);
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x001EBDF3 File Offset: 0x001E9FF3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.partitioner.Cleanup();
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x001EBE06 File Offset: 0x001EA006
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		return this.partitioner.DoDebugLayersContainItemsOnCell(cell);
	}

	// Token: 0x0600543D RID: 21565 RVA: 0x001EBE14 File Offset: 0x001EA014
	public List<ScenePartitionerLayer> GetLayers()
	{
		return this.partitioner.layers;
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001EBE21 File Offset: 0x001EA021
	public void SetToggledLayers(HashSet<ScenePartitionerLayer> toggled_layers)
	{
		this.partitioner.toggledLayers = toggled_layers;
	}

	// Token: 0x040038D3 RID: 14547
	public ScenePartitionerLayer solidChangedLayer;

	// Token: 0x040038D4 RID: 14548
	public ScenePartitionerLayer liquidChangedLayer;

	// Token: 0x040038D5 RID: 14549
	public ScenePartitionerLayer digDestroyedLayer;

	// Token: 0x040038D6 RID: 14550
	public ScenePartitionerLayer fogOfWarChangedLayer;

	// Token: 0x040038D7 RID: 14551
	public ScenePartitionerLayer decorProviderLayer;

	// Token: 0x040038D8 RID: 14552
	public ScenePartitionerLayer attackableEntitiesLayer;

	// Token: 0x040038D9 RID: 14553
	public ScenePartitionerLayer fetchChoreLayer;

	// Token: 0x040038DA RID: 14554
	public ScenePartitionerLayer pickupablesLayer;

	// Token: 0x040038DB RID: 14555
	public ScenePartitionerLayer storedPickupablesLayer;

	// Token: 0x040038DC RID: 14556
	public ScenePartitionerLayer pickupablesChangedLayer;

	// Token: 0x040038DD RID: 14557
	public ScenePartitionerLayer gasConduitsLayer;

	// Token: 0x040038DE RID: 14558
	public ScenePartitionerLayer liquidConduitsLayer;

	// Token: 0x040038DF RID: 14559
	public ScenePartitionerLayer solidConduitsLayer;

	// Token: 0x040038E0 RID: 14560
	public ScenePartitionerLayer wiresLayer;

	// Token: 0x040038E1 RID: 14561
	public ScenePartitionerLayer[] objectLayers;

	// Token: 0x040038E2 RID: 14562
	public ScenePartitionerLayer noisePolluterLayer;

	// Token: 0x040038E3 RID: 14563
	public ScenePartitionerLayer validNavCellChangedLayer;

	// Token: 0x040038E4 RID: 14564
	public ScenePartitionerLayer dirtyNavCellUpdateLayer;

	// Token: 0x040038E5 RID: 14565
	public ScenePartitionerLayer trapsLayer;

	// Token: 0x040038E6 RID: 14566
	public ScenePartitionerLayer floorSwitchActivatorLayer;

	// Token: 0x040038E7 RID: 14567
	public ScenePartitionerLayer floorSwitchActivatorChangedLayer;

	// Token: 0x040038E8 RID: 14568
	public ScenePartitionerLayer collisionLayer;

	// Token: 0x040038E9 RID: 14569
	public ScenePartitionerLayer lure;

	// Token: 0x040038EA RID: 14570
	public ScenePartitionerLayer plants;

	// Token: 0x040038EB RID: 14571
	public ScenePartitionerLayer plantsChangedLayer;

	// Token: 0x040038EC RID: 14572
	public ScenePartitionerLayer industrialBuildings;

	// Token: 0x040038ED RID: 14573
	public ScenePartitionerLayer completeBuildings;

	// Token: 0x040038EE RID: 14574
	public ScenePartitionerLayer prioritizableObjects;

	// Token: 0x040038EF RID: 14575
	public ScenePartitionerLayer contactConductiveLayer;

	// Token: 0x040038F0 RID: 14576
	private ScenePartitioner partitioner;

	// Token: 0x040038F1 RID: 14577
	private static GameScenePartitioner instance;

	// Token: 0x040038F2 RID: 14578
	private KCompactedVector<ScenePartitionerEntry> scenePartitionerEntries = new KCompactedVector<ScenePartitionerEntry>(0);

	// Token: 0x040038F3 RID: 14579
	private List<int> changedCells = new List<int>();

	// Token: 0x02001C98 RID: 7320
	// (Invoke) Token: 0x0600AE1A RID: 44570
	public delegate Util.IterationInstruction VisitorRef<ContextType>(object obj, ref ContextType context);
}
