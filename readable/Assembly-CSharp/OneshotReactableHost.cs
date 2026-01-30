using System;
using UnityEngine;

// Token: 0x02000A80 RID: 2688
[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
	// Token: 0x06004E23 RID: 20003 RVA: 0x001C681D File Offset: 0x001C4A1D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001C6849 File Offset: 0x001C4A49
	public void SetReactable(Reactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x06004E25 RID: 20005 RVA: 0x001C6854 File Offset: 0x001C4A54
	private void OnExpire(object obj)
	{
		if (!this.reactable.IsReacting)
		{
			this.reactable.Cleanup();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x0400340E RID: 13326
	private Reactable reactable;

	// Token: 0x0400340F RID: 13327
	public float lifetime = 1f;
}
