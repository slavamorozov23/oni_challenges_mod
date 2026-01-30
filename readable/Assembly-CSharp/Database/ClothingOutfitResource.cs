using System;
using System.Linq;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F31 RID: 3889
	public class ClothingOutfitResource : Resource, IHasDlcRestrictions
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06007C4D RID: 31821 RVA: 0x0030CF72 File Offset: 0x0030B172
		// (set) Token: 0x06007C4E RID: 31822 RVA: 0x0030CF7A File Offset: 0x0030B17A
		public string[] itemsInOutfit { get; private set; }

		// Token: 0x06007C4F RID: 31823 RVA: 0x0030CF83 File Offset: 0x0030B183
		public ClothingOutfitResource(string id, string[] items_in_outfit, string name, ClothingOutfitUtility.OutfitType outfitType, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name)
		{
			this.itemsInOutfit = items_in_outfit;
			this.outfitType = outfitType;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x06007C50 RID: 31824 RVA: 0x0030CFAC File Offset: 0x0030B1AC
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x06007C51 RID: 31825 RVA: 0x0030CFD7 File Offset: 0x0030B1D7
		public string GetDlcIdFrom()
		{
			if (this.requiredDlcIds == null)
			{
				return null;
			}
			return this.requiredDlcIds.Last<string>();
		}

		// Token: 0x06007C52 RID: 31826 RVA: 0x0030CFEE File Offset: 0x0030B1EE
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007C53 RID: 31827 RVA: 0x0030CFF6 File Offset: 0x0030B1F6
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x040058E7 RID: 22759
		public ClothingOutfitUtility.OutfitType outfitType;

		// Token: 0x040058E9 RID: 22761
		public string[] requiredDlcIds;

		// Token: 0x040058EA RID: 22762
		public string[] forbiddenDlcIds;
	}
}
