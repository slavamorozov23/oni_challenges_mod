using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F54 RID: 3924
	public class ScheduleBlockTypes : ResourceSet<ScheduleBlockType>
	{
		// Token: 0x06007CC1 RID: 31937 RVA: 0x00317578 File Offset: 0x00315778
		public ScheduleBlockTypes(ResourceSet parent) : base("ScheduleBlockTypes", parent)
		{
			this.Sleep = base.Add(new ScheduleBlockType("Sleep", this, UI.SCHEDULEBLOCKTYPES.SLEEP.NAME, UI.SCHEDULEBLOCKTYPES.SLEEP.DESCRIPTION, new Color(0.9843137f, 0.99215686f, 0.27058825f)));
			this.Eat = base.Add(new ScheduleBlockType("Eat", this, UI.SCHEDULEBLOCKTYPES.EAT.NAME, UI.SCHEDULEBLOCKTYPES.EAT.DESCRIPTION, new Color(0.80784315f, 0.5294118f, 0.11372549f)));
			this.Work = base.Add(new ScheduleBlockType("Work", this, UI.SCHEDULEBLOCKTYPES.WORK.NAME, UI.SCHEDULEBLOCKTYPES.WORK.DESCRIPTION, new Color(0.9372549f, 0.12941177f, 0.12941177f)));
			this.Hygiene = base.Add(new ScheduleBlockType("Hygiene", this, UI.SCHEDULEBLOCKTYPES.HYGIENE.NAME, UI.SCHEDULEBLOCKTYPES.HYGIENE.DESCRIPTION, new Color(0.45882353f, 0.1764706f, 0.34509805f)));
			this.Recreation = base.Add(new ScheduleBlockType("Recreation", this, UI.SCHEDULEBLOCKTYPES.RECREATION.NAME, UI.SCHEDULEBLOCKTYPES.RECREATION.DESCRIPTION, new Color(0.45882353f, 0.37254903f, 0.1882353f)));
		}

		// Token: 0x04005B59 RID: 23385
		public ScheduleBlockType Sleep;

		// Token: 0x04005B5A RID: 23386
		public ScheduleBlockType Eat;

		// Token: 0x04005B5B RID: 23387
		public ScheduleBlockType Work;

		// Token: 0x04005B5C RID: 23388
		public ScheduleBlockType Hygiene;

		// Token: 0x04005B5D RID: 23389
		public ScheduleBlockType Recreation;
	}
}
