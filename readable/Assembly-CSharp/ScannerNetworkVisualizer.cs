using System;
using UnityEngine;

// Token: 0x02000633 RID: 1587
[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
	// Token: 0x060025F1 RID: 9713 RVA: 0x000DA63B File Offset: 0x000D883B
	protected override void OnSpawn()
	{
		Components.ScannerVisualizers.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060025F2 RID: 9714 RVA: 0x000DA653 File Offset: 0x000D8853
	protected override void OnCleanUp()
	{
		Components.ScannerVisualizers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x0400165D RID: 5725
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x0400165E RID: 5726
	public int RangeMin;

	// Token: 0x0400165F RID: 5727
	public int RangeMax;
}
