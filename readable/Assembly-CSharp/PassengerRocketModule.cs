using System;
using System.Collections.Generic;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02000B98 RID: 2968
public class PassengerRocketModule : KMonoBehaviour
{
	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x060058A6 RID: 22694 RVA: 0x0020293B File Offset: 0x00200B3B
	public PassengerRocketModule.RequestCrewState PassengersRequested
	{
		get
		{
			return this.passengersRequested;
		}
	}

	// Token: 0x060058A7 RID: 22695 RVA: 0x00202944 File Offset: 0x00200B44
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		GameUtil.SubscribeToTags<PassengerRocketModule>(this, PassengerRocketModule.OnRocketOnGroundTagDelegate, false);
		base.Subscribe<PassengerRocketModule>(-1547247383, PassengerRocketModule.OnClustercraftStateChanged);
		base.Subscribe<PassengerRocketModule>(1655598572, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(191901966, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-71801987, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-1277991738, PassengerRocketModule.OnLaunchDelegate);
		base.Subscribe<PassengerRocketModule>(-1432940121, PassengerRocketModule.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(base.GetComponent<Workable>()).StartSM();
	}

	// Token: 0x060058A8 RID: 22696 RVA: 0x002029F5 File Offset: 0x00200BF5
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		base.OnCleanUp();
	}

	// Token: 0x060058A9 RID: 22697 RVA: 0x00202A18 File Offset: 0x00200C18
	private void OnAssignmentGroupChanged(object data)
	{
		this.RefreshOrders();
	}

