using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D84 RID: 3460
[AddComponentMenu("KMonoBehaviour/scripts/TableRow")]
public class TableRow : KMonoBehaviour
{
	// Token: 0x06006B8A RID: 27530 RVA: 0x0028CB1C File Offset: 0x0028AD1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.selectMinionButton != null)
		{
			this.selectMinionButton.onClick += this.SelectMinion;
			this.selectMinionButton.onDoubleClick += this.SelectAndFocusMinion;
		}
	}

	// Token: 0x06006B8B RID: 27531 RVA: 0x0028CB6B File Offset: 0x0028AD6B
	public GameObject GetScroller(string scrollerID)
	{
		return this.scrollers[scrollerID];
	}

	// Token: 0x06006B8C RID: 27532 RVA: 0x0028CB79 File Offset: 0x0028AD79
	public GameObject GetScrollerBorder(string scrolledID)
	{
		return this.scrollerBorders[scrolledID];
	}

	// Token: 0x06006B8D RID: 27533 RVA: 0x0028CB88 File Offset: 0x0028AD88
	public void SelectMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.Select(minionIdentity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x06006B8E RID: 27534 RVA: 0x0028CBBC File Offset: 0x0028ADBC
	public void SelectAndFocusMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
	}

	// Token: 0x06006B8F RID: 27535 RVA: 0x0028CC10 File Offset: 0x0028AE10
	public void ConfigureAsWorldDivider(Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		this.scroll_rect = component.GetReference<ScrollRect>("ScrollerScrollRect");
		this.rowType = TableRow.RowType.WorldDivider;
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.scrollerID != "")
			{
				TableColumn value = keyValuePair.Value;
				break;
			}
		}
		this.scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
		{
			if (screen.CheckScrollersDirty())
			{
				return;
			}
			screen.SetScrollersDirty(this.scroll_rect.horizontalNormalizedPosition);
		});
	}

	// Token: 0x06006B90 RID: 27536 RVA: 0x0028CCC8 File Offset: 0x0028AEC8
	public void ConfigureContent(IAssignableIdentity minion, Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		this.minion = minion;
		KImage componentInChildren = base.GetComponentInChildren<KImage>(true);
		componentInChildren.colorStyleSetting = ((minion == null) ? this.style_setting_default : this.style_setting_minion);
		componentInChildren.ColorState = KImage.ColorSelector.Inactive;
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null && minion as StoredMinionIdentity != null)
		{
			component.alpha = 0.6f;
		}
		UnityAction<Vector2> <>9__0;
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			GameObject parent = base.gameObject;
			if (keyValuePair.Value.scrollerID != "")
			{
				foreach (string text in keyValuePair.Value.screen.column_scrollers)
				{
					if (!(text != keyValuePair.Value.scrollerID))
					{
						if (!this.scrollers.ContainsKey(text))
						{
							GameObject gameObject = Util.KInstantiateUI(this.scrollerPrefab, base.gameObject, true);
							this.scroll_rect = gameObject.GetComponent<ScrollRect>();
							this.scrollbar = gameObject.GetComponentInChildren<Scrollbar>();
							this.scroll_rect.horizontalScrollbar = this.scrollbar;
							this.scroll_rect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
							UnityEvent<Vector2> onValueChanged = this.scroll_rect.onValueChanged;
							UnityAction<Vector2> call;
							if ((call = <>9__0) == null)
							{
								call = (<>9__0 = delegate(Vector2 <p0>)
								{
									if (screen.CheckScrollersDirty())
									{
										return;
									}
									screen.SetScrollersDirty(this.scroll_rect.horizontalNormalizedPosition);
								});
							}
							onValueChanged.AddListener(call);
							this.scrollers.Add(text, this.scroll_rect.content.gameObject);
							if (this.scroll_rect.content.transform.parent.Find("Border") != null)
							{
								this.scrollerBorders.Add(text, this.scroll_rect.content.transform.parent.Find("Border").gameObject);
							}
						}
						parent = this.scrollers[text];
					}
				}
			}
			GameObject value;
			if (minion == null)
			{
				if (this.isDefault)
				{
					value = keyValuePair.Value.GetDefaultWidget(parent);
				}
				else
				{
					value = keyValuePair.Value.GetHeaderWidget(parent);
				}
			}
			else
			{
				value = keyValuePair.Value.GetMinionWidget(parent);
			}
			this.widgets.Add(keyValuePair.Value, value);
			keyValuePair.Value.widgets_by_row.Add(this, value);
		}
		this.RefreshColumns(columns);
		if (minion != null)
		{
			base.gameObject.name = minion.GetProperName();
		}
		else if (this.isDefault)
		{
			base.gameObject.name = "defaultRow";
		}
		if (this.selectMinionButton)
		{
			this.selectMinionButton.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06006B91 RID: 27537 RVA: 0x0028CFEC File Offset: 0x0028B1EC
	public void PositionScrollerBorders()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair.Value.rectTransform();
			float width = rectTransform.rect.width;
			keyValuePair.Value.transform.SetParent(base.gameObject.transform);
			rectTransform.anchorMin = (rectTransform.anchorMax = new Vector2(0f, 1f));
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
			RectTransform rectTransform2 = this.scrollers[keyValuePair.Key].transform.parent.rectTransform();
			Vector3 a = this.scrollers[keyValuePair.Key].transform.parent.rectTransform().GetLocalPosition() - new Vector3(rectTransform2.sizeDelta.x / 2f, -1f * (rectTransform2.sizeDelta.y / 2f), 0f);
			a.y = 0f;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 374f);
			rectTransform.SetLocalPosition(a + Vector3.up * rectTransform.GetLocalPosition().y + Vector3.up * -rectTransform.anchoredPosition.y);
		}
	}

	// Token: 0x06006B92 RID: 27538 RVA: 0x0028D1A4 File Offset: 0x0028B3A4
	public void RefreshColumns(Dictionary<string, TableColumn> columns)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.on_load_action != null)
			{
				keyValuePair.Value.on_load_action(this.minion, keyValuePair.Value.widgets_by_row[this]);
			}
		}
	}

	// Token: 0x06006B93 RID: 27539 RVA: 0x0028D224 File Offset: 0x0028B424
	public void RefreshScrollers()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.scrollers)
		{
			ScrollRect component = keyValuePair.Value.transform.parent.GetComponent<ScrollRect>();
			component.GetComponent<LayoutElement>().minWidth = Mathf.Min(768f, component.content.sizeDelta.x);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			rectTransform.sizeDelta = new Vector2(this.scrollers[keyValuePair2.Key].transform.parent.GetComponent<LayoutElement>().minWidth, rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x06006B94 RID: 27540 RVA: 0x0028D334 File Offset: 0x0028B534
	public GameObject GetWidget(TableColumn column)
	{
		if (this.widgets.ContainsKey(column) && this.widgets[column] != null)
		{
			return this.widgets[column];
		}
		global::Debug.LogWarning("Widget is null or row does not contain widget for column " + ((column != null) ? column.ToString() : null));
		return null;
	}

	// Token: 0x06006B95 RID: 27541 RVA: 0x0028D38D File Offset: 0x0028B58D
	public IAssignableIdentity GetIdentity()
	{
		return this.minion;
	}

	// Token: 0x06006B96 RID: 27542 RVA: 0x0028D395 File Offset: 0x0028B595
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets.ContainsValue(widget);
	}

	// Token: 0x06006B97 RID: 27543 RVA: 0x0028D3A4 File Offset: 0x0028B5A4
	public void Clear()
	{
		foreach (KeyValuePair<TableColumn, GameObject> keyValuePair in this.widgets)
		{
			keyValuePair.Key.widgets_by_row.Remove(this);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040049C5 RID: 18885
	public TableRow.RowType rowType;

	// Token: 0x040049C6 RID: 18886
	private IAssignableIdentity minion;

	// Token: 0x040049C7 RID: 18887
	private Dictionary<TableColumn, GameObject> widgets = new Dictionary<TableColumn, GameObject>();

	// Token: 0x040049C8 RID: 18888
	private Dictionary<string, GameObject> scrollers = new Dictionary<string, GameObject>();

	// Token: 0x040049C9 RID: 18889
	private Dictionary<string, GameObject> scrollerBorders = new Dictionary<string, GameObject>();

	// Token: 0x040049CA RID: 18890
	public bool isDefault;

	// Token: 0x040049CB RID: 18891
	public KButton selectMinionButton;

	// Token: 0x040049CC RID: 18892
	[SerializeField]
	private ColorStyleSetting style_setting_default;

	// Token: 0x040049CD RID: 18893
	[SerializeField]
	private ColorStyleSetting style_setting_minion;

	// Token: 0x040049CE RID: 18894
	[SerializeField]
	private GameObject scrollerPrefab;

	// Token: 0x040049CF RID: 18895
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x040049D0 RID: 18896
	public ScrollRect scroll_rect;

	// Token: 0x02001FD0 RID: 8144
	public enum RowType
	{
		// Token: 0x040093E5 RID: 37861
		Header,
		// Token: 0x040093E6 RID: 37862
		Default,
		// Token: 0x040093E7 RID: 37863
		Minion,
		// Token: 0x040093E8 RID: 37864
		StoredMinon,
		// Token: 0x040093E9 RID: 37865
		WorldDivider
	}
}
