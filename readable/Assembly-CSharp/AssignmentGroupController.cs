using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000B65 RID: 2917
public class AssignmentGroupController : KMonoBehaviour
{
	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x06005651 RID: 22097 RVA: 0x001F70B2 File Offset: 0x001F52B2
	// (set) Token: 0x06005652 RID: 22098 RVA: 0x001F70BA File Offset: 0x001F52BA
	public string AssignmentGroupID
	{
		get
		{
			return this._assignmentGroupID;
		}
		private set
		{
			this._assignmentGroupID = value;
		}
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x001F70C3 File Offset: 0x001F52C3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005654 RID: 22100 RVA: 0x001F70CB File Offset: 0x001F52CB
	[OnDeserialized]
	protected void CreateOrRestoreGroupID()
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
			return;
		}
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x06005655 RID: 22101 RVA: 0x001F7108 File Offset: 0x001F5308
	public void SetGroupID(string id)
	{
		DebugUtil.DevAssert(!string.IsNullOrEmpty(id), "Trying to set Assignment group on " + base.gameObject.name + " to null or empty.", null);
		if (!string.IsNullOrEmpty(id))
		{
			this.AssignmentGroupID = id;
		}
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x001F7142 File Offset: 0x001F5342
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreGroupAssignees();
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x001F7150 File Offset: 0x001F5350
	private void GenerateGroupID()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			return;
		}
		this.SetGroupID(base.GetComponent<KPrefabID>().PrefabID().ToString() + "_" + base.GetComponent<KPrefabID>().InstanceID.ToString() + "_assignmentGroup");
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x06005658 RID: 22104 RVA: 0x001F71DC File Offset: 0x001F53DC
	private void RestoreGroupAssignees()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		this.CreateOrRestoreGroupID();
		if (this.minionsInGroupAtLoad == null)
		{
			this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[0];
		}
		for (int i = 0; i < this.minionsInGroupAtLoad.Length; i++)
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, this.minionsInGroupAtLoad[i].Get());
		}
		Ownable component = base.GetComponent<Ownable>();
		if (component != null)
		{
			component.Assign(Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID]);
			component.SetCanBeAssigned(false);
		}
	}

	// Token: 0x06005659 RID: 22105 RVA: 0x001F7278 File Offset: 0x001F5478
	public bool CheckMinionIsMember(MinionAssignablesProxy minion)
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
		}
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].HasMember(minion);
	}

	// Token: 0x0600565A RID: 22106 RVA: 0x001F72B0 File Offset: 0x001F54B0
	public void SetMember(MinionAssignablesProxy minion, bool isAllowed)
	{
		Debug.Assert(DlcManager.IsExpansion1Active());
		if (!isAllowed)
		{
			Game.Instance.assignmentManager.RemoveFromAssignmentGroup(this.AssignmentGroupID, minion);
			return;
		}
		if (!this.CheckMinionIsMember(minion))
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, minion);
		}
	}

	// Token: 0x0600565B RID: 22107 RVA: 0x001F7300 File Offset: 0x001F5500
	protected override void OnCleanUp()
	{
		if (this.generateGroupOnStart)
		{
			Game.Instance.assignmentManager.RemoveAssignmentGroup(this.AssignmentGroupID);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600565C RID: 22108 RVA: 0x001F7328 File Offset: 0x001F5528
	[OnSerializing]
	private void OnSerialize()
	{
		Debug.Assert(!string.IsNullOrEmpty(this.AssignmentGroupID), "Assignment group on " + base.gameObject.name + " has null or empty ID");
		List<IAssignableIdentity> members = Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
		this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[members.Count];
		for (int i = 0; i < members.Count; i++)
		{
			this.minionsInGroupAtLoad[i] = new Ref<MinionAssignablesProxy>((MinionAssignablesProxy)members[i]);
		}
	}

	// Token: 0x0600565D RID: 22109 RVA: 0x001F73BD File Offset: 0x001F55BD
	public List<IAssignableIdentity> GetMembers()
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
		}
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
	}

	// Token: 0x04003A49 RID: 14921
	public bool generateGroupOnStart;

	// Token: 0x04003A4A RID: 14922
	[Serialize]
	private string _assignmentGroupID;

	// Token: 0x04003A4B RID: 14923
	[Serialize]
	private Ref<MinionAssignablesProxy>[] minionsInGroupAtLoad;
}