	// Token: 0x060058AA RID: 22698 RVA: 0x00202A20 File Offset: 0x00200C20
	private void RefreshClusterStateForAudio()
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			if (activeWorld != null && activeWorld.IsModuleInterior)
			{
				UnityEngine.Object craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
				Clustercraft component = activeWorld.GetComponent<Clustercraft>();
				if (craftInterface == component.ModuleInterface)
				{
					ClusterManager.Instance.UpdateRocketInteriorAudio();
				}
			}
		}
	}

	// Token: 0x060058AB RID: 22699 RVA: 0x00202A80 File Offset: 0x00200C80
	private void OnReachableChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		KSelectable component = base.GetComponent<KSelectable>();
		if (value)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, this);
	}

	// Token: 0x060058AC RID: 22700 RVA: 0x00202AD0 File Offset: 0x00200CD0
	public void RequestCrewBoard(PassengerRocketModule.RequestCrewState requestBoard)
	{
		this.passengersRequested = requestBoard;
		this.RefreshOrders();
	}

	// Token: 0x060058AD RID: 22701 RVA: 0x00202AE0 File Offset: 0x00200CE0
	public bool ShouldCrewGetIn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		return this.passengersRequested == PassengerRocketModule.RequestCrewState.Request || (craftInterface.IsLaunchRequested() && craftInterface.CheckPreppedForLaunch());
	}

	// Token: 0x060058AE RID: 22702 RVA: 0x00202B14 File Offset: 0x00200D14
	private void RefreshOrders()
	{
		if (!this.HasTag(GameTags.RocketOnGround) || !base.GetComponent<ClustercraftExteriorDoor>().HasTargetWorld())
		{
			return;
		}
		int cell = base.GetComponent<NavTeleporter>().GetCell();
		int num = base.GetComponent<ClustercraftExteriorDoor>().TargetCell();
		bool flag = this.ShouldCrewGetIn();
		if (flag)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity minionIdentity = enumerator.Current;
					bool flag2 = Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minionIdentity.assignableProxy.Get());
					bool flag3 = minionIdentity.GetMyWorldId() == (int)Grid.WorldIdx[num];
					RocketPassengerMonitor.Instance smi = minionIdentity.GetSMI<RocketPassengerMonitor.Instance>();
					if (smi != null)
					{
						if (!flag3 && flag2)
						{
							smi.SetMoveTarget(num);
						}
						else if (flag3 && !flag2)
						{
							smi.SetMoveTarget(cell);
						}
						else
						{
							smi.ClearMoveTarget(num);
						}
					}
				}
				goto IL_146;
			}
		}
		foreach (MinionIdentity cmp in Components.LiveMinionIdentities.Items)
		{
			RocketPassengerMonitor.Instance smi2 = cmp.GetSMI<RocketPassengerMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.ClearMoveTarget(cell);
				smi2.ClearMoveTarget(num);
			}
		}
		IL_146:
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			this.RefreshAccessStatus(Components.LiveMinionIdentities[i], flag);
		}
	}

	// Token: 0x060058AF RID: 22703 RVA: 0x00202CB0 File Offset: 0x00200EB0
	private void RefreshAccessStatus(MinionIdentity minion, bool restrict)
	{
		Component interiorDoor = base.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		AccessControl component = base.GetComponent<AccessControl>();
		AccessControl component2 = interiorDoor.GetComponent<AccessControl>();
		if (!restrict)
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			return;
		}
		if (Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minion.assignableProxy.Get()))
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
			return;
		}
		component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
		component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
	}

	// Token: 0x060058B0 RID: 22704 RVA: 0x00202D78 File Offset: 0x00200F78
	public bool CheckPilotBoarded()
	{
		GameObject dupePilot = this.GetDupePilot();
		return dupePilot != null && dupePilot.GetMyWorldId() == (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()];
	}

	// Token: 0x060058B1 RID: 22705 RVA: 0x00202DB0 File Offset: 0x00200FB0
	public GameObject GetDupePilot()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return null;
		}
		List<IAssignableIdentity> list = new List<IAssignableIdentity>();
		foreach (IAssignableIdentity assignableIdentity in members)
		{
			MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
			if (minionAssignablesProxy != null)
			{
				MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					list.Add(assignableIdentity);
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		foreach (IAssignableIdentity assignableIdentity2 in list)
		{
			GameObject targetGameObject = ((MinionAssignablesProxy)assignableIdentity2).GetTargetGameObject();
			if (targetGameObject.GetMyWorldId() == (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
			{
				return targetGameObject;
			}
		}
		return ((MinionAssignablesProxy)list[0]).GetTargetGameObject();
	}

	// Token: 0x060058B2 RID: 22706 RVA: 0x00202EDC File Offset: 0x002010DC
	public global::Tuple<int, int> GetCrewBoardedFraction()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return new global::Tuple<int, int>(0, 0);
		}
		int num = 0;
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					num++;
				}
			}
		}
		return new global::Tuple<int, int>(members.Count - num, members.Count);
	}

	// Token: 0x060058B3 RID: 22707 RVA: 0x00202F74 File Offset: 0x00201174
	public bool HasCrewAssigned()
	{
		return ((ICollection<IAssignableIdentity>)base.GetComponent<AssignmentGroupController>().GetMembers()).Count > 0;
	}

	// Token: 0x060058B4 RID: 22708 RVA: 0x00202F8C File Offset: 0x0020118C
	public int GetCrewCount()
	{
		return ((ICollection<IAssignableIdentity>)base.GetComponent<AssignmentGroupController>().GetMembers()).Count;
	}

	// Token: 0x060058B5 RID: 22709 RVA: 0x00202FA0 File Offset: 0x002011A0
	public bool CheckPassengersBoarded(bool require_pilot = true)
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		if (require_pilot)
		{
			bool flag = false;
			foreach (IAssignableIdentity assignableIdentity in members)
			{
				MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
				if (minionAssignablesProxy != null)
				{
					MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
					if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060058B6 RID: 22710 RVA: 0x0020309C File Offset: 0x0020129C
	public bool CheckExtraPassengers()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			byte worldId = Grid.WorldIdx[component.TargetCell()];
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems((int)worldId, false);
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(worldItems[i].assignableProxy.Get()))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060058B7 RID: 22711 RVA: 0x00203124 File Offset: 0x00201324
	public void RemoveRocketPassenger(MinionIdentity minion)
	{
		if (minion != null)
		{
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			MinionAssignablesProxy member = minion.assignableProxy.Get();
			if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member))
			{
				Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
			}
			this.RefreshOrders();
		}
	}

	// Token: 0x060058B8 RID: 22712 RVA: 0x00203190 File Offset: 0x00201390
	public void RemovePassengersOnOtherWorlds()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			int myWorldId = component.GetMyWorldId();
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				MinionAssignablesProxy member = minionIdentity.assignableProxy.Get();
				if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member) && minionIdentity.GetMyParentWorldId() != myWorldId)
				{
					Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
				}
			}
		}
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x00203258 File Offset: 0x00201458
	public void ClearMinionAssignments(object data)
	{
		string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
		foreach (IAssignableIdentity minionIdentity in Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].GetMembers())
		{
			Game.Instance.assignmentManager.RemoveFromWorld(minionIdentity, this.GetMyWorldId());
		}
	}

	// Token: 0x04003B7D RID: 15229
	public EventReference interiorReverbSnapshot;

	// Token: 0x04003B7E RID: 15230
	[Serialize]
	private PassengerRocketModule.RequestCrewState passengersRequested;

	// Token: 0x04003B7F RID: 15231
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<PassengerRocketModule>(GameTags.RocketOnGround, delegate(PassengerRocketModule component, object data)
	{
		component.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
	});

	// Token: 0x04003B80 RID: 15232
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnClustercraftStateChanged = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshClusterStateForAudio();
	});

	// Token: 0x04003B81 RID: 15233
	private static EventSystem.IntraObjectHandler<PassengerRocketModule> RefreshDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshOrders();
		cmp.RefreshClusterStateForAudio();
	});

	// Token: 0x04003B82 RID: 15234
	private static EventSystem.IntraObjectHandler<PassengerRocketModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.ClearMinionAssignments(data);
	});

	// Token: 0x04003B83 RID: 15235
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x02001D23 RID: 7459
	public enum RequestCrewState
	{
		// Token: 0x04008A71 RID: 35441
		Release,
		// Token: 0x04008A72 RID: 35442
		Request
	}
}
