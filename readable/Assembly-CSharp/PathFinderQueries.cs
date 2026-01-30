using System;

// Token: 0x020004F8 RID: 1272
public static class PathFinderQueries
{
	// Token: 0x06001B87 RID: 7047 RVA: 0x00098920 File Offset: 0x00096B20
	public static void Reset()
	{
		PathFinderQueries.cellQuery = new CellQuery();
		PathFinderQueries.cellCostQuery = new CellCostQuery();
		PathFinderQueries.cellArrayQuery = new CellArrayQuery();
		PathFinderQueries.cellOffsetQuery = new CellOffsetQuery();
		PathFinderQueries.safeCellQuery = new SafeCellQuery();
		PathFinderQueries.idleCellQuery = new IdleCellQuery();
		PathFinderQueries.drawNavGridQuery = new DrawNavGridQuery();
		PathFinderQueries.plantableCellQuery = new PlantableCellQuery();
		PathFinderQueries.mineableCellQuery = new MineableCellQuery();
		PathFinderQueries.staterpillarCellQuery = new StaterpillarCellQuery();
		PathFinderQueries.floorCellQuery = new FloorCellQuery();
		PathFinderQueries.buildingPlacementQuery = new BuildingPlacementQuery();
	}

	// Token: 0x04000FF6 RID: 4086
	public static CellQuery cellQuery = new CellQuery();

	// Token: 0x04000FF7 RID: 4087
	public static CellCostQuery cellCostQuery = new CellCostQuery();

	// Token: 0x04000FF8 RID: 4088
	public static CellArrayQuery cellArrayQuery = new CellArrayQuery();

	// Token: 0x04000FF9 RID: 4089
	public static CellOffsetQuery cellOffsetQuery = new CellOffsetQuery();

	// Token: 0x04000FFA RID: 4090
	public static SafeCellQuery safeCellQuery = new SafeCellQuery();

	// Token: 0x04000FFB RID: 4091
	public static IdleCellQuery idleCellQuery = new IdleCellQuery();

	// Token: 0x04000FFC RID: 4092
	public static DrawNavGridQuery drawNavGridQuery = new DrawNavGridQuery();

	// Token: 0x04000FFD RID: 4093
	public static PlantableCellQuery plantableCellQuery = new PlantableCellQuery();

	// Token: 0x04000FFE RID: 4094
	public static MineableCellQuery mineableCellQuery = new MineableCellQuery();

	// Token: 0x04000FFF RID: 4095
	public static StaterpillarCellQuery staterpillarCellQuery = new StaterpillarCellQuery();

	// Token: 0x04001000 RID: 4096
	public static FloorCellQuery floorCellQuery = new FloorCellQuery();

	// Token: 0x04001001 RID: 4097
	public static BuildingPlacementQuery buildingPlacementQuery = new BuildingPlacementQuery();
}
