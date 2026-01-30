using System;
using System.Collections.Generic;

// Token: 0x020005EA RID: 1514
public class AssignmentGroup : IAssignableIdentity
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06002302 RID: 8962 RVA: 0x000CB6D9 File Offset: 0x000C98D9
	// (set) Token: 0x06002303 RID: 8963 RVA: 0x000CB6E1 File Offset: 0x000C98E1
	public string id { get; private set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06002304 RID: 8964 RVA: 0x000CB6EA File Offset: 0x000C98EA
	// (set) Token: 0x06002305 RID: 8965 RVA: 0x000CB6F2 File Offset: 0x000C98F2
	public string name { get; private set; }

	// Token: 0x06002306 RID: 8966 RVA: 0x000CB6FC File Offset: 0x000C98FC
	public AssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		this.id = id;
		this.name = name;
		foreach (IAssignableIdentity item in members)
		{
			this.members.Add(item);
		}
		if (Game.Instance != null)
		{
			Game.Instance.assignmentManager.assignment_groups.Add(id, this);
			Game.Instance.Trigger(-1123234494, this);
		}
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x000CB786 File Offset: 0x000C9986
	public void AddMember(IAssignableIdentity member)
	{
		if (!this.members.Contains(member))
		{
			this.members.Add(member);
		}
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x000CB7B2 File Offset: 0x000C99B2
	public void RemoveMember(IAssignableIdentity member)
	{
		this.members.Remove(member);
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06002309 RID: 8969 RVA: 0x000CB7D1 File Offset: 0x000C99D1
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000CB7D9 File Offset: 0x000C99D9
	public bool HasMember(IAssignableIdentity member)
	{
		return this.members.Contains(member);
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000CB7E7 File Offset: 0x000C99E7
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000CB7EA File Offset: 0x000C99EA
	public List<IAssignableIdentity> GetMembers()
	{
		return this.members;
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000CB7F4 File Offset: 0x000C99F4
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			this.current_owners.AddRange(assignableIdentity.GetOwners());
		}
		return this.current_owners;
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000CB864 File Offset: 0x000C9A64
	public Ownables GetSoleOwner()
	{
		if (this.members.Count != 1)
		{
			return null;
		}
		return this.members[0].GetSoleOwner();
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000CB888 File Offset: 0x000C9A88
	public bool HasOwner(Assignables owner)
	{
		using (List<IAssignableIdentity>.Enumerator enumerator = this.members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasOwner(owner))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000CB8E4 File Offset: 0x000C9AE4
	public int NumOwners()
	{
		int num = 0;
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			num += assignableIdentity.NumOwners();
		}
		return num;
	}

	// Token: 0x04001483 RID: 5251
	private List<IAssignableIdentity> members = new List<IAssignableIdentity>();

	// Token: 0x04001484 RID: 5252
	public List<Ownables> current_owners = new List<Ownables>();
}
