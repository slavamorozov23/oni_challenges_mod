using System;
using Database;

// Token: 0x02000590 RID: 1424
public class Blueprints_U53 : BlueprintProvider
{
	// Token: 0x06001FD9 RID: 8153 RVA: 0x000B6FD4 File Offset: 0x000B51D4
	public override void SetupBlueprints()
	{
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_hatch", "elegantbed_hatch_kanim");
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_pipsqueak", "elegantbed_pipsqueak_kanim");
	}
}
