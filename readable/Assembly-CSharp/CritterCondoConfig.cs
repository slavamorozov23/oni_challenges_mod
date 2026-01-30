using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class CritterCondoConfig : IBuildingConfig
{
	// Token: 0x060001F8 RID: 504 RVA: 0x0000E39C File Offset: 0x0000C59C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CritterCondo";
		int width = 3;
		int height = 3;
		string anim = "critter_condo_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			200f,
			10f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		return buildingDef;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000E436 File Offset: 0x0000C636
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000E438 File Offset: 0x0000C638
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000E43C File Offset: 0x0000C63C
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Effect effect = new Effect("InteractedWithCritterCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, false, false, true));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = ((CritterCondo.Instance condo_smi) => condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom() && !condo_smi.GetComponent<Floodable>().IsFlooded);
		def.moveToStatusItem = new StatusItem("CRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.interactStatusItem = new StatusItem("CRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.effectId = effect.Id;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000E582 File Offset: 0x0000C782
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000139 RID: 313
	public const string ID = "CritterCondo";

	// Token: 0x0400013A RID: 314
	public const float EFFECT_DURATION_IN_SECONDS = 600f;
}
