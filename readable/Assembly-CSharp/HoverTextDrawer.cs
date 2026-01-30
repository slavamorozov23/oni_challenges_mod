using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C5B RID: 3163
public class HoverTextDrawer
{
	// Token: 0x0600600D RID: 24589 RVA: 0x00233B70 File Offset: 0x00231D70
	public HoverTextDrawer(HoverTextDrawer.Skin skin, RectTransform parent)
	{
		this.shadowBars = new HoverTextDrawer.Pool<Image>(skin.shadowBarWidget.gameObject, parent);
		this.selectBorders = new HoverTextDrawer.Pool<Image>(skin.selectBorderWidget.gameObject, parent);
		this.textWidgets = new HoverTextDrawer.Pool<LocText>(skin.textWidget.gameObject, parent);
		this.iconWidgets = new HoverTextDrawer.Pool<Image>(skin.iconWidget.gameObject, parent);
		this.skin = skin;
	}

	// Token: 0x0600600E RID: 24590 RVA: 0x00233BE6 File Offset: 0x00231DE6
	public void SetEnabled(bool enabled)
	{
		this.shadowBars.SetEnabled(enabled);
		this.textWidgets.SetEnabled(enabled);
		this.iconWidgets.SetEnabled(enabled);
		this.selectBorders.SetEnabled(enabled);
	}

	// Token: 0x0600600F RID: 24591 RVA: 0x00233C18 File Offset: 0x00231E18
	public void BeginDrawing(Vector2 root_pos)
	{
		this.rootPos = root_pos + this.skin.baseOffset;
		if (this.skin.enableDebugOffset)
		{
			this.rootPos += this.skin.debugOffset;
		}
		this.currentPos = this.rootPos;
		this.textWidgets.BeginDrawing();
		this.iconWidgets.BeginDrawing();
		this.shadowBars.BeginDrawing();
		this.selectBorders.BeginDrawing();
		this.firstShadowBar = true;
		this.minLineHeight = 0;
	}

	// Token: 0x06006010 RID: 24592 RVA: 0x00233CAB File Offset: 0x00231EAB
	public void EndDrawing()
	{
		this.shadowBars.EndDrawing();
		this.iconWidgets.EndDrawing();
		this.textWidgets.EndDrawing();
		this.selectBorders.EndDrawing();
	}

