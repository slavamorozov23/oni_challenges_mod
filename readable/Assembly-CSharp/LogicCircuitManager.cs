using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009EC RID: 2540
public class LogicCircuitManager
{
	// Token: 0x060049FD RID: 18941 RVA: 0x001AD114 File Offset: 0x001AB314
	public LogicCircuitManager(UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduit_system)
	{
		this.conduitSystem = conduit_system;
		this.timeSinceBridgeRefresh = 0f;
		this.elapsedTime = 0f;
		for (int i = 0; i < 2; i++)
		{
			this.bridgeGroups[i] = new List<LogicUtilityNetworkLink>();
		}
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x001AD174 File Offset: 0x001AB374
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x001AD180 File Offset: 0x001AB380
	private void Refresh(float dt)
	{
		if (this.conduitSystem.IsDirty)
		{
			this.conduitSystem.Update();
			LogicCircuitNetwork.logicSoundRegister.Clear();
			this.PropagateSignals(true);
			this.elapsedTime = 0f;
		}
		else if (SpeedControlScreen.Instance != null && !SpeedControlScreen.Instance.IsPaused)
		{
			this.elapsedTime += dt;
			this.timeSinceBridgeRefresh += dt;
			while (this.elapsedTime > LogicCircuitManager.ClockTickInterval)
			{
				this.elapsedTime -= LogicCircuitManager.ClockTickInterval;
				this.PropagateSignals(false);
				if (this.onLogicTick != null)
				{
					this.onLogicTick();
				}
			}
			if (this.timeSinceBridgeRefresh > LogicCircuitManager.BridgeRefreshInterval)
			{
				this.UpdateCircuitBridgeLists();
				this.timeSinceBridgeRefresh = 0f;
			}
		}
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			this.CheckCircuitOverloaded(dt, logicCircuitNetwork.id, logicCircuitNetwork.GetBitsUsed());
		}
	}

	// Token: 0x06004A00 RID: 18944 RVA: 0x001AD2B4 File Offset: 0x001AB4B4
	private void PropagateSignals(bool force_send_events)
	{
		IList<UtilityNetwork> networks = Game.Instance.logicCircuitSystem.GetNetworks();
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			((LogicCircuitNetwork)utilityNetwork).UpdateLogicValue();
		}
		foreach (UtilityNetwork utilityNetwork2 in networks)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork2;
			logicCircuitNetwork.SendLogicEvents(force_send_events, logicCircuitNetwork.id);
		}
	}

	// Token: 0x06004A01 RID: 18945 RVA: 0x001AD350 File Offset: 0x001AB550
	public LogicCircuitNetwork GetNetworkForCell(int cell)
	{
		return this.conduitSystem.GetNetworkForCell(cell) as LogicCircuitNetwork;
	}

	// Token: 0x06004A02 RID: 18946 RVA: 0x001AD363 File Offset: 0x001AB563
	public void AddVisElem(ILogicUIElement elem)
	{
		this.uiVisElements.Add(elem);
		if (this.onElemAdded != null)
		{
			this.onElemAdded(elem);
		}
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x001AD385 File Offset: 0x001AB585
	public void RemoveVisElem(ILogicUIElement elem)
	{
		if (this.onElemRemoved != null)
		{
			this.onElemRemoved(elem);
		}
		this.uiVisElements.Remove(elem);
	}

	// Token: 0x06004A04 RID: 18948 RVA: 0x001AD3A8 File Offset: 0x001AB5A8
	public List<ILogicUIElement> GetVisElements()
	{
		return this.uiVisElements;
	}

	// Token: 0x06004A05 RID: 18949 RVA: 0x001AD3B0 File Offset: 0x001AB5B0
	public static void ToggleNoWireConnected(bool show_missing_wire, GameObject go)
	{
		go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoLogicWireConnected, show_missing_wire, null);
	}

	// Token: 0x06004A06 RID: 18950 RVA: 0x001AD3D0 File Offset: 0x001AB5D0
	private void CheckCircuitOverloaded(float dt, int id, int bits_used)
	{
		UtilityNetwork networkByID = Game.Instance.logicCircuitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)networkByID;
			if (logicCircuitNetwork != null)
			{
				logicCircuitNetwork.UpdateOverloadTime(dt, bits_used);
			}
		}
	}

	// Token: 0x06004A07 RID: 18951 RVA: 0x001AD403 File Offset: 0x001AB603
	public void Connect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Add(bridge);
	}

	// Token: 0x06004A08 RID: 18952 RVA: 0x001AD418 File Offset: 0x001AB618
	public void Disconnect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Remove(bridge);
	}

	// Token: 0x06004A09 RID: 18953 RVA: 0x001AD430 File Offset: 0x001AB630
	private void UpdateCircuitBridgeLists()
	{
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			if (this.updateEvenBridgeGroups)
			{
				if (logicCircuitNetwork.id % 2 == 0)
				{
					logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
				}
			}
			else if (logicCircuitNetwork.id % 2 == 1)
			{
				logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
			}
		}
		this.updateEvenBridgeGroups = !this.updateEvenBridgeGroups;
	}

	// Token: 0x0400311F RID: 12575
	public static float ClockTickInterval = 0.1f;

	// Token: 0x04003120 RID: 12576
	private float elapsedTime;

	// Token: 0x04003121 RID: 12577
	private UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduitSystem;

	// Token: 0x04003122 RID: 12578
	private List<ILogicUIElement> uiVisElements = new List<ILogicUIElement>();

	// Token: 0x04003123 RID: 12579
	public static float BridgeRefreshInterval = 1f;

	// Token: 0x04003124 RID: 12580
	private List<LogicUtilityNetworkLink>[] bridgeGroups = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04003125 RID: 12581
	private bool updateEvenBridgeGroups;

	// Token: 0x04003126 RID: 12582
	private float timeSinceBridgeRefresh;

	// Token: 0x04003127 RID: 12583
	public System.Action onLogicTick;

	// Token: 0x04003128 RID: 12584
	public Action<ILogicUIElement> onElemAdded;

	// Token: 0x04003129 RID: 12585
	public Action<ILogicUIElement> onElemRemoved;

	// Token: 0x02001A4F RID: 6735
	private struct Signal
	{
		// Token: 0x0600A52F RID: 42287 RVA: 0x003B602F File Offset: 0x003B422F
		public Signal(int cell, int value)
		{
			this.cell = cell;
			this.value = value;
		}

		// Token: 0x04008153 RID: 33107
		public int cell;

		// Token: 0x04008154 RID: 33108
		public int value;
	}
}
