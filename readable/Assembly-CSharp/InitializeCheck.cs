using System;
using System.IO;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x0200099A RID: 2458
public class InitializeCheck : MonoBehaviour
{
	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x060046AC RID: 18092 RVA: 0x00198FA0 File Offset: 0x001971A0
	// (set) Token: 0x060046AD RID: 18093 RVA: 0x00198FA7 File Offset: 0x001971A7
	public static InitializeCheck.SavePathIssue savePathState { get; private set; }

	// Token: 0x060046AE RID: 18094 RVA: 0x00198FB0 File Offset: 0x001971B0
	private void Awake()
	{
		this.CheckForSavePathIssue();
		if (InitializeCheck.savePathState == InitializeCheck.SavePathIssue.Ok && !KCrashReporter.hasCrash)
		{
			AudioMixer.Create();
			App.LoadScene("frontend");
			return;
		}
		Canvas cmp = base.gameObject.AddComponent<Canvas>();
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);
		Camera camera = base.gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = 200f;
		camera.backgroundColor = Color.black;
		camera.clearFlags = CameraClearFlags.Color;
		camera.nearClipPlane = 0f;
		global::Debug.Log("Cannot initialize filesystem. [" + InitializeCheck.savePathState.ToString() + "]");
		Localization.Initialize();
		GameObject.Find("BootCanvas").SetActive(false);
		this.ShowFileErrorDialogs();
	}

	// Token: 0x060046AF RID: 18095 RVA: 0x00199089 File Offset: 0x00197289
	private GameObject CreateUIRoot()
	{
		return Util.KInstantiate(this.rootCanvasPrefab, null, "CanvasRoot");
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x0019909C File Offset: 0x0019729C
	private void ShowErrorDialog(string msg)
	{
		GameObject parent = this.CreateUIRoot();
		Util.KInstantiateUI<ConfirmDialogScreen>(this.confirmDialogScreen.gameObject, parent, true).PopupConfirmDialog(msg, new System.Action(this.Quit), null, null, null, null, null, null, this.sadDupe);
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x001990E0 File Offset: 0x001972E0
	private void ShowFileErrorDialogs()
	{
		string text = null;
		switch (InitializeCheck.savePathState)
		{
		case InitializeCheck.SavePathIssue.WriteTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.SpaceTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.WorldGenFilesFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FILES, WorldGen.WORLDGEN_SAVE_FILENAME);
			break;
		}
		if (text != null)
		{
			this.ShowErrorDialog(text);
		}
	}

	// Token: 0x060046B2 RID: 18098 RVA: 0x00199158 File Offset: 0x00197358
	private void CheckForSavePathIssue()
	{
		if (this.test_issue != InitializeCheck.SavePathIssue.Ok)
		{
			InitializeCheck.savePathState = this.test_issue;
			return;
		}
		string savePrefix = SaveLoader.GetSavePrefix();
		InitializeCheck.savePathState = InitializeCheck.SavePathIssue.Ok;
		try
		{
			SaveLoader.GetSavePrefixAndCreateFolder();
			using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testFile, FileMode.Create, FileAccess.Write))
			{
				new BinaryWriter(fileStream);
				fileStream.Close();
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WriteTestFail;
			goto IL_C8;
		}
		using (FileStream fileStream2 = File.Open(savePrefix + InitializeCheck.testSave, FileMode.Create, FileAccess.Write))
		{
			try
			{
				fileStream2.SetLength(15000000L);
				new BinaryWriter(fileStream2);
				fileStream2.Close();
			}
			catch
			{
				fileStream2.Close();
				InitializeCheck.savePathState = InitializeCheck.SavePathIssue.SpaceTestFail;
				goto IL_C8;
			}
		}
		try
		{
			using (File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Append))
			{
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WorldGenFilesFail;
		}
		IL_C8:
		try
		{
			if (File.Exists(savePrefix + InitializeCheck.testFile))
			{
				File.Delete(savePrefix + InitializeCheck.testFile);
			}
			if (File.Exists(savePrefix + InitializeCheck.testSave))
			{
				File.Delete(savePrefix + InitializeCheck.testSave);
			}
		}
		catch
		{
		}
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x001992D0 File Offset: 0x001974D0
	private void Quit()
	{
		global::Debug.Log("Quitting...");
		App.QuitCode(1);
	}

	// Token: 0x04002F92 RID: 12178
	private static readonly string testFile = "testfile";

	// Token: 0x04002F93 RID: 12179
	private static readonly string testSave = "testsavefile";

	// Token: 0x04002F94 RID: 12180
	public Canvas rootCanvasPrefab;

	// Token: 0x04002F95 RID: 12181
	public ConfirmDialogScreen confirmDialogScreen;

	// Token: 0x04002F96 RID: 12182
	public Sprite sadDupe;

	// Token: 0x04002F97 RID: 12183
	private InitializeCheck.SavePathIssue test_issue;

	// Token: 0x02001A02 RID: 6658
	public enum SavePathIssue
	{
		// Token: 0x04008036 RID: 32822
		Ok,
		// Token: 0x04008037 RID: 32823
		WriteTestFail,
		// Token: 0x04008038 RID: 32824
		SpaceTestFail,
		// Token: 0x04008039 RID: 32825
		WorldGenFilesFail
	}
}
