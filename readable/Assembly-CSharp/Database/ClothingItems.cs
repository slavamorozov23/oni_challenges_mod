using System;

namespace Database
{
	// Token: 0x02000F2E RID: 3886
	public class ClothingItems : ResourceSet<ClothingItemResource>
	{
		// Token: 0x06007C40 RID: 31808 RVA: 0x0030CB58 File Offset: 0x0030AD58
		public ClothingItems(ResourceSet parent) : base("ClothingItems", parent)
		{
			base.Initialize();
			foreach (ClothingItemInfo clothingItemInfo in Blueprints.Get().all.clothingItems)
			{
				this.Add(clothingItemInfo.id, clothingItemInfo.name, clothingItemInfo.desc, clothingItemInfo.outfitType, clothingItemInfo.category, clothingItemInfo.rarity, clothingItemInfo.animFile, clothingItemInfo.GetRequiredDlcIds(), clothingItemInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x06007C41 RID: 31809 RVA: 0x0030CBFC File Offset: 0x0030ADFC
		public ClothingItemResource TryResolveAccessoryResource(ResourceGuid AccessoryGuid)
		{
			if (AccessoryGuid.Guid != null)
			{
				string[] array = AccessoryGuid.Guid.Split('.', StringSplitOptions.None);
				if (array.Length != 0)
				{
					string symbol_name = array[array.Length - 1];
					return this.resources.Find((ClothingItemResource ci) => symbol_name.Contains(ci.Id));
				}
			}
			return null;
		}

		// Token: 0x06007C42 RID: 31810 RVA: 0x0030CC50 File Offset: 0x0030AE50
		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			ClothingItemResource item = new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
