using System;
using Database;
using UnityEngine;

// Token: 0x02000D52 RID: 3410
public class KleiPermitDioramaVis_BuildingHangingHook : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06006995 RID: 27029 RVA: 0x0027FC89 File Offset: 0x0027DE89
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06006996 RID: 27030 RVA: 0x0027FC91 File Offset: 0x0027DE91
	public void ConfigureSetup()
	{
	}

	// Token: 0x06006997 RID: 27031 RVA: 0x0027FC94 File Offset: 0x0027DE94
	public void ConfigureWith(PermitResource permit)
	{
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource)permit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit), Alignment.Top());
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x04004896 RID: 18582
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04004897 RID: 18583
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
