using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F25 RID: 3877
	public class BalloonArtistFacadeResource : PermitResource
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06007C1B RID: 31771 RVA: 0x00303D40 File Offset: 0x00301F40
		// (set) Token: 0x06007C1C RID: 31772 RVA: 0x00303D48 File Offset: 0x00301F48
		public string animFilename { get; private set; }

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06007C1D RID: 31773 RVA: 0x00303D51 File Offset: 0x00301F51
		// (set) Token: 0x06007C1E RID: 31774 RVA: 0x00303D59 File Offset: 0x00301F59
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x06007C1F RID: 31775 RVA: 0x00303D64 File Offset: 0x00301F64
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name, desc, PermitCategory.JoyResponse, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.balloonFacadeType = balloonFacadeType;
			Db.Get().Accessories.AddAccessories(id, this.AnimFile);
			this.balloonOverrideSymbolIDs = this.GetBalloonOverrideSymbolIDs();
			Debug.Assert(this.balloonOverrideSymbolIDs.Length != 0);
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x00303DD8 File Offset: 0x00301FD8
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.BALLOON_ARTIST_FACADE_FOR);
			return result;
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x00303E1C File Offset: 0x0030201C
		public BalloonOverrideSymbol GetNextOverride()
		{
			int num = this.nextSymbolIndex;
			this.nextSymbolIndex = (this.nextSymbolIndex + 1) % this.balloonOverrideSymbolIDs.Length;
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[num]);
		}

		// Token: 0x06007C22 RID: 31778 RVA: 0x00303E5A File Offset: 0x0030205A
		public BalloonOverrideSymbolIter GetSymbolIter()
		{
			return new BalloonOverrideSymbolIter(this);
		}

		// Token: 0x06007C23 RID: 31779 RVA: 0x00303E67 File Offset: 0x00302067
		public BalloonOverrideSymbol GetOverrideAt(int index)
		{
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[index]);
		}

		// Token: 0x06007C24 RID: 31780 RVA: 0x00303E7C File Offset: 0x0030207C
		private string[] GetBalloonOverrideSymbolIDs()
		{
			KAnim.Build build = this.AnimFile.GetData().build;
			BalloonArtistFacadeType balloonArtistFacadeType = this.balloonFacadeType;
			string[] result;
			if (balloonArtistFacadeType != BalloonArtistFacadeType.Single)
			{
				if (balloonArtistFacadeType != BalloonArtistFacadeType.ThreeSet)
				{
					throw new NotImplementedException();
				}
				result = new string[]
				{
					"body1",
					"body2",
					"body3"
				};
			}
			else
			{
				result = new string[]
				{
					"body"
				};
			}
			return result;
		}

		// Token: 0x040056E7 RID: 22247
		private BalloonArtistFacadeType balloonFacadeType;

		// Token: 0x040056E8 RID: 22248
		public readonly string[] balloonOverrideSymbolIDs;

		// Token: 0x040056E9 RID: 22249
		public int nextSymbolIndex;
	}
}
