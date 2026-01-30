using System;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000FD2 RID: 4050
	public class DECOR
	{
		// Token: 0x04005E42 RID: 24130
		public static int LIT_BONUS = 15;

		// Token: 0x04005E43 RID: 24131
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x020021FA RID: 8698
		public class BONUS
		{
			// Token: 0x04009C54 RID: 40020
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 10,
				radius = 1
			};

			// Token: 0x04009C55 RID: 40021
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 15,
				radius = 2
			};

			// Token: 0x04009C56 RID: 40022
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 20,
				radius = 3
			};

			// Token: 0x04009C57 RID: 40023
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 25,
				radius = 4
			};

			// Token: 0x04009C58 RID: 40024
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04009C59 RID: 40025
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 35,
				radius = 6
			};

			// Token: 0x04009C5A RID: 40026
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 50,
				radius = 7
			};

			// Token: 0x04009C5B RID: 40027
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 80,
				radius = 7
			};

			// Token: 0x04009C5C RID: 40028
			public static readonly EffectorValues TIER8 = new EffectorValues
			{
				amount = 200,
				radius = 8
			};
		}

		// Token: 0x020021FB RID: 8699
		public class PENALTY
		{
			// Token: 0x04009C5D RID: 40029
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04009C5E RID: 40030
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04009C5F RID: 40031
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04009C60 RID: 40032
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04009C61 RID: 40033
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04009C62 RID: 40034
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}

		// Token: 0x020021FC RID: 8700
		public class SPACEARTIFACT
		{
			// Token: 0x04009C63 RID: 40035
			public static readonly ArtifactTier TIER_NONE = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER_NONE.key, DECOR.NONE, 0f);

			// Token: 0x04009C64 RID: 40036
			public static readonly ArtifactTier TIER0 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER0.key, DECOR.BONUS.TIER0, 0.25f);

			// Token: 0x04009C65 RID: 40037
			public static readonly ArtifactTier TIER1 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER1.key, DECOR.BONUS.TIER2, 0.4f);

			// Token: 0x04009C66 RID: 40038
			public static readonly ArtifactTier TIER2 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER2.key, DECOR.BONUS.TIER4, 0.55f);

			// Token: 0x04009C67 RID: 40039
			public static readonly ArtifactTier TIER3 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER3.key, DECOR.BONUS.TIER5, 0.7f);

			// Token: 0x04009C68 RID: 40040
			public static readonly ArtifactTier TIER4 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER4.key, DECOR.BONUS.TIER6, 0.85f);

			// Token: 0x04009C69 RID: 40041
			public static readonly ArtifactTier TIER5 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER5.key, DECOR.BONUS.TIER7, 1f);
		}
	}
}
