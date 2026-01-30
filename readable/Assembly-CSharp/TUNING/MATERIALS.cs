using System;
using System.Linq;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000FC9 RID: 4041
	public class MATERIALS
	{
		// Token: 0x06007EFC RID: 32508 RVA: 0x0032B82C File Offset: 0x00329A2C
		public static string GetMaterialString(string materialCategory)
		{
			string[] array = materialCategory.Split('&', StringSplitOptions.None);
			string result;
			if (array.Length == 1)
			{
				result = UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + materialCategory.ToUpper()), materialCategory);
			}
			else
			{
				result = string.Join(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR, from s in array
				select UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + s.ToUpper()), s));
			}
			return result;
		}

		// Token: 0x04005DD2 RID: 24018
		public const string METAL = "Metal";

		// Token: 0x04005DD3 RID: 24019
		public const string REFINED_METAL = "RefinedMetal";

		// Token: 0x04005DD4 RID: 24020
		public const string GLASS = "Glass";

		// Token: 0x04005DD5 RID: 24021
		public const string TRANSPARENT = "Transparent";

		// Token: 0x04005DD6 RID: 24022
		public const string PLASTIC = "Plastic";

		// Token: 0x04005DD7 RID: 24023
		public const string BUILDABLERAW = "BuildableRaw";

		// Token: 0x04005DD8 RID: 24024
		public const string PRECIOUSROCK = "PreciousRock";

		// Token: 0x04005DD9 RID: 24025
		public const string WOOD = "BuildingWood";

		// Token: 0x04005DDA RID: 24026
		public const string BUILDINGFIBER = "BuildingFiber";

		// Token: 0x04005DDB RID: 24027
		public const string LEAD = "Lead";

		// Token: 0x04005DDC RID: 24028
		public const string INSULATOR = "Insulator";

		// Token: 0x04005DDD RID: 24029
		public const string FOSSILS_TAG = "Fossils";

		// Token: 0x04005DDE RID: 24030
		public static readonly string[] ALL_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x04005DDF RID: 24031
		public static readonly string[] RAW_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x04005DE0 RID: 24032
		public static readonly string[] REFINED_METALS = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x04005DE1 RID: 24033
		public static readonly string[] ALLOYS = new string[]
		{
			"Alloy"
		};

		// Token: 0x04005DE2 RID: 24034
		public static readonly string[] ALL_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x04005DE3 RID: 24035
		public static readonly string[] RAW_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x04005DE4 RID: 24036
		public static readonly string[] RAW_MINERALS_OR_METALS = new string[]
		{
			"BuildableRaw&Metal"
		};

		// Token: 0x04005DE5 RID: 24037
		public static readonly string[] RAW_MINERALS_OR_WOOD = new string[]
		{
			"BuildableRaw&" + GameTags.BuildingWood.ToString()
		};

		// Token: 0x04005DE6 RID: 24038
		public static readonly string[] WOODS = new string[]
		{
			"BuildingWood"
		};

		// Token: 0x04005DE7 RID: 24039
		public static readonly string[] FOSSILS = new string[]
		{
			"Fossils"
		};

		// Token: 0x04005DE8 RID: 24040
		public static readonly string[] REFINED_MINERALS = new string[]
		{
			"BuildableProcessed"
		};

		// Token: 0x04005DE9 RID: 24041
		public static readonly string[] PRECIOUS_ROCKS = new string[]
		{
			"PreciousRock"
		};

		// Token: 0x04005DEA RID: 24042
		public static readonly string[] FARMABLE = new string[]
		{
			"Farmable"
		};

		// Token: 0x04005DEB RID: 24043
		public static readonly string[] EXTRUDABLE = new string[]
		{
			"Extrudable"
		};

		// Token: 0x04005DEC RID: 24044
		public static readonly string[] PLUMBABLE = new string[]
		{
			"Plumbable"
		};

		// Token: 0x04005DED RID: 24045
		public static readonly string[] PLUMBABLE_OR_METALS = new string[]
		{
			"Plumbable&Metal"
		};

		// Token: 0x04005DEE RID: 24046
		public static readonly string[] PLASTICS = new string[]
		{
			"Plastic"
		};

		// Token: 0x04005DEF RID: 24047
		public static readonly string[] GLASSES = new string[]
		{
			"Glass"
		};

		// Token: 0x04005DF0 RID: 24048
		public static readonly string[] TRANSPARENTS = new string[]
		{
			"Transparent"
		};

		// Token: 0x04005DF1 RID: 24049
		public static readonly string[] BUILDING_FIBER = new string[]
		{
			"BuildingFiber"
		};

		// Token: 0x04005DF2 RID: 24050
		public static readonly string[] ANY_BUILDABLE = new string[]
		{
			"BuildableAny"
		};

		// Token: 0x04005DF3 RID: 24051
		public static readonly string[] FLYING_CRITTER_FOOD = new string[]
		{
			"FlyingCritterEdible"
		};

		// Token: 0x04005DF4 RID: 24052
		public static readonly string[] RADIATION_CONTAINMENT = new string[]
		{
			"Metal",
			"Lead"
		};
	}
}
