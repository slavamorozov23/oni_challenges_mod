using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000956 RID: 2390
[AddComponentMenu("KMonoBehaviour/scripts/FileErrorReporter")]
public class FileErrorReporter : KMonoBehaviour
{
	// Token: 0x060042D0 RID: 17104 RVA: 0x00179A3B File Offset: 0x00177C3B
	protected override void OnSpawn()
	{
		this.OnFileError();
		FileUtil.onErrorMessage += this.OnFileError;
	}

	// Token: 0x060042D1 RID: 17105 RVA: 0x00179A54 File Offset: 0x00177C54
	private void OnFileError()
	{
		if (FileUtil.errorType == FileUtil.ErrorType.None)
		{
			return;
		}
		string text;
		switch (FileUtil.errorType)
		{
		case FileUtil.ErrorType.UnauthorizedAccess:
			text = string.Format(FileUtil.errorSubject.Contains("OneDrive") ? UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED_ONEDRIVE : UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, FileUtil.errorSubject);
			goto IL_7D;
		case FileUtil.ErrorType.IOError:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, FileUtil.errorSubject);
			goto IL_7D;
		}
		text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.IO_UNKNOWN, FileUtil.errorSubject);
		IL_7D:
		GameObject gameObject;
		if (FrontEndManager.Instance != null)
		{
			gameObject = FrontEndManager.Instance.gameObject;
		}
		else if (GameScreenManager.Instance != null && GameScreenManager.Instance.ssOverlayCanvas != null)
		{
			gameObject = GameScreenManager.Instance.ssOverlayCanvas;
		}
		else
		{
			gameObject = new GameObject();
			gameObject.name = "FileErrorCanvas";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 10;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		if ((FileUtil.exceptionMessage != null || FileUtil.exceptionStackTrace != null) && !KCrashReporter.hasReportedError)
		{
			KCrashReporter.ReportError(FileUtil.exceptionMessage, FileUtil.exceptionStackTrace, null, null, null, true, new string[]
			{
				KCrashReporter.CRASH_CATEGORY.FILEIO
			}, null);
		}
		ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, gameObject, true).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
		UnityEngine.Object.DontDestroyOnLoad(component.gameObject);
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x00179BC8 File Offset: 0x00177DC8
	private void OpenMoreInfo()
	{
	}
}
