using System;
using Database;
using UnityEngine;

// Token: 0x02000D56 RID: 3414
public class KleiPermitDioramaVis_BuildingPresentationStand : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069A5 RID: 27045 RVA: 0x00280092 File Offset: 0x0027E292
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069A6 RID: 27046 RVA: 0x0028009A File Offset: 0x0027E29A
	public void ConfigureSetup()
	{
	}

	// Token: 0x060069A7 RID: 27047 RVA: 0x0028009C File Offset: 0x0027E29C
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.anchorPos, KleiPermitVisUtil.GetBuildingDef(permit), this.lastAlignment);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x060069A8 RID: 27048 RVA: 0x002800F8 File Offset: 0x0027E2F8
	public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
	{
		this.lastAlignment = alignment;
		this.anchorPos = new Vector2(alignment.x.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-160f, 160f)), alignment.y.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-156f, 156f)));
		return this;
	}

	// Token: 0x0400489F RID: 18591
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x040048A0 RID: 18592
	private Alignment lastAlignment;

	// Token: 0x040048A1 RID: 18593
	private Vector2 anchorPos;

	// Token: 0x040048A2 RID: 18594
	public const float LEFT = -160f;

	// Token: 0x040048A3 RID: 18595
	public const float TOP = 156f;
}
