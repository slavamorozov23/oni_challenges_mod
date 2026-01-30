using System;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class ModularLaunchpadPortGasConfig : IBuildingConfig
{
	// Token: 0x060011C8 RID: 4552 RVA: 0x0006867A File Offset: 0x0006687A
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00068681 File Offset: 0x00066881
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGas", "conduit_port_gas_loader_kanim", ConduitType.Gas, true, 2, 2);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00068696 File Offset: 0x00066896
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, true);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x000686A6 File Offset: 0x000668A6
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000B34 RID: 2868
	public const string ID = "ModularLaunchpadPortGas";
}
