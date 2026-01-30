using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F27 RID: 3879
	public class BalloonOverrideSymbolIter
	{
		// Token: 0x06007C28 RID: 31784 RVA: 0x00303F74 File Offset: 0x00302174
		public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
		{
			global::Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
			this.facade = facade;
			if (facade.IsSome())
			{
				this.index = UnityEngine.Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
			}
			this.Next();
		}

		// Token: 0x06007C29 RID: 31785 RVA: 0x00303FD9 File Offset: 0x003021D9
		public BalloonOverrideSymbol Current()
		{
			return this.current;
		}

		// Token: 0x06007C2A RID: 31786 RVA: 0x00303FE4 File Offset: 0x003021E4
		public BalloonOverrideSymbol Next()
		{
			if (this.facade.IsSome())
			{
				BalloonArtistFacadeResource balloonArtistFacadeResource = this.facade.Unwrap();
				this.current = new BalloonOverrideSymbol(balloonArtistFacadeResource.animFilename, balloonArtistFacadeResource.balloonOverrideSymbolIDs[this.index]);
				this.index = (this.index + 1) % balloonArtistFacadeResource.balloonOverrideSymbolIDs.Length;
				return this.current;
			}
			return default(BalloonOverrideSymbol);
		}

		// Token: 0x040056EE RID: 22254
		public readonly Option<BalloonArtistFacadeResource> facade;

		// Token: 0x040056EF RID: 22255
		private BalloonOverrideSymbol current;

		// Token: 0x040056F0 RID: 22256
		private int index;
	}
}
