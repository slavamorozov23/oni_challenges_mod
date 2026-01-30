using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000834 RID: 2100
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
	// Token: 0x0600394A RID: 14666 RVA: 0x0013FEAA File Offset: 0x0013E0AA
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x0013FEB2 File Offset: 0x0013E0B2
	public void StartLoop()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play(this.anim_name_pre, KAnim.PlayMode.Once, 1f, 0f);
		component.Play(this.anim_name_loop, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x0013FEF1 File Offset: 0x0013E0F1
	public void TriggerDestroy()
	{
		base.GetComponent<KBatchedAnimController>().Play(this.anim_name_pst, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x040022F4 RID: 8948
	[Serialize]
	[NonSerialized]
	public Vector3 velocity = Vector3.zero;

	// Token: 0x040022F5 RID: 8949
	[Serialize]
	[NonSerialized]
	public float mass;

	// Token: 0x040022F6 RID: 8950
	[Serialize]
	[NonSerialized]
	public float temperature;

	// Token: 0x040022F7 RID: 8951
	[Serialize]
	[NonSerialized]
	public float lifetimeRemaining;

	// Token: 0x040022F8 RID: 8952
	[Serialize]
	[NonSerialized]
	public string kAnimFileName = "exhale_kanim";

	// Token: 0x040022F9 RID: 8953
	[Serialize]
	[NonSerialized]
	public string anim_name_pre = "exhale_pre";

	// Token: 0x040022FA RID: 8954
	[Serialize]
	[NonSerialized]
	public string anim_name_loop = "exhale_loop";

	// Token: 0x040022FB RID: 8955
	[Serialize]
	[NonSerialized]
	public string anim_name_pst = "exhale_pst";

	// Token: 0x040022FC RID: 8956
	[Serialize]
	[NonSerialized]
	public bool affectedByGravity = true;
}
