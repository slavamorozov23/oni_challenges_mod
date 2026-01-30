using System;
using UnityEngine;

// Token: 0x02000636 RID: 1590
[AddComponentMenu("KMonoBehaviour/scripts/SimpleMassStatusItem")]
public class SimpleMassStatusItem : KMonoBehaviour
{
	// Token: 0x060025FA RID: 9722 RVA: 0x000DA76E File Offset: 0x000D896E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
	}

	// Token: 0x04001661 RID: 5729
	public string symbolPrefix = "";
}
