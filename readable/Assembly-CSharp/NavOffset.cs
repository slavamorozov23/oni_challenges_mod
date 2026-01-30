using System;

// Token: 0x020004EF RID: 1263
public struct NavOffset
{
	// Token: 0x06001B40 RID: 6976 RVA: 0x000971CE File Offset: 0x000953CE
	public NavOffset(NavType nav_type, int x, int y)
	{
		this.navType = nav_type;
		this.offset.x = x;
		this.offset.y = y;
	}

	// Token: 0x04000FC7 RID: 4039
	public NavType navType;

	// Token: 0x04000FC8 RID: 4040
	public CellOffset offset;
}
