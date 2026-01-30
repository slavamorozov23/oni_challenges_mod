using System;
using UnityEngine;

// Token: 0x02000E9C RID: 3740
[AddComponentMenu("KMonoBehaviour/scripts/SpawnScreen")]
public class SpawnScreen : KMonoBehaviour
{
	// Token: 0x0600779F RID: 30623 RVA: 0x002DE2FE File Offset: 0x002DC4FE
	protected override void OnPrefabInit()
	{
		Util.KInstantiateUI(this.Screen, base.gameObject, false);
	}

	// Token: 0x0400532C RID: 21292
	public GameObject Screen;
}
