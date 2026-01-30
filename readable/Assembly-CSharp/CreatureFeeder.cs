using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000CE5 RID: 3301
[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
	// Token: 0x060065EA RID: 26090 RVA: 0x002662F9 File Offset: 0x002644F9
	protected override void OnSpawn()
	{
		this.storages = base.GetComponents<Storage>();
		Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
		base.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
	}

	// Token: 0x060065EB RID: 26091 RVA: 0x00266329 File Offset: 0x00264529
	protected override void OnCleanUp()
	{
		Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
	}

	// Token: 0x060065EC RID: 26092 RVA: 0x0026633C File Offset: 0x0026453C
	private void OnAteFromStorage(object data)
	{
		if (string.IsNullOrEmpty(this.effectId))
		{
			return;
		}
		(data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
	}

	// Token: 0x060065ED RID: 26093 RVA: 0x00266364 File Offset: 0x00264564
	public bool StoragesAreEmpty()
	{
		foreach (Storage storage in this.storages)
		{
			if (!(storage == null) && storage.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060065EE RID: 26094 RVA: 0x0026639F File Offset: 0x0026459F
	public Vector2I GetTargetFeederCell()
	{
		return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(this), this.feederOffset));
	}

	// Token: 0x0400457A RID: 17786
	public Storage[] storages;

	// Token: 0x0400457B RID: 17787
	public string effectId;

	// Token: 0x0400457C RID: 17788
	public CellOffset feederOffset = CellOffset.none;

	// Token: 0x0400457D RID: 17789
	private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data)
	{
		component.OnAteFromStorage(data);
	});
}
