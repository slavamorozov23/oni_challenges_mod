using System;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200068D RID: 1677
public class DevToolAllThingsCritter : DevTool
{
	// Token: 0x0600295C RID: 10588 RVA: 0x000EB9D1 File Offset: 0x000E9BD1
	protected override void RenderTo(DevPanel panel)
	{
		if (SelectTool.Instance.selected != null || this.lockObject != null)
		{
			this.Contents();
			return;
		}
		ImGui.Text("No Critter Selected");
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x000EBA04 File Offset: 0x000E9C04
	private void Contents()
	{
		ImGui.Spacing();
		if (Camera.main != null && SelectTool.Instance != null)
		{
			GameObject gameObject = null;
			ImGui.Checkbox("Lock", ref this.follow);
			if (this.follow)
			{
				if (this.lockObject == null && SelectTool.Instance.selected != null && SelectTool.Instance.selected.GetComponent<KPrefabID>() != null && SelectTool.Instance.selected.HasTag(GameTags.Creature))
				{
					this.lockObject = SelectTool.Instance.selected.gameObject;
				}
				gameObject = this.lockObject;
			}
			else if (SelectTool.Instance.selected != null)
			{
				if (SelectTool.Instance.selected.GetComponent<KPrefabID>() != null && SelectTool.Instance.selected.HasTag(GameTags.Creature))
				{
					gameObject = SelectTool.Instance.selected.gameObject;
				}
				this.lockObject = null;
			}
			if (gameObject != null)
			{
				ImGuiEx.SimpleField("Name", DevToolEntity.GetNameFor(gameObject));
				Vector3 position = gameObject.transform.GetPosition();
				string field_value = string.Format("X={0:F2}, Y={1:F2}, Z={2:F2}", position.x, position.y, position.z);
				ImGuiEx.SimpleField("Position", field_value);
				ImGuiEx.SimpleField("Cell", Grid.PosToCell(gameObject));
				this.NavigatorContents(position, gameObject);
				this.OccupyAreaContents(gameObject);
				this.ColliderContents(gameObject);
				this.CritterTemperatureMonitorContents(gameObject);
			}
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x000EBBA4 File Offset: 0x000E9DA4
	private void NavigatorContents(Vector3 pos, GameObject go)
	{
		Navigator component = go.GetComponent<Navigator>();
		if (component != null && ImGui.CollapsingHeader("Navigator", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Checkbox("Draw", ref this.drawNavDots);
			Vector2 positionFor = DevToolEntity.GetPositionFor(component.gameObject);
			string str = string.Format("X={0:F2}, Y={1:F2}", pos.x, pos.y);
			ImGui.TextColored(Color.green, "World: " + str);
			if (this.drawNavDots)
			{
				ImGui.GetBackgroundDrawList().AddCircleFilled(positionFor, 10f, ImGui.GetColorU32(Color.green));
			}
			Vector2 vector = component.GetComponent<KBatchedAnimController>().GetPivotSymbolPosition();
			Vector2 screenPosition = DevToolEntity.GetScreenPosition(vector);
			string str2 = string.Format("X={0:F2}, Y={1:F2}", vector.x, vector.y);
			ImGui.TextColored(Color.blue, "Pivot: " + str2);
			if (this.drawNavDots)
			{
				ImGui.GetBackgroundDrawList().AddCircleFilled(screenPosition, 10f, ImGui.GetColorU32(Color.blue));
			}
			TransitionDriver transitionDriver = component.transitionDriver;
			if (transitionDriver.GetTransition != null)
			{
				if (transitionDriver.GetTransition.navGridTransition.useXOffset)
				{
					Vector2 vector2 = go.GetComponent<KBoxCollider2D>().size / 2f;
					if (transitionDriver.GetTransition.x > 0)
					{
						pos.x += vector2.x;
					}
					else if (transitionDriver.GetTransition.x < 0)
					{
						pos.x -= vector2.x;
					}
					Vector2 screenPosition2 = DevToolEntity.GetScreenPosition(pos);
					string str3 = string.Format("X={0:F2}, Y={1:F2}", pos.x, pos.y);
					ImGui.TextColored(Color.magenta, "Nav Transition: " + str3);
					if (this.drawNavDots)
					{
						ImGui.GetBackgroundDrawList().AddCircleFilled(screenPosition2, 10f, ImGui.GetColorU32(Color.magenta));
					}
				}
				ImGuiEx.SimpleField("Transition", transitionDriver.GetTransition.navGridTransition.ToString());
			}
		}
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000EBDF8 File Offset: 0x000E9FF8
	private void OccupyAreaContents(GameObject go)
	{
		OccupyArea component = go.GetComponent<OccupyArea>();
		if (component != null && ImGui.CollapsingHeader("Occupy Area", ImGuiTreeNodeFlags.DefaultOpen))
		{
			Extents extents = component.GetExtents();
			ImGui.Checkbox("Draw Occupy Area", ref this.drawOccupyArea);
			if (this.drawOccupyArea)
			{
				Vector2 screenPosition = DevToolEntity.GetScreenPosition(Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(go), extents.width, extents.height)));
				Vector2 screenPosition2 = DevToolEntity.GetScreenPosition(Grid.CellToPos(Grid.XYToCell(extents.x, extents.y)));
				DevToolEntity.DrawScreenRect(new ValueTuple<Vector2, Vector2>(screenPosition2, screenPosition), go.name, Color.cyan, new Color(0f, 1f, 1f, 0.33f), default(Option<DevToolUtil.TextAlignment>));
			}
			ImGui.Text(string.Format("X={0:F2}, Y={1:F2}", extents.x, extents.y));
			ImGui.Text(string.Format("Width={0:F2}, Height={1:F2}", extents.width, extents.height));
		}
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x000EBF20 File Offset: 0x000EA120
	private void ColliderContents(GameObject go)
	{
		KCollider2D component = go.GetComponent<KCollider2D>();
		if (component != null && ImGui.CollapsingHeader("Collider", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Checkbox("Draw Collider", ref this.drawCollider);
			if (this.drawCollider)
			{
				Vector2 screenPosition = DevToolEntity.GetScreenPosition(component.bounds.min);
				Vector2 screenPosition2 = DevToolEntity.GetScreenPosition(component.bounds.max);
				DevToolEntity.DrawScreenRect(new ValueTuple<Vector2, Vector2>(screenPosition, screenPosition2), go.name, Color.green, new Color(0f, 1f, 0f, 0.33f), default(Option<DevToolUtil.TextAlignment>));
			}
			string field_value = string.Format("X={0:F2}, Y={1:F2}", component.offset.x, component.offset.y);
			ImGuiEx.SimpleField("Offset", field_value);
			ImGui.Text("Bounds");
			ImGuiEx.SimpleField("Offset", field_value);
			string field_value2 = string.Format("{0} Cell: {1}", component.bounds.min, Grid.PosToCell(component.bounds.min));
			ImGuiEx.SimpleField("Min", field_value2);
			string field_value3 = string.Format("{0} Cell: {1}", component.bounds.max, Grid.PosToCell(component.bounds.max));
			ImGuiEx.SimpleField("Max", field_value3);
			ImGuiEx.SimpleField("Center", component.bounds.center);
		}
	}

	// Token: 0x06002961 RID: 10593 RVA: 0x000EC0D0 File Offset: 0x000EA2D0
	private void CritterTemperatureMonitorContents(GameObject go)
	{
		CritterTemperatureMonitor.Instance smi = go.GetSMI<CritterTemperatureMonitor.Instance>();
		if (smi != null && ImGui.CollapsingHeader("Temperature Monitor", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGuiEx.SimpleField("Current State", smi.GetCurrentState().name.Replace("root.", ""));
			ImGuiEx.SimpleField("External", smi.GetTemperatureExternal());
			ImGuiEx.SimpleField("Internal", smi.GetTemperatureInternal());
		}
	}

	// Token: 0x04001871 RID: 6257
	private bool follow;

	// Token: 0x04001872 RID: 6258
	private GameObject lockObject;

	// Token: 0x04001873 RID: 6259
	private bool drawNavDots = true;

	// Token: 0x04001874 RID: 6260
	private bool drawOccupyArea;

	// Token: 0x04001875 RID: 6261
	private bool drawCollider;
}
