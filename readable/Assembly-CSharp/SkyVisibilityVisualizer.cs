using System;
using UnityEngine;

// Token: 0x02000639 RID: 1593
[AddComponentMenu("KMonoBehaviour/scripts/SkyVisibilityVisualizer")]
public class SkyVisibilityVisualizer : KMonoBehaviour
{
	// Token: 0x06002605 RID: 9733 RVA: 0x000DA972 File Offset: 0x000D8B72
	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}

	// Token: 0x04001667 RID: 5735
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x04001668 RID: 5736
	public bool TwoWideOrgin;

	// Token: 0x04001669 RID: 5737
	public int RangeMin;

	// Token: 0x0400166A RID: 5738
	public int RangeMax;

	// Token: 0x0400166B RID: 5739
	public int ScanVerticalStep;

	// Token: 0x0400166C RID: 5740
	public bool SkipOnModuleInteriors;

	// Token: 0x0400166D RID: 5741
	public bool AllOrNothingVisibility;

	// Token: 0x0400166E RID: 5742
	public Func<int, bool> SkyVisibilityCb = new Func<int, bool>(SkyVisibilityVisualizer.HasSkyVisibility);
}
