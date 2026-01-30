using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F43 RID: 3907
	public class MonumentPartResource : PermitResource
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06007C87 RID: 31879 RVA: 0x00314301 File Offset: 0x00312501
		// (set) Token: 0x06007C88 RID: 31880 RVA: 0x00314309 File Offset: 0x00312509
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06007C89 RID: 31881 RVA: 0x00314312 File Offset: 0x00312512
		// (set) Token: 0x06007C8A RID: 31882 RVA: 0x0031431A File Offset: 0x0031251A
		public string SymbolName { get; private set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06007C8B RID: 31883 RVA: 0x00314323 File Offset: 0x00312523
		// (set) Token: 0x06007C8C RID: 31884 RVA: 0x0031432B File Offset: 0x0031252B
		public string State { get; private set; }

		// Token: 0x06007C8D RID: 31885 RVA: 0x00314334 File Offset: 0x00312534
		public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

		// Token: 0x06007C8E RID: 31886 RVA: 0x00314374 File Offset: 0x00312574
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.MONUMENT_PART_FACADE_FOR);
			return result;
		}

		// Token: 0x04005ACA RID: 23242
		public MonumentPartResource.Part part;

		// Token: 0x020021A0 RID: 8608
		public enum Part
		{
			// Token: 0x04009AE1 RID: 39649
			Bottom,
			// Token: 0x04009AE2 RID: 39650
			Middle,
			// Token: 0x04009AE3 RID: 39651
			Top
		}
	}
}
