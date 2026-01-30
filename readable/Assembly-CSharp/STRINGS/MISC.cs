using System;

namespace STRINGS
{
	// Token: 0x02000FFD RID: 4093
	public class MISC
	{
		// Token: 0x0200266A RID: 9834
		public class TAGS
		{
			// Token: 0x0400AB72 RID: 43890
			public static LocString OTHER = "Miscellaneous";

			// Token: 0x0400AB73 RID: 43891
			public static LocString FILTER = UI.FormatAsLink("Filtration Medium", "FILTER");

			// Token: 0x0400AB74 RID: 43892
			public static LocString FILTER_DESC = string.Concat(new string[]
			{
				"Filtration mediums are materials used to separate purified ",
				UI.FormatAsLink("gases", "ELEMENTS_GAS"),
				" or ",
				UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
				" from their polluted forms.\n\nThey are consumables that will be transformed by the filtering process. For example, ",
				UI.FormatAsLink("Sand", "SAND"),
				" that has been used to filter ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				" will become ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				"."
			});

			// Token: 0x0400AB75 RID: 43893
			public static LocString ICEORE = UI.FormatAsLink("Ice", "ICEORE");

			// Token: 0x0400AB76 RID: 43894
			public static LocString ICEORE_DESC = string.Concat(new string[]
			{
				"Ice is a class of materials made up mostly (if not completely) of ",
				UI.FormatAsLink("Water", "WATER"),
				" in a frozen or partially frozen form.\n\nAs a material in a frigid solid or semi-solid state, these elements are very useful as a low-cost way to cool the environment around them.\n\nWhen heated, ice will melt into its original liquified form (ie.",
				UI.FormatAsLink("Brine Ice", "BRINEICE"),
				" will liquify into ",
				UI.FormatAsLink("Brine", "BRINE"),
				"). Each ice element has a different freezing and melting point based upon its composition and state."
			});

			// Token: 0x0400AB77 RID: 43895
			public static LocString PHOSPHORUS = UI.FormatAsLink("Phosphorus", "PHOSPHORUS");

			// Token: 0x0400AB78 RID: 43896
			public static LocString BUILDABLERAW = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW");

