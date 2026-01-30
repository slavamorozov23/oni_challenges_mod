using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200087A RID: 2170
[AddComponentMenu("KMonoBehaviour/scripts/ConversationManager")]
public class ConversationManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x06003BBE RID: 15294 RVA: 0x0014E295 File Offset: 0x0014C495
	protected override void OnPrefabInit()
	{
		this.conversations = new List<Conversation>();
		this.lastConvoTimeByMinion = new Dictionary<MinionIdentity, float>();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x0014E2B4 File Offset: 0x0014C4B4
	public void Sim200ms(float dt)
	{
		for (int i = this.conversations.Count - 1; i >= 0; i--)
		{
			Conversation conversation = this.conversations[i];
			for (int j = conversation.minions.Count - 1; j >= 0; j--)
			{
				MinionIdentity minionIdentity = conversation.minions[j];
				if (!this.ValidMinionTags(minionIdentity) || !this.MinionCloseEnoughToConvo(minionIdentity, conversation))
				{
					conversation.minions.RemoveAt(j);
					if (conversation.lastTalked == minionIdentity)
					{
						conversation.lastTalked = null;
					}
				}
				else
				{
					this.minionConversations[minionIdentity] = conversation;
				}
			}
			if (conversation.minions.Count <= 1)
			{
				this.conversations.RemoveAt(i);
			}
			else if (!(conversation.lastTalked != null) || !conversation.lastTalked.GetComponent<KPrefabID>().HasTag(GameTags.DoNotInterruptMe))
			{
				bool flag = conversation.minions.Find((MinionIdentity match) => match.HasTag(GameTags.CommunalDining)) != null;
				bool flag2 = true;
				if (!flag && conversation.numUtterances >= TuningData<ConversationManager.Tuning>.Get().maxUtterances)
				{
					flag2 = false;
				}
				else
				{
					bool flag3 = conversation.numUtterances == 0;
					bool flag4 = conversation.minions.Find((MinionIdentity match) => !match.HasTag(GameTags.Partying)) == null;
					float num = flag3 ? TuningData<ConversationManager.Tuning>.Get().delayBeforeStart : TuningData<ConversationManager.Tuning>.Get().delayBetweenUtterances;
					if (flag4)
					{
						num = 0f;
					}
					float num2 = flag3 ? 0f : TuningData<ConversationManager.Tuning>.Get().speakTime;
					if (flag4)
					{
						num2 /= 4f;
					}
					num2 += num;
					if (GameClock.Instance.GetTime() > conversation.lastTalkedTime + num2)
					{
						flag2 = this.TryContinueConversation(conversation, flag3);
					}
				}
				if (!flag2)
				{
					this.conversations.RemoveAt(i);
				}
			}
		}
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			if (this.ValidMinionTags(minionIdentity2) && !this.minionConversations.ContainsKey(minionIdentity2) && !this.MinionOnCooldown(minionIdentity2))
			{
				foreach (MinionIdentity minionIdentity3 in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity3 == minionIdentity2) && this.ValidMinionTags(minionIdentity3))
					{
						Conversation conversation2;
						if (this.minionConversations.TryGetValue(minionIdentity3, out conversation2))
						{
							if (conversation2.minions.Count < TuningData<ConversationManager.Tuning>.Get().maxDupesPerConvo && (this.GetCentroid(conversation2) - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f)
							{
								conversation2.minions.Add(minionIdentity2);
								this.minionConversations[minionIdentity2] = conversation2;
								break;
							}
						}
						else if (!this.MinionOnCooldown(minionIdentity3) && (minionIdentity3.transform.GetPosition() - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance)
						{
							conversation2 = new Conversation();
							conversation2.minions.Add(minionIdentity2);
							conversation2.minions.Add(minionIdentity3);
							Type type = this.convoTypes[UnityEngine.Random.Range(0, this.convoTypes.Count)];
							conversation2.conversationType = (ConversationType)Activator.CreateInstance(type);
							conversation2.lastTalkedTime = GameClock.Instance.GetTime();
							this.conversations.Add(conversation2);
							this.minionConversations[minionIdentity2] = conversation2;
							this.minionConversations[minionIdentity3] = conversation2;
							break;
						}
					}
				}
			}
		}
		this.minionConversations.Clear();
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x0014E6FC File Offset: 0x0014C8FC
	private bool TryContinueConversation(Conversation conversation, bool isOpeningLine)
	{
		ListPool<int, ConversationManager>.PooledList pooledList = ListPool<int, ConversationManager>.Allocate();
		int num = -1;
		pooledList.Capacity = Math.Max(pooledList.Capacity, conversation.minions.Count);
		for (int num2 = 0; num2 != conversation.minions.Count; num2++)
		{
			if (conversation.minions[num2] == conversation.lastTalked)
			{
				num = num2;
			}
			else
			{
				pooledList.Add(num2);
			}
		}
		pooledList.Shuffle<int>();
		if (num != -1)
		{
			pooledList.Add(num);
		}
		if (isOpeningLine)
		{
			MinionIdentity speaker = conversation.minions[pooledList[0]];
			conversation.conversationType.NewTarget(speaker);
		}
		bool result = false;
		foreach (int index in pooledList)
		{
			MinionIdentity new_speaker = conversation.minions[index];
			if (this.DoTalking(conversation, new_speaker))
			{
				result = true;
				break;
			}
		}
		pooledList.Recycle();
		return result;
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0014E800 File Offset: 0x0014CA00
	private bool DoTalking(Conversation conversation, MinionIdentity new_speaker)
	{
		DebugUtil.Assert(conversation != null, "conversation was null");
		DebugUtil.Assert(new_speaker != null, "new_speaker was null");
		DebugUtil.Assert(conversation.conversationType != null, "conversation.conversationType was null");
		Conversation.Topic nextTopic = conversation.conversationType.GetNextTopic(new_speaker, conversation.lastTopic);
		if (nextTopic == null || nextTopic.mode == Conversation.ModeType.End)
		{
			return false;
		}
		Thought thoughtForTopic = this.GetThoughtForTopic(conversation, nextTopic);
		if (thoughtForTopic == null)
		{
			return false;
		}
		ThoughtGraph.Instance smi = new_speaker.GetSMI<ThoughtGraph.Instance>();
		if (smi == null)
		{
			return false;
		}
		if (conversation.lastTalked != null)
		{
			conversation.lastTalked.Trigger(25860745, conversation.lastTalked.gameObject);
		}
		smi.AddThought(thoughtForTopic);
		conversation.lastTopic = nextTopic;
		conversation.lastTalked = new_speaker;
		conversation.lastTalkedTime = GameClock.Instance.GetTime();
		DebugUtil.Assert(this.lastConvoTimeByMinion != null, "lastConvoTimeByMinion was null");
		this.lastConvoTimeByMinion[conversation.lastTalked] = GameClock.Instance.GetTime();
		Effects component = conversation.lastTalked.GetComponent<Effects>();
		DebugUtil.Assert(component != null, "effects was null");
		component.Add("GoodConversation", true);
		Conversation.Mode mode = Conversation.Topic.Modes[(int)nextTopic.mode];
		DebugUtil.Assert(mode != null, "mode was null");
		ConversationManager.StartedTalkingEvent data = new ConversationManager.StartedTalkingEvent
		{
			talker = new_speaker.gameObject,
			anim = mode.anim
		};
		foreach (MinionIdentity minionIdentity in conversation.minions)
		{
			if (!minionIdentity)
			{
				DebugUtil.DevAssert(false, "minion in conversation.minions was null", null);
			}
			else
			{
				minionIdentity.Trigger(-594200555, data);
			}
		}
		conversation.numUtterances++;
		return true;
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0014E9D4 File Offset: 0x0014CBD4
	public bool TryGetConversation(MinionIdentity minion, out Conversation conversation)
	{
		return this.minionConversations.TryGetValue(minion, out conversation);
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0014E9E4 File Offset: 0x0014CBE4
	private Vector3 GetCentroid(Conversation conversation)
	{
		Vector3 a = Vector3.zero;
		foreach (MinionIdentity minionIdentity in conversation.minions)
		{
			if (!(minionIdentity == null))
			{
				a += minionIdentity.transform.GetPosition();
			}
		}
		return a / (float)conversation.minions.Count;
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0014EA64 File Offset: 0x0014CC64
	private Thought GetThoughtForTopic(Conversation conversation, Conversation.Topic topic)
	{
		if (string.IsNullOrEmpty(topic.topic))
		{
			DebugUtil.DevAssert(false, "topic.topic was null", null);
			return null;
		}
		Sprite sprite = conversation.conversationType.GetSprite(topic.topic);
		if (sprite != null)
		{
			Conversation.Mode mode = Conversation.Topic.Modes[(int)topic.mode];
			return new Thought("Topic_" + topic.topic, null, sprite, mode.icon, mode.voice, "bubble_chatter", mode.mouth, DUPLICANTS.THOUGHTS.CONVERSATION.TOOLTIP, true, TuningData<ConversationManager.Tuning>.Get().speakTime);
		}
		return null;
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x0014EAF8 File Offset: 0x0014CCF8
	private bool ValidMinionTags(MinionIdentity minion)
	{
		return !(minion == null) && !minion.GetComponent<KPrefabID>().HasAnyTags(ConversationManager.invalidConvoTags);
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x0014EB18 File Offset: 0x0014CD18
	private bool MinionCloseEnoughToConvo(MinionIdentity minion, Conversation conversation)
	{
		return (this.GetCentroid(conversation) - minion.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f;
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x0014EB58 File Offset: 0x0014CD58
	private bool MinionOnCooldown(MinionIdentity minion)
	{
		if (minion.GetComponent<KPrefabID>().HasTag(GameTags.AlwaysConverse))
		{
			return false;
		}
		float num;
		if (!this.lastConvoTimeByMinion.TryGetValue(minion, out num))
		{
			return false;
		}
		float num2 = GameClock.Instance.GetTime() - TuningData<ConversationManager.Tuning>.Get().minionCooldownTime;
		return num > num2;
	}

	// Token: 0x040024EE RID: 9454
	private List<Conversation> conversations;

	// Token: 0x040024EF RID: 9455
	private Dictionary<MinionIdentity, float> lastConvoTimeByMinion;

	// Token: 0x040024F0 RID: 9456
	private readonly Dictionary<MinionIdentity, Conversation> minionConversations = new Dictionary<MinionIdentity, Conversation>();

	// Token: 0x040024F1 RID: 9457
	private List<Type> convoTypes = new List<Type>
	{
		typeof(RecentThingConversation),
		typeof(AmountStateConversation),
		typeof(CurrentJobConversation)
	};

	// Token: 0x040024F2 RID: 9458
	private static readonly Tag[] invalidConvoTags = new Tag[]
	{
		GameTags.Asleep,
		GameTags.BionicBedTime,
		GameTags.HoldingBreath,
		GameTags.Dead,
		GameTags.SuppressConversation
	};

	// Token: 0x0200183D RID: 6205
	public class Tuning : TuningData<ConversationManager.Tuning>
	{
		// Token: 0x04007A59 RID: 31321
		public float cyclesBeforeFirstConversation;

		// Token: 0x04007A5A RID: 31322
		public float maxDistance;

		// Token: 0x04007A5B RID: 31323
		public int maxDupesPerConvo;

		// Token: 0x04007A5C RID: 31324
		public float minionCooldownTime;

		// Token: 0x04007A5D RID: 31325
		public float speakTime;

		// Token: 0x04007A5E RID: 31326
		public float delayBetweenUtterances;

		// Token: 0x04007A5F RID: 31327
		public float delayBeforeStart;

		// Token: 0x04007A60 RID: 31328
		public int maxUtterances;
	}

	// Token: 0x0200183E RID: 6206
	public class StartedTalkingEvent
	{
		// Token: 0x04007A61 RID: 31329
		public GameObject talker;

		// Token: 0x04007A62 RID: 31330
		public string anim;
	}
}
