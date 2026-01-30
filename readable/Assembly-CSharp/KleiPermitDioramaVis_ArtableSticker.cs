using System;
using Database;
using UnityEngine;

// Token: 0x02000D50 RID: 3408
public class KleiPermitDioramaVis_ArtableSticker : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x0600698D RID: 27021 RVA: 0x0027FB0D File Offset: 0x0027DD0D
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600698E RID: 27022 RVA: 0x0027FB15 File Offset: 0x0027DD15
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x0600698F RID: 27023 RVA: 0x0027FB28 File Offset: 0x0027DD28
	public void ConfigureWith(PermitResource permit)
	{
		DbStickerBomb artablePermit = (DbStickerBomb)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
	}

	// Token: 0x04004892 RID: 18578
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
