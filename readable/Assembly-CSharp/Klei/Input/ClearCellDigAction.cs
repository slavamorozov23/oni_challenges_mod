using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x0200106A RID: 4202
	[Action("Clear Cell")]
	public class ClearCellDigAction : DigAction
	{
		// Token: 0x060081FA RID: 33274 RVA: 0x00341340 File Offset: 0x0033F540
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, true);
			}
		}

		// Token: 0x060081FB RID: 33275 RVA: 0x00341364 File Offset: 0x0033F564
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.Dig();
		}
	}
}
