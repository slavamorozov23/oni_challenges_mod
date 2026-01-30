using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F47 RID: 3911
	public static class PermitCategories
	{
		// Token: 0x06007C91 RID: 31889 RVA: 0x00314967 File Offset: 0x00312B67
		public static string GetDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName;
		}

		// Token: 0x06007C92 RID: 31890 RVA: 0x00314979 File Offset: 0x00312B79
		public static string GetUppercaseDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName.ToUpper();
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x00314990 File Offset: 0x00312B90
		public static string GetIconName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].iconName;
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x003149A4 File Offset: 0x00312BA4
		public static PermitCategory GetCategoryForId(string id)
		{
			try
			{
				return (PermitCategory)Enum.Parse(typeof(PermitCategory), id);
			}
			catch (ArgumentException)
			{
				Debug.LogError(id + " is not a valid PermitCategory.");
			}
			return PermitCategory.Equipment;
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x003149F0 File Offset: 0x00312BF0
		public static Option<ClothingOutfitUtility.OutfitType> GetOutfitTypeFor(PermitCategory permitCategory)
		{
			return PermitCategories.CategoryInfos[permitCategory].outfitType;
		}

		// Token: 0x04005AFD RID: 23293
		private static Dictionary<PermitCategory, PermitCategories.CategoryInfo> CategoryInfos = new Dictionary<PermitCategory, PermitCategories.CategoryInfo>
		{
			{
				PermitCategory.Equipment,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.EQUIPMENT, "icon_inventory_equipment", Option.None)
			},
			{
				PermitCategory.DupeTops,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_TOPS, "icon_inventory_tops", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeBottoms,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_BOTTOMS, "icon_inventory_bottoms", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_GLOVES, "icon_inventory_gloves", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_SHOES, "icon_inventory_shoes", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeHats,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_HATS, "icon_inventory_hats", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeAccessories,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_ACCESSORIES, "icon_inventory_accessories", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.AtmoSuitHelmet,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_HELMET, "icon_inventory_atmosuit_helmet", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBody,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BODY, "icon_inventory_atmosuit_body", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_GLOVES, "icon_inventory_atmosuit_gloves", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBelt,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BELT, "icon_inventory_atmosuit_belt", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_SHOES, "icon_inventory_atmosuit_boots", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.Building,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.BUILDINGS, "icon_inventory_buildings", Option.None)
			},
			{
				PermitCategory.Critter,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.CRITTERS, "icon_inventory_critters", Option.None)
			},
			{
				PermitCategory.Sweepy,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.SWEEPYS, "icon_inventory_sweepys", Option.None)
			},
			{
				PermitCategory.Duplicant,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPLICANTS, "icon_inventory_duplicants", Option.None)
			},
			{
				PermitCategory.Artwork,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ARTWORKS, "icon_inventory_artworks", Option.None)
			},
			{
				PermitCategory.JoyResponse,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSE, "icon_inventory_joyresponses", ClothingOutfitUtility.OutfitType.JoyResponse)
			},
			{
				PermitCategory.JetSuitHelmet,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_HELMET, "icon_inventory_jetsuit_helmet", ClothingOutfitUtility.OutfitType.JetSuit)
			},
			{
				PermitCategory.JetSuitBody,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_BODY, "icon_inventory_jetsuit_body", ClothingOutfitUtility.OutfitType.JetSuit)
			},
			{
				PermitCategory.JetSuitGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_GLOVES, "icon_inventory_jetsuit_gloves", ClothingOutfitUtility.OutfitType.JetSuit)
			},
			{
				PermitCategory.JetSuitShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_SHOES, "icon_inventory_jetsuit_boots", ClothingOutfitUtility.OutfitType.JetSuit)
			}
		};

		// Token: 0x020021A2 RID: 8610
		private class CategoryInfo
		{
			// Token: 0x0600BDB9 RID: 48569 RVA: 0x004062F6 File Offset: 0x004044F6
			public CategoryInfo(string displayName, string iconName, Option<ClothingOutfitUtility.OutfitType> outfitType)
			{
				this.displayName = displayName;
				this.iconName = iconName;
				this.outfitType = outfitType;
			}

			// Token: 0x04009AE6 RID: 39654
			public string displayName;

			// Token: 0x04009AE7 RID: 39655
			public string iconName;

			// Token: 0x04009AE8 RID: 39656
			public Option<ClothingOutfitUtility.OutfitType> outfitType;
		}
	}
}
