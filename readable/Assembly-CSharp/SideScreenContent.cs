using System;
using UnityEngine;

// Token: 0x02000E77 RID: 3703
public abstract class SideScreenContent : KScreen
{
	// Token: 0x060075D1 RID: 30161 RVA: 0x002D056A File Offset: 0x002CE76A
	public virtual void SetTarget(GameObject target)
	{
	}

	// Token: 0x060075D2 RID: 30162 RVA: 0x002D056C File Offset: 0x002CE76C
	public virtual void ClearTarget()
	{
	}

	// Token: 0x060075D3 RID: 30163
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x060075D4 RID: 30164 RVA: 0x002D056E File Offset: 0x002CE76E
	public virtual int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x060075D5 RID: 30165 RVA: 0x002D0571 File Offset: 0x002CE771
	public virtual string GetTitle()
	{
		return Strings.Get(this.titleKey);
	}

	// Token: 0x04005195 RID: 20885
	[SerializeField]
	protected string titleKey;

	// Token: 0x04005196 RID: 20886
	public GameObject ContentContainer;

	// Token: 0x04005197 RID: 20887
	public Func<bool> CheckShouldShowTopTitle;
}
