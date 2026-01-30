using System;
using UnityEngine;

// Token: 0x02000AE9 RID: 2793
[ExecuteInEditMode]
public class Lighting : MonoBehaviour
{
	// Token: 0x06005142 RID: 20802 RVA: 0x001D6B88 File Offset: 0x001D4D88
	private void Awake()
	{
		Lighting.Instance = this;
	}

	// Token: 0x06005143 RID: 20803 RVA: 0x001D6B90 File Offset: 0x001D4D90
	private void OnDestroy()
	{
		Lighting.Instance = null;
	}

	// Token: 0x06005144 RID: 20804 RVA: 0x001D6B98 File Offset: 0x001D4D98
	private Color PremultiplyAlpha(Color c)
	{
		return c * c.a;
	}

	// Token: 0x06005145 RID: 20805 RVA: 0x001D6BA6 File Offset: 0x001D4DA6
	private void Start()
	{
		this.UpdateLighting();
	}

	// Token: 0x06005146 RID: 20806 RVA: 0x001D6BAE File Offset: 0x001D4DAE
	private void Update()
	{
		this.UpdateLighting();
	}

	// Token: 0x06005147 RID: 20807 RVA: 0x001D6BB8 File Offset: 0x001D4DB8
	private void UpdateLighting()
	{
		Shader.SetGlobalInt(Lighting._liquidZ, -28);
		Shader.SetGlobalVector(Lighting._DigMapMapParameters, new Vector4(this.Settings.DigMapColour.r, this.Settings.DigMapColour.g, this.Settings.DigMapColour.b, this.Settings.DigMapScale));
		Shader.SetGlobalTexture(Lighting._DigDamageMap, this.Settings.DigDamageMap);
		Shader.SetGlobalTexture(Lighting._StateTransitionMap, this.Settings.StateTransitionMap);
		Shader.SetGlobalColor(Lighting._StateTransitionColor, this.Settings.StateTransitionColor);
		Shader.SetGlobalVector(Lighting._StateTransitionParameters, new Vector4(1f / this.Settings.StateTransitionUVScale, this.Settings.StateTransitionUVOffsetRate.x, this.Settings.StateTransitionUVOffsetRate.y, 0f));
		Shader.SetGlobalTexture(Lighting._FallingSolidMap, this.Settings.FallingSolidMap);
		Shader.SetGlobalColor(Lighting._FallingSolidColor, this.Settings.FallingSolidColor);
		Shader.SetGlobalVector(Lighting._FallingSolidParameters, new Vector4(1f / this.Settings.FallingSolidUVScale, this.Settings.FallingSolidUVOffsetRate.x, this.Settings.FallingSolidUVOffsetRate.y, 0f));
		Shader.SetGlobalColor(Lighting._WaterTrimColor, this.Settings.WaterTrimColor);
		Shader.SetGlobalVector(Lighting._WaterParameters2, new Vector4(this.Settings.WaterTrimSize, this.Settings.WaterAlphaTrimSize, 0f, this.Settings.WaterAlphaThreshold));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters, new Vector4(this.Settings.WaterWaveAmplitude, this.Settings.WaterWaveFrequency, this.Settings.WaterWaveSpeed, 0f));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters2, new Vector4(this.Settings.WaterWaveAmplitude2, this.Settings.WaterWaveFrequency2, this.Settings.WaterWaveSpeed2, 0f));
		Shader.SetGlobalVector(Lighting._WaterDetailParameters, new Vector4(this.Settings.WaterCubeMapScale, this.Settings.WaterDetailTiling, this.Settings.WaterColorScale, this.Settings.WaterDetailTiling2));
		Shader.SetGlobalVector(Lighting._WaterDistortionParameters, new Vector4(this.Settings.WaterDistortionScaleStart, this.Settings.WaterDistortionScaleEnd, this.Settings.WaterDepthColorOpacityStart, this.Settings.WaterDepthColorOpacityEnd));
		Shader.SetGlobalVector(Lighting._BloomParameters, new Vector4(this.Settings.BloomScale, 0f, 0f, 0f));
		Shader.SetGlobalVector(Lighting._LiquidParameters2, new Vector4(this.Settings.LiquidMin, this.Settings.LiquidMax, this.Settings.LiquidCutoff, this.Settings.LiquidTransparency));
		Shader.SetGlobalVector(Lighting._GridParameters, new Vector4(this.Settings.GridLineWidth, this.Settings.GridSize, this.Settings.GridMinIntensity, this.Settings.GridMaxIntensity));
		Shader.SetGlobalColor(Lighting._GridColor, this.Settings.GridColor);
		Shader.SetGlobalVector(Lighting._EdgeGlowParameters, new Vector4(this.Settings.EdgeGlowCutoffStart, this.Settings.EdgeGlowCutoffEnd, this.Settings.EdgeGlowIntensity, 0f));
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, Vector4.one);
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, Vector4.one);
		}
		else
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, new Vector4(this.Settings.substanceEdgeParameters.intensity, this.Settings.substanceEdgeParameters.edgeIntensity, this.Settings.substanceEdgeParameters.directSunlightScale, this.Settings.substanceEdgeParameters.power));
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, new Vector4(this.Settings.tileEdgeParameters.intensity, this.Settings.tileEdgeParameters.edgeIntensity, this.Settings.tileEdgeParameters.directSunlightScale, this.Settings.tileEdgeParameters.power));
		}
		float w = (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() == OverlayModes.Disease.ID) ? 1f : 0f;
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(1f, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		else
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(this.Settings.AnimIntensity, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		Shader.SetGlobalVector(Lighting._GasOpacity, new Vector4(this.Settings.GasMinOpacity, this.Settings.GasMaxOpacity, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DarkenTintBackground, this.Settings.DarkenTints[0]);
		Shader.SetGlobalColor(Lighting._DarkenTintMidground, this.Settings.DarkenTints[1]);
		Shader.SetGlobalColor(Lighting._DarkenTintForeground, this.Settings.DarkenTints[2]);
		Shader.SetGlobalColor(Lighting._BrightenOverlay, this.Settings.BrightenOverlayColour);
		Shader.SetGlobalColor(Lighting._ColdFG, this.PremultiplyAlpha(this.Settings.ColdColours[2]));
		Shader.SetGlobalColor(Lighting._ColdMG, this.PremultiplyAlpha(this.Settings.ColdColours[1]));
		Shader.SetGlobalColor(Lighting._ColdBG, this.PremultiplyAlpha(this.Settings.ColdColours[0]));
		Shader.SetGlobalColor(Lighting._HotFG, this.PremultiplyAlpha(this.Settings.HotColours[2]));
		Shader.SetGlobalColor(Lighting._HotMG, this.PremultiplyAlpha(this.Settings.HotColours[1]));
		Shader.SetGlobalColor(Lighting._HotBG, this.PremultiplyAlpha(this.Settings.HotColours[0]));
		Shader.SetGlobalVector(Lighting._TemperatureParallax, this.Settings.TemperatureParallax);
		Shader.SetGlobalVector(Lighting._ColdUVOffset1, new Vector4(this.Settings.ColdBGUVOffset.x, this.Settings.ColdBGUVOffset.y, this.Settings.ColdMGUVOffset.x, this.Settings.ColdMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._ColdUVOffset2, new Vector4(this.Settings.ColdFGUVOffset.x, this.Settings.ColdFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalVector(Lighting._HotUVOffset1, new Vector4(this.Settings.HotBGUVOffset.x, this.Settings.HotBGUVOffset.y, this.Settings.HotMGUVOffset.x, this.Settings.HotMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._HotUVOffset2, new Vector4(this.Settings.HotFGUVOffset.x, this.Settings.HotFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DustColour, this.PremultiplyAlpha(this.Settings.DustColour));
		Shader.SetGlobalVector(Lighting._DustInfo, new Vector4(this.Settings.DustScale, this.Settings.DustMovement.x, this.Settings.DustMovement.y, this.Settings.DustMovement.z));
		Shader.SetGlobalTexture(Lighting._DustTex, this.Settings.DustTex);
		Shader.SetGlobalVector(Lighting._DebugShowInfo, new Vector4(this.Settings.ShowDust, this.Settings.ShowGas, this.Settings.ShowShadow, this.Settings.ShowTemperature));
		Shader.SetGlobalVector(Lighting._HeatHazeParameters, this.Settings.HeatHazeParameters);
		Shader.SetGlobalTexture(Lighting._HeatHazeTexture, this.Settings.HeatHazeTexture);
		Shader.SetGlobalVector(Lighting._ShineParams, new Vector4(this.Settings.ShineCenter.x, this.Settings.ShineCenter.y, this.Settings.ShineRange.x, this.Settings.ShineRange.y));
		Shader.SetGlobalVector(Lighting._ShineParams2, new Vector4(this.Settings.ShineZoomSpeed, 0f, 0f, 0f));
		Shader.SetGlobalFloat(Lighting._WorldZoneGasBlend, this.Settings.WorldZoneGasBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneLiquidBlend, this.Settings.WorldZoneLiquidBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneForegroundBlend, this.Settings.WorldZoneForegroundBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneSimpleAnimBlend, this.Settings.WorldZoneSimpleAnimBlend);
		Shader.SetGlobalColor(Lighting._CharacterLitColour, this.Settings.characterLighting.litColour);
		Shader.SetGlobalColor(Lighting._CharacterUnlitColour, this.Settings.characterLighting.unlitColour);
		Shader.SetGlobalTexture(Lighting._BuildingDamagedTex, this.Settings.BuildingDamagedTex);
		Shader.SetGlobalVector(Lighting._BuildingDamagedUVParameters, this.Settings.BuildingDamagedUVParameters);
		Shader.SetGlobalTexture(Lighting._DiseaseOverlayTex, this.Settings.DiseaseOverlayTex);
		Shader.SetGlobalVector(Lighting._DiseaseOverlayTexInfo, this.Settings.DiseaseOverlayTexInfo);
		if (this.Settings.ShowRadiation)
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, this.PremultiplyAlpha(this.Settings.RadColor));
		}
		else
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, Color.clear);
		}
		Shader.SetGlobalVector(Lighting._RadUVOffset1, new Vector4(this.Settings.Rad1UVOffset.x, this.Settings.Rad1UVOffset.y, this.Settings.Rad2UVOffset.x, this.Settings.Rad2UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVOffset2, new Vector4(this.Settings.Rad3UVOffset.x, this.Settings.Rad3UVOffset.y, this.Settings.Rad4UVOffset.x, this.Settings.Rad4UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVScales, new Vector4(1f / this.Settings.RadUVScales.x, 1f / this.Settings.RadUVScales.y, 1f / this.Settings.RadUVScales.z, 1f / this.Settings.RadUVScales.w));
		Shader.SetGlobalVector(Lighting._RadRange1, new Vector4(this.Settings.Rad1Range.x, this.Settings.Rad1Range.y, this.Settings.Rad2Range.x, this.Settings.Rad2Range.y));
		Shader.SetGlobalVector(Lighting._RadRange2, new Vector4(this.Settings.Rad3Range.x, this.Settings.Rad3Range.y, this.Settings.Rad4Range.x, this.Settings.Rad4Range.y));
		if (LightBuffer.Instance != null && LightBuffer.Instance.Texture != null)
		{
			Shader.SetGlobalTexture(Lighting._LightBufferTex, LightBuffer.Instance.Texture);
		}
	}

	// Token: 0x04003634 RID: 13876
	public global::LightingSettings Settings;

	// Token: 0x04003635 RID: 13877
	public static Lighting Instance;

	// Token: 0x04003636 RID: 13878
	[NonSerialized]
	public bool disableLighting;

	// Token: 0x04003637 RID: 13879
	private static int _liquidZ = Shader.PropertyToID("_LiquidZ");

	// Token: 0x04003638 RID: 13880
	private static int _DigMapMapParameters = Shader.PropertyToID("_DigMapMapParameters");

	// Token: 0x04003639 RID: 13881
	private static int _DigDamageMap = Shader.PropertyToID("_DigDamageMap");

	// Token: 0x0400363A RID: 13882
	private static int _StateTransitionMap = Shader.PropertyToID("_StateTransitionMap");

	// Token: 0x0400363B RID: 13883
	private static int _StateTransitionColor = Shader.PropertyToID("_StateTransitionColor");

	// Token: 0x0400363C RID: 13884
	private static int _StateTransitionParameters = Shader.PropertyToID("_StateTransitionParameters");

	// Token: 0x0400363D RID: 13885
	private static int _FallingSolidMap = Shader.PropertyToID("_FallingSolidMap");

	// Token: 0x0400363E RID: 13886
	private static int _FallingSolidColor = Shader.PropertyToID("_FallingSolidColor");

	// Token: 0x0400363F RID: 13887
	private static int _FallingSolidParameters = Shader.PropertyToID("_FallingSolidParameters");

	// Token: 0x04003640 RID: 13888
	private static int _WaterTrimColor = Shader.PropertyToID("_WaterTrimColor");

	// Token: 0x04003641 RID: 13889
	private static int _WaterParameters2 = Shader.PropertyToID("_WaterParameters2");

	// Token: 0x04003642 RID: 13890
	private static int _WaterWaveParameters = Shader.PropertyToID("_WaterWaveParameters");

	// Token: 0x04003643 RID: 13891
	private static int _WaterWaveParameters2 = Shader.PropertyToID("_WaterWaveParameters2");

	// Token: 0x04003644 RID: 13892
	private static int _WaterDetailParameters = Shader.PropertyToID("_WaterDetailParameters");

	// Token: 0x04003645 RID: 13893
	private static int _WaterDistortionParameters = Shader.PropertyToID("_WaterDistortionParameters");

	// Token: 0x04003646 RID: 13894
	private static int _BloomParameters = Shader.PropertyToID("_BloomParameters");

	// Token: 0x04003647 RID: 13895
	private static int _LiquidParameters2 = Shader.PropertyToID("_LiquidParameters2");

	// Token: 0x04003648 RID: 13896
	private static int _GridParameters = Shader.PropertyToID("_GridParameters");

	// Token: 0x04003649 RID: 13897
	private static int _GridColor = Shader.PropertyToID("_GridColor");

	// Token: 0x0400364A RID: 13898
	private static int _EdgeGlowParameters = Shader.PropertyToID("_EdgeGlowParameters");

	// Token: 0x0400364B RID: 13899
	private static int _SubstanceParameters = Shader.PropertyToID("_SubstanceParameters");

	// Token: 0x0400364C RID: 13900
	private static int _TileEdgeParameters = Shader.PropertyToID("_TileEdgeParameters");

	// Token: 0x0400364D RID: 13901
	private static int _AnimParameters = Shader.PropertyToID("_AnimParameters");

	// Token: 0x0400364E RID: 13902
	private static int _GasOpacity = Shader.PropertyToID("_GasOpacity");

	// Token: 0x0400364F RID: 13903
	private static int _DarkenTintBackground = Shader.PropertyToID("_DarkenTintBackground");

	// Token: 0x04003650 RID: 13904
	private static int _DarkenTintMidground = Shader.PropertyToID("_DarkenTintMidground");

	// Token: 0x04003651 RID: 13905
	private static int _DarkenTintForeground = Shader.PropertyToID("_DarkenTintForeground");

	// Token: 0x04003652 RID: 13906
	private static int _BrightenOverlay = Shader.PropertyToID("_BrightenOverlay");

	// Token: 0x04003653 RID: 13907
	private static int _ColdFG = Shader.PropertyToID("_ColdFG");

	// Token: 0x04003654 RID: 13908
	private static int _ColdMG = Shader.PropertyToID("_ColdMG");

	// Token: 0x04003655 RID: 13909
	private static int _ColdBG = Shader.PropertyToID("_ColdBG");

	// Token: 0x04003656 RID: 13910
	private static int _HotFG = Shader.PropertyToID("_HotFG");

	// Token: 0x04003657 RID: 13911
	private static int _HotMG = Shader.PropertyToID("_HotMG");

	// Token: 0x04003658 RID: 13912
	private static int _HotBG = Shader.PropertyToID("_HotBG");

	// Token: 0x04003659 RID: 13913
	private static int _TemperatureParallax = Shader.PropertyToID("_TemperatureParallax");

	// Token: 0x0400365A RID: 13914
	private static int _ColdUVOffset1 = Shader.PropertyToID("_ColdUVOffset1");

	// Token: 0x0400365B RID: 13915
	private static int _ColdUVOffset2 = Shader.PropertyToID("_ColdUVOffset2");

	// Token: 0x0400365C RID: 13916
	private static int _HotUVOffset1 = Shader.PropertyToID("_HotUVOffset1");

	// Token: 0x0400365D RID: 13917
	private static int _HotUVOffset2 = Shader.PropertyToID("_HotUVOffset2");

	// Token: 0x0400365E RID: 13918
	private static int _DustColour = Shader.PropertyToID("_DustColour");

	// Token: 0x0400365F RID: 13919
	private static int _DustInfo = Shader.PropertyToID("_DustInfo");

	// Token: 0x04003660 RID: 13920
	private static int _DustTex = Shader.PropertyToID("_DustTex");

	// Token: 0x04003661 RID: 13921
	private static int _DebugShowInfo = Shader.PropertyToID("_DebugShowInfo");

	// Token: 0x04003662 RID: 13922
	private static int _HeatHazeParameters = Shader.PropertyToID("_HeatHazeParameters");

	// Token: 0x04003663 RID: 13923
	private static int _HeatHazeTexture = Shader.PropertyToID("_HeatHazeTexture");

	// Token: 0x04003664 RID: 13924
	private static int _ShineParams = Shader.PropertyToID("_ShineParams");

	// Token: 0x04003665 RID: 13925
	private static int _ShineParams2 = Shader.PropertyToID("_ShineParams2");

	// Token: 0x04003666 RID: 13926
	private static int _WorldZoneGasBlend = Shader.PropertyToID("_WorldZoneGasBlend");

	// Token: 0x04003667 RID: 13927
	private static int _WorldZoneLiquidBlend = Shader.PropertyToID("_WorldZoneLiquidBlend");

	// Token: 0x04003668 RID: 13928
	private static int _WorldZoneForegroundBlend = Shader.PropertyToID("_WorldZoneForegroundBlend");

	// Token: 0x04003669 RID: 13929
	private static int _WorldZoneSimpleAnimBlend = Shader.PropertyToID("_WorldZoneSimpleAnimBlend");

	// Token: 0x0400366A RID: 13930
	private static int _CharacterLitColour = Shader.PropertyToID("_CharacterLitColour");

	// Token: 0x0400366B RID: 13931
	private static int _CharacterUnlitColour = Shader.PropertyToID("_CharacterUnlitColour");

	// Token: 0x0400366C RID: 13932
	private static int _BuildingDamagedTex = Shader.PropertyToID("_BuildingDamagedTex");

	// Token: 0x0400366D RID: 13933
	private static int _BuildingDamagedUVParameters = Shader.PropertyToID("_BuildingDamagedUVParameters");

	// Token: 0x0400366E RID: 13934
	private static int _DiseaseOverlayTex = Shader.PropertyToID("_DiseaseOverlayTex");

	// Token: 0x0400366F RID: 13935
	private static int _DiseaseOverlayTexInfo = Shader.PropertyToID("_DiseaseOverlayTexInfo");

	// Token: 0x04003670 RID: 13936
	private static int _RadHazeColor = Shader.PropertyToID("_RadHazeColor");

	// Token: 0x04003671 RID: 13937
	private static int _RadUVOffset1 = Shader.PropertyToID("_RadUVOffset1");

	// Token: 0x04003672 RID: 13938
	private static int _RadUVOffset2 = Shader.PropertyToID("_RadUVOffset2");

	// Token: 0x04003673 RID: 13939
	private static int _RadUVScales = Shader.PropertyToID("_RadUVScales");

	// Token: 0x04003674 RID: 13940
	private static int _RadRange1 = Shader.PropertyToID("_RadRange1");

	// Token: 0x04003675 RID: 13941
	private static int _RadRange2 = Shader.PropertyToID("_RadRange2");

	// Token: 0x04003676 RID: 13942
	private static int _LightBufferTex = Shader.PropertyToID("_LightBufferTex");
}
