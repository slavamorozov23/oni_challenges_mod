using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x020009DA RID: 2522
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class LargeComet : KMonoBehaviour, ISim33ms
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x0600494D RID: 18765 RVA: 0x001A7F9F File Offset: 0x001A619F
	// (set) Token: 0x0600494C RID: 18764 RVA: 0x001A7F96 File Offset: 0x001A6196
	public float LandingProgress { get; private set; }

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x0600494E RID: 18766 RVA: 0x001A7FA7 File Offset: 0x001A61A7
	public Vector3 VisualPosition
	{
		get
		{
			return base.transform.position + this.anim.Offset;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x0600494F RID: 18767 RVA: 0x001A7FC4 File Offset: 0x001A61C4
	public Vector3 VisualPositionCentredImage
	{
		get
		{
			return this.VisualPosition + new Vector3(0f, (float)Mathf.Abs(this.lowestTemplateYLocalPosition), 0f);
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06004950 RID: 18768 RVA: 0x001A7FEC File Offset: 0x001A61EC
	// (set) Token: 0x06004951 RID: 18769 RVA: 0x001A7FF4 File Offset: 0x001A61F4
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x001A8000 File Offset: 0x001A6200
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x001A803E File Offset: 0x001A623E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.SetVelocity();
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x001A8070 File Offset: 0x001A6270
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		this.SetupOffset();
		this.child_controllers = base.GetComponents<KBatchedAnimController>();
		KBatchedAnimController[] array = this.child_controllers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Offset = this.anim.Offset;
		}
		base.OnSpawn();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		Vector3 position = base.gameObject.transform.position;
		foreach (KeyValuePair<string, string> keyValuePair in this.additionalAnimFiles)
		{
			this.additionalAnimControllers.Add(this.AddEffectAnim(keyValuePair.Key, keyValuePair.Value, position));
			position.z -= 0.001f;
		}
		KBatchedAnimController item = this.AddEffectAnim(this.mainAnimFile.Key, this.mainAnimFile.Value, position);
		this.additionalAnimControllers.Add(item);
		this.mainChildrenAnimController = item;
		this.mainChildrenAnimController.materialType = KAnimBatchGroup.MaterialType.Invisible;
		this.initialPosition = this.VisualPosition;
		this.lowestTemplateYLocalPosition = this.asteroidTemplate.GetTemplateBounds(0).yMin;
		this.templateWidth = this.asteroidTemplate.GetTemplateBounds(0).width;
		this.InitializeMaterial();
		CameraController.Instance.RegisterCustomScreenPostProcessingEffect(new Func<RenderTexture, Material>(this.DrawComet));
		this.fromStampToCrashPosition = this.stampLocation - this.crashPosition;
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x001A8248 File Offset: 0x001A6448
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		CameraController.Instance.UnregisterCustomScreenPostProcessingEffect(new Func<RenderTexture, Material>(this.DrawComet));
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x001A8268 File Offset: 0x001A6468
	private KBatchedAnimController AddEffectAnim(string anim_file, string anim_name, Vector3 startPosition)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(anim_file, startPosition, null, false, Grid.SceneLayer.Front, false);
		kbatchedAnimController.Play(anim_name, KAnim.PlayMode.Loop, 1f, 0f);
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		kbatchedAnimController.animScale = 0.1f;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.Offset = this.anim.Offset;
		return kbatchedAnimController;
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x001A82C4 File Offset: 0x001A64C4
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
		this.worldID = myWorld.id;
		this.previousVisualPosition = this.VisualPosition;
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x001A84A0 File Offset: 0x001A66A0
	public void SetVelocity()
	{
		int num = -90;
		float f = (float)num * 3.1415927f / 180f;
		int num2 = 12;
		this.velocity = new Vector2(-Mathf.Cos(f) * (float)num2, Mathf.Sin(f) * (float)num2);
		base.GetComponent<KBatchedAnimController>().Rotation = (float)(-(float)num) - 90f;
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x001A84F4 File Offset: 0x001A66F4
	private void Explode(Vector3 pos)
	{
		this.PlayImpactSound(pos);
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
		foreach (KAnimControllerBase original in this.additionalAnimControllers)
		{
			global::Util.KDestroyGameObject(original);
		}
		global::Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x001A856C File Offset: 0x001A676C
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
			using (List<KAnimControllerBase>.Enumerator enumerator = this.additionalAnimControllers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KAnimControllerBase kanimControllerBase = enumerator.Current;
					kanimControllerBase.Offset = this.offsetPosition;
				}
				goto IL_1E2;
			}
		}
		if (this.anim.Offset != Vector3.zero)
		{
			this.anim.Offset = Vector3.zero;
			foreach (KAnimControllerBase kanimControllerBase2 in this.additionalAnimControllers)
			{
				kanimControllerBase2.Offset = this.anim.Offset;
			}
		}
		Vector3 position = base.transform.GetPosition();
		Vector3 vector2 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
		this.loopingSounds.UpdateVelocity(this.flyingSound, vector2 - position);
		base.transform.SetPosition(vector2);
		Vector3 position2 = vector2;
		foreach (KAnimControllerBase kanimControllerBase3 in this.additionalAnimControllers)
		{
			kanimControllerBase3.transform.SetPosition(position2);
			position2.z -= 0.001f;
		}
		if (vector2.y < (float)this.crashPosition.y)
		{
			this.Explode(vector2);
		}
		IL_1E2:
		Vector2I vector2I = Grid.PosToXY(this.previousVisualPosition);
		Vector2I vector2I2 = Grid.PosToXY(this.VisualPosition);
		vector2I.y = Mathf.Clamp(vector2I.y, this.crashPosition.y, int.MaxValue);
		vector2I2.y = Mathf.Clamp(vector2I2.y, this.crashPosition.y, int.MaxValue);
		if (vector2I2.y != vector2I.y)
		{
			Grid.CollectCellsInLine(Grid.XYToCell(vector2I.x, vector2I.y), Grid.XYToCell(vector2I2.x, vector2I2.y), this.cellsCentrePassedThrough);
			bool flag = false;
			Vector3 position3 = Vector3.zero;
			foreach (int cell in this.cellsCentrePassedThrough)
			{
				foreach (CellOffset cellOffset in this.bottomCellsOffsetOfTemplate.Values)
				{
					int cell2 = Grid.OffsetCell(Grid.OffsetCell(cell, 0, Mathf.Abs(this.lowestTemplateYLocalPosition)), cellOffset.x, cellOffset.y);
					if (Grid.IsValidCellInWorld(cell2, this.worldID) && this.DestroyCell(cell2) && !flag)
					{
						Vector3 position4 = Grid.CellToPos(cell2);
						if (this.IsPositionFarAwayFromOtherExplosions(position4))
						{
							flag = true;
							position3 = Grid.CellToPos(cell2);
						}
					}
				}
			}
			if (flag)
			{
				this.PlayExplosionEffectOnPosition(position3);
			}
		}
		float num = Mathf.Clamp(1f - (this.VisualPosition.y - (float)this.crashPosition.y) / (this.initialPosition.y - (float)this.crashPosition.y), 0f, 1f);
		this.mainChildrenAnimController.postProcessingParameters = Mathf.Clamp(Mathf.Ceil(num * (Mathf.Pow(10f, 3f) - 1f)), 0f, float.MaxValue);
		this.LandingProgress = num;
		this.previousVisualPosition = this.VisualPosition;
		this.age += dt;
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x001A89BC File Offset: 0x001A6BBC
	private bool IsPositionFarAwayFromOtherExplosions(Vector3 position)
	{
		this.activeExplosionPosition.z = position.z;
		for (int i = 0; i < 30; i++)
		{
			if (this.ShaderExplosions[i].z >= 0f && Time.timeSinceLevelLoad - this.ShaderExplosions[i].z < 1.2333333f)
			{
				this.activeExplosionPosition.x = this.ShaderExplosions[i].x;
				this.activeExplosionPosition.y = this.ShaderExplosions[i].y;
				if ((this.activeExplosionPosition - position).magnitude < this.minSeparationBetweenExplosions)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x001A8A7C File Offset: 0x001A6C7C
	private void PlayExplosionEffectOnPosition(Vector3 position)
	{
		for (int i = 0; i < 30; i++)
		{
			if (this.ShaderExplosions[i].z < 0f || Time.timeSinceLevelLoad - this.ShaderExplosions[i].z > 1.2333333f)
			{
				this.ShaderExplosions[i].x = position.x;
				this.ShaderExplosions[i].y = position.y;
				this.ShaderExplosions[i].z = Time.timeSinceLevelLoad;
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), position, 1f);
				this.lastExplosionPosition = position;
				return;
			}
		}
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x001A8B34 File Offset: 0x001A6D34
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x001A8BD8 File Offset: 0x001A6DD8
	public bool DestroyCell(int cell)
	{
		bool flag = false;
		ListPool<GameObject, LargeComet>.PooledList pooledList = ListPool<GameObject, LargeComet>.Allocate();
		GameObject gameObject = Grid.Objects[cell, 1];
		flag = (gameObject != null);
		pooledList.Add(gameObject);
		pooledList.Add(Grid.Objects[cell, 2]);
		pooledList.Add(Grid.Objects[cell, 12]);
		pooledList.Add(Grid.Objects[cell, 15]);
		pooledList.Add(Grid.Objects[cell, 16]);
		pooledList.Add(Grid.Objects[cell, 19]);
		pooledList.Add(Grid.Objects[cell, 20]);
		pooledList.Add(Grid.Objects[cell, 23]);
		pooledList.Add(Grid.Objects[cell, 26]);
		pooledList.Add(Grid.Objects[cell, 29]);
		pooledList.Add(Grid.Objects[cell, 31]);
		pooledList.Add(Grid.Objects[cell, 30]);
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (Grid.PosToCell(minionIdentity) == cell)
			{
				pooledList.Add(minionIdentity.gameObject);
				SaveGame.Instance.ColonyAchievementTracker.deadDupeCounter++;
			}
		}
		foreach (GameObject gameObject2 in pooledList)
		{
			if (gameObject2 != null)
			{
				global::Util.KDestroyGameObject(gameObject2);
			}
		}
		this.ClearCellPickupables(cell);
		Element element = ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element.id == SimHashes.Void)
		{
			SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
		}
		else
		{
			SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
		}
		flag = (flag || element.IsSolid);
		pooledList.Recycle();
		return flag;
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x001A8E24 File Offset: 0x001A7024
	public void ClearCellPickupables(int cell)
	{
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject != null)
		{
			ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
			while (objectLayerListItem != null)
			{
				GameObject gameObject2 = objectLayerListItem.gameObject;
				objectLayerListItem = objectLayerListItem.nextItem;
				if (!(gameObject2 == null))
				{
					global::Util.KDestroyGameObject(gameObject2);
				}
			}
		}
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x001A8E78 File Offset: 0x001A7078
	private void InitializeMaterial()
	{
		this.largeCometMaterial = new Material(Shader.Find("Klei/DLC4/LargeImpactorCometShader"));
		this.largeCometTexture = Assets.GetSprite("Demolior_final_broken");
		this.explosionTexture = Assets.GetSprite("contact_explode_fx_animationSheet");
		for (int i = 0; i < 30; i++)
		{
			this.ShaderExplosions[i] = Vector4.one * -1f;
			this.ShaderExplosions[i].w = (this.minSeparationBetweenExplosions - 1f) * 2f;
		}
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x001A8F10 File Offset: 0x001A7110
	private Material DrawComet(RenderTexture source)
	{
		this.largeCometMaterial.SetTexture("_CometTex", this.largeCometTexture.texture);
		this.largeCometMaterial.SetTexture("_ExplosionTex", this.explosionTexture.texture);
		this.largeCometMaterial.SetVector("_CometWorldPosition", this.VisualPositionCentredImage);
		this.largeCometMaterial.SetFloat("_LandingProgress", this.LandingProgress);
		this.largeCometMaterial.SetFloat("_CometWidth", (float)this.templateWidth);
		this.largeCometMaterial.SetFloat("_CometRatio", (float)this.largeCometTexture.texture.height / (float)this.largeCometTexture.texture.width);
		this.largeCometMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
		this.largeCometMaterial.SetVectorArray("_ExplosionLocations", this.ShaderExplosions);
		return this.largeCometMaterial;
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x001A8FFF File Offset: 0x001A71FF
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, LargeComet.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x040030B1 RID: 12465
	private static HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x040030B2 RID: 12466
	public string impactSound;

	// Token: 0x040030B3 RID: 12467
	public string flyingSound;

	// Token: 0x040030B4 RID: 12468
	public int flyingSoundID;

	// Token: 0x040030B6 RID: 12470
	public List<KeyValuePair<string, string>> additionalAnimFiles = new List<KeyValuePair<string, string>>();

	// Token: 0x040030B7 RID: 12471
	public KeyValuePair<string, string> mainAnimFile;

	// Token: 0x040030B8 RID: 12472
	public bool affectedByDifficulty = true;

	// Token: 0x040030B9 RID: 12473
	public bool destroyOnExplode = true;

	// Token: 0x040030BA RID: 12474
	public bool spawnWithOffset;

	// Token: 0x040030BB RID: 12475
	public Vector2I stampLocation;

	// Token: 0x040030BC RID: 12476
	public Vector2I crashPosition;

	// Token: 0x040030BD RID: 12477
	public Dictionary<int, CellOffset> bottomCellsOffsetOfTemplate;

	// Token: 0x040030BE RID: 12478
	public TemplateContainer asteroidTemplate;

	// Token: 0x040030BF RID: 12479
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x040030C0 RID: 12480
	private bool hasExploded;

	// Token: 0x040030C1 RID: 12481
	private float age;

	// Token: 0x040030C2 RID: 12482
	private int lowestTemplateYLocalPosition;

	// Token: 0x040030C3 RID: 12483
	private int templateWidth;

	// Token: 0x040030C4 RID: 12484
	private int worldID;

	// Token: 0x040030C5 RID: 12485
	private Vector3 previousVisualPosition;

	// Token: 0x040030C6 RID: 12486
	private Vector3 initialPosition;

	// Token: 0x040030C7 RID: 12487
	private Vector2I prevCell;

	// Token: 0x040030C8 RID: 12488
	public System.Action OnImpact;

	// Token: 0x040030C9 RID: 12489
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x040030CA RID: 12490
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x040030CB RID: 12491
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x040030CC RID: 12492
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040030CD RID: 12493
	private LoopingSounds loopingSounds;

	// Token: 0x040030CE RID: 12494
	private KBatchedAnimController[] child_controllers;

	// Token: 0x040030CF RID: 12495
	private List<KAnimControllerBase> additionalAnimControllers = new List<KAnimControllerBase>();

	// Token: 0x040030D0 RID: 12496
	private KBatchedAnimController mainChildrenAnimController;

	// Token: 0x040030D1 RID: 12497
	private Vector2I fromStampToCrashPosition;

	// Token: 0x040030D2 RID: 12498
	private HashSet<int> cellsCentrePassedThrough = new HashSet<int>();

	// Token: 0x040030D3 RID: 12499
	private Vector3 activeExplosionPosition = Vector3.zero;

	// Token: 0x040030D4 RID: 12500
	private Material largeCometMaterial;

	// Token: 0x040030D5 RID: 12501
	private Sprite largeCometTexture;

	// Token: 0x040030D6 RID: 12502
	private Sprite explosionTexture;

	// Token: 0x040030D7 RID: 12503
	private float minSeparationBetweenExplosions = 8f;

	// Token: 0x040030D8 RID: 12504
	private Vector3 lastExplosionPosition;

	// Token: 0x040030D9 RID: 12505
	private const string LARGE_COMET_SHADER_NAME = "Klei/DLC4/LargeImpactorCometShader";

	// Token: 0x040030DA RID: 12506
	private const int MAX_SHADER_EXPLOSION_COUNT = 30;

	// Token: 0x040030DB RID: 12507
	private const float EXPLOSION_ANIMATION_FRAME_COUNT = 37f;

	// Token: 0x040030DC RID: 12508
	private const float EXPLOSION_ANIMATION_DURATION = 1.2333333f;

	// Token: 0x040030DD RID: 12509
	private Vector4[] ShaderExplosions = new Vector4[30];
}
