using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200068C RID: 1676
public class DevToolAccessControl : DevTool
{
	// Token: 0x06002952 RID: 10578 RVA: 0x000EB3A6 File Offset: 0x000E95A6
	public DevToolAccessControl()
	{
		DevToolAccessControl.Instance = this;
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000EB3C0 File Offset: 0x000E95C0
	private bool Init()
	{
		if (Game.Instance == null)
		{
			return false;
		}
		if (!this.initialized)
		{
			this.initialized = true;
			foreach (Tag key in (from e in Assets.GetPrefabsWithTag(GameTags.BaseMinion)
			select e.GetComponent<KPrefabID>().PrefabTag).ToList<Tag>())
			{
				this.minionsByType.Add(key, new List<MinionAssignablesProxy>());
			}
			this.robotTypes = (from e in Assets.GetPrefabsWithTag(GameTags.Robots.Behaviours.HasDoorPermissions)
			select e.GetComponent<KPrefabID>().PrefabTag).ToList<Tag>();
		}
		return true;
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x000EB4A8 File Offset: 0x000E96A8
	protected override void RenderTo(DevPanel panel)
	{
		if (this.Init())
		{
			if (this.DoorSelected())
			{
				ImGui.Checkbox("Lock Selection", ref this.lockSelected);
				this.MinionContents();
				this.RobotContents();
			}
			this.GridRestrictionSerializerContents();
			return;
		}
		ImGui.Text("No Access Control selected");
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x000EB4E8 File Offset: 0x000E96E8
	private bool DoorSelected()
	{
		return SelectTool.Instance != null && SelectTool.Instance.selected != null && this.SetDoorAccessControl() != null;
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000EB518 File Offset: 0x000E9718
	private AccessControl SetDoorAccessControl()
	{
		AccessControl component = SelectTool.Instance.selected.GetComponent<AccessControl>();
		if (component != this.selectedAccessControl && !this.lockSelected)
		{
			this.selectedAccessControl = component;
		}
		return this.selectedAccessControl;
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x000EB558 File Offset: 0x000E9758
	private void MinionContents()
	{
		foreach (MinionAssignablesProxy minionAssignablesProxy in Components.MinionAssignablesProxy.Items)
		{
			Tag minionModel = minionAssignablesProxy.GetMinionModel();
			if (!this.minionsByType[minionModel].Contains(minionAssignablesProxy))
			{
				this.minionsByType[minionModel].Add(minionAssignablesProxy);
			}
		}
		foreach (Tag tag in this.minionsByType.Keys)
		{
			ImGui.PushID(tag.Name);
			ImGui.Text(tag.Name);
			AccessControl.Permission setPermission = this.selectedAccessControl.GetSetPermission(Tag.Invalid.GetHashCode(), tag);
			bool left = setPermission == AccessControl.Permission.GoLeft || setPermission == AccessControl.Permission.Both;
			bool right = setPermission == AccessControl.Permission.GoRight || setPermission == AccessControl.Permission.Both;
			ImGui.SameLine();
			if (ImGui.Checkbox("Left", ref left))
			{
				this.UpdateAccess(tag, left, right);
			}
			ImGui.SameLine();
			if (ImGui.Checkbox("Right", ref right))
			{
				this.UpdateAccess(tag, left, right);
			}
			ImGui.PopID();
			ImGui.Indent();
			ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.75f);
			foreach (MinionAssignablesProxy minionAssignablesProxy2 in this.minionsByType[tag])
			{
				ImGui.PushID(minionAssignablesProxy2.TargetInstanceID);
				ImGui.Text(minionAssignablesProxy2.target.GetProperName());
				AccessControl.Permission setPermission2 = this.selectedAccessControl.GetSetPermission(minionAssignablesProxy2);
				ImGui.SameLine();
				bool left2 = setPermission2 == AccessControl.Permission.GoLeft || setPermission2 == AccessControl.Permission.Both;
				bool right2 = setPermission2 == AccessControl.Permission.GoRight || setPermission2 == AccessControl.Permission.Both;
				if (ImGui.Checkbox("Left", ref left2))
				{
					this.UpdateMinionAccess(minionAssignablesProxy2, left2, right2);
				}
				ImGui.SameLine();
				if (ImGui.Checkbox("Right", ref right2))
				{
					this.UpdateMinionAccess(minionAssignablesProxy2, left2, right2);
				}
				ImGui.PopID();
			}
			ImGui.PopStyleVar();
			ImGui.Unindent();
		}
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x000EB7CC File Offset: 0x000E99CC
	private void RobotContents()
	{
		ImGui.PushID(GameTags.Robot.Name);
		ImGui.Text(GameTags.Robot.Name);
		AccessControl.Permission setPermission = this.selectedAccessControl.GetSetPermission(Tag.Invalid.GetHashCode(), GameTags.Robot);
		bool left = setPermission == AccessControl.Permission.GoLeft || setPermission == AccessControl.Permission.Both;
		bool right = setPermission == AccessControl.Permission.GoRight || setPermission == AccessControl.Permission.Both;
		ImGui.SameLine();
		if (ImGui.Checkbox("Left", ref left))
		{
			this.UpdateAccess(GameTags.Robot, left, right);
		}
		ImGui.SameLine();
		if (ImGui.Checkbox("Right", ref right))
		{
			this.UpdateAccess(GameTags.Robot, left, right);
		}
		ImGui.PopID();
		ImGui.Indent();
		ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.75f);
		foreach (Tag tag in this.robotTypes)
		{
			ImGui.PushID(tag.Name);
			ImGui.Text(tag.Name);
			AccessControl.Permission setPermission2 = this.selectedAccessControl.GetSetPermission(tag);
			bool left2 = setPermission2 == AccessControl.Permission.GoLeft || setPermission2 == AccessControl.Permission.Both;
			bool right2 = setPermission2 == AccessControl.Permission.GoRight || setPermission2 == AccessControl.Permission.Both;
			ImGui.SameLine();
			if (ImGui.Checkbox("Left", ref left2))
			{
				this.UpdateAccess(tag, left2, right2);
			}
			ImGui.SameLine();
			if (ImGui.Checkbox("Right", ref right2))
			{
				this.UpdateAccess(tag, left2, right2);
			}
			ImGui.PopID();
		}
		ImGui.PopStyleVar();
		ImGui.Unindent();
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x000EB968 File Offset: 0x000E9B68
	private void GridRestrictionSerializerContents()
	{
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x000EB96C File Offset: 0x000E9B6C
	private void UpdateMinionAccess(MinionAssignablesProxy proxy, bool left, bool right)
	{
		AccessControl.Permission permission;
		if (left)
		{
			if (right)
			{
				permission = AccessControl.Permission.Both;
			}
			else
			{
				permission = AccessControl.Permission.GoLeft;
			}
		}
		else if (right)
		{
			permission = AccessControl.Permission.GoRight;
		}
		else
		{
			permission = AccessControl.Permission.Neither;
		}
		this.selectedAccessControl.SetPermission(proxy, permission);
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x000EB9A0 File Offset: 0x000E9BA0
	private void UpdateAccess(Tag id, bool left, bool right)
	{
		AccessControl.Permission permission;
		if (left)
		{
			if (right)
			{
				permission = AccessControl.Permission.Both;
			}
			else
			{
				permission = AccessControl.Permission.GoLeft;
			}
		}
		else if (right)
		{
			permission = AccessControl.Permission.GoRight;
		}
		else
		{
			permission = AccessControl.Permission.Neither;
		}
		this.selectedAccessControl.SetPermission(id, permission);
	}

	// Token: 0x0400186B RID: 6251
	public static DevToolAccessControl Instance;

	// Token: 0x0400186C RID: 6252
	private bool initialized;

	// Token: 0x0400186D RID: 6253
	private Dictionary<Tag, List<MinionAssignablesProxy>> minionsByType = new Dictionary<Tag, List<MinionAssignablesProxy>>();

	// Token: 0x0400186E RID: 6254
	private AccessControl selectedAccessControl;

	// Token: 0x0400186F RID: 6255
	private bool lockSelected;

	// Token: 0x04001870 RID: 6256
	private List<Tag> robotTypes;

	// Token: 0x02001557 RID: 5463
	private struct MinionPermissions
	{
		// Token: 0x04007186 RID: 29062
		public bool Left;

		// Token: 0x04007187 RID: 29063
		public bool Right;

		// Token: 0x04007188 RID: 29064
		public MinionAssignablesProxy Proxy;
	}
}
