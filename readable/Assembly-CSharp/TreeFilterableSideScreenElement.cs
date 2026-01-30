using System;
using UnityEngine;

// Token: 0x02000E8D RID: 3725
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
	// Token: 0x060076D4 RID: 30420 RVA: 0x002D5240 File Offset: 0x002D3440
	public Tag GetElementTag()
	{
		return this.elementTag;
	}

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x060076D5 RID: 30421 RVA: 0x002D5248 File Offset: 0x002D3448
	public bool IsSelected
	{
		get
		{
			return this.checkBox.CurrentState == 1;
		}
	}

	// Token: 0x060076D6 RID: 30422 RVA: 0x002D5258 File Offset: 0x002D3458
	public MultiToggle GetCheckboxToggle()
	{
		return this.checkBox;
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x060076D7 RID: 30423 RVA: 0x002D5260 File Offset: 0x002D3460
	// (set) Token: 0x060076D8 RID: 30424 RVA: 0x002D5268 File Offset: 0x002D3468
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x060076D9 RID: 30425 RVA: 0x002D5271 File Offset: 0x002D3471
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
		this.checkBox.onClick = new System.Action(this.CheckBoxClicked);
		this.initialized = true;
	}

	// Token: 0x060076DA RID: 30426 RVA: 0x002D52B0 File Offset: 0x002D34B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
	}

	// Token: 0x060076DB RID: 30427 RVA: 0x002D52C0 File Offset: 0x002D34C0
	public Sprite GetStorageObjectSprite(Tag t)
	{
		Sprite result = null;
		GameObject prefab = Assets.GetPrefab(t);
		if (prefab != null)
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x060076DC RID: 30428 RVA: 0x002D530C File Offset: 0x002D350C
	public void SetSprite(Tag t)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(t, "ui", false);
		this.elementImg.sprite = uisprite.first;
		this.elementImg.color = uisprite.second;
		this.elementImg.gameObject.SetActive(true);
	}

	// Token: 0x060076DD RID: 30429 RVA: 0x002D5360 File Offset: 0x002D3560
	public void SetTag(Tag newTag)
	{
		this.Initialize();
		this.elementTag = newTag;
		this.SetSprite(this.elementTag);
		string text = this.elementTag.ProperName();
		if (this.parent.IsStorage)
		{
			float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
			text = text + ": " + GameUtil.GetFormattedMass(amountInStorage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		}
		this.elementName.text = text;
	}

	// Token: 0x060076DE RID: 30430 RVA: 0x002D53D7 File Offset: 0x002D35D7
	private void CheckBoxClicked()
	{
		this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
	}

	// Token: 0x060076DF RID: 30431 RVA: 0x002D53F3 File Offset: 0x002D35F3
	public void SetCheckBox(bool checkBoxState)
	{
		this.checkBox.ChangeState(checkBoxState ? 1 : 0);
		this.checkBoxImg.enabled = checkBoxState;
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
		}
	}

	// Token: 0x0400523F RID: 21055
	[SerializeField]
	private LocText elementName;

	// Token: 0x04005240 RID: 21056
	[SerializeField]
	private MultiToggle checkBox;

	// Token: 0x04005241 RID: 21057
	[SerializeField]
	private KImage elementImg;

	// Token: 0x04005242 RID: 21058
	private KImage checkBoxImg;

	// Token: 0x04005243 RID: 21059
	private Tag elementTag;

	// Token: 0x04005244 RID: 21060
	public Action<Tag, bool> OnSelectionChanged;

	// Token: 0x04005245 RID: 21061
	private TreeFilterableSideScreen parent;

	// Token: 0x04005246 RID: 21062
	private bool initialized;
}
