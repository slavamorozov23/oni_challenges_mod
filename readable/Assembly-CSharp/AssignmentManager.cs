using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006DF RID: 1759
[AddComponentMenu("KMonoBehaviour/scripts/AssignmentManager")]
public class AssignmentManager : KMonoBehaviour
{
	// Token: 0x06002B4E RID: 11086 RVA: 0x000FCED8 File Offset: 0x000FB0D8
	public IEnumerator<Assignable> GetEnumerator()
	{
		return this.assignables.GetEnumerator();
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000FCEEA File Offset: 0x000FB0EA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe<AssignmentManager>(586301400, AssignmentManager.MinionMigrationDelegate);
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x000FCF08 File Offset: 0x000FB108
	protected void MinionMigration(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject() == minionMigrationEventArgs.minionId.gameObject)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x000FCFB4 File Offset: 0x000FB1B4
	public void Add(Assignable assignable)
	{
		this.assignables.Add(assignable);
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x000FCFC2 File Offset: 0x000FB1C2
	public void Remove(Assignable assignable)
	{
		this.assignables.Remove(assignable);
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x000FCFD1 File Offset: 0x000FB1D1
	public AssignmentGroup TryCreateAssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		if (this.assignment_groups.ContainsKey(id))
		{
			return this.assignment_groups[id];
		}
		return new AssignmentGroup(id, members, name);
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x000FCFF6 File Offset: 0x000FB1F6
	public void RemoveAssignmentGroup(string id)
	{
		if (!this.assignment_groups.ContainsKey(id))
		{
			global::Debug.LogError("Assignment group with id " + id + " doesn't exists");
			return;
		}
		this.assignment_groups.Remove(id);
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x000FD029 File Offset: 0x000FB229
	public void AddToAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].AddMember(member);
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000FD04E File Offset: 0x000FB24E
	public void RemoveFromAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].RemoveMember(member);
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000FD074 File Offset: 0x000FB274
	public void RemoveFromAllGroups(IAssignableIdentity member)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee == member)
			{
				assignable.Unassign();
			}
		}
		foreach (KeyValuePair<string, AssignmentGroup> keyValuePair in this.assignment_groups)
		{
			if (keyValuePair.Value.HasMember(member))
			{
				keyValuePair.Value.RemoveMember(member);
			}
		}
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x000FD128 File Offset: 0x000FB328
	public void RemoveFromWorld(IAssignableIdentity minionIdentity, int world_id)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null && assignable.assignee.GetOwners().Count == 1)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee == minionIdentity && assignable.GetMyWorldId() == world_id)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x000FD1CC File Offset: 0x000FB3CC
	private int CompareAssignables(Assignable a, Assignable b)
	{
		int num = a.assignee.NumOwners();
		int num2 = b.assignee.NumOwners();
		int num3 = num.CompareTo(num2);
		if (num3 != 0)
		{
			if (num == 0)
			{
				return -1;
			}
			if (num2 == 0)
			{
				return 1;
			}
		}
		int num4 = a.priority.CompareTo(b.priority);
		if (num4 != 0)
		{
			return num4;
		}
		return num3;
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000FD220 File Offset: 0x000FB420
	public List<Assignable> GetPreferredAssignables(Assignables owner, Navigator ownerNavigator, AssignableSlot slot)
	{
		List<Assignable> preferredAssignableResults = this.PreferredAssignableResults;
		List<Assignable> preferredAssignableResults2;
		lock (preferredAssignableResults)
		{
			this.PreferredAssignableResults.Clear();
			foreach (Assignable assignable in this.assignables)
			{
				if (assignable.slot == slot && assignable.assignee != null && assignable.assignee.HasOwner(owner) && (!(ownerNavigator != null) || assignable.GetNavigationCost(ownerNavigator) != -1))
				{
					Room room = assignable.assignee as Room;
					if (room != null && room.roomType.priority_building_use)
					{
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
						return this.PreferredAssignableResults;
					}
					this.PreferredAssignableResults.Add(assignable);
				}
			}
			this.PreferredAssignableResults.Sort(new Comparison<Assignable>(this.CompareAssignables));
			preferredAssignableResults2 = this.PreferredAssignableResults;
		}
		return preferredAssignableResults2;
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x000FD348 File Offset: 0x000FB548
	public bool IsPreferredAssignable(Assignables owner, Assignable candidate)
	{
		IAssignableIdentity assignee = candidate.assignee;
		if (assignee == null || !assignee.HasOwner(owner))
		{
			return false;
		}
		int num = assignee.NumOwners();
		Room room = assignee as Room;
		if (room != null && room.roomType.priority_building_use)
		{
			return true;
		}
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.slot == candidate.slot && assignable.assignee != assignee)
			{
				Room room2 = assignable.assignee as Room;
				if (room2 != null && room2.roomType.priority_building_use && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
				if (assignable.assignee.NumOwners() < num && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040019E0 RID: 6624
	private List<Assignable> assignables = new List<Assignable>();

	// Token: 0x040019E1 RID: 6625
	public const string PUBLIC_GROUP_ID = "public";

	// Token: 0x040019E2 RID: 6626
	public Dictionary<string, AssignmentGroup> assignment_groups = new Dictionary<string, AssignmentGroup>
	{
		{
			"public",
			new AssignmentGroup("public", new IAssignableIdentity[0], UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.PUBLIC)
		}
	};

	// Token: 0x040019E3 RID: 6627
	private static readonly EventSystem.IntraObjectHandler<AssignmentManager> MinionMigrationDelegate = new EventSystem.IntraObjectHandler<AssignmentManager>(delegate(AssignmentManager component, object data)
	{
		component.MinionMigration(data);
	});

	// Token: 0x040019E4 RID: 6628
	private List<Assignable> PreferredAssignableResults = new List<Assignable>();
}
