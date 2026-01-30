using System;
using UnityEngine;

// Token: 0x02000AEE RID: 2798
public class MultipleRenderTargetProxy : MonoBehaviour
{
	// Token: 0x06005155 RID: 20821 RVA: 0x001D7D54 File Offset: 0x001D5F54
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.CreateRenderTarget();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x06005156 RID: 20822 RVA: 0x001D7DAB File Offset: 0x001D5FAB
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.colouredOverlayBufferEnabled = enabled;
		this.CreateRenderTarget();
	}

	// Token: 0x06005157 RID: 20823 RVA: 0x001D7DBC File Offset: 0x001D5FBC
	private void CreateRenderTarget()
	{
		RenderBuffer[] array = new RenderBuffer[this.colouredOverlayBufferEnabled ? 3 : 2];
		this.Textures[0] = this.RecreateRT(this.Textures[0], 24, RenderTextureFormat.ARGB32);
		this.Textures[0].filterMode = FilterMode.Point;
		this.Textures[0].name = "MRT0";
		this.Textures[1] = this.RecreateRT(this.Textures[1], 0, RenderTextureFormat.R8);
		this.Textures[1].filterMode = FilterMode.Point;
		this.Textures[1].name = "MRT1";
		array[0] = this.Textures[0].colorBuffer;
		array[1] = this.Textures[1].colorBuffer;
		if (this.colouredOverlayBufferEnabled)
		{
			this.Textures[2] = this.RecreateRT(this.Textures[2], 0, RenderTextureFormat.ARGB32);
			this.Textures[2].filterMode = FilterMode.Bilinear;
			this.Textures[2].name = "MRT2";
			array[2] = this.Textures[2].colorBuffer;
		}
		base.GetComponent<Camera>().SetTargetBuffers(array, this.Textures[0].depthBuffer);
		this.OnShadersReloaded();
	}

	// Token: 0x06005158 RID: 20824 RVA: 0x001D7EE8 File Offset: 0x001D60E8
	private RenderTexture RecreateRT(RenderTexture rt, int depth, RenderTextureFormat format)
	{
		RenderTexture result = rt;
		if (rt == null || rt.width != Screen.width || rt.height != Screen.height || rt.format != format)
		{
			if (rt != null)
			{
				rt.DestroyRenderTexture();
			}
			result = new RenderTexture(Screen.width, Screen.height, depth, format);
		}
		return result;
	}

	// Token: 0x06005159 RID: 20825 RVA: 0x001D7F45 File Offset: 0x001D6145
	private void OnResize()
	{
		this.CreateRenderTarget();
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x001D7F4D File Offset: 0x001D614D
	private void Update()
	{
		if (!this.Textures[0].IsCreated())
		{
			this.CreateRenderTarget();
		}
	}

	// Token: 0x0600515B RID: 20827 RVA: 0x001D7F64 File Offset: 0x001D6164
	private void OnShadersReloaded()
	{
		Shader.SetGlobalTexture("_MRT0", this.Textures[0]);
		Shader.SetGlobalTexture("_MRT1", this.Textures[1]);
		if (this.colouredOverlayBufferEnabled)
		{
			Shader.SetGlobalTexture("_MRT2", this.Textures[2]);
		}
	}

	// Token: 0x040036F8 RID: 14072
	public RenderTexture[] Textures = new RenderTexture[3];

	// Token: 0x040036F9 RID: 14073
	private bool colouredOverlayBufferEnabled;
}
