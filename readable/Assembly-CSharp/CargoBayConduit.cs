using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003D RID: 61
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBayConduit : KMonoBehaviour
{
	// Token: 0x06000121 RID: 289 RVA: 0x00008DE4 File Offset: 0x00006FE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (CargoBayConduit.connectedPortStatus == null)
		{
			CargoBayConduit.connectedPortStatus = new StatusItem("CONNECTED_ROCKET_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null);
			CargoBayConduit.connectedWrongPortStatus = new StatusItem("CONNECTED_ROCKET_WRONG_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022, null);
			CargoBayConduit.connectedNoPortStatus = new StatusItem("CONNECTED_ROCKET_NO_PORT", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Bad, true, OverlayModes.None.ID, true, 129022, null);
		}
		if (base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad != null)
		{
			this.OnLaunchpadChainChanged(null);
			base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		}
		base.Subscribe<CargoBayConduit>(-1277991738, CargoBayConduit.OnLaunchDelegate);
		base.Subscribe<CargoBayConduit>(-887025858, CargoBayConduit.OnLandDelegate);
		this.storageType = base.GetComponent<CargoBay>().storageType;
		this.UpdateStatusItems();
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00008EF8 File Offset: 0x000070F8
	protected override void OnCleanUp()
	{
		LaunchPad currentPad = base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
		if (currentPad != null)
		{
			currentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00008F3C File Offset: 0x0000713C
	public void OnLaunch(object data)
	{
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			component.conduitType = ConduitType.None;
		}
		base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00008F88 File Offset: 0x00007188
	public void OnLand(object data)
	{
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			CargoBay.CargoType cargoType = this.storageType;
			if (cargoType != CargoBay.CargoType.Liquids)
			{
				if (cargoType == CargoBay.CargoType.Gasses)
				{
					component.conduitType = ConduitType.Gas;
				}
				else
				{
					component.conduitType = ConduitType.None;
				}
			}
			else
			{
				component.conduitType = ConduitType.Liquid;
			}
		}
		base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		this.UpdateStatusItems();
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00008FFA File Offset: 0x000071FA
	private void OnLaunchpadChainChanged(object data)
	{
		this.UpdateStatusItems();
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00009004 File Offset: 0x00007204
	private void UpdateStatusItems()
	{
		bool flag;
		bool flag2;
		this.HasMatchingConduitPort(out flag, out flag2);
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedPortStatus, this);
			return;
		}
		if (flag2)
		{
			this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedWrongPortStatus, this);
			return;
		}
		this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedNoPortStatus, this);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00009074 File Offset: 0x00007274
	private void HasMatchingConduitPort(out bool hasMatch, out bool hasAny)
	{
		hasMatch = false;
		hasAny = false;
		LaunchPad currentPad = base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
		if (currentPad == null)
		{
			return;
		}
		ChainedBuilding.StatesInstance smi = currentPad.GetSMI<ChainedBuilding.StatesInstance>();
		if (smi == null)
		{
			return;
		}
		HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
		smi.GetLinkedBuildings(ref pooledHashSet);
		foreach (ChainedBuilding.StatesInstance statesInstance in pooledHashSet)
		{
			IConduitDispenser component = statesInstance.GetComponent<IConduitDispenser>();
			if (component != null)
			{
				hasAny = true;
				if (CargoBayConduit.ElementToCargoMap[component.ConduitType] == this.storageType)
				{
					hasMatch = true;
					break;
				}
			}
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x040000B2 RID: 178
	public static Dictionary<ConduitType, CargoBay.CargoType> ElementToCargoMap = new Dictionary<ConduitType, CargoBay.CargoType>
	{
		{
			ConduitType.Solid,
			CargoBay.CargoType.Solids
		},
		{
			ConduitType.Liquid,
			CargoBay.CargoType.Liquids
		},
		{
			ConduitType.Gas,
			CargoBay.CargoType.Gasses
		}
	};

	// Token: 0x040000B3 RID: 179
	private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>(delegate(CargoBayConduit component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x040000B4 RID: 180
	private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>(delegate(CargoBayConduit component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x040000B5 RID: 181
	private static StatusItem connectedPortStatus;

	// Token: 0x040000B6 RID: 182
	private static StatusItem connectedWrongPortStatus;

	// Token: 0x040000B7 RID: 183
	private static StatusItem connectedNoPortStatus;

	// Token: 0x040000B8 RID: 184
	private CargoBay.CargoType storageType;

	// Token: 0x040000B9 RID: 185
	private Guid connectedConduitPortStatusItem;
}
