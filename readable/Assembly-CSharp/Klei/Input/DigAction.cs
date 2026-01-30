using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02001067 RID: 4199
	[ActionType("InterfaceTool", "Dig", true)]
	public abstract class DigAction
	{
		// Token: 0x060081F0 RID: 33264 RVA: 0x00341230 File Offset: 0x0033F430
		public void Uproot(int cell)
		{
			if (!Grid.ObjectLayers[1].ContainsKey(cell))
			{
				if (Grid.ObjectLayers[5].ContainsKey(cell))
				{
					GameObject gameObject = Grid.ObjectLayers[5][cell];
					if (gameObject == null)
					{
						return;
					}
					IDigActionEntity component = gameObject.GetComponent<IDigActionEntity>();
					this.EntityDig(component);
				}
				return;
			}
			GameObject gameObject2 = Grid.ObjectLayers[1][cell];
			if (gameObject2 == null)
			{
				return;
			}
			IDigActionEntity component2 = gameObject2.GetComponent<IDigActionEntity>();
			this.EntityDig(component2);
		}

		// Token: 0x060081F1 RID: 33265
		public abstract void Dig(int cell, int distFromOrigin);

		// Token: 0x060081F2 RID: 33266
		protected abstract void EntityDig(IDigActionEntity digAction);
	}
}
