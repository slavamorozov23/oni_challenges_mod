using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000810 RID: 2064
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Switch")]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060037D6 RID: 14294 RVA: 0x00139257 File Offset: 0x00137457
	public bool IsSwitchedOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060037D7 RID: 14295 RVA: 0x00139260 File Offset: 0x00137460
	// (remove) Token: 0x060037D8 RID: 14296 RVA: 0x00139298 File Offset: 0x00137498
	public event Action<bool> OnToggle;

	// Token: 0x060037D9 RID: 14297 RVA: 0x001392CD File Offset: 0x001374CD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.switchedOn = this.defaultState;
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x001392E4 File Offset: 0x001374E4
	protected override void OnSpawn()
	{
		this.openToggleIndex = this.openSwitch.SetTarget(this);
		if (this.OnToggle != null)
		{
			this.OnToggle(this.switchedOn);
		}
		if (this.manuallyControlled)
		{
			base.Subscribe<Switch>(493375141, Switch.OnRefreshUserMenuDelegate);
		}
		this.UpdateSwitchStatus();
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x0013933B File Offset: 0x0013753B
	public void HandleToggle()
	{
		this.Toggle();
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x00139343 File Offset: 0x00137543
	public bool IsHandlerOn()
	{
		return this.switchedOn;
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0013934B File Offset: 0x0013754B
	private void OnMinionToggle()
	{
		if (!DebugHandler.InstantBuildMode)
		{
			this.openSwitch.Toggle(this.openToggleIndex);
			return;
		}
		this.Toggle();
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0013936C File Offset: 0x0013756C
	protected virtual void Toggle()
	{
		this.SetState(!this.switchedOn);
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x00139380 File Offset: 0x00137580
	protected virtual void SetState(bool on)
	{
		if (this.switchedOn != on)
		{
			this.switchedOn = on;
			this.UpdateSwitchStatus();
			if (this.OnToggle != null)
			{
				this.OnToggle(this.switchedOn);
			}
			if (this.manuallyControlled)
			{
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
		}
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x001393DC File Offset: 0x001375DC
	protected virtual void OnRefreshUserMenu(object data)
	{
		LocString loc_string = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF : BUILDINGS.PREFABS.SWITCH.TURN_ON;
		LocString loc_string2 = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP : BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP;
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_power", loc_string, new System.Action(this.OnMinionToggle), global::Action.ToggleEnabled, null, null, null, loc_string2, true), 1f);
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x00139458 File Offset: 0x00137658
	protected virtual void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040021F1 RID: 8689
	[SerializeField]
	public bool manuallyControlled = true;

	// Token: 0x040021F2 RID: 8690
	[SerializeField]
	public bool defaultState = true;

	// Token: 0x040021F3 RID: 8691
	[Serialize]
	protected bool switchedOn = true;

	// Token: 0x040021F4 RID: 8692
	[MyCmpAdd]
	private Toggleable openSwitch;

	// Token: 0x040021F5 RID: 8693
	private int openToggleIndex;

	// Token: 0x040021F7 RID: 8695
	private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Switch>(delegate(Switch component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
