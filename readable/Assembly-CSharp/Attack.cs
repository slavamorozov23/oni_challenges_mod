using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000860 RID: 2144
public class Attack
{
	// Token: 0x06003ADF RID: 15071 RVA: 0x0014893D File Offset: 0x00146B3D
	public Attack(AttackProperties properties, GameObject[] targets)
	{
		this.properties = properties;
		this.targets = targets;
		this.RollHits();
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0014895C File Offset: 0x00146B5C
	private void RollHits()
	{
		int num = 0;
		while (num < this.targets.Length && num <= this.properties.maxHits - 1)
		{
			if (this.targets[num] != null)
			{
				new Hit(this.properties, this.targets[num]);
			}
			num++;
		}
	}

	// Token: 0x040023C7 RID: 9159
	private AttackProperties properties;

	// Token: 0x040023C8 RID: 9160
	private GameObject[] targets;

	// Token: 0x040023C9 RID: 9161
	public List<Hit> Hits;
}
