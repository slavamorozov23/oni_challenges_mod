using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200088A RID: 2186
[AddComponentMenu("KMonoBehaviour/scripts/BudUprootedMonitor")]
public class BudUprootedMonitor : KMonoBehaviour
{
	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06003C29 RID: 15401 RVA: 0x00150CBF File Offset: 0x0014EEBF
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x00150CDB File Offset: 0x0014EEDB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x00150CF4 File Offset: 0x0014EEF4
	public void SetParentObject(KPrefabID id)
	{
		this.parentObject = new Ref<KPrefabID>(id);
		base.Subscribe(id.gameObject, 1969584890, new Action<object>(this.OnLoseParent));
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x00150D20 File Offset: 0x0014EF20
	private void OnLoseParent(object obj)
	{
		if (!this.uprooted && !base.isNull)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			this.uprooted = true;
			base.Trigger(-216549700, null);
			if (this.destroyOnParentLost)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06003C2D RID: 15405 RVA: 0x00150D74 File Offset: 0x0014EF74
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x00150D7C File Offset: 0x0014EF7C
	public static bool IsObjectUprooted(GameObject plant)
	{
		BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
		return !(component == null) && component.IsUprooted;
	}

	// Token: 0x0400251C RID: 9500
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x0400251D RID: 9501
	[Serialize]
	private bool uprooted;

	// Token: 0x0400251E RID: 9502
	public bool destroyOnParentLost;

	// Token: 0x0400251F RID: 9503
	public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();

	// Token: 0x04002520 RID: 9504
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002521 RID: 9505
	private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>(delegate(BudUprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
