using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
[AddComponentMenu("KMonoBehaviour/scripts/EffectArea")]
public class EffectArea : KMonoBehaviour
{
	// Token: 0x0600229D RID: 8861 RVA: 0x000C972A File Offset: 0x000C792A
	protected override void OnPrefabInit()
	{
		this.Effect = Db.Get().effects.Get(this.EffectName);
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000C9748 File Offset: 0x000C7948
	private void Update()
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			int num3 = 0;
			int num4 = 0;
			Grid.PosToXY(minionIdentity.transform.GetPosition(), out num3, out num4);
			if (Math.Abs(num3 - num) <= this.Area && Math.Abs(num4 - num2) <= this.Area)
			{
				minionIdentity.GetComponent<Effects>().Add(this.Effect, true);
			}
		}
	}

	// Token: 0x04001439 RID: 5177
	public string EffectName;

	// Token: 0x0400143A RID: 5178
	public int Area;

	// Token: 0x0400143B RID: 5179
	private Effect Effect;
}
