using System;
using UnityEngine;

// Token: 0x02000C3A RID: 3130
public class FillRenderTargetEffect : MonoBehaviour
{
	// Token: 0x06005E9C RID: 24220 RVA: 0x0022A741 File Offset: 0x00228941
	public void SetFillTexture(Texture tex)
	{
		this.fillTexture = tex;
	}

	// Token: 0x06005E9D RID: 24221 RVA: 0x0022A74A File Offset: 0x0022894A
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(this.fillTexture, null);
	}

	// Token: 0x04003EE9 RID: 16105
	private Texture fillTexture;
}
