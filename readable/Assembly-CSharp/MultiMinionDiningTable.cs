using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A5F RID: 2655
[SerializationConfig(MemberSerialization.OptIn)]
public class MultiMinionDiningTable : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06004D24 RID: 19748 RVA: 0x001C0D6D File Offset: 0x001BEF6D
	public static int SeatCount
	{
		get
		{
			return MultiMinionDiningTableConfig.SeatCount;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06004D25 RID: 19749 RVA: 0x001C0D74 File Offset: 0x001BEF74
	public bool HasSalt
	{
		get
		{
			return this.storage != null && this.storage.GetMassAvailable(TableSaltConfig.TAG) >= TableSaltTuning.CONSUMABLE_RATE;
		}
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x001C0DA0 File Offset: 0x001BEFA0
	private static GameObject SpawnSeat(MultiMinionDiningTable diningTable, int diningTableCell, int seatIndex)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ApproachableLocator.ID), diningTable.transform.gameObject, "MultiMinionDiningSeat");
		Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(diningTableCell, MultiMinionDiningTableConfig.seats[seatIndex].TableRelativeLocation), Grid.SceneLayer.Move);
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		gameObject.AddOrGet<MultiMinionDiningTable.Seat>().Initialize(seatIndex);
		gameObject.AddOrGet<Reservable>();
		gameObject.GetComponent<KPrefabID>().CopyTags(diningTable.GetComponent<KPrefabID>());
		return gameObject;
	}

	// Token: 0x06004D27 RID: 19751 RVA: 0x001C0E28 File Offset: 0x001BF028
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int diningTableCell = Grid.PosToCell(this);
		for (int i = 0; i < MultiMinionDiningTable.SeatCount; i++)
		{
			MultiMinionDiningTable.SpawnSeat(this, diningTableCell, i);
		}
		this.animController.Play(MultiMinionDiningTable.ANIM, KAnim.PlayMode.Once, 1f, 0f);
		this.UpdateSaltVisibility();
		this.storage.Subscribe(-1697596308, delegate(object _)
		{
			this.UpdateSaltVisibility();
		});
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x001C0E9C File Offset: 0x001BF09C
	public void UpdateSaltVisibility()
	{
		if (this.HasSalt)
		{
			foreach (MultiMinionDiningTable.Seat seat in base.gameObject.GetComponentsInChildren<MultiMinionDiningTable.Seat>())
			{
				bool is_visible = !seat.HasDiner;
				this.animController.SetSymbolVisiblity(seat.SaltSymbol, is_visible);
			}
			return;
		}
		foreach (MultiMinionDiningTable.Seat seat2 in base.gameObject.GetComponentsInChildren<MultiMinionDiningTable.Seat>())
		{
			this.animController.SetSymbolVisiblity(seat2.SaltSymbol, false);
		}
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x001C0F28 File Offset: 0x001BF128
	private void RegisterCommunalDiner(KPrefabID diner)
	{
		Effects effects;
		if (diner.TryGetComponent<Effects>(out effects))
		{
			effects.Add(MultiMinionDiningTable.COMMUNAL_DINING_EFFECT, true);
		}
		else
		{
			global::Debug.LogWarning("Diner has no Effects component");
		}
		this.communalDiners[diner.gameObject] = new MultiMinionDiningTable.Diner(this, diner);
	}

	// Token: 0x06004D2A RID: 19754 RVA: 0x001C0F70 File Offset: 0x001BF170
	private void UnregisterCommunalDiner(KPrefabID dinerKpid)
	{
		MultiMinionDiningTable.Diner diner;
		if (this.communalDiners.TryGetValue(dinerKpid.gameObject, out diner))
		{
			diner.CleanUp();
			this.communalDiners.Remove(dinerKpid.gameObject);
		}
	}

	// Token: 0x06004D2B RID: 19755 RVA: 0x001C0FAC File Offset: 0x001BF1AC
	private void OnDinerStartTalking(KPrefabID diner, object untypedStartTalkingEvent)
	{
		ConversationManager.StartedTalkingEvent startedTalkingEvent = untypedStartTalkingEvent as ConversationManager.StartedTalkingEvent;
		if (startedTalkingEvent == null)
		{
			return;
		}
		KPrefabID x;
		if (!startedTalkingEvent.talker.TryGetComponent<KPrefabID>(out x))
		{
			return;
		}
		if (x != diner)
		{
			return;
		}
		diner.AddTag(GameTags.WantsToTalk, false);
		diner.AddTag(GameTags.DoNotInterruptMe, false);
	}

	// Token: 0x06004D2C RID: 19756 RVA: 0x001C0FF8 File Offset: 0x001BF1F8
	private void OnDinerStopTalking(KPrefabID diner, object untypedStoppedTalker)
	{
		GameObject gameObject = untypedStoppedTalker as GameObject;
		if (gameObject == null)
		{
			return;
		}
		KPrefabID x;
		if (!gameObject.TryGetComponent<KPrefabID>(out x))
		{
			return;
		}
		if (x != diner)
		{
			return;
		}
		diner.RemoveTag(GameTags.WantsToTalk);
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x001C1030 File Offset: 0x001BF230
	private void OnDinerChanged(KPrefabID prevDiner, KPrefabID newDiner, int seatIndex)
	{
		MultiMinionDiningTable.Seat[] componentsInChildren = base.gameObject.GetComponentsInChildren<MultiMinionDiningTable.Seat>();
		bool is_visible = newDiner == null && this.HasSalt;
		this.animController.SetSymbolVisiblity(componentsInChildren[seatIndex].SaltSymbol, is_visible);
		if (prevDiner != null && this.communalDiners.ContainsKey(prevDiner.gameObject))
		{
			this.UnregisterCommunalDiner(prevDiner);
		}
		if (newDiner != null)
		{
			int num = 0;
			MultiMinionDiningTable.Seat[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].HasDiner)
				{
					num++;
					if (num > 1)
					{
						break;
					}
				}
			}
			if (num > 1)
			{
				foreach (MultiMinionDiningTable.Seat seat in componentsInChildren)
				{
					if (!(seat.Diner == null) && !this.communalDiners.ContainsKey(seat.Diner.gameObject))
					{
						this.RegisterCommunalDiner(seat.Diner);
					}
				}
				return;
			}
		}
		else if (this.communalDiners.Count == 1)
		{
			foreach (MultiMinionDiningTable.Seat seat2 in componentsInChildren)
			{
				if (!(seat2.Diner == null))
				{
					this.UnregisterCommunalDiner(seat2.Diner);
				}
			}
		}
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x001C1168 File Offset: 0x001BF368
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>
		{
			MultiMinionDiningTable.COMMUNAL_DINING_DESCRIPTOR
		};
		if (this.HasSalt)
		{
			list.Add(MessStation.TABLE_SALT_DESCRIPTOR);
		}
		return list;
	}

	// Token: 0x04003380 RID: 13184
	public const string SEAT_ID = "MultiMinionDiningSeat";

	// Token: 0x04003381 RID: 13185
	[MyCmpGet]
	private readonly Storage storage;

	// Token: 0x04003382 RID: 13186
	private static readonly HashedString ANIM = "salt";

	// Token: 0x04003383 RID: 13187
	[MyCmpReq]
	private readonly KAnimControllerBase animController;

	// Token: 0x04003384 RID: 13188
	private readonly Dictionary<GameObject, MultiMinionDiningTable.Diner> communalDiners = new Dictionary<GameObject, MultiMinionDiningTable.Diner>();

	// Token: 0x04003385 RID: 13189
	private static readonly HashedString COMMUNAL_DINING_EFFECT = "CommunalDining";

	// Token: 0x04003386 RID: 13190
	private const int MORALE_MODIFIER = 1;

	// Token: 0x04003387 RID: 13191
	private static readonly Descriptor COMMUNAL_DINING_DESCRIPTOR = new Descriptor(string.Format(UI.BUILDINGEFFECTS.COMMUNAL_DINING, 1), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.COMMUNAL_DINING, 1), Descriptor.DescriptorType.Effect, false);

	// Token: 0x02001B72 RID: 7026
	public class Seat : Assignable, IDiningSeat
	{
		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x0600AA09 RID: 43529 RVA: 0x003C31D1 File Offset: 0x003C13D1
		private MultiMinionDiningTableConfig.Seat SeatConfig
		{
			get
			{
				return MultiMinionDiningTableConfig.seats[this.index];
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x0600AA0A RID: 43530 RVA: 0x003C31E4 File Offset: 0x003C13E4
		public HashedString SaltSymbol
		{
			get
			{
				return this.SeatConfig.SaltSymbol;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x0600AA0B RID: 43531 RVA: 0x003C31FF File Offset: 0x003C13FF
		public GameObject DiningTable
		{
			get
			{
				return base.transform.parent.gameObject;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x0600AA0C RID: 43532 RVA: 0x003C3211 File Offset: 0x003C1411
		public bool HasSalt
		{
			get
			{
				return this.DiningTable.GetComponent<MultiMinionDiningTable>().HasSalt;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600AA0D RID: 43533 RVA: 0x003C3224 File Offset: 0x003C1424
		public HashedString EatAnim
		{
			get
			{
				return this.SeatConfig.EatAnim;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600AA0E RID: 43534 RVA: 0x003C3240 File Offset: 0x003C1440
		public HashedString ReloadElectrobankAnim
		{
			get
			{
				return this.SeatConfig.ReloadElectrobankAnim;
			}
		}

		// Token: 0x0600AA0F RID: 43535 RVA: 0x003C325B File Offset: 0x003C145B
		public Storage FindStorage()
		{
			return this.DiningTable.GetComponent<Storage>();
		}

		// Token: 0x0600AA10 RID: 43536 RVA: 0x003C3268 File Offset: 0x003C1468
		public Operational FindOperational()
		{
			return this.DiningTable.GetComponent<Operational>();
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600AA11 RID: 43537 RVA: 0x003C3275 File Offset: 0x003C1475
		// (set) Token: 0x0600AA12 RID: 43538 RVA: 0x003C3280 File Offset: 0x003C1480
		public KPrefabID Diner
		{
			get
			{
				return this.diner;
			}
			set
			{
				KPrefabID prevDiner = this.diner;
				this.diner = value;
				this.DiningTable.GetComponent<MultiMinionDiningTable>().OnDinerChanged(prevDiner, this.diner, this.index);
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x0600AA13 RID: 43539 RVA: 0x003C32B8 File Offset: 0x003C14B8
		public bool HasDiner
		{
			get
			{
				return this.Diner != null;
			}
		}

		// Token: 0x0600AA14 RID: 43540 RVA: 0x003C32C6 File Offset: 0x003C14C6
		public Seat()
		{
			this.slotID = Db.Get().AssignableSlots.MessStation.Id;
			this.canBePublic = true;
		}

		// Token: 0x0600AA15 RID: 43541 RVA: 0x003C32EF File Offset: 0x003C14EF
		public void Initialize(int index)
		{
			this.index = index;
		}

		// Token: 0x04008500 RID: 34048
		private int index;

		// Token: 0x04008501 RID: 34049
		private KPrefabID diner;
	}

	// Token: 0x02001B73 RID: 7027
	private readonly struct Diner
	{
		// Token: 0x0600AA16 RID: 43542 RVA: 0x003C32F8 File Offset: 0x003C14F8
		public Diner(MultiMinionDiningTable table, KPrefabID diner)
		{
			this.kpid = diner;
			diner.AddTag(GameTags.CommunalDining, false);
			diner.AddTag(GameTags.AlwaysConverse, false);
			this.startTalkingHandler = diner.Subscribe(-594200555, delegate(object eventData)
			{
				table.OnDinerStartTalking(diner, eventData);
			});
			this.stopTalkingHandler = diner.Subscribe(25860745, delegate(object eventData)
			{
				table.OnDinerStopTalking(diner, eventData);
			});
		}

		// Token: 0x0600AA17 RID: 43543 RVA: 0x003C338C File Offset: 0x003C158C
		public void CleanUp()
		{
			this.kpid.RemoveTag(GameTags.CommunalDining);
			this.kpid.RemoveTag(GameTags.AlwaysConverse);
			this.kpid.Unsubscribe(this.startTalkingHandler);
			this.kpid.Unsubscribe(this.stopTalkingHandler);
		}

		// Token: 0x04008502 RID: 34050
		private readonly KPrefabID kpid;

		// Token: 0x04008503 RID: 34051
		private readonly int startTalkingHandler;

		// Token: 0x04008504 RID: 34052
		private readonly int stopTalkingHandler;
	}
}
