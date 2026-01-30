using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ADD RID: 2781
public class CustomActiveScreenPostProcessingEffects : KMonoBehaviour
{
	// Token: 0x060050D9 RID: 20697 RVA: 0x001D43AC File Offset: 0x001D25AC
	public void RegisterEffect(Func<RenderTexture, Material> effectFn)
	{
		this.ActiveEffects.Add(effectFn);
	}

	// Token: 0x060050DA RID: 20698 RVA: 0x001D43BA File Offset: 0x001D25BA
	public void UnregisterEffect(Func<RenderTexture, Material> effectFn)
	{
		this.ActiveEffects.Remove(effectFn);
	}

	// Token: 0x060050DB RID: 20699 RVA: 0x001D43CC File Offset: 0x001D25CC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.ActiveEffects.Count > 0)
		{
			this.CheckTemporaryRenderTextureValidity(ref this.previousSource, source);
			this.CheckTemporaryRenderTextureValidity(ref this.previousDestination, source);
			Graphics.Blit(source, this.previousSource);
			foreach (Func<RenderTexture, Material> func in this.ActiveEffects)
			{
				Graphics.Blit(this.previousSource, this.previousDestination, func(source));
				this.previousSource.DiscardContents();
				Graphics.Blit(this.previousDestination, this.previousSource);
			}
			Graphics.Blit(this.previousSource, destination);
			return;
		}
		Graphics.Blit(source, destination);
	}

	// Token: 0x060050DC RID: 20700 RVA: 0x001D4498 File Offset: 0x001D2698
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.previousSource.Release();
		this.previousDestination.Release();
	}

	// Token: 0x060050DD RID: 20701 RVA: 0x001D44B8 File Offset: 0x001D26B8
	private void CheckTemporaryRenderTextureValidity(ref RenderTexture temporaryTexture, RenderTexture source)
	{
		if (temporaryTexture == null || temporaryTexture.width != source.width || temporaryTexture.height != source.height || temporaryTexture.depth != source.depth || temporaryTexture.format != source.format)
		{
			if (temporaryTexture != null)
			{
				temporaryTexture.Release();
			}
			temporaryTexture = RenderTexture.GetTemporary(source.width, source.height, source.depth, source.format);
		}
	}

	// Token: 0x040035F6 RID: 13814
	private List<Func<RenderTexture, Material>> ActiveEffects = new List<Func<RenderTexture, Material>>();

	// Token: 0x040035F7 RID: 13815
	private RenderTexture previousSource;

	// Token: 0x040035F8 RID: 13816
	private RenderTexture previousDestination;
}
