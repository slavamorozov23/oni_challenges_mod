using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x020006AA RID: 1706
public class DevToolResearchDebugger : DevTool
{
	// Token: 0x060029F8 RID: 10744 RVA: 0x000F321F File Offset: 0x000F141F
	public DevToolResearchDebugger()
	{
		this.RequiresGameRunning = true;
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000F3230 File Offset: 0x000F1430
	protected override void RenderTo(DevPanel panel)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			ImGui.Text("No Active Research");
			return;
		}
		ImGui.Text("Active Research");
		ImGui.Text("ID: " + activeResearch.tech.Id);
		ImGui.Text("Name: " + Util.StripTextFormatting(activeResearch.tech.Name));
		ImGui.Separator();
		ImGui.Text("Active Research Inventory");
		foreach (KeyValuePair<string, float> keyValuePair in new Dictionary<string, float>(activeResearch.progressInventory.PointsByTypeID))
		{
			if (activeResearch.tech.RequiresResearchType(keyValuePair.Key))
			{
				float num = activeResearch.tech.costsByResearchTypeID[keyValuePair.Key];
				float value = keyValuePair.Value;
				if (ImGui.Button("Fill"))
				{
					value = num;
				}
				ImGui.SameLine();
				ImGui.SetNextItemWidth(100f);
				ImGui.InputFloat(keyValuePair.Key, ref value, 1f, 10f);
				ImGui.SameLine();
				ImGui.Text(string.Format("of {0}", num));
				activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key] = Mathf.Clamp(value, 0f, num);
			}
		}
		ImGui.Separator();
		ImGui.Text("Global Points Inventory");
		foreach (KeyValuePair<string, float> keyValuePair2 in Research.Instance.globalPointInventory.PointsByTypeID)
		{
			ImGui.Text(keyValuePair2.Key + ": " + keyValuePair2.Value.ToString());
		}
	}
}
