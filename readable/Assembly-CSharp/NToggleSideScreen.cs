using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E58 RID: 3672
public class NToggleSideScreen : SideScreenContent
{
	// Token: 0x06007468 RID: 29800 RVA: 0x002C6E02 File Offset: 0x002C5002
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06007469 RID: 29801 RVA: 0x002C6E0A File Offset: 0x002C500A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<INToggleSideScreenControl>() != null;
	}

	// Token: 0x0600746A RID: 29802 RVA: 0x002C6E18 File Offset: 0x002C5018
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<INToggleSideScreenControl>();
		if (this.target == null)
		{
			return;
		}
		this.titleKey = this.target.SidescreenTitleKey;
		base.gameObject.SetActive(true);
		this.Refresh();
	}

	// Token: 0x0600746B RID: 29803 RVA: 0x002C6E64 File Offset: 0x002C5064
	private void Refresh()
	{
		for (int i = 0; i < Mathf.Max(this.target.Options.Count, this.buttonList.Count); i++)
		{
			if (i >= this.target.Options.Count)
			{
				this.buttonList[i].gameObject.SetActive(false);
			}
			else
			{
				if (i >= this.buttonList.Count)
				{
					KToggle ktoggle = Util.KInstantiateUI<KToggle>(this.buttonPrefab.gameObject, this.ContentContainer, false);
					int idx = i;
					ktoggle.onClick += delegate()
					{
						this.target.QueueSelectedOption(idx);
						this.Refresh();
					};
					this.buttonList.Add(ktoggle);
				}
				this.buttonList[i].GetComponentInChildren<LocText>().text = this.target.Options[i];
				this.buttonList[i].GetComponentInChildren<ToolTip>().toolTip = this.target.Tooltips[i];
				if (this.target.SelectedOption == i && this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				else if (this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = true;
				}
				else
				{
					this.buttonList[i].isOn = false;
					foreach (ImageToggleState imageToggleState in this.buttonList[i].GetComponentsInChildren<ImageToggleState>())
					{
						imageToggleState.SetInactive();
						imageToggleState.SetInactive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				this.buttonList[i].gameObject.SetActive(true);
			}
		}
		this.description.text = this.target.Description;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(this.target.Description));
	}

	// Token: 0x0400507B RID: 20603
	[SerializeField]
	private KToggle buttonPrefab;

	// Token: 0x0400507C RID: 20604
	[SerializeField]
	private LocText description;

	// Token: 0x0400507D RID: 20605
	private INToggleSideScreenControl target;

	// Token: 0x0400507E RID: 20606
	private List<KToggle> buttonList = new List<KToggle>();
}
