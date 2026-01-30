using System;
using UnityEngine;

// Token: 0x02000DB5 RID: 3509
public class MotdData_Box
{
	// Token: 0x06006D7E RID: 28030 RVA: 0x00297B3C File Offset: 0x00295D3C
	public bool ShouldDisplay()
	{
		long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		return num >= this.startTime && this.finishTime >= num;
	}

	// Token: 0x04004AD0 RID: 19152
	public string category;

	// Token: 0x04004AD1 RID: 19153
	public string guid;

	// Token: 0x04004AD2 RID: 19154
	public long startTime;

	// Token: 0x04004AD3 RID: 19155
	public long finishTime;

	// Token: 0x04004AD4 RID: 19156
	public string title;

	// Token: 0x04004AD5 RID: 19157
	public string text;

	// Token: 0x04004AD6 RID: 19158
	public string image;

	// Token: 0x04004AD7 RID: 19159
	public string href;

	// Token: 0x04004AD8 RID: 19160
	public Texture2D resolvedImage;

	// Token: 0x04004AD9 RID: 19161
	public bool resolvedImageIsFromDisk;
}
