using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000732 RID: 1842
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06002E44 RID: 11844
	public abstract float CurrentValue { get; }

	// Token: 0x06002E45 RID: 11845 RVA: 0x0010C0B3 File Offset: 0x0010A2B3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x0010C0CC File Offset: 0x0010A2CC
	private void OnCopySettings(object data)
	{
		ConduitThresholdSensor component = ((GameObject)data).GetComponent<ConduitThresholdSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x0010C108 File Offset: 0x0010A308
	protected override void ConduitUpdate(float dt)
	{
		if (this.GetContainedMass() <= 0f && !this.dirty)
		{
			return;
		}
		float currentValue = this.CurrentValue;
		this.dirty = false;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x0010C194 File Offset: 0x0010A394
	private float GetContainedMass()
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			return Conduit.GetFlowManager(this.conduitType).GetContents(cell).mass;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents.pickupableHandle);
		if (pickupable != null)
		{
			return pickupable.PrimaryElement.Mass;
		}
		return 0f;
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06002E49 RID: 11849 RVA: 0x0010C207 File Offset: 0x0010A407
	// (set) Token: 0x06002E4A RID: 11850 RVA: 0x0010C20F File Offset: 0x0010A40F
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06002E4B RID: 11851 RVA: 0x0010C21F File Offset: 0x0010A41F
	// (set) Token: 0x06002E4C RID: 11852 RVA: 0x0010C227 File Offset: 0x0010A427
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x04001B7C RID: 7036
	[SerializeField]
	[Serialize]
	protected float threshold;

	// Token: 0x04001B7D RID: 7037
	[SerializeField]
	[Serialize]
	protected bool activateAboveThreshold = true;

	// Token: 0x04001B7E RID: 7038
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001B7F RID: 7039
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B80 RID: 7040
	private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>(delegate(ConduitThresholdSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
