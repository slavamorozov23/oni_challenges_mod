using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B68 RID: 2920
public class CargoBayCluster : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06005675 RID: 22133 RVA: 0x001F7DFD File Offset: 0x001F5FFD
	// (set) Token: 0x06005676 RID: 22134 RVA: 0x001F7E05 File Offset: 0x001F6005
	public float UserMaxCapacity
	{
		get
		{
			return this.userMaxCapacity;
		}
		set
		{
			this.userMaxCapacity = value;
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06005677 RID: 22135 RVA: 0x001F7E1A File Offset: 0x001F601A
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06005678 RID: 22136 RVA: 0x001F7E21 File Offset: 0x001F6021
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06005679 RID: 22137 RVA: 0x001F7E2E File Offset: 0x001F602E
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x0600567A RID: 22138 RVA: 0x001F7E3B File Offset: 0x001F603B
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x0600567B RID: 22139 RVA: 0x001F7E3E File Offset: 0x001F603E
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x0600567C RID: 22140 RVA: 0x001F7E46 File Offset: 0x001F6046
	public float RemainingCapacity
	{
		get
		{
			return this.userMaxCapacity - this.storage.MassStored();
		}
	}

	// Token: 0x0600567D RID: 22141 RVA: 0x001F7E5A File Offset: 0x001F605A
	protected override void OnPrefabInit()
	{
		this.userMaxCapacity = this.storage.capacityKg;
	}

	// Token: 0x0600567E RID: 22142 RVA: 0x001F7E70 File Offset: 0x001F6070
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBayCluster>(493375141, CargoBayCluster.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
		component.matchParentOffset = true;
		component.forceAlwaysAlive = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBayCluster>(-1697596308, CargoBayCluster.OnStorageChangeDelegate);
	}

	// Token: 0x0600567F RID: 22143 RVA: 0x001F7F30 File Offset: 0x001F6130
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005680 RID: 22144 RVA: 0x001F7F8C File Offset: 0x001F618C
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		this.UpdateCargoStatusItem();
	}

	// Token: 0x06005681 RID: 22145 RVA: 0x001F7FB8 File Offset: 0x001F61B8
	private void UpdateCargoStatusItem()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		CraftModuleInterface craftInterface = component.CraftInterface;
		if (craftInterface == null)
		{
			return;
		}
		Clustercraft component2 = craftInterface.GetComponent<Clustercraft>();
		if (component2 == null)
		{
			return;
		}
		component2.UpdateStatusItem();
	}

	// Token: 0x04003A55 RID: 14933
	private MeterController meter;

	// Token: 0x04003A56 RID: 14934
	[SerializeField]
	public Storage storage;

	// Token: 0x04003A57 RID: 14935
	[SerializeField]
	public CargoBay.CargoType storageType;

	// Token: 0x04003A58 RID: 14936
	[Serialize]
	private float userMaxCapacity;

	// Token: 0x04003A59 RID: 14937
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003A5A RID: 14938
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnStorageChange(data);
	});
}
