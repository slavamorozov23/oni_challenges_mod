using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AD RID: 1965
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LogicWire")]
public class LogicWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IBitRating, IDisconnectable
{
	// Token: 0x060033BD RID: 13245 RVA: 0x00126475 File Offset: 0x00124675
	public static int GetBitDepthAsInt(LogicWire.BitDepth rating)
	{
		if (rating == LogicWire.BitDepth.OneBit)
		{
			return 1;
		}
		if (rating != LogicWire.BitDepth.FourBit)
		{
			return 0;
		}
		return 4;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x00126488 File Offset: 0x00124688
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.logicCircuitSystem.AddToNetworks(cell, this, false);
		base.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
		base.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
		this.Connect();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x001264F8 File Offset: 0x001246F8
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x060033C0 RID: 13248 RVA: 0x00126584 File Offset: 0x00124784
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.logicCircuitSystem.GetNetworkForCell(cell) is LogicCircuitNetwork;
		}
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x001265BA File Offset: 0x001247BA
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x001265C4 File Offset: 0x001247C4
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x0012660C File Offset: 0x0012480C
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x0012665C File Offset: 0x0012485C
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.logicCircuitSystem.GetConnections(cell, true);
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x0012668C File Offset: 0x0012488C
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.logicCircuitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x001266B0 File Offset: 0x001248B0
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x001266B8 File Offset: 0x001248B8
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x001266C1 File Offset: 0x001248C1
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x001266D7 File Offset: 0x001248D7
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x001266E6 File Offset: 0x001248E6
	public LogicWire.BitDepth GetMaxBitRating()
	{
		return this.MaxBitDepth;
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x001266EE File Offset: 0x001248EE
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x001266FC File Offset: 0x001248FC
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x00126738 File Offset: 0x00124938
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x0012676E File Offset: 0x0012496E
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001F40 RID: 8000
	[SerializeField]
	public LogicWire.BitDepth MaxBitDepth;

	// Token: 0x04001F41 RID: 8001
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001F42 RID: 8002
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x04001F43 RID: 8003
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001F44 RID: 8004
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04001F45 RID: 8005
	private System.Action firstFrameCallback;

	// Token: 0x020016D2 RID: 5842
	public enum BitDepth
	{
		// Token: 0x040075E3 RID: 30179
		OneBit,
		// Token: 0x040075E4 RID: 30180
		FourBit,
		// Token: 0x040075E5 RID: 30181
		NumRatings
	}
}
