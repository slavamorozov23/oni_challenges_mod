using System;
using Database;
using UnityEngine;

// Token: 0x02000D57 RID: 3415
public class KleiPermitDioramaVis_BuildingRocket : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069AA RID: 27050 RVA: 0x00280172 File Offset: 0x0027E372
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069AB RID: 27051 RVA: 0x0028017A File Offset: 0x0027E37A
	public void ConfigureSetup()
	{
	}

	// Token: 0x060069AC RID: 27052 RVA: 0x0028017C File Offset: 0x0027E37C
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x040048A4 RID: 18596
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
