using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B8D RID: 2957
[AddComponentMenu("KMonoBehaviour/scripts/LaunchConditionManager")]
public class LaunchConditionManager : KMonoBehaviour, ISim4000ms, ISim1000ms
{
	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06005841 RID: 22593 RVA: 0x00200B5C File Offset: 0x001FED5C
	// (set) Token: 0x06005842 RID: 22594 RVA: 0x00200B64 File Offset: 0x001FED64
	public List<RocketModule> rocketModules { get; private set; }

	// Token: 0x06005843 RID: 22595 RVA: 0x00200B6D File Offset: 0x001FED6D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketModules = new List<RocketModule>();
	}

	// Token: 0x06005844 RID: 22596 RVA: 0x00200B80 File Offset: 0x001FED80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.launchable = base.GetComponent<ILaunchableRocket>();
		this.FindModules();
		base.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged = delegate(object data)
		{
			this.FindModules();
		};
	}

	// Token: 0x06005845 RID: 22597 RVA: 0x00200BB1 File Offset: 0x001FEDB1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005846 RID: 22598 RVA: 0x00200BBC File Offset: 0x001FEDBC
	public void Sim1000ms(float dt)
	{
		Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
		if (spacecraftFromLaunchConditionManager == null)
		{
			return;
		}
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
		if (base.gameObject.GetComponent<LogicPorts>().GetInputValue(this.triggerPort) == 1 && spacecraftDestination != null && spacecraftDestination.id != -1)
		{
			this.Launch(spacecraftDestination);
		}
	}

	// Token: 0x06005847 RID: 22599 RVA: 0x00200C24 File Offset: 0x001FEE24
	public void FindModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null && component.conditionManager == null)
			{
				component.conditionManager = this;
				component.RegisterWithConditionManager();
			}
		}
	}

	// Token: 0x06005848 RID: 22600 RVA: 0x00200CA0 File Offset: 0x001FEEA0
	public void RegisterRocketModule(RocketModule module)
	{
		if (!this.rocketModules.Contains(module))
		{
			this.rocketModules.Add(module);
		}
	}

	// Token: 0x06005849 RID: 22601 RVA: 0x00200CBC File Offset: 0x001FEEBC
	public void UnregisterRocketModule(RocketModule module)
	{
		this.rocketModules.Remove(module);
	}

	// Token: 0x0600584A RID: 22602 RVA: 0x00200CCC File Offset: 0x001FEECC
	public List<ProcessCondition> GetLaunchConditionList()
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep, list);
			rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage, list);
		}
		return list;
	}

	// Token: 0x0600584B RID: 22603 RVA: 0x00200D30 File Offset: 0x001FEF30
	public void Launch(SpaceDestination destination)
	{
		if (destination == null)
		{
			global::Debug.LogError("Null destination passed to launch");
		}
		if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).state != Spacecraft.MissionState.Grounded)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode || (this.CheckReadyToLaunch() && this.CheckAbleToFly()))
		{
			this.launchable.LaunchableGameObject.Trigger(705820818, null);
			SpacecraftManager.instance.SetSpacecraftDestination(this, destination);
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).BeginMission(destination);
		}
	}

	// Token: 0x0600584C RID: 22604 RVA: 0x00200DA8 File Offset: 0x001FEFA8
	public bool CheckReadyToLaunch()
	{
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			foreach (RocketModule rocketModule in this.rocketModules)
			{
				rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep, list);
				using (List<ProcessCondition>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
						{
							return false;
						}
					}
				}
				list.Clear();
				rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage, list);
				using (List<ProcessCondition>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
						{
							return false;
						}
					}
				}
				list.Clear();
			}
		}
		return true;
	}

	// Token: 0x0600584D RID: 22605 RVA: 0x00200ECC File Offset: 0x001FF0CC
	public bool CheckAbleToFly()
	{
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			foreach (RocketModule rocketModule in this.rocketModules)
			{
				rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight, list);
				using (List<ProcessCondition>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
						{
							return false;
						}
					}
				}
				list.Clear();
			}
		}
		return true;
	}

	// Token: 0x0600584E RID: 22606 RVA: 0x00200F94 File Offset: 0x001FF194
	private void ClearFlightStatuses()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		foreach (KeyValuePair<ProcessCondition, Guid> keyValuePair in this.conditionStatuses)
		{
			component.RemoveStatusItem(keyValuePair.Value, false);
		}
		this.conditionStatuses.Clear();
	}

	// Token: 0x0600584F RID: 22607 RVA: 0x00201004 File Offset: 0x001FF204
	public void Sim4000ms(float dt)
	{
		bool flag = this.CheckReadyToLaunch();
		LogicPorts component = base.gameObject.GetComponent<LogicPorts>();
		if (flag)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
			if (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Grounded || spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Launching)
			{
				component.SendSignal(this.statusPort, 1);
			}
			else
			{
				component.SendSignal(this.statusPort, 0);
			}
			KSelectable component2 = base.GetComponent<KSelectable>();
			List<ProcessCondition> list;
			using (ProcessCondition.ListPool.Get(out list))
			{
				using (List<RocketModule>.Enumerator enumerator = this.rocketModules.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RocketModule rocketModule = enumerator.Current;
						rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight, list);
						foreach (ProcessCondition processCondition in list)
						{
							if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
							{
								if (!this.conditionStatuses.ContainsKey(processCondition))
								{
									StatusItem statusItem = processCondition.GetStatusItem(ProcessCondition.Status.Failure);
									this.conditionStatuses[processCondition] = component2.AddStatusItem(statusItem, processCondition);
								}
							}
							else if (this.conditionStatuses.ContainsKey(processCondition))
							{
								component2.RemoveStatusItem(this.conditionStatuses[processCondition], false);
								this.conditionStatuses.Remove(processCondition);
							}
						}
						list.Clear();
					}
					return;
				}
			}
		}
		this.ClearFlightStatuses();
		component.SendSignal(this.statusPort, 0);
	}

	// Token: 0x04003B49 RID: 15177
	public HashedString triggerPort;

	// Token: 0x04003B4A RID: 15178
	public HashedString statusPort;

	// Token: 0x04003B4C RID: 15180
	private ILaunchableRocket launchable;

	// Token: 0x04003B4D RID: 15181
	private Dictionary<ProcessCondition, Guid> conditionStatuses = new Dictionary<ProcessCondition, Guid>();

	// Token: 0x02001D14 RID: 7444
	public enum ConditionType
	{
		// Token: 0x04008A42 RID: 35394
		Launch,
		// Token: 0x04008A43 RID: 35395
		Flight
	}
}
