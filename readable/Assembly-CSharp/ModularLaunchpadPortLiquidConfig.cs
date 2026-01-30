using System;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class ModularLaunchpadPortLiquidConfig : IBuildingConfig
{
	// Token: 0x060011D2 RID: 4562 RVA: 0x000686F4 File Offset: 0x000668F4
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x000686FB File Offset: 0x000668FB
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquid", "conduit_port_liquid_loader_kanim", ConduitType.Liquid, true, 2, 2);
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00068710 File Offset: 0x00066910
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, true);
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x00068720 File Offset: 0x00066920
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000B36 RID: 2870
	public const string ID = "ModularLaunchpadPortLiquid";
}
