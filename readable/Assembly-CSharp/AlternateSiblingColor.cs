using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C8C RID: 3212
public class AlternateSiblingColor : KMonoBehaviour
{
	// Token: 0x0600629C RID: 25244 RVA: 0x002494F8 File Offset: 0x002476F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int siblingIndex = base.transform.GetSiblingIndex();
		this.RefreshColor(siblingIndex % 2 == 0);
	}

	// Token: 0x0600629D RID: 25245 RVA: 0x00249523 File Offset: 0x00247723
	private void RefreshColor(bool evenIndex)
	{
		if (this.image == null)
		{
			return;
		}
		this.image.color = (evenIndex ? this.evenColor : this.oddColor);
	}

	// Token: 0x0600629E RID: 25246 RVA: 0x00249550 File Offset: 0x00247750
	private void Update()
	{
		if (this.mySiblingIndex != base.transform.GetSiblingIndex())
		{
			this.mySiblingIndex = base.transform.GetSiblingIndex();
			this.RefreshColor(this.mySiblingIndex % 2 == 0);
		}
	}

	// Token: 0x040042FE RID: 17150
	public Color evenColor;

	// Token: 0x040042FF RID: 17151
	public Color oddColor;

	// Token: 0x04004300 RID: 17152
	public Image image;

	// Token: 0x04004301 RID: 17153
	private int mySiblingIndex;
}
