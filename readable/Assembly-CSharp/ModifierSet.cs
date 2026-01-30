using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public class ModifierSet : ScriptableObject
{
	// Token: 0x06001B30 RID: 6960 RVA: 0x00095880 File Offset: 0x00093A80
	public virtual void Initialize()
	{
		this.ResourceTable = new List<Resource>();
		this.Root = new ResourceSet<Resource>("Root", null);
		this.modifierInfos = new ModifierSet.ModifierInfos();
		this.modifierInfos.Load(this.modifiersFile);
		this.Attributes = new Database.Attributes(this.Root);
		this.BuildingAttributes = new BuildingAttributes(this.Root);
		this.CritterAttributes = new CritterAttributes(this.Root);
		this.PlantAttributes = new PlantAttributes(this.Root);
		this.effects = new ResourceSet<Effect>("Effects", this.Root);
		this.traits = new ModifierSet.TraitSet();
		this.traitGroups = new ModifierSet.TraitGroupSet();
		this.FertilityModifiers = new FertilityModifiers();
		this.MooSongModifiers = new MooSongModifiers();
		this.Amounts = new Database.Amounts();
		this.Amounts.Load();
		this.AttributeConverters = new Database.AttributeConverters();
		this.LoadEffects();
		this.LoadFertilityModifiers();
		this.LoadMooSongsModifiers();
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x0009597E File Offset: 0x00093B7E
	public static float ConvertValue(float value, Units units)
	{
		if (Units.PerDay == units)
		{
			return value * 0.0016666667f;
		}
		return value;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x00095990 File Offset: 0x00093B90
	private void LoadEffects()
	{
		foreach (ModifierSet.ModifierInfo modifierInfo in this.modifierInfos)
		{
			if (!this.effects.Exists(modifierInfo.Id) && (modifierInfo.Type == "Effect" || modifierInfo.Type == "Base" || modifierInfo.Type == "Need"))
			{
				string text = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.NAME", modifierInfo.Id.ToUpper()));
				string description = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.TOOLTIP", modifierInfo.Id.ToUpper()));
				Effect effect = new Effect(modifierInfo.Id, text, description, modifierInfo.Duration * 600f, modifierInfo.ShowInUI && modifierInfo.Type != "Need", modifierInfo.TriggerFloatingText, modifierInfo.IsBad, modifierInfo.EmoteAnim, modifierInfo.EmoteCooldown, modifierInfo.StompGroup, modifierInfo.CustomIcon);
				effect.stompPriority = modifierInfo.StompPriority;
				foreach (ModifierSet.ModifierInfo modifierInfo2 in this.modifierInfos)
				{
					if (modifierInfo2.Id == modifierInfo.Id)
					{
						effect.Add(new AttributeModifier(modifierInfo2.Attribute, ModifierSet.ConvertValue(modifierInfo2.Value, modifierInfo2.Units), text, modifierInfo2.Multiplier, false, true));
					}
				}
				this.effects.Add(effect);
			}
		}
		Reactable.ReactablePrecondition precon = delegate(GameObject go, Navigator.ActiveTransition n)
		{
			int cell = Grid.PosToCell(go);
			return Grid.IsValidCell(cell) && Grid.IsGas(cell);
		};
		this.effects.Get("WetFeet").AddEmotePrecondition(precon);
		this.effects.Get("SoakingWet").AddEmotePrecondition(precon);
		Effect effect2 = new Effect("PassedOutSleep", DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.TOOLTIP, 0f, true, true, true, null, 0f, null, true, "status_item_exhausted", -1f);
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.6666667f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, false, false, true));
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.033333335f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect resource = new Effect("WarmTouch", DUPLICANTS.MODIFIERS.WARMTOUCH.NAME, DUPLICANTS.MODIFIERS.WARMTOUCH.TOOLTIP, 120f, new string[]
		{
			"WetFeet"
		}, true, true, false, null, 0f, null, false, "", -1f);
		this.effects.Add(resource);
		Effect resource2 = new Effect("WarmTouchFood", DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.NAME, DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.TOOLTIP, 600f, new string[]
		{
			"WetFeet"
		}, true, true, false, null, 0f, null, false, "", -1f);
		this.effects.Add(resource2);
		Effect resource3 = new Effect("RefreshingTouch", DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.NAME, DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.TOOLTIP, 120f, true, true, false, null, -1f, 0f, null, "");
		this.effects.Add(resource3);
		Effect effect3 = new Effect("GunkSick", DUPLICANTS.MODIFIERS.GUNKSICK.NAME, DUPLICANTS.MODIFIERS.GUNKSICK.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.MODIFIERS.GUNKSICK.NAME, false, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("ExpellingGunk", DUPLICANTS.MODIFIERS.EXPELLINGGUNK.NAME, DUPLICANTS.MODIFIERS.EXPELLINGGUNK.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.083333336f, DUPLICANTS.MODIFIERS.GUNKSICK.NAME, false, false, true));
		this.effects.Add(effect4);
		Effect effect5 = new Effect("GunkHungover", DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, DUPLICANTS.MODIFIERS.GUNKHUNGOVER.TOOLTIP, 600f, true, false, true, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, false, false, true));
		this.effects.Add(effect5);
		Effect effect6 = new Effect("NoLubricationMinor", DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -4f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, false, false, true));
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.025f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, false, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("NoLubricationMajor", DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.NAME, DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -8f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.NAME, false, false, true));
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.05f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, false, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("BionicOffline", DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, DUPLICANTS.MODIFIERS.BIONICOFFLINE.TOOLTIP, 0f, false, true, true, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, 0f, DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, false, false, true));
		this.effects.Add(effect8);
		Effect effect9 = new Effect("BionicBedTimeEffect", DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.NAME, DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
		effect9.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.033333335f, DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.NAME, false, false, true));
		this.effects.Add(effect9);
		Effect effect10 = new Effect("BionicWaterStress", DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.NAME, DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect10.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.33333334f, DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.NAME, false, false, true));
		this.effects.Add(effect10);
		Effect resource4 = new Effect("RecentlySlippedTracker", DUPLICANTS.MODIFIERS.SLIPPED.NAME, DUPLICANTS.MODIFIERS.SLIPPED.TOOLTIP, 100f, false, false, true, null, -1f, 0f, null, "");
		this.effects.Add(resource4);
		foreach (Effect resource5 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Values)
		{
			this.effects.Add(resource5);
		}
		this.CreateRoomEffects();
		this.CreateCritteEffects();
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x00096284 File Offset: 0x00094484
	private void CreateRoomEffects()
	{
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x00096288 File Offset: 0x00094488
	private void CreateMosquitoEffects()
	{
		Effect effect = new Effect("MosquitoFed", STRINGS.CREATURES.MODIFIERS.MOSQUITO_FED.NAME, STRINGS.CREATURES.MODIFIERS.MOSQUITO_FED.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		float num = 0.4f;
		float value = 0.9f / num - 1f;
		effect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, value, effect.Name, true, false, true));
		this.effects.Add(effect);
		Effect effect2 = new Effect("DupeMosquitoBite", STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME, STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.TOOLTIP, 600f, true, true, true, null, -1f, 0f, null, "");
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME, false, false, true));
		effect2.Add(new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 5f, STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME, false, false, true));
		effect2.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -1f, STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect resource = new Effect("DupeMosquitoBiteSuppressed", STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE_SUPPRESSED.NAME, STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE_SUPPRESSED.TOOLTIP, 600f, false, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource);
		Effect effect3 = new Effect("CritterMosquitoBite", STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE.NAME, STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE.TOOLTIP, 300f, true, true, true, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE.NAME, false, false, true));
		this.effects.Add(effect3);
		Effect resource2 = new Effect("CritterMosquitoBiteSuppressed", STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE_SUPPRESSED.NAME, STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE_SUPPRESSED.TOOLTIP, 300f, false, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource2);
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x00096500 File Offset: 0x00094700
	public void CreateCritteEffects()
	{
		Effect effect = new Effect("Ranched", STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.09166667f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		this.effects.Add(effect);
		Effect effect2 = new Effect("HadMilk", STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, STRINGS.CREATURES.MODIFIERS.GOTMILK.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect2.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect effect3 = new Effect("EggSong", STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, true, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("EggHug", STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, STRINGS.CREATURES.MODIFIERS.EGGHUG.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 1f, STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, true, false, true));
		this.effects.Add(effect4);
		Effect resource = new Effect("HuggingFrenzy", STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.NAME, STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource);
		Effect effect5 = new Effect("DivergentCropTended", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true, false, true));
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.deltaAttribute.Id, 0.05f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true, false, true));
		this.effects.Add(effect5);
		Effect effect6 = new Effect("DivergentCropTendedWorm", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.5f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true, false, true));
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.deltaAttribute.Id, 0.5f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("MooWellFed", STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		effect7.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("HuskyMooFed", STRINGS.CREATURES.MODIFIERS.HUSKYMOOFED.NAME, STRINGS.CREATURES.MODIFIERS.HUSKYMOOFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, STRINGS.CREATURES.MODIFIERS.HUSKYMOOFED.NAME, false, false, true));
		this.effects.Add(effect8);
		Effect effect9 = new Effect("HuskyMooWellFed", STRINGS.CREATURES.MODIFIERS.HUSKYMOOWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.HUSKYMOOWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect9.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, () => GameUtil.SafeStringFormat(STRINGS.CREATURES.STATS.MILKPRODUCTION.DISPLAYED_NAME, new object[]
		{
			UI.StripLinkFormatting(ElementLoader.FindElementByHash(DieselMooConfig.MILK_ELEMENT).name)
		}), () => STRINGS.CREATURES.MODIFIERS.HUSKYMOOWELLFED.NAME, false, false));
		this.effects.Add(effect9);
		Effect effect10 = new Effect("WoodDeerWellFed", STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.DEERWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect10.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES * 600f), () => STRINGS.CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME()["WoodDeer"], () => STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME, false, false));
		this.effects.Add(effect10);
		Effect effect11 = new Effect("GlassDeerWellFed", STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.DEERWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect11.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 0.027777778f, () => STRINGS.CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME()["GlassDeer"], () => STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME, false, false));
		this.effects.Add(effect11);
		Effect effect12 = new Effect("IceBellyWellFed", STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect12.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), () => STRINGS.CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME()["IceBelly"], () => STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, false, false));
		this.effects.Add(effect12);
		Effect effect13 = new Effect("GoldBellyWellFed", STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect13.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 0.016666668f, () => STRINGS.CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME()["GoldBelly"], () => STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, false, false));
		this.effects.Add(effect13);
		Effect effect14 = new Effect("ButterflyPollinated", STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.NAME, STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect14.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.25f, STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.NAME, true, false, true));
		effect14.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.deltaAttribute.Id, 0.25f, STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.NAME, true, false, true));
		this.effects.Add(effect14);
		Effect resource2 = new Effect(PollinationMonitor.INITIALLY_POLLINATED_EFFECT, STRINGS.CREATURES.MODIFIERS.INITIALLYPOLLINATED.NAME, STRINGS.CREATURES.MODIFIERS.INITIALLYPOLLINATED.TOOLTIP, 600f, false, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource2);
		Effect effect15 = new Effect("RaptorWellFed", STRINGS.CREATURES.MODIFIERS.RAPTORWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.RAPTORWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect15.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (RaptorConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), () => STRINGS.CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME()["Raptor"], () => STRINGS.CREATURES.MODIFIERS.RAPTORWELLFED.NAME, false, false));
		this.effects.Add(effect15);
		Effect effect16 = new Effect("PredatorFailedHunt", STRINGS.CREATURES.MODIFIERS.HUNT_FAILED.NAME, STRINGS.CREATURES.MODIFIERS.HUNT_FAILED.TOOLTIP, 45f, true, false, true, null, -1f, 0f, null, "");
		effect16.tag = new Tag?(GameTags.Creatures.SuppressedDiet);
		this.effects.Add(effect16);
		Effect resource3 = new Effect("PreyEvadedHunt", STRINGS.CREATURES.MODIFIERS.EVADED_HUNT.NAME, STRINGS.CREATURES.MODIFIERS.EVADED_HUNT.TOOLTIP, 10f, true, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource3);
		this.CreateMosquitoEffects();
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x00096FF4 File Offset: 0x000951F4
	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait)
	{
		return this.CreateTrait(id, name, description, group_name, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait, null, null);
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x00097018 File Offset: 0x00095218
	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait, string[] requiredDlcIds, string[] forbiddenDlcIds)
	{
		Trait trait = new Trait(id, name, description, 0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait, requiredDlcIds, forbiddenDlcIds);
		this.traits.Add(trait);
		if (group_name == "" || group_name == null)
		{
			group_name = "Default";
		}
		TraitGroup traitGroup = this.traitGroups.TryGet(group_name);
		if (traitGroup == null)
		{
			traitGroup = new TraitGroup(group_name, group_name, group_name != "Default");
			this.traitGroups.Add(traitGroup);
		}
		traitGroup.Add(trait);
		return trait;
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000970A4 File Offset: 0x000952A4
	public FertilityModifier CreateFertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction)
	{
		FertilityModifier fertilityModifier = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
		this.FertilityModifiers.Add(fertilityModifier);
		return fertilityModifier;
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x000970D0 File Offset: 0x000952D0
	public MooSongModifier CreateMooSongModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, MooSongModifier.MooSongModFn applyFunction)
	{
		MooSongModifier mooSongModifier = new MooSongModifier(id, targetTag, name, description, tooltipCB, applyFunction);
		this.MooSongModifiers.Add(mooSongModifier);
		return mooSongModifier;
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000970FA File Offset: 0x000952FA
	protected void LoadTraits()
	{
		TRAITS.TRAIT_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x00097125 File Offset: 0x00095325
	protected void LoadFertilityModifiers()
	{
		TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x00097150 File Offset: 0x00095350
	protected void LoadMooSongsModifiers()
	{
		TUNING.CREATURES.MOO_SONG_MODIFIERS.MODIFIER_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x04000FB4 RID: 4020
	public TextAsset modifiersFile;

	// Token: 0x04000FB5 RID: 4021
	public ModifierSet.ModifierInfos modifierInfos;

	// Token: 0x04000FB6 RID: 4022
	public ModifierSet.TraitSet traits;

	// Token: 0x04000FB7 RID: 4023
	public ResourceSet<Effect> effects;

	// Token: 0x04000FB8 RID: 4024
	public ModifierSet.TraitGroupSet traitGroups;

	// Token: 0x04000FB9 RID: 4025
	public FertilityModifiers FertilityModifiers;

	// Token: 0x04000FBA RID: 4026
	public MooSongModifiers MooSongModifiers;

	// Token: 0x04000FBB RID: 4027
	public Database.Attributes Attributes;

	// Token: 0x04000FBC RID: 4028
	public BuildingAttributes BuildingAttributes;

	// Token: 0x04000FBD RID: 4029
	public CritterAttributes CritterAttributes;

	// Token: 0x04000FBE RID: 4030
	public PlantAttributes PlantAttributes;

	// Token: 0x04000FBF RID: 4031
	public Database.Amounts Amounts;

	// Token: 0x04000FC0 RID: 4032
	public Database.AttributeConverters AttributeConverters;

	// Token: 0x04000FC1 RID: 4033
	public ResourceSet Root;

	// Token: 0x04000FC2 RID: 4034
	public List<Resource> ResourceTable;

	// Token: 0x02001382 RID: 4994
	public class ModifierInfo : Resource
	{
		// Token: 0x04006B60 RID: 27488
		public string Type;

		// Token: 0x04006B61 RID: 27489
		public string Attribute;

		// Token: 0x04006B62 RID: 27490
		public float Value;

		// Token: 0x04006B63 RID: 27491
		public Units Units;

		// Token: 0x04006B64 RID: 27492
		public bool Multiplier;

		// Token: 0x04006B65 RID: 27493
		public float Duration;

		// Token: 0x04006B66 RID: 27494
		public bool ShowInUI;

		// Token: 0x04006B67 RID: 27495
		public string StompGroup;

		// Token: 0x04006B68 RID: 27496
		public int StompPriority;

		// Token: 0x04006B69 RID: 27497
		public bool IsBad;

		// Token: 0x04006B6A RID: 27498
		public string CustomIcon;

		// Token: 0x04006B6B RID: 27499
		public bool TriggerFloatingText;

		// Token: 0x04006B6C RID: 27500
		public string EmoteAnim;

		// Token: 0x04006B6D RID: 27501
		public float EmoteCooldown;
	}

	// Token: 0x02001383 RID: 4995
	[Serializable]
	public class ModifierInfos : ResourceLoader<ModifierSet.ModifierInfo>
	{
	}

	// Token: 0x02001384 RID: 4996
	[Serializable]
	public class TraitSet : ResourceSet<Trait>
	{
	}

	// Token: 0x02001385 RID: 4997
	[Serializable]
	public class TraitGroupSet : ResourceSet<TraitGroup>
	{
	}
}
