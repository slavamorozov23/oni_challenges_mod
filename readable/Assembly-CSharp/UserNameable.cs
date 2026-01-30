using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000C03 RID: 3075
[AddComponentMenu("KMonoBehaviour/scripts/UserNameable")]
public class UserNameable : KMonoBehaviour
{
	// Token: 0x06005C77 RID: 23671 RVA: 0x00217EFD File Offset: 0x002160FD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (string.IsNullOrEmpty(this.savedName))
		{
			this.SetName(base.gameObject.GetProperName());
			return;
		}
		this.SetName(this.savedName);
	}

	// Token: 0x06005C78 RID: 23672 RVA: 0x00217F30 File Offset: 0x00216130
	public void SetName(string name)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		base.name = name;
		if (component != null)
		{
			component.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
		if (base.GetComponent<CommandModule>() != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.GetComponent<LaunchConditionManager>()).SetRocketName(name);
		}
		else if (base.GetComponent<Clustercraft>() != null)
		{
			ClusterNameDisplayScreen.Instance.UpdateName(base.GetComponent<Clustercraft>());
		}
		this.savedName = name;
		base.Trigger(1102426921, name);
	}

	// Token: 0x04003D9C RID: 15772
	[Serialize]
	public string savedName = "";
}
