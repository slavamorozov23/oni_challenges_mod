using System;
using System.Collections.Generic;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02001066 RID: 4198
	[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
	public class InterfaceToolConfig : ScriptableObject
	{
		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060081EB RID: 33259 RVA: 0x003411CB File Offset: 0x0033F3CB
		public DigAction DigAction
		{
			get
			{
				return ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(this.digAction);
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060081EC RID: 33260 RVA: 0x003411D8 File Offset: 0x0033F3D8
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060081ED RID: 33261 RVA: 0x003411E0 File Offset: 0x0033F3E0
		public global::Action InputAction
		{
			get
			{
				return (global::Action)Enum.Parse(typeof(global::Action), this.inputAction);
			}
		}

		// Token: 0x0400624D RID: 25165
		[SerializeField]
		private DigToolActionFactory.Actions digAction;

		// Token: 0x0400624E RID: 25166
		public static InterfaceToolConfig.Comparer ConfigComparer = new InterfaceToolConfig.Comparer();

		// Token: 0x0400624F RID: 25167
		[SerializeField]
		[Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
		private int priority;

		// Token: 0x04006250 RID: 25168
		[SerializeField]
		[Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
		private string inputAction = global::Action.Invalid.ToString();

		// Token: 0x02002762 RID: 10082
		public class Comparer : IComparer<InterfaceToolConfig>
		{
			// Token: 0x0600C8D0 RID: 51408 RVA: 0x004290EB File Offset: 0x004272EB
			public int Compare(InterfaceToolConfig lhs, InterfaceToolConfig rhs)
			{
				if (lhs.Priority == rhs.Priority)
				{
					return 0;
				}
				if (lhs.Priority <= rhs.Priority)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
