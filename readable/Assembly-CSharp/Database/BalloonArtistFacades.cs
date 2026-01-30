using System;

namespace Database
{
	// Token: 0x02000F23 RID: 3875
	public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
	{
		// Token: 0x06007C17 RID: 31767 RVA: 0x00303C3C File Offset: 0x00301E3C
		public BalloonArtistFacades(ResourceSet parent) : base("BalloonArtistFacades", parent)
		{
			foreach (BalloonArtistFacadeInfo balloonArtistFacadeInfo in Blueprints.Get().all.balloonArtistFacades)
			{
				this.Add(balloonArtistFacadeInfo.id, balloonArtistFacadeInfo.name, balloonArtistFacadeInfo.desc, balloonArtistFacadeInfo.rarity, balloonArtistFacadeInfo.animFile, balloonArtistFacadeInfo.balloonFacadeType, balloonArtistFacadeInfo.GetRequiredDlcIds(), balloonArtistFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x00303CD4 File Offset: 0x00301ED4
		[Obsolete("Please use Add(...) with required/forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
		{
			this.Add(id, name, desc, rarity, animFile, balloonFacadeType, null, null);
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x00303CF4 File Offset: 0x00301EF4
		[Obsolete("Please use Add(...) with required/forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds)
		{
			this.Add(id, name, desc, rarity, animFile, balloonFacadeType, null, null);
		}

		// Token: 0x06007C1A RID: 31770 RVA: 0x00303D14 File Offset: 0x00301F14
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			BalloonArtistFacadeResource item = new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
