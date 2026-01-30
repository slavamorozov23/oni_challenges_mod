using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009AE RID: 2478
public class FloodTool : InterfaceTool
{
	// Token: 0x060047A3 RID: 18339 RVA: 0x0019E610 File Offset: 0x0019C810
	public List<int> Flood(int startCell)
	{
		HashSetPool<int, FloodTool>.PooledHashSet pooledHashSet = HashSetPool<int, FloodTool>.Allocate();
		List<int> list = new List<int>();
		GameUtil.FloodFillConditional(startCell, this.floodCriteria, pooledHashSet, list);
		pooledHashSet.Recycle();
		return list;
	}

	// Token: 0x060047A4 RID: 18340 RVA: 0x0019E63E File Offset: 0x0019C83E
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
	}

	// Token: 0x060047A5 RID: 18341 RVA: 0x0019E65E File Offset: 0x0019C85E
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.mouseCell = Grid.PosToCell(cursor_pos);
	}

	// Token: 0x04002FEC RID: 12268
	public Func<int, bool> floodCriteria;

	// Token: 0x04002FED RID: 12269
	public Action<List<int>> paintArea;

	// Token: 0x04002FEE RID: 12270
	protected Color32 areaColour = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04002FEF RID: 12271
	protected int mouseCell = -1;
}
