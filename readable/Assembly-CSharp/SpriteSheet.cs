using System;
using UnityEngine;

// Token: 0x0200063B RID: 1595
[Serializable]
public struct SpriteSheet
{
	// Token: 0x04001672 RID: 5746
	public string name;

	// Token: 0x04001673 RID: 5747
	public int numFrames;

	// Token: 0x04001674 RID: 5748
	public int numXFrames;

	// Token: 0x04001675 RID: 5749
	public Vector2 uvFrameSize;

	// Token: 0x04001676 RID: 5750
	public int renderLayer;

	// Token: 0x04001677 RID: 5751
	public Material material;

	// Token: 0x04001678 RID: 5752
	public Texture2D texture;
}
