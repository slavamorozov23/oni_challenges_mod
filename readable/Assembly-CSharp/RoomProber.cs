using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B20 RID: 2848
public class RoomProber : ISim1000ms
{
	// Token: 0x06005331 RID: 21297 RVA: 0x001E5034 File Offset: 0x001E3234
	public RoomProber()
	{
		CavityInfo cavityInfo = this.CreateNewCavity();
		cavityInfo.cells = new List<int>
		{
			Capacity = Grid.CellCount
		};
		for (int i = 0; i < Grid.CellCount; i++)
		{
			cavityInfo.cells.Add(i);
		}
		this.CellCavityID = new HandleVector<int>.Handle[Grid.CellCount];
		Array.Fill<HandleVector<int>.Handle>(this.CellCavityID, cavityInfo.handle);
		this.solidChanges.Add(0);
		this.refresh = new RoomProber.RefreshModule(this);
		this.refresh.Initialize();
		this.Refresh();
		Game instance = Game.Instance;
		instance.OnSpawnComplete = (System.Action)Delegate.Combine(instance.OnSpawnComplete, new System.Action(this.Refresh));
		World instance2 = World.Instance;
		instance2.OnSolidChanged = (Action<int>)Delegate.Combine(instance2.OnSolidChanged, new Action<int>(this.SolidChangedEvent));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingsChanged));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[2], new Action<int, object>(this.OnBuildingsChanged));
	}

	// Token: 0x06005332 RID: 21298 RVA: 0x001E5188 File Offset: 0x001E3388
	private void SolidChangedEvent(int cell)
	{
		this.SolidChangedEvent(cell, true);
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x001E5192 File Offset: 0x001E3392
	private void OnBuildingsChanged(int cell, object building)
	{
		if (this.GetCavityForCell(cell) != null)
		{
			this.solidChanges.Add(cell);
			this.dirty = true;
		}
	}

	// Token: 0x06005334 RID: 21300 RVA: 0x001E51B1 File Offset: 0x001E33B1
	public void TriggerBuildingChangedEvent(int cell, object building)
	{
		this.OnBuildingsChanged(cell, building);
	}

	// Token: 0x06005335 RID: 21301 RVA: 0x001E51BB File Offset: 0x001E33BB
	public void SolidChangedEvent(int cell, bool ignoreDoors)
	{
		if (ignoreDoors && Grid.HasDoor[cell])
		{
			return;
		}
		this.solidChanges.Add(cell);
		this.dirty = true;
	}

	// Token: 0x06005336 RID: 21302 RVA: 0x001E51E4 File Offset: 0x001E33E4
	private CavityInfo CreateNewCavity()
	{
		CavityInfo cavityInfo = new CavityInfo();
		cavityInfo.handle = this.cavityInfos.Allocate(cavityInfo);
		return cavityInfo;
	}

	// Token: 0x06005337 RID: 21303 RVA: 0x001E520A File Offset: 0x001E340A
	private static bool IsCavityBoundary(int cell)
	{
		return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || Grid.HasDoor[cell];
	}

	// Token: 0x06005338 RID: 21304 RVA: 0x001E5228 File Offset: 0x001E3428
	public void Refresh()
	{
		this.refresh.Run();
	}

	// Token: 0x06005339 RID: 21305 RVA: 0x001E5243 File Offset: 0x001E3443
	public void Sim1000ms(float dt)
	{
		if (this.dirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600533A RID: 21306 RVA: 0x001E5254 File Offset: 0x001E3454
	private void CreateRoom(CavityInfo cavity)
	{
		global::Debug.Assert(cavity.room == null);
		Room room = new Room
		{
			cavity = cavity
		};
		cavity.room = room;
		this.rooms.Add(room);
		room.roomType = Db.Get().RoomTypes.GetRoomType(room);
		this.AssignBuildingsToRoom(room);
	}

	// Token: 0x0600533B RID: 21307 RVA: 0x001E52AC File Offset: 0x001E34AC
	private void ClearRoom(Room room)
	{
		this.UnassignBuildingsToRoom(room);
		room.CleanUp();
		this.rooms.Remove(room);
	}

	// Token: 0x0600533C RID: 21308 RVA: 0x001E52C8 File Offset: 0x001E34C8
	private void RefreshRooms(List<KPrefabID> dirtyEntities)
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (CavityInfo cavityInfo in this.cavityInfos.GetDataList())
		{
			if (cavityInfo.dirty)
			{
				global::Debug.Assert(cavityInfo.room == null, "I expected info.room to always be null by this point");
				if (cavityInfo.NumCells > 0)
				{
					if (cavityInfo.NumCells <= maxRoomSize)
					{
						this.CreateRoom(cavityInfo);
					}
					foreach (KPrefabID kprefabID in cavityInfo.buildings)
					{
						kprefabID.Trigger(144050788, cavityInfo.room);
					}
					foreach (KPrefabID kprefabID2 in cavityInfo.plants)
					{
						kprefabID2.Trigger(144050788, cavityInfo.room);
					}
				}
				cavityInfo.dirty = false;
			}
		}
		foreach (KPrefabID kprefabID3 in dirtyEntities)
		{
			if (kprefabID3 != null)
			{
				kprefabID3.Trigger(144050788, null);
			}
		}
		this.dirty = false;
	}

	// Token: 0x0600533D RID: 21309 RVA: 0x001E5454 File Offset: 0x001E3654
	private void AssignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		RoomType roomType = room.roomType;
		if (roomType == Db.Get().RoomTypes.Neutral)
		{
			return;
		}
		foreach (KPrefabID kprefabID in room.buildings)
		{
			Assignable assignable;
			if (!(kprefabID == null) && !kprefabID.HasTag(GameTags.NotRoomAssignable) && kprefabID.TryGetComponent<Assignable>(out assignable) && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(kprefabID)))
			{
				assignable.Assign(room);
			}
		}
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x001E5508 File Offset: 0x001E3708
	private void UnassignKPrefabIDs(Room room, List<KPrefabID> buildings)
	{
		foreach (KPrefabID kprefabID in buildings)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, null);
				Assignable assignable;
				if (kprefabID.TryGetComponent<Assignable>(out assignable) && assignable.assignee == room)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001E5580 File Offset: 0x001E3780
	private void UnassignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		this.UnassignKPrefabIDs(room, room.buildings);
		this.UnassignKPrefabIDs(room, room.plants);
	}

	// Token: 0x06005340 RID: 21312 RVA: 0x001E55A8 File Offset: 0x001E37A8
	public void UpdateRoom(CavityInfo cavity)
	{
		if (cavity == null)
		{
			return;
		}
		if (cavity.room != null)
		{
			this.ClearRoom(cavity.room);
			cavity.room = null;
		}
		this.CreateRoom(cavity);
		foreach (KPrefabID kprefabID in cavity.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(144050788, cavity.room);
			}
		}
		foreach (KPrefabID kprefabID2 in cavity.plants)
		{
			if (kprefabID2 != null)
			{
				kprefabID2.Trigger(144050788, cavity.room);
			}
		}
	}

	// Token: 0x06005341 RID: 21313 RVA: 0x001E568C File Offset: 0x001E388C
	public Room GetRoomOfGameObject(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		int cell = Grid.PosToCell(go);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		CavityInfo cavityForCell = this.GetCavityForCell(cell);
		if (cavityForCell == null)
		{
			return null;
		}
		return cavityForCell.room;
	}

	// Token: 0x06005342 RID: 21314 RVA: 0x001E56C8 File Offset: 0x001E38C8
	public bool IsInRoomType(GameObject go, RoomType checkType)
	{
		Room roomOfGameObject = this.GetRoomOfGameObject(go);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			return checkType == roomType;
		}
		return false;
	}

	// Token: 0x06005343 RID: 21315 RVA: 0x001E56F0 File Offset: 0x001E38F0
	private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
	{
		CavityInfo result = null;
		if (id.IsValid())
		{
			result = this.cavityInfos.GetData(id);
		}
		return result;
	}

	// Token: 0x06005344 RID: 21316 RVA: 0x001E5718 File Offset: 0x001E3918
	public CavityInfo GetCavityForCell(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		HandleVector<int>.Handle id = this.CellCavityID[cell];
		return this.GetCavityInfo(id);
	}

	// Token: 0x0400384F RID: 14415
	public List<Room> rooms = new List<Room>();

	// Token: 0x04003850 RID: 14416
	private readonly KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);

	// Token: 0x04003851 RID: 14417
	private readonly HandleVector<int>.Handle[] CellCavityID;

	// Token: 0x04003852 RID: 14418
	private readonly RoomProber.RefreshModule refresh;

	// Token: 0x04003853 RID: 14419
	private readonly HashSet<int> solidChanges = new HashSet<int>();

	// Token: 0x04003854 RID: 14420
	private bool dirty = true;

	// Token: 0x02001C6E RID: 7278
	public class Tuning : TuningData<RoomProber.Tuning>
	{
		// Token: 0x04008815 RID: 34837
		public int maxRoomSize;
	}

	// Token: 0x02001C6F RID: 7279
	private struct RefreshModule
	{
		// Token: 0x0600ADA5 RID: 44453 RVA: 0x003D0568 File Offset: 0x003CE768
		public RefreshModule(RoomProber roomProber)
		{
			this.roomProber = roomProber;
			this.cavityBuilder = new RoomProber.RefreshModule.CavityBuilder();
			this.dirtyCells = new List<int>();
			this.condemnedCavities = new List<HandleVector<int>.Handle>();
			this.newCavities = new List<CavityInfo>();
			this.dirtyEntities = new List<KPrefabID>();
			this.visitedCells = new HashSet<int>();
			this.visitedCavities = new HashSet<HandleVector<int>.Handle>();
			this.visitedBuildings = new HashSet<RoomProber.RefreshModule.BuildingId>();
			this.addCellToGrid = null;
		}

		// Token: 0x0600ADA6 RID: 44454 RVA: 0x003D05DB File Offset: 0x003CE7DB
		public void Initialize()
		{
			this.addCellToGrid = new Func<int, bool>(this.AddCellToGrid);
		}

		// Token: 0x0600ADA7 RID: 44455 RVA: 0x003D05FC File Offset: 0x003CE7FC
		public void Run()
		{
			this.CollectDirtyCells();
			this.CollectCondemnedCavities();
			this.BuildNewCavities();
			foreach (HandleVector<int>.Handle handle in this.condemnedCavities)
			{
				CavityInfo data = this.roomProber.cavityInfos.GetData(handle);
				this.dirtyEntities.Capacity = Math.Max(this.dirtyEntities.Capacity, this.dirtyEntities.Count + data.creatures.Count + data.otherEntities.Count);
				foreach (KPrefabID item in data.creatures)
				{
					this.dirtyEntities.Add(item);
				}
				foreach (KPrefabID item2 in data.otherEntities)
				{
					this.dirtyEntities.Add(item2);
				}
				if (data.room != null)
				{
					this.roomProber.ClearRoom(data.room);
				}
				this.roomProber.cavityInfos.Free(handle);
			}
			this.AddRoomContentsToCavities();
			this.roomProber.RefreshRooms(this.dirtyEntities);
			this.Recycle();
		}

		// Token: 0x0600ADA8 RID: 44456 RVA: 0x003D07B0 File Offset: 0x003CE9B0
		private readonly void Recycle()
		{
			this.dirtyCells.Clear();
			this.condemnedCavities.Clear();
			this.newCavities.Clear();
			this.dirtyEntities.Clear();
		}

		// Token: 0x0600ADA9 RID: 44457 RVA: 0x003D07E0 File Offset: 0x003CE9E0
		private readonly bool AddCellToGrid(int flood_cell)
		{
			if (RoomProber.IsCavityBoundary(flood_cell))
			{
				this.roomProber.CellCavityID[flood_cell] = HandleVector<int>.InvalidHandle;
				return false;
			}
			this.cavityBuilder.AddCell(flood_cell);
			this.roomProber.CellCavityID[flood_cell] = this.cavityBuilder.CavityID;
			return true;
		}

		// Token: 0x0600ADAA RID: 44458 RVA: 0x003D0838 File Offset: 0x003CEA38
		private unsafe readonly void CollectDirtyCells()
		{
			int* ptr = stackalloc int[(UIntPtr)20];
			*ptr = 0;
			ptr[1] = -Grid.WidthInCells;
			ptr[2] = -1;
			ptr[3] = 1;
			ptr[4] = Grid.WidthInCells;
			foreach (int num in this.roomProber.solidChanges)
			{
				for (int i = 0; i < 5; i++)
				{
					int num2 = num + ptr[i];
					if (Grid.IsValidCell(num2) && this.visitedCells.Add(num2))
					{
						this.dirtyCells.Add(num2);
					}
				}
			}
			this.visitedCells.Clear();
			this.roomProber.solidChanges.Clear();
		}

		// Token: 0x0600ADAB RID: 44459 RVA: 0x003D0910 File Offset: 0x003CEB10
		private readonly void CollectCondemnedCavities()
		{
			foreach (int num in this.dirtyCells)
			{
				if (!this.visitedCells.Contains(num))
				{
					HandleVector<int>.Handle handle = this.roomProber.CellCavityID[num];
					if (!handle.IsValid())
					{
						this.visitedCells.Add(num);
					}
					else
					{
						if (this.visitedCavities.Add(handle))
						{
							this.condemnedCavities.Add(handle);
						}
						CavityInfo data = this.roomProber.cavityInfos.GetData(handle);
						this.visitedCells.EnsureCapacity(this.visitedCells.Count + data.cells.Count);
						foreach (int num2 in data.cells)
						{
							this.roomProber.CellCavityID[num2] = HandleVector<int>.InvalidHandle;
							this.visitedCells.Add(num2);
						}
					}
				}
			}
			this.visitedCells.Clear();
			this.visitedCavities.Clear();
		}

		// Token: 0x0600ADAC RID: 44460 RVA: 0x003D0A68 File Offset: 0x003CEC68
		private readonly void BuildNewCavities()
		{
			int num = 0;
			foreach (HandleVector<int>.Handle handle in this.condemnedCavities)
			{
				num += this.roomProber.cavityInfos.GetData(handle).NumCells;
			}
			this.dirtyCells.Capacity = Math.Max(this.dirtyCells.Capacity, this.dirtyCells.Count + num);
			foreach (HandleVector<int>.Handle handle2 in this.condemnedCavities)
			{
				foreach (int item in this.roomProber.cavityInfos.GetData(handle2).cells)
				{
					this.dirtyCells.Add(item);
				}
			}
			int num2 = (this.condemnedCavities.Count > 0) ? 0 : -1;
			foreach (int num3 in this.dirtyCells)
			{
				if (!this.visitedCells.Contains(num3))
				{
					HandleVector<int>.Handle handle3 = this.roomProber.CellCavityID[num3];
					if (!handle3.IsValid())
					{
						if (RoomProber.IsCavityBoundary(num3))
						{
							this.visitedCells.Add(num3);
							this.roomProber.CellCavityID[num3] = HandleVector<int>.InvalidHandle;
						}
						else
						{
							CavityInfo cavityInfo = this.roomProber.CreateNewCavity();
							if (num2 >= 0)
							{
								CavityInfo data = this.roomProber.cavityInfos.GetData(this.condemnedCavities[num2]);
								cavityInfo.cells = data.cells;
								cavityInfo.cells.Clear();
								data.cells = null;
								num2++;
								if (num2 >= this.condemnedCavities.Count)
								{
									num2 = -1;
								}
							}
							else
							{
								cavityInfo.cells = new List<int>();
							}
							this.cavityBuilder.Reset(cavityInfo.handle);
							GameUtil.FloodFillConditional(num3, this.addCellToGrid, this.visitedCells, cavityInfo.cells);
							DebugUtil.DevAssert(this.cavityBuilder.NumCells > 0, "Degenerate cavities should have been detected and rejected prior to this point", null);
							cavityInfo.minX = this.cavityBuilder.MinX;
							cavityInfo.minY = this.cavityBuilder.MinY;
							cavityInfo.maxX = this.cavityBuilder.MaxX;
							cavityInfo.maxY = this.cavityBuilder.MaxY;
							this.newCavities.Add(cavityInfo);
						}
					}
				}
			}
			this.visitedCells.Clear();
		}

		// Token: 0x0600ADAD RID: 44461 RVA: 0x003D0D94 File Offset: 0x003CEF94
		private void AddRoomContentsToCavities()
		{
			int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
			foreach (CavityInfo cavityInfo in this.newCavities)
			{
				if (cavityInfo.NumCells <= maxRoomSize)
				{
					foreach (int cell in cavityInfo.cells)
					{
						GameObject gameObject = Grid.Objects[cell, 1];
						if (!(gameObject == null))
						{
							KPrefabID component = gameObject.GetComponent<KPrefabID>();
							RoomProber.RefreshModule.BuildingId item = new RoomProber.RefreshModule.BuildingId
							{
								prefab = component.GetHashCode(),
								instance = component.InstanceID
							};
							if (this.visitedBuildings.Add(item))
							{
								if (component.HasTag(GameTags.RoomProberBuilding))
								{
									cavityInfo.AddBuilding(component);
								}
								else if (component.HasTag(GameTags.Plant))
								{
									cavityInfo.AddPlants(component);
								}
							}
						}
					}
				}
			}
			this.visitedBuildings.Clear();
		}

		// Token: 0x04008816 RID: 34838
		private readonly RoomProber.RefreshModule.CavityBuilder cavityBuilder;

		// Token: 0x04008817 RID: 34839
		private readonly RoomProber roomProber;

		// Token: 0x04008818 RID: 34840
		private readonly List<int> dirtyCells;

		// Token: 0x04008819 RID: 34841
		private readonly List<HandleVector<int>.Handle> condemnedCavities;

		// Token: 0x0400881A RID: 34842
		private readonly List<CavityInfo> newCavities;

		// Token: 0x0400881B RID: 34843
		private readonly List<KPrefabID> dirtyEntities;

		// Token: 0x0400881C RID: 34844
		private readonly HashSet<int> visitedCells;

		// Token: 0x0400881D RID: 34845
		private readonly HashSet<HandleVector<int>.Handle> visitedCavities;

		// Token: 0x0400881E RID: 34846
		private readonly HashSet<RoomProber.RefreshModule.BuildingId> visitedBuildings;

		// Token: 0x0400881F RID: 34847
		private Func<int, bool> addCellToGrid;

		// Token: 0x02002A1A RID: 10778
		private class CavityBuilder
		{
			// Token: 0x17000D7D RID: 3453
			// (get) Token: 0x0600D389 RID: 54153 RVA: 0x0043AA1A File Offset: 0x00438C1A
			// (set) Token: 0x0600D38A RID: 54154 RVA: 0x0043AA22 File Offset: 0x00438C22
			public HandleVector<int>.Handle CavityID { get; private set; }

			// Token: 0x17000D7E RID: 3454
			// (get) Token: 0x0600D38B RID: 54155 RVA: 0x0043AA2B File Offset: 0x00438C2B
			// (set) Token: 0x0600D38C RID: 54156 RVA: 0x0043AA33 File Offset: 0x00438C33
			public int MinX { get; private set; }

			// Token: 0x17000D7F RID: 3455
			// (get) Token: 0x0600D38D RID: 54157 RVA: 0x0043AA3C File Offset: 0x00438C3C
			// (set) Token: 0x0600D38E RID: 54158 RVA: 0x0043AA44 File Offset: 0x00438C44
			public int MinY { get; private set; }

			// Token: 0x17000D80 RID: 3456
			// (get) Token: 0x0600D38F RID: 54159 RVA: 0x0043AA4D File Offset: 0x00438C4D
			// (set) Token: 0x0600D390 RID: 54160 RVA: 0x0043AA55 File Offset: 0x00438C55
			public int MaxX { get; private set; }

			// Token: 0x17000D81 RID: 3457
			// (get) Token: 0x0600D391 RID: 54161 RVA: 0x0043AA5E File Offset: 0x00438C5E
			// (set) Token: 0x0600D392 RID: 54162 RVA: 0x0043AA66 File Offset: 0x00438C66
			public int MaxY { get; private set; }

			// Token: 0x17000D82 RID: 3458
			// (get) Token: 0x0600D393 RID: 54163 RVA: 0x0043AA6F File Offset: 0x00438C6F
			// (set) Token: 0x0600D394 RID: 54164 RVA: 0x0043AA77 File Offset: 0x00438C77
			public int NumCells { get; private set; }

			// Token: 0x0600D395 RID: 54165 RVA: 0x0043AA80 File Offset: 0x00438C80
			public void Reset(HandleVector<int>.Handle search_id)
			{
				this.CavityID = search_id;
				this.NumCells = 0;
				this.MinX = int.MaxValue;
				this.MinY = int.MaxValue;
				this.MaxX = 0;
				this.MaxY = 0;
			}

			// Token: 0x0600D396 RID: 54166 RVA: 0x0043AAB4 File Offset: 0x00438CB4
			public void AddCell(int flood_cell)
			{
				int val;
				int val2;
				Grid.CellToXY(flood_cell, out val, out val2);
				this.MinX = Math.Min(val, this.MinX);
				this.MinY = Math.Min(val2, this.MinY);
				this.MaxX = Math.Max(val, this.MaxX);
				this.MaxY = Math.Max(val2, this.MaxY);
				int numCells = this.NumCells + 1;
				this.NumCells = numCells;
			}
		}

		// Token: 0x02002A1B RID: 10779
		private struct BuildingId
		{
			// Token: 0x0400BA28 RID: 47656
			public int prefab;

			// Token: 0x0400BA29 RID: 47657
			public int instance;
		}
	}
}
