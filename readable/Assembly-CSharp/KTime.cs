using System;
using UnityEngine;

// Token: 0x020009D9 RID: 2521
[AddComponentMenu("KMonoBehaviour/scripts/KTime")]
public class KTime : KMonoBehaviour
{
	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06004943 RID: 18755 RVA: 0x001A7F2B File Offset: 0x001A612B
	// (set) Token: 0x06004944 RID: 18756 RVA: 0x001A7F33 File Offset: 0x001A6133
	public float UnscaledGameTime { get; set; }

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06004945 RID: 18757 RVA: 0x001A7F3C File Offset: 0x001A613C
	// (set) Token: 0x06004946 RID: 18758 RVA: 0x001A7F43 File Offset: 0x001A6143
	public static KTime Instance { get; private set; }

	// Token: 0x06004947 RID: 18759 RVA: 0x001A7F4B File Offset: 0x001A614B
	public static void DestroyInstance()
	{
		KTime.Instance = null;
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x001A7F53 File Offset: 0x001A6153
	protected override void OnPrefabInit()
	{
		KTime.Instance = this;
		this.UnscaledGameTime = Time.unscaledTime;
	}

	// Token: 0x06004949 RID: 18761 RVA: 0x001A7F66 File Offset: 0x001A6166
	protected override void OnCleanUp()
	{
		KTime.Instance = null;
	}

	// Token: 0x0600494A RID: 18762 RVA: 0x001A7F6E File Offset: 0x001A616E
	public void Update()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			this.UnscaledGameTime += Time.unscaledDeltaTime;
		}
	}
}
