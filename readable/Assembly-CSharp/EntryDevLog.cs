using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x02000D07 RID: 3335
public class EntryDevLog
{
	// Token: 0x0600673A RID: 26426 RVA: 0x0026E63C File Offset: 0x0026C83C
	[Conditional("UNITY_EDITOR")]
	public void AddModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Add(new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x0600673B RID: 26427 RVA: 0x0026E664 File Offset: 0x0026C864
	[Conditional("UNITY_EDITOR")]
	public void InsertModificationRecord(int index, EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Insert(index, new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x0600673C RID: 26428 RVA: 0x0026E690 File Offset: 0x0026C890
	private string TrimAuthor()
	{
		string text = "";
		string[] array = new string[]
		{
			"Invoke",
			"CreateInstance",
			"AwakeInternal",
			"Internal",
			"<>",
			"YamlDotNet",
			"Deserialize"
		};
		string[] array2 = new string[]
		{
			".ctor",
			"Trigger",
			"AddContentContainerRange",
			"AddContentContainer",
			"InsertContentContainer",
			"KInstantiateUI",
			"Start",
			"InitializeComponentAwake",
			"TrimAuthor",
			"InsertModificationRecord",
			"AddModificationRecord",
			"SetValue",
			"Write"
		};
		StackTrace stackTrace = new StackTrace();
		int i = 0;
		int num = 0;
		int num2 = 3;
		while (i < num2)
		{
			num++;
			if (stackTrace.FrameCount <= num)
			{
				break;
			}
			MethodBase method = stackTrace.GetFrame(num).GetMethod();
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = (flag || method.Name.Contains(array[j]));
			}
			for (int k = 0; k < array2.Length; k++)
			{
				flag = (flag || method.Name.Contains(array2[k]));
			}
			if (!flag && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("set_") && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("Instantiate"))
			{
				if (i != 0)
				{
					text += " < ";
				}
				i++;
				text += stackTrace.GetFrame(num).GetMethod().Name;
			}
		}
		return text;
	}

	// Token: 0x040046AC RID: 18092
	[SerializeField]
	public List<EntryDevLog.ModificationRecord> modificationRecords = new List<EntryDevLog.ModificationRecord>();

	// Token: 0x02001F35 RID: 7989
	public class ModificationRecord
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x0600B5B6 RID: 46518 RVA: 0x003EE98F File Offset: 0x003ECB8F
		// (set) Token: 0x0600B5B7 RID: 46519 RVA: 0x003EE997 File Offset: 0x003ECB97
		public EntryDevLog.ModificationRecord.ActionType actionType { get; private set; }

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x0600B5B8 RID: 46520 RVA: 0x003EE9A0 File Offset: 0x003ECBA0
		// (set) Token: 0x0600B5B9 RID: 46521 RVA: 0x003EE9A8 File Offset: 0x003ECBA8
		public string target { get; private set; }

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x0600B5BA RID: 46522 RVA: 0x003EE9B1 File Offset: 0x003ECBB1
		// (set) Token: 0x0600B5BB RID: 46523 RVA: 0x003EE9B9 File Offset: 0x003ECBB9
		public object newValue { get; private set; }

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x0600B5BC RID: 46524 RVA: 0x003EE9C2 File Offset: 0x003ECBC2
		// (set) Token: 0x0600B5BD RID: 46525 RVA: 0x003EE9CA File Offset: 0x003ECBCA
		public string author { get; private set; }

		// Token: 0x0600B5BE RID: 46526 RVA: 0x003EE9D3 File Offset: 0x003ECBD3
		public ModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue, string author)
		{
			this.target = target;
			this.newValue = newValue;
			this.author = author;
			this.actionType = actionType;
		}

		// Token: 0x02002A76 RID: 10870
		public enum ActionType
		{
			// Token: 0x0400BB7E RID: 47998
			Created,
			// Token: 0x0400BB7F RID: 47999
			ChangeSubEntry,
			// Token: 0x0400BB80 RID: 48000
			ChangeContent,
			// Token: 0x0400BB81 RID: 48001
			ValueChange,
			// Token: 0x0400BB82 RID: 48002
			YAMLData
		}
	}
}
