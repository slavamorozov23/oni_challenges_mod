using System;

namespace Database
{
	// Token: 0x02000F60 RID: 3936
	public class StickerBombs : ResourceSet<DbStickerBomb>
	{
		// Token: 0x06007CE8 RID: 31976 RVA: 0x00319514 File Offset: 0x00317714
		public StickerBombs(ResourceSet parent) : base("StickerBombs", parent)
		{
			foreach (StickerBombFacadeInfo stickerBombFacadeInfo in Blueprints.Get().all.stickerBombFacades)
			{
				this.Add(stickerBombFacadeInfo.id, stickerBombFacadeInfo.name, stickerBombFacadeInfo.desc, stickerBombFacadeInfo.rarity, stickerBombFacadeInfo.animFile, stickerBombFacadeInfo.sticker, stickerBombFacadeInfo.requiredDlcIds, stickerBombFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x003195AC File Offset: 0x003177AC
		private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			DbStickerBomb dbStickerBomb = new DbStickerBomb(id, name, desc, rarity, animfilename, symbolName, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(dbStickerBomb);
			return dbStickerBomb;
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x003195D9 File Offset: 0x003177D9
		public DbStickerBomb GetRandomSticker()
		{
			return this.resources.GetRandom<DbStickerBomb>();
		}
	}
}
