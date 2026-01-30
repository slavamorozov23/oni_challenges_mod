using System;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
public class LightingSettings : ScriptableObject
{
	// Token: 0x0400367E RID: 13950
	[Header("Global")]
	public bool UpdateLightSettings;

	// Token: 0x0400367F RID: 13951
	public float BloomScale;

	// Token: 0x04003680 RID: 13952
	public Color32 LightColour = Color.white;

	// Token: 0x04003681 RID: 13953
	[Header("Digging")]
	public float DigMapScale;

	// Token: 0x04003682 RID: 13954
	public Color DigMapColour;

	// Token: 0x04003683 RID: 13955
	public Texture2D DigDamageMap;

	// Token: 0x04003684 RID: 13956
	[Header("State Transition")]
	public Texture2D StateTransitionMap;

	// Token: 0x04003685 RID: 13957
	public Color StateTransitionColor;

	// Token: 0x04003686 RID: 13958
	public float StateTransitionUVScale;

	// Token: 0x04003687 RID: 13959
	public Vector2 StateTransitionUVOffsetRate;

	// Token: 0x04003688 RID: 13960
	[Header("Falling Solids")]
	public Texture2D FallingSolidMap;

	// Token: 0x04003689 RID: 13961
	public Color FallingSolidColor;

	// Token: 0x0400368A RID: 13962
	public float FallingSolidUVScale;

	// Token: 0x0400368B RID: 13963
	public Vector2 FallingSolidUVOffsetRate;

	// Token: 0x0400368C RID: 13964
	[Header("Metal Shine")]
	public Vector2 ShineCenter;

	// Token: 0x0400368D RID: 13965
	public Vector2 ShineRange;

	// Token: 0x0400368E RID: 13966
	public float ShineZoomSpeed;

	// Token: 0x0400368F RID: 13967
	[Header("Water")]
	public Color WaterTrimColor;

	// Token: 0x04003690 RID: 13968
	public float WaterTrimSize;

	// Token: 0x04003691 RID: 13969
	public float WaterAlphaTrimSize;

	// Token: 0x04003692 RID: 13970
	public float WaterAlphaThreshold;

	// Token: 0x04003693 RID: 13971
	public float WaterCubesAlphaThreshold;

	// Token: 0x04003694 RID: 13972
	public float WaterWaveAmplitude;

	// Token: 0x04003695 RID: 13973
	public float WaterWaveFrequency;

	// Token: 0x04003696 RID: 13974
	public float WaterWaveSpeed;

	// Token: 0x04003697 RID: 13975
	public float WaterDetailSpeed;

	// Token: 0x04003698 RID: 13976
	public float WaterDetailTiling;

	// Token: 0x04003699 RID: 13977
	public float WaterDetailTiling2;

	// Token: 0x0400369A RID: 13978
	public Vector2 WaterDetailDirection;

	// Token: 0x0400369B RID: 13979
	public float WaterWaveAmplitude2;

	// Token: 0x0400369C RID: 13980
	public float WaterWaveFrequency2;

	// Token: 0x0400369D RID: 13981
	public float WaterWaveSpeed2;

	// Token: 0x0400369E RID: 13982
	public float WaterCubeMapScale;

	// Token: 0x0400369F RID: 13983
	public float WaterColorScale;

	// Token: 0x040036A0 RID: 13984
	public float WaterDistortionScaleStart;

	// Token: 0x040036A1 RID: 13985
	public float WaterDistortionScaleEnd;

	// Token: 0x040036A2 RID: 13986
	public float WaterDepthColorOpacityStart;

	// Token: 0x040036A3 RID: 13987
	public float WaterDepthColorOpacityEnd;

	// Token: 0x040036A4 RID: 13988
	[Header("Liquid")]
	public float LiquidMin;

	// Token: 0x040036A5 RID: 13989
	public float LiquidMax;

	// Token: 0x040036A6 RID: 13990
	public float LiquidCutoff;

	// Token: 0x040036A7 RID: 13991
	public float LiquidTransparency;

	// Token: 0x040036A8 RID: 13992
	public float LiquidAmountOffset;

