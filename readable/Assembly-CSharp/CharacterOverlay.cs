using System;
using UnityEngine;

// Token: 0x02000CB2 RID: 3250
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
	// Token: 0x0600638F RID: 25487 RVA: 0x002511DF File Offset: 0x0024F3DF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x002511ED File Offset: 0x0024F3ED
	public void Register()
	{
		if (this.registered)
		{
			return;
		}
		this.registered = true;
		NameDisplayScreen.Instance.AddNewEntry(base.gameObject);
	}

	// Token: 0x040043A9 RID: 17321
	public bool shouldShowName;

	// Token: 0x040043AA RID: 17322
	private bool registered;
}
