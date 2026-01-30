using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x02000AE4 RID: 2788
[AddComponentMenu("KMonoBehaviour/scripts/Light2D")]
public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005101 RID: 20737 RVA: 0x001D5913 File Offset: 0x001D3B13
	private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
	{
		if (!EqualityComparer<T>.Default.Equals(old_value, new_value))
		{
			dirty = true;
			return new_value;
		}
		return old_value;
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06005102 RID: 20738 RVA: 0x001D5929 File Offset: 0x001D3B29
	// (set) Token: 0x06005103 RID: 20739 RVA: 0x001D5936 File Offset: 0x001D3B36
	public global::LightShape shape
	{
		get
		{
			return this.pending_emitter_state.shape;
		}
		set
		{
			this.pending_emitter_state.shape = this.MaybeDirty<global::LightShape>(this.pending_emitter_state.shape, value, ref this.dirty_shape);
		}
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06005104 RID: 20740 RVA: 0x001D595B File Offset: 0x001D3B5B
	// (set) Token: 0x06005105 RID: 20741 RVA: 0x001D5963 File Offset: 0x001D3B63
	public LightGridManager.LightGridEmitter emitter { get; private set; }

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06005106 RID: 20742 RVA: 0x001D596C File Offset: 0x001D3B6C
	// (set) Token: 0x06005107 RID: 20743 RVA: 0x001D5979 File Offset: 0x001D3B79
	public Color Color
	{
		get
		{
			return this.pending_emitter_state.colour;
		}
		set
		{
			this.pending_emitter_state.colour = value;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06005108 RID: 20744 RVA: 0x001D5987 File Offset: 0x001D3B87
	// (set) Token: 0x06005109 RID: 20745 RVA: 0x001D5994 File Offset: 0x001D3B94
	public int Lux
	{
		get
		{
			return this.pending_emitter_state.intensity;
		}
		set
		{
			this.pending_emitter_state.intensity = value;
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x0600510A RID: 20746 RVA: 0x001D59A2 File Offset: 0x001D3BA2
	// (set) Token: 0x0600510B RID: 20747 RVA: 0x001D59AF File Offset: 0x001D3BAF
	public DiscreteShadowCaster.Direction LightDirection
	{
		get
		{
			return this.pending_emitter_state.direction;
		}
		set
		{
			this.pending_emitter_state.direction = this.MaybeDirty<DiscreteShadowCaster.Direction>(this.pending_emitter_state.direction, value, ref this.dirty_shape);
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x0600510C RID: 20748 RVA: 0x001D59D4 File Offset: 0x001D3BD4
	// (set) Token: 0x0600510D RID: 20749 RVA: 0x001D59E1 File Offset: 0x001D3BE1
	public int Width
	{
		get
		{
			return this.pending_emitter_state.width;
		}
		set
		{
			this.pending_emitter_state.width = this.MaybeDirty<int>(this.pending_emitter_state.width, value, ref this.dirty_shape);
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x0600510E RID: 20750 RVA: 0x001D5A06 File Offset: 0x001D3C06
	// (set) Token: 0x0600510F RID: 20751 RVA: 0x001D5A13 File Offset: 0x001D3C13
	public float Range
	{
		get
		{
			return this.pending_emitter_state.radius;
		}
		set
		{
			this.pending_emitter_state.radius = this.MaybeDirty<float>(this.pending_emitter_state.radius, value, ref this.dirty_shape);
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06005110 RID: 20752 RVA: 0x001D5A38 File Offset: 0x001D3C38
	// (set) Token: 0x06005111 RID: 20753 RVA: 0x001D5A45 File Offset: 0x001D3C45
	private int origin
	{
		get
		{
			return this.pending_emitter_state.origin;
		}
		set
		{
			this.pending_emitter_state.origin = this.MaybeDirty<int>(this.pending_emitter_state.origin, value, ref this.dirty_position);
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06005112 RID: 20754 RVA: 0x001D5A6A File Offset: 0x001D3C6A
	// (set) Token: 0x06005113 RID: 20755 RVA: 0x001D5A77 File Offset: 0x001D3C77
	public float FalloffRate
	{
		get
		{
			return this.pending_emitter_state.falloffRate;
		}
		set
		{
			this.pending_emitter_state.falloffRate = this.MaybeDirty<float>(this.pending_emitter_state.falloffRate, value, ref this.dirty_falloff);
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06005114 RID: 20756 RVA: 0x001D5A9C File Offset: 0x001D3C9C
	// (set) Token: 0x06005115 RID: 20757 RVA: 0x001D5AA4 File Offset: 0x001D3CA4
	public float IntensityAnimation { get; set; }

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06005116 RID: 20758 RVA: 0x001D5AAD File Offset: 0x001D3CAD
	// (set) Token: 0x06005117 RID: 20759 RVA: 0x001D5AB5 File Offset: 0x001D3CB5
	public Vector2 Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			if (this._offset != value)
			{
				this._offset = value;
				this.origin = Grid.PosToCell(base.transform.GetPosition() + this._offset);
			}
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06005118 RID: 20760 RVA: 0x001D5AF2 File Offset: 0x001D3CF2
	private bool isRegistered
	{
		get
		{
			return this.solidPartitionerEntry != HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x001D5B04 File Offset: 0x001D3D04
	public Light2D()
	{
		this.emitter = new LightGridManager.LightGridEmitter();
		this.Range = 5f;
		this.Lux = 1000;
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x001D5B60 File Offset: 0x001D3D60
	protected override void OnPrefabInit()
	{
		base.Subscribe<Light2D>(-592767678, Light2D.OnOperationalChangedDelegate);
		if (this.disableOnStore)
		{
			base.Subscribe(856640610, new Action<object>(this.OnStore));
		}
		this.IntensityAnimation = 1f;
	}

	// Token: 0x0600511B RID: 20763 RVA: 0x001D5BA0 File Offset: 0x001D3DA0
	private void OnStore(object data)
	{
		global::Debug.Assert(this.disableOnStore, "Only Light2Ds that are disabled on storage should be subscribed to OnStore.");
		Storage storage = data as Storage;
		if (storage != null)
		{
			base.enabled = (storage.GetComponent<ItemPedestal>() != null || storage.GetComponent<MinionIdentity>() != null);
			return;
		}
		base.enabled = true;
	}

	// Token: 0x0600511C RID: 20764 RVA: 0x001D5BF8 File Offset: 0x001D3DF8
	protected override void OnCmpEnable()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		base.OnCmpEnable();
		Components.Light2Ds.Add(this);
		if (base.isSpawned)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, Light2D.OnMovedDispatcher, this, "Light2D.OnMoved");
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x001D5C63 File Offset: 0x001D3E63
	protected override void OnCmpDisable()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
		Components.Light2Ds.Remove(this);
		base.OnCmpDisable();
		this.FullRemove();
	}

	// Token: 0x0600511E RID: 20766 RVA: 0x001D5C8C File Offset: 0x001D3E8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.origin = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		this.cachedCell = this.origin;
		if (base.isActiveAndEnabled)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
	}

	// Token: 0x0600511F RID: 20767 RVA: 0x001D5CF2 File Offset: 0x001D3EF2
	protected override void OnCleanUp()
	{
		this.FullRemove();
	}

	// Token: 0x06005120 RID: 20768 RVA: 0x001D5CFA File Offset: 0x001D3EFA
	private void OnMoved()
	{
		if (base.isSpawned)
		{
			this.FullRefresh();
		}
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x001D5D0A File Offset: 0x001D3F0A
	private HandleVector<int>.Handle AddToLayer(Extents ext, ScenePartitionerLayer layer)
	{
		return GameScenePartitioner.Instance.Add("Light2D", base.gameObject, ext, layer, new Action<object>(this.OnWorldChanged));
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x001D5D30 File Offset: 0x001D3F30
	private Extents ComputeExtents()
	{
		Vector2I vector2I = Grid.CellToXY(this.origin);
		int x = 0;
		int y = 0;
		int width = 0;
		int num = 0;
		global::LightShape shape = this.shape;
		if (shape > global::LightShape.Cone)
		{
			if (shape == global::LightShape.Quad)
			{
				width = this.Width;
				num = (int)this.Range;
				int num2 = (this.Width % 2 == 0) ? (this.Width / 2 - 1) : Mathf.FloorToInt((float)(this.Width - 1) * 0.5f);
				Vector2I vector2I2 = vector2I - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection) * num2;
				x = vector2I2.x;
				switch (this.LightDirection)
				{
				case DiscreteShadowCaster.Direction.North:
					y = vector2I2.y;
					goto IL_119;
				case DiscreteShadowCaster.Direction.South:
					y = vector2I2.y - num;
					goto IL_119;
				}
				y = vector2I2.y - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection).y * num2;
			}
		}
		else
		{
			int num3 = (int)this.Range;
			int num4 = num3 * 2;
			x = vector2I.x - num3;
			y = vector2I.y - num3;
			width = num4;
			num = ((this.shape == global::LightShape.Circle) ? num4 : num3);
		}
		IL_119:
		return new Extents(x, y, width, num);
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x001D5E60 File Offset: 0x001D4060
	private void AddToScenePartitioner()
	{
		Extents ext = this.ComputeExtents();
		this.solidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.solidChangedLayer);
		this.liquidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.liquidChangedLayer);
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x001D5EA2 File Offset: 0x001D40A2
	private void RemoveFromScenePartitioner()
	{
		if (this.isRegistered)
		{
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
		}
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x001D5ECC File Offset: 0x001D40CC
	private void MoveInScenePartitioner()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.ComputeExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.liquidPartitionerEntry, this.ComputeExtents());
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001D5EFA File Offset: 0x001D40FA
	private void EmitterRefresh()
	{
		this.emitter.Refresh(this.pending_emitter_state, true);
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x001D5F0F File Offset: 0x001D410F
	[ContextMenu("Refresh")]
	public void FullRefresh()
	{
		if (!base.isSpawned || !base.isActiveAndEnabled)
		{
			return;
		}
		DebugUtil.DevAssert(this.isRegistered, "shouldn't be refreshing if we aren't spawned and enabled", null);
		this.RefreshShapeAndPosition();
		this.EmitterRefresh();
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x001D5F40 File Offset: 0x001D4140
	public void FullRemove()
	{
		this.RemoveFromScenePartitioner();
		this.emitter.RemoveFromGrid();
		this.cachedCell = Grid.InvalidCell;
	}

	// Token: 0x06005129 RID: 20777 RVA: 0x001D5F60 File Offset: 0x001D4160
	public Light2D.RefreshResult RefreshShapeAndPosition()
	{
		if (!base.isSpawned)
		{
			return Light2D.RefreshResult.None;
		}
		if (!base.isActiveAndEnabled)
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		int num = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (!Grid.IsValidCell(num))
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		this.origin = num;
		if (this.dirty_shape)
		{
			this.RemoveFromScenePartitioner();
			this.AddToScenePartitioner();
		}
		else if (this.dirty_position)
		{
			this.MoveInScenePartitioner();
		}
		if (this.dirty_falloff)
		{
			this.EmitterRefresh();
		}
		this.dirty_shape = false;
		this.dirty_position = false;
		this.dirty_falloff = false;
		this.cachedCell = num;
		return Light2D.RefreshResult.Updated;
	}

	// Token: 0x0600512A RID: 20778 RVA: 0x001D600E File Offset: 0x001D420E
	private void OnWorldChanged(object data)
	{
		this.FullRefresh();
	}

	// Token: 0x0600512B RID: 20779 RVA: 0x001D6018 File Offset: 0x001D4218
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.Range), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false),
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.Lux), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT_LUX, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04003606 RID: 13830
	public bool autoRespondToOperational = true;

	// Token: 0x04003607 RID: 13831
	private bool dirty_shape;

	// Token: 0x04003608 RID: 13832
	private bool dirty_position;

	// Token: 0x04003609 RID: 13833
	private bool dirty_falloff;

	// Token: 0x0400360A RID: 13834
	public int cachedCell;

	// Token: 0x0400360B RID: 13835
	[SerializeField]
	private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;

	// Token: 0x0400360E RID: 13838
	public float Angle;

	// Token: 0x0400360F RID: 13839
	public Vector2 Direction;

	// Token: 0x04003610 RID: 13840
	[SerializeField]
	private Vector2 _offset;

	// Token: 0x04003611 RID: 13841
	public bool drawOverlay;

	// Token: 0x04003612 RID: 13842
	public Color overlayColour;

	// Token: 0x04003613 RID: 13843
	public MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04003614 RID: 13844
	private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04003615 RID: 13845
	private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04003616 RID: 13846
	public bool disableOnStore;

	// Token: 0x04003617 RID: 13847
	private ulong cellChangedHandlerID;

	// Token: 0x04003618 RID: 13848
	private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>(delegate(Light2D light, object data)
	{
		if (light.autoRespondToOperational)
		{
			light.enabled = ((Boxed<bool>)data).value;
		}
	});

	// Token: 0x04003619 RID: 13849
	private static readonly Action<object> OnMovedDispatcher = delegate(object obj)
	{
		Unsafe.As<Light2D>(obj).OnMoved();
	};

	// Token: 0x02001C28 RID: 7208
	public enum RefreshResult
	{
		// Token: 0x0400870F RID: 34575
		None,
		// Token: 0x04008710 RID: 34576
		Removed,
		// Token: 0x04008711 RID: 34577
		Updated
	}
}
