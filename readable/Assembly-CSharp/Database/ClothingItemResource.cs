using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F2F RID: 3887
	public class ClothingItemResource : PermitResource
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06007C43 RID: 31811 RVA: 0x0030CC7E File Offset: 0x0030AE7E
		// (set) Token: 0x06007C44 RID: 31812 RVA: 0x0030CC86 File Offset: 0x0030AE86
		public string animFilename { get; private set; }

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06007C45 RID: 31813 RVA: 0x0030CC8F File Offset: 0x0030AE8F
		// (set) Token: 0x06007C46 RID: 31814 RVA: 0x0030CC97 File Offset: 0x0030AE97
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06007C47 RID: 31815 RVA: 0x0030CCA0 File Offset: 0x0030AEA0
		// (set) Token: 0x06007C48 RID: 31816 RVA: 0x0030CCA8 File Offset: 0x0030AEA8
		public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

		// Token: 0x06007C49 RID: 31817 RVA: 0x0030CCB1 File Offset: 0x0030AEB1
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name, desc, category, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.outfitType = outfitType;
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x0030CCE8 File Offset: 0x0030AEE8
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			if (this.AnimFile == null)
			{
				Debug.LogError("Clothing kanim is missing from bundle: " + this.animFilename);
			}
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.CLOTHING_ITEM_FACADE_FOR);
			return result;
		}
	}
}
