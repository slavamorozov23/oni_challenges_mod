using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AC6 RID: 2758
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x06005025 RID: 20517 RVA: 0x001D16E7 File Offset: 0x001CF8E7
	private int GetTagId(Tag game_tag)
	{
		return GridRestrictionSerializer.Instance.GetTagId(game_tag);
	}

	// Token: 0x06005026 RID: 20518 RVA: 0x001D16F4 File Offset: 0x001CF8F4
	public AccessControl.Permission GetDefaultPermission(Tag groupTag)
	{
		foreach (KeyValuePair<Tag, AccessControl.Permission> keyValuePair in this.defaultPermissionByTag)
		{
			if (keyValuePair.Key == groupTag)
			{
				return keyValuePair.Value;
			}
		}
		return AccessControl.Permission.Both;
	}

	// Token: 0x06005027 RID: 20519 RVA: 0x001D175C File Offset: 0x001CF95C
	public void SetDefaultPermission(Tag groupTag, AccessControl.Permission permission)
	{
		bool flag = false;
		KeyValuePair<Tag, AccessControl.Permission> keyValuePair = new KeyValuePair<Tag, AccessControl.Permission>(groupTag, permission);
		for (int i = 0; i < this.defaultPermissionByTag.Count; i++)
		{
			if (this.defaultPermissionByTag[i].Key == groupTag)
			{
				this.defaultPermissionByTag[i] = keyValuePair;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.defaultPermissionByTag.Add(keyValuePair);
		}
		this.SetStatusItem();
		this.SetGridRestrictions(this.GetTagId(groupTag), permission);
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06005028 RID: 20520 RVA: 0x001D17DA File Offset: 0x001CF9DA
	public bool Online
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005029 RID: 20521 RVA: 0x001D17E0 File Offset: 0x001CF9E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (AccessControl.accessControlActive == null)
		{
			AccessControl.accessControlActive = new StatusItem("accessControlActive", BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		}
		base.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
		base.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
	}

	// Token: 0x0600502A RID: 20522 RVA: 0x001D1854 File Offset: 0x001CFA54
	[Obsolete("Added support for Robots Access Controls")]
	private void CheckForBadData()
	{
		List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> list = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item in this.savedPermissions)
		{
			if (item.Key.Get() == null)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item2 in list)
		{
			this.savedPermissions.Remove(item2);
		}
	}

	// Token: 0x0600502B RID: 20523 RVA: 0x001D1904 File Offset: 0x001CFB04
	private void UpgradeSavePreRobotDoorPermission()
	{
		ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
		for (int i = this.savedPermissions.Count - 1; i >= 0; i--)
		{
			KPrefabID kprefabID = this.savedPermissions[i].Key.Get();
			if (kprefabID != null)
			{
				MinionIdentity component = kprefabID.GetComponent<MinionIdentity>();
				if (component != null)
				{
					pooledList.Add(new global::Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[i].Value));
					this.savedPermissions.RemoveAt(i);
					this.ClearGridRestrictions(kprefabID);
				}
			}
		}
		foreach (global::Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in pooledList)
		{
			this.SetPermission(tuple.first, tuple.second);
		}
		pooledList.Recycle();
	}

	// Token: 0x0600502C RID: 20524 RVA: 0x001D19F8 File Offset: 0x001CFBF8
	private void UpgradeSavesToPostRobotDoorPermissions()
	{
		if (this._defaultPermission != AccessControl.Permission.Both)
		{
			this.SetDefaultPermission(GameTags.Minions.Models.Standard, this._defaultPermission);
			this.SetDefaultPermission(GameTags.Minions.Models.Bionic, this._defaultPermission);
			this._defaultPermission = AccessControl.Permission.Both;
		}
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in this.savedPermissions)
		{
			this.SetPermission(keyValuePair.Key.Get().GetComponent<MinionAssignablesProxy>(), keyValuePair.Value);
		}
		this.savedPermissions.Clear();
	}

	// Token: 0x0600502D RID: 20525 RVA: 0x001D1AA0 File Offset: 0x001CFCA0
	protected override void OnSpawn()
	{
		this.isTeleporter = (base.GetComponent<NavTeleporter>() != null);
		base.OnSpawn();
		if (this.savedPermissions.Count > 0)
		{
			this.CheckForBadData();
		}
		if (this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
		}
		this.UpgradeSavePreRobotDoorPermission();
		this.UpgradeSavesToPostRobotDoorPermissions();
		this.SetStatusItem();
	}

	// Token: 0x0600502E RID: 20526 RVA: 0x001D1B00 File Offset: 0x001CFD00
	protected override void OnCleanUp()
	{
		this.RegisterInGrid(false);
		base.OnCleanUp();
	}

	// Token: 0x0600502F RID: 20527 RVA: 0x001D1B0F File Offset: 0x001CFD0F
	private void OnControlStateChanged(object data)
	{
		this.overrideAccess = ((Boxed<Door.ControlState>)data).value;
		this.SetStatusItem();
	}

	// Token: 0x06005030 RID: 20528 RVA: 0x001D1B28 File Offset: 0x001CFD28
	private void OnCopySettings(object data)
	{
		AccessControl component = ((GameObject)data).GetComponent<AccessControl>();
		if (component != null)
		{
			this.savedPermissionsById.Clear();
			foreach (KeyValuePair<int, AccessControl.Permission> keyValuePair in component.savedPermissionsById)
			{
				this.SetPermission(keyValuePair.Key, keyValuePair.Value);
			}
			this.defaultPermissionByTag = new List<KeyValuePair<Tag, AccessControl.Permission>>(component.defaultPermissionByTag);
			foreach (KeyValuePair<Tag, AccessControl.Permission> keyValuePair2 in this.defaultPermissionByTag)
			{
				this.SetGridRestrictions(this.GetTagId(keyValuePair2.Key), keyValuePair2.Value);
			}
		}
	}

	// Token: 0x06005031 RID: 20529 RVA: 0x001D1C14 File Offset: 0x001CFE14
	public void SetRegistered(bool newRegistered)
	{
		if (newRegistered && !this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
			return;
		}
		if (!newRegistered && this.registered)
		{
			this.RegisterInGrid(false);
		}
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x001D1C44 File Offset: 0x001CFE44
	private void SetPermission(int id, AccessControl.Permission permission)
	{
		bool flag = false;
		for (int i = 0; i < this.savedPermissionsById.Count; i++)
		{
			if (this.savedPermissionsById[i].Key == id)
			{
				flag = true;
				KeyValuePair<int, AccessControl.Permission> keyValuePair = this.savedPermissionsById[i];
				this.savedPermissionsById[i] = new KeyValuePair<int, AccessControl.Permission>(keyValuePair.Key, permission);
				break;
			}
		}
		if (!flag)
		{
			this.savedPermissionsById.Add(new KeyValuePair<int, AccessControl.Permission>(id, permission));
		}
		this.SetStatusItem();
		this.SetGridRestrictions(id, permission);
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x001D1CCD File Offset: 0x001CFECD
	public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
	{
		this.SetPermission(key.GetComponent<KPrefabID>().InstanceID, permission);
	}

	// Token: 0x06005034 RID: 20532 RVA: 0x001D1CE1 File Offset: 0x001CFEE1
	public void SetPermission(Tag gameTag, AccessControl.Permission permission)
	{
		this.SetPermission(this.GetTagId(gameTag), permission);
	}

	// Token: 0x06005035 RID: 20533 RVA: 0x001D1CF4 File Offset: 0x001CFEF4
	private void RestorePermissions()
	{
		foreach (KeyValuePair<Tag, AccessControl.Permission> keyValuePair in this.defaultPermissionByTag)
		{
			this.SetGridRestrictions(this.GetTagId(keyValuePair.Key), keyValuePair.Value);
		}
		foreach (KeyValuePair<int, AccessControl.Permission> keyValuePair2 in this.savedPermissionsById)
		{
			this.SetGridRestrictions(keyValuePair2.Key, keyValuePair2.Value);
		}
	}

	// Token: 0x06005036 RID: 20534 RVA: 0x001D1DAC File Offset: 0x001CFFAC
	private void RegisterInGrid(bool register)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		if (register)
		{
			Rotatable component3 = base.GetComponent<Rotatable>();
			Grid.Restriction.Orientation orientation;
			if (!this.isTeleporter)
			{
				orientation = ((component3 == null || component3.GetOrientation() == Orientation.Neutral) ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal);
			}
			else
			{
				orientation = Grid.Restriction.Orientation.SingleCell;
			}
			if (component != null)
			{
				this.registeredBuildingCells = component.PlacementCells;
				int[] array = this.registeredBuildingCells;
				for (int i = 0; i < array.Length; i++)
				{
					Grid.RegisterRestriction(array[i], orientation);
				}
			}
			else
			{
				foreach (CellOffset offset in component2.OccupiedCellsOffsets)
				{
					Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), orientation);
				}
			}
			if (this.isTeleporter)
			{
				Grid.RegisterRestriction(base.GetComponent<NavTeleporter>().GetCell(), orientation);
			}
		}
		else
		{
			if (component != null)
			{
				if (component.GetMyWorldId() != 255 && this.registeredBuildingCells != null)
				{
					int[] array = this.registeredBuildingCells;
					for (int i = 0; i < array.Length; i++)
					{
						Grid.UnregisterRestriction(array[i]);
					}
					this.registeredBuildingCells = null;
				}
			}
			else
			{
				foreach (CellOffset offset2 in component2.OccupiedCellsOffsets)
				{
					Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset2));
				}
			}
			if (this.isTeleporter)
			{
				int cell = base.GetComponent<NavTeleporter>().GetCell();
				if (cell != Grid.InvalidCell)
				{
					Grid.UnregisterRestriction(cell);
				}
			}
		}
		this.registered = register;
	}

	// Token: 0x06005037 RID: 20535 RVA: 0x001D1F50 File Offset: 0x001D0150
	private void SetGridRestrictions(int id, AccessControl.Permission permission)
	{
		if (!this.registered || !base.isSpawned)
		{
			return;
		}
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		switch (permission)
		{
		case AccessControl.Permission.Both:
			directions = (Grid.Restriction.Directions)0;
			break;
		case AccessControl.Permission.GoLeft:
			directions = Grid.Restriction.Directions.Right;
			break;
		case AccessControl.Permission.GoRight:
			directions = Grid.Restriction.Directions.Left;
			break;
		case AccessControl.Permission.Neither:
			directions = (Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right);
			break;
		}
		if (this.isTeleporter)
		{
			if (directions != (Grid.Restriction.Directions)0)
			{
				directions = Grid.Restriction.Directions.Teleport;
			}
			else
			{
				directions = (Grid.Restriction.Directions)0;
			}
		}
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.SetRestriction(array[i], id, directions);
			}
		}
		else
		{
			foreach (CellOffset offset in component2.OccupiedCellsOffsets)
			{
				Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), id, directions);
			}
		}
		if (this.isTeleporter)
		{
			Grid.SetRestriction(base.GetComponent<NavTeleporter>().GetCell(), id, directions);
		}
	}

	// Token: 0x06005038 RID: 20536 RVA: 0x001D2050 File Offset: 0x001D0250
	private void ClearGridRestrictions(KPrefabID kpid)
	{
		if (kpid == null)
		{
			return;
		}
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int instanceID = kpid.InstanceID;
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.ClearRestriction(array[i], instanceID);
			}
			return;
		}
		foreach (CellOffset offset in component2.OccupiedCellsOffsets)
		{
			Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), instanceID);
		}
	}

	// Token: 0x06005039 RID: 20537 RVA: 0x001D20F8 File Offset: 0x001D02F8
	private void ClearGridRestrictions(int id, Tag default_id)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = this.GetTagId(default_id);
		if (id != Tag.Invalid.GetHash())
		{
			minionInstanceID = id;
		}
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.ClearRestriction(array[i], minionInstanceID);
			}
			return;
		}
		foreach (CellOffset offset in component2.OccupiedCellsOffsets)
		{
			Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID);
		}
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x001D21A9 File Offset: 0x001D03A9
	public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
	{
		return this.GetSetPermission(key.GetComponent<KPrefabID>().InstanceID, key.GetMinionModel());
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x001D21C2 File Offset: 0x001D03C2
	public AccessControl.Permission GetSetPermission(Tag robotTag)
	{
		return this.GetSetPermission(this.GetTagId(robotTag), GameTags.Robot);
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x001D21D8 File Offset: 0x001D03D8
	public AccessControl.Permission GetSetPermission(int primary_id, Tag secondary_id)
	{
		AccessControl.Permission result = this.GetDefaultPermission(secondary_id);
		for (int i = 0; i < this.savedPermissionsById.Count; i++)
		{
			if (this.savedPermissionsById[i].Key == primary_id)
			{
				result = this.savedPermissionsById[i].Value;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600503D RID: 20541 RVA: 0x001D2234 File Offset: 0x001D0434
	public void ClearPermission(MinionAssignablesProxy key)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			this.ClearPermission(component.InstanceID, key.GetMinionModel());
		}
		this.SetStatusItem();
		this.ClearGridRestrictions(component.InstanceID, key.GetMinionModel());
	}

	// Token: 0x0600503E RID: 20542 RVA: 0x001D227C File Offset: 0x001D047C
	public void ClearPermission(Tag tag, Tag default_key)
	{
		int tagId = this.GetTagId(tag);
		this.ClearPermission(tagId, default_key);
	}

	// Token: 0x0600503F RID: 20543 RVA: 0x001D229C File Offset: 0x001D049C
	private void ClearPermission(int key, Tag default_key)
	{
		for (int i = 0; i < this.savedPermissionsById.Count; i++)
		{
			if (this.savedPermissionsById[i].Key == key)
			{
				this.savedPermissionsById.RemoveAt(i);
				break;
			}
		}
		this.SetStatusItem();
		this.ClearGridRestrictions(key, default_key);
	}

	// Token: 0x06005040 RID: 20544 RVA: 0x001D22F4 File Offset: 0x001D04F4
	public bool IsDefaultPermission(MinionAssignablesProxy key)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		return !(component != null) || this.IsDefaultPermission(component.InstanceID);
	}

	// Token: 0x06005041 RID: 20545 RVA: 0x001D231F File Offset: 0x001D051F
	public bool IsDefaultPermission(Tag robotTag)
	{
		return this.IsDefaultPermission(this.GetTagId(robotTag));
	}

	// Token: 0x06005042 RID: 20546 RVA: 0x001D2330 File Offset: 0x001D0530
	private bool IsDefaultPermission(int id)
	{
		bool flag = false;
		for (int i = 0; i < this.savedPermissionsById.Count; i++)
		{
			if (this.savedPermissionsById[i].Key == id)
			{
				flag = true;
				break;
			}
		}
		return !flag;
	}

	// Token: 0x06005043 RID: 20547 RVA: 0x001D2374 File Offset: 0x001D0574
	private void SetStatusItem()
	{
		if (this.overrideAccess == Door.ControlState.Locked)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null, null);
			return;
		}
		if (this.defaultPermissionByTag.Any((KeyValuePair<Tag, AccessControl.Permission> default_permission) => default_permission.Value > AccessControl.Permission.Both) || this.savedPermissionsById.Count > 0)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive, null);
			return;
		}
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null, null);
	}

	// Token: 0x06005044 RID: 20548 RVA: 0x001D2424 File Offset: 0x001D0624
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ACCESS_CONTROL, UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04003584 RID: 13700
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003585 RID: 13701
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003586 RID: 13702
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003587 RID: 13703
	private bool isTeleporter;

	// Token: 0x04003588 RID: 13704
	private int[] registeredBuildingCells;

	// Token: 0x04003589 RID: 13705
	[Serialize]
	[Obsolete("Added support for Robots Access Controls, use savedPermissionsById", false)]
	private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();

	// Token: 0x0400358A RID: 13706
	[Serialize]
	[Obsolete("Added support for Robots Access Controls, use defaultPermissionByTag", false)]
	private AccessControl.Permission _defaultPermission;

	// Token: 0x0400358B RID: 13707
	[Serialize]
	private List<KeyValuePair<Tag, AccessControl.Permission>> defaultPermissionByTag = new List<KeyValuePair<Tag, AccessControl.Permission>>();

	// Token: 0x0400358C RID: 13708
	[Serialize]
	private List<KeyValuePair<int, AccessControl.Permission>> savedPermissionsById = new List<KeyValuePair<int, AccessControl.Permission>>();

	// Token: 0x0400358D RID: 13709
	[Serialize]
	public bool registered = true;

	// Token: 0x0400358E RID: 13710
	[Serialize]
	public bool controlEnabled;

	// Token: 0x0400358F RID: 13711
	public Door.ControlState overrideAccess;

	// Token: 0x04003590 RID: 13712
	private static StatusItem accessControlActive;

	// Token: 0x04003591 RID: 13713
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnControlStateChanged(data);
	});

	// Token: 0x04003592 RID: 13714
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001C0C RID: 7180
	public enum Permission
	{
		// Token: 0x040086CF RID: 34511
		Both,
		// Token: 0x040086D0 RID: 34512
		GoLeft,
		// Token: 0x040086D1 RID: 34513
		GoRight,
		// Token: 0x040086D2 RID: 34514
		Neither
	}
}
