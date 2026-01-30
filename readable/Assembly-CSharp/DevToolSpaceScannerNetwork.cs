using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x020006AF RID: 1711
public class DevToolSpaceScannerNetwork : DevTool
{
	// Token: 0x06002A10 RID: 10768 RVA: 0x000F603C File Offset: 0x000F423C
	public DevToolSpaceScannerNetwork()
	{
		this.tableDrawer = ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry>.New().Column("WorldId", (DevToolSpaceScannerNetwork.Entry e) => e.worldId).Column("Network Quality (0->1)", (DevToolSpaceScannerNetwork.Entry e) => e.networkQuality).Column("Targets Detected", (DevToolSpaceScannerNetwork.Entry e) => e.targetsString).FixedHeight(300f).Build();
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x000F60E4 File Offset: 0x000F42E4
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("Game instance is null");
			return;
		}
		if (Game.Instance.spaceScannerNetworkManager == null)
		{
			ImGui.Text("SpaceScannerNetworkQualityManager instance is null");
			return;
		}
		if (ClusterManager.Instance == null)
		{
			ImGui.Text("ClusterManager instance is null");
			return;
		}
		if (ImGui.CollapsingHeader("Worlds Data"))
		{
			this.tableDrawer.Draw(DevToolSpaceScannerNetwork.GetData());
		}
		if (ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
		{
			ImGuiEx.DrawObject(Game.Instance.spaceScannerNetworkManager, null);
		}
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x000F6178 File Offset: 0x000F4378
	public static IEnumerable<DevToolSpaceScannerNetwork.Entry> GetData()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			yield return new DevToolSpaceScannerNetwork.Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), DevToolSpaceScannerNetwork.GetTargetsString(worldContainer));
		}
		List<WorldContainer>.Enumerator enumerator = default(List<WorldContainer>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000F6184 File Offset: 0x000F4384
	public static string GetTargetsString(WorldContainer world)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (!Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out spaceScannerWorldData))
		{
			return "<none>";
		}
		if (spaceScannerWorldData.targetIdsDetected.Count == 0)
		{
			return "<none>";
		}
		return string.Join(",", spaceScannerWorldData.targetIdsDetected);
	}

	// Token: 0x04001908 RID: 6408
	private ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry> tableDrawer;

	// Token: 0x0200156A RID: 5482
	public readonly struct Entry
	{
		// Token: 0x0600932B RID: 37675 RVA: 0x00375357 File Offset: 0x00373557
		public Entry(int worldId, float networkQuality, string targetsString)
		{
			this.worldId = worldId;
			this.networkQuality = networkQuality;
			this.targetsString = targetsString;
		}

		// Token: 0x040071B0 RID: 29104
		public readonly int worldId;

		// Token: 0x040071B1 RID: 29105
		public readonly float networkQuality;

		// Token: 0x040071B2 RID: 29106
		public readonly string targetsString;
	}
}
