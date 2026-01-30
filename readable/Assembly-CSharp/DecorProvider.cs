using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020008CF RID: 2255
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/DecorProvider")]
public class DecorProvider : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003E78 RID: 15992 RVA: 0x0015DB6C File Offset: 0x0015BD6C
	private void AddDecor()
	{
		this.currDecor = 0f;
		if (this.decor != null)
		{
			this.currDecor = this.decor.GetTotalValue();
		}
		if (this.prefabId.HasTag(GameTags.Stored))
		{
			this.currDecor = 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Transparent[num] && Grid.Solid[num] && this.simCellOccupier == null)
		{
			this.currDecor = 0f;
		}
		if (this.currDecor == 0f)
		{
			return;
		}
		this.cellCount = 0;
		int num2 = 5;
		if (this.decorRadius != null)
		{
			num2 = (int)this.decorRadius.GetTotalValue();
		}
		Extents extents = this.occupyArea.GetExtents();
		extents.x = Mathf.Max(extents.x - num2, 0);
		extents.y = Mathf.Max(extents.y - num2, 0);
		extents.width = Mathf.Min(extents.width + num2 * 2, Grid.WidthInCells - 1);
		extents.height = Mathf.Min(extents.height + num2 * 2, Grid.HeightInCells - 1);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatCollectDecorProviders", base.gameObject, extents, GameScenePartitioner.Instance.decorProviderLayer, this.onCollectDecorProvidersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatSolidCheck", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, this.refreshPartionerCallback);
		int num3 = extents.x + extents.width;
		int num4 = extents.y + extents.height;
		int num5 = extents.x;
		int num6 = extents.y;
		int x;
		int y;
		Grid.CellToXY(num, out x, out y);
		num3 = Math.Min(num3, Grid.WidthInCells);
		num4 = Math.Min(num4, Grid.HeightInCells);
		num5 = Math.Max(0, num5);
		num6 = Math.Max(0, num6);
		int num7 = (num3 - num5) * (num4 - num6);
		if (this.cells == null || this.cells.Length != num7)
		{
			this.cells = new int[num7];
		}
		for (int i = num5; i < num3; i++)
		{
			for (int j = num6; j < num4; j++)
			{
				if (Grid.VisibilityTest(x, y, i, j, false))
				{
					int num8 = Grid.XYToCell(i, j);
					if (Grid.IsValidCell(num8))
					{
						Grid.Decor[num8] += this.currDecor;
						int[] array = this.cells;
						int num9 = this.cellCount;
						this.cellCount = num9 + 1;
						array[num9] = num8;
					}
				}
			}
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0015DE02 File Offset: 0x0015C002
	public void Clear()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		this.RemoveDecor();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0015DE38 File Offset: 0x0015C038
	private void RemoveDecor()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		for (int i = 0; i < this.cellCount; i++)
		{
			int num = this.cells[i];
			if (Grid.IsValidCell(num))
			{
				Grid.Decor[num] -= this.currDecor;
			}
		}
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0015DE8A File Offset: 0x0015C08A
	public void Refresh()
	{
		this.Clear();
		this.AddDecor();
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0015DE98 File Offset: 0x0015C098
	public float GetDecorForCell(int cell)
	{
		for (int i = 0; i < this.cellCount; i++)
		{
			if (this.cells[i] == cell)
			{
				return this.currDecor;
			}
		}
		return 0f;
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0015DECD File Offset: 0x0015C0CD
	public void SetValues(EffectorValues values)
	{
		this.baseDecor = (float)values.amount;
		this.baseRadius = (float)values.radius;
		if (base.IsInitialized())
		{
			this.UpdateBaseDecorModifiers();
		}
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0015DEF8 File Offset: 0x0015C0F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decor = this.GetAttributes().Add(Db.Get().BuildingAttributes.Decor);
		this.decorRadius = this.GetAttributes().Add(Db.Get().BuildingAttributes.DecorRadius);
		this.UpdateBaseDecorModifiers();
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0015DF54 File Offset: 0x0015C154
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectDecorProvidersCallback = new Action<object>(this.OnCollectDecorProviders);
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, DecorProvider.RefreshDispatcher, this, "DecorProvider.OnSpawn");
		AttributeInstance attributeInstance = this.decor;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.decorRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		this.Refresh();
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0015E008 File Offset: 0x0015C208
	private void UpdateBaseDecorModifiers()
	{
		Attributes attributes = this.GetAttributes();
		if (this.baseDecorModifier != null)
		{
			attributes.Remove(this.baseDecorModifier);
			attributes.Remove(this.baseDecorRadiusModifier);
			this.baseDecorModifier = null;
			this.baseDecorRadiusModifier = null;
		}
		if (this.baseDecor != 0f)
		{
			this.baseDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, this.baseDecor, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			this.baseDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, this.baseRadius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(this.baseDecorModifier);
			attributes.Add(this.baseDecorRadiusModifier);
		}
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0015E0D3 File Offset: 0x0015C2D3
	private void OnCollectDecorProviders(object data)
	{
		((List<DecorProvider>)data).Add(this);
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0015E0E1 File Offset: 0x0015C2E1
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.overrideName))
		{
			return base.GetComponent<KSelectable>().GetName();
		}
		return this.overrideName;
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0015E104 File Offset: 0x0015C304
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			AttributeInstance attributeInstance = this.decor;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
			AttributeInstance attributeInstance2 = this.decorRadius;
			attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
		}
		this.Clear();
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0015E178 File Offset: 0x0015C378
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.decor != null && this.decorRadius != null)
		{
			float totalValue = this.decor.GetTotalValue();
			float totalValue2 = this.decorRadius.GetTotalValue();
			string arg = (this.baseDecor > 0f) ? "produced" : "consumed";
			string text = (this.baseDecor > 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			text = text + "\n\n" + this.decor.GetAttributeValueTooltip();
			string text2 = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg, text2, totalValue2), string.Format(text, text2, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseDecor != 0f)
		{
			string arg2 = (this.baseDecor >= 0f) ? "produced" : "consumed";
			string format = (this.baseDecor >= 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			string text3 = GameUtil.AddPositiveSign(this.baseDecor.ToString(), this.baseDecor > 0f);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg2, text3, this.baseRadius), string.Format(format, text3, this.baseRadius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0015E30D File Offset: 0x0015C50D
	public static int GetLightDecorBonus(int cell)
	{
		if (Grid.LightIntensity[cell] > 0)
		{
			return DECOR.LIT_BONUS;
		}
		return 0;
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0015E324 File Offset: 0x0015C524
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x04002687 RID: 9863
	public const string ID = "DecorProvider";

	// Token: 0x04002688 RID: 9864
	public float baseRadius;

	// Token: 0x04002689 RID: 9865
	public float baseDecor;

	// Token: 0x0400268A RID: 9866
	public string overrideName;

	// Token: 0x0400268B RID: 9867
	public System.Action refreshCallback;

	// Token: 0x0400268C RID: 9868
	public Action<object> refreshPartionerCallback;

	// Token: 0x0400268D RID: 9869
	public Action<object> onCollectDecorProvidersCallback;

	// Token: 0x0400268E RID: 9870
	public AttributeInstance decor;

	// Token: 0x0400268F RID: 9871
	public AttributeInstance decorRadius;

	// Token: 0x04002690 RID: 9872
	private AttributeModifier baseDecorModifier;

	// Token: 0x04002691 RID: 9873
	private AttributeModifier baseDecorRadiusModifier;

	// Token: 0x04002692 RID: 9874
	[MyCmpReq]
	private KPrefabID prefabId;

	// Token: 0x04002693 RID: 9875
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04002694 RID: 9876
	[MyCmpGet]
	public SimCellOccupier simCellOccupier;

	// Token: 0x04002695 RID: 9877
	private int[] cells;

	// Token: 0x04002696 RID: 9878
	private int cellCount;

	// Token: 0x04002697 RID: 9879
	public float currDecor;

	// Token: 0x04002698 RID: 9880
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002699 RID: 9881
	private HandleVector<int>.Handle solidChangedPartitionerEntry;

	// Token: 0x0400269A RID: 9882
	private ulong cellChangedHandlerID;

	// Token: 0x0400269B RID: 9883
	private static Action<object> RefreshDispatcher = delegate(object obj)
	{
		Unsafe.As<DecorProvider>(obj).Refresh();
	};
}
