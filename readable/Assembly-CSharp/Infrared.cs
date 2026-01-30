using System;
using UnityEngine;

// Token: 0x02000C3C RID: 3132
public class Infrared : MonoBehaviour
{
	// Token: 0x06005EA2 RID: 24226 RVA: 0x0022A78A File Offset: 0x0022898A
	public static void DestroyInstance()
	{
		Infrared.Instance = null;
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x0022A792 File Offset: 0x00228992
	private void Awake()
	{
		Infrared.temperatureParametersId = Shader.PropertyToID("_TemperatureParameters");
		Infrared.Instance = this;
		this.OnResize();
		this.UpdateState();
	}

	// Token: 0x06005EA4 RID: 24228 RVA: 0x0022A7B5 File Offset: 0x002289B5
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.minionTexture);
		Graphics.Blit(source, dest);
	}

	// Token: 0x06005EA5 RID: 24229 RVA: 0x0022A7CC File Offset: 0x002289CC
	private void OnResize()
	{
		if (this.minionTexture != null)
		{
			this.minionTexture.DestroyRenderTexture();
		}
		if (this.cameraTexture != null)
		{
			this.cameraTexture.DestroyRenderTexture();
		}
		int num = 2;
		this.minionTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		this.cameraTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		base.GetComponent<Camera>().targetTexture = this.cameraTexture;
	}

	// Token: 0x06005EA6 RID: 24230 RVA: 0x0022A854 File Offset: 0x00228A54
	public void SetMode(Infrared.Mode mode)
	{
		Vector4 zero;
		if (mode != Infrared.Mode.Disabled)
		{
			if (mode != Infrared.Mode.Disease)
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
			}
			else
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
				GameComps.InfraredVisualizers.ClearOverlayColour();
			}
		}
		else
		{
			zero = Vector4.zero;
		}
		Shader.SetGlobalVector("_ColouredOverlayParameters", zero);
		this.mode = mode;
		this.UpdateState();
	}

	// Token: 0x06005EA7 RID: 24231 RVA: 0x0022A8CC File Offset: 0x00228ACC
	private void UpdateState()
	{
		base.gameObject.SetActive(this.mode > Infrared.Mode.Disabled);
		if (base.gameObject.activeSelf)
		{
			this.Update();
		}
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x0022A8F8 File Offset: 0x00228AF8
	private void Update()
	{
		switch (this.mode)
		{
		case Infrared.Mode.Disabled:
			break;
		case Infrared.Mode.Infrared:
			GameComps.InfraredVisualizers.UpdateTemperature();
			return;
		case Infrared.Mode.Disease:
			GameComps.DiseaseContainers.UpdateOverlayColours();
			break;
		default:
			return;
		}
	}

	// Token: 0x04003EEA RID: 16106
	private RenderTexture minionTexture;

	// Token: 0x04003EEB RID: 16107
	private RenderTexture cameraTexture;

	// Token: 0x04003EEC RID: 16108
	private Infrared.Mode mode;

	// Token: 0x04003EED RID: 16109
	public static int temperatureParametersId;

	// Token: 0x04003EEE RID: 16110
	public static Infrared Instance;

	// Token: 0x02001DE2 RID: 7650
	public enum Mode
	{
		// Token: 0x04008C87 RID: 35975
		Disabled,
		// Token: 0x04008C88 RID: 35976
		Infrared,
		// Token: 0x04008C89 RID: 35977
		Disease
	}
}
