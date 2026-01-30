using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x020006A3 RID: 1699
public class DevToolMenuNodeParent : IMenuNode
{
	// Token: 0x060029DE RID: 10718 RVA: 0x000F20D8 File Offset: 0x000F02D8
	public DevToolMenuNodeParent(string name)
	{
		this.name = name;
		this.children = new List<IMenuNode>();
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000F20F2 File Offset: 0x000F02F2
	public void AddChild(IMenuNode menuNode)
	{
		this.children.Add(menuNode);
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x000F2100 File Offset: 0x000F0300
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000F2108 File Offset: 0x000F0308
	public void Draw()
	{
		if (ImGui.BeginMenu(this.name))
		{
			foreach (IMenuNode menuNode in this.children)
			{
				menuNode.Draw();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x040018CF RID: 6351
	public string name;

	// Token: 0x040018D0 RID: 6352
	public List<IMenuNode> children;
}
