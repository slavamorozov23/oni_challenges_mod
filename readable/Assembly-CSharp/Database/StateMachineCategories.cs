using System;

namespace Database
{
	// Token: 0x02000F5C RID: 3932
	public class StateMachineCategories : ResourceSet<StateMachine.Category>
	{
		// Token: 0x06007CE3 RID: 31971 RVA: 0x00319244 File Offset: 0x00317444
		public StateMachineCategories()
		{
			this.Ai = base.Add(new StateMachine.Category("Ai"));
			this.Monitor = base.Add(new StateMachine.Category("Monitor"));
			this.Chore = base.Add(new StateMachine.Category("Chore"));
			this.Misc = base.Add(new StateMachine.Category("Misc"));
		}

		// Token: 0x04005BA1 RID: 23457
		public StateMachine.Category Ai;

		// Token: 0x04005BA2 RID: 23458
		public StateMachine.Category Monitor;

		// Token: 0x04005BA3 RID: 23459
		public StateMachine.Category Chore;

		// Token: 0x04005BA4 RID: 23460
		public StateMachine.Category Misc;
	}
}
