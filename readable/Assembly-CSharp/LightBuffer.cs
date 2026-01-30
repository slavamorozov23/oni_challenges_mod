using System;
using UnityEngine;

// Token: 0x02000AE5 RID: 2789
public class LightBuffer : MonoBehaviour
{
	// Token: 0x0600512D RID: 20781 RVA: 0x001D60C0 File Offset: 0x001D42C0
	private void Awake()
	{
		LightBuffer.Instance = this;
		this.ColorRangeTag = Shader.PropertyToID("_ColorRange");
		this.LightPosTag = Shader.PropertyToID("_LightPos");
		this.LightDirectionAngleTag = Shader.PropertyToID("_LightDirectionAngle");
		this.TintColorTag = Shader.PropertyToID("_TintColor");
		this.Camera = base.GetComponent<Camera>();
		this.Layer = LayerMask.NameToLayer("Lights");
		this.Mesh = new Mesh();
		this.Mesh.name = "Light Mesh";
		this.Mesh.vertices = new Vector3[]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f)
		};
		this.Mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};
		this.Mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
		this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
		this.Texture.name = "LightBuffer";
		this.Camera.targetTexture = this.Texture;
	}

	// Token: 0x0600512E RID: 20782 RVA: 0x001D62B0 File Offset: 0x001D44B0
	private void LateUpdate()
	{
		if (PropertyTextures.instance == null)
		{
			return;
		}
		if (this.Texture.width != Screen.width || this.Texture.height != Screen.height)
		{
			this.Texture.DestroyRenderTexture();
			this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
			this.Texture.name = "LightBuffer";
			this.Camera.targetTexture = this.Texture;
		}
		Matrix4x4 matrix = default(Matrix4x4);
		this.WorldLight = PropertyTextures.instance.GetTexture(PropertyTextures.Property.WorldLight);
		this.Material.SetTexture("_PropertyWorldLight", this.WorldLight);
		this.CircleMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
		this.ConeMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
		GridArea visibleAreaExtended = GridVisibleArea.GetVisibleAreaExtended((int)this.largestLightRange + 4);
		DictionaryPool<int, int, LightBuffer>.PooledDictionary pooledDictionary = DictionaryPool<int, int, LightBuffer>.Allocate();
		foreach (Light2D light2D in Components.Light2Ds.Items)
		{
			if (!(light2D == null) && visibleAreaExtended.Contains(light2D.cachedCell))
			{
				Vector3 position = light2D.transform.GetPosition();
				int key = Grid.PosToCell(position);
				int num;
				pooledDictionary.TryGetValue(key, out num);
				if (num < this.maxLights)
				{
					pooledDictionary[key] = num + 1;
					MaterialPropertyBlock materialPropertyBlock = light2D.materialPropertyBlock;
					materialPropertyBlock.SetVector(this.ColorRangeTag, new Vector4(light2D.Color.r * light2D.IntensityAnimation, light2D.Color.g * light2D.IntensityAnimation, light2D.Color.b * light2D.IntensityAnimation, light2D.Range));
					position.x += light2D.Offset.x;
					position.y += light2D.Offset.y;
					materialPropertyBlock.SetVector(this.LightPosTag, new Vector4(position.x, position.y, 0f, 0f));
					Vector2 normalized = light2D.Direction.normalized;
					materialPropertyBlock.SetVector(this.LightDirectionAngleTag, new Vector4(normalized.x, normalized.y, 0f, light2D.Angle));
					Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, materialPropertyBlock, false, false);
					if (light2D.drawOverlay)
					{
						materialPropertyBlock.SetColor(this.TintColorTag, light2D.overlayColour);
						global::LightShape shape = light2D.shape;
						if (shape != global::LightShape.Circle)
						{
							if (shape == global::LightShape.Cone)
							{
								matrix.SetTRS(position - Vector3.up * (light2D.Range * 0.5f), Quaternion.identity, new Vector3(1f, 0.5f, 1f) * light2D.Range);
								Graphics.DrawMesh(this.Mesh, matrix, this.ConeMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
							}
						}
						else
						{
							matrix.SetTRS(position, Quaternion.identity, Vector3.one * light2D.Range);
							Graphics.DrawMesh(this.Mesh, matrix, this.CircleMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
						}
					}
					this.largestLightRange = Mathf.Max(this.largestLightRange, light2D.Range);
				}
			}
		}
		pooledDictionary.Recycle();
	}

	// Token: 0x0600512F RID: 20783 RVA: 0x001D6674 File Offset: 0x001D4874
	private void OnDestroy()
	{
		LightBuffer.Instance = null;
	}

	// Token: 0x0400361A RID: 13850
	public int maxLights = 30;

	// Token: 0x0400361B RID: 13851
	private Mesh Mesh;

	// Token: 0x0400361C RID: 13852
	private Camera Camera;

	// Token: 0x0400361D RID: 13853
	public float largestLightRange = 16f;

	// Token: 0x0400361E RID: 13854
	[NonSerialized]
	public Material Material;

	// Token: 0x0400361F RID: 13855
	[NonSerialized]
	public Material CircleMaterial;

	// Token: 0x04003620 RID: 13856
	[NonSerialized]
	public Material ConeMaterial;

	// Token: 0x04003621 RID: 13857
	private int ColorRangeTag;

	// Token: 0x04003622 RID: 13858
	private int LightPosTag;

	// Token: 0x04003623 RID: 13859
	private int LightDirectionAngleTag;

	// Token: 0x04003624 RID: 13860
	private int TintColorTag;

	// Token: 0x04003625 RID: 13861
	private int Layer;

	// Token: 0x04003626 RID: 13862
	public RenderTexture Texture;

	// Token: 0x04003627 RID: 13863
	public Texture WorldLight;

	// Token: 0x04003628 RID: 13864
	public static LightBuffer Instance;

	// Token: 0x04003629 RID: 13865
	private const RenderTextureFormat RTFormat = RenderTextureFormat.ARGBHalf;
}
