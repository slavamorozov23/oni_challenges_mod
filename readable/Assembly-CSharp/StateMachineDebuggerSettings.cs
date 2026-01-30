using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class StateMachineDebuggerSettings : ScriptableObject
{
	// Token: 0x06001CF8 RID: 7416 RVA: 0x0009DEC5 File Offset: 0x0009C0C5
	public IEnumerator<StateMachineDebuggerSettings.Entry> GetEnumerator()
	{
		return this.entries.GetEnumerator();
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x0009DED7 File Offset: 0x0009C0D7
	public static StateMachineDebuggerSettings Get()
	{
		if (StateMachineDebuggerSettings._Instance == null)
		{
			StateMachineDebuggerSettings._Instance = Resources.Load<StateMachineDebuggerSettings>("StateMachineDebuggerSettings");
			StateMachineDebuggerSettings._Instance.Initialize();
		}
		return StateMachineDebuggerSettings._Instance;
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x0009DF04 File Offset: 0x0009C104
	private void Initialize()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (typeof(StateMachine).IsAssignableFrom(type))
			{
				this.CreateEntry(type);
			}
		}
		this.entries.RemoveAll((StateMachineDebuggerSettings.Entry x) => x.type == null);
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x0009DF94 File Offset: 0x0009C194
	public StateMachineDebuggerSettings.Entry CreateEntry(Type type)
	{
		foreach (StateMachineDebuggerSettings.Entry entry in this.entries)
		{
			if (type.FullName == entry.typeName)
			{
				entry.type = type;
				return entry;
			}
		}
		StateMachineDebuggerSettings.Entry entry2 = new StateMachineDebuggerSettings.Entry(type);
		this.entries.Add(entry2);
		return entry2;
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x0009E014 File Offset: 0x0009C214
	public void Clear()
	{
		this.entries.Clear();
		this.Initialize();
	}

	// Token: 0x04001105 RID: 4357
	public List<StateMachineDebuggerSettings.Entry> entries = new List<StateMachineDebuggerSettings.Entry>();

	// Token: 0x04001106 RID: 4358
	private static StateMachineDebuggerSettings _Instance;

	// Token: 0x020013DC RID: 5084
	[Serializable]
	public class Entry
	{
		// Token: 0x06008E22 RID: 36386 RVA: 0x00367C48 File Offset: 0x00365E48
		public Entry(Type type)
		{
			this.typeName = type.FullName;
			this.type = type;
		}

		// Token: 0x04006C7F RID: 27775
		public Type type;

		// Token: 0x04006C80 RID: 27776
		public string typeName;

		// Token: 0x04006C81 RID: 27777
		public bool breakOnGoTo;

		// Token: 0x04006C82 RID: 27778
		public bool enableConsoleLogging;

		// Token: 0x04006C83 RID: 27779
		public bool saveHistory;
	}
}
