using System;

// Token: 0x02000678 RID: 1656
public class Accessory : Resource
{
	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06002893 RID: 10387 RVA: 0x000E9484 File Offset: 0x000E7684
	// (set) Token: 0x06002894 RID: 10388 RVA: 0x000E948C File Offset: 0x000E768C
	public KAnim.Build.Symbol symbol { get; private set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06002895 RID: 10389 RVA: 0x000E9495 File Offset: 0x000E7695
	// (set) Token: 0x06002896 RID: 10390 RVA: 0x000E949D File Offset: 0x000E769D
	public HashedString batchSource { get; private set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06002897 RID: 10391 RVA: 0x000E94A6 File Offset: 0x000E76A6
	// (set) Token: 0x06002898 RID: 10392 RVA: 0x000E94AE File Offset: 0x000E76AE
	public AccessorySlot slot { get; private set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06002899 RID: 10393 RVA: 0x000E94B7 File Offset: 0x000E76B7
	// (set) Token: 0x0600289A RID: 10394 RVA: 0x000E94BF File Offset: 0x000E76BF
	public KAnimFile animFile { get; private set; }

	// Token: 0x0600289B RID: 10395 RVA: 0x000E94C8 File Offset: 0x000E76C8
	public Accessory(string id, ResourceSet parent, AccessorySlot slot, HashedString batchSource, KAnim.Build.Symbol symbol, KAnimFile animFile = null, KAnimFile defaultAnimFile = null) : base(id, parent, null)
	{
		this.slot = slot;
		this.symbol = symbol;
		this.batchSource = batchSource;
		this.animFile = animFile;
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000E94F2 File Offset: 0x000E76F2
	public bool IsDefault()
	{
		return this.animFile == this.slot.defaultAnimFile;
	}
}
