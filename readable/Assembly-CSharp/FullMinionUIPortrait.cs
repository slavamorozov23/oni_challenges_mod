using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000325 RID: 805
public class FullMinionUIPortrait : IEntityConfig
{
	// Token: 0x0600109F RID: 4255 RVA: 0x00062D70 File Offset: 0x00060F70
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(FullMinionUIPortrait.ID, FullMinionUIPortrait.ID, true);
		RectTransform rectTransform = gameObject.AddOrGet<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		LayoutElement layoutElement = gameObject.AddOrGet<LayoutElement>();
		layoutElement.preferredHeight = 100f;
		layoutElement.preferredWidth = 100f;
		gameObject.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
		gameObject.AddOrGet<FaceGraph>();
		gameObject.AddOrGet<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kbatchedAnimController.animScale = 0.5f;
		kbatchedAnimController.setScaleFromAnim = false;
		kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_idle_healthy_kanim"),
			Assets.GetAnim("anim_cheer_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x00062EDC File Offset: 0x000610DC
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x00062EDE File Offset: 0x000610DE
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A96 RID: 2710
	public static string ID = "FullMinionUIPortrait";
}
