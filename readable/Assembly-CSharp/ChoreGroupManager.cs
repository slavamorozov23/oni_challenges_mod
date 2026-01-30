using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000841 RID: 2113
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreGroupManager")]
public class ChoreGroupManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600399A RID: 14746 RVA: 0x00141B63 File Offset: 0x0013FD63
	public static void DestroyInstance()
	{
		ChoreGroupManager.instance = null;
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x0600399B RID: 14747 RVA: 0x00141B6B File Offset: 0x0013FD6B
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600399C RID: 14748 RVA: 0x00141B73 File Offset: 0x0013FD73
	public Dictionary<Tag, int> DefaultChorePermission
	{
		get
		{
			return this.defaultChorePermissions;
		}
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x00141B7C File Offset: 0x0013FD7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ChoreGroupManager.instance = this;
		this.ConvertOldVersion();
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			if (!this.defaultChorePermissions.ContainsKey(choreGroup.Id.ToTag()))
			{
				this.defaultChorePermissions.Add(choreGroup.Id.ToTag(), 2);
			}
		}
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x00141C14 File Offset: 0x0013FE14
	private void ConvertOldVersion()
	{
		foreach (Tag key in this.defaultForbiddenTagsList)
		{
			if (!this.defaultChorePermissions.ContainsKey(key))
			{
				this.defaultChorePermissions.Add(key, -1);
			}
			this.defaultChorePermissions[key] = 0;
		}
		this.defaultForbiddenTagsList.Clear();
	}

	// Token: 0x04002348 RID: 9032
	public static ChoreGroupManager instance;

	// Token: 0x04002349 RID: 9033
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x0400234A RID: 9034
	[Serialize]
	private Dictionary<Tag, int> defaultChorePermissions = new Dictionary<Tag, int>();
}