	// Token: 0x040036A9 RID: 13993
	public float LiquidMaxMass;

	// Token: 0x040036AA RID: 13994
	[Header("Grid")]
	public float GridLineWidth;

	// Token: 0x040036AB RID: 13995
	public float GridSize;

	// Token: 0x040036AC RID: 13996
	public float GridMaxIntensity;

	// Token: 0x040036AD RID: 13997
	public float GridMinIntensity;

	// Token: 0x040036AE RID: 13998
	public Color GridColor;

	// Token: 0x040036AF RID: 13999
	[Header("Terrain")]
	public float EdgeGlowCutoffStart;

	// Token: 0x040036B0 RID: 14000
	public float EdgeGlowCutoffEnd;

	// Token: 0x040036B1 RID: 14001
	public float EdgeGlowIntensity;

	// Token: 0x040036B2 RID: 14002
	public int BackgroundLayers;

	// Token: 0x040036B3 RID: 14003
	public float BackgroundBaseParallax;

	// Token: 0x040036B4 RID: 14004
	public float BackgroundLayerParallax;

	// Token: 0x040036B5 RID: 14005
	public float BackgroundDarkening;

	// Token: 0x040036B6 RID: 14006
	public float BackgroundClip;

	// Token: 0x040036B7 RID: 14007
	public float BackgroundUVScale;

	// Token: 0x040036B8 RID: 14008
	public global::LightingSettings.EdgeLighting substanceEdgeParameters;

	// Token: 0x040036B9 RID: 14009
	public global::LightingSettings.EdgeLighting tileEdgeParameters;

	// Token: 0x040036BA RID: 14010
	public float AnimIntensity;

	// Token: 0x040036BB RID: 14011
	public float GasMinOpacity;

	// Token: 0x040036BC RID: 14012
	public float GasMaxOpacity;

	// Token: 0x040036BD RID: 14013
	public Color[] DarkenTints;

	// Token: 0x040036BE RID: 14014
	public global::LightingSettings.LightingColours characterLighting;

	// Token: 0x040036BF RID: 14015
	public Color BrightenOverlayColour;

	// Token: 0x040036C0 RID: 14016
	public Color[] ColdColours;

	// Token: 0x040036C1 RID: 14017
	public Color[] HotColours;

	// Token: 0x040036C2 RID: 14018
	[Header("Temperature Overlay Effects")]
	public Vector4 TemperatureParallax;

	// Token: 0x040036C3 RID: 14019
	public Texture2D EmberTex;

	// Token: 0x040036C4 RID: 14020
	public Texture2D FrostTex;

	// Token: 0x040036C5 RID: 14021
	public Texture2D Thermal1Tex;

	// Token: 0x040036C6 RID: 14022
	public Texture2D Thermal2Tex;

	// Token: 0x040036C7 RID: 14023
	public Vector2 ColdFGUVOffset;

	// Token: 0x040036C8 RID: 14024
	public Vector2 ColdMGUVOffset;

	// Token: 0x040036C9 RID: 14025
	public Vector2 ColdBGUVOffset;

	// Token: 0x040036CA RID: 14026
	public Vector2 HotFGUVOffset;

	// Token: 0x040036CB RID: 14027
	public Vector2 HotMGUVOffset;

	// Token: 0x040036CC RID: 14028
	public Vector2 HotBGUVOffset;

	// Token: 0x040036CD RID: 14029
	public Texture2D DustTex;

	// Token: 0x040036CE RID: 14030
	public Color DustColour;

	// Token: 0x040036CF RID: 14031
	public float DustScale;

	// Token: 0x040036D0 RID: 14032
	public Vector3 DustMovement;

	// Token: 0x040036D1 RID: 14033
	public float ShowGas;

	// Token: 0x040036D2 RID: 14034
	public float ShowTemperature;

	// Token: 0x040036D3 RID: 14035
	public float ShowDust;

	// Token: 0x040036D4 RID: 14036
	public float ShowShadow;

	// Token: 0x040036D5 RID: 14037
	public Vector4 HeatHazeParameters;

