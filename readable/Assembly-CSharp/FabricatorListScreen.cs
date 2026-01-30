using System;
using System.Collections.Generic;

// Token: 0x02000D0A RID: 3338
public class FabricatorListScreen : KToggleMenu
{
	// Token: 0x0600674A RID: 26442 RVA: 0x0026F338 File Offset: 0x0026D538
	private void Refresh()
	{
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Fabricator fabricator in Components.Fabricators.Items)
		{
			KSelectable component = fabricator.GetComponent<KSelectable>();
			list.Add(new KToggleMenu.ToggleInfo(component.GetName(), fabricator, global::Action.NumActions));
		}
		base.Setup(list);
	}

	// Token: 0x0600674B RID: 26443 RVA: 0x0026F3B4 File Offset: 0x0026D5B4
	protected override void OnSpawn()
	{
		base.onSelect += this.OnClickFabricator;
	}

	// Token: 0x0600674C RID: 26444 RVA: 0x0026F3C8 File Offset: 0x0026D5C8
	protected override void OnActivate()
	{
		base.OnActivate();
		this.Refresh();
	}

	// Token: 0x0600674D RID: 26445 RVA: 0x0026F3D8 File Offset: 0x0026D5D8
	private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
	{
		Fabricator fabricator = (Fabricator)toggle_info.userData;
		SelectTool.Instance.Select(fabricator.GetComponent<KSelectable>(), false);
	}
}
