using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000729 RID: 1833
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Conduit")]
public class Conduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IDisconnectable, FlowUtilityNetwork.IItem
{
	// Token: 0x06002E02 RID: 11778 RVA: 0x0010B1A4 File Offset: 0x001093A4
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x0010B1BA File Offset: 0x001093BA
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield break;
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x0010B1CC File Offset: 0x001093CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Conduit>(-1201923725, Conduit.OnHighlightedDelegate);
		base.Subscribe<Conduit>(-700727624, Conduit.OnConduitFrozenDelegate);
		base.Subscribe<Conduit>(-1152799878, Conduit.OnConduitBoilingDelegate);
		base.Subscribe<Conduit>(-1555603773, Conduit.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x0010B223 File Offset: 0x00109423
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate);
		base.Subscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x0010B250 File Offset: 0x00109450
	protected virtual void OnStructureTemperatureRegistered(object _)
	{
		int cell = Grid.PosToCell(this);
		this.GetNetworkManager().AddToNetworks(cell, this, false);
		this.Connect();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, this);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().AddThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x0010B2E8 File Offset: 0x001094E8
	protected override void OnCleanUp()
	{
		base.Unsubscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate, false);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().RemoveThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
		}
		base.OnCleanUp();
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x0010B3DC File Offset: 0x001095DC
	protected ConduitFlowVisualizer GetFlowVisualizer()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidFlowVisualizer;
		}
		return Game.Instance.gasFlowVisualizer;
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x0010B3FC File Offset: 0x001095FC
	public IUtilityNetworkMgr GetNetworkManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x0010B41C File Offset: 0x0010961C
	public ConduitFlow GetFlowManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x0010B43C File Offset: 0x0010963C
	public static ConduitFlow GetFlowManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x0010B457 File Offset: 0x00109657
	public static IUtilityNetworkMgr GetNetworkManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x0010B474 File Offset: 0x00109674
	public virtual void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x0010B4A0 File Offset: 0x001096A0
	public virtual bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		return networks.Contains(networkForCell);
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x0010B4C6 File Offset: 0x001096C6
	public virtual int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x0010B4D0 File Offset: 0x001096D0
	private void OnHighlighted(object data)
	{
		int highlightedCell = ((Boxed<bool>)data).value ? Grid.PosToCell(base.transform.GetPosition()) : -1;
		this.GetFlowVisualizer().SetHighlightedCell(highlightedCell);
	}

	// Token: 0x06002E11 RID: 11793 RVA: 0x0010B50C File Offset: 0x0010970C
	private void OnConduitFrozen(object data)
	{
		base.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
			takeDamageEffect = ((this.ConduitType == ConduitType.Gas) ? SpawnFXHashes.BuildingLeakLiquid : SpawnFXHashes.BuildingFreeze),
			fullDamageEffectName = ((this.ConduitType == ConduitType.Gas) ? "water_damage_kanim" : "ice_damage_kanim")
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06002E12 RID: 11794 RVA: 0x0010B5AC File Offset: 0x001097AC
	private void OnConduitBoiling(object data)
	{
		base.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
			takeDamageEffect = SpawnFXHashes.BuildingLeakGas,
			fullDamageEffectName = "gas_damage_kanim"
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x0010B62A File Offset: 0x0010982A
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x0010B632 File Offset: 0x00109832
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x0010B63B File Offset: 0x0010983B
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x0010B644 File Offset: 0x00109844
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			this.GetNetworkManager().ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x0010B685 File Offset: 0x00109885
	public void Disconnect()
	{
		this.disconnected = true;
		this.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x17000260 RID: 608
	// (set) Token: 0x06002E18 RID: 11800 RVA: 0x0010B699 File Offset: 0x00109899
	public FlowUtilityNetwork Network
	{
		set
		{
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06002E19 RID: 11801 RVA: 0x0010B69B File Offset: 0x0010989B
	public int Cell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06002E1A RID: 11802 RVA: 0x0010B6A3 File Offset: 0x001098A3
	public Endpoint EndpointType
	{
		get
		{
			return Endpoint.Conduit;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06002E1B RID: 11803 RVA: 0x0010B6A6 File Offset: 0x001098A6
	public ConduitType ConduitType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06002E1C RID: 11804 RVA: 0x0010B6AE File Offset: 0x001098AE
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x04001B5B RID: 7003
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04001B5C RID: 7004
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001B5D RID: 7005
	public ConduitType type;

	// Token: 0x04001B5E RID: 7006
	private System.Action firstFrameCallback;

	// Token: 0x04001B5F RID: 7007
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnHighlighted(data);
	});

	// Token: 0x04001B60 RID: 7008
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitFrozen(data);
	});

	// Token: 0x04001B61 RID: 7009
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitBoiling(data);
	});

	// Token: 0x04001B62 RID: 7010
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x04001B63 RID: 7011
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001B64 RID: 7012
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
