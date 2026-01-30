using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008CB RID: 2251
[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
	// Token: 0x06003E5A RID: 15962 RVA: 0x0015C6B0 File Offset: 0x0015A8B0
	private void Update()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.cells[i] != PathFinder.InvalidCell)
			{
				DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[i], Grid.SceneLayer.Background), 1f, 0f, true);
			}
		}
	}

	// Token: 0x04002672 RID: 9842
	public List<int> cells;
}
