using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
public class NavGrid
{
	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06001B41 RID: 6977 RVA: 0x000971EF File Offset: 0x000953EF
	// (set) Token: 0x06001B42 RID: 6978 RVA: 0x000971F7 File Offset: 0x000953F7
	public NavTable NavTable { get; private set; }

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06001B43 RID: 6979 RVA: 0x00097200 File Offset: 0x00095400
	// (set) Token: 0x06001B44 RID: 6980 RVA: 0x00097208 File Offset: 0x00095408
	public NavGrid.Transition[] transitions { get; set; }

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06001B45 RID: 6981 RVA: 0x00097211 File Offset: 0x00095411
	// (set) Token: 0x06001B46 RID: 6982 RVA: 0x00097219 File Offset: 0x00095419
	public NavGrid.Transition[][] transitionsByNavType { get; private set; }

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06001B47 RID: 6983 RVA: 0x00097222 File Offset: 0x00095422
	// (set) Token: 0x06001B48 RID: 6984 RVA: 0x0009722A File Offset: 0x0009542A
	public int updateRangeX { get; private set; }

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06001B49 RID: 6985 RVA: 0x00097233 File Offset: 0x00095433
	// (set) Token: 0x06001B4A RID: 6986 RVA: 0x0009723B File Offset: 0x0009543B
	public int updateRangeY { get; private set; }

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06001B4B RID: 6987 RVA: 0x00097244 File Offset: 0x00095444
	// (set) Token: 0x06001B4C RID: 6988 RVA: 0x0009724C File Offset: 0x0009544C
	public int maxLinksPerCell { get; private set; }

