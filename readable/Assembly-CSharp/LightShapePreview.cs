using System;
using UnityEngine;

// Token: 0x020009DF RID: 2527
[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
	// Token: 0x06004982 RID: 18818 RVA: 0x001A9BF0 File Offset: 0x001A7DF0
	private void Update()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num != this.previousCell)
		{
			this.previousCell = num;
			LightGridManager.DestroyPreview();
			LightGridManager.CreatePreview(Grid.OffsetCell(num, this.offset), this.radius, this.shape, this.lux, this.width, this.direction);
		}
	}

	// Token: 0x06004983 RID: 18819 RVA: 0x001A9C52 File Offset: 0x001A7E52
	protected override void OnCleanUp()
	{
		LightGridManager.DestroyPreview();
	}

	// Token: 0x040030F2 RID: 12530
	public float radius;

	// Token: 0x040030F3 RID: 12531
	public int lux;

	// Token: 0x040030F4 RID: 12532
	public int width;

	// Token: 0x040030F5 RID: 12533
	public DiscreteShadowCaster.Direction direction;

	// Token: 0x040030F6 RID: 12534
	public global::LightShape shape;

	// Token: 0x040030F7 RID: 12535
	public CellOffset offset;

	// Token: 0x040030F8 RID: 12536
	private int previousCell = -1;
}
