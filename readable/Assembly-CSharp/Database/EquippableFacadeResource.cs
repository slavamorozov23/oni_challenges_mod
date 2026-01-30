using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F3C RID: 3900
	public class EquippableFacadeResource : PermitResource
	{
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06007C69 RID: 31849 RVA: 0x00310FA2 File Offset: 0x0030F1A2
		// (set) Token: 0x06007C6A RID: 31850 RVA: 0x00310FAA File Offset: 0x0030F1AA
		public string BuildOverride { get; private set; }

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06007C6B RID: 31851 RVA: 0x00310FB3 File Offset: 0x0030F1B3
		// (set) Token: 0x06007C6C RID: 31852 RVA: 0x00310FBB File Offset: 0x0030F1BB
		public string DefID { get; private set; }

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06007C6D RID: 31853 RVA: 0x00310FC4 File Offset: 0x0030F1C4
		// (set) Token: 0x06007C6E RID: 31854 RVA: 0x00310FCC File Offset: 0x0030F1CC
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x06007C6F RID: 31855 RVA: 0x00310FD5 File Offset: 0x0030F1D5
		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Equipment, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.DefID = defID;
			this.BuildOverride = buildOverride;
			this.AnimFile = Assets.GetAnim(animFile);
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x0031100C File Offset: 0x0030F20C
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			if (this.AnimFile == null)
			{
				global::Debug.LogError("Facade AnimFile is null: " + this.DefID);
			}
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
		}

		// Token: 0x06007C71 RID: 31857 RVA: 0x0031106C File Offset: 0x0030F26C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			GameObject gameObject = Assets.TryGetPrefab(this.DefID);
			if (gameObject == null || !gameObject)
			{
				result.SetFacadeForPrefabID(this.DefID);
			}
			else
			{
				result.SetFacadeForPrefabName(gameObject.GetProperName());
			}
			return result;
		}
	}
}
