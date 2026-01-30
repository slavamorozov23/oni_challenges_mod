using System;
using System.Collections.Generic;

// Token: 0x020004F7 RID: 1271
public class PathFinder
{
	// Token: 0x06001B79 RID: 7033 RVA: 0x00098134 File Offset: 0x00096334
	public static void Initialize()
	{
		NavType[] array = new NavType[11];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NavType)i;
		}
		PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, array);
		for (int j = 0; j < Grid.CellCount; j++)
		{
			if (Grid.Visible[j] > 0 || Grid.Spawnable[j] > 0)
			{
				HashSetPool<int, PathFinder>.PooledHashSet pooledHashSet = HashSetPool<int, PathFinder>.Allocate();
				GameUtil.FloodFillConditional(j, PathFinder.allowPathfindingFloodFillCb, pooledHashSet, null);
				Grid.AllowPathfinding[j] = true;
				pooledHashSet.Recycle();
			}
		}
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(PathFinder.OnReveal));
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000981DB File Offset: 0x000963DB
	private static void OnReveal(int cell)
	{
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000981DD File Offset: 0x000963DD
	public static void UpdatePath(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000981EC File Offset: 0x000963EC
	public static bool ValidatePath(NavGrid nav_grid, PathFinderAbilities abilities, ref PathFinder.Path path, PathFinder.PotentialPath.Flags flags)
	{
		if (!path.IsValid())
		{
			return false;
		}
		for (int i = 0; i < path.nodes.Count; i++)
		{
			PathFinder.Path.Node node = path.nodes[i];
			if (i < path.nodes.Count - 1)
			{
				PathFinder.Path.Node node2 = path.nodes[i + 1];
				int num = node.cell * nav_grid.maxLinksPerCell;
				bool flag = false;
				NavGrid.Link link = nav_grid.Links[num];
				while (link.link != PathFinder.InvalidHandle)
				{
					if (link.link == node2.cell && node2.navType == link.endNavType && node.navType == link.startNavType)
					{
						PathFinder.PotentialPath potentialPath = new PathFinder.PotentialPath(node2.cell, node.navType, flags);
						flag = abilities.TraversePath(ref potentialPath, node.cell, node.navType, 0, (int)link.transitionId, false);
						if (flag)
						{
							flags = potentialPath.flags;
							break;
						}
					}
					num++;
					link = nav_grid.Links[num];
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0009830C File Offset: 0x0009650C
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query)
	{
		int invalidCell = PathFinder.InvalidCell;
		NavType nav_type = NavType.NumNavTypes;
		query.ClearResult();
		if (!Grid.IsValidCell(potential_path.cell))
		{
			return;
		}
		PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref nav_type);
		if (invalidCell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, nav_type, out flag);
			query.SetResult(invalidCell, cell.cost, nav_type);
		}
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x00098370 File Offset: 0x00096570
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query);
		if (query.GetResultCell() != PathFinder.InvalidCell)
		{
			PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
			return;
		}
		path.Clear();
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x000983A4 File Offset: 0x000965A4
	private static void BuildResultPath(int path_cell, NavType path_nav_type, ref PathFinder.Path path)
	{
		if (path_cell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out flag);
			path.Clear();
			path.cost = cell.cost;
			while (path_cell != PathFinder.InvalidCell)
			{
				path.AddNode(new PathFinder.Path.Node
				{
					cell = path_cell,
					navType = cell.navType,
					transitionId = cell.transitionId
				});
				path_cell = cell.parent;
				if (path_cell != PathFinder.InvalidCell)
				{
					cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out flag);
				}
			}
			if (path.nodes != null)
			{
				for (int i = 0; i < path.nodes.Count / 2; i++)
				{
					PathFinder.Path.Node value = path.nodes[i];
					path.nodes[i] = path.nodes[path.nodes.Count - i - 1];
					path.nodes[path.nodes.Count - i - 1] = value;
				}
			}
		}
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x000984B0 File Offset: 0x000966B0
	private static void FindPaths(NavGrid nav_grid, ref PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, PathFinder.PotentialList potentials, ref int result_cell, ref NavType result_nav_type)
	{
		potentials.Clear();
		ushort num = PathFinder.PathGrid.SerialNo + 1;
		if (num == 0)
		{
			num += 1;
		}
		PathFinder.PathGrid.BeginUpdate(num, potential_path.cell, null);
		bool flag;
		PathFinder.Cell cell = PathFinder.PathGrid.GetCell(potential_path, out flag);
		PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, 0, potentials, PathFinder.PathGrid, ref cell);
		int num2 = int.MaxValue;
		while (potentials.Count > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
			cell = PathFinder.PathGrid.GetCell(keyValuePair.Value, out flag);
			if (cell.cost == keyValuePair.Key)
			{
				if (cell.navType != NavType.Tube && query.IsMatch(keyValuePair.Value.cell, cell.parent, cell.cost) && cell.cost < num2)
				{
					result_cell = keyValuePair.Value.cell;
					num2 = cell.cost;
					result_nav_type = cell.navType;
					break;
				}
				PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell.cost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell.parent, cell.parentNavType);
			}
		}
		PathFinder.PathGrid.EndUpdate();
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000985F7 File Offset: 0x000967F7
	public static void AddPotential(PathFinder.PotentialPath potential_path, int parent_cell, NavType parent_nav_type, int cost, byte transition_id, PathFinder.PotentialList potentials, PathGrid path_grid, ref PathFinder.Cell cell_data)
	{
		cell_data.cost = cost;
		cell_data.parent = parent_cell;
		cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
		cell_data.transitionId = transition_id;
		potentials.Add(cost, potential_path);
		path_grid.SetCell(potential_path, ref cell_data);
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x00098634 File Offset: 0x00096834
	public static bool IsSubmerged(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num = Grid.CellAbove(cell);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsLiquid) || (Grid.Element[cell].IsLiquid && Grid.IsValidCell(num) && Grid.Solid[num]);
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x00098690 File Offset: 0x00096890
	public static void AddPotentials(PathFinder.PotentialScratchPad potential_scratch_pad, PathFinder.PotentialPath potential, int cost, ref PathFinderAbilities abilities, PathFinderQuery query, int max_links_per_cell, NavGrid.Link[] links, PathFinder.PotentialList potentials, PathGrid path_grid, int parent_cell, NavType parent_nav_type)
	{
		if (!Grid.IsValidCell(potential.cell))
		{
			return;
		}
		int num = 0;
		NavGrid.Link[] linksWithCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
		int num2 = potential.cell * max_links_per_cell;
		int num3 = num2 + max_links_per_cell;
		for (int i = num2; i < num3; i++)
		{
			NavGrid.Link link = links[i];
			int link2 = link.link;
			if (link2 == PathFinder.InvalidHandle)
			{
				break;
			}
			if (link.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link.startNavType))
			{
				linksWithCorrectNavType[num++] = link;
			}
		}
		int num4 = 0;
		PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
		for (int j = 0; j < num; j++)
		{
			NavGrid.Link link3 = linksWithCorrectNavType[j];
			int link4 = link3.link;
			bool flag = false;
			PathFinder.Cell cell = path_grid.GetCell(link4, link3.endNavType, out flag);
			if (flag)
			{
				int num5 = cost + (int)link3.cost;
				bool flag2 = cell.cost == -1;
				bool flag3 = num5 < cell.cost;
				if (flag2 || flag3)
				{
					linksInCellRange[num4++] = new PathFinder.PotentialScratchPad.PathGridCellData
					{
						pathGridCell = cell,
						link = link3
					};
				}
			}
		}
		for (int k = 0; k < num4; k++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[k];
			int link5 = pathGridCellData.link.link;
			pathGridCellData.isSubmerged = PathFinder.IsSubmerged(link5);
			linksInCellRange[k] = pathGridCellData;
		}
		for (int l = 0; l < num4; l++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData2 = linksInCellRange[l];
			NavGrid.Link link6 = pathGridCellData2.link;
			int link7 = link6.link;
			PathFinder.Cell pathGridCell = pathGridCellData2.pathGridCell;
			int num6 = cost + (int)link6.cost;
			PathFinder.PotentialPath potentialPath = potential;
			potentialPath.cell = link7;
			potentialPath.navType = link6.endNavType;
			if (pathGridCellData2.isSubmerged)
			{
				int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potentialPath, link6);
				num6 += submergedPathCostPenalty;
			}
			PathFinder.PotentialPath.Flags flags = potentialPath.flags;
			bool flag4 = abilities.TraversePath(ref potentialPath, potential.cell, potential.navType, num6, (int)link6.transitionId, pathGridCellData2.isSubmerged);
			PathFinder.PotentialPath.Flags flags2 = potentialPath.flags;
			if (flag4)
			{
				PathFinder.AddPotential(potentialPath, potential.cell, potential.navType, num6, link6.transitionId, potentials, path_grid, ref pathGridCell);
			}
		}
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x000988D3 File Offset: 0x00096AD3
	public static void DestroyStatics()
	{
		PathFinder.PathGrid.OnCleanUp();
		PathFinder.PathGrid = null;
		PathFinder.Temp.Potentials.Clear();
	}

	// Token: 0x04000FF1 RID: 4081
	public static int InvalidHandle = -1;

	// Token: 0x04000FF2 RID: 4082
	public static int InvalidIdx = -1;

	// Token: 0x04000FF3 RID: 4083
	public static int InvalidCell = -1;

	// Token: 0x04000FF4 RID: 4084
	public static PathGrid PathGrid;

	// Token: 0x04000FF5 RID: 4085
	private static readonly Func<int, bool> allowPathfindingFloodFillCb = delegate(int cell)
	{
		if (Grid.Solid[cell])
		{
			return false;
		}
		if (Grid.AllowPathfinding[cell])
		{
			return false;
		}
		Grid.AllowPathfinding[cell] = true;
		return true;
	};

	// Token: 0x0200138D RID: 5005
	public struct Cell
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06008C46 RID: 35910 RVA: 0x003610F0 File Offset: 0x0035F2F0
		public NavType navType
		{
			get
			{
				return (NavType)(this.navTypes & 15);
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06008C47 RID: 35911 RVA: 0x003610FC File Offset: 0x0035F2FC
		public NavType parentNavType
		{
			get
			{
				return (NavType)(this.navTypes >> 4);
			}
		}

		// Token: 0x06008C48 RID: 35912 RVA: 0x00361108 File Offset: 0x0035F308
		public void SetNavTypes(NavType type, NavType parent_type)
		{
			this.navTypes = (byte)(type | parent_type << 4);
		}

		// Token: 0x04006BA6 RID: 27558
		public int cost;

		// Token: 0x04006BA7 RID: 27559
		public int parent;

		// Token: 0x04006BA8 RID: 27560
		public ushort queryId;

		// Token: 0x04006BA9 RID: 27561
		private byte navTypes;

		// Token: 0x04006BAA RID: 27562
		public byte transitionId;
	}

	// Token: 0x0200138E RID: 5006
	public struct PotentialPath
	{
		// Token: 0x06008C49 RID: 35913 RVA: 0x00361125 File Offset: 0x0035F325
		public PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
		{
			this.cell = cell;
			this.navType = nav_type;
			this.flags = flags;
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x0036113C File Offset: 0x0035F33C
		public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags |= new_flags;
		}

		// Token: 0x06008C4B RID: 35915 RVA: 0x0036114C File Offset: 0x0035F34C
		public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags &= ~new_flags;
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x0036115E File Offset: 0x0035F35E
		public bool HasFlag(PathFinder.PotentialPath.Flags flag)
		{
			return this.HasAnyFlag(flag);
		}

		// Token: 0x06008C4D RID: 35917 RVA: 0x00361167 File Offset: 0x0035F367
		public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask)
		{
			return (this.flags & mask) > PathFinder.PotentialPath.Flags.None;
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06008C4E RID: 35918 RVA: 0x00361174 File Offset: 0x0035F374
		// (set) Token: 0x06008C4F RID: 35919 RVA: 0x0036117C File Offset: 0x0035F37C
		public PathFinder.PotentialPath.Flags flags { readonly get; private set; }

		// Token: 0x04006BAB RID: 27563
		public int cell;

		// Token: 0x04006BAC RID: 27564
		public NavType navType;

		// Token: 0x020027FD RID: 10237
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x0400B14D RID: 45389
			None = 0,
			// Token: 0x0400B14E RID: 45390
			HasAtmoSuit = 1,
			// Token: 0x0400B14F RID: 45391
			HasJetPack = 2,
			// Token: 0x0400B150 RID: 45392
			HasOxygenMask = 4,
			// Token: 0x0400B151 RID: 45393
			PerformSuitChecks = 8,
			// Token: 0x0400B152 RID: 45394
			HasLeadSuit = 16
		}
	}

	// Token: 0x0200138F RID: 5007
	public struct Path
	{
		// Token: 0x06008C50 RID: 35920 RVA: 0x00361185 File Offset: 0x0035F385
		public void AddNode(PathFinder.Path.Node node)
		{
			if (this.nodes == null)
			{
				this.nodes = new List<PathFinder.Path.Node>();
			}
			this.nodes.Add(node);
		}

		// Token: 0x06008C51 RID: 35921 RVA: 0x003611A6 File Offset: 0x0035F3A6
		public bool IsValid()
		{
			return this.nodes != null && this.nodes.Count > 1;
		}

		// Token: 0x06008C52 RID: 35922 RVA: 0x003611C0 File Offset: 0x0035F3C0
		public bool HasArrived()
		{
			return this.nodes != null && this.nodes.Count > 0;
		}

		// Token: 0x06008C53 RID: 35923 RVA: 0x003611DA File Offset: 0x0035F3DA
		public void Clear()
		{
			this.cost = 0;
			if (this.nodes != null)
			{
				this.nodes.Clear();
			}
		}

		// Token: 0x04006BAE RID: 27566
		public int cost;

		// Token: 0x04006BAF RID: 27567
		public List<PathFinder.Path.Node> nodes;

		// Token: 0x020027FE RID: 10238
		public struct Node
		{
			// Token: 0x0400B153 RID: 45395
			public int cell;

			// Token: 0x0400B154 RID: 45396
			public NavType navType;

			// Token: 0x0400B155 RID: 45397
			public byte transitionId;
		}
	}

	// Token: 0x02001390 RID: 5008
	public class PotentialList
	{
		// Token: 0x06008C54 RID: 35924 RVA: 0x003611F6 File Offset: 0x0035F3F6
		public KeyValuePair<int, PathFinder.PotentialPath> Next()
		{
			return this.queue.Dequeue();
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06008C55 RID: 35925 RVA: 0x00361203 File Offset: 0x0035F403
		public int Count
		{
			get
			{
				return this.queue.Count;
			}
		}

		// Token: 0x06008C56 RID: 35926 RVA: 0x00361210 File Offset: 0x0035F410
		public void Add(int cost, PathFinder.PotentialPath path)
		{
			this.queue.Enqueue(cost, path);
		}

		// Token: 0x06008C57 RID: 35927 RVA: 0x0036121F File Offset: 0x0035F41F
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04006BB0 RID: 27568
		private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

		// Token: 0x020027FF RID: 10239
		public class PriorityQueue<TValue>
		{
			// Token: 0x0600CACE RID: 51918 RVA: 0x0042BFE5 File Offset: 0x0042A1E5
			public PriorityQueue()
			{
				this._baseHeap = new List<KeyValuePair<int, TValue>>();
			}

			// Token: 0x0600CACF RID: 51919 RVA: 0x0042BFF8 File Offset: 0x0042A1F8
			public void Enqueue(int priority, TValue value)
			{
				this.Insert(priority, value);
			}

			// Token: 0x0600CAD0 RID: 51920 RVA: 0x0042C002 File Offset: 0x0042A202
			public KeyValuePair<int, TValue> Dequeue()
			{
				KeyValuePair<int, TValue> result = this._baseHeap[0];
				this.DeleteRoot();
				return result;
			}

			// Token: 0x0600CAD1 RID: 51921 RVA: 0x0042C016 File Offset: 0x0042A216
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.Count > 0)
				{
					return this._baseHeap[0];
				}
				throw new InvalidOperationException("Priority queue is empty");
			}

			// Token: 0x0600CAD2 RID: 51922 RVA: 0x0042C038 File Offset: 0x0042A238
			private void ExchangeElements(int pos1, int pos2)
			{
				KeyValuePair<int, TValue> value = this._baseHeap[pos1];
				this._baseHeap[pos1] = this._baseHeap[pos2];
				this._baseHeap[pos2] = value;
			}

			// Token: 0x0600CAD3 RID: 51923 RVA: 0x0042C078 File Offset: 0x0042A278
			private void Insert(int priority, TValue value)
			{
				KeyValuePair<int, TValue> item = new KeyValuePair<int, TValue>(priority, value);
				this._baseHeap.Add(item);
				this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
			}

			// Token: 0x0600CAD4 RID: 51924 RVA: 0x0042C0B0 File Offset: 0x0042A2B0
			private int HeapifyFromEndToBeginning(int pos)
			{
				if (pos >= this._baseHeap.Count)
				{
					return -1;
				}
				while (pos > 0)
				{
					int num = (pos - 1) / 2;
					if (this._baseHeap[num].Key - this._baseHeap[pos].Key <= 0)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
				return pos;
			}

			// Token: 0x0600CAD5 RID: 51925 RVA: 0x0042C110 File Offset: 0x0042A310
			private void DeleteRoot()
			{
				if (this._baseHeap.Count <= 1)
				{
					this._baseHeap.Clear();
					return;
				}
				this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
				this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
				this.HeapifyFromBeginningToEnd(0);
			}

			// Token: 0x0600CAD6 RID: 51926 RVA: 0x0042C17C File Offset: 0x0042A37C
			private void HeapifyFromBeginningToEnd(int pos)
			{
				int count = this._baseHeap.Count;
				if (pos >= count)
				{
					return;
				}
				for (;;)
				{
					int num = pos;
					int num2 = 2 * pos + 1;
					int num3 = 2 * pos + 2;
					if (num2 < count && this._baseHeap[num].Key - this._baseHeap[num2].Key > 0)
					{
						num = num2;
					}
					if (num3 < count && this._baseHeap[num].Key - this._baseHeap[num3].Key > 0)
					{
						num = num3;
					}
					if (num == pos)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
			}

			// Token: 0x0600CAD7 RID: 51927 RVA: 0x0042C224 File Offset: 0x0042A424
			public void Clear()
			{
				this._baseHeap.Clear();
			}

			// Token: 0x17000D64 RID: 3428
			// (get) Token: 0x0600CAD8 RID: 51928 RVA: 0x0042C231 File Offset: 0x0042A431
			public int Count
			{
				get
				{
					return this._baseHeap.Count;
				}
			}

			// Token: 0x0400B156 RID: 45398
			private List<KeyValuePair<int, TValue>> _baseHeap;
		}

		// Token: 0x02002800 RID: 10240
		private class HOTQueue<TValue>
		{
			// Token: 0x0600CAD9 RID: 51929 RVA: 0x0042C240 File Offset: 0x0042A440
			public KeyValuePair<int, TValue> Dequeue()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				this.count--;
				return this.hotQueue.Dequeue();
			}

			// Token: 0x0600CADA RID: 51930 RVA: 0x0042C29C File Offset: 0x0042A49C
			public void Enqueue(int priority, TValue value)
			{
				if (priority <= this.hotThreshold)
				{
					this.hotQueue.Enqueue(priority, value);
				}
				else
				{
					this.coldQueue.Enqueue(priority, value);
					this.coldThreshold = Math.Max(this.coldThreshold, priority);
				}
				this.count++;
			}

			// Token: 0x0600CADB RID: 51931 RVA: 0x0042C2F0 File Offset: 0x0042A4F0
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				return this.hotQueue.Peek();
			}

			// Token: 0x0600CADC RID: 51932 RVA: 0x0042C33B File Offset: 0x0042A53B
			public void Clear()
			{
				this.count = 0;
				this.hotThreshold = int.MinValue;
				this.hotQueue.Clear();
				this.coldThreshold = int.MinValue;
				this.coldQueue.Clear();
			}

			// Token: 0x17000D65 RID: 3429
			// (get) Token: 0x0600CADD RID: 51933 RVA: 0x0042C370 File Offset: 0x0042A570
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x0400B157 RID: 45399
			private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x0400B158 RID: 45400
			private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x0400B159 RID: 45401
			private int hotThreshold = int.MinValue;

			// Token: 0x0400B15A RID: 45402
			private int coldThreshold = int.MinValue;

			// Token: 0x0400B15B RID: 45403
			private int count;
		}
	}

	// Token: 0x02001391 RID: 5009
	private class Temp
	{
		// Token: 0x04006BB1 RID: 27569
		public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
	}

	// Token: 0x02001392 RID: 5010
	public class PotentialScratchPad
	{
		// Token: 0x06008C5B RID: 35931 RVA: 0x00361253 File Offset: 0x0035F453
		public PotentialScratchPad(int max_links_per_cell)
		{
			this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
			this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
		}

		// Token: 0x04006BB2 RID: 27570
		public NavGrid.Link[] linksWithCorrectNavType;

		// Token: 0x04006BB3 RID: 27571
		public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

		// Token: 0x02002801 RID: 10241
		public struct PathGridCellData
		{
			// Token: 0x0400B15C RID: 45404
			public PathFinder.Cell pathGridCell;

			// Token: 0x0400B15D RID: 45405
			public NavGrid.Link link;

			// Token: 0x0400B15E RID: 45406
			public bool isSubmerged;
		}
	}
}
