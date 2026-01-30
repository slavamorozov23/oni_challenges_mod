using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053B RID: 1339
public static class StateMachineControllerExtensions
{
	// Token: 0x06001CF0 RID: 7408 RVA: 0x0009DDD8 File Offset: 0x0009BFD8
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this StateMachine.Instance smi) where StateMachineInstanceType : StateMachine.Instance
	{
		return smi.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x0009DDE5 File Offset: 0x0009BFE5
	public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef
	{
		return cmp.gameObject.GetDef<DefType>();
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x0009DDF4 File Offset: 0x0009BFF4
	public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component == null)
		{
			return default(DefType);
		}
		return component.GetDef<DefType>();
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x0009DE24 File Offset: 0x0009C024
	public static InterfaceType GetDefImplementingInterface<InterfaceType>(this GameObject go) where InterfaceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component == null)
		{
			return default(InterfaceType);
		}
		return component.GetDefImplementingInterfaceOfType<InterfaceType>();
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0009DE51 File Offset: 0x0009C051
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0009DE60 File Offset: 0x0009C060
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<StateMachineInstanceType>();
		}
		return default(StateMachineInstanceType);
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x0009DE8D File Offset: 0x0009C08D
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x0009DE9C File Offset: 0x0009C09C
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetAllSMI<StateMachineInstanceType>();
		}
		return new List<StateMachineInstanceType>();
	}
}
