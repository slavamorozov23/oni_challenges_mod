using System;
using ImGuiNET;

// Token: 0x02000691 RID: 1681
public class DevToolBigBaseMutations : DevTool
{
	// Token: 0x0600296C RID: 10604 RVA: 0x000ECC99 File Offset: 0x000EAE99
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance != null)
		{
			this.ShowButtons();
			return;
		}
		ImGui.Text("Game not available");
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x000ECCBC File Offset: 0x000EAEBC
	private void ShowButtons()
	{
		if (ImGui.Button("Destroy Ladders"))
		{
			this.DestroyGameObjects<Ladder>(Components.Ladders, Tag.Invalid);
		}
		if (ImGui.Button("Destroy Tiles"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.FloorTiles);
		}
		if (ImGui.Button("Destroy Wires"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Wires);
		}
		if (ImGui.Button("Destroy Pipes"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Pipes);
		}
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x000ECD3C File Offset: 0x000EAF3C
	private void DestroyGameObjects<T>(Components.Cmps<T> componentsList, Tag filterForTag)
	{
		for (int i = componentsList.Count - 1; i >= 0; i--)
		{
			if (!componentsList[i].IsNullOrDestroyed() && (!(filterForTag != Tag.Invalid) || (componentsList[i] as KMonoBehaviour).gameObject.HasTag(filterForTag)))
			{
				Util.KDestroyGameObject(componentsList[i] as KMonoBehaviour);
			}
		}
	}
}
