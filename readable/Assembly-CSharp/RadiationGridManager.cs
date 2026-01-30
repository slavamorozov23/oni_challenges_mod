using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEF RID: 2799
public static class RadiationGridManager
{
	// Token: 0x0600515D RID: 20829 RVA: 0x001D7FB8 File Offset: 0x001D61B8
	public static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x0600515E RID: 20830 RVA: 0x001D7FD5 File Offset: 0x001D61D5
	public static void Initialise()
	{
		RadiationGridManager.emitters = new List<RadiationGridEmitter>();
	}

	// Token: 0x0600515F RID: 20831 RVA: 0x001D7FE1 File Offset: 0x001D61E1
	public static void Shutdown()
	{
		RadiationGridManager.emitters.Clear();
	}

	// Token: 0x06005160 RID: 20832 RVA: 0x001D7FF0 File Offset: 0x001D61F0
	public static void Refresh()
	{
		for (int i = 0; i < RadiationGridManager.emitters.Count; i++)
		{
			if (RadiationGridManager.emitters[i].enabled)
			{
				RadiationGridManager.emitters[i].Emit();
			}
		}
	}

	// Token: 0x040036FA RID: 14074
	public const float STANDARD_MASS_FALLOFF = 1000000f;

	// Token: 0x040036FB RID: 14075
	public const int RADIATION_LINGER_RATE = 4;

	// Token: 0x040036FC RID: 14076
	public static List<RadiationGridEmitter> emitters = new List<RadiationGridEmitter>();

	// Token: 0x040036FD RID: 14077
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x040036FE RID: 14078
	public static int[] previewLux;
}
