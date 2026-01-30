using System;
using System.Collections.Generic;

// Token: 0x020004F2 RID: 1266
public class NavGridUpdater
{
	// Token: 0x06001B60 RID: 7008 RVA: 0x00097B5E File Offset: 0x00095D5E
	public static void InitializeNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type)
	{
		NavGridUpdater.MarkValidCells(nav_table, validators, bounding_offsets);
		NavGridUpdater.CreateLinks(nav_table, max_links_per_cell, links, transitions_by_nav_type, new Dictionary<int, int>());
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x00097B78 File Offset: 0x00095D78
	public static void UpdateNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions, List<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateValidCells(dirty_nav_cells, nav_table, validators, bounding_offsets);
		NavGridUpdater.UpdateLinks(dirty_nav_cells, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x00097B94 File Offset: 0x00095D94
	private static void UpdateValidCells(List<int> dirty_solid_cells, NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		foreach (int cell in dirty_solid_cells)
		{
			for (int i = 0; i < validators.Length; i++)
			{
				validators[i].UpdateCell(cell, nav_table, bounding_offsets);
			}
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x00097BF8 File Offset: 0x00095DF8
	private static void CreateLinksForCell(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		NavGridUpdater.CreateLinks(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x00097C08 File Offset: 0x00095E08
	private static void UpdateLinks(List<int> dirty_nav_cells, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		foreach (int cell in dirty_nav_cells)
		{
			NavGridUpdater.CreateLinksForCell(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
		}
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x00097C5C File Offset: 0x00095E5C
	private static void CreateLinks(NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.CreateLinkWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x00097CAC File Offset: 0x00095EAC
	private static void CreateLinks(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		int num = cell * max_links_per_cell;
		int num2 = 0;
		for (int i = 0; i < 11; i++)
		{
			NavType nav_type = (NavType)i;
			NavGrid.Transition[] array = transitions_by_nav_type[i];
			if (array != null && nav_table.IsValid(cell, nav_type))
			{
				NavGrid.Transition[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					NavGrid.Transition transition;
					if ((transition = array2[j]).start == NavType.Teleport && teleport_transitions.ContainsKey(cell))
					{
						int num3;
						int num4;
						Grid.CellToXY(cell, out num3, out num4);
						int num5 = teleport_transitions[cell];
						int num6;
						int num7;
						Grid.CellToXY(teleport_transitions[cell], out num6, out num7);
						transition.x = num6 - num3;
						transition.y = num7 - num4;
					}
					int num8 = transition.IsValid(cell, nav_table);
					if (num8 != Grid.InvalidCell)
					{
						links[num] = new NavGrid.Link(num8, transition.start, transition.end, transition.id, transition.cost);
						num++;
						num2++;
					}
				}
			}
		}
		if (num2 >= max_links_per_cell)
		{
			Debug.LogError("Out of nav links. Need to increase maxLinksPerCell:" + max_links_per_cell.ToString());
		}
		links[num].link = Grid.InvalidCell;
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x00097DD8 File Offset: 0x00095FD8
	private static void MarkValidCells(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.MarkValidCellWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, bounding_offsets, validators));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x00097E23 File Offset: 0x00096023
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x00097E38 File Offset: 0x00096038
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGridUpdater.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x04000FEA RID: 4074
	public static int InvalidHandle = -1;

	// Token: 0x04000FEB RID: 4075
	public static int InvalidIdx = -1;

	// Token: 0x04000FEC RID: 4076
	public static int InvalidCell = -1;

	// Token: 0x0200138B RID: 5003
	private struct CreateLinkWorkItem : IWorkItem<object>
	{
		// Token: 0x06008C42 RID: 35906 RVA: 0x00361006 File Offset: 0x0035F206
		public CreateLinkWorkItem(int start_cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.maxLinksPerCell = max_links_per_cell;
			this.links = links;
			this.transitionsByNavType = transitions_by_nav_type;
			this.teleportTransitions = teleport_transitions;
		}

		// Token: 0x06008C43 RID: 35907 RVA: 0x00361038 File Offset: 0x0035F238
		public void Run(object shared_data, int threadIndex)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				NavGridUpdater.CreateLinksForCell(this.startCell + i, this.navTable, this.maxLinksPerCell, this.links, this.transitionsByNavType, this.teleportTransitions);
			}
		}

		// Token: 0x04006B9C RID: 27548
		private int startCell;

		// Token: 0x04006B9D RID: 27549
		private NavTable navTable;

		// Token: 0x04006B9E RID: 27550
		private int maxLinksPerCell;

		// Token: 0x04006B9F RID: 27551
		private NavGrid.Link[] links;

		// Token: 0x04006BA0 RID: 27552
		private NavGrid.Transition[][] transitionsByNavType;

		// Token: 0x04006BA1 RID: 27553
		private Dictionary<int, int> teleportTransitions;
	}

	// Token: 0x0200138C RID: 5004
	private struct MarkValidCellWorkItem : IWorkItem<object>
	{
		// Token: 0x06008C44 RID: 35908 RVA: 0x00361080 File Offset: 0x0035F280
		public MarkValidCellWorkItem(int start_cell, NavTable nav_table, CellOffset[] bounding_offsets, NavTableValidator[] validators)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.boundingOffsets = bounding_offsets;
			this.validators = validators;
		}

		// Token: 0x06008C45 RID: 35909 RVA: 0x003610A0 File Offset: 0x0035F2A0
		public void Run(object shared_data, int threadIndex)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				int cell = this.startCell + i;
				NavTableValidator[] array = this.validators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].UpdateCell(cell, this.navTable, this.boundingOffsets);
				}
			}
		}

		// Token: 0x04006BA2 RID: 27554
		private NavTable navTable;

		// Token: 0x04006BA3 RID: 27555
		private CellOffset[] boundingOffsets;

		// Token: 0x04006BA4 RID: 27556
		private NavTableValidator[] validators;

		// Token: 0x04006BA5 RID: 27557
		private int startCell;
	}
}
