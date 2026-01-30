using System;
using UnityEngine;

// Token: 0x0200095A RID: 2394
[AddComponentMenu("KMonoBehaviour/scripts/FlowOffsetRenderer")]
public class FlowOffsetRenderer : KMonoBehaviour
{
	// Token: 0x060042EB RID: 17131 RVA: 0x0017A3C8 File Offset: 0x001785C8
	protected override void OnSpawn()
	{
		this.FlowMaterial = new Material(Shader.Find("Klei/Flow"));
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.DoUpdate(0.1f);
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x0017A424 File Offset: 0x00178624
	private void OnResize()
	{
		for (int i = 0; i < this.OffsetTextures.Length; i++)
		{
			if (this.OffsetTextures[i] != null)
			{
				this.OffsetTextures[i].DestroyRenderTexture();
			}
			this.OffsetTextures[i] = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
			this.OffsetTextures[i].filterMode = FilterMode.Bilinear;
			this.OffsetTextures[i].name = "FlowOffsetTexture";
		}
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x0017A4A0 File Offset: 0x001786A0
	private void LateUpdate()
	{
		if ((Time.deltaTime > 0f && Time.timeScale > 0f) || this.forceUpdate)
		{
			float num = Time.deltaTime / Time.timeScale;
			this.DoUpdate(num * Time.timeScale / 4f + num * 0.5f);
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x0017A4F4 File Offset: 0x001786F4
	private void DoUpdate(float dt)
	{
		this.CurrentTime += dt;
		float num = this.CurrentTime * this.PhaseMultiplier;
		num -= (float)((int)num);
		float num2 = num - (float)((int)num);
		float y = 1f;
		if (num2 <= this.GasPhase0)
		{
			y = 0f;
		}
		this.GasPhase0 = num2;
		float z = 1f;
		float num3 = num + 0.5f - (float)((int)(num + 0.5f));
		if (num3 <= this.GasPhase1)
		{
			z = 0f;
		}
		this.GasPhase1 = num3;
		Shader.SetGlobalVector(this.ParametersName, new Vector4(this.GasPhase0, 0f, 0f, 0f));
		Shader.SetGlobalVector("_NoiseParameters", new Vector4(this.NoiseInfluence, this.NoiseScale, 0f, 0f));
		RenderTexture renderTexture = this.OffsetTextures[this.OffsetIdx];
		this.OffsetIdx = (this.OffsetIdx + 1) % 2;
		RenderTexture renderTexture2 = this.OffsetTextures[this.OffsetIdx];
		Material flowMaterial = this.FlowMaterial;
		flowMaterial.SetTexture("_PreviousOffsetTex", renderTexture);
		flowMaterial.SetVector("_FlowParameters", new Vector4(Time.deltaTime * this.OffsetSpeed, y, z, 0f));
		flowMaterial.SetVector("_MinFlow", new Vector4(this.MinFlow0.x, this.MinFlow0.y, this.MinFlow1.x, this.MinFlow1.y));
		flowMaterial.SetVector("_VisibleArea", new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells));
		flowMaterial.SetVector("_LiquidGasMask", new Vector4(this.LiquidGasMask.x, this.LiquidGasMask.y, 0f, 0f));
		Graphics.Blit(renderTexture, renderTexture2, flowMaterial);
		Shader.SetGlobalTexture(this.OffsetTextureName, renderTexture2);
	}

	// Token: 0x04002A12 RID: 10770
	private float GasPhase0;

	// Token: 0x04002A13 RID: 10771
	private float GasPhase1;

	// Token: 0x04002A14 RID: 10772
	public float PhaseMultiplier;

	// Token: 0x04002A15 RID: 10773
	public float NoiseInfluence;

	// Token: 0x04002A16 RID: 10774
	public float NoiseScale;

	// Token: 0x04002A17 RID: 10775
	public float OffsetSpeed;

	// Token: 0x04002A18 RID: 10776
	public string OffsetTextureName;

	// Token: 0x04002A19 RID: 10777
	public string ParametersName;

	// Token: 0x04002A1A RID: 10778
	public Vector2 MinFlow0;

	// Token: 0x04002A1B RID: 10779
	public Vector2 MinFlow1;

	// Token: 0x04002A1C RID: 10780
	public Vector2 LiquidGasMask;

	// Token: 0x04002A1D RID: 10781
	[SerializeField]
	private Material FlowMaterial;

	// Token: 0x04002A1E RID: 10782
	[SerializeField]
	private bool forceUpdate;

	// Token: 0x04002A1F RID: 10783
	private TextureLerper FlowLerper;

	// Token: 0x04002A20 RID: 10784
	public RenderTexture[] OffsetTextures = new RenderTexture[2];

	// Token: 0x04002A21 RID: 10785
	private int OffsetIdx;

	// Token: 0x04002A22 RID: 10786
	private float CurrentTime;
}
