using System;
using UnityEngine;

// Token: 0x02000ADB RID: 2779
public class CameraReferenceTexture : MonoBehaviour
{
	// Token: 0x060050D0 RID: 20688 RVA: 0x001D423C File Offset: 0x001D243C
	private void OnPreCull()
	{
		if (this.quad == null)
		{
			this.quad = new FullScreenQuad("CameraReferenceTexture", base.GetComponent<Camera>(), this.referenceCamera.GetComponent<CameraRenderTexture>().ShouldFlip());
		}
		if (this.referenceCamera != null)
		{
			this.quad.Draw(this.referenceCamera.GetComponent<CameraRenderTexture>().GetTexture());
		}
	}

	// Token: 0x040035F1 RID: 13809
	public Camera referenceCamera;

	// Token: 0x040035F2 RID: 13810
	private FullScreenQuad quad;
}
