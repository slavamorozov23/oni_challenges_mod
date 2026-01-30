using System;
using System.Collections.Generic;

// Token: 0x020004D5 RID: 1237
public class ChoreTable
{
	// Token: 0x06001A9C RID: 6812 RVA: 0x00092CE5 File Offset: 0x00090EE5
	public ChoreTable(ChoreTable.Entry[] entries)
	{
		this.entries = entries;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x00092CF4 File Offset: 0x00090EF4
	public ref ChoreTable.Entry GetEntry<T>()
	{
		ref ChoreTable.Entry result = ref ChoreTable.InvalidEntry;
		for (int i = 0; i < this.entries.Length; i++)
		{
			if (this.entries[i].stateMachineDef is T)
			{
				result = ref this.entries[i];
				break;
			}
		}
		return ref result;
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x00092D44 File Offset: 0x00090F44
	public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
	{
		for (int i = 0; i < this.entries.Length; i++)
		{
			ChoreTable.Entry entry = this.entries[i];
			if (entry.stateMachineDef.GetStateMachineType() == typeof(StateMachineType))
			{
				return entry.choreType.priority;
			}
		}
		Debug.LogError(chore_consumer.name + "'s chore table does not have an entry for: " + typeof(StateMachineType).Name);
		return -1;
	}

	// Token: 0x04000F54 RID: 3924
	private ChoreTable.Entry[] entries;

	// Token: 0x04000F55 RID: 3925
	public static ChoreTable.Entry InvalidEntry;

	// Token: 0x02001347 RID: 4935
	public class Builder
	{
		// Token: 0x06008B73 RID: 35699 RVA: 0x0035F301 File Offset: 0x0035D501
		public ChoreTable.Builder PushInterruptGroup()
		{
			this.interruptGroupId++;
			return this;
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x0035F312 File Offset: 0x0035D512
		public ChoreTable.Builder PopInterruptGroup()
		{
			DebugUtil.Assert(this.interruptGroupId > 0);
			this.interruptGroupId--;
			return this;
		}

		// Token: 0x06008B75 RID: 35701 RVA: 0x0035F334 File Offset: 0x0035D534
		public ChoreTable.Builder Add(StateMachine.BaseDef def, bool condition = true, int forcePriority = -1)
		{
			if (condition)
			{
				ChoreTable.Builder.Info item = new ChoreTable.Builder.Info
				{
					interruptGroupId = this.interruptGroupId,
					forcePriority = forcePriority,
					def = def
				};
				this.infos.Add(item);
			}
			return this;
		}

		// Token: 0x06008B76 RID: 35702 RVA: 0x0035F378 File Offset: 0x0035D578
		public bool HasChoreType(Type choreType)
		{
			return this.infos.Exists((ChoreTable.Builder.Info info) => info.def.GetType() == choreType);
		}

		// Token: 0x06008B77 RID: 35703 RVA: 0x0035F3AC File Offset: 0x0035D5AC
		public bool TryGetChoreDef<T>(out T def) where T : StateMachine.BaseDef
		{
			for (int i = 0; i < this.infos.Count; i++)
			{
				if (this.infos[i].def != null && typeof(T).IsAssignableFrom(this.infos[i].def.GetType()))
				{
					def = (T)((object)this.infos[i].def);
					return true;
				}
			}
			def = default(T);
			return false;
		}

		// Token: 0x06008B78 RID: 35704 RVA: 0x0035F430 File Offset: 0x0035D630
		public ChoreTable CreateTable()
		{
			DebugUtil.Assert(this.interruptGroupId == 0);
			ChoreTable.Entry[] array = new ChoreTable.Entry[this.infos.Count];
			Stack<int> stack = new Stack<int>();
			int num = 10000;
			for (int i = 0; i < this.infos.Count; i++)
			{
				int num2 = (this.infos[i].forcePriority != -1) ? this.infos[i].forcePriority : (num - 100);
				num = num2;
				int num3 = 10000 - i * 100;
				int num4 = this.infos[i].interruptGroupId;
				if (num4 != 0)
				{
					if (stack.Count != num4)
					{
						stack.Push(num3);
					}
					else
					{
						num3 = stack.Peek();
					}
				}
				else if (stack.Count > 0)
				{
					stack.Pop();
				}
				array[i] = new ChoreTable.Entry(this.infos[i].def, num2, num3);
			}
			return new ChoreTable(array);
		}

		// Token: 0x04006AD0 RID: 27344
		private int interruptGroupId;

		// Token: 0x04006AD1 RID: 27345
		private List<ChoreTable.Builder.Info> infos = new List<ChoreTable.Builder.Info>();

		// Token: 0x04006AD2 RID: 27346
		private const int INVALID_PRIORITY = -1;

		// Token: 0x020027F6 RID: 10230
		private struct Info
		{
			// Token: 0x0400B130 RID: 45360
			public int interruptGroupId;

			// Token: 0x0400B131 RID: 45361
			public int forcePriority;

			// Token: 0x0400B132 RID: 45362
			public StateMachine.BaseDef def;
		}
	}

	// Token: 0x02001348 RID: 4936
	public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType> where StateMachineInstanceType : StateMachine.Instance
	{
		// Token: 0x06008B7A RID: 35706 RVA: 0x0035F540 File Offset: 0x0035D740
		public ChoreTableChore(StateMachine.BaseDef state_machine_def, ChoreType chore_type, KPrefabID prefab_id) : base(chore_type, prefab_id, prefab_id.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
		{
			this.showAvailabilityInHoverText = false;
			base.smi = (state_machine_def.CreateSMI(this) as StateMachineInstanceType);
		}
	}

	// Token: 0x02001349 RID: 4937
	public struct Entry
	{
		// Token: 0x06008B7B RID: 35707 RVA: 0x0035F588 File Offset: 0x0035D788
		public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
		{
			Type stateMachineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
			Type[] typeArguments = new Type[]
			{
				state_machine_def.GetStateMachineType(),
				stateMachineInstanceType
			};
			this.choreClassType = typeof(ChoreTable.ChoreTableChore<, >).MakeGenericType(typeArguments);
			this.choreType = new ChoreType(state_machine_def.ToString(), null, new string[0], "", "", "", "", new Tag[0], priority, priority);
			this.choreType.interruptPriority = interrupt_priority;
			this.stateMachineDef = state_machine_def;
		}

		// Token: 0x04006AD3 RID: 27347
		public Type choreClassType;

		// Token: 0x04006AD4 RID: 27348
		public ChoreType choreType;

		// Token: 0x04006AD5 RID: 27349
		public StateMachine.BaseDef stateMachineDef;
	}

	// Token: 0x0200134A RID: 4938
	public class Instance
	{
		// Token: 0x06008B7C RID: 35708 RVA: 0x0035F61C File Offset: 0x0035D81C
		public static void ResetParameters()
		{
			for (int i = 0; i < ChoreTable.Instance.parameters.Length; i++)
			{
				ChoreTable.Instance.parameters[i] = null;
			}
		}

		// Token: 0x06008B7D RID: 35709 RVA: 0x0035F644 File Offset: 0x0035D844
		public Instance(ChoreTable chore_table, KPrefabID prefab_id)
		{
			this.prefabId = prefab_id;
			this.entries = ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.Allocate();
			for (int i = 0; i < chore_table.entries.Length; i++)
			{
				this.entries.Add(new ChoreTable.Instance.Entry(chore_table.entries[i], prefab_id));
			}
		}

		// Token: 0x06008B7E RID: 35710 RVA: 0x0035F69C File Offset: 0x0035D89C
		~Instance()
		{
			this.OnCleanUp(this.prefabId);
		}

		// Token: 0x06008B7F RID: 35711 RVA: 0x0035F6D0 File Offset: 0x0035D8D0
		public void OnCleanUp(KPrefabID prefab_id)
		{
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].OnCleanUp(prefab_id);
			}
			this.entries.Recycle();
			this.entries = null;
		}

		// Token: 0x04006AD6 RID: 27350
		private static object[] parameters = new object[3];

		// Token: 0x04006AD7 RID: 27351
		private KPrefabID prefabId;

		// Token: 0x04006AD8 RID: 27352
		private ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList entries;

		// Token: 0x020027F8 RID: 10232
		private struct Entry
		{
			// Token: 0x0600CABD RID: 51901 RVA: 0x0042BACC File Offset: 0x00429CCC
			public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
			{
				ChoreTable.Instance.parameters[0] = chore_table_entry.stateMachineDef;
				ChoreTable.Instance.parameters[1] = chore_table_entry.choreType;
				ChoreTable.Instance.parameters[2] = prefab_id;
				this.chore = (Chore)Activator.CreateInstance(chore_table_entry.choreClassType, ChoreTable.Instance.parameters);
				ChoreTable.Instance.parameters[0] = null;
				ChoreTable.Instance.parameters[1] = null;
				ChoreTable.Instance.parameters[2] = null;
			}

			// Token: 0x0600CABE RID: 51902 RVA: 0x0042BB2E File Offset: 0x00429D2E
			public void OnCleanUp(KPrefabID prefab_id)
			{
				if (this.chore != null)
				{
					this.chore.Cancel("ChoreTable.Instance.OnCleanUp");
					this.chore = null;
				}
			}

			// Token: 0x0400B134 RID: 45364
			public Chore chore;
		}
	}
}
