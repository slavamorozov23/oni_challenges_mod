using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E20 RID: 3616
public class ButtonMenuSideScreen : SideScreenContent
{
	// Token: 0x060072AD RID: 29357 RVA: 0x002BCA9C File Offset: 0x002BAC9C
	public override bool IsValidForTarget(GameObject target)
	{
		ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>();
		if (sidescreenButtonControl == null)
		{
			sidescreenButtonControl = target.GetSMI<ISidescreenButtonControl>();
		}
		return sidescreenButtonControl != null && sidescreenButtonControl.SidescreenEnabled();
	}

	// Token: 0x060072AE RID: 29358 RVA: 0x002BCAC5 File Offset: 0x002BACC5
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].ButtonSideScreenSortOrder();
	}

	// Token: 0x060072AF RID: 29359 RVA: 0x002BCAE3 File Offset: 0x002BACE3
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = new_target.GetAllSMI<ISidescreenButtonControl>();
		this.targets.AddRange(new_target.GetComponents<ISidescreenButtonControl>());
		this.Refresh();
	}

	// Token: 0x060072B0 RID: 29360 RVA: 0x002BCB1C File Offset: 0x002BAD1C
	public GameObject GetHorizontalGroup(int id)
	{
		if (!this.horizontalGroups.ContainsKey(id))
		{
			this.horizontalGroups.Add(id, Util.KInstantiateUI(this.horizontalGroupPrefab, this.buttonContainer.gameObject, true));
		}
		return this.horizontalGroups[id];
	}

	// Token: 0x060072B1 RID: 29361 RVA: 0x002BCB5C File Offset: 0x002BAD5C
	public void CopyLayoutSettings(LayoutElement to, LayoutElement from)
	{
		to.ignoreLayout = from.ignoreLayout;
		to.minWidth = from.minWidth;
		to.minHeight = from.minHeight;
		to.preferredWidth = from.preferredWidth;
		to.preferredHeight = from.preferredHeight;
		to.flexibleWidth = from.flexibleWidth;
		to.flexibleHeight = from.flexibleHeight;
		to.layoutPriority = from.layoutPriority;
	}

	// Token: 0x060072B2 RID: 29362 RVA: 0x002BCBCC File Offset: 0x002BADCC
	private void Refresh()
	{
		while (this.liveButtons.Count < this.targets.Count)
		{
			this.liveButtons.Add(Util.KInstantiateUI(this.buttonPrefab.gameObject, this.buttonContainer.gameObject, true));
		}
		foreach (int key in this.horizontalGroups.Keys)
		{
			this.horizontalGroups[key].SetActive(false);
		}
		for (int i = 0; i < this.liveButtons.Count; i++)
		{
			if (i >= this.targets.Count)
			{
				this.liveButtons[i].SetActive(false);
			}
			else
			{
				if (!this.liveButtons[i].activeSelf)
				{
					this.liveButtons[i].SetActive(true);
				}
				int num = this.targets[i].HorizontalGroupID();
				LayoutElement component = this.liveButtons[i].GetComponent<LayoutElement>();
				KButton componentInChildren = this.liveButtons[i].GetComponentInChildren<KButton>();
				ToolTip componentInChildren2 = this.liveButtons[i].GetComponentInChildren<ToolTip>();
				LocText componentInChildren3 = this.liveButtons[i].GetComponentInChildren<LocText>();
				if (num >= 0)
				{
					GameObject horizontalGroup = this.GetHorizontalGroup(num);
					horizontalGroup.SetActive(true);
					this.liveButtons[i].transform.SetParent(horizontalGroup.transform, false);
					this.CopyLayoutSettings(component, this.horizontalButtonPrefab);
				}
				else
				{
					this.liveButtons[i].transform.SetParent(this.buttonContainer, false);
					this.CopyLayoutSettings(component, this.buttonPrefab);
				}
				componentInChildren.isInteractable = this.targets[i].SidescreenButtonInteractable();
				componentInChildren.ClearOnClick();
				componentInChildren.onClick += this.targets[i].OnSidescreenButtonPressed;
				componentInChildren.onClick += this.Refresh;
				componentInChildren3.SetText(this.targets[i].SidescreenButtonText);
				componentInChildren2.SetSimpleTooltip(this.targets[i].SidescreenButtonTooltip);
			}
		}
	}

	// Token: 0x04004F49 RID: 20297
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	// Token: 0x04004F4A RID: 20298
	public LayoutElement buttonPrefab;

	// Token: 0x04004F4B RID: 20299
	public LayoutElement horizontalButtonPrefab;

	// Token: 0x04004F4C RID: 20300
	public GameObject horizontalGroupPrefab;

	// Token: 0x04004F4D RID: 20301
	public RectTransform buttonContainer;

	// Token: 0x04004F4E RID: 20302
	private List<GameObject> liveButtons = new List<GameObject>();

	// Token: 0x04004F4F RID: 20303
	private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();

	// Token: 0x04004F50 RID: 20304
	private List<ISidescreenButtonControl> targets;
}
