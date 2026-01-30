using System;
using UnityEngine;

// Token: 0x020008B0 RID: 2224
[AddComponentMenu("KMonoBehaviour/scripts/NotCapturable")]
public class NotCapturable : KMonoBehaviour
{
	// Token: 0x06003D4B RID: 15691 RVA: 0x0015629C File Offset: 0x0015449C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (base.GetComponent<Capturable>() != null)
		{
			DebugUtil.LogErrorArgs(this, new object[]
			{
				"Entity has both Capturable and NotCapturable!"
			});
		}
		Components.NotCapturables.Add(this);
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x001562D1 File Offset: 0x001544D1
	protected override void OnCleanUp()
	{
		Components.NotCapturables.Remove(this);
		base.OnCleanUp();
	}
}
