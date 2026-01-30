using System;
using UnityEngine;

// Token: 0x02000AE2 RID: 2786
public class GridCompositor : MonoBehaviour
{
	// Token: 0x060050F4 RID: 20724 RVA: 0x001D5838 File Offset: 0x001D3A38
	public static void DestroyInstance()
	{
		GridCompositor.Instance = null;
	}

	// Token: 0x060050F5 RID: 20725 RVA: 0x001D5840 File Offset: 0x001D3A40
	private void Awake()
	{
		GridCompositor.Instance = this;
		base.enabled = false;
	}

	// Token: 0x060050F6 RID: 20726 RVA: 0x001D584F File Offset: 0x001D3A4F
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/GridCompositor"));
	}

	// Token: 0x060050F7 RID: 20727 RVA: 0x001D5866 File Offset: 0x001D3A66
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x060050F8 RID: 20728 RVA: 0x001D5875 File Offset: 0x001D3A75
	public void ToggleMajor(bool on)
	{
		this.onMajor = on;
		this.Refresh();
	}

	// Token: 0x060050F9 RID: 20729 RVA: 0x001D5884 File Offset: 0x001D3A84
	public void ToggleMinor(bool on)
	{
		this.onMinor = on;
		this.Refresh();
	}

	// Token: 0x060050FA RID: 20730 RVA: 0x001D5893 File Offset: 0x001D3A93
	private void Refresh()
	{
		base.enabled = (this.onMinor || this.onMajor);
	}

	// Token: 0x04003601 RID: 13825
	public Material material;

	// Token: 0x04003602 RID: 13826
	public static GridCompositor Instance;

	// Token: 0x04003603 RID: 13827
	private bool onMajor;

	// Token: 0x04003604 RID: 13828
	private bool onMinor;
}
