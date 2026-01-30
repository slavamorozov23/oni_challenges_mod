using System;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class ModularLaunchpadPortSolidUnloaderConfig : IBuildingConfig
{
	// Token: 0x060011E1 RID: 4577 RVA: 0x000687AB File Offset: 0x000669AB
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x000687B2 File Offset: 0x000669B2
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolidUnloader", "conduit_port_solid_unloader_kanim", ConduitType.Solid, false, 2, 3);
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x000687C7 File Offset: 0x000669C7
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, false);
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x000687D7 File Offset: 0x000669D7
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000B39 RID: 2873
	public const string ID = "ModularLaunchpadPortSolidUnloader";
}
