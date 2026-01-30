using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x020009EF RID: 2543
public class LogicCircuitNetwork : UtilityNetwork
{
	// Token: 0x06004A20 RID: 18976 RVA: 0x001AD810 File Offset: 0x001ABA10
	public override void AddItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			LogicWire.BitDepth maxBitDepth = logicWire.MaxBitDepth;
			List<LogicWire> list = this.wireGroups[(int)maxBitDepth];
			if (list == null)
			{
				list = new List<LogicWire>();
				this.wireGroups[(int)maxBitDepth] = list;
			}
			list.Add(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = (ILogicEventReceiver)item;
			this.receivers.Add(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Add(item3);
		}
	}

	// Token: 0x06004A21 RID: 18977 RVA: 0x001AD890 File Offset: 0x001ABA90
	public override void RemoveItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			this.wireGroups[(int)logicWire.MaxBitDepth].Remove(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = item as ILogicEventReceiver;
			this.receivers.Remove(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Remove(item3);
		}
	}

	// Token: 0x06004A22 RID: 18978 RVA: 0x001AD8FA File Offset: 0x001ABAFA
	public override void ConnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			((ILogicEventReceiver)item).OnLogicNetworkConnectionChanged(true);
			return;
		}
		if (item is ILogicEventSender)
		{
			((ILogicEventSender)item).OnLogicNetworkConnectionChanged(true);
		}
	}

	// Token: 0x06004A23 RID: 18979 RVA: 0x001AD925 File Offset: 0x001ABB25
	public override void DisconnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver logicEventReceiver = item as ILogicEventReceiver;
			logicEventReceiver.ReceiveLogicEvent(0);
			logicEventReceiver.OnLogicNetworkConnectionChanged(false);
			return;
		}
		if (item is ILogicEventSender)
		{
			(item as ILogicEventSender).OnLogicNetworkConnectionChanged(false);
		}
	}

	// Token: 0x06004A24 RID: 18980 RVA: 0x001AD958 File Offset: 0x001ABB58
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		this.resetting = true;
		this.previousValue = -1;
		this.outputValue = 0;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list = this.wireGroups[i];
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					LogicWire logicWire = list[j];
					if (logicWire != null)
					{
						int num = Grid.PosToCell(logicWire.transform.GetPosition());
						UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
						utilityNetworkGridNode.networkIdx = -1;
						grid[num] = utilityNetworkGridNode;
					}
				}
				list.Clear();
			}
		}
		this.senders.Clear();
		this.receivers.Clear();
		this.resetting = false;
		this.RemoveOverloadedNotification();
	}

	// Token: 0x06004A25 RID: 18981 RVA: 0x001ADA0C File Offset: 0x001ABC0C
	public void UpdateLogicValue()
	{
		if (this.resetting)
		{
			return;
		}
		this.previousValue = this.outputValue;
		this.outputValue = 0;
		foreach (ILogicEventSender logicEventSender in this.senders)
		{
			logicEventSender.LogicTick();
		}
		foreach (ILogicEventSender logicEventSender2 in this.senders)
		{
			int logicValue = logicEventSender2.GetLogicValue();
			this.outputValue |= logicValue;
		}
	}

	// Token: 0x06004A26 RID: 18982 RVA: 0x001ADAC8 File Offset: 0x001ABCC8
	public int GetBitsUsed()
	{
		int result;
		if (this.outputValue > 1)
		{
			result = 4;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	// Token: 0x06004A27 RID: 18983 RVA: 0x001ADAE7 File Offset: 0x001ABCE7
	public bool IsBitActive(int bit)
	{
		return (this.OutputValue & 1 << bit) > 0;
	}

	// Token: 0x06004A28 RID: 18984 RVA: 0x001ADAF9 File Offset: 0x001ABCF9
	public static bool IsBitActive(int bit, int value)
	{
		return (value & 1 << bit) > 0;
	}

	// Token: 0x06004A29 RID: 18985 RVA: 0x001ADB06 File Offset: 0x001ABD06
	public static int GetBitValue(int bit, int value)
	{
		return value & 1 << bit;
	}

	// Token: 0x06004A2A RID: 18986 RVA: 0x001ADB10 File Offset: 0x001ABD10
	public void SendLogicEvents(bool force_send, int id)
	{
		if (this.resetting)
		{
			return;
		}
		if (this.outputValue != this.previousValue || force_send)
		{
			foreach (ILogicEventReceiver logicEventReceiver in this.receivers)
			{
				logicEventReceiver.ReceiveLogicEvent(this.outputValue);
			}
			if (!force_send)
			{
				this.TriggerAudio((this.previousValue >= 0) ? this.previousValue : 0, id);
			}
		}
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x001ADBA0 File Offset: 0x001ABDA0
	private void TriggerAudio(int old_value, int id)
	{
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (old_value != this.outputValue && instance != null && !instance.IsPaused)
		{
			int num = 0;
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			List<LogicWire> list = new List<LogicWire>();
			for (int i = 0; i < 2; i++)
			{
				List<LogicWire> list2 = this.wireGroups[i];
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						num++;
						if (visibleArea.Min <= list2[j].transform.GetPosition() && list2[j].transform.GetPosition() <= visibleArea.Max)
						{
							list.Add(list2[j]);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				int index = Mathf.CeilToInt((float)(list.Count / 2));
				if (list[index] != null)
				{
					Vector3 position = list[index].transform.GetPosition();
					position.z = 0f;
					string name = "Logic_Circuit_Toggle";
					LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
					if (!LogicCircuitNetwork.logicSoundRegister.ContainsKey(id))
					{
						LogicCircuitNetwork.logicSoundRegister.Add(id, logicSoundPair);
					}
					else
					{
						logicSoundPair.playedIndex = LogicCircuitNetwork.logicSoundRegister[id].playedIndex;
						logicSoundPair.lastPlayed = LogicCircuitNetwork.logicSoundRegister[id].lastPlayed;
					}
					if (logicSoundPair.playedIndex < 2)
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
					}
					else
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = 0;
						LogicCircuitNetwork.logicSoundRegister[id].lastPlayed = Time.time;
					}
					float value = (Time.time - logicSoundPair.lastPlayed) / 3f;
					EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), position, 1f);
					instance2.setParameterByName("logic_volumeModifer", value, false);
					instance2.setParameterByName("wireCount", (float)(num % 24), false);
					instance2.setParameterByName("enabled", (float)this.outputValue, false);
					KFMOD.EndOneShot(instance2);
				}
			}
		}
	}

	// Token: 0x06004A2C RID: 18988 RVA: 0x001ADDD8 File Offset: 0x001ABFD8
	public void UpdateOverloadTime(float dt, int bits_used)
	{
		bool flag = false;
		List<LogicWire> list = null;
		List<LogicUtilityNetworkLink> list2 = null;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list3 = this.wireGroups[i];
			List<LogicUtilityNetworkLink> list4 = this.relevantBridges[i];
			float num = (float)LogicWire.GetBitDepthAsInt((LogicWire.BitDepth)i);
			if ((float)bits_used > num && ((list4 != null && list4.Count > 0) || (list3 != null && list3.Count > 0)))
			{
				flag = true;
				list = list3;
				list2 = list4;
				break;
			}
		}
		if (list != null)
		{
			list.RemoveAll((LogicWire x) => x == null);
		}
		if (list2 != null)
		{
			list2.RemoveAll((LogicUtilityNetworkLink x) => x == null);
		}
		if (flag)
		{
			this.timeOverloaded += dt;
			if (this.timeOverloaded > 6f)
			{
				this.timeOverloaded = 0f;
				if (this.targetOverloadedWire == null)
				{
					if (list2 != null && list2.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, list2.Count);
						this.targetOverloadedWire = list2[index].gameObject;
					}
					else if (list != null && list.Count > 0)
					{
						int index2 = UnityEngine.Random.Range(0, list.Count);
						this.targetOverloadedWire = list[index2].gameObject;
					}
				}
				if (this.targetOverloadedWire != null)
				{
					this.targetOverloadedWire.BoxingTrigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = BUILDINGS.DAMAGESOURCES.LOGIC_CIRCUIT_OVERLOADED,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LOGIC_CIRCUIT_OVERLOADED,
						takeDamageEffect = SpawnFXHashes.BuildingLogicOverload,
						fullDamageEffectName = "logic_ribbon_damage_kanim",
						statusItemID = Db.Get().BuildingStatusItems.LogicOverloaded.Id
					});
				}
				if (this.overloadedNotification == null)
				{
					this.timeOverloadNotificationDisplayed = 0f;
					this.overloadedNotification = new Notification(MISC.NOTIFICATIONS.LOGIC_CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, null, null, true, 0f, null, null, this.targetOverloadedWire.transform, true, false, false);
					Game.Instance.FindOrAdd<Notifier>().Add(this.overloadedNotification, "");
					return;
				}
			}
		}
		else
		{
			this.timeOverloaded = Mathf.Max(0f, this.timeOverloaded - dt * 0.95f);
			this.timeOverloadNotificationDisplayed += dt;
			if (this.timeOverloadNotificationDisplayed > 5f)
			{
				this.RemoveOverloadedNotification();
			}
		}
	}

	// Token: 0x06004A2D RID: 18989 RVA: 0x001AE04E File Offset: 0x001AC24E
	private void RemoveOverloadedNotification()
	{
		if (this.overloadedNotification != null)
		{
			Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
			this.overloadedNotification = null;
		}
	}

	// Token: 0x06004A2E RID: 18990 RVA: 0x001AE074 File Offset: 0x001AC274
	public void UpdateRelevantBridges(List<LogicUtilityNetworkLink>[] bridgeGroups)
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		for (int i = 0; i < bridgeGroups.Length; i++)
		{
			if (this.relevantBridges[i] != null)
			{
				this.relevantBridges[i].Clear();
			}
			for (int j = 0; j < bridgeGroups[i].Count; j++)
			{
				if (logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_one) == this || logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_two) == this)
				{
					if (this.relevantBridges[i] == null)
					{
						this.relevantBridges[i] = new List<LogicUtilityNetworkLink>();
					}
					this.relevantBridges[i].Add(bridgeGroups[i][j]);
				}
			}
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06004A2F RID: 18991 RVA: 0x001AE125 File Offset: 0x001AC325
	public int OutputValue
	{
		get
		{
			return this.outputValue;
		}
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06004A30 RID: 18992 RVA: 0x001AE130 File Offset: 0x001AC330
	public int WireCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (this.wireGroups[i] != null)
				{
					num += this.wireGroups[i].Count;
				}
			}
			return num;
		}
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06004A31 RID: 18993 RVA: 0x001AE166 File Offset: 0x001AC366
	public List<ILogicEventSender> Senders
	{
		get
		{
			return this.senders;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06004A32 RID: 18994 RVA: 0x001AE16E File Offset: 0x001AC36E
	public List<ILogicEventReceiver> Receivers
	{
		get
		{
			return this.receivers;
		}
	}

	// Token: 0x04003135 RID: 12597
	private List<LogicWire>[] wireGroups = new List<LogicWire>[2];

	// Token: 0x04003136 RID: 12598
	private List<LogicUtilityNetworkLink>[] relevantBridges = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04003137 RID: 12599
	private List<ILogicEventReceiver> receivers = new List<ILogicEventReceiver>();

	// Token: 0x04003138 RID: 12600
	private List<ILogicEventSender> senders = new List<ILogicEventSender>();

	// Token: 0x04003139 RID: 12601
	private int previousValue = -1;

	// Token: 0x0400313A RID: 12602
	private int outputValue;

	// Token: 0x0400313B RID: 12603
	private bool resetting;

	// Token: 0x0400313C RID: 12604
	public static float logicSoundLastPlayedTime = 0f;

	// Token: 0x0400313D RID: 12605
	private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;

	// Token: 0x0400313E RID: 12606
	private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;

	// Token: 0x0400313F RID: 12607
	public const int VALID_LOGIC_SIGNAL_MASK = 15;

	// Token: 0x04003140 RID: 12608
	public const int UNINITIALIZED_LOGIC_STATE = -16;

	// Token: 0x04003141 RID: 12609
	private GameObject targetOverloadedWire;

	// Token: 0x04003142 RID: 12610
	private float timeOverloaded;

	// Token: 0x04003143 RID: 12611
	private float timeOverloadNotificationDisplayed;

	// Token: 0x04003144 RID: 12612
	private Notification overloadedNotification;

	// Token: 0x04003145 RID: 12613
	public static Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = new Dictionary<int, LogicCircuitNetwork.LogicSoundPair>();

	// Token: 0x02001A50 RID: 6736
	public class LogicSoundPair
	{
		// Token: 0x04008155 RID: 33109
		public int playedIndex;

		// Token: 0x04008156 RID: 33110
		public float lastPlayed;
	}
}
