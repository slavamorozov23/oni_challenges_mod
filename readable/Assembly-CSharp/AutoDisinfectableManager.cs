using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059B RID: 1435
[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x0600203B RID: 8251 RVA: 0x000BA007 File Offset: 0x000B8207
	public static void DestroyInstance()
	{
		AutoDisinfectableManager.Instance = null;
	}

	// Token: 0x0600203C RID: 8252 RVA: 0x000BA00F File Offset: 0x000B820F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AutoDisinfectableManager.Instance = this;
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000BA01D File Offset: 0x000B821D
	public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		this.autoDisinfectables.Add(auto_disinfectable);
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x000BA02B File Offset: 0x000B822B
	public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		auto_disinfectable.CancelChore();
		this.autoDisinfectables.Remove(auto_disinfectable);
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000BA040 File Offset: 0x000B8240
	public void Sim1000ms(float dt)
	{
		for (int i = 0; i < this.autoDisinfectables.Count; i++)
		{
			this.autoDisinfectables[i].RefreshChore();
		}
	}

	// Token: 0x040012BE RID: 4798
	private List<AutoDisinfectable> autoDisinfectables = new List<AutoDisinfectable>();

	// Token: 0x040012BF RID: 4799
	public static AutoDisinfectableManager Instance;
}
