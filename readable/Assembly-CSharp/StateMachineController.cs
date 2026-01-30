using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

// Token: 0x0200053A RID: 1338
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StateMachineController")]
public class StateMachineController : KMonoBehaviour, ISaveLoadableDetails, IStateMachineControllerHack
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x0009D7CE File Offset: 0x0009B9CE
	public StateMachineController.CmpDef cmpdef
	{
		get
		{
			return this.defHandle.Get<StateMachineController.CmpDef>();
		}
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x0009D7DB File Offset: 0x0009B9DB
	public IEnumerator<StateMachine.Instance> GetEnumerator()
	{
		return this.stateMachines.GetEnumerator();
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x0009D7ED File Offset: 0x0009B9ED
	public void AddStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!this.stateMachines.Contains(state_machine))
		{
			this.stateMachines.Add(state_machine);
			MyAttributes.OnAwake(state_machine, this);
		}
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x0009D810 File Offset: 0x0009BA10
	public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetStateMachine().saveHistory && !state_machine.GetStateMachine().debugSettings.saveHistory)
		{
			this.stateMachines.Remove(state_machine);
		}
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x0009D83E File Offset: 0x0009BA3E
	public bool HasStateMachineInstance(StateMachine.Instance state_machine)
	{
		return this.stateMachines.Contains(state_machine);
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x0009D84C File Offset: 0x0009BA4C
	public void AddDef(StateMachine.BaseDef def)
	{
		this.cmpdef.defs.Add(def);
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x0009D85F File Offset: 0x0009BA5F
	public LoggerFSSSS GetLog()
	{
		return this.log;
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x0009D867 File Offset: 0x0009BA67
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
		base.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x0009D894 File Offset: 0x0009BA94
	private void OnTargetDestroyed(object data)
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
		base.Unsubscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate, false);
		base.Unsubscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate, false);
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x0009D8F8 File Offset: 0x0009BAF8
	protected override void OnLoadLevel()
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.FreeResources();
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x0009D938 File Offset: 0x0009BB38
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x0009D980 File Offset: 0x0009BB80
	public void CreateSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			baseDef.CreateSMI(this);
		}
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0009D9E8 File Offset: 0x0009BBE8
	public void StartSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (!baseDef.preventStartSMIOnSpawn)
			{
				StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(baseDef.GetStateMachineType()).GetStateMachineInstanceType());
				if (smi != null && !smi.IsRunning())
				{
					smi.StartSM();
				}
			}
		}
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x0009DA7C File Offset: 0x0009BC7C
	public void Serialize(BinaryWriter writer)
	{
		this.serializer.Serialize(this.stateMachines, writer);
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x0009DA90 File Offset: 0x0009BC90
	public void Deserialize(IReader reader)
	{
		this.serializer.Deserialize(reader);
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x0009DA9E File Offset: 0x0009BC9E
	public bool Restore(StateMachine.Instance smi)
	{
		return this.serializer.Restore(smi);
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x0009DAAC File Offset: 0x0009BCAC
	public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
	{
		if (!this.defHandle.IsValid())
		{
			return default(DefType);
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				return defType;
			}
		}
		return default(DefType);
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x0009DB38 File Offset: 0x0009BD38
	public InterfaceType GetDefImplementingInterfaceOfType<InterfaceType>() where InterfaceType : class
	{
		if (!this.defHandle.IsValid())
		{
			return default(InterfaceType);
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			InterfaceType interfaceType = baseDef as InterfaceType;
			if (interfaceType != null)
			{
				return interfaceType;
			}
		}
		return default(InterfaceType);
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x0009DBC4 File Offset: 0x0009BDC4
	public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
	{
		List<DefType> list = new List<DefType>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				list.Add(defType);
			}
		}
		return list;
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x0009DC44 File Offset: 0x0009BE44
	public StateMachine.Instance GetSMI(Type type)
	{
		for (int i = 0; i < this.stateMachines.Count; i++)
		{
			StateMachine.Instance instance = this.stateMachines[i];
			if (type.IsAssignableFrom(instance.GetType()))
			{
				return instance;
			}
		}
		return null;
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x0009DC85 File Offset: 0x0009BE85
	public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		return this.GetSMI(typeof(StateMachineInstanceType)) as StateMachineInstanceType;
	}

	// Token: 0x06001CEC RID: 7404 RVA: 0x0009DCA4 File Offset: 0x0009BEA4
	public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		List<StateMachineInstanceType> list = new List<StateMachineInstanceType>();
		foreach (StateMachine.Instance instance in this.stateMachines)
		{
			StateMachineInstanceType stateMachineInstanceType = instance as StateMachineInstanceType;
			if (stateMachineInstanceType != null)
			{
				list.Add(stateMachineInstanceType);
			}
		}
		return list;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x0009DD10 File Offset: 0x0009BF10
	public List<IGameObjectEffectDescriptor> GetDescriptors()
	{
		List<IGameObjectEffectDescriptor> list = new List<IGameObjectEffectDescriptor>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (baseDef is IGameObjectEffectDescriptor)
			{
				list.Add(baseDef as IGameObjectEffectDescriptor);
			}
		}
		return list;
	}

	// Token: 0x04001100 RID: 4352
	public DefHandle defHandle;

	// Token: 0x04001101 RID: 4353
	private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();

	// Token: 0x04001102 RID: 4354
	private LoggerFSSSS log = new LoggerFSSSS("StateMachineController", 35);

	// Token: 0x04001103 RID: 4355
	private StateMachineSerializer serializer = new StateMachineSerializer();

	// Token: 0x04001104 RID: 4356
	private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>(delegate(StateMachineController component, object data)
	{
		component.OnTargetDestroyed(data);
	});

	// Token: 0x020013DA RID: 5082
	public class CmpDef
	{
		// Token: 0x04006C7D RID: 27773
		public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
	}
}
