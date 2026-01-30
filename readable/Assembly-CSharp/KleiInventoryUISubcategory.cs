using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D41 RID: 3393
public class KleiInventoryUISubcategory : KMonoBehaviour
{
	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06006929 RID: 26921 RVA: 0x0027DC99 File Offset: 0x0027BE99
	public bool IsOpen
	{
		get
		{
			return this.stateExpanded;
		}
	}

	// Token: 0x0600692A RID: 26922 RVA: 0x0027DCA1 File Offset: 0x0027BEA1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.expandButton.onClick = delegate()
		{
			this.ToggleOpen(!this.stateExpanded);
		};
	}

	// Token: 0x0600692B RID: 26923 RVA: 0x0027DCC0 File Offset: 0x0027BEC0
	public void SetIdentity(string label, Sprite icon)
	{
		this.label.SetText(label);
		this.icon.sprite = icon;
	}

	// Token: 0x0600692C RID: 26924 RVA: 0x0027DCDC File Offset: 0x0027BEDC
	public void RefreshDisplay()
	{
		foreach (GameObject gameObject in this.dummyItems)
		{
			gameObject.SetActive(false);
		}
		int num = 0;
		for (int i = 0; i < this.gridLayout.transform.childCount; i++)
		{
			if (this.gridLayout.transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		base.gameObject.SetActive(num != 0);
		int j = 0;
		int num2 = num % this.gridLayout.constraintCount;
		if (num2 > 0)
		{
			j = this.gridLayout.constraintCount - num2;
		}
		while (j > this.dummyItems.Count)
		{
			this.dummyItems.Add(Util.KInstantiateUI(this.dummyPrefab, this.gridLayout.gameObject, false));
		}
		for (int k = 0; k < j; k++)
		{
			this.dummyItems[k].SetActive(true);
			this.dummyItems[k].transform.SetAsLastSibling();
		}
		this.headerLayout.minWidth = base.transform.parent.rectTransform().rect.width - 8f;
	}

	// Token: 0x0600692D RID: 26925 RVA: 0x0027DE3C File Offset: 0x0027C03C
	public void ToggleOpen(bool open)
	{
		this.gridLayout.gameObject.SetActive(open);
		this.stateExpanded = open;
		this.expandButton.ChangeState(this.stateExpanded ? 1 : 0);
	}

	// Token: 0x04004842 RID: 18498
	[SerializeField]
	private GameObject dummyPrefab;

	// Token: 0x04004843 RID: 18499
	public string subcategoryID;

	// Token: 0x04004844 RID: 18500
	public GridLayoutGroup gridLayout;

	// Token: 0x04004845 RID: 18501
	public List<GameObject> dummyItems;

	// Token: 0x04004846 RID: 18502
	[SerializeField]
	private LayoutElement headerLayout;

	// Token: 0x04004847 RID: 18503
	[SerializeField]
	private Image icon;

	// Token: 0x04004848 RID: 18504
	[SerializeField]
	private LocText label;

	// Token: 0x04004849 RID: 18505
	[SerializeField]
	private MultiToggle expandButton;

	// Token: 0x0400484A RID: 18506
	private bool stateExpanded = true;
}
