using System;
using KSerialization;

// Token: 0x020007D2 RID: 2002
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
	// Token: 0x06003529 RID: 13609 RVA: 0x0012CBB1 File Offset: 0x0012ADB1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.manuallyControlled = false;
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x0012CBC0 File Offset: 0x0012ADC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x0012CBDC File Offset: 0x0012ADDC
	private void OnOperationalChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		this.SetState(value);
	}

	// Token: 0x0400202E RID: 8238
	private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>(delegate(OperationalControlledSwitch component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
