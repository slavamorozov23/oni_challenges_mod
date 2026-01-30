using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B48 RID: 2888
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimCellOccupier")]
public class SimCellOccupier : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060054F9 RID: 21753 RVA: 0x001EFD90 File Offset: 0x001EDF90
	public bool IsVisuallySolid
	{
		get
		{
			return this.doReplaceElement;
		}
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x001EFD98 File Offset: 0x001EDF98
	protected override void OnPrefabInit()
	{
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		if (this.building.Def.IsFoundation)
		{
			this.setConstructedTile = true;
		}
	}

	// Token: 0x060054FB RID: 21755 RVA: 0x001EFDEC File Offset: 0x001EDFEC
	protected override void OnSpawn()
	{
		HandleVector<Game.CallbackInfo>.Handle callbackHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnModifyComplete), false));
		int num = this.building.Def.PlacementOffsets.Length;
		float mass_per_cell = this.primaryElement.Mass / (float)num;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			if (this.doReplaceElement)
			{
				SimMessages.ReplaceAndDisplaceElement(offset_cell, this.primaryElement.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, mass_per_cell, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount, callbackHandle.index);
				callbackHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
				SimMessages.SetStrength(offset_cell, 0, this.strengthMultiplier);
				Game.Instance.RemoveSolidChangedFilter(offset_cell);
			}
			else
			{
				if (SaveGame.Instance.sandboxEnabled && Grid.Element[offset_cell].IsSolid)
				{
					SimMessages.Dig(offset_cell, -1, false);
				}
				this.ForceSetGameCellData(offset_cell);
				Game.Instance.AddSolidChangedFilter(offset_cell);
			}
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.SetCellProperties(offset_cell, (byte)simCellProperties);
			Grid.RenderedByWorld[offset_cell] = false;
			Game.Instance.GetComponent<EntombedItemVisualizer>().ForceClear(offset_cell);
		});
		base.Subscribe(675471409, new Action<object>(this.OnMelted));
		base.Subscribe<SimCellOccupier>(-1699355994, SimCellOccupier.OnBuildingRepairedDelegate);
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x001EFE94 File Offset: 0x001EE094
	private void OnMelted(object o)
	{
		Building.CreateBuildingMeltedNotification(base.gameObject);
	}

	// Token: 0x060054FD RID: 21757 RVA: 0x001EFEA1 File Offset: 0x001EE0A1
	protected override void OnCleanUp()
	{
		if (this.callDestroy)
		{
			this.DestroySelf(null);
		}
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x001EFEB4 File Offset: 0x001EE0B4
	private Sim.Cell.Properties GetSimCellProperties()
	{
		Sim.Cell.Properties properties = Sim.Cell.Properties.SolidImpermeable;
		if (this.setGasImpermeable)
		{
			properties |= Sim.Cell.Properties.GasImpermeable;
		}
		if (this.setLiquidImpermeable)
		{
			properties |= Sim.Cell.Properties.LiquidImpermeable;
		}
		if (this.setTransparent)
		{
			properties |= Sim.Cell.Properties.Transparent;
		}
		if (this.setOpaque)
		{
			properties |= Sim.Cell.Properties.Opaque;
		}
		if (this.setConstructedTile)
		{
			properties |= Sim.Cell.Properties.ConstructedTile;
		}
		if (this.notifyOnMelt)
		{
			properties |= Sim.Cell.Properties.NotifyOnMelt;
		}
		return properties;
	}

	// Token: 0x060054FF RID: 21759 RVA: 0x001EFF14 File Offset: 0x001EE114
	public void DestroySelf(System.Action onComplete)
	{
		this.callDestroy = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			int num = this.building.PlacementCells[i];
			Game.Instance.RemoveSolidChangedFilter(num);
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.ClearCellProperties(num, (byte)simCellProperties);
			if (this.doReplaceElement && Grid.Element[num].id == this.primaryElement.ElementID)
			{
				HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
				if (handle.IsValid())
				{
					DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
					header.diseaseIdx = Grid.DiseaseIdx[num];
					header.diseaseCount = Grid.DiseaseCount[num];
					GameComps.DiseaseContainers.SetHeader(handle, header);
				}
				if (onComplete != null)
				{
					HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(onComplete, false));
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, handle2.index);
				}
				else
				{
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, -1);
				}
				SimMessages.SetStrength(num, 1, 1f);
			}
			else
			{
				Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
				onComplete.Signal();
				World.Instance.OnSolidChanged(num);
				GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
			}
		}
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x001F00A7 File Offset: 0x001EE2A7
	public bool IsReady()
	{
		return this.isReady;
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x001F00B0 File Offset: 0x001EE2B0
	private void OnModifyComplete()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		this.isReady = true;
		base.GetComponent<PrimaryElement>().SetUseSimDiseaseInfo(true);
		Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
		GameScenePartitioner.Instance.TriggerEvent(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.solidChangedLayer, null);
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x001F011C File Offset: 0x001EE31C
	private void ForceSetGameCellData(int cell)
	{
		bool solid = !Grid.DupePassable[cell];
		Grid.SetSolid(cell, solid, CellEventLogger.Instance.SimCellOccupierForceSolid);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, null);
		Grid.Damage[cell] = 0f;
	}

	// Token: 0x06005503 RID: 21763 RVA: 0x001F0178 File Offset: 0x001EE378
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.movementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None), this.movementSpeedMultiplier - 1f >= 0f)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06005504 RID: 21764 RVA: 0x001F0220 File Offset: 0x001EE420
	private void OnBuildingRepaired(object data)
	{
		BuildingHP buildingHP = (BuildingHP)data;
		float damage = 1f - (float)buildingHP.HitPoints / (float)buildingHP.MaxHitPoints;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			WorldDamage.Instance.RestoreDamageToValue(offset_cell, damage);
		});
	}

	// Token: 0x04003960 RID: 14688
	[MyCmpReq]
	private Building building;

	// Token: 0x04003961 RID: 14689
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04003962 RID: 14690
	[SerializeField]
	public bool doReplaceElement = true;

	// Token: 0x04003963 RID: 14691
	[SerializeField]
	public bool setGasImpermeable;

	// Token: 0x04003964 RID: 14692
	[SerializeField]
	public bool setLiquidImpermeable;

	// Token: 0x04003965 RID: 14693
	[SerializeField]
	public bool setTransparent;

	// Token: 0x04003966 RID: 14694
	[SerializeField]
	public bool setOpaque;

	// Token: 0x04003967 RID: 14695
	[SerializeField]
	public bool notifyOnMelt;

	// Token: 0x04003968 RID: 14696
	[SerializeField]
	private bool setConstructedTile;

	// Token: 0x04003969 RID: 14697
	[SerializeField]
	public float strengthMultiplier = 1f;

	// Token: 0x0400396A RID: 14698
	[SerializeField]
	public float movementSpeedMultiplier = 1f;

	// Token: 0x0400396B RID: 14699
	private bool isReady;

	// Token: 0x0400396C RID: 14700
	private bool callDestroy = true;

	// Token: 0x0400396D RID: 14701
	private static readonly EventSystem.IntraObjectHandler<SimCellOccupier> OnBuildingRepairedDelegate = new EventSystem.IntraObjectHandler<SimCellOccupier>(delegate(SimCellOccupier component, object data)
	{
		component.OnBuildingRepaired(data);
	});
}
