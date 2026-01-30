using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AED RID: 2797
public class MultipleRenderTarget : MonoBehaviour
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x0600514E RID: 20814 RVA: 0x001D7C7C File Offset: 0x001D5E7C
	// (remove) Token: 0x0600514F RID: 20815 RVA: 0x001D7CB4 File Offset: 0x001D5EB4
	public event Action<Camera> onSetupComplete;

	// Token: 0x06005150 RID: 20816 RVA: 0x001D7CE9 File Offset: 0x001D5EE9
	private void Start()
	{
		base.StartCoroutine(this.SetupProxy());
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x001D7CF8 File Offset: 0x001D5EF8
	private IEnumerator SetupProxy()
	{
		yield return null;
		Camera component = base.GetComponent<Camera>();
		Camera camera = new GameObject().AddComponent<Camera>();
		camera.CopyFrom(component);
		this.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
		camera.name = component.name + " MRT";
		camera.transform.parent = component.transform;
		camera.transform.SetLocalPosition(Vector3.zero);
		camera.depth = component.depth - 1f;
		component.cullingMask = 0;
		component.clearFlags = CameraClearFlags.Color;
		this.quad = new FullScreenQuad("MultipleRenderTarget", component, true);
		if (this.onSetupComplete != null)
		{
			this.onSetupComplete(camera);
		}
		yield break;
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x001D7D07 File Offset: 0x001D5F07
	private void OnPreCull()
	{
		if (this.renderProxy != null)
		{
			this.quad.Draw(this.renderProxy.Textures[0]);
		}
	}

	// Token: 0x06005153 RID: 20819 RVA: 0x001D7D2F File Offset: 0x001D5F2F
	public void ToggleColouredOverlayView(bool enabled)
	{
		if (this.renderProxy != null)
		{
			this.renderProxy.ToggleColouredOverlayView(enabled);
		}
	}

	// Token: 0x040036F4 RID: 14068
	private MultipleRenderTargetProxy renderProxy;

	// Token: 0x040036F5 RID: 14069
	private FullScreenQuad quad;

	// Token: 0x040036F7 RID: 14071
	public bool isFrontEnd;
}
