using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020006B5 RID: 1717
public class DevToolUI : DevTool
{
	// Token: 0x06002A3E RID: 10814 RVA: 0x000F7616 File Offset: 0x000F5816
	protected override void RenderTo(DevPanel panel)
	{
		this.RepopulateRaycastHits();
		this.DrawPingObject();
		this.DrawRaycastHits();
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000F762C File Offset: 0x000F582C
	private void DrawPingObject()
	{
		if (this.m_last_pinged_hit != null)
		{
			GameObject gameObject = this.m_last_pinged_hit.Value.gameObject;
			if (gameObject != null && gameObject)
			{
				ImGui.Text("Last Pinged: \"" + DevToolUI.GetQualifiedName(gameObject) + "\"");
				ImGui.SameLine();
				if (ImGui.Button("Inspect"))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				ImGui.Spacing();
				ImGui.Spacing();
			}
			else
			{
				this.m_last_pinged_hit = null;
			}
		}
		ImGui.Text("Press \",\" to ping the top hovered ui object");
		ImGui.Spacing();
		ImGui.Spacing();
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x000F76CB File Offset: 0x000F58CB
	private void Internal_Ping(RaycastResult raycastResult)
	{
		GameObject gameObject = raycastResult.gameObject;
		this.m_last_pinged_hit = new RaycastResult?(raycastResult);
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x000F76E4 File Offset: 0x000F58E4
	public static void PingHoveredObject()
	{
		using (ListPool<RaycastResult, DevToolUI>.PooledList pooledList = PoolsFor<DevToolUI>.AllocateList<RaycastResult>())
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (!(current == null) && current)
			{
				current.RaycastAll(new PointerEventData(current)
				{
					position = Input.mousePosition
				}, pooledList);
				DevToolUI devToolUI = DevToolManager.Instance.panels.AddOrGetDevTool<DevToolUI>();
				if (pooledList.Count > 0)
				{
					devToolUI.Internal_Ping(pooledList[0]);
				}
			}
		}
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000F7770 File Offset: 0x000F5970
	private void DrawRaycastHits()
	{
		if (this.m_raycast_hits.Count <= 0)
		{
			ImGui.Text("Didn't hit any ui");
			return;
		}
		ImGui.Text("Raycast Hits:");
		ImGui.Indent();
		for (int i = 0; i < this.m_raycast_hits.Count; i++)
		{
			RaycastResult raycastResult = this.m_raycast_hits[i];
			ImGui.BulletText(string.Format("[{0}] {1}", i, DevToolUI.GetQualifiedName(raycastResult.gameObject)));
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000F77F0 File Offset: 0x000F59F0
	private void RepopulateRaycastHits()
	{
		this.m_raycast_hits.Clear();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null || !current)
		{
			return;
		}
		current.RaycastAll(new PointerEventData(current)
		{
			position = Input.mousePosition
		}, this.m_raycast_hits);
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000F7844 File Offset: 0x000F5A44
	private static string GetQualifiedName(GameObject game_object)
	{
		KScreen componentInParent = game_object.GetComponentInParent<KScreen>();
		if (componentInParent != null)
		{
			return componentInParent.gameObject.name + " :: " + game_object.name;
		}
		return game_object.name ?? "";
	}

	// Token: 0x0400191B RID: 6427
	private List<RaycastResult> m_raycast_hits = new List<RaycastResult>();

	// Token: 0x0400191C RID: 6428
	private RaycastResult? m_last_pinged_hit;
}
