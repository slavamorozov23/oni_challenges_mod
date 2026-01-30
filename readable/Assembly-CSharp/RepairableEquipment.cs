using System;
using KSerialization;

// Token: 0x02000AF8 RID: 2808
public class RepairableEquipment : KMonoBehaviour
{
	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001DA0FE File Offset: 0x001D82FE
	// (set) Token: 0x060051AA RID: 20906 RVA: 0x001DA10B File Offset: 0x001D830B
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001DA11C File Offset: 0x001D831C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x001DA16C File Offset: 0x001D836C
	protected override void OnSpawn()
	{
		if (!this.facadeID.IsNullOrWhiteSpace())
		{
			KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(this.facadeID).AnimFile.GetData().build.GetSymbol("object");
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			component.TryRemoveSymbolOverride("object", 0);
			component.AddSymbolOverride("object", symbol, 0);
		}
	}

	// Token: 0x04003741 RID: 14145
	public DefHandle defHandle;

	// Token: 0x04003742 RID: 14146
	[Serialize]
	public string facadeID;
}
