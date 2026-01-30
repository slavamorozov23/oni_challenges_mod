using System;
using UnityEngine;

// Token: 0x02000AE6 RID: 2790
public class LightBufferCompositor : MonoBehaviour
{
	// Token: 0x06005131 RID: 20785 RVA: 0x001D6698 File Offset: 0x001D4898
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/LightBufferCompositor"));
		this.material.SetTexture("_InvalidTex", Assets.instance.invalidAreaTex);
		this.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		this.OnShadersReloaded();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x06005132 RID: 20786 RVA: 0x001D6700 File Offset: 0x001D4900
	private void OnEnable()
	{
		this.OnShadersReloaded();
	}

	// Token: 0x06005133 RID: 20787 RVA: 0x001D6708 File Offset: 0x001D4908
	private void DownSample4x(Texture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06005134 RID: 20788 RVA: 0x001D676A File Offset: 0x001D496A
	[ContextMenu("ToggleParticles")]
	private void ToggleParticles()
	{
		this.particlesEnabled = !this.particlesEnabled;
		this.UpdateMaterialState();
	}

	// Token: 0x06005135 RID: 20789 RVA: 0x001D6781 File Offset: 0x001D4981
	public void SetParticlesEnabled(bool enabled)
	{
		this.particlesEnabled = enabled;
		this.UpdateMaterialState();
	}

	// Token: 0x06005136 RID: 20790 RVA: 0x001D6790 File Offset: 0x001D4990
	private void UpdateMaterialState()
	{
		if (this.particlesEnabled)
		{
			this.material.DisableKeyword("DISABLE_TEMPERATURE_PARTICLES");
			return;
		}
		this.material.EnableKeyword("DISABLE_TEMPERATURE_PARTICLES");
	}

	// Token: 0x06005137 RID: 20791 RVA: 0x001D67BC File Offset: 0x001D49BC
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		RenderTexture renderTexture = null;
		if (PropertyTextures.instance != null)
		{
			Texture texture = PropertyTextures.instance.GetTexture(PropertyTextures.Property.Temperature);
			texture.name = "temperature_tex";
			renderTexture = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(texture, renderTexture, this.blurMaterial);
			Shader.SetGlobalTexture("_BlurredTemperature", renderTexture);
		}
		this.material.SetTexture("_LightBufferTex", LightBuffer.Instance.Texture);
		Graphics.Blit(src, dest, this.material);
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	// Token: 0x06005138 RID: 20792 RVA: 0x001D6858 File Offset: 0x001D4A58
	private void OnShadersReloaded()
	{
		if (this.material != null && Lighting.Instance != null)
		{
			this.material.SetTexture("_EmberTex", Lighting.Instance.Settings.EmberTex);
			this.material.SetTexture("_FrostTex", Lighting.Instance.Settings.FrostTex);
			this.material.SetTexture("_Thermal1Tex", Lighting.Instance.Settings.Thermal1Tex);
			this.material.SetTexture("_Thermal2Tex", Lighting.Instance.Settings.Thermal2Tex);
			this.material.SetTexture("_RadHaze1Tex", Lighting.Instance.Settings.Radiation1Tex);
			this.material.SetTexture("_RadHaze2Tex", Lighting.Instance.Settings.Radiation2Tex);
			this.material.SetTexture("_RadHaze3Tex", Lighting.Instance.Settings.Radiation3Tex);
			this.material.SetTexture("_NoiseTex", Lighting.Instance.Settings.NoiseTex);
		}
	}

	// Token: 0x0400362A RID: 13866
	[SerializeField]
	private Material material;

	// Token: 0x0400362B RID: 13867
	[SerializeField]
	private Material blurMaterial;

	// Token: 0x0400362C RID: 13868
	private bool particlesEnabled = true;
}
