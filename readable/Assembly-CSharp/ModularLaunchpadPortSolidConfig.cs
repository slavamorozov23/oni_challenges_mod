using System;
using UnityEngine;

// Token: 0x02000359 RID: 857
public class ModularLaunchpadPortSolidConfig : IBuildingConfig
{
	// Token: 0x060011DC RID: 4572 RVA: 0x0006876E File Offset: 0x0006696E
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00068775 File Offset: 0x00066975
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolid", "conduit_port_solid_loader_kanim", ConduitType.Solid, true, 2, 2);
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x0006878A File Offset: 0x0006698A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, true);
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x0006879A File Offset: 0x0006699A
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000B38 RID: 2872
	public const string ID = "ModularLaunchpadPortSolid";
}
