using System;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class ModularLaunchpadPortGasUnloaderConfig : IBuildingConfig
{
	// Token: 0x060011CD RID: 4557 RVA: 0x000686B7 File Offset: 0x000668B7
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x000686BE File Offset: 0x000668BE
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGasUnloader", "conduit_port_gas_unloader_kanim", ConduitType.Gas, false, 2, 3);
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x000686D3 File Offset: 0x000668D3
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, false);
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x000686E3 File Offset: 0x000668E3
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000B35 RID: 2869
	public const string ID = "ModularLaunchpadPortGasUnloader";
}
