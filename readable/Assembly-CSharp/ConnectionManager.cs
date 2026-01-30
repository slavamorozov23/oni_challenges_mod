using System;
using KSerialization;
using STRINGS;

// Token: 0x0200076D RID: 1901
public class ConnectionManager : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06003040 RID: 12352 RVA: 0x001169FC File Offset: 0x00114BFC
	// (set) Token: 0x06003041 RID: 12353 RVA: 0x00116A04 File Offset: 0x00114C04
	public bool IsConnected
	{
		get
		{
			return this.connected;
		}
		set
		{
			this.connected = value;
			if (this.connectedMeter != null)
			{
				this.connectedMeter.SetPositionPercent(value ? 1f : 0f);
			}
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06003042 RID: 12354 RVA: 0x00116A2F File Offset: 0x00114C2F
	public bool WaitingForToggle
	{
		get
		{
			return this.toggleQueued;
		}
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x00116A37 File Offset: 0x00114C37
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleIdx = this.toggleable.SetTarget(this);
		base.Subscribe<ConnectionManager>(493375141, ConnectionManager.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x00116A64 File Offset: 0x00114C64
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.toggleQueued)
		{
			this.OnMenuToggle();
		}
		this.connectedMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_connected_target", "meter_connected", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.CONNECTED_SYMBOLS);
		this.connectedMeter.SetPositionPercent(this.IsConnected ? 1f : 0f);
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x00116AC7 File Offset: 0x00114CC7
	public void HandleToggle()
	{
		this.toggleQueued = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x00116AE1 File Offset: 0x00114CE1
	private void OnToggle()
	{
		this.IsConnected = !this.IsConnected;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x00116B08 File Offset: 0x00114D08
	private void OnMenuToggle()
	{
		if (!this.toggleable.IsToggleQueued(this.toggleIdx))
		{
			if (this.IsConnected)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.toggleQueued = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.toggleQueued = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.toggleable.Toggle(this.toggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x00116B8C File Offset: 0x00114D8C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showButton)
		{
			return;
		}
		bool isConnected = this.IsConnected;
		bool flag = this.toggleable.IsToggleQueued(this.toggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isConnected && !flag) || (!isConnected && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003049 RID: 12361 RVA: 0x00116C50 File Offset: 0x00114E50
	bool IToggleHandler.IsHandlerOn()
	{
		return this.IsConnected;
	}

	// Token: 0x04001CB8 RID: 7352
	[MyCmpAdd]
	private ToggleGeothermalVentConnection toggleable;

	// Token: 0x04001CB9 RID: 7353
	[MyCmpGet]
	private GeothermalVent vent;

	// Token: 0x04001CBA RID: 7354
	private int toggleIdx;

	// Token: 0x04001CBB RID: 7355
	private MeterController connectedMeter;

	// Token: 0x04001CBC RID: 7356
	public bool showButton;

	// Token: 0x04001CBD RID: 7357
	[Serialize]
	private bool connected;

	// Token: 0x04001CBE RID: 7358
	[Serialize]
	private bool toggleQueued;

	// Token: 0x04001CBF RID: 7359
	private static readonly EventSystem.IntraObjectHandler<ConnectionManager> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ConnectionManager>(delegate(ConnectionManager component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
