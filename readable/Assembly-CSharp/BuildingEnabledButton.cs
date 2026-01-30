using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006FC RID: 1788
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingEnabledButton")]
public class BuildingEnabledButton : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06002C47 RID: 11335 RVA: 0x00101F83 File Offset: 0x00100183
	// (set) Token: 0x06002C48 RID: 11336 RVA: 0x00101FA8 File Offset: 0x001001A8
	public bool IsEnabled
	{
		get
		{
			return this.Operational != null && this.Operational.GetFlag(BuildingEnabledButton.EnabledFlag);
		}
		set
		{
			this.Operational.SetFlag(BuildingEnabledButton.EnabledFlag, value);
			Game.Instance.userMenu.Refresh(base.gameObject);
			this.buildingEnabled = value;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.BuildingDisabled, !this.buildingEnabled, null);
			base.Trigger(1088293757, BoxedBools.Box(this.buildingEnabled));
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06002C49 RID: 11337 RVA: 0x0010201D File Offset: 0x0010021D
	public bool WaitingForDisable
	{
		get
		{
			return this.IsEnabled && this.Toggleable.IsToggleQueued(this.ToggleIdx);
		}
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x0010203A File Offset: 0x0010023A
	protected override void OnPrefabInit()
	{
		this.ToggleIdx = this.Toggleable.SetTarget(this);
		base.Subscribe<BuildingEnabledButton>(493375141, BuildingEnabledButton.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06002C4B RID: 11339 RVA: 0x0010205F File Offset: 0x0010025F
	protected override void OnSpawn()
	{
		this.IsEnabled = this.buildingEnabled;
		if (this.queuedToggle)
		{
			this.OnMenuToggle();
		}
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x0010207B File Offset: 0x0010027B
	public void HandleToggle()
	{
		this.queuedToggle = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x06002C4D RID: 11341 RVA: 0x00102095 File Offset: 0x00100295
	public bool IsHandlerOn()
	{
		return this.IsEnabled;
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x0010209D File Offset: 0x0010029D
	private void OnToggle()
	{
		this.IsEnabled = !this.IsEnabled;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x001020C4 File Offset: 0x001002C4
	private void OnMenuToggle()
	{
		if (!this.Toggleable.IsToggleQueued(this.ToggleIdx))
		{
			if (this.IsEnabled)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.queuedToggle = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.queuedToggle = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.Toggleable.Toggle(this.ToggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002C50 RID: 11344 RVA: 0x00102148 File Offset: 0x00100348
	private void OnRefreshUserMenu(object data)
	{
		bool isEnabled = this.IsEnabled;
		bool flag = this.Toggleable.IsToggleQueued(this.ToggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isEnabled && !flag) || (!isEnabled && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME_OFF, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04001A43 RID: 6723
	[MyCmpAdd]
	private Toggleable Toggleable;

	// Token: 0x04001A44 RID: 6724
	[MyCmpReq]
	private Operational Operational;

	// Token: 0x04001A45 RID: 6725
	private int ToggleIdx;

	// Token: 0x04001A46 RID: 6726
	[Serialize]
	private bool buildingEnabled = true;

	// Token: 0x04001A47 RID: 6727
	[Serialize]
	private bool queuedToggle;

	// Token: 0x04001A48 RID: 6728
	public static readonly Operational.Flag EnabledFlag = new Operational.Flag("building_enabled", Operational.Flag.Type.Functional);

	// Token: 0x04001A49 RID: 6729
	private static readonly EventSystem.IntraObjectHandler<BuildingEnabledButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingEnabledButton>(delegate(BuildingEnabledButton component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
