using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CA8 RID: 3240
public struct AsteroidDescriptor
{
	// Token: 0x0600631A RID: 25370 RVA: 0x0024BAC3 File Offset: 0x00249CC3
	public AsteroidDescriptor(string text, string tooltip, Color associatedColor, List<global::Tuple<string, Color, float>> bands = null, string associatedIcon = null)
	{
		this.text = text;
		this.tooltip = tooltip;
		this.associatedColor = associatedColor;
		this.bands = bands;
		this.associatedIcon = associatedIcon;
	}

	// Token: 0x04004329 RID: 17193
	public string text;

	// Token: 0x0400432A RID: 17194
	public string tooltip;

	// Token: 0x0400432B RID: 17195
	public List<global::Tuple<string, Color, float>> bands;

	// Token: 0x0400432C RID: 17196
	public Color associatedColor;

	// Token: 0x0400432D RID: 17197
	public string associatedIcon;
}
