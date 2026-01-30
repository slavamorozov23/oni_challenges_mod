using System;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0C RID: 3340
public class FeedbackScreen : KModalScreen
{
	// Token: 0x06006764 RID: 26468 RVA: 0x0026FDCC File Offset: 0x0026DFCC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.bugForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		};
		this.suggestionForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/");
		};
		this.logsDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(Util.LogsFolder());
		};
		this.saveFilesDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		};
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.logsDirectoryButton.GetComponentInParent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
			this.saveFilesDirectoryButton.gameObject.SetActive(false);
			this.logsDirectoryButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x040046D3 RID: 18131
	public LocText title;

	// Token: 0x040046D4 RID: 18132
	public KButton dismissButton;

	// Token: 0x040046D5 RID: 18133
	public KButton closeButton;

	// Token: 0x040046D6 RID: 18134
	public KButton bugForumsButton;

	// Token: 0x040046D7 RID: 18135
	public KButton suggestionForumsButton;

	// Token: 0x040046D8 RID: 18136
	public KButton logsDirectoryButton;

	// Token: 0x040046D9 RID: 18137
	public KButton saveFilesDirectoryButton;
}
