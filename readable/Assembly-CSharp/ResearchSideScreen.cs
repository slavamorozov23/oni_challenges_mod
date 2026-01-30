using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E6C RID: 3692
public class ResearchSideScreen : SideScreenContent
{
	// Token: 0x0600754B RID: 30027 RVA: 0x002CC415 File Offset: 0x002CA615
	public ResearchSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x0600754C RID: 30028 RVA: 0x002CC430 File Offset: 0x002CA630
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectResearchButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleResearch();
		};
		Research.Instance.Subscribe(-1914338957, this.refreshDisplayStateDelegate);
		Research.Instance.Subscribe(-125623018, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x0600754D RID: 30029 RVA: 0x002CC4A0 File Offset: 0x002CA6A0
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
		this.target.gameObject.Subscribe(-1852328367, this.refreshDisplayStateDelegate);
		this.target.gameObject.Subscribe(-592767678, this.refreshDisplayStateDelegate);
	}

	// Token: 0x0600754E RID: 30030 RVA: 0x002CC50C File Offset: 0x002CA70C
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
			this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
			this.target = null;
		}
	}

	// Token: 0x0600754F RID: 30031 RVA: 0x002CC56C File Offset: 0x002CA76C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Research.Instance.Unsubscribe(-1914338957, this.refreshDisplayStateDelegate);
		Research.Instance.Unsubscribe(-125623018, this.refreshDisplayStateDelegate);
		if (this.target)
		{
			this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
			this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
			this.target = null;
		}
	}

	// Token: 0x06007550 RID: 30032 RVA: 0x002CC5F3 File Offset: 0x002CA7F3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ResearchCenter>() != null || target.GetComponent<NuclearResearchCenter>() != null;
	}

	// Token: 0x06007551 RID: 30033 RVA: 0x002CC614 File Offset: 0x002CA814
	private void RefreshDisplayState(object _ = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		string text = "";
		ResearchCenter component = SelectTool.Instance.selected.GetComponent<ResearchCenter>();
		NuclearResearchCenter component2 = SelectTool.Instance.selected.GetComponent<NuclearResearchCenter>();
		if (component != null)
		{
			text = component.research_point_type_id;
		}
		if (component2 != null)
		{
			text = component2.researchTypeID;
		}
		if (component == null && component2 == null)
		{
			return;
		}
		this.researchButtonIcon.sprite = Research.Instance.researchTypes.GetResearchType(text).sprite;
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.DescriptionText.text = "<b>" + UI.UISIDESCREENS.RESEARCHSIDESCREEN.NOSELECTEDRESEARCH + "</b>";
			return;
		}
		string text2 = "";
		if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(text) || activeResearch.tech.costsByResearchTypeID[text] <= 0f)
		{
			text2 += "<color=#7f7f7f>";
		}
		text2 = text2 + "<b>" + activeResearch.tech.Name + "</b>";
		if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(text) || activeResearch.tech.costsByResearchTypeID[text] <= 0f)
		{
			text2 += "</color>";
		}
		foreach (KeyValuePair<string, float> keyValuePair in activeResearch.tech.costsByResearchTypeID)
		{
			if (keyValuePair.Value != 0f)
			{
				bool flag = keyValuePair.Key == text;
				text2 += "\n   ";
				text2 += "<b>";
				if (!flag)
				{
					text2 += "<color=#7f7f7f>";
				}
				text2 = string.Concat(new string[]
				{
					text2,
					"- ",
					Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name,
					": ",
					activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key].ToString(),
					"/",
					activeResearch.tech.costsByResearchTypeID[keyValuePair.Key].ToString()
				});
				if (!flag)
				{
					text2 += "</color>";
				}
				text2 += "</b>";
			}
		}
		this.DescriptionText.text = text2;
	}

	// Token: 0x04005127 RID: 20775
	public KButton selectResearchButton;

	// Token: 0x04005128 RID: 20776
	public Image researchButtonIcon;

	// Token: 0x04005129 RID: 20777
	public GameObject content;

	// Token: 0x0400512A RID: 20778
	private GameObject target;

	// Token: 0x0400512B RID: 20779
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x0400512C RID: 20780
	public LocText DescriptionText;
}
