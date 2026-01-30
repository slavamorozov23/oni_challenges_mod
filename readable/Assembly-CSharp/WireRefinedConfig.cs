using System;
using TUNING;
using UnityEngine;

// Token: 0x0200046A RID: 1130
public class WireRefinedConfig : BaseWireConfig
{
	// Token: 0x060017A5 RID: 6053 RVA: 0x000860D4 File Offset: 0x000842D4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefined";
		string anim = "utilities_electric_conduct_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.NONE, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		return buildingDef;
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x00086117 File Offset: 0x00084317
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max2000, go);
	}

	// Token: 0x04000DE0 RID: 3552
	public const string ID = "WireRefined";
}
