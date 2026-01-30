using System;

namespace Database
{
	// Token: 0x02000F5F RID: 3935
	public class DbStickerBomb : PermitResource
	{
		// Token: 0x06007CE6 RID: 31974 RVA: 0x0031948B File Offset: 0x0031768B
		public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.id = id;
			this.sticker = sticker;
			this.animFile = Assets.GetAnim(animfilename);
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x003194C0 File Offset: 0x003176C0
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			return new PermitPresentationInfo
			{
				sprite = Def.GetUISpriteFromMultiObjectAnim(this.animFile, string.Format("{0}_{1}", "idle_sticker", this.sticker), false, string.Format("{0}_{1}", "sticker", this.sticker))
			};
		}

		// Token: 0x04005BB9 RID: 23481
		public string id;

		// Token: 0x04005BBA RID: 23482
		public string sticker;

		// Token: 0x04005BBB RID: 23483
		public KAnimFile animFile;

		// Token: 0x04005BBC RID: 23484
		private const string stickerAnimPrefix = "idle_sticker";

		// Token: 0x04005BBD RID: 23485
		private const string stickerSymbolPrefix = "sticker";
	}
}
