using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
public static class EntityTemplateExtensions
{
	// Token: 0x060002B3 RID: 691 RVA: 0x00013298 File Offset: 0x00011498
	public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController stateMachineController = go.AddOrGet<StateMachineController>();
		DefType defType = stateMachineController.GetDef<DefType>();
		if (defType == null)
		{
			defType = Activator.CreateInstance<DefType>();
			stateMachineController.AddDef(defType);
			defType.Configure(go);
		}
		return defType;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x000132DC File Offset: 0x000114DC
	public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
	{
		ComponentType componentType = go.GetComponent<ComponentType>();
		if (componentType == null)
		{
			componentType = go.AddComponent<ComponentType>();
		}
		return componentType;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00013308 File Offset: 0x00011508
	public static void RemoveDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController stateMachineController;
		if (!go.TryGetComponent<StateMachineController>(out stateMachineController))
		{
			return;
		}
		DefType def = stateMachineController.GetDef<DefType>();
		if (def == null)
		{
			return;
		}
		stateMachineController.cmpdef.defs.Remove(def);
	}
}
