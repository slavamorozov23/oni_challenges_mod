using System;
using KSerialization;

// Token: 0x02000795 RID: 1941
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicElementSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x06003200 RID: 12800 RVA: 0x0011F9FA File Offset: 0x0011DBFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Filterable>().onFilterChanged += this.OnElementSelected;
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x0011FA1C File Offset: 0x0011DC1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		base.Subscribe<LogicElementSensor>(-592767678, LogicElementSensor.OnOperationalChangedDelegate);
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x0011FA6C File Offset: 0x0011DC6C
	public void Sim200ms(float dt)
	{
		int i = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			this.samples[this.sampleIdx] = (Grid.ElementIdx[i] == this.desiredElementIdx);
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		bool flag = true;
		bool[] array = this.samples;
		for (int j = 0; j < array.Length; j++)
		{
			flag = (array[j] && flag);
		}
		if (base.IsSwitchedOn != flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x0011FAEB File Offset: 0x0011DCEB
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x0011FAFC File Offset: 0x0011DCFC
	private void UpdateLogicCircuit()
	{
		bool flag = this.switchedOn && base.GetComponent<Operational>().IsOperational;
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x0011FB38 File Offset: 0x0011DD38
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x0011FBC0 File Offset: 0x0011DDC0
	private void OnElementSelected(Tag element_tag)
	{
		if (!element_tag.IsValid)
		{
			return;
		}
		Element element = ElementLoader.GetElement(element_tag);
		bool on = true;
		if (element != null)
		{
			this.desiredElementIdx = ElementLoader.GetElementIndex(element.id);
			on = (element.id == SimHashes.Void || element.id == SimHashes.Vacuum);
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x0011FC2F File Offset: 0x0011DE2F
	private void OnOperationalChanged(object _)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06003208 RID: 12808 RVA: 0x0011FC40 File Offset: 0x0011DE40
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001E12 RID: 7698
	private bool wasOn;

	// Token: 0x04001E13 RID: 7699
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04001E14 RID: 7700
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001E15 RID: 7701
	private bool[] samples = new bool[8];

	// Token: 0x04001E16 RID: 7702
	private int sampleIdx;

	// Token: 0x04001E17 RID: 7703
	private ushort desiredElementIdx = ushort.MaxValue;

	// Token: 0x04001E18 RID: 7704
	private static readonly EventSystem.IntraObjectHandler<LogicElementSensor> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicElementSensor>(delegate(LogicElementSensor component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
