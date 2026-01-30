using System;
using UnityEngine;

// Token: 0x02000BFA RID: 3066
public class TrapTrigger : KMonoBehaviour
{
	// Token: 0x06005C18 RID: 23576 RVA: 0x002154D8 File Offset: 0x002136D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameObject gameObject = base.gameObject;
		this.SetTriggerCell(Grid.PosToCell(gameObject));
		foreach (GameObject gameObject2 in this.storage.items)
		{
			this.SetStoredPosition(gameObject2);
			KBoxCollider2D component = gameObject2.GetComponent<KBoxCollider2D>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	// Token: 0x06005C19 RID: 23577 RVA: 0x00215560 File Offset: 0x00213760
	public void SetTriggerCell(int cell)
	{
		HandleVector<int>.Handle handle = this.partitionerEntry;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Trap", base.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
	}

	// Token: 0x06005C1A RID: 23578 RVA: 0x002155B8 File Offset: 0x002137B8
	public void SetStoredPosition(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		Vector3 vector = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
		if (this.addTrappedAnimationOffset)
		{
			vector.x += this.trappedOffset.x - component.Offset.x;
			vector.y += this.trappedOffset.y - component.Offset.y;
		}
		else
		{
			vector.x += this.trappedOffset.x;
			vector.y += this.trappedOffset.y;
		}
		go.transform.SetPosition(vector);
		go.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell(vector));
		component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
	}

	// Token: 0x06005C1B RID: 23579 RVA: 0x00215690 File Offset: 0x00213890
	public void OnCreatureOnTrap(object data)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.storage.IsEmpty())
		{
			return;
		}
		Trappable trappable = (Trappable)data;
		if (trappable.HasTag(GameTags.Stored))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Trapped))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Creatures.Bagged))
		{
			return;
		}
		bool flag = false;
		foreach (Tag tag in this.trappableCreatures)
		{
			if (trappable.HasTag(tag))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		if (this.customConditionsToTrap != null && !this.customConditionsToTrap(trappable.gameObject))
		{
			return;
		}
		this.storage.Store(trappable.gameObject, true, false, true, false);
		this.SetStoredPosition(trappable.gameObject);
		base.Trigger(-358342870, trappable.gameObject);
	}

	// Token: 0x06005C1C RID: 23580 RVA: 0x00215766 File Offset: 0x00213966
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04003D66 RID: 15718
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003D67 RID: 15719
	public Func<GameObject, bool> customConditionsToTrap;

	// Token: 0x04003D68 RID: 15720
	public Tag[] trappableCreatures;

	// Token: 0x04003D69 RID: 15721
	public Vector2 trappedOffset = Vector2.zero;

	// Token: 0x04003D6A RID: 15722
	public bool addTrappedAnimationOffset = true;

	// Token: 0x04003D6B RID: 15723
	[MyCmpReq]
	private Storage storage;
}
