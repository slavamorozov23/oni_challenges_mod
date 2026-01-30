using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000335 RID: 821
public class MannequinUIPortrait : IEntityConfig
{
	// Token: 0x060010F9 RID: 4345 RVA: 0x000653BC File Offset: 0x000635BC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MannequinUIPortrait.ID, MannequinUIPortrait.ID, true);
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
		gameObject.AddOrGet<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kbatchedAnimController.animScale = 0.5f;
		kbatchedAnimController.setScaleFromAnim = false;
		kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mannequin_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x000654EB File Offset: 0x000636EB
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x000654ED File Offset: 0x000636ED
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AE5 RID: 2789
	public static string ID = "MannequinUIPortrait";
}
