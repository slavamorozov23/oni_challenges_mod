using System;
using System.Collections.Generic;
using System.Linq;

namespace TUNING
{
	// Token: 0x02000FE1 RID: 4065
	public class STORAGEFILTERS
	{
		// Token: 0x04005F67 RID: 24423
		public static List<Tag> DEHYDRATED = new List<Tag>
		{
			GameTags.Dehydrated
		};

		// Token: 0x04005F68 RID: 24424
		public static List<Tag> FOOD = new List<Tag>
		{
			GameTags.Edible,
			GameTags.CookingIngredient,
			GameTags.Medicine
		};

		// Token: 0x04005F69 RID: 24425
		public static List<Tag> BAGABLE_CREATURES = new List<Tag>
		{
			GameTags.BagableCreature
		};

		// Token: 0x04005F6A RID: 24426
		public static List<Tag> SWIMMING_CREATURES = new List<Tag>
		{
			GameTags.SwimmingCreature
		};

		// Token: 0x04005F6B RID: 24427
		public static List<Tag> NOT_EDIBLE_SOLIDS = new List<Tag>
		{
			GameTags.Alloy,
			GameTags.RefinedMetal,
			GameTags.Metal,
			GameTags.BuildableRaw,
			GameTags.BuildableProcessed,
			GameTags.Farmable,
			GameTags.Organics,
			GameTags.Compostable,
			GameTags.Seed,
			GameTags.Agriculture,
			GameTags.Filter,
			GameTags.ConsumableOre,
			GameTags.Sublimating,
			GameTags.Liquifiable,
			GameTags.IndustrialProduct,
			GameTags.IndustrialIngredient,
			GameTags.TechComponents,
			GameTags.MedicalSupplies,
			GameTags.Clothes,
			GameTags.ManufacturedMaterial,
			GameTags.Egg,
			GameTags.RareMaterials,
			GameTags.Other,
			GameTags.StoryTraitResource,
			GameTags.Dehydrated,
			GameTags.ChargedPortableBattery,
			GameTags.BionicUpgrade
		};

		// Token: 0x04005F6C RID: 24428
		public static List<Tag> SPECIAL_STORAGE = new List<Tag>
		{
			GameTags.Clothes,
			GameTags.Egg,
			GameTags.Sublimating
		};

		// Token: 0x04005F6D RID: 24429
		public static List<Tag> STORAGE_LOCKERS_STANDARD = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Union(new List<Tag>
		{
			GameTags.Medicine
		}).ToList<Tag>();

		// Token: 0x04005F6E RID: 24430
		public static List<Tag> STORAGE_SOLID_CARGO_BAY = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Union(new List<Tag>
		{
			GameTags.Medicine
		}).Except(new List<Tag>
		{
			GameTags.TechComponents
		}).ToList<Tag>();

		// Token: 0x04005F6F RID: 24431
		public static List<Tag> POWER_BANKS = new List<Tag>
		{
			GameTags.ChargedPortableBattery
		};

		// Token: 0x04005F70 RID: 24432
		public static List<Tag> LIQUIDS = new List<Tag>
		{
			GameTags.Liquid
		};

		// Token: 0x04005F71 RID: 24433
		public static List<Tag> SO_DATABANKS = new List<Tag>
		{
			"OrbitalResearchDatabank"
		};

		// Token: 0x04005F72 RID: 24434
		public static List<Tag> GASES = new List<Tag>
		{
			GameTags.Breathable,
			GameTags.Unbreathable
		};

		// Token: 0x04005F73 RID: 24435
		public static List<Tag> PAYLOADS = new List<Tag>
		{
			"RailGunPayload"
		};

		// Token: 0x04005F74 RID: 24436
		public static Tag[] SOLID_TRANSFER_ARM_CONVEYABLE = new List<Tag>
		{
			GameTags.Seed,
			GameTags.CropSeed
		}.Concat(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Concat(STORAGEFILTERS.FOOD).Concat(STORAGEFILTERS.PAYLOADS)).ToArray<Tag>();
	}
}
