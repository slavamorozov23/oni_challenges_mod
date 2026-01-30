using System;
using UnityEngine;

// Token: 0x02000ADC RID: 2780
public class CameraRenderTexture : MonoBehaviour
{
	// Token: 0x060050D2 RID: 20690 RVA: 0x001D42A8 File Offset: 0x001D24A8
	private void Awake()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
	}

	// Token: 0x060050D3 RID: 20691 RVA: 0x001D42BF File Offset: 0x001D24BF
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.OnResize();
	}

	// Token: 0x060050D4 RID: 20692 RVA: 0x001D42FC File Offset: 0x001D24FC
	private void OnResize()
	{
		if (this.resultTexture != null)
		{
			this.resultTexture.DestroyRenderTexture();
		}
		this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		this.resultTexture.name = base.name;
		this.resultTexture.filterMode = FilterMode.Point;
		this.resultTexture.autoGenerateMips = false;
		if (this.TextureName != "")
		{
			Shader.SetGlobalTexture(this.TextureName, this.resultTexture);
		}
	}

	// Token: 0x060050D5 RID: 20693 RVA: 0x001D4385 File Offset: 0x001D2585
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.resultTexture, this.material);
	}

	// Token: 0x060050D6 RID: 20694 RVA: 0x001D4399 File Offset: 0x001D2599
	public RenderTexture GetTexture()
	{
		return this.resultTexture;
	}

	// Token: 0x060050D7 RID: 20695 RVA: 0x001D43A1 File Offset: 0x001D25A1
	public bool ShouldFlip()
	{
		return false;
	}

	// Token: 0x040035F3 RID: 13811
	public string TextureName;

	// Token: 0x040035F4 RID: 13812
	private RenderTexture resultTexture;

	// Token: 0x040035F5 RID: 13813
	private Material material;
}
