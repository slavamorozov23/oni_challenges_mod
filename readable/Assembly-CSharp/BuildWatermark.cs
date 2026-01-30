using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class BuildWatermark : KScreen
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002C28 File Offset: 0x00000E28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildWatermark.Instance = this;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002C36 File Offset: 0x00000E36
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshText();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002C44 File Offset: 0x00000E44
	public static string GetBuildText()
	{
		string text = DistributionPlatform.Initialized ? (LaunchInitializer.BuildPrefix() + "-") : "??-";
		if (Application.isEditor)
		{
			text += "<EDITOR>";
		}
		else
		{
			text += 706793U.ToString();
		}
		if (DistributionPlatform.Initialized)
		{
			text = text + "-" + DlcManager.GetSubscribedContentLetters();
		}
		else
		{
			text += "-?";
		}
		if (DebugHandler.enabled)
		{
			text += "D";
		}
		if (!"".IsNullOrWhiteSpace())
		{
			text += "-<Patch:>";
		}
		return text;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002CEC File Offset: 0x00000EEC
	public void RefreshText()
	{
		bool flag = true;
		bool flag2 = DistributionPlatform.Initialized && DistributionPlatform.Inst.IsArchiveBranch;
		string buildText = BuildWatermark.GetBuildText();
		this.button.ClearOnClick();
		if (flag)
		{
			this.textDisplay.SetText(string.Format(UI.DEVELOPMENTBUILDS.WATERMARK, buildText));
			this.toolTip.ClearMultiStringTooltip();
		}
		else
		{
			this.textDisplay.SetText(string.Format(UI.DEVELOPMENTBUILDS.TESTING_WATERMARK, buildText));
			this.toolTip.SetSimpleTooltip(UI.DEVELOPMENTBUILDS.TESTING_TOOLTIP);
			if (this.interactable)
			{
				this.button.onClick += this.ShowTestingMessage;
			}
		}
		foreach (GameObject gameObject in this.archiveIcons)
		{
			gameObject.SetActive(flag && flag2);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002DE4 File Offset: 0x00000FE4
	private void ShowTestingMessage()
	{
		Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, Global.Instance.globalCanvas, true).PopupConfirmDialog(UI.DEVELOPMENTBUILDS.TESTING_MESSAGE, delegate
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}, delegate
		{
		}, null, null, UI.DEVELOPMENTBUILDS.TESTING_MESSAGE_TITLE, UI.DEVELOPMENTBUILDS.TESTING_MORE_INFO, null, null);
	}

	// Token: 0x04000035 RID: 53
	public bool interactable = true;

	// Token: 0x04000036 RID: 54
	public LocText textDisplay;

	// Token: 0x04000037 RID: 55
	public ToolTip toolTip;

	// Token: 0x04000038 RID: 56
	public KButton button;

	// Token: 0x04000039 RID: 57
	public List<GameObject> archiveIcons;

	// Token: 0x0400003A RID: 58
	public static BuildWatermark Instance;
}
