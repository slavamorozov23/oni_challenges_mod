using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000551 RID: 1361
[DebuggerDisplay("{name} visible={isVisible} suspendUpdates={suspendUpdates} moving={moving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter
{
	// Token: 0x06001DF2 RID: 7666 RVA: 0x000A23F6 File Offset: 0x000A05F6
	public int GetCurrentFrameIndex()
	{
		return this.curAnimFrameIdx;
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x000A23FE File Offset: 0x000A05FE
	public KBatchedAnimInstanceData GetBatchInstanceData()
	{
		return this.batchInstanceData;
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x000A2406 File Offset: 0x000A0606
	// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x000A240E File Offset: 0x000A060E
	protected bool forceRebuild
	{
		get
		{
			return this._forceRebuild;
		}
		set
		{
			this._forceRebuild = value;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x000A2417 File Offset: 0x000A0617
	public bool IsMoving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x000A2420 File Offset: 0x000A0620
	public KBatchedAnimController()
	{
		this.batchInstanceData = new KBatchedAnimInstanceData(this);
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x000A249E File Offset: 0x000A069E
	public bool IsActive()
	{
		return base.isActiveAndEnabled && this._enabled;
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x000A24B0 File Offset: 0x000A06B0
	public bool IsVisible()
	{
		return this.isVisible;
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x000A24B8 File Offset: 0x000A06B8
	public Vector4 GetPositionData()
	{
		if (this.getPositionDataFunctionInUse != null)
		{
			return this.getPositionDataFunctionInUse();
		}
		Vector3 position = base.transform.GetPosition();
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		return new Vector4(position.x, position.y, positionIncludingOffset.x, positionIncludingOffset.y);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x000A250C File Offset: 0x000A070C
	public void SetSymbolScale(KAnimHashedString symbol_name, float scale)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x000A2554 File Offset: 0x000A0754
	public void SetSymbolTint(KAnimHashedString symbol_name, Color color)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x000A259C File Offset: 0x000A079C
	public Vector2I GetCellXY()
	{
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		if (Grid.CellSizeInMeters == 0f)
		{
			return new Vector2I((int)positionIncludingOffset.x, (int)positionIncludingOffset.y);
		}
		return Grid.PosToXY(positionIncludingOffset);
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x000A25D6 File Offset: 0x000A07D6
	public float GetZ()
	{
		return base.transform.GetPosition().z;
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x000A25E8 File Offset: 0x000A07E8
	public string GetName()
	{
		return base.name;
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000A25F0 File Offset: 0x000A07F0
	public override KAnim.Anim GetAnim(int index)
	{
		if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
		{
			global::Debug.LogError(base.name + " batch not ready");
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchGroupID);
		global::Debug.Assert(batchGroupData != null);
		return batchGroupData.GetAnim(index);
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000A2654 File Offset: 0x000A0854
	private void Initialize()
	{
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.DeRegister();
			this.Register();
		}
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x000A268F File Offset: 0x000A088F
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving == this.moving)
		{
			return;
		}
		this.moving = is_moving;
		this.SetDirty();
		this.ConfigureUpdateListener();
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x000A26B0 File Offset: 0x000A08B0
	private void SetBatchGroup(KAnimFileData kafd)
	{
		if (this.batchGroupID.IsValid && kafd != null && this.batchGroupID == kafd.batchTag)
		{
			return;
		}
		DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
		if (kafd == null)
		{
			DebugUtil.Assert(kafd != null, "Null anim data!! For", base.name);
		}
		base.curBuild = kafd.build;
		if (base.curBuild == null)
		{
			DebugUtil.Assert(base.curBuild != null, "Null build for anim!! ", base.name, kafd.name);
		}
		KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(base.curBuild.batchTag);
		HashedString batchGroupID = kafd.build.batchTag;
		if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
		{
			bool isValid = group.swapTarget.IsValid;
			string str = "Invalid swap target fro group [";
			HashedString id = group.id;
			global::Debug.Assert(isValid, str + id.ToString() + "]");
			batchGroupID = group.swapTarget;
		}
		this.batchGroupID = batchGroupID;
		base.symbolInstanceGpuData = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).maxSymbolsPerBuild);
		base.symbolOverrideInfoGpuData = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).symbolFrameInstances.Count);
		if (!this.batchGroupID.IsValid || this.batchGroupID == KAnimBatchManager.NO_BATCH)
		{
			global::Debug.LogError("Batch is not ready: " + base.name);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x000A2858 File Offset: 0x000A0A58
	public void LoadAnims()
	{
		if (!KAnimBatchManager.Instance().isReady)
		{
			global::Debug.LogError("KAnimBatchManager is not ready when loading anim:" + base.name);
		}
		if (this.animFiles.Length == 0)
		{
			DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + base.name);
		}
		if (!this.animFiles[0].IsBuildLoaded)
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				string.Format("First anim file needs to be the build file but {0} doesn't have an associated build", this.animFiles[0].GetData().name)
			});
		}
		this.overrideAnims.Clear();
		this.anims.Clear();
		this.SetBatchGroup(this.animFiles[0].GetData());
		for (int i = 0; i < this.animFiles.Length; i++)
		{
			base.AddAnims(this.animFiles[i]);
		}
		this.forceRebuild = true;
		if (this.layering != null)
		{
			this.layering.HideSymbols();
		}
		if (this.usingNewSymbolOverrideSystem)
		{
			DebugUtil.Assert(base.GetComponent<SymbolOverrideController>() != null);
		}
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x000A2964 File Offset: 0x000A0B64
	public void SwapAnims(KAnimFile[] anims)
	{
		if (this.batchGroupID.IsValid)
		{
			this.DeRegister();
			this.batchGroupID = HashedString.Invalid;
		}
		base.AnimFiles = anims;
		this.LoadAnims();
		if (base.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(this.hiddenSymbolsSet);
		}
		if (this.curAnim != null)
		{
			this.curAnim = base.GetAnim(this.curAnim.name);
			if (this.eventManagerHandle.IsValid() && this.curAnim != null)
			{
				this.aem.SwapAnim(this.eventManagerHandle, this.curAnim);
			}
		}
		this.Register();
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x000A2A0C File Offset: 0x000A0C0C
	public void UpdateAnim(float dt)
	{
		if (this.batch != null && base.transform.hasChanged)
		{
			base.transform.hasChanged = false;
			if (this.batch != null && this.batch.group.maxGroupSize == 1 && this.lastPos.z != base.transform.GetPosition().z)
			{
				this.batch.OverrideZ(base.transform.GetPosition().z);
			}
			Vector3 positionIncludingOffset = base.PositionIncludingOffset;
			this.lastPos = positionIncludingOffset;
			if (this.visibilityType != KAnimControllerBase.VisibilityType.Always && KAnimBatchManager.ControllerToChunkXY(this) != this.lastChunkXY && this.lastChunkXY != KBatchedAnimUpdater.INVALID_CHUNK_ID)
			{
				this.DeRegister();
				this.Register();
			}
			this.SetDirty();
		}
		if (this.batchGroupID == KAnimBatchManager.NO_BATCH || !this.IsActive())
		{
			return;
		}
		if (!this.forceRebuild && (this.mode == KAnim.PlayMode.Paused || this.stopped || this.curAnim == null || (this.mode == KAnim.PlayMode.Once && this.curAnim != null && (this.elapsedTime > this.curAnim.totalTime || this.curAnim.totalTime <= 0f) && this.animQueue.Count == 0)))
		{
			this.SuspendUpdates(true);
		}
		if (!this.isVisible && !this.forceRebuild)
		{
			if (this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate && !this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
			}
			return;
		}
		this.curAnimFrameIdx = base.GetFrameIdx(this.elapsedTime, true);
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			float elapsedTime = this.aem.GetElapsedTime(this.eventManagerHandle);
			if ((int)((this.elapsedTime - elapsedTime) * 100f) != 0)
			{
				base.UpdateAnimEventSequenceTime();
			}
		}
		this.UpdateFrame(this.elapsedTime);
		if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
		{
			base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
		}
		this.forceRebuild = false;
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x000A2C34 File Offset: 0x000A0E34
	protected override void UpdateFrame(float t)
	{
		base.previousFrame = base.currentFrame;
		if (!this.stopped || this.forceRebuild)
		{
			if (this.curAnim != null && (this.mode == KAnim.PlayMode.Loop || this.elapsedTime <= base.GetDuration() || this.forceRebuild))
			{
				base.currentFrame = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
				if (base.currentFrame != base.previousFrame || this.forceRebuild)
				{
					this.SetDirty();
				}
			}
			else
			{
				this.TriggerStop();
			}
			if (!this.stopped && this.mode == KAnim.PlayMode.Loop && base.currentFrame == 0)
			{
				base.AnimEnter(this.curAnim.hash);
			}
		}
		if (this.synchronizer != null)
		{
			this.synchronizer.SyncTime();
		}
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000A2D04 File Offset: 0x000A0F04
	public override void TriggerStop()
	{
		if (this.animQueue.Count > 0)
		{
			base.StartQueuedAnim();
			return;
		}
		if (this.curAnim != null && this.mode == KAnim.PlayMode.Once)
		{
			base.currentFrame = this.curAnim.numFrames - 1;
			base.Stop();
			base.gameObject.Trigger(-1061186183, null);
			if (this.destroyOnAnimComplete)
			{
				base.DestroySelf();
			}
		}
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x000A2D70 File Offset: 0x000A0F70
	public override void UpdateHiddenSymbol(KAnimHashedString symbolToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (!(symbolToUpdate != batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(symbol.symbolIndexInSourceBuild, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x000A2DF8 File Offset: 0x000A0FF8
	public override void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> symbolsToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (symbolsToUpdate.Contains(batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(symbol.symbolIndexInSourceBuild, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x000A2E80 File Offset: 0x000A1080
	public override void UpdateAllHiddenSymbols()
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
			bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
			base.symbolInstanceGpuData.SetVisible(symbol.symbolIndexInSourceBuild, is_visible);
		}
		this.SetDirty();
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x000A2EEE File Offset: 0x000A10EE
	public int GetMaxVisible()
	{
		return this.maxSymbols;
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06001E0D RID: 7693 RVA: 0x000A2EF6 File Offset: 0x000A10F6
	// (set) Token: 0x06001E0E RID: 7694 RVA: 0x000A2EFE File Offset: 0x000A10FE
	public HashedString batchGroupID { get; private set; }

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06001E0F RID: 7695 RVA: 0x000A2F07 File Offset: 0x000A1107
	// (set) Token: 0x06001E10 RID: 7696 RVA: 0x000A2F0F File Offset: 0x000A110F
	public HashedString batchGroupIDOverride { get; private set; }

	// Token: 0x06001E11 RID: 7697 RVA: 0x000A2F18 File Offset: 0x000A1118
	public HashedString GetBatchGroupID(bool isEditorWindow = false)
	{
		global::Debug.Assert(isEditorWindow || this.animFiles == null || this.animFiles.Length == 0 || (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH));
		return this.batchGroupID;
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x000A2F6D File Offset: 0x000A116D
	public HashedString GetBatchGroupIDOverride()
	{
		return this.batchGroupIDOverride;
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x000A2F75 File Offset: 0x000A1175
	public void SetBatchGroupOverride(HashedString id)
	{
		this.batchGroupIDOverride = id;
		this.DeRegister();
		this.Register();
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000A2F8A File Offset: 0x000A118A
	public int GetLayer()
	{
		return base.gameObject.layer;
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x000A2F97 File Offset: 0x000A1197
	public KAnimBatch GetBatch()
	{
		return this.batch;
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x000A2FA0 File Offset: 0x000A11A0
	public void SetBatch(KAnimBatch new_batch)
	{
		this.batch = new_batch;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			KBatchedAnimCanvasRenderer kbatchedAnimCanvasRenderer = base.GetComponent<KBatchedAnimCanvasRenderer>();
			if (kbatchedAnimCanvasRenderer == null && new_batch != null)
			{
				kbatchedAnimCanvasRenderer = base.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
			}
			if (kbatchedAnimCanvasRenderer != null)
			{
				kbatchedAnimCanvasRenderer.SetBatch(this);
			}
		}
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x000A2FEC File Offset: 0x000A11EC
	public int GetCurrentNumFrames()
	{
		if (this.curAnim == null)
		{
			return 0;
		}
		return this.curAnim.numFrames;
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000A3003 File Offset: 0x000A1203
	public int GetFirstFrameIndex()
	{
		if (this.curAnim == null)
		{
			return -1;
		}
		return this.curAnim.firstFrameIdx;
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000A301C File Offset: 0x000A121C
	private Canvas GetRootCanvas()
	{
		if (this.rt == null)
		{
			return null;
		}
		RectTransform rectTransform = this.rt.parent.GetComponent<RectTransform>();
		while (rectTransform != null)
		{
			Canvas component = rectTransform.GetComponent<Canvas>();
			if (component != null && component.isRootCanvas)
			{
				return component;
			}
			rectTransform = ((rectTransform.parent == null) ? null : rectTransform.parent.GetComponent<RectTransform>());
		}
		return null;
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000A3090 File Offset: 0x000A1290
	public override Matrix2x3 GetTransformMatrix()
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = new Vector2(this.animScale * this.animWidth, -this.animScale * this.animHeight);
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000A34B0 File Offset: 0x000A16B0
	public Matrix2x3 GetTransformMatrix(Vector2 customScale)
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = customScale;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x000A38B0 File Offset: 0x000A1AB0
	public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		if (this.curAnimFrameIdx != -1 && this.batch != null)
		{
			Matrix2x3 symbolLocalTransform = this.GetSymbolLocalTransform(symbol, out symbolVisible);
			if (symbolVisible)
			{
				return this.GetTransformMatrix() * symbolLocalTransform;
			}
		}
		symbolVisible = false;
		return default(Matrix4x4);
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000A3900 File Offset: 0x000A1B00
	public override Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible)
	{
		KAnim.Anim.Frame frame;
		if (this.curAnimFrameIdx != -1 && this.batch != null && this.batch.group.data.TryGetFrame(this.curAnimFrameIdx, out frame))
		{
			for (int i = 0; i < frame.numElements; i++)
			{
				int num = frame.firstElementIdx + i;
				if (num < this.batch.group.data.frameElements.Count)
				{
					KAnim.Anim.FrameElement frameElement = this.batch.group.data.frameElements[num];
					if (frameElement.symbol == symbol)
					{
						symbolVisible = true;
						return frameElement.transform;
					}
				}
			}
		}
		symbolVisible = false;
		return Matrix2x3.identity;
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000A39B6 File Offset: 0x000A1BB6
	public override void SetLayer(int layer)
	{
		if (layer == base.gameObject.layer)
		{
			return;
		}
		base.SetLayer(layer);
		this.DeRegister();
		base.gameObject.layer = layer;
		this.Register();
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000A39E6 File Offset: 0x000A1BE6
	public override void SetDirty()
	{
		if (this.batch != null)
		{
			this.batch.SetDirty(this);
		}
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x000A39FC File Offset: 0x000A1BFC
	protected override void OnStartQueuedAnim()
	{
		this.SuspendUpdates(false);
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x000A3A08 File Offset: 0x000A1C08
	protected override void OnAwake()
	{
		this.LoadAnims();
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Default)
		{
			this.visibilityType = ((this.materialType == KAnimBatchGroup.MaterialType.UI) ? KAnimControllerBase.VisibilityType.Always : this.visibilityType);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.UpdateAllHiddenSymbols();
		this.hasEnableRun = false;
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x000A3A78 File Offset: 0x000A1C78
	protected override void OnStart()
	{
		if (this.batch == null)
		{
			this.Initialize();
		}
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			this.ConfigureUpdateListener();
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			this.movingStateChangedHandlerID = instance.RegisterMovementStateChanged(base.transform, KBatchedAnimController.OnMovementStateChangedDispatcher, null);
			this.moving = instance.IsMoving(base.transform);
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.SetDirty();
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x000A3AF0 File Offset: 0x000A1CF0
	protected override void OnStop()
	{
		this.SetDirty();
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000A3AF8 File Offset: 0x000A1CF8
	private void OnEnable()
	{
		if (this._enabled)
		{
			this.Enable();
		}
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000A3B08 File Offset: 0x000A1D08
	protected override void Enable()
	{
		if (this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = true;
		if (this.batch == null)
		{
			this.Initialize();
		}
		this.SetDirty();
		this.SuspendUpdates(false);
		this.ConfigureVisibilityListener(true);
		if (!this.stopped && this.curAnim != null && this.mode != KAnim.PlayMode.Paused && !this.eventManagerHandle.IsValid())
		{
			base.StartAnimEventSequence();
		}
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000A3B73 File Offset: 0x000A1D73
	private void OnDisable()
	{
		this.Disable();
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000A3B7C File Offset: 0x000A1D7C
	protected override void Disable()
	{
		if (App.IsExiting || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = false;
		this.SuspendUpdates(true);
		if (this.batch != null)
		{
			this.DeRegister();
		}
		this.ConfigureVisibilityListener(false);
		base.StopAnimEventSequence();
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x000A3BCC File Offset: 0x000A1DCC
	protected override void OnDestroy()
	{
		if (App.IsExiting)
		{
			return;
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.UnregisterMovementStateChanged(ref this.movingStateChangedHandlerID);
		}
		KBatchedAnimUpdater instance2 = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance2 != null)
		{
			instance2.UpdateUnregister(this);
		}
		this.isVisible = false;
		this.DeRegister();
		this.stopped = true;
		base.StopAnimEventSequence();
		this.batchInstanceData = null;
		this.batch = null;
		base.OnDestroy();
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x000A3C34 File Offset: 0x000A1E34
	public void SetBlendValue(float value)
	{
		this.batchInstanceData.SetBlend(value);
		this.SetDirty();
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x000A3C48 File Offset: 0x000A1E48
	public SymbolOverrideController SetupSymbolOverriding()
	{
		if (!this.symbolOverrideController.IsNullOrDestroyed())
		{
			return this.symbolOverrideController;
		}
		this.usingNewSymbolOverrideSystem = true;
		this.symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(base.gameObject);
		return this.symbolOverrideController;
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x000A3C7C File Offset: 0x000A1E7C
	public bool ApplySymbolOverrides()
	{
		this.batch.atlases.Apply(this.batch.matProperties);
		if (this.symbolOverrideController != null)
		{
			if (this.symbolOverrideControllerVersion != this.symbolOverrideController.version || this.symbolOverrideController.applySymbolOverridesEveryFrame)
			{
				this.symbolOverrideControllerVersion = this.symbolOverrideController.version;
				this.symbolOverrideController.ApplyOverrides();
			}
			this.symbolOverrideController.ApplyAtlases();
			return true;
		}
		return false;
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x000A3CFC File Offset: 0x000A1EFC
	public void SetSymbolOverrides(int symbol_start_idx, int symbol_num_frames, int atlas_idx, KBatchGroupData source_data, int source_start_idx, int source_num_frames)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_start_idx, symbol_num_frames, atlas_idx, source_data, source_start_idx, source_num_frames);
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000A3D12 File Offset: 0x000A1F12
	public void SetSymbolOverride(int symbol_idx, ref KAnim.Build.SymbolFrameInstance symbol_frame_instance)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, ref symbol_frame_instance);
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000A3D24 File Offset: 0x000A1F24
	protected override void Register()
	{
		if (!this.IsActive())
		{
			return;
		}
		if (this.batch != null)
		{
			return;
		}
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.lastChunkXY = KAnimBatchManager.ControllerToChunkXY(this);
			KAnimBatchManager.Instance().Register(this);
			this.forceRebuild = true;
			this.SetDirty();
		}
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x000A3D89 File Offset: 0x000A1F89
	protected override void DeRegister()
	{
		if (this.batch != null)
		{
			this.batch.Deregister(this);
		}
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000A3DA0 File Offset: 0x000A1FA0
	private void ConfigureUpdateListener()
	{
		if ((this.IsActive() && !this.suspendUpdates && this.isVisible) || this.moving || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate || this.visibilityType == KAnimControllerBase.VisibilityType.Always)
		{
			Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
			return;
		}
		Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x000A3DFB File Offset: 0x000A1FFB
	protected override void SuspendUpdates(bool suspend)
	{
		this.suspendUpdates = suspend;
		this.ConfigureUpdateListener();
	}

	// Token: 0x06001E32 RID: 7730 RVA: 0x000A3E0A File Offset: 0x000A200A
	public void SetVisiblity(bool is_visible)
	{
		if (is_visible != this.isVisible)
		{
			this.isVisible = is_visible;
			if (is_visible)
			{
				this.SuspendUpdates(false);
				this.SetDirty();
				base.UpdateAnimEventSequenceTime();
				return;
			}
			this.SuspendUpdates(true);
			this.SetDirty();
		}
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x000A3E40 File Offset: 0x000A2040
	private void ConfigureVisibilityListener(bool enabled)
	{
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			return;
		}
		if (enabled)
		{
			this.RegisterVisibilityListener();
			return;
		}
		this.UnregisterVisibilityListener();
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x000A3E65 File Offset: 0x000A2065
	public virtual KAnimConverter.PostProcessingEffects GetPostProcessingEffectsCompatibility()
	{
		return this.postProcessingEffectsAllowed;
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x000A3E6D File Offset: 0x000A206D
	public float GetPostProcessingParams()
	{
		return this.postProcessingParameters;
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x000A3E75 File Offset: 0x000A2075
	protected override void RefreshVisibilityListener()
	{
		if (!this.visibilityListenerRegistered)
		{
			return;
		}
		this.ConfigureVisibilityListener(false);
		this.ConfigureVisibilityListener(true);
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x000A3E8E File Offset: 0x000A208E
	private void RegisterVisibilityListener()
	{
		DebugUtil.Assert(!this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
		this.visibilityListenerRegistered = true;
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x000A3EB0 File Offset: 0x000A20B0
	private void UnregisterVisibilityListener()
	{
		DebugUtil.Assert(this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
		this.visibilityListenerRegistered = false;
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x000A3ED0 File Offset: 0x000A20D0
	public void SetSceneLayer(Grid.SceneLayer layer)
	{
		float layerZ = Grid.GetLayerZ(layer);
		this.sceneLayer = layer;
		Vector3 position = base.transform.GetPosition();
		position.z = layerZ;
		base.transform.SetPosition(position);
		this.DeRegister();
		this.Register();
	}

	// Token: 0x04001182 RID: 4482
	[NonSerialized]
	protected bool _forceRebuild;

	// Token: 0x04001183 RID: 4483
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x04001184 RID: 4484
	private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;

	// Token: 0x04001185 RID: 4485
	private KAnimBatch batch;

	// Token: 0x04001186 RID: 4486
	public float animScale = 0.005f;

	// Token: 0x04001187 RID: 4487
	private bool suspendUpdates;

	// Token: 0x04001188 RID: 4488
	private bool visibilityListenerRegistered;

	// Token: 0x04001189 RID: 4489
	private bool moving;

	// Token: 0x0400118A RID: 4490
	private ulong movingStateChangedHandlerID;

	// Token: 0x0400118B RID: 4491
	private SymbolOverrideController symbolOverrideController;

	// Token: 0x0400118C RID: 4492
	private int symbolOverrideControllerVersion;

	// Token: 0x0400118D RID: 4493
	[NonSerialized]
	public KBatchedAnimUpdater.RegistrationState updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;

	// Token: 0x0400118E RID: 4494
	public Grid.SceneLayer sceneLayer;

	// Token: 0x0400118F RID: 4495
	private RectTransform rt;

	// Token: 0x04001190 RID: 4496
	private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04001191 RID: 4497
	public Matrix2x3 navMatrix = Matrix2x3.identity;

	// Token: 0x04001192 RID: 4498
	private CanvasScaler scaler;

	// Token: 0x04001193 RID: 4499
	public bool setScaleFromAnim = true;

	// Token: 0x04001194 RID: 4500
	public Vector2 animOverrideSize = Vector2.one;

	// Token: 0x04001195 RID: 4501
	private Canvas rootCanvas;

	// Token: 0x04001196 RID: 4502
	public bool isMovable;

	// Token: 0x04001197 RID: 4503
	public ulong movementChangedHandlerId;

	// Token: 0x04001198 RID: 4504
	public Func<Vector4> getPositionDataFunctionInUse;

	// Token: 0x04001199 RID: 4505
	public KAnimConverter.PostProcessingEffects postProcessingEffectsAllowed;

	// Token: 0x0400119A RID: 4506
	public float postProcessingParameters;

	// Token: 0x0400119B RID: 4507
	private static Action<Transform, bool, object> OnMovementStateChangedDispatcher = delegate(Transform transform, bool is_moving, object ignored)
	{
		transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
	};
}
