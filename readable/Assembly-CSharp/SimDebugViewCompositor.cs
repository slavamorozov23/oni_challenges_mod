using System;
using UnityEngine;

// Token: 0x02000AF3 RID: 2803
public class SimDebugViewCompositor : MonoBehaviour
{
	// Token: 0x06005171 RID: 20849 RVA: 0x001D8477 File Offset: 0x001D6677
	private void Awake()
	{
		SimDebugViewCompositor.Instance = this;
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x001D847F File Offset: 0x001D667F
	private void OnDestroy()
	{
		SimDebugViewCompositor.Instance = null;
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x001D8487 File Offset: 0x001D6687
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
		this.Toggle(false);
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x001D84A5 File Offset: 0x001D66A5
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x06005175 RID: 20853 RVA: 0x001D84B4 File Offset: 0x001D66B4
	public void Toggle(bool is_on)
	{
		base.enabled = is_on;
	}

	// Token: 0x04003711 RID: 14097
	public Material material;

	// Token: 0x04003712 RID: 14098
	public static SimDebugViewCompositor Instance;
}
