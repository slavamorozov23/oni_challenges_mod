using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E7D RID: 3709
public class SingleItemSelectionSideScreen_SelectedItemSection : KMonoBehaviour
{
	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06007614 RID: 30228 RVA: 0x002D1248 File Offset: 0x002CF448
	// (set) Token: 0x06007613 RID: 30227 RVA: 0x002D123F File Offset: 0x002CF43F
	public Tag Item { get; private set; }

	// Token: 0x06007615 RID: 30229 RVA: 0x002D1250 File Offset: 0x002CF450
	public void Clear()
	{
		this.SetItem(null);
	}

	// Token: 0x06007616 RID: 30230 RVA: 0x002D1260 File Offset: 0x002CF460
	public void SetItem(Tag item)
	{
		this.Item = item;
		if (this.Item != GameTags.Void)
		{
			this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.TITLE);
			this.SetContentText(this.Item.ProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(this.Item, "ui", false);
			this.SetImage(uisprite.first, uisprite.second);
			return;
		}
		this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_TITLE);
		this.SetContentText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_MESSAGE);
		this.SetImage(null, Color.white);
	}

	// Token: 0x06007617 RID: 30231 RVA: 0x002D12FD File Offset: 0x002CF4FD
	private void SetTitleText(string text)
	{
		this.title.text = text;
	}

	// Token: 0x06007618 RID: 30232 RVA: 0x002D130B File Offset: 0x002CF50B
	private void SetContentText(string text)
	{
		this.contentText.text = text;
	}

	// Token: 0x06007619 RID: 30233 RVA: 0x002D1319 File Offset: 0x002CF519
	private void SetImage(Sprite sprite, Color color)
	{
		this.image.sprite = sprite;
		this.image.color = color;
		this.image.gameObject.SetActive(sprite != null);
	}

	// Token: 0x040051B5 RID: 20917
	[Header("References")]
	[SerializeField]
	private LocText title;

	// Token: 0x040051B6 RID: 20918
	[SerializeField]
	private LocText contentText;

	// Token: 0x040051B7 RID: 20919
	[SerializeField]
	private KImage image;
}
