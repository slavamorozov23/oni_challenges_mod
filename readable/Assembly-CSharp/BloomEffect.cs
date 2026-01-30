using System;
using UnityEngine;

// Token: 0x02000C39 RID: 3129
public class BloomEffect : MonoBehaviour
{
	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06005E95 RID: 24213 RVA: 0x0022A4CB File Offset: 0x002286CB
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.blurShader);
				this.m_Material.hideFlags = HideFlags.DontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06005E96 RID: 24214 RVA: 0x0022A4FF File Offset: 0x002286FF
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06005E97 RID: 24215 RVA: 0x0022A51C File Offset: 0x0022871C
	protected void Start()
	{
		if (!this.blurShader || !this.material.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.BloomMaskMaterial = new Material(Shader.Find("Klei/PostFX/BloomMask"));
		this.BloomCompositeMaterial = new Material(Shader.Find("Klei/PostFX/BloomComposite"));
	}

	// Token: 0x06005E98 RID: 24216 RVA: 0x0022A57C File Offset: 0x0022877C
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06005E99 RID: 24217 RVA: 0x0022A5E8 File Offset: 0x002287E8
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06005E9A RID: 24218 RVA: 0x0022A64C File Offset: 0x0022884C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		temporary.name = "bloom_source";
		Graphics.Blit(source, temporary, this.BloomMaskMaterial);
		int width = Math.Max(source.width / 4, 4);
		int height = Math.Max(source.height / 4, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
		renderTexture.name = "bloom_downsampled";
		this.DownSample4x(temporary, renderTexture);
		RenderTexture.ReleaseTemporary(temporary);
		for (int i = 0; i < this.iterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
			temporary2.name = "bloom_blurred";
			this.FourTapCone(renderTexture, temporary2, i);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary2;
		}
		this.BloomCompositeMaterial.SetTexture("_BloomTex", renderTexture);
		Graphics.Blit(source, destination, this.BloomCompositeMaterial);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04003EE3 RID: 16099
	private Material BloomMaskMaterial;

	// Token: 0x04003EE4 RID: 16100
	private Material BloomCompositeMaterial;

	// Token: 0x04003EE5 RID: 16101
	public int iterations = 3;

	// Token: 0x04003EE6 RID: 16102
	public float blurSpread = 0.6f;

	// Token: 0x04003EE7 RID: 16103
	public Shader blurShader;

	// Token: 0x04003EE8 RID: 16104
	private Material m_Material;
}
