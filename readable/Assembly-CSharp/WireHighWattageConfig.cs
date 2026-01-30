using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000467 RID: 1127
public class WireHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001798 RID: 6040 RVA: 0x00085EF0 File Offset: 0x000840F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighWattageWire";
		string anim = "utilities_electric_insulated_kanim";
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, TUNING.BUILDINGS.DECOR.PENALTY.TIER5, none);
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x00085F4F File Offset: 0x0008414F
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x00085F59 File Offset: 0x00084159
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x04000DDD RID: 3549
	public const string ID = "HighWattageWire";
}
