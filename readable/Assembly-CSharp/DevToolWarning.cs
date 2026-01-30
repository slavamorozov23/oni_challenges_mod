using System;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x020006B9 RID: 1721
public class DevToolWarning
{
	// Token: 0x06002A5D RID: 10845 RVA: 0x000F7F73 File Offset: 0x000F6173
	public DevToolWarning()
	{
		this.Name = UI.FRONTEND.DEVTOOLS.TITLE;
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x000F7F8B File Offset: 0x000F618B
	public void DrawMenuBar()
	{
		if (ImGui.BeginMainMenuBar())
		{
			ImGui.Checkbox(this.Name, ref this.ShouldDrawWindow);
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x000F7FAC File Offset: 0x000F61AC
	public void DrawWindow(out bool isOpen)
	{
		ImGuiWindowFlags flags = ImGuiWindowFlags.None;
		isOpen = true;
		if (ImGui.Begin(this.Name + "###ID_DevToolWarning", ref isOpen, flags))
		{
			if (!isOpen)
			{
				ImGui.End();
				return;
			}
			ImGui.SetWindowSize(new Vector2(500f, 250f));
			ImGui.TextWrapped(UI.FRONTEND.DEVTOOLS.WARNING);
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Checkbox(UI.FRONTEND.DEVTOOLS.DONTSHOW, ref this.showAgain);
			if (ImGui.Button(UI.FRONTEND.DEVTOOLS.BUTTON))
			{
				if (this.showAgain)
				{
					KPlayerPrefs.SetInt("ShowDevtools", 1);
				}
				DevToolManager.Instance.UserAcceptedWarning = true;
				isOpen = false;
			}
			ImGui.End();
		}
	}

	// Token: 0x04001922 RID: 6434
	private bool showAgain;

	// Token: 0x04001923 RID: 6435
	public string Name;

	// Token: 0x04001924 RID: 6436
	public bool ShouldDrawWindow;
}
