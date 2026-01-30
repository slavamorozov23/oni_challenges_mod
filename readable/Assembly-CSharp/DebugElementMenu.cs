using System;
using UnityEngine;

// Token: 0x02000CF1 RID: 3313
public class DebugElementMenu : KButtonMenu
{
	// Token: 0x06006653 RID: 26195 RVA: 0x002685BF File Offset: 0x002667BF
	protected override void OnPrefabInit()
	{
		DebugElementMenu.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006654 RID: 26196 RVA: 0x002685D4 File Offset: 0x002667D4
	protected override void OnForcedCleanUp()
	{
		DebugElementMenu.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006655 RID: 26197 RVA: 0x002685E2 File Offset: 0x002667E2
	public void Turnoff()
	{
		this.root.gameObject.SetActive(false);
	}

	// Token: 0x040045D2 RID: 17874
	public static DebugElementMenu Instance;

	// Token: 0x040045D3 RID: 17875
	public GameObject root;
}
