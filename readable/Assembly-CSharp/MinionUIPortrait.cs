using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200033B RID: 827
public class MinionUIPortrait : IEntityConfig
{
	// Token: 0x0600111A RID: 4378 RVA: 0x00065D34 File Offset: 0x00063F34
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionUIPortrait.ID, MinionUIPortrait.ID, true);
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
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_idle_healthy_kanim"),
			Assets.GetAnim("anim_cheer_kanim"),
			Assets.GetAnim("inventory_screen_dupe_kanim"),
			Assets.GetAnim("anim_react_wave_shy_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x00065EBD File Offset: 0x000640BD
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x00065EBF File Offset: 0x000640BF
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AF0 RID: 2800
	public static string ID = "MinionUIPortrait";
}
