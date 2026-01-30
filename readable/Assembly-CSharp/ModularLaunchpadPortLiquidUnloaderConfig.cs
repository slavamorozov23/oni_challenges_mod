using System;
using UnityEngine;

// Token: 0x02000358 RID: 856
public class ModularLaunchpadPortLiquidUnloaderConfig : IBuildingConfig
{
	// Token: 0x060011D7 RID: 4567 RVA: 0x00068731 File Offset: 0x00066931
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x00068738 File Offset: 0x00066938
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquidUnloader", "conduit_port_liquid_unloader_kanim", ConduitType.Liquid, false, 2, 3);
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x0006874D File Offset: 0x0006694D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, false);
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x0006875D File Offset: 0x0006695D
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000B37 RID: 2871
	public const string ID = "ModularLaunchpadPortLiquidUnloader";
}
