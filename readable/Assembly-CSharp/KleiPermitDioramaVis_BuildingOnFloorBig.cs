using System;
using Database;
using UnityEngine;

// Token: 0x02000D55 RID: 3413
public class KleiPermitDioramaVis_BuildingOnFloorBig : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069A1 RID: 27041 RVA: 0x0027FF89 File Offset: 0x0027E189
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069A2 RID: 27042 RVA: 0x0027FF91 File Offset: 0x0027E191
	public void ConfigureSetup()
	{
		this.defaultAnchoredPosition = this.buildingKAnim.rectTransform().anchoredPosition;
	}

	// Token: 0x060069A3 RID: 27043 RVA: 0x0027FFAC File Offset: 0x0027E1AC
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = (BuildingFacadeResource)permit;
		this.buildingKAnim.SetSymbolVisiblity("booster", false);
		this.buildingKAnim.SetSymbolVisiblity("blue_light_bloom", false);
		this.buildingKAnim.rectTransform().anchoredPosition = this.defaultAnchoredPosition;
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.825f;
		string place_anim = "place";
		if (buildingFacadeResource.PrefabID == "SteamTurbine2")
		{
			this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, 140f);
			place_anim = "place_alt";
		}
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingFacadeResource);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), place_anim);
	}

	// Token: 0x0400489D RID: 18589
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x0400489E RID: 18590
	private Vector2 defaultAnchoredPosition;
}
