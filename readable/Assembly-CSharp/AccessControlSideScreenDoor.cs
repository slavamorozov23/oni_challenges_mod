using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E0F RID: 3599
[AddComponentMenu("KMonoBehaviour/scripts/AccessControlSideScreenDoor")]
public class AccessControlSideScreenDoor : KMonoBehaviour
{
	// Token: 0x060071FF RID: 29183 RVA: 0x002B933D File Offset: 0x002B753D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.leftButton.onClick += this.OnPermissionButtonClicked;
		this.rightButton.onClick += this.OnPermissionButtonClicked;
	}

	// Token: 0x06007200 RID: 29184 RVA: 0x002B9374 File Offset: 0x002B7574
	private void OnPermissionButtonClicked()
	{
		AccessControl.Permission arg;
		if (this.leftButton.isOn)
		{
			if (this.rightButton.isOn)
			{
				arg = AccessControl.Permission.Both;
			}
			else
			{
				arg = AccessControl.Permission.GoLeft;
			}
		}
		else if (this.rightButton.isOn)
		{
			arg = AccessControl.Permission.GoRight;
		}
		else
		{
			arg = AccessControl.Permission.Neither;
		}
		this.UpdateButtonStates(false);
		if (this.permissionChangedCallback != null)
		{
			this.permissionChangedCallback(this.targetIdentity, arg);
			return;
		}
		this.permissionChangedTagCallback(this.targetTag, arg);
	}

	// Token: 0x06007201 RID: 29185 RVA: 0x002B93EC File Offset: 0x002B75EC
	protected virtual void UpdateButtonStates(bool isDefault)
	{
		ToolTip component = this.leftButton.GetComponent<ToolTip>();
		ToolTip component2 = this.rightButton.GetComponent<ToolTip>();
		if (this.isUpDown)
		{
			component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_DISABLED);
			component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_DISABLED);
			return;
		}
		component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_DISABLED);
		component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_DISABLED);
	}

	// Token: 0x06007202 RID: 29186 RVA: 0x002B94AA File Offset: 0x002B76AA
	public void SetRotated(bool rotated)
	{
		this.isUpDown = rotated;
	}

	// Token: 0x06007203 RID: 29187 RVA: 0x002B94B3 File Offset: 0x002B76B3
	public void SetContent(AccessControl.Permission permission, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange)
	{
		this.permissionChangedCallback = onPermissionChange;
		this.leftButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft);
		this.rightButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight);
		this.UpdateButtonStates(false);
	}

	// Token: 0x06007204 RID: 29188 RVA: 0x002B94ED File Offset: 0x002B76ED
	public void SetDefaultContent(Tag defaultTag, AccessControl.Permission permission, Action<Tag, AccessControl.Permission> onPermissionChange)
	{
		this.SetContent(permission, onPermissionChange);
		this.targetTag = defaultTag;
	}

	// Token: 0x06007205 RID: 29189 RVA: 0x002B94FE File Offset: 0x002B76FE
	public void SetContent(AccessControl.Permission permission, Action<Tag, AccessControl.Permission> onPermissionChange)
	{
		this.permissionChangedTagCallback = onPermissionChange;
		this.leftButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft);
		this.rightButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight);
		this.UpdateButtonStates(false);
	}

	// Token: 0x04004EC3 RID: 20163
	public KToggle leftButton;

	// Token: 0x04004EC4 RID: 20164
	public KToggle rightButton;

	// Token: 0x04004EC5 RID: 20165
	private Action<MinionAssignablesProxy, AccessControl.Permission> permissionChangedCallback;

	// Token: 0x04004EC6 RID: 20166
	private Action<Tag, AccessControl.Permission> permissionChangedTagCallback;

	// Token: 0x04004EC7 RID: 20167
	private bool isUpDown;

	// Token: 0x04004EC8 RID: 20168
	protected MinionAssignablesProxy targetIdentity;

	// Token: 0x04004EC9 RID: 20169
	protected Tag targetTag;
}
