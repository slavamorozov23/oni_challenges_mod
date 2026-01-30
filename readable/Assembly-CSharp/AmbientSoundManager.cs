using System;
using UnityEngine;

// Token: 0x020006CD RID: 1741
[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000F9A1D File Offset: 0x000F7C1D
	// (set) Token: 0x06002AA2 RID: 10914 RVA: 0x000F9A24 File Offset: 0x000F7C24
	public static AmbientSoundManager Instance { get; private set; }

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000F9A2C File Offset: 0x000F7C2C
	public static void Destroy()
	{
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000F9A34 File Offset: 0x000F7C34
	protected override void OnPrefabInit()
	{
		AmbientSoundManager.Instance = this;
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000F9A3C File Offset: 0x000F7C3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000F9A44 File Offset: 0x000F7C44
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x0400195E RID: 6494
	[MyCmpAdd]
	private LoopingSounds loopingSounds;
}
