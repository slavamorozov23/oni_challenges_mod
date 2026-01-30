using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x020006B3 RID: 1715
public class DevToolStoryManager : DevTool
{
	// Token: 0x06002A38 RID: 10808 RVA: 0x000F7260 File Offset: 0x000F5460
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Story Instance Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.DrawStoryInstanceData();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Story Telemetry Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.DrawTelemetryData();
		}
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x000F7290 File Offset: 0x000F5490
	private void DrawStoryInstanceData()
	{
		if (StoryManager.Instance == null)
		{
			ImGui.Text("Couldn't find StoryManager instance");
			return;
		}
		ImGui.Text(string.Format("Stories (count: {0})", StoryManager.Instance.GetStoryInstances().Count));
		string str = (StoryManager.Instance.GetHighestCoordinate() == -2) ? "Before stories" : StoryManager.Instance.GetHighestCoordinate().ToString();
		ImGui.Text("Highest generated: " + str);
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in StoryManager.Instance.GetStoryInstances())
		{
			ImGui.Text(" - " + keyValuePair.Value.storyId + ": " + keyValuePair.Value.CurrentState.ToString());
		}
		if (StoryManager.Instance.GetStoryInstances().Count == 0)
		{
			ImGui.Text(" - No stories");
		}
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x000F73AC File Offset: 0x000F55AC
	private void DrawTelemetryData()
	{
		ImGuiEx.DrawObjectTable<StoryManager.StoryTelemetry>("ID_telemetry", StoryManager.GetTelemetry(), null);
	}
}
