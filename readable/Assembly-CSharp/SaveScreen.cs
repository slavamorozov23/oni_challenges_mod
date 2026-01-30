using System;
using System.IO;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000C75 RID: 3189
public class SaveScreen : KModalScreen
{
	// Token: 0x0600613B RID: 24891 RVA: 0x0023BBF4 File Offset: 0x00239DF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.oldSaveButtonPrefab.gameObject.SetActive(false);
		this.newSaveButton.onClick += this.OnClickNewSave;
		this.closeButton.onClick += this.Deactivate;
	}

	// Token: 0x0600613C RID: 24892 RVA: 0x0023BC48 File Offset: 0x00239E48
	protected override void OnCmpEnable()
	{
		foreach (SaveLoader.SaveFileEntry saveFileEntry in SaveLoader.GetAllColonyFiles(true, SearchOption.TopDirectoryOnly))
		{
			this.AddExistingSaveFile(saveFileEntry.path);
		}
		SpeedControlScreen.Instance.Pause(true, false);
	}

	// Token: 0x0600613D RID: 24893 RVA: 0x0023BCB0 File Offset: 0x00239EB0
	protected override void OnDeactivate()
	{
		SpeedControlScreen.Instance.Unpause(true);
		base.OnDeactivate();
	}

	// Token: 0x0600613E RID: 24894 RVA: 0x0023BCC4 File Offset: 0x00239EC4
	private void AddExistingSaveFile(string filename)
	{
		KButton kbutton = Util.KInstantiateUI<KButton>(this.oldSaveButtonPrefab.gameObject, this.oldSavesRoot.gameObject, true);
		HierarchyReferences component = kbutton.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("Title").GetComponent<LocText>();
		TMP_Text component3 = component.GetReference<RectTransform>("Date").GetComponent<LocText>();
		System.DateTime lastWriteTime = File.GetLastWriteTime(filename);
		component2.text = string.Format("{0}", Path.GetFileNameWithoutExtension(filename));
		component3.text = string.Format("{0:H:mm:ss}" + Localization.GetFileDateFormat(0), lastWriteTime);
		kbutton.onClick += delegate()
		{
			this.Save(filename);
		};
	}

	// Token: 0x0600613F RID: 24895 RVA: 0x0023BD80 File Offset: 0x00239F80
	public static string GetValidSaveFilename(string filename)
	{
		string text = ".sav";
		if (Path.GetExtension(filename).ToLower() != text)
		{
			filename += text;
		}
		return filename;
	}

	// Token: 0x06006140 RID: 24896 RVA: 0x0023BDB0 File Offset: 0x00239FB0
	public void Save(string filename)
	{
		filename = SaveScreen.GetValidSaveFilename(filename);
		if (File.Exists(filename))
		{
			ScreenPrefabs.Instance.ConfirmDoAction(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
			}, base.transform.parent);
			return;
		}
		this.DoSave(filename);
	}

	// Token: 0x06006141 RID: 24897 RVA: 0x0023BE38 File Offset: 0x0023A038
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			PauseScreen.Instance.OnSaveComplete();
			this.Deactivate();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.FILEIO
				}, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x06006142 RID: 24898 RVA: 0x0023BEF8 File Offset: 0x0023A0F8
	public void OnClickNewSave()
	{
		FileNameDialog fileNameDialog = (FileNameDialog)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.FileNameDialog.gameObject, base.transform.parent.gameObject);
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath != null)
		{
			string text = SaveLoader.GetOriginalSaveFileName(activeSaveFilePath);
			text = Path.GetFileNameWithoutExtension(text);
			fileNameDialog.SetTextAndSelect(text);
		}
		fileNameDialog.onConfirm = delegate(string filename)
		{
			filename = Path.Combine(SaveLoader.GetActiveSaveColonyFolder(), filename);
			this.Save(filename);
		};
	}

	// Token: 0x06006143 RID: 24899 RVA: 0x0023BF64 File Offset: 0x0023A164
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		e.Consumed = true;
	}

	// Token: 0x06006144 RID: 24900 RVA: 0x0023BF7C File Offset: 0x0023A17C
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x0400410A RID: 16650
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400410B RID: 16651
	[SerializeField]
	private KButton newSaveButton;

	// Token: 0x0400410C RID: 16652
	[SerializeField]
	private KButton oldSaveButtonPrefab;

	// Token: 0x0400410D RID: 16653
	[SerializeField]
	private Transform oldSavesRoot;
}
