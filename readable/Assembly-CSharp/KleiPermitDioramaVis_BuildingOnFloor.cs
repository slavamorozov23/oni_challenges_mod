using System;
using Database;
using UnityEngine;

// Token: 0x02000D54 RID: 3412
public class KleiPermitDioramaVis_BuildingOnFloor : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x0600699D RID: 27037 RVA: 0x0027FE73 File Offset: 0x0027E073
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600699E RID: 27038 RVA: 0x0027FE7B File Offset: 0x0027E07B
	public void ConfigureSetup()
	{
		this.rectTransform = this.buildingKAnim.rectTransform();
		this.defaultScale = this.rectTransform.localScale;
	}

	// Token: 0x0600699F RID: 27039 RVA: 0x0027FEA4 File Offset: 0x0027E0A4
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = (BuildingFacadeResource)permit;
		string place_anim = "place";
		this.buildingKAnim.SetSymbolVisiblity("sweep", false);
		if (buildingFacadeResource.PrefabID == "LiquidPumpingStation")
		{
			this.rectTransform.localScale = Vector3.one * 0.7f;
			this.buildingKAnim.SetSymbolVisiblity("pipe2", false);
			this.buildingKAnim.SetSymbolVisiblity("pipe3", false);
			this.buildingKAnim.SetSymbolVisiblity("pipe4", false);
			place_anim = "place_alt";
		}
		else
		{
			this.rectTransform.localScale = this.defaultScale;
		}
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingFacadeResource);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), place_anim);
	}

	// Token: 0x0400489A RID: 18586
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x0400489B RID: 18587
	private Vector2 defaultScale;

	// Token: 0x0400489C RID: 18588
	private RectTransform rectTransform;
}
