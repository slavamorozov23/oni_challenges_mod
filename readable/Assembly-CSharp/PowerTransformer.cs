using System;
using System.Diagnostics;

// Token: 0x020007E0 RID: 2016
[DebuggerDisplay("{name}")]
public class PowerTransformer : Generator
{
	// Token: 0x06003594 RID: 13716 RVA: 0x0012EBAC File Offset: 0x0012CDAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.battery = base.GetComponent<Battery>();
		base.Subscribe<PowerTransformer>(-592767678, PowerTransformer.OnOperationalChangedDelegate);
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x0012EBD7 File Offset: 0x0012CDD7
	public override void ApplyDeltaJoules(float joules_delta, bool can_over_power = false)
	{
		this.battery.ConsumeEnergy(-joules_delta);
		base.ApplyDeltaJoules(joules_delta, can_over_power);
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x0012EBEE File Offset: 0x0012CDEE
	public override void ConsumeEnergy(float joules)
	{
		this.battery.ConsumeEnergy(joules);
		base.ConsumeEnergy(joules);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x0012EC03 File Offset: 0x0012CE03
	private void OnOperationalChanged(object _)
	{
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x0012EC0B File Offset: 0x0012CE0B
	private void UpdateJoulesLostPerSecond()
	{
		if (this.operational.IsOperational)
		{
			this.battery.joulesLostPerSecond = 0f;
			return;
		}
		this.battery.joulesLostPerSecond = 3.3333333f;
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x0012EC3C File Offset: 0x0012CE3C
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		float joulesAvailable = this.operational.IsOperational ? Math.Min(this.battery.JoulesAvailable, base.WattageRating * dt) : 0f;
		base.AssignJoulesAvailable(joulesAvailable);
		ushort circuitID = this.battery.CircuitID;
		ushort circuitID2 = base.CircuitID;
		bool flag = circuitID == circuitID2 && circuitID != ushort.MaxValue;
		if (this.mLoopDetected != flag)
		{
			this.mLoopDetected = flag;
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.PowerLoopDetected, this.mLoopDetected, this);
		}
	}

	// Token: 0x04002079 RID: 8313
	private Battery battery;

	// Token: 0x0400207A RID: 8314
	private bool mLoopDetected;

	// Token: 0x0400207B RID: 8315
	private static readonly EventSystem.IntraObjectHandler<PowerTransformer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PowerTransformer>(delegate(PowerTransformer component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