	// Token: 0x06006011 RID: 24593 RVA: 0x00233CDC File Offset: 0x00231EDC
	public void DrawText(string text, TextStyleSetting style, Color color, bool override_color = true)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		LocText widget = this.textWidgets.Draw(this.currentPos).widget;
		Color color2 = Color.white;
		if (widget.textStyleSetting != style)
		{
			widget.textStyleSetting = style;
			widget.ApplySettings();
		}
		if (style != null)
		{
			color2 = style.textColor;
		}
		if (override_color)
		{
			color2 = color;
		}
		widget.color = color2;
		if (widget.text != text)
		{
			widget.text = text;
			widget.KForceUpdateDirty();
		}
		this.currentPos.x = this.currentPos.x + widget.renderedWidth;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
		this.minLineHeight = (int)Mathf.Max((float)this.minLineHeight, widget.renderedHeight);
	}

	// Token: 0x06006012 RID: 24594 RVA: 0x00233DB1 File Offset: 0x00231FB1
	public void DrawText(string text, TextStyleSetting style)
	{
		this.DrawText(text, style, Color.white, false);
	}

	// Token: 0x06006013 RID: 24595 RVA: 0x00233DC1 File Offset: 0x00231FC1
	public void AddIndent(int width = 36)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.x = this.currentPos.x + (float)width;
	}

	// Token: 0x06006014 RID: 24596 RVA: 0x00233DE4 File Offset: 0x00231FE4
	public void NewLine(int min_height = 26)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.y = this.currentPos.y - (float)Math.Max(min_height, this.minLineHeight);
		this.currentPos.x = this.rootPos.x;
		this.minLineHeight = 0;
	}

	// Token: 0x06006015 RID: 24597 RVA: 0x00233E38 File Offset: 0x00232038
	public void DrawIcon(Sprite icon, int min_width = 18)
	{
		this.DrawIcon(icon, Color.white, min_width, 2);
	}

	// Token: 0x06006016 RID: 24598 RVA: 0x00233E48 File Offset: 0x00232048
	public void DrawIcon(Sprite icon, Color color, int image_size = 18, int horizontal_spacing = 2)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.AddIndent(horizontal_spacing);
		HoverTextDrawer.Pool<Image>.Entry entry = this.iconWidgets.Draw(this.currentPos + this.skin.shadowImageOffset);
		entry.widget.sprite = icon;
		entry.widget.color = this.skin.shadowImageColor;
		entry.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		HoverTextDrawer.Pool<Image>.Entry entry2 = this.iconWidgets.Draw(this.currentPos);
		entry2.widget.sprite = icon;
		entry2.widget.color = color;
		entry2.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		this.AddIndent(horizontal_spacing);
		this.currentPos.x = this.currentPos.x + (float)image_size;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
	}

	// Token: 0x06006017 RID: 24599 RVA: 0x00233F34 File Offset: 0x00232134
	public void BeginShadowBar(bool selected = false)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		if (this.firstShadowBar)
		{
			this.firstShadowBar = false;
		}
		else
		{
			this.NewLine(22);
		}
		this.isShadowBarSelected = selected;
		this.shadowStartPos = this.currentPos;
		this.maxShadowX = this.rootPos.x;
	}

	// Token: 0x06006018 RID: 24600 RVA: 0x00233F8C File Offset: 0x0023218C
	public void EndShadowBar()
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.NewLine(22);
		HoverTextDrawer.Pool<Image>.Entry entry = this.shadowBars.Draw(this.currentPos);
		entry.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x, this.skin.shadowBarBorder.y);
		entry.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f);
		if (this.isShadowBarSelected)
		{
			HoverTextDrawer.Pool<Image>.Entry entry2 = this.selectBorders.Draw(this.currentPos);
			entry2.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x - this.skin.selectBorder.x, this.skin.shadowBarBorder.y + this.skin.selectBorder.y);
			entry2.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f + this.skin.selectBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f + this.skin.selectBorder.y * 2f);
		}
	}

	// Token: 0x06006019 RID: 24601 RVA: 0x00234170 File Offset: 0x00232370
	public void Cleanup()
	{
		this.shadowBars.Cleanup();
		this.textWidgets.Cleanup();
		this.iconWidgets.Cleanup();
	}

	// Token: 0x04004031 RID: 16433
	public HoverTextDrawer.Skin skin;

	// Token: 0x04004032 RID: 16434
	private Vector2 currentPos;

	// Token: 0x04004033 RID: 16435
	private Vector2 rootPos;

	// Token: 0x04004034 RID: 16436
	private Vector2 shadowStartPos;

	// Token: 0x04004035 RID: 16437
	private float maxShadowX;

	// Token: 0x04004036 RID: 16438
	private bool firstShadowBar;

	// Token: 0x04004037 RID: 16439
	private bool isShadowBarSelected;

	// Token: 0x04004038 RID: 16440
	private int minLineHeight;

	// Token: 0x04004039 RID: 16441
	private HoverTextDrawer.Pool<LocText> textWidgets;

	// Token: 0x0400403A RID: 16442
	private HoverTextDrawer.Pool<Image> iconWidgets;

	// Token: 0x0400403B RID: 16443
	private HoverTextDrawer.Pool<Image> shadowBars;

	// Token: 0x0400403C RID: 16444
	private HoverTextDrawer.Pool<Image> selectBorders;

	// Token: 0x02001E05 RID: 7685
	[Serializable]
	public class Skin
	{
		// Token: 0x04008D00 RID: 36096
		public Vector2 baseOffset;

		// Token: 0x04008D01 RID: 36097
		public LocText textWidget;

		// Token: 0x04008D02 RID: 36098
		public Image iconWidget;

		// Token: 0x04008D03 RID: 36099
		public Vector2 shadowImageOffset;

		// Token: 0x04008D04 RID: 36100
		public Color shadowImageColor;

		// Token: 0x04008D05 RID: 36101
		public Image shadowBarWidget;

		// Token: 0x04008D06 RID: 36102
		public Image selectBorderWidget;

		// Token: 0x04008D07 RID: 36103
		public Vector2 shadowBarBorder;

		// Token: 0x04008D08 RID: 36104
		public Vector2 selectBorder;

		// Token: 0x04008D09 RID: 36105
		public bool drawWidgets;

		// Token: 0x04008D0A RID: 36106
		public bool enableProfiling;

		// Token: 0x04008D0B RID: 36107
		public bool enableDebugOffset;

		// Token: 0x04008D0C RID: 36108
		public bool drawInProgressHoverText;

		// Token: 0x04008D0D RID: 36109
		public Vector2 debugOffset;
	}

	// Token: 0x02001E06 RID: 7686
	private class Pool<WidgetType> where WidgetType : MonoBehaviour
	{
		// Token: 0x0600B2D9 RID: 45785 RVA: 0x003E1B8C File Offset: 0x003DFD8C
		public Pool(GameObject prefab, RectTransform master_root)
		{
			this.prefab = prefab;
			GameObject gameObject = new GameObject(typeof(WidgetType).Name);
			this.root = gameObject.AddComponent<RectTransform>();
			this.root.SetParent(master_root);
			this.root.anchoredPosition = Vector2.zero;
			this.root.anchorMin = Vector2.zero;
			this.root.anchorMax = Vector2.one;
			this.root.sizeDelta = Vector2.zero;
			gameObject.AddComponent<CanvasGroup>();
		}

		// Token: 0x0600B2DA RID: 45786 RVA: 0x003E1C28 File Offset: 0x003DFE28
		public HoverTextDrawer.Pool<WidgetType>.Entry Draw(Vector2 pos)
		{
			HoverTextDrawer.Pool<WidgetType>.Entry entry;
			if (this.drawnWidgets < this.entries.Count)
			{
				entry = this.entries[this.drawnWidgets];
				if (!entry.widget.gameObject.activeSelf)
				{
					entry.widget.gameObject.SetActive(true);
				}
			}
			else
			{
				GameObject gameObject = Util.KInstantiateUI(this.prefab, this.root.gameObject, false);
				gameObject.SetActive(true);
				entry.widget = gameObject.GetComponent<WidgetType>();
				entry.rect = gameObject.GetComponent<RectTransform>();
				this.entries.Add(entry);
			}
			entry.rect.anchoredPosition = new Vector2(pos.x, pos.y);
			this.drawnWidgets++;
			return entry;
		}

		// Token: 0x0600B2DB RID: 45787 RVA: 0x003E1CF9 File Offset: 0x003DFEF9
		public void BeginDrawing()
		{
			this.drawnWidgets = 0;
		}

		// Token: 0x0600B2DC RID: 45788 RVA: 0x003E1D04 File Offset: 0x003DFF04
		public void EndDrawing()
		{
			for (int i = this.drawnWidgets; i < this.entries.Count; i++)
			{
				if (this.entries[i].widget.gameObject.activeSelf)
				{
					this.entries[i].widget.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600B2DD RID: 45789 RVA: 0x003E1D6F File Offset: 0x003DFF6F
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.root.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				return;
			}
			this.root.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}

		// Token: 0x0600B2DE RID: 45790 RVA: 0x003E1DAC File Offset: 0x003DFFAC
		public void Cleanup()
		{
			foreach (HoverTextDrawer.Pool<WidgetType>.Entry entry in this.entries)
			{
				UnityEngine.Object.Destroy(entry.widget.gameObject);
			}
			this.entries.Clear();
		}

		// Token: 0x04008D0E RID: 36110
		private GameObject prefab;

		// Token: 0x04008D0F RID: 36111
		private RectTransform root;

		// Token: 0x04008D10 RID: 36112
		private List<HoverTextDrawer.Pool<WidgetType>.Entry> entries = new List<HoverTextDrawer.Pool<WidgetType>.Entry>();

		// Token: 0x04008D11 RID: 36113
		private int drawnWidgets;

		// Token: 0x02002A57 RID: 10839
		public struct Entry
		{
			// Token: 0x0400BB1B RID: 47899
			public WidgetType widget;

			// Token: 0x0400BB1C RID: 47900
			public RectTransform rect;
		}
	}
}
