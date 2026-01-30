using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000A17 RID: 2583
public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
	// Token: 0x06004BC2 RID: 19394 RVA: 0x001B8490 File Offset: 0x001B6690
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.TopicDiscussed, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscussed(obj);
		}).EventHandler(GameHashes.TopicDiscovered, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscovered(obj);
		});
	}

	// Token: 0x04003234 RID: 12852
	private const int MAX_RECENT_TOPICS = 5;

	// Token: 0x04003235 RID: 12853
	private const int MAX_FAVOURITE_TOPICS = 5;

	// Token: 0x04003236 RID: 12854
	private const float FAVOURITE_CHANCE = 0.033333335f;

	// Token: 0x04003237 RID: 12855
	private const float LEARN_CHANCE = 0.33333334f;

	// Token: 0x02001AB6 RID: 6838
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AB7 RID: 6839
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
	{
		// Token: 0x0600A6DC RID: 42716 RVA: 0x003BAD30 File Offset: 0x003B8F30
		public Instance(IStateMachineTarget master, ConversationMonitor.Def def) : base(master, def)
		{
			this.recentTopics = new Queue<string>();
			this.favouriteTopics = new List<string>
			{
				ConversationMonitor.Instance.randomTopics[UnityEngine.Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
			};
			this.personalTopics = new List<string>();
		}

		// Token: 0x0600A6DD RID: 42717 RVA: 0x003BAD88 File Offset: 0x003B8F88
		public string GetATopic()
		{
			int maxExclusive = this.recentTopics.Count + this.favouriteTopics.Count * 2 + this.personalTopics.Count;
			int num = UnityEngine.Random.Range(0, maxExclusive);
			if (num < this.recentTopics.Count)
			{
				return this.recentTopics.Dequeue();
			}
			num -= this.recentTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.personalTopics.Count)
			{
				return this.personalTopics[num];
			}
			return "";
		}

		// Token: 0x0600A6DE RID: 42718 RVA: 0x003BAE60 File Offset: 0x003B9060
		public void OnTopicDiscovered(object data)
		{
			string item = (string)data;
			if (!this.recentTopics.Contains(item))
			{
				this.recentTopics.Enqueue(item);
				if (this.recentTopics.Count > 5)
				{
					string topic = this.recentTopics.Dequeue();
					this.TryMakeFavouriteTopic(topic);
				}
			}
		}

		// Token: 0x0600A6DF RID: 42719 RVA: 0x003BAEB0 File Offset: 0x003B90B0
		public void OnTopicDiscussed(object data)
		{
			string data2 = (string)data;
			if (UnityEngine.Random.value < 0.33333334f)
			{
				this.OnTopicDiscovered(data2);
			}
		}

		// Token: 0x0600A6E0 RID: 42720 RVA: 0x003BAED8 File Offset: 0x003B90D8
		private void TryMakeFavouriteTopic(string topic)
		{
			if (UnityEngine.Random.value < 0.033333335f)
			{
				if (this.favouriteTopics.Count < 5)
				{
					this.favouriteTopics.Add(topic);
					return;
				}
				this.favouriteTopics[UnityEngine.Random.Range(0, this.favouriteTopics.Count)] = topic;
			}
		}

		// Token: 0x0400828A RID: 33418
		[Serialize]
		private Queue<string> recentTopics;

		// Token: 0x0400828B RID: 33419
		[Serialize]
		private List<string> favouriteTopics;

		// Token: 0x0400828C RID: 33420
		private List<string> personalTopics;

		// Token: 0x0400828D RID: 33421
		private static readonly List<string> randomTopics = new List<string>
		{
			"Headquarters"
		};
	}
}
