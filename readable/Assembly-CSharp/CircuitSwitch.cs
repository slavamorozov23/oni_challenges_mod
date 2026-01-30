using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200071D RID: 1821
[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06002D76 RID: 11638 RVA: 0x001076E4 File Offset: 0x001058E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x00107700 File Offset: 0x00105900
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.CircuitOnToggle;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		Wire wire = (gameObject != null) ? gameObject.GetComponent<Wire>() : null;
		if (wire == null)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
		this.AttachWire(wire);
		this.wasOn = this.switchedOn;
		this.UpdateCircuit(true);
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x001077D0 File Offset: 0x001059D0
	protected override void OnCleanUp()
	{
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		bool switchedOn = this.switchedOn;
		this.switchedOn = true;
		this.UpdateCircuit(false);
		this.switchedOn = switchedOn;
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x00107814 File Offset: 0x00105A14
	private void OnCopySettings(object data)
	{
		CircuitSwitch component = ((GameObject)data).GetComponent<CircuitSwitch>();
		if (component != null)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateCircuit(true);
		}
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x0010784C File Offset: 0x00105A4C
	public bool IsConnected()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<IDisconnectable>() != null;
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x00107890 File Offset: 0x00105A90
	private void CircuitOnToggle(bool on)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x0010789C File Offset: 0x00105A9C
	public void AttachWire(Wire wire)
	{
		if (wire == this.attachedWire)
		{
			return;
		}
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		this.attachedWire = wire;
		if (this.attachedWire != null)
		{
			this.SubscribeToWire(this.attachedWire);
			this.UpdateCircuit(true);
			this.wireConnectedGUID = base.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID, false);
			return;
		}
		if (this.wireConnectedGUID == Guid.Empty)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x00107946 File Offset: 0x00105B46
	private void OnWireDestroyed(object data)
	{
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x00107962 File Offset: 0x00105B62
	private void OnWireStateChanged(object data)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x0010796C File Offset: 0x00105B6C
	private void SubscribeToWire(Wire wire)
	{
		this.objectDestroyedHandle = wire.Subscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		this.buildingFullyRepairedHandle = wire.Subscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		this.buildingBrokenHandle = wire.Subscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x001079D0 File Offset: 0x00105BD0
	private void UnsubscribeFromWire(Wire wire)
	{
		wire.Unsubscribe(ref this.objectDestroyedHandle);
		wire.Unsubscribe(ref this.buildingFullyRepairedHandle);
		wire.Unsubscribe(ref this.buildingBrokenHandle);
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x001079F8 File Offset: 0x00105BF8
	private void UpdateCircuit(bool should_update_anim = true)
	{
		if (this.attachedWire != null)
		{
			if (this.switchedOn)
			{
				this.attachedWire.Connect();
			}
			else
			{
				this.attachedWire.Disconnect();
			}
		}
		if (should_update_anim && this.wasOn != this.switchedOn)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x00107ABF File Offset: 0x00105CBF
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x00107AF3 File Offset: 0x00105CF3
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x00107AFB File Offset: 0x00105CFB
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x00107B03 File Offset: 0x00105D03
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06002D86 RID: 11654 RVA: 0x00107B0B File Offset: 0x00105D0B
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06002D87 RID: 11655 RVA: 0x00107B12 File Offset: 0x00105D12
	// (set) Token: 0x06002D88 RID: 11656 RVA: 0x00107B1A File Offset: 0x00105D1A
	public bool ToggleRequested { get; set; }

	// Token: 0x04001B07 RID: 6919
	[SerializeField]
	public ObjectLayer objectLayer;

	// Token: 0x04001B08 RID: 6920
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B09 RID: 6921
	private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>(delegate(CircuitSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001B0A RID: 6922
	private Wire attachedWire;

	// Token: 0x04001B0B RID: 6923
	private Guid wireConnectedGUID;

	// Token: 0x04001B0C RID: 6924
	private bool wasOn;

	// Token: 0x04001B0D RID: 6925
	private int objectDestroyedHandle = -1;

	// Token: 0x04001B0E RID: 6926
	private int buildingFullyRepairedHandle = -1;

	// Token: 0x04001B0F RID: 6927
	private int buildingBrokenHandle = -1;
}
