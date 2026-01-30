using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E7A RID: 3706
public class SingleItemSelectionRow : KMonoBehaviour
{
	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x060075E3 RID: 30179 RVA: 0x002D06CC File Offset: 0x002CE8CC
	public virtual string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x060075E4 RID: 30180 RVA: 0x002D06D8 File Offset: 0x002CE8D8
	// (set) Token: 0x060075E5 RID: 30181 RVA: 0x002D06E0 File Offset: 0x002CE8E0
	public Tag InvalidTag { get; protected set; } = GameTags.Void;

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x060075E6 RID: 30182 RVA: 0x002D06E9 File Offset: 0x002CE8E9
	// (set) Token: 0x060075E7 RID: 30183 RVA: 0x002D06F1 File Offset: 0x002CE8F1
	public new Tag tag { get; protected set; }

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x060075E8 RID: 30184 RVA: 0x002D06FA File Offset: 0x002CE8FA
	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x060075E9 RID: 30185 RVA: 0x002D0707 File Offset: 0x002CE907
	// (set) Token: 0x060075EA RID: 30186 RVA: 0x002D070F File Offset: 0x002CE90F
	public bool IsSelected { get; protected set; }

	// Token: 0x060075EB RID: 30187 RVA: 0x002D0718 File Offset: 0x002CE918
	protected override void OnPrefabInit()
	{
		this.regularColor = this.outline.color;
		base.OnPrefabInit();
	}

	// Token: 0x060075EC RID: 30188 RVA: 0x002D0734 File Offset: 0x002CE934
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.button != null)
		{
			this.button.onPointerEnter += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.outlineHighLightColor;
				}
			};
			this.button.onPointerExit += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.regularColor;
				}
			};
			this.button.onClick += this.OnItemClicked;
		}
	}

	// Token: 0x060075ED RID: 30189 RVA: 0x002D079B File Offset: 0x002CE99B
	public virtual void SetVisibleState(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x060075EE RID: 30190 RVA: 0x002D07A9 File Offset: 0x002CE9A9
	protected virtual void OnItemClicked()
	{
		Action<SingleItemSelectionRow> clicked = this.Clicked;
		if (clicked == null)
		{
			return;
		}
		clicked(this);
	}

	// Token: 0x060075EF RID: 30191 RVA: 0x002D07BC File Offset: 0x002CE9BC
	public virtual void SetTag(Tag tag)
	{
		this.tag = tag;
		this.SetText((tag == this.InvalidTag) ? this.InvalidTagTitle : tag.ProperName());
		if (tag != this.InvalidTag)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			this.SetIcon(uisprite.first, uisprite.second);
			return;
		}
		this.SetIcon(null, Color.white);
	}

	// Token: 0x060075F0 RID: 30192 RVA: 0x002D0831 File Offset: 0x002CEA31
	protected virtual void SetText(string assignmentStr)
	{
		this.labelText.text = ((!string.IsNullOrEmpty(assignmentStr)) ? assignmentStr : "-");
	}

	// Token: 0x060075F1 RID: 30193 RVA: 0x002D084E File Offset: 0x002CEA4E
	public virtual void SetSelected(bool selected)
	{
		this.IsSelected = selected;
		this.outline.color = (selected ? this.outlineHighLightColor : this.outlineDefaultColor);
		this.BG.color = (selected ? this.BGHighLightColor : Color.white);
	}

	// Token: 0x060075F2 RID: 30194 RVA: 0x002D088E File Offset: 0x002CEA8E
	protected virtual void SetIcon(Sprite sprite, Color color)
	{
		this.icon.sprite = sprite;
		this.icon.color = color;
		this.icon.gameObject.SetActive(sprite != null);
	}

	// Token: 0x0400519C RID: 20892
	[SerializeField]
	protected Image icon;

	// Token: 0x0400519D RID: 20893
	[SerializeField]
	protected LocText labelText;

	// Token: 0x0400519E RID: 20894
	[SerializeField]
	protected Image BG;

	// Token: 0x0400519F RID: 20895
	[SerializeField]
	protected Image outline;

	// Token: 0x040051A0 RID: 20896
	[SerializeField]
	protected Color outlineHighLightColor = new Color32(168, 74, 121, byte.MaxValue);

	// Token: 0x040051A1 RID: 20897
	[SerializeField]
	protected Color BGHighLightColor = new Color32(168, 74, 121, 80);

	// Token: 0x040051A2 RID: 20898
	[SerializeField]
	protected Color outlineDefaultColor = new Color32(204, 204, 204, byte.MaxValue);

	// Token: 0x040051A3 RID: 20899
	protected Color regularColor = Color.white;

	// Token: 0x040051A4 RID: 20900
	[SerializeField]
	public KButton button;

	// Token: 0x040051A8 RID: 20904
	public Action<SingleItemSelectionRow> Clicked;
}
