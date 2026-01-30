using System;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class MainCamera : MonoBehaviour
{
	// Token: 0x060023D0 RID: 9168 RVA: 0x000CF307 File Offset: 0x000CD507
	private void Awake()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.Destroy(Camera.main.gameObject);
		}
		base.gameObject.tag = "MainCamera";
	}
}
