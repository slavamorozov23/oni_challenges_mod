using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
	// Token: 0x02000F30 RID: 3888
	public class ClothingOutfits : ResourceSet<ClothingOutfitResource>
	{
		// Token: 0x06007C4B RID: 31819 RVA: 0x0030CD50 File Offset: 0x0030AF50
		public ClothingOutfits(ResourceSet parent, ClothingItems items_resource) : base("ClothingOutfits", parent)
		{
			base.Initialize();
			this.resources.AddRange(Blueprints.Get().all.outfits);
			foreach (ClothingOutfitResource clothingOutfitResource in this.resources)
			{
				string[] itemsInOutfit = clothingOutfitResource.itemsInOutfit;
				for (int i = 0; i < itemsInOutfit.Length; i++)
				{
					string itemId = itemsInOutfit[i];
					int num = items_resource.resources.FindIndex((ClothingItemResource e) => e.Id == itemId);
					if (num < 0)
					{
						DebugUtil.DevAssert(false, string.Concat(new string[]
						{
							"Outfit \"",
							clothingOutfitResource.Id,
							"\" contains an item that doesn't exist. Given item id: \"",
							itemId,
							"\""
						}), null);
					}
					else
					{
						ClothingItemResource clothingItemResource = items_resource.resources[num];
						if (clothingItemResource.outfitType != clothingOutfitResource.outfitType)
						{
							DebugUtil.DevAssert(false, string.Format("Outfit \"{0}\" contains an item that has a mis-matched outfit type. Defined outfit's type: \"{1}\". Given item: {{ id: \"{2}\" forOutfitType: \"{3}\" }}", new object[]
							{
								clothingOutfitResource.Id,
								clothingOutfitResource.outfitType,
								itemId,
								clothingItemResource.outfitType
							}), null);
						}
					}
				}
			}
			ClothingOutfitUtility.LoadClothingOutfitData(this);
			this.SortStandardOutfits();
		}

		// Token: 0x06007C4C RID: 31820 RVA: 0x0030CED8 File Offset: 0x0030B0D8
		private void SortStandardOutfits()
		{
			List<string> standard_outfits = new List<string>
			{
				"StandardYellow",
				"StandardRed",
				"StandardGreen",
				"StandardBlue",
				"permit_standard_bionic_outfit",
				"permit_standard_regal_neutronium_outfit"
			};
			this.resources = this.resources.OrderBy(delegate(ClothingOutfitResource item)
			{
				if (!standard_outfits.Contains(item.Id))
				{
					return 1;
				}
				return 0;
			}).ThenBy((ClothingOutfitResource item) => this.resources.IndexOf(item)).ToList<ClothingOutfitResource>();
		}
	}
}
