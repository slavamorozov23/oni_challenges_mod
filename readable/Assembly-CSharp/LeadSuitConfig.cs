using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class LeadSuitConfig : IEquipmentConfig, IHasDlcRestrictions
{
	// Token: 0x060002D4 RID: 724 RVA: 0x00014611 File Offset: 0x00012811
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00014618 File Offset: 0x00012818
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0001461C File Offset: 0x0001281C
	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> list = new List<AttributeModifier>();
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float)TUNING.EQUIPMENT.SUITS.LEADSUIT_ATHLETICS, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.LEADSUIT_SCALDING, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.ScoldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.LEADSUIT_SCOLDING, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TUNING.EQUIPMENT.SUITS.LEADSUIT_RADIATION_SHIELDING, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.Strength.Id, (float)TUNING.EQUIPMENT.SUITS.LEADSUIT_STRENGTH, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float)TUNING.EQUIPMENT.SUITS.LEADSUIT_INSULATION, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, TUNING.EQUIPMENT.SUITS.LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.NAME, false, false, true));
		this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float)(-(float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS), Db.Get().Skills.Suits1.Name, false, false, true);
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Lead_Suit", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Dirt, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS, "suit_leadsuit_kanim", "", "body_leadsuit_kanim", 6, list, null, true, EntityTemplates.CollisionShape.CIRCLE, 0.325f, 0.325f, new Tag[]
		{
			GameTags.Suit,
			GameTags.Clothes
		}, null);
		equipmentDef.wornID = "Worn_Lead_Suit";
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC;
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("SoakingWet"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("ColdAir"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WarmAir"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("PoppedEarDrums"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("RecentlySlippedTracker"));
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				Navigator component = targetGameObject.GetComponent<Navigator>();
				if (component != null)
				{
					component.SetFlags(PathFinder.PotentialPath.Flags.HasLeadSuit);
				}
				MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
				if (component2 != null && component2.HasPerk(Db.Get().SkillPerks.ExosuitExpertise.Id))
				{
					targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
				}
				targetGameObject.AddTag(GameTags.HasAirtightSuit);
			}
		};
		equipmentDef.OnUnequipCallBack = delegate(Equippable eq)
		{
			if (eq.assignee != null)
			{
				Ownables soleOwner = eq.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					if (targetGameObject)
					{
						Attributes attributes = targetGameObject.GetAttributes();
						if (attributes != null)
						{
							attributes.Get(Db.Get().Attributes.Athletics).Remove(this.expertAthleticsModifier);
						}
						Navigator component = targetGameObject.GetComponent<Navigator>();
						if (component != null)
						{
							component.ClearFlags(PathFinder.PotentialPath.Flags.HasLeadSuit);
						}
						Effects component2 = targetGameObject.GetComponent<Effects>();
						if (component2 != null && component2.HasEffect("SoiledSuit"))
						{
							component2.Remove("SoiledSuit");
						}
						targetGameObject.RemoveTag(GameTags.HasAirtightSuit);
					}
					Tag elementTag = eq.GetComponent<SuitTank>().elementTag;
					eq.GetComponent<Storage>().DropUnlessHasTag(elementTag);
				}
			}
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Lead_Suit");
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
		return equipmentDef;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x000148FC File Offset: 0x00012AFC
	public void DoPostConfigure(GameObject go)
	{
		SuitTank suitTank = go.AddComponent<SuitTank>();
		suitTank.element = "Oxygen";
		suitTank.capacity = DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 400f;
		suitTank.elementTag = GameTags.Breathable;
		suitTank.SafeCellFlagsToIgnoreOnEquipped = (SafeCellQuery.SafeFlags)496;
		go.AddComponent<LeadSuitTank>().batteryDuration = 200f;
		go.AddComponent<HelmetController>();
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Clothes, false);
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.AddTag(GameTags.AirtightSuit, false);
		Durability durability = go.AddComponent<Durability>();
		durability.wornEquipmentPrefabID = "Worn_Lead_Suit";
		durability.durabilityLossPerCycle = TUNING.EQUIPMENT.SUITS.ATMOSUIT_DECAY;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.showInUI = true;
		go.AddOrGet<AtmoSuit>();
		go.AddComponent<SuitDiseaseHandler>();
	}

	// Token: 0x040001A2 RID: 418
	public const string ID = "Lead_Suit";

	// Token: 0x040001A3 RID: 419
	public const string WORN_ID = "Worn_Lead_Suit";

	// Token: 0x040001A4 RID: 420
	public static ComplexRecipe recipe;

	// Token: 0x040001A5 RID: 421
	private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasLeadSuit;

	// Token: 0x040001A6 RID: 422
	private AttributeModifier expertAthleticsModifier;
}
