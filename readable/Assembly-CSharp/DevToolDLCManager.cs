using System;
using ImGuiNET;

// Token: 0x02000696 RID: 1686
public class DevToolDLCManager : DevTool
{
	// Token: 0x06002985 RID: 10629 RVA: 0x000EDE1C File Offset: 0x000EC01C
	protected override void RenderTo(DevPanel panel)
	{
		string name = DistributionPlatform.Inst.Name;
		if (!DistributionPlatform.Initialized)
		{
			ImGui.Text("Failed to initialize " + name);
			return;
		}
		ImGui.Text("Active content letters: " + DlcManager.GetActiveContentLetters());
		ImGui.Separator();
		foreach (string text in DlcManager.RELEASED_VERSIONS)
		{
			if (!text.IsNullOrWhiteSpace())
			{
				ImGui.Text(text);
				ImGui.SameLine();
				bool flag = DlcManager.IsContentSubscribed(text);
				if (ImGui.Checkbox("Enabled ", ref flag))
				{
					DlcManager.ToggleDLC(text);
				}
				ImGui.SameLine();
				bool flag2 = DistributionPlatform.Inst.IsDLCSubscribed(text);
				if (ImGui.Checkbox("Subscribed ", ref flag2))
				{
					DistributionPlatform.Inst.ToggleDLCSubscription(text);
				}
			}
		}
	}
}