			// Token: 0x0400AB79 RID: 43897
			public static LocString BUILDABLERAW_DESC = string.Concat(new string[]
			{
				"Raw minerals are the unrefined forms of organic solids. Almost all raw minerals can be processed in the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				", although a handful require the use of the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400AB7A RID: 43898
			public static LocString BUILDABLEPROCESSED = UI.FormatAsLink("Refined Mineral", "BUILDABLEPROCESSED");

			// Token: 0x0400AB7B RID: 43899
			public static LocString BUILDABLEANY = UI.FormatAsLink("General Buildable", "BUILDABLEANY");

			// Token: 0x0400AB7C RID: 43900
			public static LocString BUILDABLEANY_DESC = "";

			// Token: 0x0400AB7D RID: 43901
			public static LocString DEHYDRATED = "Dehydrated";

			// Token: 0x0400AB7E RID: 43902
			public static LocString FOSSILS = UI.FormatAsLink("Fossil", "FOSSILS");

			// Token: 0x0400AB7F RID: 43903
			public static LocString FOSSILS_DESC = "Fossil is a category of composite rocks and minerals that contain traces of petrified lifeforms.\n\nThey have varied uses as basic building materials, sculpting blocks, or raw ingredients in the production of higher-grade materials.";

			// Token: 0x0400AB80 RID: 43904
			public static LocString PLASTIFIABLELIQUID = UI.FormatAsLink("Plastic Monomer", "PLASTIFIABLELIQUID");

			// Token: 0x0400AB81 RID: 43905
			public static LocString PLASTIFIABLELIQUID_DESC = string.Concat(new string[]
			{
				"Plastic monomers are organic compounds that can be processed into ",
				UI.FormatAsLink("Plastics", "PLASTIC"),
				" that have valuable applications as advanced building materials.\n\nPlastics derived from these monomers can also be used as packaging materials for ",
				UI.FormatAsLink("Food", "FOOD"),
				" preservation."
			});

			// Token: 0x0400AB82 RID: 43906
			public static LocString UNREFINEDOIL = UI.FormatAsLink("Unrefined Oil", "UNREFINEDOIL");

			// Token: 0x0400AB83 RID: 43907
			public static LocString UNREFINEDOIL_DESC = "Oils in their raw, minimally processed forms. They can be used as industrial lubricants or refined for other applications at designated buildings.";

			// Token: 0x0400AB84 RID: 43908
			public static LocString REFINEDMETAL = UI.FormatAsLink("Refined Metal", "REFINEDMETAL");

			// Token: 0x0400AB85 RID: 43909
			public static LocString REFINEDMETAL_DESC = string.Concat(new string[]
			{
				"Refined metals are purified forms of metal often used in higher-tier electronics due to their tendency to be able to withstand higher temperatures when they are made into wires. Other benefits include the increased decor value for some metals which can greatly improve the well-being of a colony.\n\nMetal ore can be refined in either the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				"."
			});

			// Token: 0x0400AB86 RID: 43910
			public static LocString METAL = UI.FormatAsLink("Metal Ore", "METAL");

			// Token: 0x0400AB87 RID: 43911
			public static LocString METAL_DESC = string.Concat(new string[]
			{
				"Metal ore is the raw form of metal, and has a wide variety of practical applications in electronics and general construction.\n\nMetal ore is typically processed into ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" using the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				".\n\nSome rare metal ores can also be refined in the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400AB88 RID: 43912
			public static LocString PRECIOUSMETAL = UI.FormatAsLink("Precious Metal", "PRECIOUSMETAL");

			// Token: 0x0400AB89 RID: 43913
			public static LocString RAWPRECIOUSMETAL = "Precious Metal Ore";

			// Token: 0x0400AB8A RID: 43914
			public static LocString PRECIOUSROCK = UI.FormatAsLink("Precious Rock", "PRECIOUSROCK");

			// Token: 0x0400AB8B RID: 43915
			public static LocString PRECIOUSROCK_DESC = "Precious rocks are raw minerals. Their extreme hardness produces durable " + UI.FormatAsLink("Decor", "DECOR") + ".\n\nSome precious rocks are inherently attractive even in their natural, unfinished form.";

			// Token: 0x0400AB8C RID: 43916
			public static LocString ALLOY = UI.FormatAsLink("Alloy", "ALLOY");

			// Token: 0x0400AB8D RID: 43917
			public static LocString BUILDINGFIBER = UI.FormatAsLink("Fibers", "BUILDINGFIBER");

			// Token: 0x0400AB8E RID: 43918
			public static LocString BUILDINGFIBER_DESC = "Fibers are organically sourced polymers which are both sturdy and sensorially pleasant, making them suitable in the construction of " + UI.FormatAsLink("Morale", "MORALE") + "-boosting buildings.";

			// Token: 0x0400AB8F RID: 43919
			public static LocString BUILDINGWOOD = UI.FormatAsLink("Wood", "BUILDINGWOOD");

			// Token: 0x0400AB90 RID: 43920
			public static LocString BUILDINGWOOD_DESC = string.Concat(new string[]
			{
				"Wood is a renewable building material which can also be used as a valuable source of fuel and electricity when refined at the ",
				UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR"),
				" or the ",
				UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY"),
				"."
			});

			// Token: 0x0400AB91 RID: 43921
			public static LocString CRUSHABLE = "Crushable";

			// Token: 0x0400AB92 RID: 43922
			public static LocString CROPSEEDS = "Crop Seeds";

			// Token: 0x0400AB93 RID: 43923
			public static LocString CROPSEED = "Crop Seed";

			// Token: 0x0400AB94 RID: 43924
			public static LocString WATERSEED = "Aquatic Seed";

			// Token: 0x0400AB95 RID: 43925
			public static LocString DECORSEED = "Decor Seed";

			// Token: 0x0400AB96 RID: 43926
			public static LocString CERAMIC = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400AB97 RID: 43927
			public static LocString POLYPROPYLENE = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400AB98 RID: 43928
			public static LocString BAGABLECREATURE = UI.FormatAsLink("Critter", "CREATURES");

			// Token: 0x0400AB99 RID: 43929
			public static LocString SWIMMINGCREATURE = "Aquatic Critter";

			// Token: 0x0400AB9A RID: 43930
			public static LocString LIFE = "Life";

			// Token: 0x0400AB9B RID: 43931
			public static LocString LIQUIFIABLE = "Liquefiable";

			// Token: 0x0400AB9C RID: 43932
			public static LocString LIQUID = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID");

			// Token: 0x0400AB9D RID: 43933
			public static LocString LUBRICATINGOIL = UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL");

			// Token: 0x0400AB9E RID: 43934
			public static LocString LUBRICATINGOIL_DESC = "Gear oils are lubricating fluids useful in the maintenance of complex machinery, protecting gear systems from damage and minimizing friction between moving parts to support optimal performance.";

			// Token: 0x0400AB9F RID: 43935
			public static LocString REMOTEOPERABLE = UI.FormatAsLink("Remote Workable", "REMOTEOPERABLE");

			// Token: 0x0400ABA0 RID: 43936
			public static LocString REMOTEOPERABLE_DESC = string.Concat(new string[]
			{
				"These buildings can be operated from a distance by a ",
				UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL"),
				" so long as they are built within range of a ",
				UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK"),
				"."
			});

			// Token: 0x0400ABA1 RID: 43937
			public static LocString SLIPPERY = UI.FormatAsLink("Slippery", "SLIPPERY");

			// Token: 0x0400ABA2 RID: 43938
			public static LocString SLIPPERY_DESC = string.Concat(new string[]
			{
				"Some ",
				UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
				" and ",
				UI.FormatAsLink("solids", "ELEMENTS_SOLID"),
				" have a remarkably low coefficient of friction. These present a safety hazard if left on the ground, as Duplicants may slip and fall when traveling across them."
			});

			// Token: 0x0400ABA3 RID: 43939
			public static LocString LEAD = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400ABA4 RID: 43940
			public static LocString CHARGEDPORTABLEBATTERY = UI.FormatAsLink("Power Banks", "ELECTROBANK");

			// Token: 0x0400ABA5 RID: 43941
			public static LocString EMPTYPORTABLEBATTERY = UI.FormatAsLink("Empty Eco Power Banks", "ELECTROBANK_EMPTY");

			// Token: 0x0400ABA6 RID: 43942
			public static LocString SPECIAL = "Special";

			// Token: 0x0400ABA7 RID: 43943
			public static LocString FARMABLE = UI.FormatAsLink("Cultivable Soil", "FARMABLE");

			// Token: 0x0400ABA8 RID: 43944
			public static LocString FARMABLE_DESC = "Cultivable soil is a fundamental building block of basic agricultural systems and can also be useful in the production of clean " + UI.FormatAsLink("Oxygen", "OXYGEN") + ".";

			// Token: 0x0400ABA9 RID: 43945
			public static LocString AGRICULTURE = UI.FormatAsLink("Agriculture", "AGRICULTURE");

			// Token: 0x0400ABAA RID: 43946
			public static LocString COAL = "Coal";

			// Token: 0x0400ABAB RID: 43947
			public static LocString BLEACHSTONE = "Bleach Stone";

			// Token: 0x0400ABAC RID: 43948
			public static LocString ORGANICS = "Organic";

			// Token: 0x0400ABAD RID: 43949
			public static LocString ORGANICS_DESC = string.Concat(new string[]
			{
				"Organic materials are useful ingredients gathered from the environment, harvested from ",
				UI.FormatAsLink("plants", "PLANTS"),
				" or derived from ",
				UI.FormatAsLink("critters", "CREATURES"),
				".\n\nSome, not all, require additional processing before they can be used."
			});

			// Token: 0x0400ABAE RID: 43950
			public static LocString CONSUMABLEORE = "Consumable Ore";

			// Token: 0x0400ABAF RID: 43951
			public static LocString SUBLIMATING = UI.FormatAsLink("Sublimator", "SUBLIMATES");

			// Token: 0x0400ABB0 RID: 43952
			public static LocString SUBLIMATING_SUBHEADER = "Off-Gassing Elements";

			// Token: 0x0400ABB1 RID: 43953
			public static LocString SUBLIMATING_DESC = string.Concat(new string[]
			{
				"Sublimators are a class of ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" elements that passively convert to a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state. When off-gassing is complete, no trace of the original solid remains.\n\nThis passive conversion persists when the element is left in storage."
			});

			// Token: 0x0400ABB2 RID: 43954
			public static LocString ORE = "Ore";

			// Token: 0x0400ABB3 RID: 43955
			public static LocString BREATHABLE = "Breathable Gas";

			// Token: 0x0400ABB4 RID: 43956
			public static LocString UNBREATHABLE = "Unbreathable Gas";

			// Token: 0x0400ABB5 RID: 43957
			public static LocString GAS = "Gas";

			// Token: 0x0400ABB6 RID: 43958
			public static LocString BURNS = "Flammable";

			// Token: 0x0400ABB7 RID: 43959
			public static LocString UNSTABLE = "Unstable";

			// Token: 0x0400ABB8 RID: 43960
			public static LocString TOXIC = "Toxic";

			// Token: 0x0400ABB9 RID: 43961
			public static LocString MIXTURE = "Mixture";

			// Token: 0x0400ABBA RID: 43962
			public static LocString SOLID = UI.FormatAsLink("Solid", "ELEMENTS_SOLID");

			// Token: 0x0400ABBB RID: 43963
			public static LocString FLYINGCRITTEREDIBLE = "Bait";

			// Token: 0x0400ABBC RID: 43964
			public static LocString INDUSTRIALPRODUCT = "Industrial Product";

			// Token: 0x0400ABBD RID: 43965
			public static LocString INDUSTRIALINGREDIENT = UI.FormatAsLink("Industrial Ingredient", "INDUSTRIALINGREDIENT");

			// Token: 0x0400ABBE RID: 43966
			public static LocString TECHCOMPONENTS = UI.FormatAsLink("Tech Component", "TECHCOMPONENTS");

			// Token: 0x0400ABBF RID: 43967
			public static LocString ORNAMENT = UI.FormatAsLink("Ornament", "ORNAMENT");

			// Token: 0x0400ABC0 RID: 43968
			public static LocString MEDICALSUPPLIES = "Medical Supplies";

			// Token: 0x0400ABC1 RID: 43969
			public static LocString CLOTHES = UI.FormatAsLink("Clothing", "EQUIPMENT");

			// Token: 0x0400ABC2 RID: 43970
			public static LocString EMITSLIGHT = UI.FormatAsLink("Light Emitter", "LIGHT");

			// Token: 0x0400ABC3 RID: 43971
			public static LocString BED = "Beds";

			// Token: 0x0400ABC4 RID: 43972
			public static LocString MESSSTATION = "Dining Tables";

			// Token: 0x0400ABC5 RID: 43973
			public static LocString TOY = "Toy";

			// Token: 0x0400ABC6 RID: 43974
			public static LocString SUIT = "Suits";

			// Token: 0x0400ABC7 RID: 43975
			public static LocString MULTITOOL = "Multitool";

			// Token: 0x0400ABC8 RID: 43976
			public static LocString CLINIC = "Clinic";

			// Token: 0x0400ABC9 RID: 43977
			public static LocString RELAXATION_POINT = "Leisure Area";

			// Token: 0x0400ABCA RID: 43978
			public static LocString SOLIDMATERIAL = "Solid Material";

			// Token: 0x0400ABCB RID: 43979
			public static LocString EXTRUDABLE = "Extrudable";

			// Token: 0x0400ABCC RID: 43980
			public static LocString PLUMBABLE = UI.FormatAsLink("Plumbable", "PLUMBABLE");

			// Token: 0x0400ABCD RID: 43981
			public static LocString PLUMBABLE_DESC = "";

			// Token: 0x0400ABCE RID: 43982
			public static LocString COMPOSTABLE = UI.FormatAsLink("Compostable", "COMPOSTABLE");

			// Token: 0x0400ABCF RID: 43983
			public static LocString COMPOSTABLE_SUBHEADER = "Recyclable Organics";

			// Token: 0x0400ABD0 RID: 43984
			public static LocString COMPOSTABLE_DESC = string.Concat(new string[]
			{
				"Compostables are biological materials which can be put into a ",
				UI.FormatAsLink("Compost", "COMPOST"),
				" to generate clean ",
				UI.FormatAsLink("Dirt", "DIRT"),
				".\n\nComposting also generates a small amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				".\n\nOnce it starts to rot, consumable food should be composted to prevent ",
				DUPLICANTS.DISEASES.FOODPOISONING.NAME,
				"."
			});

			// Token: 0x0400ABD1 RID: 43985
			public static LocString COMPOSTBASICPLANTFOOD = "Compost Muckroot";

			// Token: 0x0400ABD2 RID: 43986
			public static LocString EDIBLE = "Edible";

			// Token: 0x0400ABD3 RID: 43987
			public static LocString OXIDIZER = "Oxidizer";

			// Token: 0x0400ABD4 RID: 43988
			public static LocString COOKINGINGREDIENT = "Cooking Ingredient";

			// Token: 0x0400ABD5 RID: 43989
			public static LocString MEDICINE = "Medicine";

			// Token: 0x0400ABD6 RID: 43990
			public static LocString SEED = "Seed";

			// Token: 0x0400ABD7 RID: 43991
			public static LocString ANYWATER = "Water Based";

			// Token: 0x0400ABD8 RID: 43992
			public static LocString MARKEDFORCOMPOST = "Marked For Compost";

			// Token: 0x0400ABD9 RID: 43993
			public static LocString MARKEDFORCOMPOSTINSTORAGE = "In Compost Storage";

			// Token: 0x0400ABDA RID: 43994
			public static LocString COMPOSTMEAT = "Compost Meat";

			// Token: 0x0400ABDB RID: 43995
			public static LocString PICKLED = "Pickled";

			// Token: 0x0400ABDC RID: 43996
			public static LocString PLASTIC = UI.FormatAsLink("Plastics", "PLASTIC");

			// Token: 0x0400ABDD RID: 43997
			public static LocString PLASTIC_DESC = string.Concat(new string[]
			{
				"Plastics are synthetic ",
				UI.FormatAsLink("Solids", "ELEMENTSSOLID"),
				" that are pliable and minimize the transfer of ",
				UI.FormatAsLink("Heat", "Heat"),
				". They typically have a low melting point, although more advanced plastics have been developed to circumvent this issue."
			});

			// Token: 0x0400ABDE RID: 43998
			public static LocString TOILET = "Toilets";

			// Token: 0x0400ABDF RID: 43999
			public static LocString MASSAGE_TABLE = "Massage Tables";

			// Token: 0x0400ABE0 RID: 44000
			public static LocString POWERSTATION = "Power Station";

			// Token: 0x0400ABE1 RID: 44001
			public static LocString FARMSTATION = "Farm Station";

			// Token: 0x0400ABE2 RID: 44002
			public static LocString MACHINE_SHOP = "Machine Shop";

			// Token: 0x0400ABE3 RID: 44003
			public static LocString ANTISEPTIC = "Antiseptic";

			// Token: 0x0400ABE4 RID: 44004
			public static LocString OIL = "Hydrocarbon";

			// Token: 0x0400ABE5 RID: 44005
			public static LocString DECORATION = "Decoration";

			// Token: 0x0400ABE6 RID: 44006
			public static LocString EGG = "Critter Egg";

			// Token: 0x0400ABE7 RID: 44007
			public static LocString EGGSHELL = "Egg Shell";

			// Token: 0x0400ABE8 RID: 44008
			public static LocString MANUFACTUREDMATERIAL = "Manufactured Material";

			// Token: 0x0400ABE9 RID: 44009
			public static LocString STEEL = "Steel";

			// Token: 0x0400ABEA RID: 44010
			public static LocString RAW = "Raw Animal Product";

			// Token: 0x0400ABEB RID: 44011
			public static LocString FOSSIL = "Fossil";

			// Token: 0x0400ABEC RID: 44012
			public static LocString ICE = "Ice";

			// Token: 0x0400ABED RID: 44013
			public static LocString ANY = "Any";

			// Token: 0x0400ABEE RID: 44014
			public static LocString TRANSPARENT = "Transparent";

			// Token: 0x0400ABEF RID: 44015
			public static LocString TRANSPARENT_DESC = string.Concat(new string[]
			{
				"Transparent materials allow ",
				UI.FormatAsLink("Light", "LIGHT"),
				" to pass through. Illumination boosts Duplicant productivity during working hours, but undermines sleep quality.\n\nTransparency is also important for buildings that require a clear line of sight in order to function correctly, such as the ",
				UI.FormatAsLink("Space Scanner", "COMETDETECTOR"),
				"."
			});

			// Token: 0x0400ABF0 RID: 44016
			public static LocString RAREMATERIALS = "Rare Resource";

			// Token: 0x0400ABF1 RID: 44017
			public static LocString FARMINGMATERIAL = "Fertilizer";

			// Token: 0x0400ABF2 RID: 44018
			public static LocString INSULATOR = UI.FormatAsLink("Insulator", "INSULATOR");

			// Token: 0x0400ABF3 RID: 44019
			public static LocString INSULATOR_DESC = "Insulators have low thermal conductivity, and effectively reduce the speed at which " + UI.FormatAsLink("Heat", "Heat") + " is transferred through them.";

			// Token: 0x0400ABF4 RID: 44020
			public static LocString RAILGUNPAYLOADEMPTYABLE = "Payload";

			// Token: 0x0400ABF5 RID: 44021
			public static LocString NONCRUSHABLE = "Uncrushable";

			// Token: 0x0400ABF6 RID: 44022
			public static LocString STORYTRAITRESOURCE = "Story Trait";

			// Token: 0x0400ABF7 RID: 44023
			public static LocString GLASS = "Glass";

			// Token: 0x0400ABF8 RID: 44024
			public static LocString OBSIDIAN = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400ABF9 RID: 44025
			public static LocString DIAMOND = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400ABFA RID: 44026
			public static LocString SNOW = UI.FormatAsLink("Snow", "STABLESNOW");

			// Token: 0x0400ABFB RID: 44027
			public static LocString WOODLOG = UI.FormatAsLink("Wood", "WOODLOG");

			// Token: 0x0400ABFC RID: 44028
			public static LocString OXYGENCANISTER = "Oxygen Canister";

			// Token: 0x0400ABFD RID: 44029
			public static LocString ORNAMENTDISPLAYED = "Ornament";

			// Token: 0x0400ABFE RID: 44030
			public static LocString PEDESTALDISPLAYABLE = "Other Pedestal Displayable";

			// Token: 0x0400ABFF RID: 44031
			public static LocString COMMAND_MODULE = "Command Module";

			// Token: 0x0400AC00 RID: 44032
			public static LocString HABITAT_MODULE = "Habitat Module";

			// Token: 0x0400AC01 RID: 44033
			public static LocString COMBUSTIBLEGAS = UI.FormatAsLink("Combustible Gas", "COMBUSTIBLEGAS");

			// Token: 0x0400AC02 RID: 44034
			public static LocString COMBUSTIBLEGAS_DESC = string.Concat(new string[]
			{
				"Combustible Gases can be burned as fuel to be used in the production of ",
				UI.FormatAsLink("Power", "POWER"),
				" and ",
				UI.FormatAsLink("Food", "FOOD"),
				"."
			});

			// Token: 0x0400AC03 RID: 44035
			public static LocString COMBUSTIBLELIQUID = UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID");

			// Token: 0x0400AC04 RID: 44036
			public static LocString COMBUSTIBLELIQUID_DESC = string.Concat(new string[]
			{
				"Combustible Liquids can be burned as fuels to be used in energy production, such as in a ",
				UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR"),
				" or a ",
				UI.FormatAsLink(KeroseneEngineHelper.NAME, KeroseneEngineHelper.CODEXID),
				".\n\nThough these liquids have other uses, such as fertilizer for growing a ",
				UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED"),
				", their primary usefulness lies in their ability to be burned for ",
				UI.FormatAsLink("Power", "POWER"),
				"."
			});

			// Token: 0x0400AC05 RID: 44037
			public static LocString COMBUSTIBLESOLID = UI.FormatAsLink("Combustible Solid", "COMBUSTIBLESOLID");

			// Token: 0x0400AC06 RID: 44038
			public static LocString COMBUSTIBLESOLID_DESC = "Combustible Solids can be burned as fuel to be used in " + UI.FormatAsLink("Power", "POWER") + " production.";

			// Token: 0x0400AC07 RID: 44039
			public static LocString UNIDENTIFIEDSEED = "Seed (Unidentified Mutation)";

			// Token: 0x0400AC08 RID: 44040
			public static LocString ARTIFACT = "Artifact";

			// Token: 0x0400AC09 RID: 44041
			public static LocString CHARMEDARTIFACT = "Artifact of Interest";

			// Token: 0x0400AC0A RID: 44042
			public static LocString GENE_SHUFFLER = "Neural Vacillator";

			// Token: 0x0400AC0B RID: 44043
			public static LocString WARP_PORTAL = "Teleportal";

			// Token: 0x0400AC0C RID: 44044
			public static LocString BIONICUPGRADE = "Boosters";

			// Token: 0x0400AC0D RID: 44045
			public static LocString FARMING = "Farm Build-Delivery";

			// Token: 0x0400AC0E RID: 44046
			public static LocString RESEARCH = "Research Delivery";

			// Token: 0x0400AC0F RID: 44047
			public static LocString POWER = "Generator Delivery";

			// Token: 0x0400AC10 RID: 44048
			public static LocString BUILDING = "Build Dig-Delivery";

			// Token: 0x0400AC11 RID: 44049
			public static LocString COOKING = "Cook Delivery";

			// Token: 0x0400AC12 RID: 44050
			public static LocString FABRICATING = "Fabricate Delivery";

			// Token: 0x0400AC13 RID: 44051
			public static LocString WIRING = "Wire Build-Delivery";

			// Token: 0x0400AC14 RID: 44052
			public static LocString ART = "Art Build-Delivery";

			// Token: 0x0400AC15 RID: 44053
			public static LocString DOCTORING = "Treatment Delivery";

			// Token: 0x0400AC16 RID: 44054
			public static LocString CONVEYOR = "Shipping Build";

			// Token: 0x0400AC17 RID: 44055
			public static LocString COMPOST_FORMAT = "{Item}";

			// Token: 0x0400AC18 RID: 44056
			public static LocString ADVANCEDDOCTORSTATIONMEDICALSUPPLIES = "Serum Vial";

			// Token: 0x0400AC19 RID: 44057
			public static LocString DOCTORSTATIONMEDICALSUPPLIES = "Medical Pack";

			// Token: 0x0400AC1A RID: 44058
			public static LocString LONGRANGEMISSILE = "Long Range Missile";
		}

		// Token: 0x0200266B RID: 9835
		public class STATUSITEMS
		{
			// Token: 0x02003957 RID: 14679
			public class ATTENTIONREQUIRED
			{
				// Token: 0x0400E844 RID: 59460
				public static LocString NAME = "Attention Required!";

				// Token: 0x0400E845 RID: 59461
				public static LocString TOOLTIP = "Something in my colony needs to be attended to";
			}

			// Token: 0x02003958 RID: 14680
			public class SUBLIMATIONBLOCKED
			{
				// Token: 0x0400E846 RID: 59462
				public static LocString NAME = "{SubElement} emission blocked";

				// Token: 0x0400E847 RID: 59463
				public static LocString TOOLTIP = "This {Element} deposit is not exposed to air and cannot emit {SubElement}";
			}

			// Token: 0x02003959 RID: 14681
			public class SUBLIMATIONOVERPRESSURE
			{
				// Token: 0x0400E848 RID: 59464
				public static LocString NAME = "Inert";

				// Token: 0x0400E849 RID: 59465
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Environmental ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is too high for this {Element} deposit to emit {SubElement}"
				});
			}

			// Token: 0x0200395A RID: 14682
			public class SUBLIMATIONEMITTING
			{
				// Token: 0x0400E84A RID: 59466
				public static LocString NAME = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;

				// Token: 0x0400E84B RID: 59467
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.EMITTINGGASAVG.TOOLTIP;
			}

			// Token: 0x0200395B RID: 14683
			public class SPACE
			{
				// Token: 0x0400E84C RID: 59468
				public static LocString NAME = "Space exposure";

				// Token: 0x0400E84D RID: 59469
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This region is exposed to the vacuum of space and will result in the loss of ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" resources"
				});
			}

