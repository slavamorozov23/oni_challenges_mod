using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x02000BFB RID: 3067
[AddComponentMenu("KMonoBehaviour/scripts/Trappable")]
public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005C1E RID: 23582 RVA: 0x00215798 File Offset: 0x00213998
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x06005C1F RID: 23583 RVA: 0x002157AC File Offset: 0x002139AC
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06005C20 RID: 23584 RVA: 0x002157BC File Offset: 0x002139BC
	private void OnCellChange()
	{
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.trapsLayer, this);
	}

	// Token: 0x06005C21 RID: 23585 RVA: 0x002157E6 File Offset: 0x002139E6
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x002157F4 File Offset: 0x002139F4
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x00215804 File Offset: 0x00213A04
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		base.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, Trappable.OnCellChangedDispatcher, this, "Trappable.Register");
		this.registered = true;
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x00215853 File Offset: 0x00213A53
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		base.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate, false);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
		this.registered = false;
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x00215886 File Offset: 0x00213A86
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_LAND_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x002158B0 File Offset: 0x00213AB0
	public void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage && (storage.GetComponent<Trap>() != null || storage.GetSMI<ReusableTrap.Instance>() != null))
		{
			base.gameObject.AddTag(GameTags.Trapped);
			Navigator component = base.gameObject.GetComponent<Navigator>();
			if (component != null)
			{
				component.Stop(false, true);
			}
			Brain component2 = base.gameObject.GetComponent<Brain>();
			if (component2 != null)
			{
				Game.BrainScheduler.PrioritizeBrain(component2);
				return;
			}
		}
		else
		{
			base.gameObject.RemoveTag(GameTags.Trapped);
		}
	}

	// Token: 0x04003D6C RID: 15724
	private bool registered;

	// Token: 0x04003D6D RID: 15725
	private ulong cellChangedHandlerID;

	// Token: 0x04003D6E RID: 15726
	private static readonly Action<object> OnCellChangedDispatcher = delegate(object obj)
	{
		Unsafe.As<Trappable>(obj).OnCellChange();
	};

	// Token: 0x04003D6F RID: 15727
	private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>(delegate(Trappable component, object data)
	{
		component.OnStore(data);
	});
}
