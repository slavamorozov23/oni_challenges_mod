using System;
using ImGuiNET;

// Token: 0x020006AB RID: 1707
public class DevToolSaveGameInfo : DevTool
{
	// Token: 0x060029FB RID: 10747 RVA: 0x000F3434 File Offset: 0x000F1634
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("No game loaded");
			return;
		}
		ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
		ImGui.Text("Generated: " + Game.Instance.dateGenerated);
		ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed.ToString());
		ImGui.Text("Content Enabled: ");
		foreach (string text in SaveLoader.Instance.GameInfo.dlcIds)
		{
			string str = (text == "") ? "VANILLA_ID" : text;
			ImGui.Text(" - " + str);
		}
		ImGui.PushItemWidth(100f);
		ImGui.NewLine();
		ImGui.Text("Changelists played on");
		ImGui.InputText("Search", ref this.clSearch, 10U);
		ImGui.PopItemWidth();
		foreach (uint num in Game.Instance.changelistsPlayedOn)
		{
			if (this.clSearch.IsNullOrWhiteSpace() || num.ToString().Contains(this.clSearch))
			{
				ImGui.Text(num.ToString());
			}
		}
		ImGui.NewLine();
	}

	// Token: 0x040018E2 RID: 6370
	private string clSearch = "";
}