			// Token: 0x0200395C RID: 14684
			public class EDIBLE
			{
				// Token: 0x0400E84E RID: 59470
				public static LocString NAME = "Rations: {0}";

				// Token: 0x0400E84F RID: 59471
				public static LocString TOOLTIP = "Can provide " + UI.FormatAsLink("{0}", "KCAL") + " of energy to Duplicants";
			}

			// Token: 0x0200395D RID: 14685
			public class REHYDRATEDFOOD
			{
				// Token: 0x0400E850 RID: 59472
				public static LocString NAME = "Rehydrated Food";

				// Token: 0x0400E851 RID: 59473
				public static LocString TOOLTIP = string.Format(string.Concat(new string[]
				{
					"This food has been carefully re-moistened for consumption\n\n",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					": {0}"
				}), -1f, UI.FormatAsLink(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME, DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME));
			}

			// Token: 0x0200395E RID: 14686
			public class MARKEDFORDISINFECTION
			{
				// Token: 0x0400E852 RID: 59474
				public static LocString NAME = "Disinfect Errand";

				// Token: 0x0400E853 RID: 59475
				public static LocString TOOLTIP = "Building will be disinfected once a Duplicant is available";
			}

			// Token: 0x0200395F RID: 14687
			public class PENDINGCLEAR
			{
				// Token: 0x0400E854 RID: 59476
				public static LocString NAME = "Sweep Errand";

				// Token: 0x0400E855 RID: 59477
				public static LocString TOOLTIP = "Debris will be swept once a Duplicant is available";
			}

			// Token: 0x02003960 RID: 14688
			public class PENDINGCLEARNOSTORAGE
			{
				// Token: 0x0400E856 RID: 59478
				public static LocString NAME = "Storage Unavailable";

				// Token: 0x0400E857 RID: 59479
				public static LocString TOOLTIP = "No available " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " can accept this item\n\nMake sure the filter on your storage is correctly set and there is sufficient space remaining";
			}

			// Token: 0x02003961 RID: 14689
			public class MARKEDFORCOMPOST
			{
				// Token: 0x0400E858 RID: 59480
				public static LocString NAME = "Compost Errand";

				// Token: 0x0400E859 RID: 59481
				public static LocString TOOLTIP = "Object is marked and will be moved to " + BUILDINGS.PREFABS.COMPOST.NAME + " once a Duplicant is available";
			}

			// Token: 0x02003962 RID: 14690
			public class NOCLEARLOCATIONSAVAILABLE
			{
				// Token: 0x0400E85A RID: 59482
				public static LocString NAME = "No Sweep Destination";

				// Token: 0x0400E85B RID: 59483
				public static LocString TOOLTIP = "There are no valid destinations for this object to be swept to";
			}

			// Token: 0x02003963 RID: 14691
			public class PENDINGHARVEST
			{
				// Token: 0x0400E85C RID: 59484
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400E85D RID: 59485
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02003964 RID: 14692
			public class PENDINGUPROOT
			{
				// Token: 0x0400E85E RID: 59486
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400E85F RID: 59487
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02003965 RID: 14693
			public class WAITINGFORDIG
			{
				// Token: 0x0400E860 RID: 59488
				public static LocString NAME = "Dig Errand";

				// Token: 0x0400E861 RID: 59489
				public static LocString TOOLTIP = "Tile will be dug out once a Duplicant is available";
			}

			// Token: 0x02003966 RID: 14694
			public class WAITINGFORMOP
			{
				// Token: 0x0400E862 RID: 59490
				public static LocString NAME = "Mop Errand";

				// Token: 0x0400E863 RID: 59491
				public static LocString TOOLTIP = "Spill will be mopped once a Duplicant is available";
			}

			// Token: 0x02003967 RID: 14695
			public class NOTMARKEDFORHARVEST
			{
				// Token: 0x0400E864 RID: 59492
				public static LocString NAME = "No Harvest Pending";

				// Token: 0x0400E865 RID: 59493
				public static LocString TOOLTIP = "Use the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to mark this plant for harvest";
			}

			// Token: 0x02003968 RID: 14696
			public class GROWINGBRANCHES
			{
				// Token: 0x0400E866 RID: 59494
				public static LocString NAME = "Growing Branches";

				// Token: 0x0400E867 RID: 59495
				public static LocString TOOLTIP = "This tree is working hard to grow new branches right now";
			}

			// Token: 0x02003969 RID: 14697
			public class CLUSTERMETEORREMAININGTRAVELTIME
			{
				// Token: 0x0400E868 RID: 59496
				public static LocString NAME = "Time to collision: {time}";

				// Token: 0x0400E869 RID: 59497
				public static LocString TOOLTIP = "The time remaining before this meteor reaches its destination";
			}

			// Token: 0x0200396A RID: 14698
			public class ELEMENTALCATEGORY
			{
				// Token: 0x0400E86A RID: 59498
				public static LocString NAME = "{Category}";

				// Token: 0x0400E86B RID: 59499
				public static LocString TOOLTIP = "The selected object belongs to the <b>{Category}</b> resource category";
			}

			// Token: 0x0200396B RID: 14699
			public class ELEMENTALMASS
			{
				// Token: 0x0400E86C RID: 59500
				public static LocString NAME = "{Mass}";

				// Token: 0x0400E86D RID: 59501
				public static LocString TOOLTIP = "The selected object has a mass of <b>{Mass}</b>";
			}

			// Token: 0x0200396C RID: 14700
			public class ELEMENTALDISEASE
			{
				// Token: 0x0400E86E RID: 59502
				public static LocString NAME = "{Disease}";

				// Token: 0x0400E86F RID: 59503
				public static LocString TOOLTIP = "Current disease: {Disease}";
			}

			// Token: 0x0200396D RID: 14701
			public class ELEMENTALTEMPERATURE
			{
				// Token: 0x0400E870 RID: 59504
				public static LocString NAME = "{Temp}";

				// Token: 0x0400E871 RID: 59505
				public static LocString TOOLTIP = "The selected object is currently <b>{Temp}</b>";
			}

			// Token: 0x0200396E RID: 14702
			public class MARKEDFORCOMPOSTINSTORAGE
			{
				// Token: 0x0400E872 RID: 59506
				public static LocString NAME = "Composted";

				// Token: 0x0400E873 RID: 59507
				public static LocString TOOLTIP = "The selected object is currently in the compost";
			}

			// Token: 0x0200396F RID: 14703
			public class BURIEDITEM
			{
				// Token: 0x0400E874 RID: 59508
				public static LocString NAME = "Buried Object";

				// Token: 0x0400E875 RID: 59509
				public static LocString TOOLTIP = "Something seems to be hidden here";

				// Token: 0x0400E876 RID: 59510
				public static LocString NOTIFICATION = "Buried object discovered";

				// Token: 0x0400E877 RID: 59511
				public static LocString NOTIFICATION_TOOLTIP = "My Duplicants have uncovered a {Uncoverable}!\n\n" + UI.CLICK(UI.ClickType.Click) + " to jump to its location.";
			}

			// Token: 0x02003970 RID: 14704
			public class GENETICANALYSISCOMPLETED
			{
				// Token: 0x0400E878 RID: 59512
				public static LocString NAME = "Genome Sequenced";

				// Token: 0x0400E879 RID: 59513
				public static LocString TOOLTIP = "This Station has sequenced a new seed mutation";
			}

			// Token: 0x02003971 RID: 14705
			public class HEALTHSTATUS
			{
				// Token: 0x02003DA4 RID: 15780
				public class PERFECT
				{
					// Token: 0x0400F333 RID: 62259
					public static LocString NAME = "None";

					// Token: 0x0400F334 RID: 62260
					public static LocString TOOLTIP = "This Duplicant is in peak condition";
				}

				// Token: 0x02003DA5 RID: 15781
				public class ALRIGHT
				{
					// Token: 0x0400F335 RID: 62261
					public static LocString NAME = "None";

					// Token: 0x0400F336 RID: 62262
					public static LocString TOOLTIP = "This Duplicant is none the worse for wear";
				}

				// Token: 0x02003DA6 RID: 15782
				public class SCUFFED
				{
					// Token: 0x0400F337 RID: 62263
					public static LocString NAME = "Minor";

					// Token: 0x0400F338 RID: 62264
					public static LocString TOOLTIP = "This Duplicant has a few scrapes and bruises";
				}

				// Token: 0x02003DA7 RID: 15783
				public class INJURED
				{
					// Token: 0x0400F339 RID: 62265
					public static LocString NAME = "Moderate";

					// Token: 0x0400F33A RID: 62266
					public static LocString TOOLTIP = "This Duplicant needs some patching up";
				}

				// Token: 0x02003DA8 RID: 15784
				public class CRITICAL
				{
					// Token: 0x0400F33B RID: 62267
					public static LocString NAME = "Severe";

					// Token: 0x0400F33C RID: 62268
					public static LocString TOOLTIP = "This Duplicant is in serious need of medical attention";
				}

				// Token: 0x02003DA9 RID: 15785
				public class INCAPACITATED
				{
					// Token: 0x0400F33D RID: 62269
					public static LocString NAME = "Paralyzing";

					// Token: 0x0400F33E RID: 62270
					public static LocString TOOLTIP = "This Duplicant will die if they do not receive medical attention";
				}

				// Token: 0x02003DAA RID: 15786
				public class DEAD
				{
					// Token: 0x0400F33F RID: 62271
					public static LocString NAME = "Conclusive";

					// Token: 0x0400F340 RID: 62272
					public static LocString TOOLTIP = "This Duplicant won't be getting back up";
				}
			}

			// Token: 0x02003972 RID: 14706
			public class HIT
			{
				// Token: 0x0400E87A RID: 59514
				public static LocString NAME = "{targetName} took {damageAmount} damage from {attackerName}'s attack!";
			}

			// Token: 0x02003973 RID: 14707
			public class OREMASS
			{
				// Token: 0x0400E87B RID: 59515
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALMASS.NAME;

				// Token: 0x0400E87C RID: 59516
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALMASS.TOOLTIP;
			}

			// Token: 0x02003974 RID: 14708
			public class ORETEMP
			{
				// Token: 0x0400E87D RID: 59517
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.NAME;

				// Token: 0x0400E87E RID: 59518
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.TOOLTIP;
			}

			// Token: 0x02003975 RID: 14709
			public class TREEFILTERABLETAGS
			{
				// Token: 0x0400E87F RID: 59519
				public static LocString NAME = "{Tags}";

				// Token: 0x0400E880 RID: 59520
				public static LocString TOOLTIP = "{Tags}";
			}

			// Token: 0x02003976 RID: 14710
			public class SPOUTOVERPRESSURE
			{
				// Token: 0x0400E881 RID: 59521
				public static LocString NAME = "Overpressure {StudiedDetails}";

				// Token: 0x0400E882 RID: 59522
				public static LocString TOOLTIP = "Spout cannot vent due to high environmental pressure";

				// Token: 0x0400E883 RID: 59523
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003977 RID: 14711
			public class SPOUTEMITTING
			{
				// Token: 0x0400E884 RID: 59524
				public static LocString NAME = "Venting {StudiedDetails}";

				// Token: 0x0400E885 RID: 59525
				public static LocString TOOLTIP = "This geyser is erupting";

				// Token: 0x0400E886 RID: 59526
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003978 RID: 14712
			public class SPOUTPRESSUREBUILDING
			{
				// Token: 0x0400E887 RID: 59527
				public static LocString NAME = "Rising pressure {StudiedDetails}";

				// Token: 0x0400E888 RID: 59528
				public static LocString TOOLTIP = "This geyser's internal pressure is steadily building";

