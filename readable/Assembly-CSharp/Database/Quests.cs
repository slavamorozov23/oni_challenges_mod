using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F50 RID: 3920
	public class Quests : ResourceSet<Quest>
	{
		// Token: 0x06007CB6 RID: 31926 RVA: 0x00315D24 File Offset: 0x00313F24
		public Quests(ResourceSet parent) : base("Quests", parent)
		{
			this.LonelyMinionGreetingQuest = base.Add(new Quest("KnockQuest", new QuestCriteria[]
			{
				new QuestCriteria("Neighbor", null, 1, null, QuestCriteria.BehaviorFlags.None)
			}));
			this.LonelyMinionFoodQuest = base.Add(new Quest("FoodQuest", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("FoodQuality", new float[]
				{
					4f
				}, 3, new HashSet<Tag>
				{
					GameTags.Edible
				}, QuestCriteria.BehaviorFlags.UniqueItems)
			}));
			this.LonelyMinionPowerQuest = base.Add(new Quest("PluggedIn", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("SuppliedPower", new float[]
				{
					3000f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
			this.LonelyMinionDecorQuest = base.Add(new Quest("HighDecor", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("Decor", new float[]
				{
					120f
				}, 1, null, (QuestCriteria.BehaviorFlags)6)
			}));
			this.FossilHuntQuest = base.Add(new Quest("FossilHuntQuest", new QuestCriteria[]
			{
				new QuestCriteria_Equals("LostSpecimen", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostIceFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostResinFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostRockFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
		}

		// Token: 0x04005B2A RID: 23338
		public Quest LonelyMinionGreetingQuest;

		// Token: 0x04005B2B RID: 23339
		public Quest LonelyMinionFoodQuest;

		// Token: 0x04005B2C RID: 23340
		public Quest LonelyMinionPowerQuest;

		// Token: 0x04005B2D RID: 23341
		public Quest LonelyMinionDecorQuest;

		// Token: 0x04005B2E RID: 23342
		public Quest FossilHuntQuest;
	}
}
