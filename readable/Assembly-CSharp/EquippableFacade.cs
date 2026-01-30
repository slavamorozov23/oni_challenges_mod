using System;
using KSerialization;

// Token: 0x02000934 RID: 2356
public class EquippableFacade : KMonoBehaviour
{
	// Token: 0x060041DB RID: 16859 RVA: 0x00173E08 File Offset: 0x00172008
	public static void AddFacadeToEquippable(Equippable equippable, string facadeID)
	{
		EquippableFacade equippableFacade = equippable.gameObject.AddOrGet<EquippableFacade>();
		equippableFacade.FacadeID = facadeID;
		equippableFacade.BuildOverride = Db.GetEquippableFacades().Get(facadeID).BuildOverride;
		equippableFacade.ApplyAnimOverride();
	}

	// Token: 0x060041DC RID: 16860 RVA: 0x00173E37 File Offset: 0x00172037
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OverrideName();
		this.ApplyAnimOverride();
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060041DD RID: 16861 RVA: 0x00173E4B File Offset: 0x0017204B
	// (set) Token: 0x060041DE RID: 16862 RVA: 0x00173E53 File Offset: 0x00172053
	public string FacadeID
	{
		get
		{
			return this._facadeID;
		}
		private set
		{
			this._facadeID = value;
			this.OverrideName();
		}
	}

	// Token: 0x060041DF RID: 16863 RVA: 0x00173E62 File Offset: 0x00172062
	public void ApplyAnimOverride()
	{
		if (this.FacadeID.IsNullOrWhiteSpace())
		{
			return;
		}
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Db.GetEquippableFacades().Get(this.FacadeID).AnimFile
		});
	}

	// Token: 0x060041E0 RID: 16864 RVA: 0x00173E9B File Offset: 0x0017209B
	private void OverrideName()
	{
		base.GetComponent<KSelectable>().SetName(EquippableFacade.GetNameOverride(base.GetComponent<Equippable>().def.Id, this.FacadeID));
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x00173EC3 File Offset: 0x001720C3
	public static string GetNameOverride(string defID, string facadeID)
	{
		if (facadeID.IsNullOrWhiteSpace())
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + defID.ToUpper() + ".NAME");
		}
		return Db.GetEquippableFacades().Get(facadeID).Name;
	}

	// Token: 0x04002923 RID: 10531
	[Serialize]
	private string _facadeID;

	// Token: 0x04002924 RID: 10532
	[Serialize]
	public string BuildOverride;
}
