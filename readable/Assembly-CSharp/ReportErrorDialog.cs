using System;
using System.Collections.Generic;
using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE9 RID: 3561
public class ReportErrorDialog : MonoBehaviour
{
	// Token: 0x0600700B RID: 28683 RVA: 0x002A8E6C File Offset: 0x002A706C
	private void Start()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(true);
		}
		this.StackTrace.SetActive(false);
		this.CrashLabel.text = ((this.mode == ReportErrorDialog.Mode.SubmitError) ? UI.CRASHSCREEN.TITLE : UI.CRASHSCREEN.TITLE_MODS);
		this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
		this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
		if (this.mode == ReportErrorDialog.Mode.DisableMods)
		{
			this.BuildModsList();
		}
		this.submitButton.gameObject.SetActive(this.submitAction != null);
		this.submitButton.onClick += this.OnSelect_SUBMIT;
		this.moreInfoButton.onClick += this.OnSelect_MOREINFO;
		this.continueGameButton.gameObject.SetActive(this.continueAction != null);
		this.continueGameButton.onClick += this.OnSelect_CONTINUE;
		this.quitButton.onClick += this.OnSelect_QUIT;
		this.messageInputField.text = UI.CRASHSCREEN.BODY;
		KCrashReporter.onCrashReported += this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress += this.UpdateProgressBar;
	}

	// Token: 0x0600700C RID: 28684 RVA: 0x002A8FC8 File Offset: 0x002A71C8
	private void BuildModsList()
	{
		DebugUtil.Assert(Global.Instance != null && Global.Instance.modManager != null);
		Manager mod_mgr = Global.Instance.modManager;
		List<Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
		allCrashableMods.Sort((Mod x, Mod y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace));
		foreach (Mod mod in allCrashableMods)
		{
			if (mod.foundInStackTrace && mod.label.distribution_platform != Label.DistributionPlatform.Dev)
			{
				mod_mgr.EnableMod(mod.label, false, this);
			}
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject, false);
			LocText reference = hierarchyReferences.GetReference<LocText>("Title");
			reference.text = mod.title;
			reference.color = (mod.foundInStackTrace ? Color.red : Color.white);
			MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
			toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
			Label mod_label = mod.label;
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				bool flag = !mod_mgr.IsModEnabled(mod_label);
				toggle.ChangeState(flag ? 1 : 0);
				mod_mgr.EnableMod(mod_label, flag, this);
			}));
			toggle.GetComponent<ToolTip>().OnToolTip = (() => mod_mgr.IsModEnabled(mod_label) ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
			hierarchyReferences.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600700D RID: 28685 RVA: 0x002A919C File Offset: 0x002A739C
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x0600700E RID: 28686 RVA: 0x002A91A4 File Offset: 0x002A73A4
	private void OnDestroy()
	{
		if (KCrashReporter.terminateOnError)
		{
			App.QuitCode(1);
		}
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(false);
		}
		KCrashReporter.onCrashReported -= this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress -= this.UpdateProgressBar;
	}

	// Token: 0x0600700F RID: 28687 RVA: 0x002A91F7 File Offset: 0x002A73F7
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_QUIT();
		}
	}

	// Token: 0x06007010 RID: 28688 RVA: 0x002A9208 File Offset: 0x002A7408
	public void PopupSubmitErrorDialog(string stackTrace, System.Action onSubmit, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.SubmitError;
		this.m_stackTrace = stackTrace;
		this.submitAction = onSubmit;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x06007011 RID: 28689 RVA: 0x002A922E File Offset: 0x002A742E
	public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.DisableMods;
		this.m_stackTrace = stackTrace;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x06007012 RID: 28690 RVA: 0x002A924C File Offset: 0x002A744C
	public void OnSelect_MOREINFO()
	{
		this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
		this.StackTrace.SetActive(true);
		this.moreInfoButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
		this.moreInfoButton.ClearOnClick();
		this.moreInfoButton.onClick += this.OnSelect_COPYTOCLIPBOARD;
	}

	// Token: 0x06007013 RID: 28691 RVA: 0x002A92B7 File Offset: 0x002A74B7
	public void OnSelect_COPYTOCLIPBOARD()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.m_stackTrace + "\nBuild: " + BuildWatermark.GetBuildText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x06007014 RID: 28692 RVA: 0x002A92E4 File Offset: 0x002A74E4
	public void OnSelect_SUBMIT()
	{
		this.submitButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.REPORTING;
		this.submitButton.GetComponent<KButton>().isInteractable = false;
		this.Submit();
	}

	// Token: 0x06007015 RID: 28693 RVA: 0x002A9317 File Offset: 0x002A7517
	public void OnSelect_QUIT()
	{
		if (this.quitAction != null)
		{
			this.quitAction();
		}
	}

	// Token: 0x06007016 RID: 28694 RVA: 0x002A932C File Offset: 0x002A752C
	public void OnSelect_CONTINUE()
	{
		if (this.continueAction != null)
		{
			this.continueAction();
		}
	}

	// Token: 0x06007017 RID: 28695 RVA: 0x002A9344 File Offset: 0x002A7544
	public void OpenRefMessage(bool success)
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(false);
		this.referenceMessage.SetActive(true);
		this.messageText.text = (success ? UI.CRASHSCREEN.THANKYOU : UI.CRASHSCREEN.UPLOAD_FAILED);
		this.m_crashSubmitted = success;
	}

	// Token: 0x06007018 RID: 28696 RVA: 0x002A93A0 File Offset: 0x002A75A0
	public void OpenUploadingMessagee()
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(true);
		this.referenceMessage.SetActive(false);
		this.progressBar.fillAmount = 0f;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0f, GameUtil.TimeSlice.None));
	}

	// Token: 0x06007019 RID: 28697 RVA: 0x002A940B File Offset: 0x002A760B
	public void OnSelect_MESSAGE()
	{
		if (!this.m_crashSubmitted)
		{
			Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}
	}

	// Token: 0x0600701A RID: 28698 RVA: 0x002A941F File Offset: 0x002A761F
	public string UserMessage()
	{
		return this.messageInputField.text;
	}

	// Token: 0x0600701B RID: 28699 RVA: 0x002A942C File Offset: 0x002A762C
	private void Submit()
	{
		this.submitAction();
		this.OpenUploadingMessagee();
	}

	// Token: 0x0600701C RID: 28700 RVA: 0x002A943F File Offset: 0x002A763F
	public void UpdateProgressBar(float progress)
	{
		this.progressBar.fillAmount = progress;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x04004CD8 RID: 19672
	private System.Action submitAction;

	// Token: 0x04004CD9 RID: 19673
	private System.Action quitAction;

	// Token: 0x04004CDA RID: 19674
	private System.Action continueAction;

	// Token: 0x04004CDB RID: 19675
	public KInputTextField messageInputField;

	// Token: 0x04004CDC RID: 19676
	[Header("Message")]
	public GameObject referenceMessage;

	// Token: 0x04004CDD RID: 19677
	public LocText messageText;

	// Token: 0x04004CDE RID: 19678
	[Header("Upload Progress")]
	public GameObject uploadInProgress;

	// Token: 0x04004CDF RID: 19679
	public Image progressBar;

	// Token: 0x04004CE0 RID: 19680
	public LocText progressText;

	// Token: 0x04004CE1 RID: 19681
	private string m_stackTrace;

	// Token: 0x04004CE2 RID: 19682
	private bool m_crashSubmitted;

	// Token: 0x04004CE3 RID: 19683
	[SerializeField]
	private KButton submitButton;

	// Token: 0x04004CE4 RID: 19684
	[SerializeField]
	private KButton moreInfoButton;

	// Token: 0x04004CE5 RID: 19685
	[SerializeField]
	private KButton quitButton;

	// Token: 0x04004CE6 RID: 19686
	[SerializeField]
	private KButton continueGameButton;

	// Token: 0x04004CE7 RID: 19687
	[SerializeField]
	private LocText CrashLabel;

	// Token: 0x04004CE8 RID: 19688
	[SerializeField]
	private GameObject CrashDescription;

	// Token: 0x04004CE9 RID: 19689
	[SerializeField]
	private GameObject ModsInfo;

	// Token: 0x04004CEA RID: 19690
	[SerializeField]
	private GameObject StackTrace;

	// Token: 0x04004CEB RID: 19691
	[SerializeField]
	private GameObject modEntryPrefab;

	// Token: 0x04004CEC RID: 19692
	[SerializeField]
	private Transform modEntryParent;

	// Token: 0x04004CED RID: 19693
	private ReportErrorDialog.Mode mode;

	// Token: 0x02002052 RID: 8274
	private enum Mode
	{
		// Token: 0x040095B4 RID: 38324
		SubmitError,
		// Token: 0x040095B5 RID: 38325
		DisableMods
	}
}
