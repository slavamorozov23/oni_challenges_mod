using System;
using System.IO;
using ImGuiNET;

// Token: 0x020006A1 RID: 1697
public class DevToolMenuNodeList
{
	// Token: 0x060029D7 RID: 10711 RVA: 0x000F1F80 File Offset: 0x000F0180
	public DevToolMenuNodeParent AddOrGetParentFor(string childPath)
	{
		string[] array = Path.GetDirectoryName(childPath).Split('/', StringSplitOptions.None);
		string text = "";
		DevToolMenuNodeParent devToolMenuNodeParent = this.root;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string split = array2[i];
			text += devToolMenuNodeParent.GetName();
			IMenuNode menuNode = devToolMenuNodeParent.children.Find((IMenuNode x) => x.GetName() == split);
			DevToolMenuNodeParent devToolMenuNodeParent3;
			if (menuNode != null)
			{
				DevToolMenuNodeParent devToolMenuNodeParent2 = menuNode as DevToolMenuNodeParent;
				if (devToolMenuNodeParent2 == null)
				{
					throw new Exception("Conflict! Both a leaf and parent node exist at path: " + text);
				}
				devToolMenuNodeParent3 = devToolMenuNodeParent2;
			}
			else
			{
				devToolMenuNodeParent3 = new DevToolMenuNodeParent(split);
				devToolMenuNodeParent.AddChild(devToolMenuNodeParent3);
			}
			devToolMenuNodeParent = devToolMenuNodeParent3;
		}
		return devToolMenuNodeParent;
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000F202C File Offset: 0x000F022C
	public DevToolMenuNodeAction AddAction(string path, System.Action onClickFn)
	{
		DevToolMenuNodeAction devToolMenuNodeAction = new DevToolMenuNodeAction(Path.GetFileName(path), onClickFn);
		this.AddOrGetParentFor(path).AddChild(devToolMenuNodeAction);
		return devToolMenuNodeAction;
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000F2054 File Offset: 0x000F0254
	public void Draw()
	{
		foreach (IMenuNode menuNode in this.root.children)
		{
			menuNode.Draw();
		}
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x000F20AC File Offset: 0x000F02AC
	public void DrawFull()
	{
		if (ImGui.BeginMainMenuBar())
		{
			this.Draw();
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x040018CE RID: 6350
	private DevToolMenuNodeParent root = new DevToolMenuNodeParent("<root>");
}
