using System;
using ImGuiNET;

// Token: 0x0200069C RID: 1692
public class DevToolEntity_SearchGameObjects : DevTool
{
	// Token: 0x060029A9 RID: 10665 RVA: 0x000EF72D File Offset: 0x000ED92D
	public DevToolEntity_SearchGameObjects(Action<DevToolEntityTarget> onSelectionMadeFn)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x000EF73C File Offset: 0x000ED93C
	protected override void RenderTo(DevPanel panel)
	{
		ImGui.Text("Not implemented yet");
	}

	// Token: 0x04001896 RID: 6294
	private Action<DevToolEntityTarget> onSelectionMadeFn;
}
