using System;

// Token: 0x02000B9C RID: 2972
public class RoboPilotModule : KMonoBehaviour
{
	// Token: 0x060058CA RID: 22730 RVA: 0x002036C4 File Offset: 0x002018C4
	protected override void OnSpawn()
	{
		this.databankStorage = base.GetComponent<Storage>();
		this.manualDeliveryChore = base.GetComponent<ManualDeliveryKG>();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.UpdateMeter(null);
		this.databankStorage.SetOffsets(RoboPilotModule.dataDeliveryOffsets);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
		base.Subscribe(-778359855, new Action<object>(this.PlayDeliveryAnimation));
		base.Subscribe(-887025858, new Action<object>(this.OnRocketLanded));
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			component.CraftInterface.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
			component.CraftInterface.Subscribe(543433792, new Action<object>(this.RequestDataBanksForDestination));
		}
		else
		{
			base.Subscribe(705820818, new Action<object>(this.OnRocketLaunched));
			base.GetComponent<RocketModule>().FindLaunchConditionManager().Subscribe(929158128, new Action<object>(this.RequestDataBanksForDestination));
		}
		this.RequestDataBanksForDestination(null);
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x00203828 File Offset: 0x00201A28
	private void RequestDataBanksForDestination(object _ = null)
	{
		int num = -1;
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			ClusterTraveler component2 = component.CraftInterface.GetComponent<ClusterTraveler>();
			if (component2 != null && component2.CurrentPath != null)
			{
				num = component2.RemainingTravelNodes() * 2;
			}
		}
		else
		{
			LaunchConditionManager launchConditionManager = base.GetComponent<RocketModule>().FindLaunchConditionManager();
			if (launchConditionManager != null)
			{
				SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(launchConditionManager);
				if (spacecraftDestination != null)
				{
					num = spacecraftDestination.OneBasedDistance * 2;
				}
			}
		}
		if (num > 0 && !this.HasResourcesToMove(num))
		{
			this.manualDeliveryChore.refillMass = MathF.Min(this.ResourcesRequiredToMove(num), this.databankStorage.Capacity() - this.databankStorage.UnitsStored());
		}
	}

	// Token: 0x060058CC RID: 22732 RVA: 0x002038DC File Offset: 0x00201ADC
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1697596308, new Action<object>(this.UpdateMeter));
		base.Unsubscribe(-887025858, new Action<object>(this.OnRocketLanded));
		base.Unsubscribe(-778359855, new Action<object>(this.PlayDeliveryAnimation));
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			component.CraftInterface.Unsubscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
			component.CraftInterface.Unsubscribe(543433792, new Action<object>(this.RequestDataBanksForDestination));
		}
		else
		{
			base.Unsubscribe(705820818, new Action<object>(this.OnRocketLaunched));
			base.GetComponent<RocketModule>().FindLaunchConditionManager().Unsubscribe(929158128, new Action<object>(this.RequestDataBanksForDestination));
		}
		base.OnCleanUp();
	}

	// Token: 0x060058CD RID: 22733 RVA: 0x002039B8 File Offset: 0x00201BB8
	private void OnLaunchConditionChanged(object data)
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null && component.CraftInterface.IsLaunchRequested())
		{
			component.CraftInterface.GetComponent<Clustercraft>().Launch(false);
		}
	}

	// Token: 0x060058CE RID: 22734 RVA: 0x002039F4 File Offset: 0x00201BF4
	private void OnRocketLanded(object o)
	{
		if (this.consumeDataBanksOnLand)
		{
			LaunchConditionManager lcm = base.GetComponent<RocketModule>().FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(lcm);
			float amount = Math.Min((float)(SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id).OneBasedDistance * this.dataBankConsumption * 2), this.databankStorage.MassStored());
			this.databankStorage.ConsumeIgnoringDisease(DatabankHelper.TAG, amount);
		}
		this.RequestDataBanksForDestination(null);
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x00203A6C File Offset: 0x00201C6C
	private void OnRocketLaunched(object o)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play("launch_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("launch", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("launch_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060058D0 RID: 22736 RVA: 0x00203ACF File Offset: 0x00201CCF
	public void ConsumeDataBanksInFlight()
	{
		if (this.databankStorage != null)
		{
			this.databankStorage.ConsumeIgnoringDisease(DatabankHelper.TAG, (float)this.dataBankConsumption);
		}
	}

	// Token: 0x060058D1 RID: 22737 RVA: 0x00203AF8 File Offset: 0x00201CF8
	private void PlayDeliveryAnimation(object data = null)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		HashedString currentAnim = component.currentAnim;
		component.Play("databank_delivery_reaction", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue(currentAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060058D2 RID: 22738 RVA: 0x00203B3E File Offset: 0x00201D3E
	private void UpdateMeter(object data = null)
	{
		this.meter.SetPositionPercent(this.databankStorage.MassStored() / this.databankStorage.Capacity());
	}

	// Token: 0x060058D3 RID: 22739 RVA: 0x00203B62 File Offset: 0x00201D62
	public bool HasResourcesToMove(int distance)
	{
		return this.databankStorage.UnitsStored() >= (float)(distance * this.dataBankConsumption);
	}

	// Token: 0x060058D4 RID: 22740 RVA: 0x00203B7D File Offset: 0x00201D7D
	public float ResourcesRequiredToMove(int distance)
	{
		return (float)(distance * this.dataBankConsumption);
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x00203B88 File Offset: 0x00201D88
	public bool IsFull()
	{
		return this.databankStorage.MassStored() >= this.databankStorage.Capacity();
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x00203BA5 File Offset: 0x00201DA5
	public float GetDataBanksStored()
	{
		if (!(this.databankStorage != null))
		{
			return 0f;
		}
		return this.databankStorage.UnitsStored();
	}

	// Token: 0x060058D7 RID: 22743 RVA: 0x00203BC8 File Offset: 0x00201DC8
	public float GetDataBankRange()
	{
		if (this.databankStorage == null)
		{
			return 0f;
		}
		if (this.consumeDataBanksOnLand)
		{
			return this.databankStorage.UnitsStored() / (float)this.dataBankConsumption * RoboPilotCommandModuleConfig.DATABANKRANGE;
		}
		return this.databankStorage.UnitsStored() / (float)this.dataBankConsumption * 600f;
	}

	// Token: 0x060058D8 RID: 22744 RVA: 0x00203C28 File Offset: 0x00201E28
	public float GetMaxDataBankRange()
	{
		if (this.databankStorage == null)
		{
			return 0f;
		}
		if (this.consumeDataBanksOnLand)
		{
			return this.databankStorage.Capacity() / (float)this.dataBankConsumption * RoboPilotCommandModuleConfig.DATABANKRANGE;
		}
		return this.databankStorage.Capacity() / (float)this.dataBankConsumption * 600f;
	}

	// Token: 0x04003B90 RID: 15248
	private MeterController meter;

	// Token: 0x04003B91 RID: 15249
	private Storage databankStorage;

	// Token: 0x04003B92 RID: 15250
	private ManualDeliveryKG manualDeliveryChore;

	// Token: 0x04003B93 RID: 15251
	public int dataBankConsumption = 2;

	// Token: 0x04003B94 RID: 15252
	public bool consumeDataBanksOnLand;

	// Token: 0x04003B95 RID: 15253
	private static CellOffset[] dataDeliveryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(2, 0),
		new CellOffset(3, 0),
		new CellOffset(-1, 0),
		new CellOffset(-2, 0),
		new CellOffset(-3, 0)
	};
}
