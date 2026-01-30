using System;
using UnityEngine;

// Token: 0x02000AE3 RID: 2787
public class KAnimActivePostProcessingEffects : KMonoBehaviour
{
	// Token: 0x060050FC RID: 20732 RVA: 0x001D58B4 File Offset: 0x001D3AB4
	public void EnableEffect(KAnimConverter.PostProcessingEffects effect_flag)
	{
		this.currentActiveEffects |= effect_flag;
	}

	// Token: 0x060050FD RID: 20733 RVA: 0x001D58C4 File Offset: 0x001D3AC4
	public void DisableEffect(KAnimConverter.PostProcessingEffects effect_flag)
	{
		if (this.IsEffectActive(effect_flag))
		{
			this.currentActiveEffects ^= effect_flag;
		}
	}

	// Token: 0x060050FE RID: 20734 RVA: 0x001D58DD File Offset: 0x001D3ADD
	public bool IsEffectActive(KAnimConverter.PostProcessingEffects effect_flag)
	{
		return (this.currentActiveEffects & effect_flag) > (KAnimConverter.PostProcessingEffects)0;
	}

	// Token: 0x060050FF RID: 20735 RVA: 0x001D58EA File Offset: 0x001D3AEA
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination);
		if (this.currentActiveEffects != (KAnimConverter.PostProcessingEffects)0)
		{
			KAnimBatchManager.Instance().RenderKAnimPostProcessingEffects(this.currentActiveEffects);
		}
	}

	// Token: 0x04003605 RID: 13829
	private KAnimConverter.PostProcessingEffects currentActiveEffects;
}
