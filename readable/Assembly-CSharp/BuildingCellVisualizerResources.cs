using System;
using UnityEngine;

// Token: 0x02000C3E RID: 3134
public class BuildingCellVisualizerResources : ScriptableObject
{
	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x06005EAD RID: 24237 RVA: 0x0022A9C0 File Offset: 0x00228BC0
	public string heatSourceAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06005EAE RID: 24238 RVA: 0x0022A9C7 File Offset: 0x00228BC7
	public string heatAnimName
	{
		get
		{
			return "heatfx_a";
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06005EAF RID: 24239 RVA: 0x0022A9CE File Offset: 0x00228BCE
	public string heatSinkAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06005EB0 RID: 24240 RVA: 0x0022A9D5 File Offset: 0x00228BD5
	public string heatSinkAnimName
	{
		get
		{
			return "heatfx_b";
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x0022A9DC File Offset: 0x00228BDC
	// (set) Token: 0x06005EB2 RID: 24242 RVA: 0x0022A9E4 File Offset: 0x00228BE4
	public Material backgroundMaterial { get; set; }

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06005EB3 RID: 24243 RVA: 0x0022A9ED File Offset: 0x00228BED
	// (set) Token: 0x06005EB4 RID: 24244 RVA: 0x0022A9F5 File Offset: 0x00228BF5
	public Material iconBackgroundMaterial { get; set; }

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06005EB5 RID: 24245 RVA: 0x0022A9FE File Offset: 0x00228BFE
	// (set) Token: 0x06005EB6 RID: 24246 RVA: 0x0022AA06 File Offset: 0x00228C06
	public Material powerInputMaterial { get; set; }

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06005EB7 RID: 24247 RVA: 0x0022AA0F File Offset: 0x00228C0F
	// (set) Token: 0x06005EB8 RID: 24248 RVA: 0x0022AA17 File Offset: 0x00228C17
	public Material powerOutputMaterial { get; set; }

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x0022AA20 File Offset: 0x00228C20
	// (set) Token: 0x06005EBA RID: 24250 RVA: 0x0022AA28 File Offset: 0x00228C28
	public Material liquidInputMaterial { get; set; }

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06005EBB RID: 24251 RVA: 0x0022AA31 File Offset: 0x00228C31
	// (set) Token: 0x06005EBC RID: 24252 RVA: 0x0022AA39 File Offset: 0x00228C39
	public Material liquidOutputMaterial { get; set; }

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x06005EBD RID: 24253 RVA: 0x0022AA42 File Offset: 0x00228C42
	// (set) Token: 0x06005EBE RID: 24254 RVA: 0x0022AA4A File Offset: 0x00228C4A
	public Material gasInputMaterial { get; set; }

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x06005EBF RID: 24255 RVA: 0x0022AA53 File Offset: 0x00228C53
	// (set) Token: 0x06005EC0 RID: 24256 RVA: 0x0022AA5B File Offset: 0x00228C5B
	public Material gasOutputMaterial { get; set; }

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x06005EC1 RID: 24257 RVA: 0x0022AA64 File Offset: 0x00228C64
	// (set) Token: 0x06005EC2 RID: 24258 RVA: 0x0022AA6C File Offset: 0x00228C6C
	public Material highEnergyParticleInputMaterial { get; set; }

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x0022AA75 File Offset: 0x00228C75
	// (set) Token: 0x06005EC4 RID: 24260 RVA: 0x0022AA7D File Offset: 0x00228C7D
	public Material highEnergyParticleOutputMaterial { get; set; }

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06005EC5 RID: 24261 RVA: 0x0022AA86 File Offset: 0x00228C86
	// (set) Token: 0x06005EC6 RID: 24262 RVA: 0x0022AA8E File Offset: 0x00228C8E
	public Mesh backgroundMesh { get; set; }

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06005EC7 RID: 24263 RVA: 0x0022AA97 File Offset: 0x00228C97
	// (set) Token: 0x06005EC8 RID: 24264 RVA: 0x0022AA9F File Offset: 0x00228C9F
	public Mesh iconMesh { get; set; }

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06005EC9 RID: 24265 RVA: 0x0022AAA8 File Offset: 0x00228CA8
	// (set) Token: 0x06005ECA RID: 24266 RVA: 0x0022AAB0 File Offset: 0x00228CB0
	public int backgroundLayer { get; set; }

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06005ECB RID: 24267 RVA: 0x0022AAB9 File Offset: 0x00228CB9
	// (set) Token: 0x06005ECC RID: 24268 RVA: 0x0022AAC1 File Offset: 0x00228CC1
	public int iconLayer { get; set; }

	// Token: 0x06005ECD RID: 24269 RVA: 0x0022AACA File Offset: 0x00228CCA
	public static void DestroyInstance()
	{
		BuildingCellVisualizerResources._Instance = null;
	}

	// Token: 0x06005ECE RID: 24270 RVA: 0x0022AAD2 File Offset: 0x00228CD2
	public static BuildingCellVisualizerResources Instance()
	{
		if (BuildingCellVisualizerResources._Instance == null)
		{
			BuildingCellVisualizerResources._Instance = Resources.Load<BuildingCellVisualizerResources>("BuildingCellVisualizerResources");
			BuildingCellVisualizerResources._Instance.Initialize();
		}
		return BuildingCellVisualizerResources._Instance;
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x0022AB00 File Offset: 0x00228D00
	private void Initialize()
	{
		Shader shader = Shader.Find("Klei/BuildingCell");
		this.backgroundMaterial = new Material(shader);
		this.backgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.iconBackgroundMaterial = new Material(shader);
		this.iconBackgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.powerInputMaterial = new Material(shader);
		this.powerOutputMaterial = new Material(shader);
		this.liquidInputMaterial = new Material(shader);
		this.liquidOutputMaterial = new Material(shader);
		this.gasInputMaterial = new Material(shader);
		this.gasOutputMaterial = new Material(shader);
		this.highEnergyParticleInputMaterial = new Material(shader);
		this.highEnergyParticleOutputMaterial = new Material(shader);
		this.backgroundMesh = this.CreateMesh("BuildingCellVisualizer", Vector2.zero, 0.5f);
		float num = 0.5f;
		this.iconMesh = this.CreateMesh("BuildingCellVisualizerIcon", Vector2.zero, num * 0.5f);
		this.backgroundLayer = LayerMask.NameToLayer("Default");
		this.iconLayer = LayerMask.NameToLayer("Place");
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x0022AC18 File Offset: 0x00228E18
	private Mesh CreateMesh(string name, Vector2 base_offset, float half_size)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = new Vector3[]
		{
			new Vector3(-half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(-half_size + base_offset.x, half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, half_size + base_offset.y, 0f)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x04003F1B RID: 16155
	[Header("Electricity")]
	public Color electricityInputColor;

	// Token: 0x04003F1C RID: 16156
	public Color electricityOutputColor;

	// Token: 0x04003F1D RID: 16157
	public Sprite electricityInputIcon;

	// Token: 0x04003F1E RID: 16158
	public Sprite electricityOutputIcon;

	// Token: 0x04003F1F RID: 16159
	public Sprite electricityConnectedIcon;

	// Token: 0x04003F20 RID: 16160
	public Sprite electricityBridgeIcon;

	// Token: 0x04003F21 RID: 16161
	public Sprite electricityBridgeConnectedIcon;

	// Token: 0x04003F22 RID: 16162
	public Sprite electricityArrowIcon;

	// Token: 0x04003F23 RID: 16163
	public Sprite switchIcon;

	// Token: 0x04003F24 RID: 16164
	public Color32 switchColor;

	// Token: 0x04003F25 RID: 16165
	public Color32 switchOffColor = Color.red;

	// Token: 0x04003F26 RID: 16166
	[Header("Gas")]
	public Sprite gasInputIcon;

	// Token: 0x04003F27 RID: 16167
	public Sprite gasOutputIcon;

	// Token: 0x04003F28 RID: 16168
	public BuildingCellVisualizerResources.IOColours gasIOColours;

	// Token: 0x04003F29 RID: 16169
	[Header("Liquid")]
	public Sprite liquidInputIcon;

	// Token: 0x04003F2A RID: 16170
	public Sprite liquidOutputIcon;

	// Token: 0x04003F2B RID: 16171
	public BuildingCellVisualizerResources.IOColours liquidIOColours;

	// Token: 0x04003F2C RID: 16172
	[Header("High Energy Particle")]
	public Sprite highEnergyParticleInputIcon;

	// Token: 0x04003F2D RID: 16173
	public Sprite[] highEnergyParticleOutputIcons;

	// Token: 0x04003F2E RID: 16174
	public Color highEnergyParticleInputColour;

	// Token: 0x04003F2F RID: 16175
	public Color highEnergyParticleOutputColour;

	// Token: 0x04003F30 RID: 16176
	[Header("Heat Sources and Sinks")]
	public Sprite heatSourceIcon;

	// Token: 0x04003F31 RID: 16177
	public Sprite heatSinkIcon;

	// Token: 0x04003F32 RID: 16178
	[Header("Alternate IO Colours")]
	public BuildingCellVisualizerResources.IOColours alternateIOColours;

	// Token: 0x04003F41 RID: 16193
	private static BuildingCellVisualizerResources _Instance;

	// Token: 0x02001DE3 RID: 7651
	[Serializable]
	public struct ConnectedDisconnectedColours
	{
		// Token: 0x04008C8A RID: 35978
		public Color32 connected;

		// Token: 0x04008C8B RID: 35979
		public Color32 disconnected;
	}

	// Token: 0x02001DE4 RID: 7652
	[Serializable]
	public struct IOColours
	{
		// Token: 0x04008C8C RID: 35980
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours input;

		// Token: 0x04008C8D RID: 35981
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours output;
	}
}
