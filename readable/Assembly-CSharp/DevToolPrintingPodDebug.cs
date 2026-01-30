using System;
using ImGuiNET;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
public class DevToolPrintingPodDebug : DevTool
{
	// Token: 0x060029F5 RID: 10741 RVA: 0x000F3151 File Offset: 0x000F1351
	protected override void RenderTo(DevPanel panel)
	{
		if (Immigration.Instance != null)
		{
			this.ShowButtons();
			return;
		}
		ImGui.Text("Game not available");
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000F3174 File Offset: 0x000F1374
	private void ShowButtons()
	{
		if (Components.Telepads.Count == 0)
		{
			ImGui.Text("No printing pods available");
			return;
		}
		ImGui.Text("Time until next print available: " + Mathf.CeilToInt(Immigration.Instance.timeBeforeSpawn).ToString() + "s");
		if (ImGui.Button("Activate now"))
		{
			Immigration.Instance.timeBeforeSpawn = 0f;
		}
		if (ImGui.Button("Shuffle Options"))
		{
			if (ImmigrantScreen.instance.Telepad == null)
			{
				ImmigrantScreen.InitializeImmigrantScreen(Components.Telepads[0]);
				return;
			}
			ImmigrantScreen.instance.DebugShuffleOptions();
		}
	}
}
