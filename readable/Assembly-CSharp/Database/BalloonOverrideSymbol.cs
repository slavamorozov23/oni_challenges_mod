using System;

namespace Database
{
	// Token: 0x02000F26 RID: 3878
	public readonly struct BalloonOverrideSymbol
	{
		// Token: 0x06007C25 RID: 31781 RVA: 0x00303EE4 File Offset: 0x003020E4
		public BalloonOverrideSymbol(string animFileID, string animFileSymbolID)
		{
			if (string.IsNullOrEmpty(animFileID) || string.IsNullOrEmpty(animFileSymbolID))
			{
				this = default(BalloonOverrideSymbol);
				return;
			}
			this.animFileID = animFileID;
			this.animFileSymbolID = animFileSymbolID;
			this.animFile = Assets.GetAnim(animFileID);
			this.symbol = this.animFile.Value.GetData().build.GetSymbol(animFileSymbolID);
		}

		// Token: 0x06007C26 RID: 31782 RVA: 0x00303F58 File Offset: 0x00302158
		public void ApplyTo(BalloonArtist.Instance artist)
		{
			artist.SetBalloonSymbolOverride(this);
		}

		// Token: 0x06007C27 RID: 31783 RVA: 0x00303F66 File Offset: 0x00302166
		public void ApplyTo(BalloonFX.Instance balloon)
		{
			balloon.SetBalloonSymbolOverride(this);
		}

		// Token: 0x040056EA RID: 22250
		public readonly Option<KAnim.Build.Symbol> symbol;

		// Token: 0x040056EB RID: 22251
		public readonly Option<KAnimFile> animFile;

		// Token: 0x040056EC RID: 22252
		public readonly string animFileID;

		// Token: 0x040056ED RID: 22253
		public readonly string animFileSymbolID;
	}
}
