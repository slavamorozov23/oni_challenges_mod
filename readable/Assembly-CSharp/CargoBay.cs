using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B67 RID: 2919
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBay : KMonoBehaviour
{
	// Token: 0x0600566B RID: 22123 RVA: 0x001F7820 File Offset: 0x001F5A20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBay>(-1277991738, CargoBay.OnLaunchDelegate);
		base.Subscribe<CargoBay>(-887025858, CargoBay.OnLandDelegate);
		base.Subscribe<CargoBay>(493375141, CargoBay.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBay>(-1697596308, CargoBay.OnStorageChangeDelegate);
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x001F78F8 File Offset: 0x001F5AF8
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0600566D RID: 22125 RVA: 0x001F7954 File Offset: 0x001F5B54
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x0600566E RID: 22126 RVA: 0x001F7978 File Offset: 0x001F5B78
	public void SpawnResources(object data)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		ILaunchableRocket component = base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>();
		if (component.registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			return;
		}
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(component));
		int rootCell = Grid.PosToCell(base.gameObject);
		foreach (KeyValuePair<SimHashes, float> keyValuePair in spacecraftDestination.GetMissionResourceResult(this.storage.RemainingCapacity(), this.reservedResources, this.storageType == CargoBay.CargoType.Solids, this.storageType == CargoBay.CargoType.Liquids, this.storageType == CargoBay.CargoType.Gasses))
		{
			Element element = ElementLoader.FindElementByHash(keyValuePair.Key);
			if (this.storageType == CargoBay.CargoType.Solids && element.IsSolid)
			{
				GameObject gameObject = Scenario.SpawnPrefab(rootCell, 0, 0, element.tag.Name, Grid.SceneLayer.Ore);
				gameObject.GetComponent<PrimaryElement>().Mass = keyValuePair.Value;
				gameObject.GetComponent<PrimaryElement>().Temperature = ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature;
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			else if (this.storageType == CargoBay.CargoType.Liquids && element.IsLiquid)
			{
				this.storage.AddLiquid(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
			}
			else if (this.storageType == CargoBay.CargoType.Gasses && element.IsGas)
			{
				this.storage.AddGasChunk(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
			}
		}
		if (this.storageType == CargoBay.CargoType.Entities)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair2 in spacecraftDestination.GetMissionEntityResult())
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair2.Key);
				if (prefab == null)
				{
					KCrashReporter.Assert(false, "Missing prefab: " + keyValuePair2.Key.Name, null);
				}
				else
				{
					for (int i = 0; i < keyValuePair2.Value; i++)
					{
						GameObject gameObject2 = Util.KInstantiate(prefab, base.transform.position);
						gameObject2.SetActive(true);
						this.storage.Store(gameObject2, false, false, true, false);
						Baggable component2 = gameObject2.GetComponent<Baggable>();
						if (component2 != null)
						{
							component2.keepWrangledNextTimeRemovedFromStorage = true;
							component2.SetWrangled();
						}
					}
				}
			}
		}
	}

	// Token: 0x0600566F RID: 22127 RVA: 0x001F7C68 File Offset: 0x001F5E68
	public void OnLaunch(object data)
	{
		this.ReserveResources();
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			component.conduitType = ConduitType.None;
		}
	}

	// Token: 0x06005670 RID: 22128 RVA: 0x001F7C94 File Offset: 0x001F5E94
	private void ReserveResources()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		ILaunchableRocket component = base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>();
		if (component.registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			return;
		}
		int spacecraftID = SpacecraftManager.instance.GetSpacecraftID(component);
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftID);
		this.reservedResources = spacecraftDestination.ReserveResources(this);
	}

	// Token: 0x06005671 RID: 22129 RVA: 0x001F7CEC File Offset: 0x001F5EEC
	public void OnLand(object data)
	{
		this.SpawnResources(data);
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			CargoBay.CargoType cargoType = this.storageType;
			if (cargoType == CargoBay.CargoType.Liquids)
			{
				component.conduitType = ConduitType.Liquid;
				return;
			}
			if (cargoType == CargoBay.CargoType.Gasses)
			{
				component.conduitType = ConduitType.Gas;
				return;
			}
			component.conduitType = ConduitType.None;
		}
	}

	// Token: 0x04003A4C RID: 14924
	public Storage storage;

	// Token: 0x04003A4D RID: 14925
	private MeterController meter;

	// Token: 0x04003A4E RID: 14926
	[Serialize]
	public float reservedResources;

	// Token: 0x04003A4F RID: 14927
	public CargoBay.CargoType storageType;

	// Token: 0x04003A50 RID: 14928
	public static Dictionary<Element.State, CargoBay.CargoType> ElementStateToCargoTypes = new Dictionary<Element.State, CargoBay.CargoType>
	{
		{
			Element.State.Gas,
			CargoBay.CargoType.Gasses
		},
		{
			Element.State.Liquid,
			CargoBay.CargoType.Liquids
		},
		{
			Element.State.Solid,
			CargoBay.CargoType.Solids
		}
	};

	// Token: 0x04003A51 RID: 14929
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04003A52 RID: 14930
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x04003A53 RID: 14931
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003A54 RID: 14932
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001CD9 RID: 7385
	public enum CargoType
	{
		// Token: 0x0400896A RID: 35178
		Solids,
		// Token: 0x0400896B RID: 35179
		Liquids,
		// Token: 0x0400896C RID: 35180
		Gasses,
		// Token: 0x0400896D RID: 35181
		Entities
	}
}
