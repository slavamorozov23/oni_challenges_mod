using System;
using UnityEngine;

// Token: 0x02000C5C RID: 3164
public class HoverTextScreen : KScreen
{
	// Token: 0x0600601A RID: 24602 RVA: 0x00234193 File Offset: 0x00232393
	public static void DestroyInstance()
	{
		HoverTextScreen.Instance = null;
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x0023419B File Offset: 0x0023239B
	protected override void OnActivate()
	{
		base.OnActivate();
		HoverTextScreen.Instance = this;
		this.drawer = new HoverTextDrawer(this.skin.skin, base.GetComponent<RectTransform>());
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x002341C8 File Offset: 0x002323C8
	public HoverTextDrawer BeginDrawing()
	{
		Vector2 zero = Vector2.zero;
		Vector2 screenPoint = KInputManager.GetMousePos();
		RectTransform rectTransform = base.transform.parent as RectTransform;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, base.transform.parent.GetComponent<Canvas>().worldCamera, out zero);
		zero.x += rectTransform.sizeDelta.x / 2f;
		zero.y -= rectTransform.sizeDelta.y / 2f;
		this.drawer.BeginDrawing(zero);
		return this.drawer;
	}

	// Token: 0x0600601D RID: 24605 RVA: 0x00234260 File Offset: 0x00232460
	private void Update()
	{
		bool enabled = PlayerController.Instance.ActiveTool.ShowHoverUI();
		this.drawer.SetEnabled(enabled);
	}

	// Token: 0x0600601E RID: 24606 RVA: 0x0023428C File Offset: 0x0023248C
	public Sprite GetSprite(string byName)
	{
		foreach (Sprite sprite in this.HoverIcons)
		{
			if (sprite != null && sprite.name == byName)
			{
				return sprite;
			}
		}
		global::Debug.LogWarning("No icon named " + byName + " was found on HoverTextScreen.prefab");
		return null;
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x002342E1 File Offset: 0x002324E1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.drawer.Cleanup();
	}

	// Token: 0x0400403D RID: 16445
	[SerializeField]
	private HoverTextSkin skin;

	// Token: 0x0400403E RID: 16446
	public Sprite[] HoverIcons;

	// Token: 0x0400403F RID: 16447
	public HoverTextDrawer drawer;

	// Token: 0x04004040 RID: 16448
	public static HoverTextScreen Instance;
}
