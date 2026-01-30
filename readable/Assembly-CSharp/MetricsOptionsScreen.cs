using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DAB RID: 3499
public class MetricsOptionsScreen : KModalScreen
{
	// Token: 0x06006CE4 RID: 27876 RVA: 0x00292E88 File Offset: 0x00291088
	private bool IsSettingsDirty()
	{
		return this.disableDataCollection != KPrivacyPrefs.instance.disableDataCollection;
	}

	// Token: 0x06006CE5 RID: 27877 RVA: 0x00292E9F File Offset: 0x0029109F
	public override void OnKeyDown(KButtonEvent e)
	{
		if ((e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)) && !this.IsSettingsDirty())
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006CE6 RID: 27878 RVA: 0x00292ECC File Offset: 0x002910CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.disableDataCollection = KPrivacyPrefs.instance.disableDataCollection;
		this.title.SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
		GameObject gameObject = this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.OnClickToggle();
		};
		this.enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
		this.dismissButton.onClick += delegate()
		{
			if (this.IsSettingsDirty())
			{
				this.ApplySettingsAndDoRestart();
				return;
			}
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.descriptionButton.onClick.AddListener(delegate()
		{
			App.OpenWebURL("https://www.kleientertainment.com/privacy-policy");
		});
		this.openKleiAccountButton.onClick += this.OpenKleiAccount;
		this.Refresh();
	}

	// Token: 0x06006CE7 RID: 27879 RVA: 0x00292FE7 File Offset: 0x002911E7
	private void OnClickToggle()
	{
		this.disableDataCollection = !this.disableDataCollection;
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(this.disableDataCollection);
		this.Refresh();
	}

	// Token: 0x06006CE8 RID: 27880 RVA: 0x00293024 File Offset: 0x00291224
	private void ApplySettingsAndDoRestart()
	{
		KPrivacyPrefs.instance.disableDataCollection = this.disableDataCollection;
		KPrivacyPrefs.Save();
		KPlayerPrefs.SetString("DisableDataCollection", KPrivacyPrefs.instance.disableDataCollection ? "yes" : "no");
		KPlayerPrefs.Save();
		ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
		App.instance.Restart();
	}

	// Token: 0x06006CE9 RID: 27881 RVA: 0x002930B8 File Offset: 0x002912B8
	private void Refresh()
	{
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").transform.GetChild(0).gameObject.SetActive(!this.disableDataCollection);
		this.closeButton.isInteractable = !this.IsSettingsDirty();
		this.restartWarningText.gameObject.SetActive(this.IsSettingsDirty());
		if (this.IsSettingsDirty())
		{
			this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.RESTART_BUTTON;
			return;
		}
		this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.DONE_BUTTON;
	}

	// Token: 0x06006CEA RID: 27882 RVA: 0x0029315F File Offset: 0x0029135F
	private void OpenKleiAccount()
	{
		App.OpenWebURL("https://accounts.klei.com/login/auto?Game=ONI&ClientToken=" + KleiAccount.KleiToken);
	}

	// Token: 0x04004A73 RID: 19059
	public LocText title;

	// Token: 0x04004A74 RID: 19060
	public KButton dismissButton;

	// Token: 0x04004A75 RID: 19061
	public KButton closeButton;

	// Token: 0x04004A76 RID: 19062
	public GameObject enableButton;

	// Token: 0x04004A77 RID: 19063
	public Button descriptionButton;

	// Token: 0x04004A78 RID: 19064
	public LocText restartWarningText;

	// Token: 0x04004A79 RID: 19065
	private bool disableDataCollection;

	// Token: 0x04004A7A RID: 19066
	public KButton openKleiAccountButton;
}
