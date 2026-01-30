using System;
using System.Collections.Generic;

namespace Klei.Actions
{
	// Token: 0x0200106D RID: 4205
	public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
	{
		// Token: 0x06008203 RID: 33283 RVA: 0x00341414 File Offset: 0x0033F614
		public static ActionType GetOrCreateAction(EnumType actionType)
		{
			ActionType result;
			if (!ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances.TryGetValue(actionType, out result))
			{
				ActionFactory<ActionFactoryType, ActionType, EnumType>.EnsureFactoryInstance();
				result = (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances[actionType] = ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory.CreateAction(actionType));
			}
			return result;
		}

		// Token: 0x06008204 RID: 33284 RVA: 0x00341453 File Offset: 0x0033F653
		private static void EnsureFactoryInstance()
		{
			if (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory != null)
			{
				return;
			}
			ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory = (Activator.CreateInstance(typeof(ActionFactoryType)) as ActionFactoryType);
		}

		// Token: 0x06008205 RID: 33285 RVA: 0x00341480 File Offset: 0x0033F680
		protected virtual ActionType CreateAction(EnumType actionType)
		{
			throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
		}

		// Token: 0x04006255 RID: 25173
		private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();

		// Token: 0x04006256 RID: 25174
		private static ActionFactoryType actionFactory = default(ActionFactoryType);
	}
}
