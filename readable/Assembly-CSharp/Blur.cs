using System;
using UnityEngine;

// Token: 0x02000ADA RID: 2778
public static class Blur
{
	// Token: 0x060050CF RID: 20687 RVA: 0x001D4216 File Offset: 0x001D2416
	public static RenderTexture Run(Texture2D image)
	{
		if (Blur.blurMaterial == null)
		{
			Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		}
		return null;
	}

	// Token: 0x040035F0 RID: 13808
	private static Material blurMaterial;
}
