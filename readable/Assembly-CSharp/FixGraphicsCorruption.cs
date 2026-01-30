using System;
using UnityEngine;

// Token: 0x02000C3B RID: 3131
public class FixGraphicsCorruption : MonoBehaviour
{
	// Token: 0x06005E9F RID: 24223 RVA: 0x0022A760 File Offset: 0x00228960
	private void Start()
	{
		Camera component = base.GetComponent<Camera>();
		component.transparencySortMode = TransparencySortMode.Orthographic;
		component.tag = "Untagged";
	}

	// Token: 0x06005EA0 RID: 24224 RVA: 0x0022A779 File Offset: 0x00228979
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}
}
