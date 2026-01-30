using System;
using Klei.Input;

namespace Klei.Actions
{
	// Token: 0x0200106E RID: 4206
	public class DigToolActionFactory : ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
	{
		// Token: 0x06008208 RID: 33288 RVA: 0x003414AB File Offset: 0x0033F6AB
		protected override DigAction CreateAction(DigToolActionFactory.Actions action)
		{
			if (action == DigToolActionFactory.Actions.Immediate)
			{
				return new ImmediateDigAction();
			}
			if (action == DigToolActionFactory.Actions.ClearCell)
			{
				return new ClearCellDigAction();
			}
			if (action == DigToolActionFactory.Actions.MarkCell)
			{
				return new MarkCellDigAction();
			}
			throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action.");
		}

		// Token: 0x02002763 RID: 10083
		public enum Actions
		{
			// Token: 0x0400AF3A RID: 44858
			MarkCell = 145163119,
			// Token: 0x0400AF3B RID: 44859
			Immediate = -1044758767,
			// Token: 0x0400AF3C RID: 44860
			ClearCell = -1011242513,
			// Token: 0x0400AF3D RID: 44861
			Count = -1427607121
		}
	}
}
