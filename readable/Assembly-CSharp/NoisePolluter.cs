using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A70 RID: 2672
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
	// Token: 0x06004D9C RID: 19868 RVA: 0x001C31CB File Offset: 0x001C13CB
	public static bool IsNoiseableCell(int cell)
	{
		return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell, 0.35f));
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001C31EF File Offset: 0x001C13EF
	public void ResetCells()
	{
		if (this.radius == 0)
		{
			global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
			{
				this.GetName()
			});
			return;
		}
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x001C3213 File Offset: 0x001C1413
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.sourceName = name;
		this.noise = dB;
	}

	// Token: 0x06004D9F RID: 19871 RVA: 0x001C3224 File Offset: 0x001C1424
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x06004DA0 RID: 19872 RVA: 0x001C322C File Offset: 0x001C142C
	public int GetNoise()
	{
		return this.noise;
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001C3234 File Offset: 0x001C1434
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06004DA2 RID: 19874 RVA: 0x001C323C File Offset: 0x001C143C
	public void SetSplat(NoiseSplat new_splat)
	{
		this.splat = new_splat;
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x001C3245 File Offset: 0x001C1445
	public void Clear()
	{
		if (this.splat != null)
		{
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x06004DA4 RID: 19876 RVA: 0x001C3261 File Offset: 0x001C1461
	public Vector2 GetPosition()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06004DA5 RID: 19877 RVA: 0x001C3273 File Offset: 0x001C1473
	// (set) Token: 0x06004DA6 RID: 19878 RVA: 0x001C327B File Offset: 0x001C147B
	public string sourceName { get; private set; }

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x001C3284 File Offset: 0x001C1484
	// (set) Token: 0x06004DA8 RID: 19880 RVA: 0x001C328C File Offset: 0x001C148C
	public bool active { get; private set; }

	// Token: 0x06004DA9 RID: 19881 RVA: 0x001C3295 File Offset: 0x001C1495
	public void SetActive(bool active = true)
	{
		if (!active && this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
		this.active = active;
	}

	// Token: 0x06004DAA RID: 19882 RVA: 0x001C32C4 File Offset: 0x001C14C4
	public void Refresh()
	{
		if (this.active)
		{
			if (this.splat != null)
			{
				AudioEventManager.Get().ClearNoiseSplat(this.splat);
				this.splat.Clear();
			}
			KSelectable component = base.GetComponent<KSelectable>();
			string name = (component != null) ? component.GetName() : base.name;
			GameObject gameObject = base.GetComponent<KMonoBehaviour>().gameObject;
			this.splat = AudioEventManager.Get().CreateNoiseSplat(this.GetPosition(), this.noise, this.radius, name, gameObject);
		}
	}

	// Token: 0x06004DAB RID: 19883 RVA: 0x001C334C File Offset: 0x001C154C
	private void OnActiveChanged(object data)
	{
		bool isActive = ((Operational)data).IsActive;
		this.SetActive(isActive);
		this.Refresh();
	}

	// Token: 0x06004DAC RID: 19884 RVA: 0x001C3372 File Offset: 0x001C1572
	public void SetValues(EffectorValues values)
	{
		this.noise = values.amount;
		this.radius = values.radius;
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001C338C File Offset: 0x001C158C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radius == 0 || this.noise == 0)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] noise: [",
				this.noise.ToString(),
				"] radius: [",
				this.radius.ToString(),
				"]"
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.ResetCells();
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			base.Subscribe<NoisePolluter>(824508782, NoisePolluter.OnActiveChangedDelegate);
		}
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectNoisePollutersCallback = new Action<object>(this.OnCollectNoisePolluters);
		Attributes attributes = this.GetAttributes();
		Db db = Db.Get();
		this.dB = attributes.Add(db.BuildingAttributes.NoisePollution);
		this.dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
		if (this.noise != 0 && this.radius != 0)
		{
			AttributeModifier modifier = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, (float)this.noise, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			AttributeModifier modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id, (float)this.radius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(modifier);
			attributes.Add(modifier2);
		}
		else
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] radius: [",
				this.radius.ToString(),
				"] noise: [",
				this.noise.ToString(),
				"]"
			}));
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.isMovable = (component2 != null && component2.isMovable);
		this.cellChangedHandlerCallback = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, NoisePolluter.RefreshDispatcher, this, "NoisePolluter.OnSpawn");
		AttributeInstance attributeInstance = this.dB;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.dBRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		if (component != null)
		{
			this.OnActiveChanged(component.IsActive);
		}
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001C360C File Offset: 0x001C180C
	private void OnCollectNoisePolluters(object data)
	{
		((List<NoisePolluter>)data).Add(this);
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x001C361A File Offset: 0x001C181A
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.sourceName))
		{
			this.sourceName = base.GetComponent<KSelectable>().GetName();
		}
		return this.sourceName;
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001C3640 File Offset: 0x001C1840
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			if (this.dB != null)
			{
				AttributeInstance attributeInstance = this.dB;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
				AttributeInstance attributeInstance2 = this.dBRadius;
				attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			}
			if (this.isMovable)
			{
				Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerCallback);
			}
		}
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001C36E0 File Offset: 0x001C18E0
	public float GetNoiseForCell(int cell)
	{
		return this.splat.GetDBForCell(cell);
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001C36F0 File Offset: 0x001C18F0
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.dB != null && this.dBRadius != null)
		{
			float totalValue = this.dB.GetTotalValue();
			float totalValue2 = this.dBRadius.GetTotalValue();
			string text = (this.noise > 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			text = text + "\n\n" + this.dB.GetAttributeValueTooltip();
			string arg = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg, totalValue2), string.Format(text, arg, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.noise != 0)
		{
			string format = (this.noise >= 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			string arg2 = GameUtil.AddPositiveSign(this.noise.ToString(), this.noise > 0);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg2, this.radius), string.Format(format, arg2, this.radius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001C3835 File Offset: 0x001C1A35
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x040033B6 RID: 13238
	public const string ID = "NoisePolluter";

	// Token: 0x040033B7 RID: 13239
	public int radius;

	// Token: 0x040033B8 RID: 13240
	public int noise;

	// Token: 0x040033B9 RID: 13241
	public AttributeInstance dB;

	// Token: 0x040033BA RID: 13242
	public AttributeInstance dBRadius;

	// Token: 0x040033BB RID: 13243
	private NoiseSplat splat;

	// Token: 0x040033BD RID: 13245
	public System.Action refreshCallback;

	// Token: 0x040033BE RID: 13246
	public Action<object> refreshPartionerCallback;

	// Token: 0x040033BF RID: 13247
	public Action<object> onCollectNoisePollutersCallback;

	// Token: 0x040033C0 RID: 13248
	public bool isMovable;

	// Token: 0x040033C1 RID: 13249
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x040033C3 RID: 13251
	private ulong cellChangedHandlerCallback;

	// Token: 0x040033C4 RID: 13252
	private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040033C5 RID: 13253
	private static readonly Action<object> RefreshDispatcher = delegate(object obj)
	{
		Unsafe.As<NoisePolluter>(obj).Refresh();
	};
}
