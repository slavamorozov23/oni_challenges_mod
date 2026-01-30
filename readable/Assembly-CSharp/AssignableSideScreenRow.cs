using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E17 RID: 3607
[AddComponentMenu("KMonoBehaviour/scripts/AssignableSideScreenRow")]
public class AssignableSideScreenRow : KMonoBehaviour
{
	// Token: 0x06007263 RID: 29283 RVA: 0x002BB3AC File Offset: 0x002B95AC
	public void Refresh(object data = null)
	{
		if (!this.sideScreen.targetAssignable.CanAssignTo(this.targetIdentity))
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Disabled;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED;
		}
		else if (this.sideScreen.targetAssignable.assignee == this.targetIdentity)
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Selected;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGNED;
		}
		else
		{
			bool flag = false;
			KMonoBehaviour kmonoBehaviour = this.targetIdentity as KMonoBehaviour;
			if (kmonoBehaviour != null)
			{
				Ownables component = kmonoBehaviour.GetComponent<Ownables>();
				if (component != null)
				{
					AssignableSlotInstance[] slots = component.GetSlots(this.sideScreen.targetAssignable.slot);
					if (slots != null && slots.Length != 0)
					{
						AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance s) => !s.IsAssigned());
						if (assignableSlotInstance == null)
						{
							assignableSlotInstance = slots[0];
						}
						if (assignableSlotInstance != null && assignableSlotInstance.IsAssigned())
						{
							this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
							this.assignmentText.text = assignableSlotInstance.assignable.GetProperName();
							flag = true;
						}
					}
				}
				Equipment component2 = kmonoBehaviour.GetComponent<Equipment>();
				if (component2 != null)
				{
					AssignableSlotInstance[] slots2 = component2.GetSlots(this.sideScreen.targetAssignable.slot);
					if (slots2 != null && slots2.Length != 0)
					{
						AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance s) => !s.IsAssigned());
						if (assignableSlotInstance2 == null)
						{
							assignableSlotInstance2 = slots2[0];
						}
						if (assignableSlotInstance2 != null && assignableSlotInstance2.IsAssigned())
						{
							this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
							this.assignmentText.text = assignableSlotInstance2.assignable.GetProperName();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				this.currentState = AssignableSideScreenRow.AssignableState.Unassigned;
				this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED;
			}
		}
		this.toggle.ChangeState((int)this.currentState);
	}

	// Token: 0x06007264 RID: 29284 RVA: 0x002BB596 File Offset: 0x002B9796
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(ref this.refreshHandle);
		base.OnCleanUp();
	}

	// Token: 0x06007265 RID: 29285 RVA: 0x002BB5B0 File Offset: 0x002B97B0
	public void SetContent(IAssignableIdentity identity_object, Action<IAssignableIdentity> selectionCallback, AssignableSideScreen assignableSideScreen)
	{
		Game.Instance.Unsubscribe(ref this.refreshHandle);
		this.refreshHandle = Game.Instance.Subscribe(-2146166042, delegate(object o)
		{
			if (this != null && this.gameObject != null && this.gameObject.activeInHierarchy)
			{
				this.Refresh(null);
			}
		});
		this.toggle = base.GetComponent<MultiToggle>();
		this.sideScreen = assignableSideScreen;
		this.targetIdentity = identity_object;
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, base.gameObject, false);
			this.portraitInstance.transform.SetSiblingIndex(1);
			this.portraitInstance.SetAlpha(1f);
		}
		this.toggle.onClick = delegate()
		{
			selectionCallback(this.targetIdentity);
		};
		this.portraitInstance.SetIdentityObject(identity_object, false);
		base.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.GetTooltip);
		this.Refresh(null);
	}

	// Token: 0x06007266 RID: 29286 RVA: 0x002BB6AC File Offset: 0x002B98AC
	private string GetTooltip()
	{
		ToolTip component = base.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		if (this.sideScreen.targetAssignable.customAssignablesUITooltipFunc != null)
		{
			return this.sideScreen.targetAssignable.customAssignablesUITooltipFunc(this.targetIdentity.GetSoleOwner());
		}
		if (this.targetIdentity != null && !this.targetIdentity.IsNull())
		{
			AssignableSideScreenRow.AssignableState assignableState = this.currentState;
			if (assignableState != AssignableSideScreenRow.AssignableState.Selected)
			{
				if (assignableState != AssignableSideScreenRow.AssignableState.Disabled)
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGN_TO_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
				else
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
			}
			else
			{
				component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGN_TOOLTIP, this.targetIdentity.GetProperName()), null);
			}
		}
		return "";
	}

	// Token: 0x04004F02 RID: 20226
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x04004F03 RID: 20227
	[SerializeField]
	private LocText assignmentText;

	// Token: 0x04004F04 RID: 20228
	public AssignableSideScreen sideScreen;

	// Token: 0x04004F05 RID: 20229
	private CrewPortrait portraitInstance;

	// Token: 0x04004F06 RID: 20230
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x04004F07 RID: 20231
	public IAssignableIdentity targetIdentity;

	// Token: 0x04004F08 RID: 20232
	public AssignableSideScreenRow.AssignableState currentState;

	// Token: 0x04004F09 RID: 20233
	private int refreshHandle = -1;

	// Token: 0x0200209D RID: 8349
	public enum AssignableState
	{
		// Token: 0x040096B1 RID: 38577
		Selected,
		// Token: 0x040096B2 RID: 38578
		AssignedToOther,
		// Token: 0x040096B3 RID: 38579
		Unassigned,
		// Token: 0x040096B4 RID: 38580
		Disabled
	}
}
