using System;
using UnityEngine;

// Token: 0x0200055A RID: 1370
public class NativeAnimBatchLoader : MonoBehaviour
{
	// Token: 0x06001E75 RID: 7797 RVA: 0x000A5AB0 File Offset: 0x000A3CB0
	private void Start()
	{
		if (this.generateObjects)
		{
			for (int i = 0; i < this.enableObjects.Length; i++)
			{
				if (this.enableObjects[i] != null)
				{
					this.enableObjects[i].GetComponent<KBatchedAnimController>().visibilityType = KAnimControllerBase.VisibilityType.Always;
					this.enableObjects[i].SetActive(true);
				}
			}
		}
		if (this.setTimeScale)
		{
			Time.timeScale = 1f;
		}
		if (this.destroySelf)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x000A5B2C File Offset: 0x000A3D2C
	private void LateUpdate()
	{
		if (this.destroySelf)
		{
			return;
		}
		if (this.performUpdate)
		{
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		}
		if (this.performRender)
		{
			KAnimBatchManager.Instance().Render();
		}
	}

	// Token: 0x040011CD RID: 4557
	public bool performTimeUpdate;

	// Token: 0x040011CE RID: 4558
	public bool performUpdate;

	// Token: 0x040011CF RID: 4559
	public bool performRender;

	// Token: 0x040011D0 RID: 4560
	public bool setTimeScale;

	// Token: 0x040011D1 RID: 4561
	public bool destroySelf;

	// Token: 0x040011D2 RID: 4562
	public bool generateObjects;

	// Token: 0x040011D3 RID: 4563
	public GameObject[] enableObjects;
}