				// Token: 0x0400E889 RID: 59529
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003979 RID: 14713
			public class SPOUTIDLE
			{
				// Token: 0x0400E88A RID: 59530
				public static LocString NAME = "Idle {StudiedDetails}";

				// Token: 0x0400E88B RID: 59531
				public static LocString TOOLTIP = "This geyser is not currently erupting";

				// Token: 0x0400E88C RID: 59532
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x0200397A RID: 14714
			public class SPOUTDORMANT
			{
				// Token: 0x0400E88D RID: 59533
				public static LocString NAME = "Dormant";

				// Token: 0x0400E88E RID: 59534
				public static LocString TOOLTIP = "This geyser's geoactivity has halted\n\nIt won't erupt again for some time";
			}

			// Token: 0x0200397B RID: 14715
			public class SPICEDFOOD
			{
				// Token: 0x0400E88F RID: 59535
				public static LocString NAME = "Seasoned";

				// Token: 0x0400E890 RID: 59536
				public static LocString TOOLTIP = "This food has been improved with spice from the " + BUILDINGS.PREFABS.SPICEGRINDER.NAME;
			}

			// Token: 0x0200397C RID: 14716
			public class PICKUPABLEUNREACHABLE
			{
				// Token: 0x0400E891 RID: 59537
				public static LocString NAME = "Unreachable";

				// Token: 0x0400E892 RID: 59538
				public static LocString TOOLTIP = "Duplicants cannot reach this object";
			}

			// Token: 0x0200397D RID: 14717
			public class PRIORITIZED
			{
				// Token: 0x0400E893 RID: 59539
				public static LocString NAME = "High Priority";

				// Token: 0x0400E894 RID: 59540
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Errand",
					UI.PST_KEYWORD,
					" has been marked as important and will be preferred over other pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200397E RID: 14718
			public class USING
			{
				// Token: 0x0400E895 RID: 59541
				public static LocString NAME = "Using {Target}";

				// Token: 0x0400E896 RID: 59542
				public static LocString TOOLTIP = "{Target} is currently in use";
			}

			// Token: 0x0200397F RID: 14719
			public class ORDERATTACK
			{
				// Token: 0x0400E897 RID: 59543
				public static LocString NAME = "Pending Attack";

				// Token: 0x0400E898 RID: 59544
				public static LocString TOOLTIP = "Waiting for a Duplicant to murderize this defenseless " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
			}

			// Token: 0x02003980 RID: 14720
			public class ORDERCAPTURE
			{
				// Token: 0x0400E899 RID: 59545
				public static LocString NAME = "Pending Wrangle";

				// Token: 0x0400E89A RID: 59546
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Waiting for a Duplicant to capture this ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"\n\nOnly Duplicants with the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill can catch critters without traps"
				});
			}

			// Token: 0x02003981 RID: 14721
			public class OPERATING
			{
				// Token: 0x0400E89B RID: 59547
				public static LocString NAME = "In Use";

				// Token: 0x0400E89C RID: 59548
				public static LocString TOOLTIP = "This object is currently being used";
			}

			// Token: 0x02003982 RID: 14722
			public class CLEANING
			{
				// Token: 0x0400E89D RID: 59549
				public static LocString NAME = "Cleaning";

				// Token: 0x0400E89E RID: 59550
				public static LocString TOOLTIP = "This building is currently being cleaned";
			}

			// Token: 0x02003983 RID: 14723
			public class REGIONISBLOCKED
			{
				// Token: 0x0400E89F RID: 59551
				public static LocString NAME = "Blocked";

				// Token: 0x0400E8A0 RID: 59552
				public static LocString TOOLTIP = "Undug material is blocking off an essential tile";
			}

			// Token: 0x02003984 RID: 14724
			public class STUDIED
			{
				// Token: 0x0400E8A1 RID: 59553
				public static LocString NAME = "Analysis Complete";

				// Token: 0x0400E8A2 RID: 59554
				public static LocString TOOLTIP = "Information on this Natural Feature has been compiled below.";
			}

			// Token: 0x02003985 RID: 14725
			public class AWAITINGSTUDY
			{
				// Token: 0x0400E8A3 RID: 59555
				public static LocString NAME = "Analysis Pending";

				// Token: 0x0400E8A4 RID: 59556
				public static LocString TOOLTIP = "New information on this Natural Feature will be compiled once the field study is complete";
			}

			// Token: 0x02003986 RID: 14726
			public class DURABILITY
			{
				// Token: 0x0400E8A5 RID: 59557
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400E8A6 RID: 59558
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x02003987 RID: 14727
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400E8A7 RID: 59559
				public static LocString NAME = "Stored Geodata: {0}";

				// Token: 0x0400E8A8 RID: 59560
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Dowsing Boosters" + UI.PST_KEYWORD + " retain geodata gathered by Bionic Duplicants\n\nWhen dowsing is complete and this booster is installed in a Bionic Duplicant, a new geyser will be revealed";
			}

			// Token: 0x02003988 RID: 14728
			public class BIONICEXPLORERBOOSTERREADY
			{
				// Token: 0x0400E8A9 RID: 59561
				public static LocString NAME = "Dowsing Complete";

				// Token: 0x0400E8AA RID: 59562
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Dowsing Booster",
					UI.PST_KEYWORD,
					" has sufficient geodata stored to reveal a new geyser\n\nIt must be installed in a Bionic Duplicant in order to function"
				});
			}

			// Token: 0x02003989 RID: 14729
			public class UNASSIGNEDBIONICBOOSTER
			{
				// Token: 0x0400E8AB RID: 59563
				public static LocString NAME = "Unassigned";

				// Token: 0x0400E8AC RID: 59564
				public static LocString TOOLTIP = "This booster has not yet been assigned to a Bionic Duplicant";
			}

			// Token: 0x0200398A RID: 14730
			public class STOREDITEMDURABILITY
			{
				// Token: 0x0400E8AD RID: 59565
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400E8AE RID: 59566
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x0200398B RID: 14731
			public class ARTIFACTENTOMBED
			{
				// Token: 0x0400E8AF RID: 59567
				public static LocString NAME = "Entombed Artifact";

				// Token: 0x0400E8B0 RID: 59568
				public static LocString TOOLTIP = "This artifact is trapped in an obscuring shell limiting its decor. A skilled artist can remove it at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x0200398C RID: 14732
			public class TEAROPEN
			{
				// Token: 0x0400E8B1 RID: 59569
				public static LocString NAME = "Temporal Tear open";

				// Token: 0x0400E8B2 RID: 59570
				public static LocString TOOLTIP = "An open passage through spacetime";
			}

			// Token: 0x0200398D RID: 14733
			public class TEARCLOSED
			{
				// Token: 0x0400E8B3 RID: 59571
				public static LocString NAME = "Temporal Tear closed";

				// Token: 0x0400E8B4 RID: 59572
				public static LocString TOOLTIP = "Perhaps some technology could open the passage";
			}

			// Token: 0x0200398E RID: 14734
			public class LARGEIMPACTORSTATUS
			{
				// Token: 0x0400E8B5 RID: 59573
				public static LocString NAME = "Time until impact: {0}";

				// Token: 0x0400E8B6 RID: 59574
				public static LocString TOOLTIP = "This impactor asteroid will reach its target in {0}";
			}

			// Token: 0x0200398F RID: 14735
			public class LARGEIMPACTORHEALTH
			{
				// Token: 0x0400E8B7 RID: 59575
				public static LocString NAME = "Health: {0} / {1}";

				// Token: 0x0400E8B8 RID: 59576
				public static LocString TOOLTIP = "Collision damage can be avoided by destroying this impactor asteroid with " + UI.FormatAsLink("Intracosmic Blastshot", "LONGRANGEMISSILE") + " before it makes contact";
			}

			// Token: 0x02003990 RID: 14736
			public class LONGRANGEMISSILETTI
			{
				// Token: 0x0400E8B9 RID: 59577
				public static LocString NAME = "Time To Intercept {0}: {1}";

				// Token: 0x0400E8BA RID: 59578
				public static LocString TOOLTIP = "This projectile will reach its destination in {1}";
			}

			// Token: 0x02003991 RID: 14737
			public class MARKEDFORMOVE
			{
				// Token: 0x0400E8BB RID: 59579
				public static LocString NAME = "Pending Move";

				// Token: 0x0400E8BC RID: 59580
				public static LocString TOOLTIP = "Waiting for a Duplicant to move this object";
			}

			// Token: 0x02003992 RID: 14738
			public class MOVESTORAGEUNREACHABLE
			{
				// Token: 0x0400E8BD RID: 59581
				public static LocString NAME = "Unreachable Move";

				// Token: 0x0400E8BE RID: 59582
				public static LocString TOOLTIP = "Duplicants cannot reach this object to move it";
			}

			// Token: 0x02003993 RID: 14739
			public class CLUSTERMAPHARVESTABLERESOURCE
			{
				// Token: 0x0400E8BF RID: 59583
				public static LocString NAME = "{0}";

				// Token: 0x0400E8C0 RID: 59584
				public static LocString TOOLTIP = "{0}";
			}

			// Token: 0x02003994 RID: 14740
			public class PENDINGCARVE
			{
				// Token: 0x0400E8C1 RID: 59585
				public static LocString NAME = "Carve Errand";

				// Token: 0x0400E8C2 RID: 59586
				public static LocString TOOLTIP = "Rock will be carved once a Duplicant is available";
			}

			// Token: 0x02003995 RID: 14741
			public class ELECTROBANKLIFETIMEREMAINING
			{
				// Token: 0x0400E8C3 RID: 59587
				public static LocString NAME = "Lifetime Remaining: {0}";

				// Token: 0x0400E8C4 RID: 59588
				public static LocString TOOLTIP = "Self-charging will continue for {0}\n\nWhen lifetime reaches zero, this  " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " will explode";
			}

			// Token: 0x02003996 RID: 14742
			public class ELECTROBANKSELFCHARGING
			{
				// Token: 0x0400E8C5 RID: 59589
				public static LocString NAME = "Self-Charging: {0}";

				// Token: 0x0400E8C6 RID: 59590
				public static LocString TOOLTIP = "This " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " is always slowly charging itself";
			}
		}

		// Token: 0x0200266C RID: 9836
		public class POPFX
		{
			// Token: 0x0400AC1B RID: 44059
			public static LocString RESOURCE_EATEN = "Resource Eaten";

			// Token: 0x0400AC1C RID: 44060
			public static LocString RESOURCE_SELECTION_CHANGED = "Changed to {0}";

			// Token: 0x0400AC1D RID: 44061
			public static LocString EXTRA_POWERBANKS_BIONIC = "Extra Power Banks";
		}

		// Token: 0x0200266D RID: 9837
		public class NOTIFICATIONS
		{
			// Token: 0x02003997 RID: 14743
			public class BASICCONTROLS
			{
				// Token: 0x0400E8C7 RID: 59591
				public static LocString NAME = "Tutorial: Basic Controls";

				// Token: 0x0400E8C8 RID: 59592
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.PanLeft),
					" and ",
					UI.FormatAsHotKey(global::Action.PanRight),
					" to pan my view left and right, and ",
					UI.FormatAsHotKey(global::Action.PanUp),
					" and ",
					UI.FormatAsHotKey(global::Action.PanDown),
					" to pan up and down.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• ",
					UI.FormatAsHotKey(global::Action.CameraHome),
					" returns my view to the Printing Pod.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.SpeedUp),
					" or ",
					UI.FormatAsHotKey(global::Action.SlowDown),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400E8C9 RID: 59593
				public static LocString MESSAGEBODYALT = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.AnalogCamera),
					" to pan my view.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.CycleSpeed),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400E8CA RID: 59594
				public static LocString TOOLTIP = "Notes on using my HUD";
			}

			// Token: 0x02003998 RID: 14744
			public class CODEXUNLOCK
			{
				// Token: 0x0400E8CB RID: 59595
				public static LocString NAME = "New Log Entry";

				// Token: 0x0400E8CC RID: 59596
				public static LocString MESSAGEBODY = "I've added a new log entry to my Database";

				// Token: 0x0400E8CD RID: 59597
				public static LocString TOOLTIP = "I've added a new log entry to my Database";
			}

			// Token: 0x02003999 RID: 14745
			public class WELCOMEMESSAGE
			{
				// Token: 0x0400E8CE RID: 59598
				public static LocString NAME = "Tutorial: Colony Management";

				// Token: 0x0400E8CF RID: 59599
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"I can use the ",
					UI.FormatAsTool("Dig Tool", global::Action.Dig),
					" and the ",
					UI.FormatAsBuildMenuTab("Build Menu"),
					" in the lower left of the screen to begin planning my first construction tasks.\n\nOnce I've placed a few errands my Duplicants will automatically get to work, without me needing to direct them individually."
				});

				// Token: 0x0400E8D0 RID: 59600
				public static LocString TOOLTIP = "Notes on getting Duplicants to do my bidding";
			}

			// Token: 0x0200399A RID: 14746
			public class STRESSMANAGEMENTMESSAGE
			{
				// Token: 0x0400E8D1 RID: 59601
				public static LocString NAME = "Tutorial: Stress Management";

				// Token: 0x0400E8D2 RID: 59602
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"At 100% ",
					UI.FormatAsLink("Stress", "STRESS"),
					", a Duplicant will have a nervous breakdown and be unable to work.\n\nBreakdowns can manifest in different colony-threatening ways, such as the destruction of buildings or the binge eating of food.\n\nI can help my Duplicants manage stressful situations by giving them access to good ",
					UI.FormatAsLink("Food", "FOOD"),
					", fancy ",
					UI.FormatAsLink("Decor", "DECOR"),
					" and comfort items which boost their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nI can select a Duplicant and mouse over ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in their CONDITION TAB to view current statuses, and hopefully manage things before they become a problem.\n\nRelated ",
					UI.FormatAsLink("Video: Duplicant Morale", "VIDEOS13"),
					" "
				});

