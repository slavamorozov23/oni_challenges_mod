using System;
using UnityEngine;

// Token: 0x02000BA4 RID: 2980
public class RocketModuleCluster : RocketModule
{
	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06005911 RID: 22801 RVA: 0x00205373 File Offset: 0x00203573
	// (set) Token: 0x06005912 RID: 22802 RVA: 0x0020537B File Offset: 0x0020357B
	public CraftModuleInterface CraftInterface
	{
		get
		{
			return this._craftInterface;
		}
		set
		{
			this._craftInterface = value;
			if (this._craftInterface != null)
			{
				base.name = base.name + ": " + this.GetParentRocketName();
			}
		}
	}

	// Token: 0x06005913 RID: 22803 RVA: 0x002053AE File Offset: 0x002035AE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketModuleCluster>(2121280625, RocketModuleCluster.OnNewConstructionDelegate);
	}

	// Token: 0x06005914 RID: 22804 RVA: 0x002053C8 File Offset: 0x002035C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.CraftInterface == null && DlcManager.FeatureClusterSpaceEnabled())
		{
			this.RegisterWithCraftModuleInterface();
		}
		if (base.GetComponent<RocketEngine>() == null && base.GetComponent<RocketEngineCluster>() == null && base.GetComponent<BuildingUnderConstruction>() == null)
		{
			base.Subscribe<RocketModuleCluster>(1655598572, RocketModuleCluster.OnLaunchConditionChangedDelegate);
			base.Subscribe<RocketModuleCluster>(-887025858, RocketModuleCluster.OnLandDelegate);
		}
	}

	// Token: 0x06005915 RID: 22805 RVA: 0x00205444 File Offset: 0x00203644
	protected void OnNewConstruction(object data)
	{
		Constructable constructable = (Constructable)data;
		if (constructable == null)
		{
			return;
		}
		RocketModuleCluster component = constructable.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		if (component.CraftInterface != null)
		{
			component.CraftInterface.AddModule(this);
		}
	}

	// Token: 0x06005916 RID: 22806 RVA: 0x00205490 File Offset: 0x00203690
	private void RegisterWithCraftModuleInterface()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			if (!(gameObject == base.gameObject))
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				if (component != null)
				{
					component.CraftInterface.AddModule(this);
					break;
				}
			}
		}
	}

	// Token: 0x06005917 RID: 22807 RVA: 0x00205510 File Offset: 0x00203710
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.CraftInterface.RemoveModule(this);
	}

	// Token: 0x06005918 RID: 22808 RVA: 0x00205524 File Offset: 0x00203724
	public override LaunchConditionManager FindLaunchConditionManager()
	{
		return this.CraftInterface.FindLaunchConditionManager();
	}

	// Token: 0x06005919 RID: 22809 RVA: 0x00205531 File Offset: 0x00203731
	public override string GetParentRocketName()
	{
		if (this.CraftInterface != null)
		{
			return this.CraftInterface.GetComponent<Clustercraft>().Name;
		}
		return this.parentRocketName;
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x00205558 File Offset: 0x00203758
	private void OnLaunchConditionChanged(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x0600591B RID: 22811 RVA: 0x00205560 File Offset: 0x00203760
	private void OnLand(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x0600591C RID: 22812 RVA: 0x00205568 File Offset: 0x00203768
	protected void UpdateAnimations()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Clustercraft clustercraft = (this.CraftInterface == null) ? null : this.CraftInterface.GetComponent<Clustercraft>();
		if (clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Launching && component.HasAnimation("launch"))
		{
			component.ClearQueue();
			if (component.HasAnimation("launch_pre"))
			{
				component.Play("launch_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
			component.Queue("launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (this.CraftInterface != null && this.CraftInterface.CheckPreppedForLaunch())
		{
			component.initialAnim = "ready_to_launch";
			component.Play("pre_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("ready_to_launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		component.initialAnim = "grounded";
		component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("grounded", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x04003BCF RID: 15311
	public RocketModulePerformance performanceStats;

	// Token: 0x04003BD0 RID: 15312
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnNewConstructionDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnNewConstruction(data);
	});

	// Token: 0x04003BD1 RID: 15313
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLaunchConditionChangedDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLaunchConditionChanged(data);
	});

	// Token: 0x04003BD2 RID: 15314
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLandDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x04003BD3 RID: 15315
	private CraftModuleInterface _craftInterface;
}
