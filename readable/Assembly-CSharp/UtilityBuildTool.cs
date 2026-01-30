using System;

// Token: 0x020009CF RID: 2511
public class UtilityBuildTool : BaseUtilityBuildTool
{
	// Token: 0x060048E9 RID: 18665 RVA: 0x001A624B File Offset: 0x001A444B
	public static void DestroyInstance()
	{
		UtilityBuildTool.Instance = null;
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x001A6253 File Offset: 0x001A4453
	protected override void OnPrefabInit()
	{
		UtilityBuildTool.Instance = this;
		base.OnPrefabInit();
		this.populateHitsList = true;
		this.canChangeDragAxis = false;
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x001A6270 File Offset: 0x001A4470
	protected override void ApplyPathToConduitSystem()
	{
		if (this.path.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this.path.Count; i++)
		{
			if (this.path[i - 1].valid && this.path[i].valid)
			{
				int cell = this.path[i - 1].cell;
				int cell2 = this.path[i].cell;
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, cell2);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					string text;
					if (this.conduitMgr.CanAddConnection(utilityConnections, cell, false, out text) && this.conduitMgr.CanAddConnection(new_connection, cell2, false, out text))
					{
						this.conduitMgr.AddConnection(utilityConnections, cell, false);
						this.conduitMgr.AddConnection(new_connection, cell2, false);
					}
					else if (i == this.path.Count - 1 && this.lastPathHead != i)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, text, null, Grid.CellToPosCCC(cell2, (Grid.SceneLayer)0), 1.5f, false, false);
					}
				}
			}
		}
		this.lastPathHead = this.path.Count - 1;
	}

	// Token: 0x0400307C RID: 12412
	public static UtilityBuildTool Instance;

	// Token: 0x0400307D RID: 12413
	private int lastPathHead = -1;
}