	// Token: 0x040036D6 RID: 14038
	public Texture2D HeatHazeTexture;

	// Token: 0x040036D7 RID: 14039
	[Header("Biome")]
	public float WorldZoneGasBlend;

	// Token: 0x040036D8 RID: 14040
	public float WorldZoneLiquidBlend;

	// Token: 0x040036D9 RID: 14041
	public float WorldZoneForegroundBlend;

	// Token: 0x040036DA RID: 14042
	public float WorldZoneSimpleAnimBlend;

	// Token: 0x040036DB RID: 14043
	public float WorldZoneAnimBlend;

	// Token: 0x040036DC RID: 14044
	[Header("FX")]
	public Color32 SmokeDamageTint;

	// Token: 0x040036DD RID: 14045
	[Header("Building Damage")]
	public Texture2D BuildingDamagedTex;

	// Token: 0x040036DE RID: 14046
	public Vector4 BuildingDamagedUVParameters;

	// Token: 0x040036DF RID: 14047
	[Header("Disease")]
	public Texture2D DiseaseOverlayTex;

	// Token: 0x040036E0 RID: 14048
	public Vector4 DiseaseOverlayTexInfo;

	// Token: 0x040036E1 RID: 14049
	[Header("Conduits")]
	public ConduitFlowVisualizer.Tuning GasConduit;

	// Token: 0x040036E2 RID: 14050
	public ConduitFlowVisualizer.Tuning LiquidConduit;

	// Token: 0x040036E3 RID: 14051
	public SolidConduitFlowVisualizer.Tuning SolidConduit;

	// Token: 0x040036E4 RID: 14052
	[Header("Radiation Overlay")]
	public bool ShowRadiation;

	// Token: 0x040036E5 RID: 14053
	public Texture2D Radiation1Tex;

	// Token: 0x040036E6 RID: 14054
	public Texture2D Radiation2Tex;

	// Token: 0x040036E7 RID: 14055
	public Texture2D Radiation3Tex;

	// Token: 0x040036E8 RID: 14056
	public Texture2D Radiation4Tex;

	// Token: 0x040036E9 RID: 14057
	public Texture2D NoiseTex;

	// Token: 0x040036EA RID: 14058
	public Color RadColor;

	// Token: 0x040036EB RID: 14059
	public Vector2 Rad1UVOffset;

	// Token: 0x040036EC RID: 14060
	public Vector2 Rad2UVOffset;

	// Token: 0x040036ED RID: 14061
	public Vector2 Rad3UVOffset;

	// Token: 0x040036EE RID: 14062
	public Vector2 Rad4UVOffset;

	// Token: 0x040036EF RID: 14063
	public Vector4 RadUVScales;

	// Token: 0x040036F0 RID: 14064
	public Vector2 Rad1Range;

	// Token: 0x040036F1 RID: 14065
	public Vector2 Rad2Range;

	// Token: 0x040036F2 RID: 14066
	public Vector2 Rad3Range;

	// Token: 0x040036F3 RID: 14067
	public Vector2 Rad4Range;

	// Token: 0x02001C2B RID: 7211
	[Serializable]
	public struct EdgeLighting
	{
		// Token: 0x04008715 RID: 34581
		public float intensity;

		// Token: 0x04008716 RID: 34582
		public float edgeIntensity;

		// Token: 0x04008717 RID: 34583
		public float directSunlightScale;

		// Token: 0x04008718 RID: 34584
		public float power;
	}

	// Token: 0x02001C2C RID: 7212
	public enum TintLayers
	{
		// Token: 0x0400871A RID: 34586
		Background,
		// Token: 0x0400871B RID: 34587
		Midground,
		// Token: 0x0400871C RID: 34588
		Foreground,
		// Token: 0x0400871D RID: 34589
		NumLayers
	}

	// Token: 0x02001C2D RID: 7213
	[Serializable]
	public struct LightingColours
	{
		// Token: 0x0400871E RID: 34590
		public Color32 litColour;

		// Token: 0x0400871F RID: 34591
		public Color32 unlitColour;
	}
}
