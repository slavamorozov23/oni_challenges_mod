using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AAC RID: 2732
[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour
{
	// Token: 0x06004F27 RID: 20263 RVA: 0x001CBAD8 File Offset: 0x001C9CD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranch.Instance smi = base.gameObject.GetSMI<PlantBranch.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x06004F28 RID: 20264 RVA: 0x001CBB08 File Offset: 0x001C9D08
	public BuddingTrunk GetAndForgetOldTrunk()
	{
		BuddingTrunk result = (this.buddingTrunk == null) ? null : this.buddingTrunk.Get();
		this.buddingTrunk = null;
		return result;
	}

	// Token: 0x040034EB RID: 13547
	[Serialize]
	public Ref<BuddingTrunk> buddingTrunk;
}
