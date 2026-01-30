using System;
using KSerialization;

// Token: 0x020007D3 RID: 2003
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
	// Token: 0x0600352E RID: 13614 RVA: 0x0012CC20 File Offset: 0x0012AE20
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x0012CC39 File Offset: 0x0012AE39
	protected override void OnSpawn()
	{
		this.OnOperationalChanged(this.operational.IsOperational);
		base.OnSpawn();
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x0012CC52 File Offset: 0x0012AE52
	protected override void OnCleanUp()
	{
		base.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x0012CC6B File Offset: 0x0012AE6B
	private void OnOperationalChanged(object data)
	{
		this.OnOperationalChanged(((Boxed<bool>)data).value);
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x0012CC7E File Offset: 0x0012AE7E
	private void OnOperationalChanged(bool isOperational)
	{
		if (isOperational)
		{
			base.CurrentFlow = base.MaxFlow;
		}
		else
		{
			base.CurrentFlow = 0f;
		}
		this.operational.SetActive(isOperational, false);
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x0012CCA9 File Offset: 0x0012AEA9
	protected override void OnMassTransfer(float amount)
	{
		this.isDispensing = (amount > 0f);
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x0012CCBC File Offset: 0x0012AEBC
	public override void UpdateAnim()
	{
		if (!this.operational.IsOperational)
		{
			this.controller.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.isDispensing)
		{
			this.controller.Queue("on_flow", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.controller.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0400202F RID: 8239
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002030 RID: 8240
	private bool isDispensing;

	// Token: 0x04002031 RID: 8241
	private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>(delegate(OperationalValve component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
