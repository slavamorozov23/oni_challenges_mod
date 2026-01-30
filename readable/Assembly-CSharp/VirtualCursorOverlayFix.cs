using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EC6 RID: 3782
public class VirtualCursorOverlayFix : MonoBehaviour
{
	// Token: 0x06007920 RID: 31008 RVA: 0x002E8E18 File Offset: 0x002E7018
	private void Awake()
	{
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		this.cursorRendTex = new RenderTexture(width, height, 0);
		this.screenSpaceCamera.enabled = true;
		this.screenSpaceCamera.targetTexture = this.cursorRendTex;
		this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
		base.StartCoroutine(this.RenderVirtualCursor());
	}

	// Token: 0x06007921 RID: 31009 RVA: 0x002E8E94 File Offset: 0x002E7094
	private IEnumerator RenderVirtualCursor()
	{
		bool ShowCursor = KInputManager.currentControllerIsGamepad;
		while (Application.isPlaying)
		{
			ShowCursor = KInputManager.currentControllerIsGamepad;
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C))
			{
				ShowCursor = true;
			}
			this.screenSpaceCamera.enabled = true;
			if (!this.screenSpaceOverlayImage.enabled && ShowCursor)
			{
				yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
			}
			this.actualCursor.enabled = ShowCursor;
			this.screenSpaceOverlayImage.enabled = ShowCursor;
			this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400547A RID: 21626
	private RenderTexture cursorRendTex;

	// Token: 0x0400547B RID: 21627
	public Camera screenSpaceCamera;

	// Token: 0x0400547C RID: 21628
	public Image screenSpaceOverlayImage;

	// Token: 0x0400547D RID: 21629
	public RawImage actualCursor;
}
