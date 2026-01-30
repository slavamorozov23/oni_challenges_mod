using System;
using Database;
using UnityEngine;

// Token: 0x02000D5B RID: 3419
public class KleiPermitDioramaVis_MonumentPart : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069C0 RID: 27072 RVA: 0x0028052E File Offset: 0x0027E72E
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069C1 RID: 27073 RVA: 0x00280536 File Offset: 0x0027E736
	public void ConfigureSetup()
	{
	}

	// Token: 0x060069C2 RID: 27074 RVA: 0x00280538 File Offset: 0x0027E738
	public void ConfigureWith(PermitResource permit)
	{
		MonumentPartResource monumentPermit = (MonumentPartResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, monumentPermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f + (float)(buildingDef.HeightInCells * 6));
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.55f;
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x040048B8 RID: 18616
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x040048B9 RID: 18617
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
