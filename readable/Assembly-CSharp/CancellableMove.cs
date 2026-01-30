using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
public class CancellableMove : Cancellable
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060020E0 RID: 8416 RVA: 0x000BE29F File Offset: 0x000BC49F
	public List<Ref<Movable>> movingObjects
	{
		get
		{
			return this.movables;
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000BE2A8 File Offset: 0x000BC4A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!component.IsPrioritizable())
		{
			component.AddRef();
		}
		if (this.fetchChore == null)
		{
			GameObject nextTarget = this.GetNextTarget();
			if (!(nextTarget != null) || nextTarget.IsNullOrDestroyed())
			{
				global::Debug.LogWarning("MovePickupable spawned with no objects to move. Destroying placer.");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
		}
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = base.gameObject;
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000BE380 File Offset: 0x000BC580
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = null;
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000BE3B3 File Offset: 0x000BC5B3
	public void CancelAll()
	{
		this.OnCancel(null);
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000BE3BC File Offset: 0x000BC5BC
	public void OnCancel(Movable cancel_movable = null)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			Ref<Movable> @ref = this.movables[i];
			if (@ref != null)
			{
				Movable movable = @ref.Get();
				if (cancel_movable == null || movable == cancel_movable)
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("CancelMove");
			if (this.fetchChore.driver == null && this.movables.Count <= 0)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000BE460 File Offset: 0x000BC660
	protected override void OnCancel(object data)
	{
		this.OnCancel(null);
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000BE46C File Offset: 0x000BC66C
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.CancelAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true), 1f);
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000BE4C8 File Offset: 0x000BC6C8
	public void SetMovable(Movable movable)
	{
		if (this.fetchChore == null)
		{
			this.fetchChore = new MovePickupableChore(this, movable.gameObject, new Action<Chore>(this.OnChoreEnd));
		}
		if (this.movables.Find((Ref<Movable> move) => move.Get() == movable) == null)
		{
			this.movables.Add(new Ref<Movable>(movable));
		}
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000BE53C File Offset: 0x000BC73C
	public void OnChoreEnd(Chore chore)
	{
		GameObject nextTarget = this.GetNextTarget();
		if (nextTarget == null)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000BE57E File Offset: 0x000BC77E
	public bool IsDeliveryComplete()
	{
		this.ValidateMovables();
		return this.movables.Count <= 0;
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x000BE598 File Offset: 0x000BC798
	public void RemoveMovable(Movable moved)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i].Get() == null || this.movables[i].Get() == moved)
			{
				this.movables.RemoveAt(i);
			}
		}
		if (this.movables.Count <= 0)
		{
			this.OnCancel(null);
		}
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x000BE610 File Offset: 0x000BC810
	public GameObject GetNextTarget()
	{
		this.ValidateMovables();
		if (this.movables.Count > 0)
		{
			return this.movables[0].Get().gameObject;
		}
		return null;
	}

	// Token: 0x060020EC RID: 8428 RVA: 0x000BE640 File Offset: 0x000BC840
	private void ValidateMovables()
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i] == null)
			{
				this.movables.RemoveAt(i);
			}
			else
			{
				Movable movable = this.movables[i].Get();
				if (movable == null)
				{
					this.movables.RemoveAt(i);
				}
				else if (Grid.PosToCell(movable) == Grid.PosToCell(this))
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x0400132C RID: 4908
	[Serialize]
	private List<Ref<Movable>> movables = new List<Ref<Movable>>();

	// Token: 0x0400132D RID: 4909
	private MovePickupableChore fetchChore;
}
