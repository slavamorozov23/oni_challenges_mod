using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000DEA RID: 3562
public class ReportScreen : KScreen
{
	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x0600701E RID: 28702 RVA: 0x002A947C File Offset: 0x002A767C
	// (set) Token: 0x0600701F RID: 28703 RVA: 0x002A9483 File Offset: 0x002A7683
	public static ReportScreen Instance { get; private set; }

	// Token: 0x06007020 RID: 28704 RVA: 0x002A948B File Offset: 0x002A768B
	public static void DestroyInstance()
	{
		ReportScreen.Instance = null;
	}

	// Token: 0x06007021 RID: 28705 RVA: 0x002A9494 File Offset: 0x002A7694
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ReportScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.prevButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day - 1);
		};
		this.nextButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day + 1);
		};
		this.summaryButton.onClick += delegate()
		{
			RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
		};
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06007022 RID: 28706 RVA: 0x002A9536 File Offset: 0x002A7736
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06007023 RID: 28707 RVA: 0x002A953E File Offset: 0x002A773E
	protected override void OnShow(bool bShow)
	{
		base.OnShow(bShow);
		if (ReportManager.Instance != null)
		{
			this.currentReport = ReportManager.Instance.TodaysReport;
		}
	}

	// Token: 0x06007024 RID: 28708 RVA: 0x002A9564 File Offset: 0x002A7764
	public void SetTitle(string title)
	{
		this.title.text = title;
	}

	// Token: 0x06007025 RID: 28709 RVA: 0x002A9572 File Offset: 0x002A7772
	public override void ScreenUpdate(bool b)
	{
		base.ScreenUpdate(b);
		this.Refresh();
	}

	// Token: 0x06007026 RID: 28710 RVA: 0x002A9584 File Offset: 0x002A7784
	private void Refresh()
	{
		global::Debug.Assert(this.currentReport != null);
		if (this.currentReport.day == ReportManager.Instance.TodaysReport.day)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_TODAY, this.currentReport.day));
		}
		else if (this.currentReport.day == ReportManager.Instance.TodaysReport.day - 1)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_YESTERDAY, this.currentReport.day));
		}
		else
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day));
		}
		bool flag = this.currentReport.day < ReportManager.Instance.TodaysReport.day;
		this.nextButton.isInteractable = flag;
		if (flag)
		{
			this.nextButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day + 1);
			this.nextButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.nextButton.GetComponent<ToolTip>().enabled = false;
		}
		flag = (this.currentReport.day > 1);
		this.prevButton.isInteractable = flag;
		if (flag)
		{
			this.prevButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day - 1);
			this.prevButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.prevButton.GetComponent<ToolTip>().enabled = false;
		}
		this.AddSpacer(0);
		int num = 1;
		foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in ReportManager.Instance.ReportGroups)
		{
			ReportManager.ReportEntry entry = this.currentReport.GetEntry(keyValuePair.Key);
			if (num != keyValuePair.Value.group)
			{
				num = keyValuePair.Value.group;
				this.AddSpacer(num);
			}
			bool flag2 = entry.accumulate != 0f || keyValuePair.Value.reportIfZero;
			if (keyValuePair.Value.isHeader)
			{
				this.CreateHeader(keyValuePair.Value);
			}
			else if (flag2)
			{
				this.CreateOrUpdateLine(entry, keyValuePair.Value, flag2);
			}
		}
	}

	// Token: 0x06007027 RID: 28711 RVA: 0x002A9820 File Offset: 0x002A7A20
	public void ShowReport(int day)
	{
		this.currentReport = ReportManager.Instance.FindReport(day);
		global::Debug.Assert(this.currentReport != null, "Can't find report for day: " + day.ToString());
		this.Refresh();
	}

	// Token: 0x06007028 RID: 28712 RVA: 0x002A9858 File Offset: 0x002A7A58
	private GameObject AddSpacer(int group)
	{
		GameObject gameObject;
		if (this.lineItems.ContainsKey(group.ToString()))
		{
			gameObject = this.lineItems[group.ToString()];
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.lineItemSpacer, this.contentFolder, false);
			gameObject.name = "Spacer" + group.ToString();
			this.lineItems[group.ToString()] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06007029 RID: 28713 RVA: 0x002A98D8 File Offset: 0x002A7AD8
	private GameObject CreateHeader(ReportManager.ReportGroup reportGroup)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (gameObject == null)
		{
			gameObject = Util.KInstantiateUI(this.lineItemHeader, this.contentFolder, true);
			gameObject.name = "LineItemHeader" + this.lineItems.Count.ToString();
			this.lineItems[reportGroup.stringKey] = gameObject;
		}
		gameObject.SetActive(true);
		gameObject.GetComponent<ReportScreenHeader>().SetMainEntry(reportGroup);
		return gameObject;
	}

	// Token: 0x0600702A RID: 28714 RVA: 0x002A9960 File Offset: 0x002A7B60
	private GameObject CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (!is_line_active)
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if (gameObject == null)
			{
				gameObject = Util.KInstantiateUI(this.lineItem, this.contentFolder, true);
				gameObject.name = "LineItem" + this.lineItems.Count.ToString();
				this.lineItems[reportGroup.stringKey] = gameObject;
			}
			gameObject.SetActive(true);
			gameObject.GetComponent<ReportScreenEntry>().SetMainEntry(entry, reportGroup);
		}
		return gameObject;
	}

	// Token: 0x0600702B RID: 28715 RVA: 0x002A9A06 File Offset: 0x002A7C06
	private void OnClickClose()
	{
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Close", false));
		this.Show(false);
	}

	// Token: 0x04004CEE RID: 19694
	[SerializeField]
	private LocText title;

	// Token: 0x04004CEF RID: 19695
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004CF0 RID: 19696
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04004CF1 RID: 19697
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04004CF2 RID: 19698
	[SerializeField]
	private KButton summaryButton;

	// Token: 0x04004CF3 RID: 19699
	[SerializeField]
	private GameObject lineItem;

	// Token: 0x04004CF4 RID: 19700
	[SerializeField]
	private GameObject lineItemSpacer;

	// Token: 0x04004CF5 RID: 19701
	[SerializeField]
	private GameObject lineItemHeader;

	// Token: 0x04004CF6 RID: 19702
	[SerializeField]
	private GameObject contentFolder;

	// Token: 0x04004CF7 RID: 19703
	private Dictionary<string, GameObject> lineItems = new Dictionary<string, GameObject>();

	// Token: 0x04004CF8 RID: 19704
	private ReportManager.DailyReport currentReport;
}
