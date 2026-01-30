using System;
using UnityEngine;

// Token: 0x02000DD2 RID: 3538
public class LegendEntry
{
	// Token: 0x06006EC1 RID: 28353 RVA: 0x0029F1B0 File Offset: 0x0029D3B0
	public LegendEntry(string name, string desc, Color colour, string desc_arg = null, Sprite sprite = null, bool displaySprite = true)
	{
		this.name = name;
		this.desc = desc;
		this.colour = colour;
		this.desc_arg = desc_arg;
		this.sprite = ((sprite == null) ? Assets.instance.LegendColourBox : sprite);
		this.displaySprite = displaySprite;
	}

	// Token: 0x04004BB8 RID: 19384
	public string name;

	// Token: 0x04004BB9 RID: 19385
	public string desc;

	// Token: 0x04004BBA RID: 19386
	public string desc_arg;

	// Token: 0x04004BBB RID: 19387
	public Color colour;

	// Token: 0x04004BBC RID: 19388
	public Sprite sprite;

	// Token: 0x04004BBD RID: 19389
	public bool displaySprite;
}
