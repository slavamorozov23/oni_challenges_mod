using System;
using UnityEngine;

// Token: 0x02000623 RID: 1571
[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
	// Token: 0x040015E9 RID: 5609
	public Vector2I OriginOffset;

	// Token: 0x040015EA RID: 5610
	public Vector2I RangeMin;

	// Token: 0x040015EB RID: 5611
	public Vector2I RangeMax;

	// Token: 0x040015EC RID: 5612
	public Vector2I TexSize = new Vector2I(64, 64);

	// Token: 0x040015ED RID: 5613
	public bool TestLineOfSight = true;

	// Token: 0x040015EE RID: 5614
	public bool BlockingTileVisible;

	// Token: 0x040015EF RID: 5615
	public Func<int, bool> BlockingVisibleCb;

	// Token: 0x040015F0 RID: 5616
	public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);

	// Token: 0x040015F1 RID: 5617
	public bool AllowLineOfSightInvalidCells;
}
