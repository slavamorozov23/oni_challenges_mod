using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02001069 RID: 4201
	[Action("Immediate")]
	public class ImmediateDigAction : DigAction
	{
		// Token: 0x060081F7 RID: 33271 RVA: 0x00341308 File Offset: 0x0033F508
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, false);
			}
		}

		// Token: 0x060081F8 RID: 33272 RVA: 0x0034132C File Offset: 0x0033F52C
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
