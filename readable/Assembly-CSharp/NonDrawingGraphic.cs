using System;
using UnityEngine.UI;

// Token: 0x02000DBD RID: 3517
public class NonDrawingGraphic : Graphic
{
	// Token: 0x06006DD9 RID: 28121 RVA: 0x00299E55 File Offset: 0x00298055
	public override void SetMaterialDirty()
	{
	}

	// Token: 0x06006DDA RID: 28122 RVA: 0x00299E57 File Offset: 0x00298057
	public override void SetVerticesDirty()
	{
	}

	// Token: 0x06006DDB RID: 28123 RVA: 0x00299E59 File Offset: 0x00298059
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
