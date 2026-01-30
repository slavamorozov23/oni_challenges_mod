using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class UnderwaterCritterCondoConfig : IBuildingConfig
{
	// Token: 0x06001709 RID: 5897 RVA: 0x00083454 File Offset: 0x00081654
	public override BuildingDef CreateBuildingDef()
	{
		string id = "UnderwaterCritterCondo";
		int width = 3;
		int height = 3;
		string anim = "underwater_critter_condo_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			200f
		};
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.Floodable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WATER);
		return buildingDef;
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x000834EC File Offset: 0x000816EC
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x000834EE File Offset: 0x000816EE
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x000834F0 File Offset: 0x000816F0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Submergable>();
		Effect effect = new Effect("InteractedWithUnderwaterCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.UNDERWATERCRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, false, false, true));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = delegate(CritterCondo.Instance condo_smi)
		{
			Building component = condo_smi.GetComponent<Building>();
			for (int i = 0; i < component.PlacementCells.Length; i++)
			{
				if (!Grid.IsLiquid(component.PlacementCells[i]))
				{
					return false;
				}
			}
			return true;
		};
		def.UpdateForegroundVisibilitySymbols = delegate(KBatchedAnimController foreground_controller, bool is_large_critter)
		{
			if (foreground_controller != null)
			{
				foreground_controller.SetSymbolVisiblity("doorway_fg", !is_large_critter);
				foreground_controller.SetSymbolVisiblity("condo_fg", is_large_critter);
			}
		};
		def.moveToStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.interactStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.condoTag = "UnderwaterCritterCondo";
		def.effectId = effect.Id;
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x0008363B File Offset: 0x0008183B
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000DA0 RID: 3488
	public const string ID = "UnderwaterCritterCondo";

	// Token: 0x04000DA1 RID: 3489
	public static readonly Operational.Flag Submerged = new Operational.Flag("Submerged", Operational.Flag.Type.Requirement);
}
