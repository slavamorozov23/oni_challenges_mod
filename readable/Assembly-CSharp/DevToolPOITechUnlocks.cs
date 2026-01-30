using System;
using ImGuiNET;

// Token: 0x020006A7 RID: 1703
public class DevToolPOITechUnlocks : DevTool
{
	// Token: 0x060029F2 RID: 10738 RVA: 0x000F2C9C File Offset: 0x000F0E9C
	protected override void RenderTo(DevPanel panel)
	{
		if (Research.Instance == null)
		{
			return;
		}
		foreach (TechItem techItem in Db.Get().TechItems.resources)
		{
			if (techItem.isPOIUnlock)
			{
				ImGui.Text(techItem.Id);
				ImGui.SameLine();
				bool flag = techItem.IsComplete();
				if (ImGui.Checkbox("Unlocked ", ref flag))
				{
					techItem.POIUnlocked();
				}
			}
		}
	}
}
