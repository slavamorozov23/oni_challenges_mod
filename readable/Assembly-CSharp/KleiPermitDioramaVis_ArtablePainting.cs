using System;
using Database;
using UnityEngine;

// Token: 0x02000D4E RID: 3406
public class KleiPermitDioramaVis_ArtablePainting : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06006985 RID: 27013 RVA: 0x0027F9D2 File Offset: 0x0027DBD2
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06006986 RID: 27014 RVA: 0x0027F9DA File Offset: 0x0027DBDA
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x06006987 RID: 27015 RVA: 0x0027F9F0 File Offset: 0x0027DBF0
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f * (float)buildingDef.HeightInCells / 2f + 176f);
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.9f;
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x0400488F RID: 18575
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04004890 RID: 18576
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