				// Token: 0x0400E8D3 RID: 59603
				public static LocString TOOLTIP = "Notes on keeping Duplicants happy and productive";
			}

			// Token: 0x0200399B RID: 14747
			public class TASKPRIORITIESMESSAGE
			{
				// Token: 0x0400E8D4 RID: 59604
				public static LocString NAME = "Tutorial: Priority";

				// Token: 0x0400E8D5 RID: 59605
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants always perform errands in order of highest to lowest priority. They will harvest ",
					UI.FormatAsLink("Food", "FOOD"),
					" before they build, for example, or always build new structures before they mine materials.\n\nI can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set which Errand Types Duplicants may or may not perform, or to specialize skilled Duplicants for particular Errand Types."
				});

				// Token: 0x0400E8D6 RID: 59606
				public static LocString TOOLTIP = "Notes on managing Duplicants' errands";
			}

			// Token: 0x0200399C RID: 14748
			public class MOPPINGMESSAGE
			{
				// Token: 0x0400E8D7 RID: 59607
				public static LocString NAME = "Tutorial: Polluted Water";

				// Token: 0x0400E8D8 RID: 59608
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" slowly emits ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" which accelerates the spread of ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nDuplicants will also be ",
					UI.FormatAsLink("Stressed", "STRESS"),
					" by walking through Polluted Water, so I should have my Duplicants clean up spills by ",
					UI.CLICK(UI.ClickType.clicking),
					" and dragging the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop)
				});

				// Token: 0x0400E8D9 RID: 59609
				public static LocString TOOLTIP = "Notes on handling polluted materials";
			}

			// Token: 0x0200399D RID: 14749
			public class LOCOMOTIONMESSAGE
			{
				// Token: 0x0400E8DA RID: 59610
				public static LocString NAME = "Video: Duplicant Movement";

				// Token: 0x0400E8DB RID: 59611
				public static LocString MESSAGEBODY = "Duplicants have limited jumping and climbing abilities. They can only climb two tiles high and cannot fit into spaces shorter than two tiles, or cross gaps wider than one tile. I should keep this in mind while placing errands.\n\nTo check if an errand I've placed is accessible, I can select a Duplicant and " + UI.CLICK(UI.ClickType.click) + " <b>Show Navigation</b> to view all areas within their reach.";

				// Token: 0x0400E8DC RID: 59612
				public static LocString TOOLTIP = "Notes on my Duplicants' maneuverability";
			}

			// Token: 0x0200399E RID: 14750
			public class PRIORITIESMESSAGE
			{
				// Token: 0x0400E8DD RID: 59613
				public static LocString NAME = "Tutorial: Errand Priorities";

				// Token: 0x0400E8DE RID: 59614
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants will choose where to work based on the priority of the errands that I give them. I can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set their ",
					UI.PRE_KEYWORD,
					"Duplicant Priorities",
					UI.PST_KEYWORD,
					", and the ",
					UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
					" to fine tune ",
					UI.PRE_KEYWORD,
					"Building Priority",
					UI.PST_KEYWORD,
					". Many buildings will also let me change their Priority level when I select them."
				});

				// Token: 0x0400E8DF RID: 59615
				public static LocString TOOLTIP = "Notes on my Duplicants' priorities";
			}

			// Token: 0x0200399F RID: 14751
			public class FETCHINGWATERMESSAGE
			{
				// Token: 0x0400E8E0 RID: 59616
				public static LocString NAME = "Tutorial: Fetching Water";

				// Token: 0x0400E8E1 RID: 59617
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"By building a ",
					UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" over a pool of liquid, my Duplicants will be able to bottle it up and manually deliver it wherever it needs to go."
				});

				// Token: 0x0400E8E2 RID: 59618
				public static LocString TOOLTIP = "Notes on liquid resource gathering";
			}

			// Token: 0x020039A0 RID: 14752
			public class SCHEDULEMESSAGE
			{
				// Token: 0x0400E8E3 RID: 59619
				public static LocString NAME = "Tutorial: Scheduling";

				// Token: 0x0400E8E4 RID: 59620
				public static LocString MESSAGEBODY = "My Duplicants will only eat, sleep, work, or bathe during the times I allot for such activities.\n\nTo make the best use of their time, I can open the " + UI.FormatAsManagementMenu("Schedule Tab", global::Action.ManageSchedule) + " to adjust the colony's schedule and plan how they should utilize their day.";

				// Token: 0x0400E8E5 RID: 59621
				public static LocString TOOLTIP = "Notes on scheduling my Duplicants' time";
			}

			// Token: 0x020039A1 RID: 14753
			public class THERMALCOMFORT
			{
				// Token: 0x0400E8E6 RID: 59622
				public static LocString NAME = "Tutorial: Duplicant Temperature";

				// Token: 0x0400E8E7 RID: 59623
				public static LocString TOOLTIP = "Notes on helping Duplicants keep their cool";

				// Token: 0x0400E8E8 RID: 59624
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Environments that are extremely ",
					UI.FormatAsLink("Hot", "HEAT"),
					" or ",
					UI.FormatAsLink("Cold", "HEAT"),
					" affect my Duplicants' internal body temperature and cause undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or unscheduled naps.\n\nOpening the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" and checking the <b>Thermal Tolerance</b> box allows me to view all areas where my Duplicants will feel discomfort and be unable to regulate their internal body temperature.\n\nRelated ",
					UI.FormatAsLink("Video: Insulation", "VIDEOS17")
				});
			}

			// Token: 0x020039A2 RID: 14754
			public class TUTORIAL_OVERHEATING
			{
				// Token: 0x0400E8E9 RID: 59625
				public static LocString NAME = "Tutorial: Building Temperature";

				// Token: 0x0400E8EA RID: 59626
				public static LocString TOOLTIP = "Notes on preventing building from breaking";

				// Token: 0x0400E8EB RID: 59627
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When constructing buildings, I should always take note of their ",
					UI.FormatAsLink("Overheat Temperature", "HEAT"),
					" and plan their locations accordingly. Maintaining low ambient temperatures and good ventilation in the colony will also help keep building temperatures down.\n\nThe <b>Relative Temperature</b> slider tool in the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" allows me to change adjust the overlay's color-coding in order to highlight specific temperature ranges.\n\nIf I allow buildings to exceed their Overheat Temperature they will begin to take damage, and if left unattended, they will break down and be unusable until repaired."
				});
			}

			// Token: 0x020039A3 RID: 14755
			public class LOTS_OF_GERMS
			{
				// Token: 0x0400E8EC RID: 59628
				public static LocString NAME = "Tutorial: Germs and Disease";

				// Token: 0x0400E8ED RID: 59629
				public static LocString TOOLTIP = "Notes on Duplicant disease risks";

				// Token: 0x0400E8EE RID: 59630
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" such as ",
					DUPLICANTS.DISEASES.FOODPOISONING.NAME,
					" and ",
					DUPLICANTS.DISEASES.SLIMELUNG.NAME,
					" can cause ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" in my Duplicants. I can use the ",
					UI.FormatAsOverlay("Germ Overlay", global::Action.Overlay9),
					" to view all germ concentrations in my colony, and even detect the sources spawning them.\n\nBuilding Wash Basins from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					" near colony toilets will tell my Duplicants they need to wash up.\n\nRelated ",
					UI.FormatAsLink("Video: Plumbing and Ventilation", "VIDEOS18")
				});
			}

			// Token: 0x020039A4 RID: 14756
			public class BEING_INFECTED
			{
				// Token: 0x0400E8EF RID: 59631
				public static LocString NAME = "Tutorial: Immune Systems";

				// Token: 0x0400E8F0 RID: 59632
				public static LocString TOOLTIP = "Notes on keeping Duplicants in peak health";

				// Token: 0x0400E8F1 RID: 59633
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When Duplicants come into contact with various ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", they'll need to expend points of ",
					UI.FormatAsLink("Immunity", "IMMUNE SYSTEM"),
					" to resist them and remain healthy. If repeated exposes causes their Immunity to drop to 0%, they'll be unable to resist germs and will contract the next disease they encounter.\n\nDoors with Access Permissions can be built from the BASE TAB<color=#F44A47> <b>[1]</b></color> of the ",
					UI.FormatAsLink("Build menu", "misc"),
					" to block Duplicants from entering biohazardous areas while they recover their spent immunity points."
				});
			}

			// Token: 0x020039A5 RID: 14757
			public class DISEASE_COOKING
			{
				// Token: 0x0400E8F2 RID: 59634
				public static LocString NAME = "Tutorial: Food Safety";

				// Token: 0x0400E8F3 RID: 59635
				public static LocString TOOLTIP = "Notes on managing food contamination";

				// Token: 0x0400E8F4 RID: 59636
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsLink("Food", "FOOD"),
					" my Duplicants cook will only ever be as clean as the ingredients used to make it. Storing food in sterile or ",
					UI.FormatAsLink("Refrigerated", "REFRIGERATOR"),
					" environments will keep food free of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", while carefully placed hygiene stations like ",
					BUILDINGS.PREFABS.WASHBASIN.NAME,
					" or ",
					BUILDINGS.PREFABS.SHOWER.NAME,
					" will prevent the cooks from infecting the food by handling it.\n\nDangerously contaminated food can be sent to compost by ",
					UI.CLICK(UI.ClickType.clicking),
					" the <b>Compost</b> button on the selected item."
				});
			}

			// Token: 0x020039A6 RID: 14758
			public class SUITS
			{
				// Token: 0x0400E8F5 RID: 59637
				public static LocString NAME = "Tutorial: Atmo Suits";

				// Token: 0x0400E8F6 RID: 59638
				public static LocString TOOLTIP = "Notes on using atmo suits";

				// Token: 0x0400E8F7 RID: 59639
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" can be equipped to protect my Duplicants from environmental hazards like extreme ",
					UI.FormatAsLink("Heat", "Heat"),
					", airborne ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", or unbreathable ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					". In order to utilize these suits, I'll need to hook up an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" , then store one of the suits inside.\n\nDuplicants will equip a suit when they walk past the checkpoint in the chosen direction, and will unequip their suit when walking back the opposite way."
				});
			}

			// Token: 0x020039A7 RID: 14759
			public class RADIATION
			{
				// Token: 0x0400E8F8 RID: 59640
				public static LocString NAME = "Tutorial: Radiation";

				// Token: 0x0400E8F9 RID: 59641
				public static LocString TOOLTIP = "Notes on managing radiation";

				// Token: 0x0400E8FA RID: 59642
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Objects such as ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" and ",
					UI.FormatAsLink("Beeta Hives", "BEE"),
					" emit a ",
					UI.FormatAsLink("Radioactive", "RADIOACTIVE"),
					" energy that can be toxic to my Duplicants.\n\nI can use the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to check the scope of the Radiation field. Building thick walls around radiation emitters will dampen the field and protect my Duplicants from getting ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					"."
				});
			}

			// Token: 0x020039A8 RID: 14760
			public class SPACETRAVEL
			{
				// Token: 0x0400E8FB RID: 59643
				public static LocString NAME = "Tutorial: Space Travel";

				// Token: 0x0400E8FC RID: 59644
				public static LocString TOOLTIP = "Notes on traveling in space";

				// Token: 0x0400E8FD RID: 59645
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Building a rocket first requires constructing a ",
					UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
					" and adding modules from the menu. All components of the Rocket Checklist will need to be complete before being capable of launching.\n\nA ",
					UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
					" needs to be built on the surface of a Planetoid in order to use the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to see and set course for new destinations."
				});
			}

			// Token: 0x020039A9 RID: 14761
			public class MORALE
			{
				// Token: 0x0400E8FE RID: 59646
				public static LocString NAME = "Video: Duplicant Morale";

				// Token: 0x0400E8FF RID: 59647
				public static LocString TOOLTIP = "Notes on Duplicant expectations";

				// Token: 0x0400E900 RID: 59648
				public static LocString MESSAGEBODY = "Food, Rooms, Decor, and Recreation all have an effect on Duplicant Morale. Good experiences improve their Morale, while poor experiences lower it. When a Duplicant's Morale is below their Expectations, they will become Stressed.\n\nDuplicants' Expectations will get higher as they are given new Skills, and the colony will have to be improved to keep up their Morale. An overview of Morale and Stress can be viewed on the Vitals screen.\n\nRelated " + UI.FormatAsLink("Tutorial: Stress Management", "MISCELLANEOUSTIPS");
			}

			// Token: 0x020039AA RID: 14762
			public class POWER
			{
				// Token: 0x0400E901 RID: 59649
				public static LocString NAME = "Video: Power Circuits";

				// Token: 0x0400E902 RID: 59650
				public static LocString TOOLTIP = "Notes on managing electricity";

				// Token: 0x0400E903 RID: 59651
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Generators are considered \"Producers\" of Power, while the various buildings and machines in the colony are considered \"Consumers\". Each Consumer will pull a certain wattage from the power circuit it is connected to, which can be checked at any time by ",
					UI.CLICK(UI.ClickType.clicking),
					" the building and going to the Energy Tab.\n\nI can use the Power Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay2),
					" to quickly check the status of all my circuits. If the Consumers are taking more wattage than the Generators are creating, the Batteries will drain and there will be brownouts.\n\nAdditionally, if the Consumers are pulling more wattage through the Wires than the Wires can handle, they will overload and burn out. To correct both these situations, I will need to reorganize my Consumers onto separate circuits."
				});
			}

			// Token: 0x020039AB RID: 14763
			public class BIONICBATTERY
			{
				// Token: 0x0400E904 RID: 59652
				public static LocString NAME = "Tutorial: Powering Bionics";

				// Token: 0x0400E905 RID: 59653
				public static LocString TOOLTIP = "Notes on Duplicant power bank needs";

				// Token: 0x0400E906 RID: 59654
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants require ",
					UI.FormatAsLink("Power Banks", "ELECTROBANK"),
					" to function. Bionic Duplicants who run out of ",
					UI.FormatAsLink("Power", "POWER"),
					" will become incapacitated and require another Duplicant to reboot them.\n\nBasic power banks can be made at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x020039AC RID: 14764
			public class GUNKEDTOILET
			{
				// Token: 0x0400E907 RID: 59655
				public static LocString NAME = "Tutorial: Gunked Toilets";

				// Token: 0x0400E908 RID: 59656
				public static LocString TOOLTIP = "Notes on unclogging toilets";

				// Token: 0x0400E909 RID: 59657
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants can dump built-up ",
					UI.FormatAsLink("Gunk", "LIQUIDGUNK"),
					" into ",
					UI.FormatAsLink("Toilets", "REQUIREMENTCLASSTOILETTYPE"),
					" if no other options are available. This invariably clogs the plumbing, however, and must be removed before facilities can be used by other Duplicants.\n\nBuilding a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" will ensure that Bionic Duplicants can dispose of their waste appropriately."
				});
			}

			// Token: 0x020039AD RID: 14765
			public class SLIPPERYSURFACE
			{
				// Token: 0x0400E90A RID: 59658
				public static LocString NAME = "Tutorial: Wet Surfaces";

				// Token: 0x0400E90B RID: 59659
				public static LocString TOOLTIP = "Notes on slipping hazards";

				// Token: 0x0400E90C RID: 59660
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"My Duplicants may slip and fall on wet surfaces, and Duplicants with bionic systems can experience disruptive glitching.\n\nI can help my colony avoid undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" and potential injury by using the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" to clean up spills. Building ",
					UI.FormatAsLink("Toilets", "REQUIREMENTCLASSTOILETTYPE"),
					" and ",
					UI.FormatAsLink("Gunk Extractors", "GUNKEMPTIER"),
					" can help minimize the incidence of spills."
				});
			}

			// Token: 0x020039AE RID: 14766
			public class BIONICOIL
			{
				// Token: 0x0400E90D RID: 59661
				public static LocString NAME = "Tutorial: Oiling Bionics";

				// Token: 0x0400E90E RID: 59662
				public static LocString TOOLTIP = "Notes on keeping Bionics working efficiently";

				// Token: 0x0400E90F RID: 59663
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants with insufficient ",
					UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
					" will slow down significantly to avoid grinding their gears.\n\nI can keep them running smoothly by supplying ",
					UI.FormatAsLink("Gear Balm", "LUBRICATIONSTICK"),
					", or by building a ",
					UI.FormatAsLink("Lubrication Station", "OILCHANGER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					"."
				});
			}

			// Token: 0x020039AF RID: 14767
			public class DIGGING
			{
				// Token: 0x0400E910 RID: 59664
				public static LocString NAME = "Video: Digging for Resources";

				// Token: 0x0400E911 RID: 59665
				public static LocString TOOLTIP = "Notes on buried riches";

				// Token: 0x0400E912 RID: 59666
				public static LocString MESSAGEBODY = "Everything a colony needs to get going is found in the ground. Instructing Duplicants to dig out areas means we can find food, mine resources to build infrastructure, and clear space for the colony to grow. I can access the Dig Tool with " + UI.FormatAsHotKey(global::Action.Dig) + ", which allows me to select the area where I want my Duplicants to dig.\n\nDuplicants will need to gain the Superhard Digging skill to mine Abyssalite and the Superduperhard Digging skill to mine Diamond and Obsidian. Without the proper skills, these materials will be undiggable.";
			}

			// Token: 0x020039B0 RID: 14768
			public class INSULATION
			{
				// Token: 0x0400E913 RID: 59667
				public static LocString NAME = "Video: Insulation";

				// Token: 0x0400E914 RID: 59668
				public static LocString TOOLTIP = "Notes on effective temperature management";

				// Token: 0x0400E915 RID: 59669
				public static LocString MESSAGEBODY = "The temperature of an environment can have positive or negative effects on the well-being of my Duplicants, as well as the plants and critters in my colony. Selecting " + UI.FormatAsHotKey(global::Action.Overlay3) + " will open the Temperature Overlay where I can check for any hot or cold spots.\n\nI can use a Utility building like an Ice-E Fan or a Space Heater to make an area colder or warmer. However, I will have limited success changing the temperature of a room unless I build the area with insulating tiles to prevent cold or warm air from escaping.";
			}

			// Token: 0x020039B1 RID: 14769
			public class PLUMBING
			{
				// Token: 0x0400E916 RID: 59670
				public static LocString NAME = "Video: Plumbing and Ventilation";

				// Token: 0x0400E917 RID: 59671
				public static LocString TOOLTIP = "Notes on connecting buildings with pipes";

				// Token: 0x0400E918 RID: 59672
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When connecting pipes for plumbing, it is useful to have the Plumbing Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay6),
					" selected. Each building which requires plumbing must have their Building Intake connected to the Output Pipe from a source such as a Liquid Pump. Liquid Pumps must be submerged in liquid and attached to a power source to function.\n\nBuildings often output contaminated water which must flow out of the building through piping from the Output Pipe. The water can then be expelled through a Liquid Vent, or filtered through a Water Sieve for reuse.\n\nVentilation applies the same principles to gases. Select the Ventilation Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay7),
					" to see how gases are being moved around the colony."
				});
			}

			// Token: 0x020039B2 RID: 14770
			public class NEW_AUTOMATION_WARNING
			{
				// Token: 0x0400E919 RID: 59673
				public static LocString NAME = "New Automation Port";

				// Token: 0x0400E91A RID: 59674
				public static LocString TOOLTIP = "This building has a new automation port and is unintentionally connected to an existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME;
			}

			// Token: 0x020039B3 RID: 14771
			public class DTU
			{
				// Token: 0x0400E91B RID: 59675
				public static LocString NAME = "Tutorial: Duplicant Thermal Units";

				// Token: 0x0400E91C RID: 59676
				public static LocString TOOLTIP = "Notes on measuring heat energy";

				// Token: 0x0400E91D RID: 59677
				public static LocString MESSAGEBODY = "My Duplicants measure heat energy in Duplicant Thermal Units or DTU.\n\n1 DTU = 1055.06 J";
			}

			// Token: 0x020039B4 RID: 14772
			public class NOMESSAGES
			{
				// Token: 0x0400E91E RID: 59678
				public static LocString NAME = "";

				// Token: 0x0400E91F RID: 59679
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039B5 RID: 14773
			public class NOALERTS
			{
				// Token: 0x0400E920 RID: 59680
				public static LocString NAME = "";

				// Token: 0x0400E921 RID: 59681
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039B6 RID: 14774
			public class NEWTRAIT
			{
				// Token: 0x0400E922 RID: 59682
				public static LocString NAME = "{0} has developed a trait";

				// Token: 0x0400E923 RID: 59683
				public static LocString TOOLTIP = "{0} has developed the trait(s):\n    • {1}";
			}

			// Token: 0x020039B7 RID: 14775
			public class RESEARCHCOMPLETE
			{
				// Token: 0x0400E924 RID: 59684
				public static LocString NAME = "Research Complete";

				// Token: 0x0400E925 RID: 59685
				public static LocString MESSAGEBODY = "Eureka! We've discovered {0} Technology.\n\nNew buildings have become available:\n  • {1}";

				// Token: 0x0400E926 RID: 59686
				public static LocString TOOLTIP = "{0} research complete!";
			}

			// Token: 0x020039B8 RID: 14776
			public class WORLDDETECTED
			{
				// Token: 0x0400E927 RID: 59687
				public static LocString NAME = "New " + UI.CLUSTERMAP.PLANETOID + " detected";

				// Token: 0x0400E928 RID: 59688
				public static LocString MESSAGEBODY = "My Duplicants' astronomical efforts have uncovered a new " + UI.CLUSTERMAP.PLANETOID + ":\n{0}";

				// Token: 0x0400E929 RID: 59689
				public static LocString TOOLTIP = "{0} discovered";
			}

			// Token: 0x020039B9 RID: 14777
			public class SKILL_POINT_EARNED
			{
				// Token: 0x0400E92A RID: 59690
				public static LocString NAME = "{Duplicant} earned a skill point!";

				// Token: 0x0400E92B RID: 59691
				public static LocString MESSAGEBODY = "These Duplicants have Skill Points that can be spent on new abilities:\n{0}";

				// Token: 0x0400E92C RID: 59692
				public static LocString LINE = "\n• <b>{0}</b>";

				// Token: 0x0400E92D RID: 59693
				public static LocString TOOLTIP = "{Duplicant} has been working hard and is ready to learn a new skill";
			}

			// Token: 0x020039BA RID: 14778
			public class DUPLICANTABSORBED
			{
				// Token: 0x0400E92E RID: 59694
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400E92F RID: 59695
				public static LocString MESSAGEBODY = "The Printing Pod is no longer available for printing.\nCountdown to the next production has been rebooted.";

				// Token: 0x0400E930 RID: 59696
				public static LocString TOOLTIP = "Printing countdown rebooted";
			}

			// Token: 0x020039BB RID: 14779
			public class DUPLICANTDIED
			{
				// Token: 0x0400E931 RID: 59697
				public static LocString NAME = "Duplicants have died";

				// Token: 0x0400E932 RID: 59698
				public static LocString TOOLTIP = "These Duplicants have died:";
			}

			// Token: 0x020039BC RID: 14780
			public class FOODROT
			{
				// Token: 0x0400E933 RID: 59699
				public static LocString NAME = "Food has decayed";

				// Token: 0x0400E934 RID: 59700
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have rotted and are no longer edible:{0}";
			}

			// Token: 0x020039BD RID: 14781
			public class FOODSTALE
			{
				// Token: 0x0400E935 RID: 59701
				public static LocString NAME = "Food has become stale";

				// Token: 0x0400E936 RID: 59702
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have become stale and could rot if not stored:";
			}

			// Token: 0x020039BE RID: 14782
			public class YELLOWALERT
			{
				// Token: 0x0400E937 RID: 59703
				public static LocString NAME = "Yellow Alert";

				// Token: 0x0400E938 RID: 59704
				public static LocString TOOLTIP = "The colony has some top priority tasks to complete before resuming a normal schedule";
			}

			// Token: 0x020039BF RID: 14783
			public class REDALERT
			{
				// Token: 0x0400E939 RID: 59705
				public static LocString NAME = "Red Alert";

				// Token: 0x0400E93A RID: 59706
				public static LocString TOOLTIP = "The colony is prioritizing work over their individual well-being";
			}

			// Token: 0x020039C0 RID: 14784
			public class REACTORMELTDOWN
			{
				// Token: 0x0400E93B RID: 59707
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400E93C RID: 59708
				public static LocString TOOLTIP = "A Research Reactor has overheated and is melting down! Extreme radiation is flooding the area";
			}

			// Token: 0x020039C1 RID: 14785
			public class HEALING
			{
				// Token: 0x0400E93D RID: 59709
				public static LocString NAME = "Healing";

				// Token: 0x0400E93E RID: 59710
				public static LocString TOOLTIP = "This Duplicant is recovering from an injury";
			}

			// Token: 0x020039C2 RID: 14786
			public class UNREACHABLEITEM
			{
				// Token: 0x0400E93F RID: 59711
				public static LocString NAME = "Unreachable resources";

				// Token: 0x0400E940 RID: 59712
				public static LocString TOOLTIP = "Duplicants cannot retrieve these resources:";
			}

			// Token: 0x020039C3 RID: 14787
			public class INVALIDCONSTRUCTIONLOCATION
			{
				// Token: 0x0400E941 RID: 59713
				public static LocString NAME = "Invalid construction location";

				// Token: 0x0400E942 RID: 59714
				public static LocString TOOLTIP = "These buildings cannot be constructed in the planned areas:";
			}

			// Token: 0x020039C4 RID: 14788
			public class MISSINGMATERIALS
			{
				// Token: 0x0400E943 RID: 59715
				public static LocString NAME = "Missing materials";

				// Token: 0x0400E944 RID: 59716
				public static LocString TOOLTIP = "These resources are not available:";
			}

			// Token: 0x020039C5 RID: 14789
			public class BUILDINGOVERHEATED
			{
				// Token: 0x0400E945 RID: 59717
				public static LocString NAME = "Damage: Overheated";

				// Token: 0x0400E946 RID: 59718
				public static LocString TOOLTIP = "Extreme heat is damaging these buildings:";
			}

			// Token: 0x020039C6 RID: 14790
			public class TILECOLLAPSE
			{
				// Token: 0x0400E947 RID: 59719
				public static LocString NAME = "Ceiling Collapse!";

				// Token: 0x0400E948 RID: 59720
				public static LocString TOOLTIP = "Falling material fell on these Duplicants and displaced them:";
			}

			// Token: 0x020039C7 RID: 14791
			public class NO_OXYGEN_GENERATOR
			{
				// Token: 0x0400E949 RID: 59721
				public static LocString NAME = "No " + UI.FormatAsLink("Oxygen Generator", "OXYGEN") + " built";

				// Token: 0x0400E94A RID: 59722
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is not producing any new ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"\n\n",
					UI.FormatAsLink("Oxygen Diffusers", "MINERALDEOXIDIZER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Oxygen Tab", global::Action.Plan2)
				});
			}

			// Token: 0x020039C8 RID: 14792
			public class INSUFFICIENTOXYGENLASTCYCLE
			{
				// Token: 0x0400E94B RID: 59723
				public static LocString NAME = "Insufficient Oxygen generation";

				// Token: 0x0400E94C RID: 59724
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is consuming more ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" than it is producing, and will run out air if I do not increase production.\n\nI should check my existing oxygen production buildings to ensure they're operating correctly\n\n• ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" produced last cycle: {EmittingRate}\n• Consumed last cycle: {ConsumptionRate}"
				});
			}

			// Token: 0x020039C9 RID: 14793
			public class UNREFRIGERATEDFOOD
			{
				// Token: 0x0400E94D RID: 59725
				public static LocString NAME = "Unrefrigerated Food";

				// Token: 0x0400E94E RID: 59726
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items are stored but not refrigerated:\n";
			}

			// Token: 0x020039CA RID: 14794
			public class FOODLOW
			{
				// Token: 0x0400E94F RID: 59727
				public static LocString NAME = "Food shortage";

				// Token: 0x0400E950 RID: 59728
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony's ",
					UI.FormatAsLink("Food", "FOOD"),
					" reserves are low:\n\n    • {0} are currently available\n    • {1} is being consumed per cycle\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x020039CB RID: 14795
			public class NO_MEDICAL_COTS
			{
				// Token: 0x0400E951 RID: 59729
				public static LocString NAME = "No " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION") + " built";

				// Token: 0x0400E952 RID: 59730
				public static LocString TOOLTIP = "There is nowhere for sick Duplicants receive medical care\n\n" + UI.FormatAsLink("Sick Bays", "DOCTORSTATION") + " can be built from the " + UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8);
			}

			// Token: 0x020039CC RID: 14796
			public class NEEDTOILET
			{
				// Token: 0x0400E953 RID: 59731
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " built";

				// Token: 0x0400E954 RID: 59732
				public static LocString TOOLTIP = "My Duplicants have nowhere to relieve themselves\n\n" + UI.FormatAsLink("Outhouses", "OUTHOUSE") + " can be built from the " + UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5);
			}

			// Token: 0x020039CD RID: 14797
			public class NEEDFOOD
			{
				// Token: 0x0400E955 RID: 59733
				public static LocString NAME = "Colony requires a food source";

				// Token: 0x0400E956 RID: 59734
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony will exhaust their supplies without a new ",
					UI.FormatAsLink("Food", "FOOD"),
					" source\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x020039CE RID: 14798
			public class HYGENE_NEEDED
			{
				// Token: 0x0400E957 RID: 59735
				public static LocString NAME = "No " + UI.FormatAsLink("Wash Basin", "WASHBASIN") + " built";

				// Token: 0x0400E958 RID: 59736
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" are spreading in the colony because my Duplicants have nowhere to clean up\n\n",
					UI.FormatAsLink("Wash Basins", "WASHBASIN"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8)
				});
			}

			// Token: 0x020039CF RID: 14799
			public class NEEDSLEEP
			{
				// Token: 0x0400E959 RID: 59737
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED") + " built";

				// Token: 0x0400E95A RID: 59738
				public static LocString TOOLTIP = "My Duplicants would appreciate a place to sleep\n\n" + UI.FormatAsLink("Cots", "BED") + " can be built from the " + UI.FormatAsBuildMenuTab("Furniture Tab", global::Action.Plan9);
			}

			// Token: 0x020039D0 RID: 14800
			public class NEEDENERGYSOURCE
			{
				// Token: 0x0400E95B RID: 59739
				public static LocString NAME = "Colony requires a " + UI.FormatAsLink("Power", "POWER") + " source";

				// Token: 0x0400E95C RID: 59740
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Power", "POWER"),
					" is required to operate electrical buildings\n\n",
					UI.FormatAsLink("Manual Generators", "MANUALGENERATOR"),
					" and ",
					UI.FormatAsLink("Wire", "WIRE"),
					" can be built from the ",
					UI.FormatAsLink("Power Tab", "[3]")
				});
			}

			// Token: 0x020039D1 RID: 14801
			public class RESOURCEMELTED
			{
				// Token: 0x0400E95D RID: 59741
				public static LocString NAME = "Resources melted";

				// Token: 0x0400E95E RID: 59742
				public static LocString TOOLTIP = "These resources have melted:";
			}

			// Token: 0x020039D2 RID: 14802
			public class VENTOVERPRESSURE
			{
				// Token: 0x0400E95F RID: 59743
				public static LocString NAME = "Vent overpressurized";

				// Token: 0x0400E960 RID: 59744
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" systems have exited the ideal ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" range:"
				});
			}

			// Token: 0x020039D3 RID: 14803
			public class VENTBLOCKED
			{
				// Token: 0x0400E961 RID: 59745
				public static LocString NAME = "Vent blocked";

				// Token: 0x0400E962 RID: 59746
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x020039D4 RID: 14804
			public class OUTPUTBLOCKED
			{
				// Token: 0x0400E963 RID: 59747
				public static LocString NAME = "Output blocked";

				// Token: 0x0400E964 RID: 59748
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x020039D5 RID: 14805
			public class BROKENMACHINE
			{
				// Token: 0x0400E965 RID: 59749
				public static LocString NAME = "Building broken";

				// Token: 0x0400E966 RID: 59750
				public static LocString TOOLTIP = "These buildings have taken significant damage and are nonfunctional:";
			}

			// Token: 0x020039D6 RID: 14806
			public class STRUCTURALDAMAGE
			{
				// Token: 0x0400E967 RID: 59751
				public static LocString NAME = "Structural damage";

				// Token: 0x0400E968 RID: 59752
				public static LocString TOOLTIP = "These buildings' structural integrity has been compromised";
			}

			// Token: 0x020039D7 RID: 14807
			public class STRUCTURALCOLLAPSE
			{
				// Token: 0x0400E969 RID: 59753
				public static LocString NAME = "Structural collapse";

				// Token: 0x0400E96A RID: 59754
				public static LocString TOOLTIP = "These buildings have collapsed:";
			}

			// Token: 0x020039D8 RID: 14808
			public class GASCLOUDWARNING
			{
				// Token: 0x0400E96B RID: 59755
				public static LocString NAME = "A gas cloud approaches";

				// Token: 0x0400E96C RID: 59756
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A toxic ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" cloud will soon envelop the colony"
				});
			}

			// Token: 0x020039D9 RID: 14809
			public class GASCLOUDARRIVING
			{
				// Token: 0x0400E96D RID: 59757
				public static LocString NAME = "The colony is entering a cloud of gas";

				// Token: 0x0400E96E RID: 59758
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039DA RID: 14810
			public class GASCLOUDPEAK
			{
				// Token: 0x0400E96F RID: 59759
				public static LocString NAME = "The gas cloud is at its densest point";

				// Token: 0x0400E970 RID: 59760
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039DB RID: 14811
			public class GASCLOUDDEPARTING
			{
				// Token: 0x0400E971 RID: 59761
				public static LocString NAME = "The gas cloud is receding";

				// Token: 0x0400E972 RID: 59762
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039DC RID: 14812
			public class GASCLOUDGONE
			{
				// Token: 0x0400E973 RID: 59763
				public static LocString NAME = "The colony is once again in open space";

				// Token: 0x0400E974 RID: 59764
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039DD RID: 14813
			public class AVAILABLE
			{
				// Token: 0x0400E975 RID: 59765
				public static LocString NAME = "Resource available";

				// Token: 0x0400E976 RID: 59766
				public static LocString TOOLTIP = "These resources have become available:";
			}

			// Token: 0x020039DE RID: 14814
			public class ALLOCATED
			{
				// Token: 0x0400E977 RID: 59767
				public static LocString NAME = "Resource allocated";

				// Token: 0x0400E978 RID: 59768
				public static LocString TOOLTIP = "These resources are reserved for a planned building:";
			}

			// Token: 0x020039DF RID: 14815
			public class INCREASEDEXPECTATIONS
			{
				// Token: 0x0400E979 RID: 59769
				public static LocString NAME = "Duplicants' expectations increased";

				// Token: 0x0400E97A RID: 59770
				public static LocString TOOLTIP = "Duplicants require better amenities over time.\nThese Duplicants have increased their expectations:";
			}

			// Token: 0x020039E0 RID: 14816
			public class NEARLYDRY
			{
				// Token: 0x0400E97B RID: 59771
				public static LocString NAME = "Nearly dry";

				// Token: 0x0400E97C RID: 59772
				public static LocString TOOLTIP = "These Duplicants will dry off soon:";
			}

			// Token: 0x020039E1 RID: 14817
			public class IMMIGRANTSLEFT
			{
				// Token: 0x0400E97D RID: 59773
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400E97E RID: 59774
				public static LocString TOOLTIP = "The care packages have been disintegrated and printable Duplicants have been Oozed";
			}

			// Token: 0x020039E2 RID: 14818
			public class LEVELUP
			{
				// Token: 0x0400E97F RID: 59775
				public static LocString NAME = "Attribute increase";

				// Token: 0x0400E980 RID: 59776
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" have improved:"
				});

				// Token: 0x0400E981 RID: 59777
				public static LocString SUFFIX = " - {0} Attribute Level modifier raised to +{1}";
			}

			// Token: 0x020039E3 RID: 14819
			public class RESETSKILL
			{
				// Token: 0x0400E982 RID: 59778
				public static LocString NAME = "Skills reset";

				// Token: 0x0400E983 RID: 59779
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have had their ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" refunded:"
				});
			}

			// Token: 0x020039E4 RID: 14820
			public class BADROCKETPATH
			{
				// Token: 0x0400E984 RID: 59780
				public static LocString NAME = "Flight Path Obstructed";

				// Token: 0x0400E985 RID: 59781
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A rocket's flight path has been interrupted by a new astronomical discovery.\nOpen the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to reassign rocket paths"
				});
			}

			// Token: 0x020039E5 RID: 14821
			public class SCHEDULE_CHANGED
			{
				// Token: 0x0400E986 RID: 59782
				public static LocString NAME = "{0}: {1}!";

				// Token: 0x0400E987 RID: 59783
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants assigned to ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" have started their <b>{1}</b> block.\n\n{2}\n\nOpen the ",
					UI.PRE_KEYWORD,
					"Schedule Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageSchedule),
					" to change blocks or assignments"
				});
			}

			// Token: 0x020039E6 RID: 14822
			public class GENESHUFFLER
			{
				// Token: 0x0400E988 RID: 59784
				public static LocString NAME = "Genes Shuffled";

				// Token: 0x0400E989 RID: 59785
				public static LocString TOOLTIP = "These Duplicants had their genetic makeup modified:";

				// Token: 0x0400E98A RID: 59786
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020039E7 RID: 14823
			public class HEALINGTRAITGAIN
			{
				// Token: 0x0400E98B RID: 59787
				public static LocString NAME = "New trait";

				// Token: 0x0400E98C RID: 59788
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' injuries weren't set and healed improperly.\nThey developed ",
					UI.PRE_KEYWORD,
					"Traits",
					UI.PST_KEYWORD,
					" as a result:"
				});

				// Token: 0x0400E98D RID: 59789
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020039E8 RID: 14824
			public class COLONYLOST
			{
				// Token: 0x0400E98E RID: 59790
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400E98F RID: 59791
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x020039E9 RID: 14825
			public class FABRICATOREMPTY
			{
				// Token: 0x0400E990 RID: 59792
				public static LocString NAME = "Fabricator idle";

				// Token: 0x0400E991 RID: 59793
				public static LocString TOOLTIP = "These fabricators have no recipes queued:";
			}

			// Token: 0x020039EA RID: 14826
			public class BUILDING_MELTED
			{
				// Token: 0x0400E992 RID: 59794
				public static LocString NAME = "Building melted";

				// Token: 0x0400E993 RID: 59795
				public static LocString TOOLTIP = "Extreme heat has melted these buildings:";
			}

			// Token: 0x020039EB RID: 14827
			public class LARGE_IMPACTOR_GEYSER_ERUPTION
			{
				// Token: 0x0400E994 RID: 59796
				public static LocString NAME = "Geyser triggered";

				// Token: 0x0400E995 RID: 59797
				public static LocString TOOLTIP = "Demolior's impact has triggered the eruption of a natural vent on this world";
			}

			// Token: 0x020039EC RID: 14828
			public class LARGE_IMPACTOR_KEEPSAKE
			{
				// Token: 0x0400E996 RID: 59798
				public static LocString NAME = "Artifact found";

				// Token: 0x0400E997 RID: 59799
				public static LocString TOOLTIP = "An artifact has been found in Demolior's rubble";
			}

			// Token: 0x020039ED RID: 14829
			public class SUIT_DROPPED
			{
				// Token: 0x0400E998 RID: 59800
				public static LocString NAME = "No Docks available";

				// Token: 0x0400E999 RID: 59801
				public static LocString TOOLTIP = "An exosuit was dropped because there were no empty docks available";
			}

			// Token: 0x020039EE RID: 14830
			public class DEATH_SUFFOCATION
			{
				// Token: 0x0400E99A RID: 59802
				public static LocString NAME = "Duplicants suffocated";

				// Token: 0x0400E99B RID: 59803
				public static LocString TOOLTIP = "These Duplicants died from a lack of " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x020039EF RID: 14831
			public class DEATH_FROZENSOLID
			{
				// Token: 0x0400E99C RID: 59804
				public static LocString NAME = "Duplicants have frozen";

				// Token: 0x0400E99D RID: 59805
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extremely low ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020039F0 RID: 14832
			public class DEATH_OVERHEATING
			{
				// Token: 0x0400E99E RID: 59806
				public static LocString NAME = "Duplicants have overheated";

				// Token: 0x0400E99F RID: 59807
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020039F1 RID: 14833
			public class DEATH_STARVATION
			{
				// Token: 0x0400E9A0 RID: 59808
				public static LocString NAME = "Duplicants have starved";

				// Token: 0x0400E9A1 RID: 59809
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from a lack of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020039F2 RID: 14834
			public class DEATH_FELL
			{
				// Token: 0x0400E9A2 RID: 59810
				public static LocString NAME = "Duplicants splattered";

				// Token: 0x0400E9A3 RID: 59811
				public static LocString TOOLTIP = "These Duplicants fell to their deaths:";
			}

			// Token: 0x020039F3 RID: 14835
			public class DEATH_CRUSHED
			{
				// Token: 0x0400E9A4 RID: 59812
				public static LocString NAME = "Duplicants crushed";

				// Token: 0x0400E9A5 RID: 59813
				public static LocString TOOLTIP = "These Duplicants have been crushed:";
			}

			// Token: 0x020039F4 RID: 14836
			public class DEATH_SUFFOCATEDTANKEMPTY
			{
				// Token: 0x0400E9A6 RID: 59814
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400E9A7 RID: 59815
				public static LocString TOOLTIP = "These Duplicants were unable to reach " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and died:";
			}

			// Token: 0x020039F5 RID: 14837
			public class DEATH_SUFFOCATEDAIRTOOHOT
			{
				// Token: 0x0400E9A8 RID: 59816
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400E9A9 RID: 59817
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have asphyxiated in ",
					UI.PRE_KEYWORD,
					"Hot",
					UI.PST_KEYWORD,
					" air:"
				});
			}

			// Token: 0x020039F6 RID: 14838
			public class DEATH_SUFFOCATEDAIRTOOCOLD
			{
				// Token: 0x0400E9AA RID: 59818
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400E9AB RID: 59819
				public static LocString TOOLTIP = "These Duplicants have asphyxiated in " + UI.FormatAsLink("Cold", "HEAT") + " air:";
			}

			// Token: 0x020039F7 RID: 14839
			public class DEATH_DROWNED
			{
				// Token: 0x0400E9AC RID: 59820
				public static LocString NAME = "Duplicants have drowned";

				// Token: 0x0400E9AD RID: 59821
				public static LocString TOOLTIP = "These Duplicants have drowned:";
			}

			// Token: 0x020039F8 RID: 14840
			public class DEATH_ENTOUMBED
			{
				// Token: 0x0400E9AE RID: 59822
				public static LocString NAME = "Duplicants have been entombed";

				// Token: 0x0400E9AF RID: 59823
				public static LocString TOOLTIP = "These Duplicants are trapped and need assistance:";
			}

			// Token: 0x020039F9 RID: 14841
			public class DEATH_RAPIDDECOMPRESSION
			{
				// Token: 0x0400E9B0 RID: 59824
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400E9B1 RID: 59825
				public static LocString TOOLTIP = "These Duplicants died in a low pressure environment:";
			}

			// Token: 0x020039FA RID: 14842
			public class DEATH_OVERPRESSURE
			{
				// Token: 0x0400E9B2 RID: 59826
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400E9B3 RID: 59827
				public static LocString TOOLTIP = "These Duplicants died in a high pressure environment:";
			}

			// Token: 0x020039FB RID: 14843
			public class DEATH_POISONED
			{
				// Token: 0x0400E9B4 RID: 59828
				public static LocString NAME = "Duplicants poisoned";

				// Token: 0x0400E9B5 RID: 59829
				public static LocString TOOLTIP = "These Duplicants died as a result of poisoning:";
			}

			// Token: 0x020039FC RID: 14844
			public class DEATH_DISEASE
			{
				// Token: 0x0400E9B6 RID: 59830
				public static LocString NAME = "Duplicants have succumbed to disease";

				// Token: 0x0400E9B7 RID: 59831
				public static LocString TOOLTIP = "These Duplicants died from an untreated " + UI.FormatAsLink("Disease", "DISEASE") + ":";
			}

			// Token: 0x020039FD RID: 14845
			public class CIRCUIT_OVERLOADED
			{
				// Token: 0x0400E9B8 RID: 59832
				public static LocString NAME = "Circuit Overloaded";

				// Token: 0x0400E9B9 RID: 59833
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.WIRE.NAME + "s melted due to excessive current demands on their circuits";
			}

			// Token: 0x020039FE RID: 14846
			public class LOGIC_CIRCUIT_OVERLOADED
			{
				// Token: 0x0400E9BA RID: 59834
				public static LocString NAME = "Logic Circuit Overloaded";

				// Token: 0x0400E9BB RID: 59835
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s melted due to more bits of data being sent over them than they can support";
			}

			// Token: 0x020039FF RID: 14847
			public class DISCOVERED_SPACE
			{
				// Token: 0x0400E9BC RID: 59836
				public static LocString NAME = "ALERT - Surface Breach";

				// Token: 0x0400E9BD RID: 59837
				public static LocString TOOLTIP = "Amazing!\n\nMy Duplicants have managed to breach the surface of our rocky prison.\n\nI should be careful; the region is extremely inhospitable and I could easily lose resources to the vacuum of space.";
			}

			// Token: 0x02003A00 RID: 14848
			public class COLONY_ACHIEVEMENT_EARNED
			{
				// Token: 0x0400E9BE RID: 59838
				public static LocString NAME = "Colony Achievement earned";

				// Token: 0x0400E9BF RID: 59839
				public static LocString TOOLTIP = "The colony has earned a new achievement.";
			}

			// Token: 0x02003A01 RID: 14849
			public class WARP_PORTAL_DUPE_READY
			{
				// Token: 0x0400E9C0 RID: 59840
				public static LocString NAME = "Duplicant warp ready";

				// Token: 0x0400E9C1 RID: 59841
				public static LocString TOOLTIP = "{dupe} is ready to warp from the " + BUILDINGS.PREFABS.WARPPORTAL.NAME;
			}

			// Token: 0x02003A02 RID: 14850
			public class GENETICANALYSISCOMPLETE
			{
				// Token: 0x0400E9C2 RID: 59842
				public static LocString NAME = "Seed Analysis Complete";

				// Token: 0x0400E9C3 RID: 59843
				public static LocString MESSAGEBODY = "Deeply probing the genes of the {Plant} plant have led to the discovery of a promising new cultivatable mutation:\n\n<b>{Subspecies}</b>\n\n{Info}";

				// Token: 0x0400E9C4 RID: 59844
				public static LocString TOOLTIP = "{Plant} Analysis complete!";
			}

			// Token: 0x02003A03 RID: 14851
			public class NEWMUTANTSEED
			{
				// Token: 0x0400E9C5 RID: 59845
				public static LocString NAME = "New Mutant Seed Discovered";

				// Token: 0x0400E9C6 RID: 59846
				public static LocString TOOLTIP = "A new mutant variety of the {Plant} has been found. Analyze it at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " to learn more!";
			}

			// Token: 0x02003A04 RID: 14852
			public class DUPLICANT_CRASH_LANDED
			{
				// Token: 0x0400E9C7 RID: 59847
				public static LocString NAME = "Duplicant Crash Landed!";

				// Token: 0x0400E9C8 RID: 59848
				public static LocString TOOLTIP = "A Duplicant has successfully crashed an Escape Pod onto the surface of a nearby Planetoid.";
			}

			// Token: 0x02003A05 RID: 14853
			public class POIRESEARCHUNLOCKCOMPLETE
			{
				// Token: 0x0400E9C9 RID: 59849
				public static LocString NAME = "Portal Unlocked!";

				// Token: 0x0400E9CA RID: 59850
				public static LocString MESSAGEBODY = "Eureka! We've decrypted the Research Portal's final transmission. New buildings have become available:\n  {0}\n\nOne file was labeled \"Open This First.\" New Database Entry unlocked.";

				// Token: 0x0400E9CB RID: 59851
				public static LocString TOOLTIP = "{0} unlocked!";

				// Token: 0x0400E9CC RID: 59852
				public static LocString BUTTON_VIEW_LORE = "View entry";
			}

			// Token: 0x02003A06 RID: 14854
			public class POIRESEARCHUNLOCKCOMPLETE_NOLORE
			{
				// Token: 0x0400E9CD RID: 59853
				public static LocString NAME = "Portal Unlocked!";

				// Token: 0x0400E9CE RID: 59854
				public static LocString MESSAGEBODY = "Eureka! We've decrypted the Research Portal's final transmission. New buildings have become available:\n  {0}\n\n";

				// Token: 0x0400E9CF RID: 59855
				public static LocString TOOLTIP = "{0} unlocked!";
			}

			// Token: 0x02003A07 RID: 14855
			public class INCOMINGPREHISTORICASTEROIDNOTIFICATION
			{
				// Token: 0x0400E9D0 RID: 59856
				public static LocString NAME = "DEMOLIOR";

				// Token: 0x0400E9D1 RID: 59857
				public static LocString TOOLTIP = "Incoming Asteroid: <b><color=#ff1111>DEMOLIOR</color></b>\n• Health: {0}/{1}\n• Time until impact: {2}\n\nCollision damage can be avoided by destroying <b><color=#ff1111>DEMOLIOR</color></b> with " + UI.FormatAsLink("Intracosmic Blastshot", "LONGRANGEMISSILE") + " before it makes contact";

				// Token: 0x0400E9D2 RID: 59858
				public static LocString TOGGLE_TOOLTIP = "Click to toggle impact zone preview";
			}

			// Token: 0x02003A08 RID: 14856
			public class LARGEIMPACTORREVEALSEQUENCE
			{
				// Token: 0x02003DAB RID: 15787
				public class RETICLE
				{
					// Token: 0x0400F341 RID: 62273
					public static LocString LARGE_IMPACTOR_NAME = "DEMOLIOR";

					// Token: 0x0400F342 RID: 62274
					public static LocString SIDE_PANEL_TITLE = "IMMINENT THREAT";

					// Token: 0x0400F343 RID: 62275
					public static LocString SIDE_PANEL_DESCRIPTION = "\n\nTIME UNTIL IMPACT: {0} CYCLES.";

					// Token: 0x0400F344 RID: 62276
					public static LocString CALCULATING_IMPACT_ZONE_TEXT = "CALCULATING IMPACT ZONE...";
				}
			}

			// Token: 0x02003A09 RID: 14857
			public class BIONICRESEARCHUNLOCK
			{
				// Token: 0x0400E9D3 RID: 59859
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400E9D4 RID: 59860
				public static LocString MESSAGEBODY = "My new Bionic Duplicant has built-in programming that they've shared with the colony.\n\nNew buildings have become available:\n  • {0}";

				// Token: 0x0400E9D5 RID: 59861
				public static LocString TOOLTIP = "{0} research discovered!";
			}

			// Token: 0x02003A0A RID: 14858
			public class BIONICLIQUIDDAMAGE
			{
				// Token: 0x0400E9D6 RID: 59862
				public static LocString NAME = "Liquid Damage";

				// Token: 0x0400E9D7 RID: 59863
				public static LocString TOOLTIP = "This Duplicant stepped in liquid and damaged their bionic systems!";
			}
		}

		// Token: 0x0200266E RID: 9838
		public class TUTORIAL
		{
			// Token: 0x0400AC1E RID: 44062
			public static LocString DONT_SHOW_AGAIN = "Don't Show Again";
		}

		// Token: 0x0200266F RID: 9839
		public class PLACERS
		{
			// Token: 0x02003A0B RID: 14859
			public class DIGPLACER
			{
				// Token: 0x0400E9D8 RID: 59864
				public static LocString NAME = "Dig";
			}

			// Token: 0x02003A0C RID: 14860
			public class MOPPLACER
			{
				// Token: 0x0400E9D9 RID: 59865
				public static LocString NAME = "Mop";
			}

			// Token: 0x02003A0D RID: 14861
			public class MOVEPICKUPABLEPLACER
			{
				// Token: 0x0400E9DA RID: 59866
				public static LocString NAME = "Relocate Here";

				// Token: 0x0400E9DB RID: 59867
				public static LocString PLACER_STATUS = "Next Destination";

				// Token: 0x0400E9DC RID: 59868
				public static LocString PLACER_STATUS_TOOLTIP = "Click to see where this item will be relocated to";
			}
		}

		// Token: 0x02002670 RID: 9840
		public class MONUMENT_COMPLETE
		{
			// Token: 0x0400AC1F RID: 44063
			public static LocString NAME = "Great Monument";

			// Token: 0x0400AC20 RID: 44064
			public static LocString DESC = "A feat of artistic vision and expert engineering that will doubtless inspire Duplicants for thousands of cycles to come";
		}
	}
}
