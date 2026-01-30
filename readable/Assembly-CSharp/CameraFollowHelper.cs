using System;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
[AddComponentMenu("KMonoBehaviour/scripts/CameraFollowHelper")]
public class CameraFollowHelper : KMonoBehaviour
{
	// Token: 0x060020D6 RID: 8406 RVA: 0x000BE0E3 File Offset: 0x000BC2E3
	private void LateUpdate()
	{
		if (CameraController.Instance != null)
		{
			CameraController.Instance.UpdateFollowTarget();
		}
	}
}
