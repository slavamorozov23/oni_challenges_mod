using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200087E RID: 2174
public class RecentThingConversation : ConversationType
{
	// Token: 0x06003BD6 RID: 15318 RVA: 0x0014EF23 File Offset: 0x0014D123
	public RecentThingConversation()
	{
		this.id = "RecentThingConversation";
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x0014EF38 File Offset: 0x0014D138
	public override void NewTarget(MinionIdentity speaker)
	{
		ConversationMonitor.Instance smi = speaker.GetSMI<ConversationMonitor.Instance>();
		this.target = smi.GetATopic();
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x0014EF58 File Offset: 0x0014D158
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (string.IsNullOrEmpty(this.target))
		{
			return null;
		}
		List<Conversation.ModeType> list = (lastTopic == null) ? RecentThingConversation.INITIAL_MODES : RecentThingConversation.transitions[lastTopic.mode];
		Conversation.ModeType modeType = list[UnityEngine.Random.Range(0, list.Count)];
		if (modeType == Conversation.ModeType.Segue)
		{
			this.NewTarget(speaker);
			modeType = RecentThingConversation.INITIAL_MODES[UnityEngine.Random.Range(0, RecentThingConversation.INITIAL_MODES.Count)];
		}
		return new Conversation.Topic(this.target, modeType);
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0014EFD8 File Offset: 0x0014D1D8
	public override Sprite GetSprite(string topic)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			return uisprite.first;
		}
		return null;
	}

	// Token: 0x040024FC RID: 9468
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Musing
			}
		},
		{
			Conversation.ModeType.Statement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Query,
				Conversation.ModeType.Segue
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Satisfaction
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Dissatisfaction
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.Statement,
				Conversation.ModeType.Segue
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		}
	};

	// Token: 0x040024FD RID: 9469
	private static readonly List<Conversation.ModeType> INITIAL_MODES = new List<Conversation.ModeType>
	{
		Conversation.ModeType.Query,
		Conversation.ModeType.Statement,
		Conversation.ModeType.Musing
	};
}
