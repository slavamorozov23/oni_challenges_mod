using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A99 RID: 2713
[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour
{
	// Token: 0x06004EB5 RID: 20149 RVA: 0x001C9BF8 File Offset: 0x001C7DF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranchGrower.Instance smi = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x06004EB6 RID: 20150 RVA: 0x001C9C28 File Offset: 0x001C7E28
	public KPrefabID[] GetAndForgetOldSerializedBranches()
	{
		KPrefabID[] array = null;
		if (this.buds != null)
		{
			array = new KPrefabID[this.buds.Length];
			for (int i = 0; i < this.buds.Length; i++)
			{
				HarvestDesignatable harvestDesignatable = (this.buds[i] == null) ? null : this.buds[i].Get();
				array[i] = ((harvestDesignatable == null) ? null : harvestDesignatable.GetComponent<KPrefabID>());
			}
		}
		this.buds = null;
		return array;
	}

	// Token: 0x04003482 RID: 13442
	[Serialize]
	private Ref<HarvestDesignatable>[] buds;
}
