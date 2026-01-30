using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F4A RID: 3914
	public static class PermitRarityExtensions
	{
		// Token: 0x06007C9D RID: 31901 RVA: 0x00314DB0 File Offset: 0x00312FB0
		public static string GetLocStringName(this PermitRarity rarity)
		{
			switch (rarity)
			{
			case PermitRarity.Unknown:
				return UI.PERMIT_RARITY.UNKNOWN;
			case PermitRarity.Universal:
				return UI.PERMIT_RARITY.UNIVERSAL;
			case PermitRarity.Loyalty:
				return UI.PERMIT_RARITY.LOYALTY;
			case PermitRarity.Common:
				return UI.PERMIT_RARITY.COMMON;
			case PermitRarity.Decent:
				return UI.PERMIT_RARITY.DECENT;
			case PermitRarity.Nifty:
				return UI.PERMIT_RARITY.NIFTY;
			case PermitRarity.Splendid:
				return UI.PERMIT_RARITY.SPLENDID;
			}
			DebugUtil.DevAssert(false, string.Format("Couldn't get name for rarity {0}", rarity), null);
			return "-";
		}
	}
}
