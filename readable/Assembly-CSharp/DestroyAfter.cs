using System;
using UnityEngine;

// Token: 0x020005D2 RID: 1490
[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
	// Token: 0x06002247 RID: 8775 RVA: 0x000C7313 File Offset: 0x000C5513
	protected override void OnSpawn()
	{
		this.particleSystems = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000C7328 File Offset: 0x000C5528
	private bool IsAlive()
	{
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			if (this.particleSystems[i].IsAlive(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000C735B File Offset: 0x000C555B
	private void Update()
	{
		if (this.particleSystems != null && !this.IsAlive())
		{
			this.DeleteObject();
		}
	}

	// Token: 0x04001402 RID: 5122
	private ParticleSystem[] particleSystems;
}
