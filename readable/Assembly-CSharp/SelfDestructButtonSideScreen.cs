using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E75 RID: 3701
public class SelfDestructButtonSideScreen : SideScreenContent
{
	// Token: 0x060075C5 RID: 30149 RVA: 0x002D0362 File Offset: 0x002CE562
	protected override void OnSpawn()
	{
		this.Refresh();
		this.button.onClick += this.TriggerDestruct;
	}

	// Token: 0x060075C6 RID: 30150 RVA: 0x002D0381 File Offset: 0x002CE581
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x060075C7 RID: 30151 RVA: 0x002D0388 File Offset: 0x002CE588
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CraftModuleInterface>() != null && target.HasTag(GameTags.RocketInSpace);
	}

	// Token: 0x060075C8 RID: 30152 RVA: 0x002D03A5 File Offset: 0x002CE5A5
	public override void SetTarget(GameObject target)
	{
		this.craftInterface = target.GetComponent<CraftModuleInterface>();
		this.acknowledgeWarnings = false;
		this.craftInterface.Subscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate);
		this.Refresh();
	}

	// Token: 0x060075C9 RID: 30153 RVA: 0x002D03D6 File Offset: 0x002CE5D6
	public override void ClearTarget()
	{
		if (this.craftInterface != null)
		{
			this.craftInterface.Unsubscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate, false);
			this.craftInterface = null;
		}
	}

	// Token: 0x060075CA RID: 30154 RVA: 0x002D0403 File Offset: 0x002CE603
	private void OnTagsChanged(object data)
	{
		if (((Boxed<TagChangedEventData>)data).value.tag == GameTags.RocketStranded)
		{
			this.Refresh();
		}
	}

	// Token: 0x060075CB RID: 30155 RVA: 0x002D0427 File Offset: 0x002CE627
	private void TriggerDestruct()
	{
		if (this.acknowledgeWarnings)
		{
			this.craftInterface.gameObject.Trigger(-1061799784, null);
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.acknowledgeWarnings = true;
		}
		this.Refresh();
	}

	// Token: 0x060075CC RID: 30156 RVA: 0x002D0460 File Offset: 0x002CE660
	private void Refresh()
	{
		if (this.craftInterface == null)
		{
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.MESSAGE_TEXT;
		if (this.acknowledgeWarnings)
		{
			this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT_CONFIRM;
			this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP_CONFIRM;
			return;
		}
		this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT;
		this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP;
	}

	// Token: 0x0400518F RID: 20879
	public KButton button;

	// Token: 0x04005190 RID: 20880
	public LocText statusText;

	// Token: 0x04005191 RID: 20881
	private CraftModuleInterface craftInterface;

	// Token: 0x04005192 RID: 20882
	private bool acknowledgeWarnings;

	// Token: 0x04005193 RID: 20883
	private static readonly EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen> TagsChangedDelegate = new EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen>(delegate(SelfDestructButtonSideScreen cmp, object data)
	{
		cmp.OnTagsChanged(data);
	});
}
