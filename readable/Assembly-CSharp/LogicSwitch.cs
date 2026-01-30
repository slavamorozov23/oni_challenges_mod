using System;
using System.Collections;
using KSerialization;
using UnityEngine;

// Token: 0x020007A7 RID: 1959
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06003355 RID: 13141 RVA: 0x00125516 File Offset: 0x00123716
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicSwitch>(-905833192, LogicSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x00125530 File Offset: 0x00123730
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wasOn = this.switchedOn;
		this.UpdateLogicCircuit();
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x00125584 File Offset: 0x00123784
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x0012558C File Offset: 0x0012378C
	private void OnCopySettings(object data)
	{
		LogicSwitch component = ((GameObject)data).GetComponent<LogicSwitch>();
		if (component != null && this.switchedOn != component.switchedOn)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateVisualization();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x001255D4 File Offset: 0x001237D4
	protected override void Toggle()
	{
		base.Toggle();
		this.UpdateVisualization();
		this.UpdateLogicCircuit();
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x001255E8 File Offset: 0x001237E8
	private void UpdateVisualization()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.wasOn != this.switchedOn)
		{
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x0012566A File Offset: 0x0012386A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x00125688 File Offset: 0x00123888
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusActive : Db.Get().BuildingStatusItems.LogicSwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x001256DB File Offset: 0x001238DB
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x0012570F File Offset: 0x0012390F
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x00125725 File Offset: 0x00123925
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x00125734 File Offset: 0x00123934
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x0012573C File Offset: 0x0012393C
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x00125744 File Offset: 0x00123944
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06003363 RID: 13155 RVA: 0x0012574C File Offset: 0x0012394C
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06003364 RID: 13156 RVA: 0x00125753 File Offset: 0x00123953
	// (set) Token: 0x06003365 RID: 13157 RVA: 0x0012575B File Offset: 0x0012395B
	public bool ToggleRequested { get; set; }

	// Token: 0x04001F18 RID: 7960
	public static readonly HashedString PORT_ID = "LogicSwitch";

	// Token: 0x04001F19 RID: 7961
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F1A RID: 7962
	private static readonly EventSystem.IntraObjectHandler<LogicSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicSwitch>(delegate(LogicSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001F1B RID: 7963
	private bool wasOn;

	// Token: 0x04001F1C RID: 7964
	private System.Action firstFrameCallback;
}
