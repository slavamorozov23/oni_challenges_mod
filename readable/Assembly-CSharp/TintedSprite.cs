using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
	// Token: 0x06002AED RID: 10989 RVA: 0x000FB26A File Offset: 0x000F946A
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x000FB26C File Offset: 0x000F946C
	public void OnBeforeSerialize()
	{
		if (this.sprite != null)
		{
			this.name = this.sprite.name;
		}
	}

	// Token: 0x04001996 RID: 6550
	[ReadOnly]
	public string name;

	// Token: 0x04001997 RID: 6551
	public Sprite sprite;

	// Token: 0x04001998 RID: 6552
	public Color color;
}
