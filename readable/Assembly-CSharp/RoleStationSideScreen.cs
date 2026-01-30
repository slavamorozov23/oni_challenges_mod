using System;
using UnityEngine;

// Token: 0x02000E70 RID: 3696
public class RoleStationSideScreen : SideScreenContent
{
	// Token: 0x06007578 RID: 30072 RVA: 0x002CD445 File Offset: 0x002CB645
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06007579 RID: 30073 RVA: 0x002CD44D File Offset: 0x002CB64D
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x0400514A RID: 20810
	public GameObject content;

	// Token: 0x0400514B RID: 20811
	private GameObject target;

	// Token: 0x0400514C RID: 20812
	public LocText DescriptionText;
}
