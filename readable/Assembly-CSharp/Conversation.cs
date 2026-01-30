using System;
using System.Collections.Generic;

// Token: 0x0200087B RID: 2171
public class Conversation
{
	// Token: 0x040024F3 RID: 9459
	public List<MinionIdentity> minions = new List<MinionIdentity>();

	// Token: 0x040024F4 RID: 9460
	public MinionIdentity lastTalked;

	// Token: 0x040024F5 RID: 9461
	public ConversationType conversationType;

	// Token: 0x040024F6 RID: 9462
	public float lastTalkedTime;

	// Token: 0x040024F7 RID: 9463
	public Conversation.Topic lastTopic;

	// Token: 0x040024F8 RID: 9464
	public int numUtterances;

	// Token: 0x02001840 RID: 6208
	public enum ModeType
	{
		// Token: 0x04007A67 RID: 31335
		Query,
		// Token: 0x04007A68 RID: 31336
		Statement,
		// Token: 0x04007A69 RID: 31337
		Agreement,
		// Token: 0x04007A6A RID: 31338
		Disagreement,
		// Token: 0x04007A6B RID: 31339
		Musing,
		// Token: 0x04007A6C RID: 31340
		Satisfaction,
		// Token: 0x04007A6D RID: 31341
		Nominal,
		// Token: 0x04007A6E RID: 31342
		Dissatisfaction,
		// Token: 0x04007A6F RID: 31343
		Stressing,
		// Token: 0x04007A70 RID: 31344
		Segue,
		// Token: 0x04007A71 RID: 31345
		End
	}

	// Token: 0x02001841 RID: 6209
	public class Mode
	{
		// Token: 0x06009E64 RID: 40548 RVA: 0x003A3080 File Offset: 0x003A1280
		public Mode(Conversation.ModeType type, string voice, string icon, string mouth, string anim, bool newTopic = false)
		{
			this.type = type;
			this.voice = voice;
			this.mouth = mouth;
			this.anim = anim;
			this.icon = icon;
			this.newTopic = newTopic;
		}

		// Token: 0x04007A72 RID: 31346
		public Conversation.ModeType type;

		// Token: 0x04007A73 RID: 31347
		public string voice;

		// Token: 0x04007A74 RID: 31348
		public string mouth;

		// Token: 0x04007A75 RID: 31349
		public string anim;

		// Token: 0x04007A76 RID: 31350
		public string icon;

		// Token: 0x04007A77 RID: 31351
		public bool newTopic;
	}

	// Token: 0x02001842 RID: 6210
	public class Topic
	{
		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06009E65 RID: 40549 RVA: 0x003A30B8 File Offset: 0x003A12B8
		public static Dictionary<int, Conversation.Mode> Modes
		{
			get
			{
				if (Conversation.Topic._modes == null)
				{
					Conversation.Topic._modes = new Dictionary<int, Conversation.Mode>();
					foreach (Conversation.Mode mode in Conversation.Topic.modeList)
					{
						Conversation.Topic._modes[(int)mode.type] = mode;
					}
				}
				return Conversation.Topic._modes;
			}
		}

		// Token: 0x06009E66 RID: 40550 RVA: 0x003A312C File Offset: 0x003A132C
		public Topic(string topic, Conversation.ModeType mode)
		{
			this.topic = topic;
			this.mode = mode;
		}

		// Token: 0x04007A78 RID: 31352
		public static List<Conversation.Mode> modeList = new List<Conversation.Mode>
		{
			new Conversation.Mode(Conversation.ModeType.Query, "conversation_question", "mode_query", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Statement, "conversation_answer", "mode_statement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Agreement, "conversation_answer", "mode_agreement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Disagreement, "conversation_answer", "mode_disagreement", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Musing, "conversation_short", "mode_musing", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Satisfaction, "conversation_short", "mode_satisfaction", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Nominal, "conversation_short", "mode_nominal", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Dissatisfaction, "conversation_short", "mode_dissatisfaction", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Stressing, "conversation_short", "mode_stressing", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Segue, "conversation_question", "mode_segue", SpeechMonitor.PREFIX_HAPPY, "happy", true)
		};

		// Token: 0x04007A79 RID: 31353
		private static Dictionary<int, Conversation.Mode> _modes;

		// Token: 0x04007A7A RID: 31354
		public string topic;

		// Token: 0x04007A7B RID: 31355
		public Conversation.ModeType mode;
	}
}
