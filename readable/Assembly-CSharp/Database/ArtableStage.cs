using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F1D RID: 3869
	public class ArtableStage : PermitResource
	{
		// Token: 0x06007C06 RID: 31750 RVA: 0x00302608 File Offset: 0x00300808
		[Obsolete("Use ArtableStage with required/forbidden")]
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, null, null)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x00302664 File Offset: 0x00300864
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x003026C4 File Offset: 0x003008C4
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.animFile), "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(this.prefabId).GetProperName()).Replace("{ArtableQuality}", this.statusItem.GetName(null)));
			return result;
		}

		// Token: 0x0400568C RID: 22156
		public string id;

		// Token: 0x0400568D RID: 22157
		public string anim;

		// Token: 0x0400568E RID: 22158
		public string animFile;

		// Token: 0x0400568F RID: 22159
		public string prefabId;

		// Token: 0x04005690 RID: 22160
		public string symbolName;

		// Token: 0x04005691 RID: 22161
		public int decor;

		// Token: 0x04005692 RID: 22162
		public bool cheerOnComplete;

		// Token: 0x04005693 RID: 22163
		public ArtableStatusItem statusItem;
	}
}
