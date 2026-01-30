using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02001068 RID: 4200
	[Action("Mark Cell")]
	public class MarkCellDigAction : DigAction
	{
		// Token: 0x060081F4 RID: 33268 RVA: 0x003412B0 File Offset: 0x0033F4B0
		public override void Dig(int cell, int distFromOrigin)
		{
			GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
			if (gameObject != null)
			{
				Prioritizable component = gameObject.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}

		// Token: 0x060081F5 RID: 33269 RVA: 0x003412F3 File Offset: 0x0033F4F3
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.MarkForDig(true);
		}
	}
}
