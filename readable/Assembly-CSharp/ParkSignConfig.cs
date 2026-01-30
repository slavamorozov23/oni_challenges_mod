using System;
using TUNING;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class ParkSignConfig : IBuildingConfig
{
	// Token: 0x060012CC RID: 4812 RVA: 0x0006D5C0 File Offset: 0x0006B7C0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ParkSign", 1, 2, "parksign_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.ANY_BUILDABLE, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		return buildingDef;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0006D61A File Offset: 0x0006B81A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Park, false);
		go.AddOrGet<ParkSign>();
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0006D634 File Offset: 0x0006B834
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BE3 RID: 3043
	public const string ID = "ParkSign";
}
