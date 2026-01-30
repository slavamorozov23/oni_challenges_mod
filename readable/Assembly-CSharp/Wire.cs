using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000830 RID: 2096
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Wire")]
public class Wire : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x0600391B RID: 14619 RVA: 0x0013F270 File Offset: 0x0013D470
	public static float GetMaxWattageAsFloat(Wire.WattageRating rating)
	{
		switch (rating)
		{
		case Wire.WattageRating.Max500:
			return 500f;
		case Wire.WattageRating.Max1000:
			return 1000f;
		case Wire.WattageRating.Max2000:
			return 2000f;
		case Wire.WattageRating.Max20000:
			return 20000f;
		case Wire.WattageRating.Max50000:
			return 50000f;
		default:
			return 0f;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x0600391C RID: 14620 RVA: 0x0013F2BC File Offset: 0x0013D4BC
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) is ElectricalUtilityNetwork;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x0600391D RID: 14621 RVA: 0x0013F2F4 File Offset: 0x0013D4F4
	public ushort NetworkID
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) as ElectricalUtilityNetwork;
			if (electricalUtilityNetwork == null)
			{
				return ushort.MaxValue;
			}
			return (ushort)electricalUtilityNetwork.id;
		}
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x0013F338 File Offset: 0x0013D538
	protected override void OnSpawn()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.electricalConduitSystem.AddToNetworks(cell, this, false);
		this.InitializeSwitchState();
		base.Subscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
		base.Subscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireCircuitStatus, this);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireMaxWattageStatus, this);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Wire.OutlineSymbol, false);
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x0013F3C8 File Offset: 0x0013D5C8
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.electricalConduitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06003920 RID: 14624 RVA: 0x0013F454 File Offset: 0x0013D654
	private void InitializeSwitchState()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		bool flag = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			CircuitSwitch component = gameObject.GetComponent<CircuitSwitch>();
			if (component != null)
			{
				flag = true;
				component.AttachWire(this);
			}
		}
		if (!flag)
		{
			this.Connect();
		}
	}

	// Token: 0x06003921 RID: 14625 RVA: 0x0013F4B0 File Offset: 0x0013D6B0
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.electricalConduitSystem.GetConnections(cell, true);
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x0013F4E0 File Offset: 0x0013D6E0
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.electricalConduitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x0013F504 File Offset: 0x0013D704
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06003924 RID: 14628 RVA: 0x0013F50C File Offset: 0x0013D70C
	private void OnBuildingFullyRepaired(object data)
	{
		this.InitializeSwitchState();
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x0013F514 File Offset: 0x0013D714
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.Wires, false);
		if (Wire.WireCircuitStatus == null)
		{
			Wire.WireCircuitStatus = new StatusItem("WireCircuitStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				string wireLoadColor = GameUtil.GetWireLoadColor(wattsUsedByCircuit, maxWattageAsFloat, wattsNeededWhenActive);
				string text = (wattsUsedByCircuit < 0f) ? "?" : GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true);
				str = str.Replace("{CurrentLoadAndColor}", (wireLoadColor == Color.white.ToHexString()) ? text : string.Concat(new string[]
				{
					"<color=#",
					wireLoadColor,
					">",
					text,
					"</color>"
				}));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				str = str.Replace("{WireType}", this.GetProperName());
				return str;
			});
		}
		if (Wire.WireMaxWattageStatus == null)
		{
			Wire.WireMaxWattageStatus = new StatusItem("WireMaxWattageStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				str = str.Replace("{TotalPotentialLoadAndColor}", (wattsNeededWhenActive > maxWattageAsFloat) ? string.Concat(new string[]
				{
					"<color=#",
					new Color(0.9843137f, 0.6901961f, 0.23137255f).ToHexString(),
					">",
					GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true),
					"</color>"
				}) : GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				return str;
			});
		}
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x0013F5CB File Offset: 0x0013D7CB
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.MaxWattageRating;
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x0013F5D3 File Offset: 0x0013D7D3
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x0013F5DC File Offset: 0x0013D7DC
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x0013F624 File Offset: 0x0013D824
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x0013F672 File Offset: 0x0013D872
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x0013F688 File Offset: 0x0013D888
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

	// Token: 0x0600392C RID: 14636 RVA: 0x0013F697 File Offset: 0x0013D897
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x0013F6A4 File Offset: 0x0013D8A4
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x0013F6E0 File Offset: 0x0013D8E0
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x0013F716 File Offset: 0x0013D916
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x040022DE RID: 8926
	[SerializeField]
	public Wire.WattageRating MaxWattageRating;

	// Token: 0x040022DF RID: 8927
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x040022E0 RID: 8928
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x040022E1 RID: 8929
	public float circuitOverloadTime;

	// Token: 0x040022E2 RID: 8930
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x040022E3 RID: 8931
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x040022E4 RID: 8932
	private static StatusItem WireCircuitStatus = null;

	// Token: 0x040022E5 RID: 8933
	private static StatusItem WireMaxWattageStatus = null;

	// Token: 0x040022E6 RID: 8934
	private System.Action firstFrameCallback;

	// Token: 0x020017D8 RID: 6104
	public enum WattageRating
	{
		// Token: 0x040078EC RID: 30956
		Max500,
		// Token: 0x040078ED RID: 30957
		Max1000,
		// Token: 0x040078EE RID: 30958
		Max2000,
		// Token: 0x040078EF RID: 30959
		Max20000,
		// Token: 0x040078F0 RID: 30960
		Max50000,
		// Token: 0x040078F1 RID: 30961
		NumRatings
	}
}
