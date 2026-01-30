using System;
using UnityEngine;

// Token: 0x02000770 RID: 1904
public class GlassForge : ComplexFabricator
{
	// Token: 0x0600306F RID: 12399 RVA: 0x00117A70 File Offset: 0x00115C70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<GlassForge>(-2094018600, GlassForge.CheckPipesDelegate);
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x00117A8C File Offset: 0x00115C8C
	private void CheckPipes(object _)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		int cell = Grid.OffsetCell(Grid.PosToCell(this), GlassForgeConfig.outPipeOffset);
		GameObject gameObject = Grid.Objects[cell, 16];
		if (!(gameObject != null))
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		if (gameObject.GetComponent<PrimaryElement>().Element.highTemp > ElementLoader.FindElementByHash(SimHashes.MoltenGlass).lowTemp)
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, null);
	}

	// Token: 0x04001CD2 RID: 7378
	private Guid statusHandle;

	// Token: 0x04001CD3 RID: 7379
	private static readonly EventSystem.IntraObjectHandler<GlassForge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<GlassForge>(delegate(GlassForge component, object data)
	{
		component.CheckPipes(data);
	});
}
