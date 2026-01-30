using System;
using System.Collections.Generic;

// Token: 0x02000679 RID: 1657
public class AccessorySlot : Resource
{
	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x0600289D RID: 10397 RVA: 0x000E950A File Offset: 0x000E770A
	// (set) Token: 0x0600289E RID: 10398 RVA: 0x000E9512 File Offset: 0x000E7712
	public KAnimHashedString targetSymbolId { get; private set; }

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x0600289F RID: 10399 RVA: 0x000E951B File Offset: 0x000E771B
	// (set) Token: 0x060028A0 RID: 10400 RVA: 0x000E9523 File Offset: 0x000E7723
	public List<Accessory> accessories { get; private set; }

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000E952C File Offset: 0x000E772C
	public KAnimFile AnimFile
	{
		get
		{
			return this.file;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000E9534 File Offset: 0x000E7734
	// (set) Token: 0x060028A3 RID: 10403 RVA: 0x000E953C File Offset: 0x000E773C
	public KAnimFile defaultAnimFile { get; private set; }

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060028A4 RID: 10404 RVA: 0x000E9545 File Offset: 0x000E7745
	// (set) Token: 0x060028A5 RID: 10405 RVA: 0x000E954D File Offset: 0x000E774D
	public int overrideLayer { get; private set; }

	// Token: 0x060028A6 RID: 10406 RVA: 0x000E9558 File Offset: 0x000E7758
	public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.overrideLayer = overrideLayer;
		this.defaultAnimFile = swap_build;
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x000E95C8 File Offset: 0x000E77C8
	public AccessorySlot(string id, ResourceSet parent, KAnimHashedString target_symbol_id, KAnimFile swap_build, KAnimFile defaultAnimFile = null, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = target_symbol_id;
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.defaultAnimFile = ((defaultAnimFile != null) ? defaultAnimFile : swap_build);
		this.overrideLayer = overrideLayer;
	}

	// Token: 0x060028A8 RID: 10408 RVA: 0x000E9634 File Offset: 0x000E7834
	public void AddAccessories(KAnimFile default_build, ResourceSet parent)
	{
		KAnim.Build build = default_build.GetData().build;
		default_build.GetData().build.GetSymbol(this.targetSymbolId);
		string value = this.Id.ToLower();
		for (int i = 0; i < build.symbols.Length; i++)
		{
			string text = HashCache.Get().Get(build.symbols[i].hash);
			if (text.StartsWith(value))
			{
				Accessory accessory = new Accessory(text, parent, this, this.file.batchTag, build.symbols[i], default_build, null);
				this.accessories.Add(accessory);
				HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
			}
		}
	}

	// Token: 0x060028A9 RID: 10409 RVA: 0x000E96ED File Offset: 0x000E78ED
	public Accessory Lookup(string id)
	{
		return this.Lookup(new HashedString(id));
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x000E96FC File Offset: 0x000E78FC
	public Accessory Lookup(HashedString full_id)
	{
		if (!full_id.IsValid)
		{
			return null;
		}
		return this.accessories.Find((Accessory a) => a.IdHash == full_id);
	}

	// Token: 0x040017FE RID: 6142
	private KAnimFile file;
}
