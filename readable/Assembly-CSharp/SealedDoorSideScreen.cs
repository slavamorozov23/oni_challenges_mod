using System;
using UnityEngine;

// Token: 0x02000E71 RID: 3697
public class SealedDoorSideScreen : SideScreenContent
{
	// Token: 0x0600757B RID: 30075 RVA: 0x002CD458 File Offset: 0x002CB658
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += delegate()
		{
			this.target.OrderUnseal();
		};
		this.Refresh();
	}

	// Token: 0x0600757C RID: 30076 RVA: 0x002CD47D File Offset: 0x002CB67D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x0600757D RID: 30077 RVA: 0x002CD48C File Offset: 0x002CB68C
	public override void SetTarget(GameObject target)
	{
		Door component = target.GetComponent<Door>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a Door associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x0600757E RID: 30078 RVA: 0x002CD4C1 File Offset: 0x002CB6C1
	private void Refresh()
	{
		if (!this.target.isSealed)
		{
			this.ContentContainer.SetActive(false);
			return;
		}
		this.ContentContainer.SetActive(true);
	}

	// Token: 0x0400514D RID: 20813
	[SerializeField]
	private LocText label;

	// Token: 0x0400514E RID: 20814
	[SerializeField]
	private KButton button;

	// Token: 0x0400514F RID: 20815
	[SerializeField]
	private Door target;
}