	// Token: 0x06001B4D RID: 6989 RVA: 0x00097255 File Offset: 0x00095455
	public static NavType MirrorNavType(NavType nav_type)
	{
		if (nav_type == NavType.LeftWall)
		{
			return NavType.RightWall;
		}
		if (nav_type == NavType.RightWall)
		{
			return NavType.LeftWall;
		}
		return nav_type;
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x00097264 File Offset: 0x00095464
	public NavGrid(string id, NavGrid.Transition[] transitions, NavGrid.NavTypeData[] nav_type_data, CellOffset[] bounding_offsets, NavTableValidator[] validators, int update_range_x, int update_range_y, int max_links_per_cell)
	{
		this.DirtyBitFlags = new byte[(Grid.CellCount + 7) / 8];
		this.DirtyCells = new List<int>();
		this.id = id;
		this.Validators = validators;
		this.navTypeData = nav_type_data;
		this.transitions = transitions;
		this.boundingOffsets = bounding_offsets;
		List<NavType> list = new List<NavType>();
		this.updateRangeX = update_range_x;
		this.updateRangeY = update_range_y;
		this.maxLinksPerCell = max_links_per_cell + 1;
		for (int i = 0; i < transitions.Length; i++)
		{
			DebugUtil.Assert(i >= 0 && i <= 255);
			transitions[i].id = (byte)i;
			if (!list.Contains(transitions[i].start))
			{
				list.Add(transitions[i].start);
			}
			if (!list.Contains(transitions[i].end))
			{
				list.Add(transitions[i].end);
			}
		}
		this.ValidNavTypes = list.ToArray();
		this.DebugViewLinkType = new bool[this.ValidNavTypes.Length];
		this.DebugViewValidCellsType = new bool[this.ValidNavTypes.Length];
		foreach (NavType nav_type in this.ValidNavTypes)
		{
			this.GetNavTypeData(nav_type);
		}
		this.Links = new NavGrid.Link[this.maxLinksPerCell * Grid.CellCount];
		this.NavTable = new NavTable(Grid.CellCount);
		this.transitions = transitions;
		this.transitionsByNavType = new NavGrid.Transition[11][];
		for (int k = 0; k < 11; k++)
		{
			List<NavGrid.Transition> list2 = new List<NavGrid.Transition>();
			NavType navType = (NavType)k;
			foreach (NavGrid.Transition transition in transitions)
			{
				if (transition.start == navType)
				{
					list2.Add(transition);
				}
			}
			this.transitionsByNavType[k] = list2.ToArray();
		}
		foreach (NavTableValidator navTableValidator in validators)
		{
			navTableValidator.onDirty = (Action<int>)Delegate.Combine(navTableValidator.onDirty, new Action<int>(this.AddDirtyCell));
		}
		this.potentialScratchPad = new PathFinder.PotentialScratchPad(this.maxLinksPerCell);
		this.InitializeGraph();
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000974B0 File Offset: 0x000956B0
	public NavGrid.NavTypeData GetNavTypeData(NavType nav_type)
	{
		foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
		{
			if (navTypeData.navType == nav_type)
			{
				return navTypeData;
			}
		}
		throw new Exception("Missing nav type data for nav type:" + nav_type.ToString());
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x00097504 File Offset: 0x00095704
	public bool HasNavTypeData(NavType nav_type)
	{
		NavGrid.NavTypeData[] array = this.navTypeData;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].navType == nav_type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x00097538 File Offset: 0x00095738
	public HashedString GetIdleAnim(NavType nav_type)
	{
		return this.GetNavTypeData(nav_type).idleAnim;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x00097546 File Offset: 0x00095746
	public void InitializeGraph()
	{
		NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x00097574 File Offset: 0x00095774
	public void UpdateGraph()
	{
		int count = this.DirtyCells.Count;
		for (int i = 0; i < count; i++)
		{
			int num;
			int num2;
			Grid.CellToXY(this.DirtyCells[i], out num, out num2);
			int num3 = Grid.ClampX(num - this.updateRangeX);
			int num4 = Grid.ClampY(num2 - this.updateRangeY);
			int num5 = Grid.ClampX(num + this.updateRangeX);
			int num6 = Grid.ClampY(num2 + this.updateRangeY);
			for (int j = num4; j <= num6; j++)
			{
				for (int k = num3; k <= num5; k++)
				{
					this.AddDirtyCell(Grid.XYToCell(k, j));
				}
			}
		}
		this.UpdateGraph(this.DirtyCells);
		foreach (int num7 in this.DirtyCells)
		{
			this.DirtyBitFlags[num7 / 8] = 0;
		}
		this.DirtyCells.Clear();
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x00097684 File Offset: 0x00095884
	public void UpdateGraph(List<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
		if (this.OnNavGridUpdateComplete != null)
		{
			this.OnNavGridUpdateComplete(dirty_nav_cells);
		}
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000976D5 File Offset: 0x000958D5
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x000976EC File Offset: 0x000958EC
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGrid.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x00097744 File Offset: 0x00095944
	private void DebugDrawValidCells()
	{
		Color white = Color.white;
		int cellCount = Grid.CellCount;
		for (int i = 0; i < cellCount; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				NavType nav_type = (NavType)j;
				if (this.NavTable.IsValid(i, nav_type) && this.DrawNavTypeCell(nav_type, ref white))
				{
					DebugExtension.DebugPoint(NavTypeHelper.GetNavPos(i, nav_type), white, 1f, 0f, false);
				}
			}
		}
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000977B0 File Offset: 0x000959B0
	private void DebugDrawLinks()
	{
		Color white = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * this.maxLinksPerCell;
			for (int link = this.Links[num].link; link != NavGrid.InvalidCell; link = this.Links[num].link)
			{
				NavTypeHelper.GetNavPos(i, this.Links[num].startNavType);
				if (this.DrawNavTypeLink(this.Links[num].startNavType, ref white) || this.DrawNavTypeLink(this.Links[num].endNavType, ref white))
				{
					NavTypeHelper.GetNavPos(link, this.Links[num].endNavType);
				}
				num++;
			}
		}
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x00097880 File Offset: 0x00095A80
	private bool DrawNavTypeLink(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewLinksAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewLinkType[i];
			}
		}
		return false;
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000978CC File Offset: 0x00095ACC
	private bool DrawNavTypeCell(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewValidCellsAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewValidCellsType[i];
			}
		}
		return false;
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x00097918 File Offset: 0x00095B18
	public void DebugUpdate()
	{
		if (this.DebugViewValidCells)
		{
			this.DebugDrawValidCells();
		}
		if (this.DebugViewLinks)
		{
			this.DebugDrawLinks();
		}
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x00097938 File Offset: 0x00095B38
	public void AddDirtyCell(int cell)
	{
		if (Grid.IsValidCell(cell) && ((int)this.DirtyBitFlags[cell / 8] & 1 << cell % 8) == 0)
		{
			this.DirtyCells.Add(cell);
			byte[] dirtyBitFlags = this.DirtyBitFlags;
			int num = cell / 8;
			dirtyBitFlags[num] |= (byte)(1 << cell % 8);
		}
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x0009798C File Offset: 0x00095B8C
	public void Clear()
	{
		NavTableValidator[] validators = this.Validators;
		for (int i = 0; i < validators.Length; i++)
		{
			validators[i].Clear();
		}
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x000979B6 File Offset: 0x00095BB6
	public Color NavTypeColor(NavType navType)
	{
		return NavGrid.debugColorLookup[(int)navType];
	}

	// Token: 0x04000FCD RID: 4045
	public bool DebugViewAllPaths;

	// Token: 0x04000FCE RID: 4046
	public bool DebugViewValidCells;

	// Token: 0x04000FCF RID: 4047
	public bool[] DebugViewValidCellsType;

	// Token: 0x04000FD0 RID: 4048
	public bool DebugViewValidCellsAll;

	// Token: 0x04000FD1 RID: 4049
	public bool DebugViewLinks;

	// Token: 0x04000FD2 RID: 4050
	public bool[] DebugViewLinkType;

	// Token: 0x04000FD3 RID: 4051
	public bool DebugViewLinksAll;

	// Token: 0x04000FD4 RID: 4052
	public static int InvalidHandle = -1;

	// Token: 0x04000FD5 RID: 4053
	public static int InvalidIdx = -1;

	// Token: 0x04000FD6 RID: 4054
	public static int InvalidCell = -1;

	// Token: 0x04000FD7 RID: 4055
	public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();

	// Token: 0x04000FD8 RID: 4056
	public NavGrid.Link[] Links;

	// Token: 0x04000FDA RID: 4058
	private byte[] DirtyBitFlags;

	// Token: 0x04000FDB RID: 4059
	private List<int> DirtyCells;

	// Token: 0x04000FDC RID: 4060
	private NavTableValidator[] Validators = new NavTableValidator[0];

	// Token: 0x04000FDD RID: 4061
	private CellOffset[] boundingOffsets;

	// Token: 0x04000FDE RID: 4062
	public string id;

	// Token: 0x04000FDF RID: 4063
	public bool updateEveryFrame;

	// Token: 0x04000FE0 RID: 4064
	public PathFinder.PotentialScratchPad potentialScratchPad;

	// Token: 0x04000FE1 RID: 4065
	public Action<List<int>> OnNavGridUpdateComplete;

	// Token: 0x04000FE4 RID: 4068
	public NavType[] ValidNavTypes;

	// Token: 0x04000FE5 RID: 4069
	public NavGrid.NavTypeData[] navTypeData;

	// Token: 0x04000FE9 RID: 4073
	private static Color[] debugColorLookup = new Color[]
	{
		new Color(0.918f, 0f, 0.394f, 1f),
		new Color(0.719f, 0.375f, 0f, 1f),
		new Color(0.564f, 0.455f, 0f, 1f),
		new Color(0.425f, 0.498f, 0f, 1f),
		new Color(0f, 0.542f, 0.158f, 1f),
		new Color(1f, 0.9215686f, 0.01568628f, 1f),
		new Color(0f, 1f, 0f, 1f),
		new Color(0f, 0.505f, 0.651f, 1f),
		new Color(0.256f, 0.411f, 1f, 1f),
		new Color(0.782f, 0f, 0.937f, 1f),
		new Color(0.865f, 0f, 0.686f, 1f),
		Color.red
	};

	// Token: 0x02001388 RID: 5000
	public struct Link
	{
		// Token: 0x06008C3E RID: 35902 RVA: 0x0036093D File Offset: 0x0035EB3D
		public Link(int link, NavType start_nav_type, NavType end_nav_type, byte transition_id, byte cost)
		{
			this.link = link;
			this.startNavType = start_nav_type;
			this.endNavType = end_nav_type;
			this.transitionId = transition_id;
			this.cost = cost;
		}

		// Token: 0x04006B7F RID: 27519
		public int link;

		// Token: 0x04006B80 RID: 27520
		public NavType startNavType;

		// Token: 0x04006B81 RID: 27521
		public NavType endNavType;

		// Token: 0x04006B82 RID: 27522
		public byte transitionId;

		// Token: 0x04006B83 RID: 27523
		public byte cost;
	}

	// Token: 0x02001389 RID: 5001
	public struct NavTypeData
	{
		// Token: 0x04006B84 RID: 27524
		public NavType navType;

		// Token: 0x04006B85 RID: 27525
		public Vector2 animControllerOffset;

		// Token: 0x04006B86 RID: 27526
		public bool flipX;

		// Token: 0x04006B87 RID: 27527
		public bool flipY;

		// Token: 0x04006B88 RID: 27528
		public float rotation;

		// Token: 0x04006B89 RID: 27529
		public HashedString idleAnim;
	}

	// Token: 0x0200138A RID: 5002
	public struct Transition
	{
		// Token: 0x06008C3F RID: 35903 RVA: 0x00360964 File Offset: 0x0035EB64
		public override string ToString()
		{
			return string.Format("{0}: {1}->{2} ({3}); offset {4},{5}", new object[]
			{
				this.id,
				this.start,
				this.end,
				this.startAxis,
				this.x,
				this.y
			});
		}

		// Token: 0x06008C40 RID: 35904 RVA: 0x003609D8 File Offset: 0x0035EBD8
		public Transition(NavType start, NavType end, int x, int y, NavAxis start_axis, bool is_looping, bool loop_has_pre, bool is_escape, int cost, string anim, CellOffset[] void_offsets, CellOffset[] solid_offsets, NavOffset[] valid_nav_offsets, NavOffset[] invalid_nav_offsets, bool critter = false, float animSpeed = 1f, bool useOffsetX = false)
		{
			DebugUtil.Assert(cost <= 255 && cost >= 0);
			this.id = byte.MaxValue;
			this.start = start;
			this.end = end;
			this.x = x;
			this.y = y;
			this.startAxis = start_axis;
			this.isLooping = is_looping;
			this.isEscape = is_escape;
			this.anim = anim;
			this.preAnim = "";
			this.cost = (byte)cost;
			if (string.IsNullOrEmpty(this.anim))
			{
				this.anim = string.Concat(new string[]
				{
					start.ToString().ToLower(),
					"_",
					end.ToString().ToLower(),
					"_",
					x.ToString(),
					"_",
					y.ToString()
				});
			}
			if (this.isLooping)
			{
				if (loop_has_pre)
				{
					this.preAnim = this.anim + "_pre";
				}
				this.anim += "_loop";
			}
			if (this.startAxis != NavAxis.NA)
			{
				this.anim += ((this.startAxis == NavAxis.X) ? "_x" : "_y");
			}
			this.voidOffsets = void_offsets;
			this.solidOffsets = solid_offsets;
			this.validNavOffsets = valid_nav_offsets;
			this.invalidNavOffsets = invalid_nav_offsets;
			this.isCritter = critter;
			this.useXOffset = useOffsetX;
			this.animSpeed = animSpeed;
		}

		// Token: 0x06008C41 RID: 35905 RVA: 0x00360B6C File Offset: 0x0035ED6C
		public int IsValid(int cell, NavTable nav_table)
		{
			if (!Grid.IsCellOffsetValid(cell, this.x, this.y))
			{
				return Grid.InvalidCell;
			}
			int num = Grid.OffsetCell(cell, this.x, this.y);
			if (!nav_table.IsValid(num, this.end))
			{
				return Grid.InvalidCell;
			}
			Grid.BuildFlags buildFlags = Grid.BuildFlags.Solid | Grid.BuildFlags.DupeImpassable;
			if (this.isCritter)
			{
				buildFlags |= Grid.BuildFlags.CritterImpassable;
			}
			foreach (CellOffset cellOffset in this.voidOffsets)
			{
				int num2 = Grid.OffsetCell(cell, cellOffset.x, cellOffset.y);
				if (Grid.IsValidCell(num2) && (Grid.BuildMasks[num2] & buildFlags) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
				{
					if (this.isCritter)
					{
						return Grid.InvalidCell;
					}
					if ((Grid.BuildMasks[num2] & Grid.BuildFlags.DupePassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
					{
						return Grid.InvalidCell;
					}
				}
			}
			foreach (CellOffset cellOffset2 in this.solidOffsets)
			{
				int num3 = Grid.OffsetCell(cell, cellOffset2.x, cellOffset2.y);
				if (Grid.IsValidCell(num3) && !Grid.Solid[num3])
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset in this.validNavOffsets)
			{
				int cell2 = Grid.OffsetCell(cell, navOffset.offset.x, navOffset.offset.y);
				if (!nav_table.IsValid(cell2, navOffset.navType))
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset2 in this.invalidNavOffsets)
			{
				int cell3 = Grid.OffsetCell(cell, navOffset2.offset.x, navOffset2.offset.y);
				if (nav_table.IsValid(cell3, navOffset2.navType))
				{
					return Grid.InvalidCell;
				}
			}
			if (this.start == NavType.Tube)
			{
				if (this.end == NavType.Tube)
				{
					GameObject gameObject = Grid.Objects[cell, 9];
					GameObject gameObject2 = Grid.Objects[num, 9];
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = gameObject ? gameObject.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink2 = gameObject2 ? gameObject2.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					if (travelTubeUtilityNetworkLink)
					{
						int num4;
						int num5;
						travelTubeUtilityNetworkLink.GetCells(out num4, out num5);
						if (num != num4 && num != num5)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, num);
						if (utilityConnections == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(num, false) != utilityConnections)
						{
							return Grid.InvalidCell;
						}
					}
					else if (travelTubeUtilityNetworkLink2)
					{
						int num6;
						int num7;
						travelTubeUtilityNetworkLink2.GetCells(out num6, out num7);
						if (cell != num6 && cell != num7)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections2 = UtilityConnectionsExtensions.DirectionFromToCell(num, cell);
						if (utilityConnections2 == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(cell, false) != utilityConnections2)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						bool flag = this.startAxis == NavAxis.X;
						int cell4 = cell;
						for (int j = 0; j < 2; j++)
						{
							if ((flag && j == 0) || (!flag && j == 1))
							{
								int num8 = (this.x > 0) ? 1 : -1;
								for (int k = 0; k < Mathf.Abs(this.x); k++)
								{
									UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num8 > 0 && (connections & UtilityConnections.Right) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num8 < 0 && (connections & UtilityConnections.Left) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, num8, 0);
								}
							}
							else
							{
								int num9 = (this.y > 0) ? 1 : -1;
								for (int l = 0; l < Mathf.Abs(this.y); l++)
								{
									UtilityConnections connections2 = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num9 > 0 && (connections2 & UtilityConnections.Up) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num9 < 0 && (connections2 & UtilityConnections.Down) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, 0, num9);
								}
							}
						}
					}
				}
				else
				{
					UtilityConnections connections3 = Game.Instance.travelTubeSystem.GetConnections(cell, false);
					if (this.y > 0)
					{
						if (connections3 != UtilityConnections.Down)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x > 0)
					{
						if (connections3 != UtilityConnections.Left)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x < 0)
					{
						if (connections3 != UtilityConnections.Right)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						if (this.y >= 0)
						{
							return Grid.InvalidCell;
						}
						if (connections3 != UtilityConnections.Up)
						{
							return Grid.InvalidCell;
						}
					}
				}
			}
			else if (this.start == NavType.Floor && this.end == NavType.Tube)
			{
				int cell5 = Grid.OffsetCell(cell, this.x, this.y);
				if (Game.Instance.travelTubeSystem.GetConnections(cell5, false) != UtilityConnections.Up)
				{
					return Grid.InvalidCell;
				}
			}
			return num;
		}

		// Token: 0x04006B8A RID: 27530
		public NavType start;

		// Token: 0x04006B8B RID: 27531
		public NavType end;

		// Token: 0x04006B8C RID: 27532
		public NavAxis startAxis;

		// Token: 0x04006B8D RID: 27533
		public int x;

		// Token: 0x04006B8E RID: 27534
		public int y;

		// Token: 0x04006B8F RID: 27535
		public byte id;

		// Token: 0x04006B90 RID: 27536
		public byte cost;

		// Token: 0x04006B91 RID: 27537
		public bool isLooping;

		// Token: 0x04006B92 RID: 27538
		public bool isEscape;

		// Token: 0x04006B93 RID: 27539
		public string preAnim;

		// Token: 0x04006B94 RID: 27540
		public string anim;

		// Token: 0x04006B95 RID: 27541
		public float animSpeed;

		// Token: 0x04006B96 RID: 27542
		public CellOffset[] voidOffsets;

		// Token: 0x04006B97 RID: 27543
		public CellOffset[] solidOffsets;

		// Token: 0x04006B98 RID: 27544
		public NavOffset[] validNavOffsets;

		// Token: 0x04006B99 RID: 27545
		public NavOffset[] invalidNavOffsets;

		// Token: 0x04006B9A RID: 27546
		public bool isCritter;

		// Token: 0x04006B9B RID: 27547
		public bool useXOffset;
	}
}
