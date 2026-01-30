using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200070F RID: 1807
[AddComponentMenu("KMonoBehaviour/Workable/Bed")]
public class Bed : Workable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06002D00 RID: 11520 RVA: 0x001050FE File Offset: 0x001032FE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x00105110 File Offset: 0x00103310
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.BasicBuildings.Add(this);
		this.sleepable = base.GetComponent<Sleepable>();
		Sleepable sleepable = this.sleepable;
		sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x00105161 File Offset: 0x00103361
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddEffects();
			return;
		}
		if (workable_event == Workable.WorkableEvent.WorkStopped)
		{
			this.RemoveEffects();
		}
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x00105178 File Offset: 0x00103378
	private void AddEffects()
	{
		this.targetWorker = this.sleepable.worker;
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Add(effect_id, false);
			}
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
		RoomType roomType = roomOfGameObject.roomType;
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			if (keyValuePair.Key == roomType.Id)
			{
				this.targetWorker.GetComponent<Effects>().Add(keyValuePair.Value, false);
			}
		}
		roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), this.targetWorker.GetComponent<Effects>());
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x00105274 File Offset: 0x00103474
	private void RemoveEffects()
	{
		if (this.targetWorker == null)
		{
			return;
		}
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Remove(effect_id);
			}
		}
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			this.targetWorker.GetComponent<Effects>().Remove(keyValuePair.Value);
		}
		this.targetWorker = null;
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x00105320 File Offset: 0x00103520
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.effects != null)
		{
			foreach (string text in this.effects)
			{
				if (text != null && text != "")
				{
					Effect.AddModifierDescriptions(base.gameObject, list, text, false);
				}
			}
		}
		return list;
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x00105374 File Offset: 0x00103574
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		if (this.sleepable != null)
		{
			Sleepable sleepable = this.sleepable;
			sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		}
	}

	// Token: 0x04001ACA RID: 6858
	[MyCmpReq]
	private Sleepable sleepable;

	// Token: 0x04001ACB RID: 6859
	private WorkerBase targetWorker;

	// Token: 0x04001ACC RID: 6860
	public string[] effects;

	// Token: 0x04001ACD RID: 6861
	public static readonly Dictionary<string, string> roomSleepingEffects = new Dictionary<string, string>
	{
		{
			"Barracks",
			"BarracksStamina"
		},
		{
			"Luxury Barracks",
			"BarracksStamina"
		},
		{
			"Private Bedroom",
			"BedroomStamina"
		}
	};
}
