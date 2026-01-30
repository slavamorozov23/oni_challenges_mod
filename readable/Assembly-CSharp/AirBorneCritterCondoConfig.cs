using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class AirBorneCritterCondoConfig : IBuildingConfig
{
	// Token: 0x0600005F RID: 95 RVA: 0x00004814 File Offset: 0x00002A14
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AirBorneCritterCondo";
		int width = 3;
		int height = 3;
		string anim = "critter_condo_airborne_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			200f
		};
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		return buildingDef;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000048A0 File Offset: 0x00002AA0
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000048A2 File Offset: 0x00002AA2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000048A4 File Offset: 0x00002AA4
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Effect effect = new Effect("InteractedWithAirborneCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.AIRBORNECRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, false, false, true));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = ((CritterCondo.Instance condo_smi) => condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom());
		def.moveToStatusItem = new StatusItem("AIRBORNECRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.interactStatusItem = new StatusItem("AIRBORNECRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.condoTag = "AirBorneCritterCondo";
		def.effectId = effect.Id;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000049FA File Offset: 0x00002BFA
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000053 RID: 83
	public const string ID = "AirBorneCritterCondo";
}
