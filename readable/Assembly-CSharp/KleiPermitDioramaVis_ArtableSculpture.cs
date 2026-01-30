using System;
using Database;
using UnityEngine;

// Token: 0x02000D4F RID: 3407
public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06006989 RID: 27017 RVA: 0x0027FAB1 File Offset: 0x0027DCB1
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600698A RID: 27018 RVA: 0x0027FAB9 File Offset: 0x0027DCB9
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x0600698B RID: 27019 RVA: 0x0027FACC File Offset: 0x0027DCCC
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x04004891 RID: 18577
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
