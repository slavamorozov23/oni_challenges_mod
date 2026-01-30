using System;

// Token: 0x0200067D RID: 1661
public class RoomTypeCategory : Resource
{
	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060028BA RID: 10426 RVA: 0x000E99A5 File Offset: 0x000E7BA5
	// (set) Token: 0x060028BB RID: 10427 RVA: 0x000E99AD File Offset: 0x000E7BAD
	public string colorName { get; private set; }

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060028BC RID: 10428 RVA: 0x000E99B6 File Offset: 0x000E7BB6
	// (set) Token: 0x060028BD RID: 10429 RVA: 0x000E99BE File Offset: 0x000E7BBE
	public string icon { get; private set; }

	// Token: 0x060028BE RID: 10430 RVA: 0x000E99C7 File Offset: 0x000E7BC7
	public RoomTypeCategory(string id, string name, string colorName, string icon) : base(id, name)
	{
		this.colorName = colorName;
		this.icon = icon;
	}
}
