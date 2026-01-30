using System;

namespace Klei.Actions
{
	// Token: 0x0200106C RID: 4204
	[AttributeUsage(AttributeTargets.Class)]
	public class ActionAttribute : Attribute
	{
		// Token: 0x06008202 RID: 33282 RVA: 0x00341402 File Offset: 0x0033F602
		public ActionAttribute(string actionName)
		{
			this.ActionName = actionName;
		}

		// Token: 0x04006254 RID: 25172
		public readonly string ActionName;
	}
}
