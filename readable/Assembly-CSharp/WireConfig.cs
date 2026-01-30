using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class WireConfig : BaseWireConfig
{
	// Token: 0x06001795 RID: 6037 RVA: 0x00085E84 File Offset: 0x00084084
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Wire";
		string anim = "utilities_electric_kanim";
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none);
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x00085EDC File Offset: 0x000840DC
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
	}

	// Token: 0x04000DDC RID: 3548
	public const string ID = "Wire";
}
