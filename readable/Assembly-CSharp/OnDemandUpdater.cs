using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A7F RID: 2687
public class OnDemandUpdater : MonoBehaviour
{
	// Token: 0x06004E1D RID: 19997 RVA: 0x001C677F File Offset: 0x001C497F
	public static void DestroyInstance()
	{
		OnDemandUpdater.Instance = null;
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x001C6787 File Offset: 0x001C4987
	private void Awake()
	{
		OnDemandUpdater.Instance = this;
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x001C678F File Offset: 0x001C498F
	public void Register(IUpdateOnDemand updater)
	{
		if (!this.Updaters.Contains(updater))
		{
			this.Updaters.Add(updater);
		}
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x001C67AB File Offset: 0x001C49AB
	public void Unregister(IUpdateOnDemand updater)
	{
		if (this.Updaters.Contains(updater))
		{
			this.Updaters.Remove(updater);
		}
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001C67C8 File Offset: 0x001C49C8
	private void Update()
	{
		for (int i = 0; i < this.Updaters.Count; i++)
		{
			if (this.Updaters[i] != null)
			{
				this.Updaters[i].UpdateOnDemand();
			}
		}
	}

	// Token: 0x0400340C RID: 13324
	private List<IUpdateOnDemand> Updaters = new List<IUpdateOnDemand>();

	// Token: 0x0400340D RID: 13325
	public static OnDemandUpdater Instance;
}
