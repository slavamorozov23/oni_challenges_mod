using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E8E RID: 3726
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x060076E1 RID: 30433 RVA: 0x002D5435 File Offset: 0x002D3635
	// (set) Token: 0x060076E2 RID: 30434 RVA: 0x002D543D File Offset: 0x002D363D
	public bool ArrowExpanded { get; private set; }

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x060076E3 RID: 30435 RVA: 0x002D5446 File Offset: 0x002D3646
	// (set) Token: 0x060076E4 RID: 30436 RVA: 0x002D544E File Offset: 0x002D364E
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

	// Token: 0x060076E5 RID: 30437 RVA: 0x002D5458 File Offset: 0x002D3658
	public TreeFilterableSideScreenRow.State GetState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			if (this.parent.GetElementTagAcceptedState(treeFilterableSideScreenElement.GetElementTag()))
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (this.rowElements.Count <= 0)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		return TreeFilterableSideScreenRow.State.On;
	}

	// Token: 0x060076E6 RID: 30438 RVA: 0x002D54EC File Offset: 0x002D36EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.checkBoxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (this.parent.CurrentSearchValue == "")
			{
				TreeFilterableSideScreenRow.State state = this.GetState();
				if (state > TreeFilterableSideScreenRow.State.Mixed)
				{
					if (state == TreeFilterableSideScreenRow.State.On)
					{
						this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
						return;
					}
				}
				else
				{
					this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
				}
			}
		}));
	}

	// Token: 0x060076E7 RID: 30439 RVA: 0x002D551B File Offset: 0x002D371B
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SetArrowToggleState(this.GetState() > TreeFilterableSideScreenRow.State.Off);
	}

	// Token: 0x060076E8 RID: 30440 RVA: 0x002D5532 File Offset: 0x002D3732
	protected override void OnCmpDisable()
	{
		this.SetArrowToggleState(false);
		base.OnCmpDisable();
	}

	// Token: 0x060076E9 RID: 30441 RVA: 0x002D5541 File Offset: 0x002D3741
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060076EA RID: 30442 RVA: 0x002D5549 File Offset: 0x002D3749
	public void UpdateCheckBoxVisualState()
	{
		this.checkBoxToggle.ChangeState((int)this.GetState());
		this.visualDirty = false;
	}

	// Token: 0x060076EB RID: 30443 RVA: 0x002D5564 File Offset: 0x002D3764
	public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			for (int i = 0; i < this.rowElements.Count; i++)
			{
				this.rowElements[i].SetCheckBox(false);
			}
			break;
		case TreeFilterableSideScreenRow.State.On:
			for (int j = 0; j < this.rowElements.Count; j++)
			{
				this.rowElements[j].SetCheckBox(true);
			}
			break;
		}
		this.visualDirty = true;
	}

	// Token: 0x060076EC RID: 30444 RVA: 0x002D55DE File Offset: 0x002D37DE
	private void ArrowToggleClicked()
	{
		this.SetArrowToggleState(!this.ArrowExpanded);
		this.RefreshArrowToggleState();
	}

	// Token: 0x060076ED RID: 30445 RVA: 0x002D55F5 File Offset: 0x002D37F5
	public void SetArrowToggleState(bool state)
	{
		this.ArrowExpanded = state;
		this.RefreshArrowToggleState();
	}

	// Token: 0x060076EE RID: 30446 RVA: 0x002D5604 File Offset: 0x002D3804
	private void RefreshArrowToggleState()
	{
		this.arrowToggle.ChangeState(this.ArrowExpanded ? 1 : 0);
		this.elementGroup.SetActive(this.ArrowExpanded);
		this.bgImg.enabled = this.ArrowExpanded;
	}

	// Token: 0x060076EF RID: 30447 RVA: 0x002D563F File Offset: 0x002D383F
	private void ArrowToggleDisabledClick()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x060076F0 RID: 30448 RVA: 0x002D5651 File Offset: 0x002D3851
	public void ShowToggleBox(bool show)
	{
		this.checkBoxToggle.gameObject.SetActive(show);
	}

	// Token: 0x060076F1 RID: 30449 RVA: 0x002D5664 File Offset: 0x002D3864
	private void OnElementSelectionChanged(Tag t, bool state)
	{
		if (state)
		{
			this.parent.AddTag(t);
		}
		else
		{
			this.parent.RemoveTag(t);
		}
		this.visualDirty = true;
	}

	// Token: 0x060076F2 RID: 30450 RVA: 0x002D568C File Offset: 0x002D388C
	public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
	{
		this.subTags.Clear();
		this.rowElements.Clear();
		this.elementName.text = mainElementTag.ProperName();
		this.bgImg.enabled = false;
		string simpleTooltip = string.Format(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, mainElementTag.ProperName());
		this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		if (filterMap.Count == 0)
		{
			if (this.elementGroup.activeInHierarchy)
			{
				this.elementGroup.SetActive(false);
			}
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleDisabledClick);
			this.arrowToggle.ChangeState(0);
		}
		else
		{
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleClicked);
			this.arrowToggle.ChangeState(0);
			foreach (KeyValuePair<Tag, bool> keyValuePair in filterMap)
			{
				TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
				freeElement.Parent = this.parent;
				freeElement.SetTag(keyValuePair.Key);
				freeElement.SetCheckBox(keyValuePair.Value);
				freeElement.OnSelectionChanged = new Action<Tag, bool>(this.OnElementSelectionChanged);
				freeElement.SetCheckBox(this.parent.IsTagAllowed(keyValuePair.Key));
				this.rowElements.Add(freeElement);
				this.subTags.Add(keyValuePair.Key);
			}
		}
		this.UpdateCheckBoxVisualState();
	}

	// Token: 0x060076F3 RID: 30451 RVA: 0x002D582C File Offset: 0x002D3A2C
	public void RefreshRowElements()
	{
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			treeFilterableSideScreenElement.SetCheckBox(this.parent.IsTagAllowed(treeFilterableSideScreenElement.GetElementTag()));
		}
	}

	// Token: 0x060076F4 RID: 30452 RVA: 0x002D5890 File Offset: 0x002D3A90
	public void FilterAgainstSearch(Tag thisCategoryTag, string search)
	{
		bool flag = false;
		bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		search = search.ToUpper();
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			bool flag3 = flag2 || treeFilterableSideScreenElement.GetElementTag().ProperNameStripLink().ToUpper().Contains(search.ToUpper());
			treeFilterableSideScreenElement.gameObject.SetActive(flag3);
			flag = (flag || flag3);
		}
		base.gameObject.SetActive(flag);
		if (search != "" && flag && this.arrowToggle.CurrentState == 0)
		{
			this.SetArrowToggleState(true);
		}
	}

	// Token: 0x04005247 RID: 21063
	public bool visualDirty;

	// Token: 0x04005248 RID: 21064
	public bool standardCommodity = true;

	// Token: 0x04005249 RID: 21065
	[SerializeField]
	private LocText elementName;

	// Token: 0x0400524A RID: 21066
	[SerializeField]
	private GameObject elementGroup;

	// Token: 0x0400524B RID: 21067
	[SerializeField]
	private MultiToggle checkBoxToggle;

	// Token: 0x0400524C RID: 21068
	[SerializeField]
	private MultiToggle arrowToggle;

	// Token: 0x0400524D RID: 21069
	[SerializeField]
	private KImage bgImg;

	// Token: 0x0400524E RID: 21070
	private List<Tag> subTags = new List<Tag>();

	// Token: 0x0400524F RID: 21071
	private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();

	// Token: 0x04005250 RID: 21072
	private TreeFilterableSideScreen parent;

	// Token: 0x020020F2 RID: 8434
	public enum State
	{
		// Token: 0x04009799 RID: 38809
		Off,
		// Token: 0x0400979A RID: 38810
		Mixed,
		// Token: 0x0400979B RID: 38811
		On
	}
}
