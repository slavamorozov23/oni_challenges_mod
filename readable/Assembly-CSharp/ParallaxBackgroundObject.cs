using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A8D RID: 2701
public class ParallaxBackgroundObject : KMonoBehaviour
{
	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x06004E6E RID: 20078 RVA: 0x001C873D File Offset: 0x001C693D
	public static Mesh Mesh
	{
		get
		{
			if (ParallaxBackgroundObject.mesh == null)
			{
				ParallaxBackgroundObject.mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
			}
			return ParallaxBackgroundObject.mesh;
		}
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x06004E6F RID: 20079 RVA: 0x001C8760 File Offset: 0x001C6960
	public static int Layer
	{
		get
		{
			int value = ParallaxBackgroundObject.layer.GetValueOrDefault();
			if (ParallaxBackgroundObject.layer == null)
			{
				value = LayerMask.NameToLayer("Default");
				ParallaxBackgroundObject.layer = new int?(value);
			}
			return ParallaxBackgroundObject.layer.Value;
		}
	}

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06004E70 RID: 20080 RVA: 0x001C87A4 File Offset: 0x001C69A4
	public static float Depth
	{
		get
		{
			float value = ParallaxBackgroundObject.depth.GetValueOrDefault();
			if (ParallaxBackgroundObject.depth == null)
			{
				value = Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.8f;
				ParallaxBackgroundObject.depth = new float?(value);
			}
			return ParallaxBackgroundObject.depth.Value;
		}
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x001C87EC File Offset: 0x001C69EC
	private void OnActiveWorldChanged(object data)
	{
		if (this.worldId == null)
		{
			return;
		}
		int first = ((global::Tuple<int, int>)data).first;
		this.visible = (first == this.worldId.Value);
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x001C8827 File Offset: 0x001C6A27
	public void Initialize(string texture)
	{
		this.sprite = Assets.GetSprite(texture);
	}

	// Token: 0x06004E73 RID: 20083 RVA: 0x001C883A File Offset: 0x001C6A3A
	public void SetVisibilityState(bool visible)
	{
		this.visible = visible;
	}

	// Token: 0x06004E74 RID: 20084 RVA: 0x001C8843 File Offset: 0x001C6A43
	public void PlayPlayerClickFeedback()
	{
		this.material.SetFloat("_LastTimePlayerClickedNotification", Time.unscaledTime);
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x001C885A File Offset: 0x001C6A5A
	public void PlayExplosion()
	{
		this.material.SetFloat("_LastTimeExploding", Time.unscaledTime);
	}

	// Token: 0x06004E76 RID: 20086 RVA: 0x001C8874 File Offset: 0x001C6A74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.distanceUpdate = true;
		this.startOffset = new Vector2(0f, 0f);
		this.endOffset = new Vector2(0.5f, 0.2f);
		Material source = Assets.GetMaterial("BGPlanet");
		this.material = new Material(source);
		this.material.SetTexture("_MainTex", this.sprite.texture);
		this.material.SetFloat("_LastTimeDamaged", float.MinValue);
		this.material.SetFloat("_LastTimePlayerClickedNotification", float.MinValue);
		this.material.SetFloat("_SizeProgress", 0f);
		this.material.renderQueue = RenderQueues.Stars;
	}

	// Token: 0x06004E77 RID: 20087 RVA: 0x001C8955 File Offset: 0x001C6B55
	public void TriggerShaderDamagedEffect(int _)
	{
		this.material.SetFloat("_LastTimeDamaged", Time.unscaledTime);
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06004E79 RID: 20089 RVA: 0x001C8975 File Offset: 0x001C6B75
	// (set) Token: 0x06004E78 RID: 20088 RVA: 0x001C896C File Offset: 0x001C6B6C
	public float lastScaleUsed { get; private set; } = 1f;

	// Token: 0x06004E7A RID: 20090 RVA: 0x001C8980 File Offset: 0x001C6B80
	private void LateUpdate()
	{
		if (this.motion == null)
		{
			return;
		}
		if (!this.visible)
		{
			return;
		}
		if (this.distanceUpdate)
		{
			float duration = this.motion.GetDuration();
			this.normalizedDistance = ((duration == 0f) ? 1f : (1f - Mathf.Pow(this.motion.GetETA() / duration, 4f)));
			this.motion.OnNormalizedDistanceChanged(this.normalizedDistance);
		}
		Color a = new Color(0.16862746f, 0.22745098f, 0.36078432f, 0f);
		this.material.color = Color.Lerp(a, Color.white, this.normalizedDistance);
		float num = Mathf.Lerp(this.scaleMin, this.scaleMax, this.normalizedDistance);
		this.lastScaleUsed = num;
		Vector2 v = Vector2.Lerp(this.startOffset, this.endOffset, this.normalizedDistance);
		Vector3 position = CameraController.Instance.baseCamera.transform.position;
		Vector3 a2 = new Vector3(position.x * this.parallaxFactor, position.y * this.parallaxFactor, ParallaxBackgroundObject.Depth);
		float num2 = CameraController.Instance.baseCamera.orthographicSize / 1f;
		Vector3 vector = a2 + v * num2;
		Vector3 vector2 = num * num2 * Vector3.one;
		Quaternion q = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0f, 0f, -20f), this.normalizedDistance);
		this.material.SetFloat("_UnscaledTime", Time.unscaledTime);
		this.material.SetVector("_Random", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value));
		this.material.SetFloat("_SizeProgress", this.normalizedDistance);
		Matrix4x4 matrix = Matrix4x4.Translate(vector) * Matrix4x4.Scale(vector2) * Matrix4x4.Rotate(q);
		Graphics.DrawMesh(ParallaxBackgroundObject.Mesh, matrix, this.material, ParallaxBackgroundObject.Layer);
	}

	// Token: 0x04003447 RID: 13383
	private static Mesh mesh;

	// Token: 0x04003448 RID: 13384
	private static int? layer;

	// Token: 0x04003449 RID: 13385
	private static float? depth;

	// Token: 0x0400344A RID: 13386
	[SerializeField]
	private Sprite sprite;

	// Token: 0x0400344B RID: 13387
	[SerializeField]
	private float parallaxFactor = 1f;

	// Token: 0x0400344C RID: 13388
	[Range(0f, 5f)]
	public float scaleMin = 0.25f;

	// Token: 0x0400344D RID: 13389
	[Range(0f, 5f)]
	public float scaleMax = 3f;

	// Token: 0x0400344E RID: 13390
	[Serialize]
	private bool visible = true;

	// Token: 0x0400344F RID: 13391
	private const string SHADER_DAMAGED_TIME_VARIABLE_NAME = "_LastTimeDamaged";

	// Token: 0x04003450 RID: 13392
	private const string SHADER_PLAYER_CLICKED_TIME_VARIABLE_NAME = "_LastTimePlayerClickedNotification";

	// Token: 0x04003451 RID: 13393
	private const string SHADER_SIZE_PROGRESS_VARIABLE_NAME = "_SizeProgress";

	// Token: 0x04003452 RID: 13394
	private const string SHADER_EXPLOSION_START_TIME_VARIABLE_NAME = "_LastTimeExploding";

	// Token: 0x04003453 RID: 13395
	[SerializeField]
	private Material material;

	// Token: 0x04003454 RID: 13396
	[SerializeField]
	[Range(0f, 1f)]
	private float normalizedDistance;

	// Token: 0x04003455 RID: 13397
	[SerializeField]
	private bool distanceUpdate;

	// Token: 0x04003456 RID: 13398
	[SerializeField]
	private Vector2 startOffset;

	// Token: 0x04003457 RID: 13399
	[SerializeField]
	private Vector2 endOffset;

	// Token: 0x04003458 RID: 13400
	[Serialize]
	public int? worldId;

	// Token: 0x04003459 RID: 13401
	public ParallaxBackgroundObject.IMotion motion;

	// Token: 0x02001BA8 RID: 7080
	public interface IMotion
	{
		// Token: 0x0600AAAF RID: 43695
		float GetETA();

		// Token: 0x0600AAB0 RID: 43696
		float GetDuration();

		// Token: 0x0600AAB1 RID: 43697
		void OnNormalizedDistanceChanged(float normalizedDistance);
	}
}
