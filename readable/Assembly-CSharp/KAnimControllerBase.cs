using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public abstract class KAnimControllerBase : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06001D4F RID: 7503 RVA: 0x0009FD64 File Offset: 0x0009DF64
	protected KAnimControllerBase()
	{
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.PlaySpeedMultiplier = 1f;
		this.synchronizer = new KAnimSynchronizer(this);
		this.layering = new KAnimLayering(this, this.fgLayer);
		this.isVisible = true;
	}

	// Token: 0x06001D50 RID: 7504
	public abstract KAnim.Anim GetAnim(int index);

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06001D51 RID: 7505 RVA: 0x0009FE61 File Offset: 0x0009E061
	// (set) Token: 0x06001D52 RID: 7506 RVA: 0x0009FE69 File Offset: 0x0009E069
	public KAnim.Build curBuild { get; protected set; }

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001D53 RID: 7507 RVA: 0x0009FE74 File Offset: 0x0009E074
	// (remove) Token: 0x06001D54 RID: 7508 RVA: 0x0009FEAC File Offset: 0x0009E0AC
	public event Action<Color32> OnOverlayColourChanged;

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06001D55 RID: 7509 RVA: 0x0009FEE1 File Offset: 0x0009E0E1
	// (set) Token: 0x06001D56 RID: 7510 RVA: 0x0009FEE9 File Offset: 0x0009E0E9
	public new bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
			if (!this.hasAwakeRun)
			{
				return;
			}
			if (this._enabled)
			{
				this.Enable();
				return;
			}
			this.Disable();
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06001D57 RID: 7511 RVA: 0x0009FF10 File Offset: 0x0009E110
	public bool HasBatchInstanceData
	{
		get
		{
			return this.batchInstanceData != null;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0009FF1B File Offset: 0x0009E11B
	// (set) Token: 0x06001D59 RID: 7513 RVA: 0x0009FF23 File Offset: 0x0009E123
	public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06001D5A RID: 7514 RVA: 0x0009FF2C File Offset: 0x0009E12C
	// (set) Token: 0x06001D5B RID: 7515 RVA: 0x0009FF34 File Offset: 0x0009E134
	public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06001D5C RID: 7516 RVA: 0x0009FF3D File Offset: 0x0009E13D
	// (set) Token: 0x06001D5D RID: 7517 RVA: 0x0009FF50 File Offset: 0x0009E150
	public Color32 TintColour
	{
		get
		{
			return this.batchInstanceData.GetTintColour();
		}
		set
		{
			if (this.batchInstanceData != null && this.batchInstanceData.SetTintColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnTintChanged != null)
				{
					this.OnTintChanged(value);
				}
			}
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06001D5E RID: 7518 RVA: 0x0009FF9E File Offset: 0x0009E19E
	// (set) Token: 0x06001D5F RID: 7519 RVA: 0x0009FFB0 File Offset: 0x0009E1B0
	public Color32 HighlightColour
	{
		get
		{
			return this.batchInstanceData.GetHighlightcolour();
		}
		set
		{
			if (this.batchInstanceData.SetHighlightColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnHighlightChanged != null)
				{
					this.OnHighlightChanged(value);
				}
			}
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06001D60 RID: 7520 RVA: 0x0009FFEB File Offset: 0x0009E1EB
	// (set) Token: 0x06001D61 RID: 7521 RVA: 0x0009FFF8 File Offset: 0x0009E1F8
	public Color OverlayColour
	{
		get
		{
			return this.batchInstanceData.GetOverlayColour();
		}
		set
		{
			if (this.batchInstanceData.SetOverlayColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnOverlayColourChanged != null)
				{
					this.OnOverlayColourChanged(value);
				}
			}
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001D62 RID: 7522 RVA: 0x000A0030 File Offset: 0x0009E230
	// (remove) Token: 0x06001D63 RID: 7523 RVA: 0x000A0068 File Offset: 0x0009E268
	public event KAnimControllerBase.KAnimEvent onAnimEnter;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06001D64 RID: 7524 RVA: 0x000A00A0 File Offset: 0x0009E2A0
	// (remove) Token: 0x06001D65 RID: 7525 RVA: 0x000A00D8 File Offset: 0x0009E2D8
	public event KAnimControllerBase.KAnimEvent onAnimComplete;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06001D66 RID: 7526 RVA: 0x000A0110 File Offset: 0x0009E310
	// (remove) Token: 0x06001D67 RID: 7527 RVA: 0x000A0148 File Offset: 0x0009E348
	public event Action<int> onLayerChanged;

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06001D68 RID: 7528 RVA: 0x000A017D File Offset: 0x0009E37D
	// (set) Token: 0x06001D69 RID: 7529 RVA: 0x000A0185 File Offset: 0x0009E385
	public int previousFrame { get; protected set; }

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06001D6A RID: 7530 RVA: 0x000A018E File Offset: 0x0009E38E
	// (set) Token: 0x06001D6B RID: 7531 RVA: 0x000A0196 File Offset: 0x0009E396
	public int currentFrame { get; protected set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06001D6C RID: 7532 RVA: 0x000A01A0 File Offset: 0x0009E3A0
	public HashedString currentAnim
	{
		get
		{
			if (this.curAnim == null)
			{
				return default(HashedString);
			}
			return this.curAnim.hash;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000A01D3 File Offset: 0x0009E3D3
	// (set) Token: 0x06001D6D RID: 7533 RVA: 0x000A01CA File Offset: 0x0009E3CA
	public float PlaySpeedMultiplier { get; set; }

	// Token: 0x06001D6F RID: 7535 RVA: 0x000A01DB File Offset: 0x0009E3DB
	public void SetFGLayer(Grid.SceneLayer layer)
	{
		this.fgLayer = layer;
		this.GetLayering();
		if (this.layering != null)
		{
			this.layering.SetLayer(this.fgLayer);
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06001D70 RID: 7536 RVA: 0x000A0204 File Offset: 0x0009E404
	// (set) Token: 0x06001D71 RID: 7537 RVA: 0x000A020C File Offset: 0x0009E40C
	public KAnim.PlayMode PlayMode
	{
		get
		{
			return this.mode;
		}
		set
		{
			this.mode = value;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06001D72 RID: 7538 RVA: 0x000A0215 File Offset: 0x0009E415
	// (set) Token: 0x06001D73 RID: 7539 RVA: 0x000A021D File Offset: 0x0009E41D
	public bool FlipX
	{
		get
		{
			return this.flipX;
		}
		set
		{
			this.flipX = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06001D74 RID: 7540 RVA: 0x000A023F File Offset: 0x0009E43F
	// (set) Token: 0x06001D75 RID: 7541 RVA: 0x000A0247 File Offset: 0x0009E447
	public bool FlipY
	{
		get
		{
			return this.flipY;
		}
		set
		{
			this.flipY = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06001D76 RID: 7542 RVA: 0x000A0269 File Offset: 0x0009E469
	// (set) Token: 0x06001D77 RID: 7543 RVA: 0x000A0271 File Offset: 0x0009E471
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.DeRegister();
			this.Register();
			this.RefreshVisibilityListener();
			this.SetDirty();
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06001D78 RID: 7544 RVA: 0x000A02A5 File Offset: 0x0009E4A5
	// (set) Token: 0x06001D79 RID: 7545 RVA: 0x000A02AD File Offset: 0x0009E4AD
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06001D7A RID: 7546 RVA: 0x000A02CF File Offset: 0x0009E4CF
	// (set) Token: 0x06001D7B RID: 7547 RVA: 0x000A02D7 File Offset: 0x0009E4D7
	public Vector3 Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			this.pivot = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06001D7C RID: 7548 RVA: 0x000A02F9 File Offset: 0x0009E4F9
	public Vector3 PositionIncludingOffset
	{
		get
		{
			return base.transform.GetPosition() + this.Offset;
		}
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000A0311 File Offset: 0x0009E511
	public KAnimBatchGroup.MaterialType GetMaterialType()
	{
		return this.materialType;
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000A031C File Offset: 0x0009E51C
	public Vector3 GetWorldPivot()
	{
		Vector3 position = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			position.x += component.offset.x;
			position.y += component.offset.y - component.size.y / 2f;
		}
		return position;
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x000A0384 File Offset: 0x0009E584
	public KAnim.Anim GetCurrentAnim()
	{
		return this.curAnim;
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000A038C File Offset: 0x0009E58C
	public KAnimHashedString GetBuildHash()
	{
		if (this.curBuild == null)
		{
			return KAnimBatchManager.NO_BATCH;
		}
		return this.curBuild.fileHash;
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x000A03AC File Offset: 0x0009E5AC
	protected float GetDuration()
	{
		if (this.curAnim != null)
		{
			return (float)this.curAnim.numFrames / this.curAnim.frameRate;
		}
		return 0f;
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x000A03D4 File Offset: 0x0009E5D4
	protected int GetFrameIdxFromOffset(int offset)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = offset + this.curAnim.firstFrameIdx;
		}
		return result;
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x000A03FC File Offset: 0x0009E5FC
	public int GetFrameIdx(float time, bool absolute)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = this.curAnim.GetFrameIdx(this.mode, time) + (absolute ? this.curAnim.firstFrameIdx : 0);
		}
		return result;
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x000A0439 File Offset: 0x0009E639
	public bool IsStopped()
	{
		return this.stopped;
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06001D85 RID: 7557 RVA: 0x000A0441 File Offset: 0x0009E641
	public KAnim.Anim CurrentAnim
	{
		get
		{
			return this.curAnim;
		}
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000A0449 File Offset: 0x0009E649
	public KAnimSynchronizer GetSynchronizer()
	{
		return this.synchronizer;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000A0451 File Offset: 0x0009E651
	public KAnimLayering GetLayering()
	{
		if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
		{
			this.layering = new KAnimLayering(this, this.fgLayer);
		}
		return this.layering;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000A047D File Offset: 0x0009E67D
	public KAnim.PlayMode GetMode()
	{
		return this.mode;
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000A0485 File Offset: 0x0009E685
	public static string GetModeString(KAnim.PlayMode mode)
	{
		switch (mode)
		{
		case KAnim.PlayMode.Loop:
			return "Loop";
		case KAnim.PlayMode.Once:
			return "Once";
		case KAnim.PlayMode.Paused:
			return "Paused";
		default:
			return "Unknown";
		}
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x000A04B2 File Offset: 0x0009E6B2
	public float GetPlaySpeed()
	{
		return this.playSpeed;
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x000A04BA File Offset: 0x0009E6BA
	public void SetElapsedTime(float value)
	{
		this.elapsedTime = value;
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x000A04C3 File Offset: 0x0009E6C3
	public float GetElapsedTime()
	{
		return this.elapsedTime;
	}

	// Token: 0x06001D8D RID: 7565
	protected abstract void SuspendUpdates(bool suspend);

	// Token: 0x06001D8E RID: 7566
	protected abstract void OnStartQueuedAnim();

	// Token: 0x06001D8F RID: 7567
	public abstract void SetDirty();

	// Token: 0x06001D90 RID: 7568
	protected abstract void RefreshVisibilityListener();

	// Token: 0x06001D91 RID: 7569
	protected abstract void DeRegister();

	// Token: 0x06001D92 RID: 7570
	protected abstract void Register();

	// Token: 0x06001D93 RID: 7571
	protected abstract void OnAwake();

	// Token: 0x06001D94 RID: 7572
	protected abstract void OnStart();

	// Token: 0x06001D95 RID: 7573
	protected abstract void OnStop();

	// Token: 0x06001D96 RID: 7574
	protected abstract void Enable();

	// Token: 0x06001D97 RID: 7575
	protected abstract void Disable();

	// Token: 0x06001D98 RID: 7576
	protected abstract void UpdateFrame(float t);

	// Token: 0x06001D99 RID: 7577
	public abstract Matrix2x3 GetTransformMatrix();

	// Token: 0x06001D9A RID: 7578
	public abstract Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible);

	// Token: 0x06001D9B RID: 7579
	public abstract void UpdateAllHiddenSymbols();

	// Token: 0x06001D9C RID: 7580
	public abstract void UpdateHiddenSymbol(KAnimHashedString specificSymbol);

	// Token: 0x06001D9D RID: 7581
	public abstract void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> specificSymbols);

	// Token: 0x06001D9E RID: 7582
	public abstract void TriggerStop();

	// Token: 0x06001D9F RID: 7583 RVA: 0x000A04CB File Offset: 0x0009E6CB
	public virtual void SetLayer(int layer)
	{
		if (this.onLayerChanged != null)
		{
			this.onLayerChanged(layer);
		}
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x000A04E4 File Offset: 0x0009E6E4
	public Vector3 GetPivotSymbolPosition()
	{
		bool flag = false;
		Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out flag);
		Vector3 position = base.transform.GetPosition();
		if (flag)
		{
			position = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
		}
		return position;
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x000A0533 File Offset: 0x0009E733
	public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		symbolVisible = false;
		return Matrix4x4.identity;
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000A0540 File Offset: 0x0009E740
	private void Awake()
	{
		this.aem = Singleton<AnimEventManager>.Instance;
		this.SetFGLayer(this.fgLayer);
		this.OnAwake();
		if (!string.IsNullOrEmpty(this.initialAnim))
		{
			this.SetDirty();
			this.Play(this.initialAnim, this.initialMode, 1f, 0f);
		}
		this.hasAwakeRun = true;
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000A05A5 File Offset: 0x0009E7A5
	private void Start()
	{
		this.OnStart();
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000A05B0 File Offset: 0x0009E7B0
	protected virtual void OnDestroy()
	{
		this.animFiles = null;
		this.curAnim = null;
		this.curBuild = null;
		this.synchronizer = null;
		this.layering = null;
		this.animQueue = null;
		this.overrideAnims = null;
		this.anims = null;
		this.synchronizer = null;
		this.layering = null;
		this.overrideAnimFiles = null;
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000A060A File Offset: 0x0009E80A
	protected void AnimEnter(HashedString hashed_name)
	{
		if (this.onAnimEnter != null)
		{
			this.onAnimEnter(hashed_name);
		}
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x000A0620 File Offset: 0x0009E820
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000A063C File Offset: 0x0009E83C
	public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		for (int i = 0; i < anim_names.Length - 1; i++)
		{
			this.Queue(anim_names[i], KAnim.PlayMode.Once, 1f, 0f);
		}
		global::Debug.Assert(anim_names.Length != 0, "Play was called with an empty anim array");
		this.Queue(anim_names[anim_names.Length - 1], mode, 1f, 0f);
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000A06AC File Offset: 0x0009E8AC
	public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.animQueue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		this.mode = ((mode == KAnim.PlayMode.Paused) ? KAnim.PlayMode.Paused : KAnim.PlayMode.Once);
		if (this.aem != null)
		{
			this.aem.SetMode(this.eventManagerHandle, this.mode);
		}
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x000A0737 File Offset: 0x0009E937
	public void QueueAndSyncTransition(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.SyncTransition();
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000A074A File Offset: 0x0009E94A
	public void SyncTransition()
	{
		this.elapsedTime %= Mathf.Max(float.Epsilon, this.GetDuration());
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000A0769 File Offset: 0x0009E969
	public void ClearQueue()
	{
		this.animQueue.Clear();
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x000A0778 File Offset: 0x0009E978
	private void Restart(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.curBuild == null)
		{
			string[] array = new string[5];
			array[0] = "[";
			array[1] = base.gameObject.name;
			array[2] = "] Missing build while trying to play anim [";
			int num = 3;
			HashedString hashedString = anim_name;
			array[num] = hashedString.ToString();
			array[4] = "]";
			global::Debug.LogWarning(string.Concat(array), base.gameObject);
			return;
		}
		Queue<KAnimControllerBase.AnimData> queue = new Queue<KAnimControllerBase.AnimData>();
		queue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		while (this.animQueue.Count > 0)
		{
			queue.Enqueue(this.animQueue.Dequeue());
		}
		this.animQueue = queue;
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x000A0858 File Offset: 0x0009EA58
	protected void StartQueuedAnim()
	{
		this.StopAnimEventSequence();
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.SuspendUpdates(false);
		this.stopped = false;
		this.OnStartQueuedAnim();
		KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
		while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
		{
			animData = this.animQueue.Dequeue();
		}
		KAnimControllerBase.AnimLookupData animLookupData;
		if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
		{
			if (!this.anims.TryGetValue(animData.anim, out animLookupData))
			{
				bool flag = true;
				if (this.showWhenMissing != null)
				{
					this.showWhenMissing.SetActive(true);
				}
				if (flag)
				{
					this.TriggerStop();
					return;
				}
			}
			else if (this.showWhenMissing != null)
			{
				this.showWhenMissing.SetActive(false);
			}
		}
		this.curAnim = this.GetAnim(animLookupData.animIndex);
		int num = 0;
		if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
		{
			num = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
		}
		this.prevAnimFrame = -1;
		this.curAnimFrameIdx = this.GetFrameIdxFromOffset(num);
		this.currentFrame = this.curAnimFrameIdx;
		this.mode = animData.mode;
		this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
		this.SetElapsedTime((float)num / this.curAnim.frameRate + animData.timeOffset);
		this.synchronizer.Sync();
		this.StartAnimEventSequence();
		this.AnimEnter(animData.anim);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x000A09DC File Offset: 0x0009EBDC
	public bool GetSymbolVisiblity(KAnimHashedString symbol)
	{
		return !this.hiddenSymbolsSet.Contains(symbol);
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x000A09ED File Offset: 0x0009EBED
	public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
	{
		if (is_visible)
		{
			this.hiddenSymbolsSet.Remove(symbol);
		}
		else if (!this.hiddenSymbolsSet.Contains(symbol))
		{
			this.hiddenSymbolsSet.Add(symbol);
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbol(symbol);
		}
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x000A0A2C File Offset: 0x0009EC2C
	public void BatchSetSymbolsVisiblity(HashSet<KAnimHashedString> symbols, bool is_visible)
	{
		foreach (KAnimHashedString item in symbols)
		{
			if (is_visible)
			{
				this.hiddenSymbolsSet.Remove(item);
			}
			else if (!this.hiddenSymbolsSet.Contains(item))
			{
				this.hiddenSymbolsSet.Add(item);
			}
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(symbols);
		}
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x000A0AB0 File Offset: 0x0009ECB0
	public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0f)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("AddAnimOverrides tried to add a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.AddBuildOverride(kanim_file.GetData(), 0);
		}
		this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData
		{
			priority = priority,
			file = kanim_file
		});
		this.overrideAnimFiles.Sort((KAnimControllerBase.OverrideAnimFileData a, KAnimControllerBase.OverrideAnimFileData b) => b.priority.CompareTo(a.priority));
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x000A0B88 File Offset: 0x0009ED88
	public void RemoveAnimOverrides(KAnimFile kanim_file)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("RemoveAnimOverrides tried to remove a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.TryRemoveBuildOverride(kanim_file.GetData(), 0);
		}
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			if (this.overrideAnimFiles[i].file == kanim_file)
			{
				this.overrideAnimFiles.RemoveAt(i);
				break;
			}
		}
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x000A0C50 File Offset: 0x0009EE50
	private void RebuildOverrides(KAnimFile kanim_file)
	{
		bool flag = false;
		this.overrideAnims.Clear();
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			KAnimControllerBase.OverrideAnimFileData overrideAnimFileData = this.overrideAnimFiles[i];
			KAnimFileData data = overrideAnimFileData.file.GetData();
			for (int j = 0; j < data.animCount; j++)
			{
				KAnim.Anim anim = data.GetAnim(j);
				if (anim.animFile.hashName != data.hashName)
				{
					global::Debug.LogError(string.Format("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", data.name, anim.animFile.name, j));
				}
				KAnimControllerBase.AnimLookupData value = default(KAnimControllerBase.AnimLookupData);
				value.animIndex = anim.index;
				HashedString hashedString = new HashedString(anim.name);
				if (!this.overrideAnims.ContainsKey(hashedString))
				{
					this.overrideAnims[hashedString] = value;
				}
				if (this.curAnim != null && this.curAnim.hash == hashedString && overrideAnimFileData.file == kanim_file)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Restart(this.curAnim.name, this.mode, this.playSpeed, 0f);
		}
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x000A0DA0 File Offset: 0x0009EFA0
	public bool HasAnimation(HashedString anim_name)
	{
		bool flag = anim_name.IsValid;
		if (flag)
		{
			bool flag2 = this.anims.ContainsKey(anim_name);
			bool flag3 = !flag2 && this.overrideAnims.ContainsKey(anim_name);
			flag = (flag2 || flag3);
		}
		return flag;
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x000A0DDC File Offset: 0x0009EFDC
	public KAnim.Anim GetAnim(HashedString anim_name)
	{
		KAnim.Anim result = null;
		if (anim_name.IsValid)
		{
			KAnimControllerBase.AnimLookupData animLookupData;
			if (this.anims.TryGetValue(anim_name, out animLookupData))
			{
				result = this.GetAnim(animLookupData.animIndex);
			}
			else if (this.overrideAnims.TryGetValue(anim_name, out animLookupData))
			{
				result = this.GetAnim(animLookupData.animIndex);
			}
		}
		return result;
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x000A0E34 File Offset: 0x0009F034
	public bool HasAnimationFile(KAnimHashedString anim_file_name)
	{
		KAnimFile kanimFile = null;
		return this.TryGetAnimationFile(anim_file_name, out kanimFile);
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x000A0E4C File Offset: 0x0009F04C
	public bool TryGetAnimationFile(KAnimHashedString anim_file_name, out KAnimFile match)
	{
		match = null;
		if (!anim_file_name.IsValid())
		{
			return false;
		}
		KAnimFileData kanimFileData = null;
		int num = 0;
		int num2 = this.overrideAnimFiles.Count - 1;
		int num3 = (int)((float)this.overrideAnimFiles.Count * 0.5f);
		while (num3 > 0 && match == null && num < num3)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
				break;
			}
			if (this.overrideAnimFiles[num2].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num2].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num2].file;
			}
			num++;
			num2--;
		}
		if (match == null && this.overrideAnimFiles.Count % 2 != 0)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
			}
		}
		kanimFileData = null;
		if (match == null && this.animFiles != null)
		{
			num = 0;
			num2 = this.animFiles.Length - 1;
			num3 = (int)((float)this.animFiles.Length * 0.5f);
			while (num3 > 0 && match == null && num < num3)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
					break;
				}
				if (this.animFiles[num2] != null)
				{
					kanimFileData = this.animFiles[num2].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num2];
				}
				num++;
				num2--;
			}
			if (match == null && this.animFiles.Length % 2 != 0)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
				}
			}
		}
		return match != null;
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x000A1128 File Offset: 0x0009F328
	public void AddAnims(KAnimFile anim_file)
	{
		KAnimFileData data = anim_file.GetData();
		if (data == null)
		{
			global::Debug.LogError("AddAnims() Null animfile data");
			return;
		}
		this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.animFile.hashName != data.hashName)
			{
				global::Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", new object[]
				{
					data.name,
					anim.animFile.name,
					i
				});
			}
			this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData
			{
				animIndex = anim.index
			};
		}
		if (this.usingNewSymbolOverrideSystem && data.buildIndex != -1 && data.build.symbols != null && data.build.symbols.Length != 0)
		{
			base.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x000A122A File Offset: 0x0009F42A
	// (set) Token: 0x06001DBA RID: 7610 RVA: 0x000A1234 File Offset: 0x0009F434
	public KAnimFile[] AnimFiles
	{
		get
		{
			return this.animFiles;
		}
		set
		{
			DebugUtil.AssertArgs(value.Length != 0, new object[]
			{
				"Controller has no anim files.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0] != null, new object[]
			{
				"First anim file needs to be non-null.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0].IsBuildLoaded, new object[]
			{
				"First anim file needs to be the build file.",
				base.gameObject
			});
			for (int i = 0; i < value.Length; i++)
			{
				DebugUtil.AssertArgs(value[i] != null, new object[]
				{
					"Anim file is null",
					base.gameObject
				});
			}
			this.animFiles = new KAnimFile[value.Length];
			for (int j = 0; j < value.Length; j++)
			{
				this.animFiles[j] = value[j];
			}
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06001DBB RID: 7611 RVA: 0x000A1305 File Offset: 0x0009F505
	public IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> OverrideAnimFiles
	{
		get
		{
			return this.overrideAnimFiles;
		}
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x000A1310 File Offset: 0x0009F510
	public void Stop()
	{
		if (this.curAnim != null)
		{
			this.StopAnimEventSequence();
		}
		this.animQueue.Clear();
		this.stopped = true;
		if (this.onAnimComplete != null)
		{
			this.onAnimComplete((this.curAnim == null) ? HashedString.Invalid : this.curAnim.hash);
		}
		this.OnStop();
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x000A1370 File Offset: 0x0009F570
	public void StopAndClear()
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.bounds.center = Vector3.zero;
		this.bounds.extents = Vector3.zero;
		if (this.OnUpdateBounds != null)
		{
			this.OnUpdateBounds(this.bounds);
		}
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x000A13C4 File Offset: 0x0009F5C4
	public float GetPositionPercent()
	{
		return this.GetElapsedTime() / this.GetDuration();
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x000A13D4 File Offset: 0x0009F5D4
	public void SetPositionPercent(float percent)
	{
		if (this.curAnim == null)
		{
			return;
		}
		this.SetElapsedTime(percent * (float)this.curAnim.numFrames / this.curAnim.frameRate);
		int frameIdx = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
		if (this.currentFrame != frameIdx)
		{
			this.SetDirty();
			this.UpdateAnimEventSequenceTime();
			this.SuspendUpdates(false);
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x000A1440 File Offset: 0x0009F640
	protected void StartAnimEventSequence()
	{
		if (!this.layering.GetIsForeground() && this.aem != null)
		{
			this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x000A148F File Offset: 0x0009F68F
	protected void UpdateAnimEventSequenceTime()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
		}
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x000A14C0 File Offset: 0x0009F6C0
	protected void StopAnimEventSequence()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
			}
			this.aem.StopAnim(this.eventManagerHandle);
			this.eventManagerHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000A1526 File Offset: 0x0009F726
	protected void DestroySelf()
	{
		if (this.onDestroySelf != null)
		{
			this.onDestroySelf(base.gameObject);
			return;
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000A154D File Offset: 0x0009F74D
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this.hiddenSymbols.Clear();
		this.hiddenSymbols = new List<KAnimHashedString>(this.hiddenSymbolsSet);
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000A156B File Offset: 0x0009F76B
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.hiddenSymbolsSet = new HashSet<KAnimHashedString>(this.hiddenSymbols);
		this.hiddenSymbols.Clear();
	}

	// Token: 0x04001131 RID: 4401
	[NonSerialized]
	public GameObject showWhenMissing;

	// Token: 0x04001132 RID: 4402
	[SerializeField]
	public KAnimBatchGroup.MaterialType materialType;

	// Token: 0x04001133 RID: 4403
	[SerializeField]
	public string initialAnim;

	// Token: 0x04001134 RID: 4404
	[SerializeField]
	public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;

	// Token: 0x04001135 RID: 4405
	[SerializeField]
	protected KAnimFile[] animFiles = new KAnimFile[0];

	// Token: 0x04001136 RID: 4406
	[SerializeField]
	protected Vector3 offset;

	// Token: 0x04001137 RID: 4407
	[SerializeField]
	protected Vector3 pivot;

	// Token: 0x04001138 RID: 4408
	[SerializeField]
	protected float rotation;

	// Token: 0x04001139 RID: 4409
	[SerializeField]
	public bool destroyOnAnimComplete;

	// Token: 0x0400113A RID: 4410
	[SerializeField]
	public bool inactiveDisable;

	// Token: 0x0400113B RID: 4411
	[SerializeField]
	protected bool flipX;

	// Token: 0x0400113C RID: 4412
	[SerializeField]
	protected bool flipY;

	// Token: 0x0400113D RID: 4413
	[SerializeField]
	public bool forceUseGameTime;

	// Token: 0x0400113E RID: 4414
	public string defaultAnim;

	// Token: 0x0400113F RID: 4415
	protected KAnim.Anim curAnim;

	// Token: 0x04001140 RID: 4416
	protected int curAnimFrameIdx = -1;

	// Token: 0x04001141 RID: 4417
	protected int prevAnimFrame = -1;

	// Token: 0x04001142 RID: 4418
	public bool usingNewSymbolOverrideSystem;

	// Token: 0x04001144 RID: 4420
	protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04001145 RID: 4421
	protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();

	// Token: 0x04001146 RID: 4422
	public bool randomiseLoopedOffset;

	// Token: 0x04001147 RID: 4423
	protected float elapsedTime;

	// Token: 0x04001148 RID: 4424
	protected float playSpeed = 1f;

	// Token: 0x04001149 RID: 4425
	protected KAnim.PlayMode mode = KAnim.PlayMode.Once;

	// Token: 0x0400114A RID: 4426
	protected bool stopped = true;

	// Token: 0x0400114B RID: 4427
	public float animHeight = 1f;

	// Token: 0x0400114C RID: 4428
	public float animWidth = 1f;

	// Token: 0x0400114D RID: 4429
	protected bool isVisible;

	// Token: 0x0400114E RID: 4430
	protected Bounds bounds;

	// Token: 0x0400114F RID: 4431
	public Action<Bounds> OnUpdateBounds;

	// Token: 0x04001150 RID: 4432
	public Action<Color> OnTintChanged;

	// Token: 0x04001151 RID: 4433
	public Action<Color> OnHighlightChanged;

	// Token: 0x04001153 RID: 4435
	protected KAnimSynchronizer synchronizer;

	// Token: 0x04001154 RID: 4436
	protected KAnimLayering layering;

	// Token: 0x04001155 RID: 4437
	[SerializeField]
	protected bool _enabled = true;

	// Token: 0x04001156 RID: 4438
	protected bool hasEnableRun;

	// Token: 0x04001157 RID: 4439
	protected bool hasAwakeRun;

	// Token: 0x04001158 RID: 4440
	protected KBatchedAnimInstanceData batchInstanceData;

	// Token: 0x0400115B RID: 4443
	public KAnimControllerBase.VisibilityType visibilityType;

	// Token: 0x0400115F RID: 4447
	public Action<GameObject> onDestroySelf;

	// Token: 0x04001162 RID: 4450
	[SerializeField]
	protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();

	// Token: 0x04001163 RID: 4451
	[SerializeField]
	protected HashSet<KAnimHashedString> hiddenSymbolsSet = new HashSet<KAnimHashedString>();

	// Token: 0x04001164 RID: 4452
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04001165 RID: 4453
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04001166 RID: 4454
	protected Queue<KAnimControllerBase.AnimData> animQueue = new Queue<KAnimControllerBase.AnimData>();

	// Token: 0x04001167 RID: 4455
	protected int maxSymbols;

	// Token: 0x04001169 RID: 4457
	public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;

	// Token: 0x0400116A RID: 4458
	protected AnimEventManager aem;

	// Token: 0x0400116B RID: 4459
	private static HashedString snaptoPivot = new HashedString("snapTo_pivot");

	// Token: 0x020013E6 RID: 5094
	public struct OverrideAnimFileData
	{
		// Token: 0x04006CA5 RID: 27813
		public float priority;

		// Token: 0x04006CA6 RID: 27814
		public KAnimFile file;
	}

	// Token: 0x020013E7 RID: 5095
	public struct AnimLookupData
	{
		// Token: 0x04006CA7 RID: 27815
		public int animIndex;
	}

	// Token: 0x020013E8 RID: 5096
	public struct AnimData
	{
		// Token: 0x04006CA8 RID: 27816
		public HashedString anim;

		// Token: 0x04006CA9 RID: 27817
		public KAnim.PlayMode mode;

		// Token: 0x04006CAA RID: 27818
		public float speed;

		// Token: 0x04006CAB RID: 27819
		public float timeOffset;
	}

	// Token: 0x020013E9 RID: 5097
	public enum VisibilityType
	{
		// Token: 0x04006CAD RID: 27821
		Default,
		// Token: 0x04006CAE RID: 27822
		OffscreenUpdate,
		// Token: 0x04006CAF RID: 27823
		Always
	}

	// Token: 0x020013EA RID: 5098
	// (Invoke) Token: 0x06008E40 RID: 36416
	public delegate void KAnimEvent(HashedString name);
}
