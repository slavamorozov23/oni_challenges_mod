using System;
using UnityEngine;

// Token: 0x02000637 RID: 1591
[AddComponentMenu("KMonoBehaviour/scripts/SimpleVent")]
public class SimpleVent : KMonoBehaviour
{
	// Token: 0x060025FC RID: 9724 RVA: 0x000DA7AA File Offset: 0x000D89AA
	protected override void OnPrefabInit()
	{
		base.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
		base.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x000DA7CE File Offset: 0x000D89CE
	protected override void OnSpawn()
	{
		this.OnChanged(null);
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x000DA7D8 File Offset: 0x000D89D8
	private void OnChanged(object data)
	{
		if (this.operational.IsFunctional)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
	}

	// Token: 0x04001662 RID: 5730
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001663 RID: 5731
	private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>(delegate(SimpleVent component, object data)
	{
		component.OnChanged(data);
	});
}
