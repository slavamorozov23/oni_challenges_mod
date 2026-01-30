using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x02000533 RID: 1331
public class MySmi : MyAttributeManager<StateMachine.Instance>
{
	// Token: 0x06001CAB RID: 7339 RVA: 0x0009D150 File Offset: 0x0009B350
	public static void Init()
	{
		MyAttributes.Register(new MySmi(new Dictionary<Type, MethodInfo>
		{
			{
				typeof(MySmiGet),
				typeof(MySmi).GetMethod("FindSmi")
			},
			{
				typeof(MySmiReq),
				typeof(MySmi).GetMethod("RequireSmi")
			}
		}));
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x0009D1B4 File Offset: 0x0009B3B4
	public MySmi(Dictionary<Type, MethodInfo> attributeMap) : base(attributeMap, null)
	{
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x0009D1C0 File Offset: 0x0009B3C0
	public static StateMachine.Instance FindSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		StateMachineController component = c.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<T>();
		}
		return null;
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x0009D1EC File Offset: 0x0009B3EC
	public static StateMachine.Instance RequireSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		if (isStart)
		{
			StateMachine.Instance instance = MySmi.FindSmi<T>(c, isStart);
			Debug.Assert(instance != null, string.Format("{0} '{1}' requires a StateMachineInstance of type {2}!", c.GetType().ToString(), c.name, typeof(T)));
			return instance;
		}
		return MySmi.FindSmi<T>(c, isStart);
	}
}
