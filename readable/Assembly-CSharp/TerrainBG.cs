using System;
using TUNING;
using UnityEngine;

// Token: 0x02000BF3 RID: 3059
[AddComponentMenu("KMonoBehaviour/scripts/TerrainBG")]
public class TerrainBG : KMonoBehaviour
{
	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x00213313 File Offset: 0x00211513
	public bool LargeImpactorFragmentsVisible
	{
		get
		{
			return ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.largeImpactorFragments == FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.ALLOWED && SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Defeated;
		}
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x00213351 File Offset: 0x00211551
	public float LargeImpactorBackgroundScale
	{
		get
		{
			return SaveGame.Instance.ColonyAchievementTracker.LargeImpactorBackgroundScale;
		}
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x00213362 File Offset: 0x00211562
	protected override void OnPrefabInit()
	{
		TerrainBG.preventLargeImpactorFragmentsFromProgressing = false;
		base.OnPrefabInit();
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x00213370 File Offset: 0x00211570
	protected override void OnSpawn()
	{
		this.layer = LayerMask.NameToLayer("Default");
		this.noiseVolume = this.CreateTexture3D(32);
		this.starsPlane = this.CreateStarsPlane("StarsPlane");
		this.northernLightsPlane = this.CreateNorthernLightsPlane("NorthernLightsPlane");
		this.largeImpactorDefeatedPlane = this.CreateGridSizePlane("LargeImpactorDefeatedPlane");
		this.worldPlane = this.CreateWorldPlane("WorldPlane");
		this.gasPlane = this.CreateGasPlane("GasPlane");
		this.propertyBlocks = new MaterialPropertyBlock[Lighting.Instance.Settings.BackgroundLayers];
		for (int i = 0; i < this.propertyBlocks.Length; i++)
		{
			this.propertyBlocks[i] = new MaterialPropertyBlock();
		}
		this.LargeImpactorEntryProgress = (float)(this.LargeImpactorFragmentsVisible ? 1 : -1);
		this.largeImpactorFragmentsMaterial.SetFloat("_EntryProgress", this.LargeImpactorEntryProgress);
		this.largeImpactorFragmentsMaterial.SetFloat("_LargeImpactorScale", this.LargeImpactorBackgroundScale);
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x0021346C File Offset: 0x0021166C
	private Texture3D CreateTexture3D(int size)
	{
		Color32[] array = new Color32[size * size * size];
		Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.RGBA32, true);
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				for (int k = 0; k < size; k++)
				{
					Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255));
					array[i + j * size + k * size * size] = color;
				}
			}
		}
		texture3D.SetPixels32(array);
		texture3D.Apply();
		return texture3D;
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x00213510 File Offset: 0x00211710
	public Mesh CreateGasPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, Grid.HeightInMeters, 0f),
			new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(new Vector3((float)Grid.WidthInCells * 0.5f, (float)Grid.HeightInCells * 0.5f, 0f), new Vector3((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f));
		return mesh;
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x00213680 File Offset: 0x00211880
	public Mesh CreateWorldPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		vertices = new Vector3[]
		{
			new Vector3((float)(-(float)Grid.WidthInCells), (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)Grid.WidthInCells * 2f, (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)(-(float)Grid.WidthInCells), Grid.HeightInMeters * 2f, 0f),
			new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(new Vector3((float)Grid.WidthInCells * 0.5f, (float)Grid.HeightInCells * 0.5f, 0f), new Vector3((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f));
		return mesh;
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x00213810 File Offset: 0x00211A10
	public Mesh CreateStarsPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		vertices = new Vector3[]
		{
			new Vector3((float)(-(float)Grid.WidthInCells), (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)Grid.WidthInCells * 2f, (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)(-(float)Grid.WidthInCells), Grid.HeightInMeters * 2f, 0f),
			new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		Vector2 vector = new Vector2((float)Grid.WidthInCells, 2f * (float)Grid.HeightInCells);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return mesh;
	}

	// Token: 0x06005BDA RID: 23514 RVA: 0x002139C0 File Offset: 0x00211BC0
	public Mesh CreateNorthernLightsPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float num2 = 1f;
		float num3 = this.northernLightSkySize * 0.5f;
		vertices = new Vector3[]
		{
			new Vector3(-num2, -num3, 0f),
			new Vector3(num2, -num3, 0f),
			new Vector3(-num2, num3, 0f),
			new Vector3(num2, num3, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		return mesh;
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x00213AEC File Offset: 0x00211CEC
	public Mesh CreateGridSizePlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float num2 = (float)Mathf.Max(Grid.WidthInCells, Grid.HeightInCells) / 2f;
		vertices = new Vector3[]
		{
			new Vector3(-num2, -num2, 0f),
			new Vector3(num2, -num2, 0f),
			new Vector3(-num2, num2, 0f),
			new Vector3(num2, num2, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		return mesh;
	}

	// Token: 0x06005BDC RID: 23516 RVA: 0x00213C1C File Offset: 0x00211E1C
	private void LateUpdate()
	{
		if (!this.doDraw)
		{
			return;
		}
		Material material = this.starsMaterial_surface;
		if (ClusterManager.Instance.activeWorld.IsModuleInterior)
		{
			Clustercraft component = ClusterManager.Instance.activeWorld.GetComponent<Clustercraft>();
			if (component.Status != Clustercraft.CraftStatus.InFlight)
			{
				material = this.starsMaterial_surface;
			}
			else if (ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(component.Location, EntityLayer.Asteroid) != null)
			{
				material = this.starsMaterial_space;
			}
			else
			{
				material = this.starsMaterial_space;
			}
			bool flag = component.IsFlightInProgress() && component.HasResourcesToMove(1, Clustercraft.CombustionResource.All);
			material.SetFloat("_IsInFlight", (float)(flag ? 1 : 0));
			material.SetFloat("_AccelerationTimeStamp", component.LastTimeFlightBegan);
			material.SetFloat("_DecelerationTimeStamp", component.LastTimeFlightStopped);
		}
		material.renderQueue = RenderQueues.Stars;
		material.SetTexture("_NoiseVolume", this.noiseVolume);
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Background) + 1f);
		Graphics.DrawMesh(this.starsPlane, position, Quaternion.identity, material, this.layer);
		if (this.LargeImpactorFragmentsVisible)
		{
			Vector3 position2 = new Vector3(CameraController.Instance.transform.position.x, CameraController.Instance.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.85f);
			if (!TerrainBG.preventLargeImpactorFragmentsFromProgressing && this.LargeImpactorEntryProgress < 1f)
			{
				if (this.LargeImpactorEntryProgress < 0f)
				{
					this.LargeImpactorEntryProgress = 0f;
					this.largeImpactorFragmentsMaterial.SetFloat("_LargeImpactorScale", this.LargeImpactorBackgroundScale);
				}
				if (!SpeedControlScreen.Instance.IsPaused)
				{
					if (this.LargeImpactorEntryProgress == 0f)
					{
						KFMOD.PlayUISound(GlobalAssets.GetSound("Asteroid_destroyed_end", false));
					}
					this.LargeImpactorEntryProgress += Time.unscaledDeltaTime / 2.5f;
				}
				this.LargeImpactorEntryProgress = Mathf.Clamp01(this.LargeImpactorEntryProgress);
				this.largeImpactorFragmentsMaterial.SetFloat("_EntryProgress", this.LargeImpactorEntryProgress);
			}
			this.largeImpactorFragmentsMaterial.SetFloat("_UnscaledTime", Time.timeSinceLevelLoad);
			Graphics.DrawMesh(this.largeImpactorDefeatedPlane, position2, Quaternion.identity, this.largeImpactorFragmentsMaterial, this.layer);
		}
		if (ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.northernlights > 0)
		{
			Vector3 position3 = new Vector3(CameraController.Instance.transform.position.x, CameraController.Instance.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.8f);
			Graphics.DrawMesh(this.northernLightsPlane, position3, Quaternion.identity, this.northernLightMaterial_ceres, this.layer);
		}
		this.backgroundMaterial.renderQueue = RenderQueues.Backwall;
		for (int i = 0; i < Lighting.Instance.Settings.BackgroundLayers; i++)
		{
			if (i >= Lighting.Instance.Settings.BackgroundLayers - 1)
			{
				float t = (float)i / (float)(Lighting.Instance.Settings.BackgroundLayers - 1);
				float x = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundDarkening, t);
				float z = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundUVScale, t);
				float w = 1f;
				if (i == Lighting.Instance.Settings.BackgroundLayers - 1)
				{
					w = 0f;
				}
				MaterialPropertyBlock materialPropertyBlock = this.propertyBlocks[i];
				materialPropertyBlock.SetVector("_BackWallParameters", new Vector4(x, Lighting.Instance.Settings.BackgroundClip, z, w));
				Vector3 position4 = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Background));
				Graphics.DrawMesh(this.worldPlane, position4, Quaternion.identity, this.backgroundMaterial, this.layer, null, 0, materialPropertyBlock);
			}
		}
		this.gasMaterial.renderQueue = RenderQueues.Gas;
		Vector3 position5 = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Gas));
		Graphics.DrawMesh(this.gasPlane, position5, Quaternion.identity, this.gasMaterial, this.layer);
		Vector3 position6 = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasFront));
		Graphics.DrawMesh(this.gasPlane, position6, Quaternion.identity, this.gasMaterial, this.layer);
	}

	// Token: 0x04003D19 RID: 15641
	public Material northernLightMaterial_ceres;

	// Token: 0x04003D1A RID: 15642
	public Material largeImpactorFragmentsMaterial;

	// Token: 0x04003D1B RID: 15643
	public Material starsMaterial_surface;

	// Token: 0x04003D1C RID: 15644
	public Material starsMaterial_orbit;

	// Token: 0x04003D1D RID: 15645
	public Material starsMaterial_space;

	// Token: 0x04003D1E RID: 15646
	public Material backgroundMaterial;

	// Token: 0x04003D1F RID: 15647
	public Material gasMaterial;

	// Token: 0x04003D20 RID: 15648
	public bool doDraw = true;

	// Token: 0x04003D21 RID: 15649
	private const string Sound_Destroyed_Victory_End_Sequence = "Asteroid_destroyed_end";

	// Token: 0x04003D22 RID: 15650
	[SerializeField]
	private Texture3D noiseVolume;

	// Token: 0x04003D23 RID: 15651
	private Mesh starsPlane;

	// Token: 0x04003D24 RID: 15652
	private Mesh northernLightsPlane;

	// Token: 0x04003D25 RID: 15653
	private Mesh largeImpactorDefeatedPlane;

	// Token: 0x04003D26 RID: 15654
	private Mesh worldPlane;

	// Token: 0x04003D27 RID: 15655
	private Mesh gasPlane;

	// Token: 0x04003D28 RID: 15656
	private int layer;

	// Token: 0x04003D29 RID: 15657
	private float northernLightSkySize = 2f;

	// Token: 0x04003D2A RID: 15658
	public static bool preventLargeImpactorFragmentsFromProgressing;

	// Token: 0x04003D2B RID: 15659
	public const float LargeImpactorFragmentsEntryEffectDuration = 2.5f;

	// Token: 0x04003D2C RID: 15660
	private float LargeImpactorEntryProgress = -1f;

	// Token: 0x04003D2D RID: 15661
	private MaterialPropertyBlock[] propertyBlocks;
}
