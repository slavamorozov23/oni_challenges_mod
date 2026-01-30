using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x020009FE RID: 2558
public class MemorySnapshot
{
	// Token: 0x06004AB8 RID: 19128 RVA: 0x001B08CC File Offset: 0x001AEACC
	public static MemorySnapshot.TypeData GetTypeData(Type type, Dictionary<int, MemorySnapshot.TypeData> types)
	{
		int hashCode = type.GetHashCode();
		MemorySnapshot.TypeData typeData = null;
		if (!types.TryGetValue(hashCode, out typeData))
		{
			typeData = new MemorySnapshot.TypeData(type);
			types[hashCode] = typeData;
		}
		return typeData;
	}

	// Token: 0x06004AB9 RID: 19129 RVA: 0x001B0900 File Offset: 0x001AEB00
	public static void IncrementFieldCount(Dictionary<int, MemorySnapshot.FieldCount> field_counts, string name)
	{
		int hashCode = name.GetHashCode();
		MemorySnapshot.FieldCount fieldCount = null;
		if (!field_counts.TryGetValue(hashCode, out fieldCount))
		{
			fieldCount = new MemorySnapshot.FieldCount();
			fieldCount.name = name;
			field_counts[hashCode] = fieldCount;
		}
		fieldCount.count++;
	}

	// Token: 0x06004ABA RID: 19130 RVA: 0x001B0944 File Offset: 0x001AEB44
	private void CountReference(MemorySnapshot.ReferenceArgs refArgs)
	{
		if (MemorySnapshot.ShouldExclude(refArgs.reference_type))
		{
			return;
		}
		if (refArgs.reference_type == MemorySnapshot.detailType)
		{
			string text;
			if (refArgs.lineage.obj as UnityEngine.Object != null)
			{
				text = "\"" + ((UnityEngine.Object)refArgs.lineage.obj).name;
			}
			else
			{
				text = "\"" + MemorySnapshot.detailTypeStr;
			}
			if (refArgs.lineage.parent0 != null)
			{
				text += "\",\"";
				text += refArgs.lineage.parent0.ToString();
			}
			if (refArgs.lineage.parent1 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent1.ToString();
			}
			if (refArgs.lineage.parent2 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent2.ToString();
			}
			if (refArgs.lineage.parent3 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent3.ToString();
			}
			if (refArgs.lineage.parent4 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent4.ToString();
			}
			text += "\"\n";
			MemorySnapshot.DetailInfo value;
			this.detailTypeCount.TryGetValue(text, out value);
			value.count++;
			if (typeof(Array).IsAssignableFrom(refArgs.reference_type) && refArgs.lineage.obj != null)
			{
				Array array = refArgs.lineage.obj as Array;
				value.numArrayEntries += ((array != null) ? array.Length : 0);
			}
			this.detailTypeCount[text] = value;
		}
		if (refArgs.reference_type.IsClass)
		{
			MemorySnapshot.GetTypeData(refArgs.reference_type, this.types).refCount++;
			MemorySnapshot.IncrementFieldCount(this.fieldCounts, refArgs.field_name);
		}
		if (refArgs.lineage.obj == null)
		{
			return;
		}
		try
		{
			if (refArgs.lineage.obj.GetType().IsClass && !this.walked.Add(refArgs.lineage.obj))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		MemorySnapshot.TypeData typeData = MemorySnapshot.GetTypeData(refArgs.lineage.obj.GetType(), this.types);
		if (typeData.type.IsClass)
		{
			typeData.instanceCount++;
			if (typeof(Array).IsAssignableFrom(typeData.type))
			{
				Array array2 = refArgs.lineage.obj as Array;
				typeData.numArrayEntries += ((array2 != null) ? array2.Length : 0);
			}
			MemorySnapshot.HierarchyNode key = new MemorySnapshot.HierarchyNode(refArgs.lineage.parent0, refArgs.lineage.parent1, refArgs.lineage.parent2, refArgs.lineage.parent3, refArgs.lineage.parent4);
			int num = 0;
			typeData.hierarchies.TryGetValue(key, out num);
			typeData.hierarchies[key] = num + 1;
		}
		foreach (FieldInfo fieldInfo in typeData.fields)
		{
			this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, new MemorySnapshot.Lineage(refArgs.lineage.obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, fieldInfo.DeclaringType)));
		}
		ICollection collection = refArgs.lineage.obj as ICollection;
		if (collection != null)
		{
			Type type = typeof(object);
			if (collection.GetType().GetElementType() != null)
			{
				type = collection.GetType().GetElementType();
			}
			else if (collection.GetType().GetGenericArguments().Length != 0)
			{
				type = collection.GetType().GetGenericArguments()[0];
			}
			if (!MemorySnapshot.ShouldExclude(type))
			{
				foreach (object obj in collection)
				{
					this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(type, refArgs.field_name + ".Item", new MemorySnapshot.Lineage(obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, collection.GetType())));
				}
			}
		}
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x001B0E4C File Offset: 0x001AF04C
	private void CountField(MemorySnapshot.FieldArgs fieldArgs)
	{
		if (MemorySnapshot.ShouldExclude(fieldArgs.field.FieldType))
		{
			return;
		}
		object obj = null;
		try
		{
			if (!fieldArgs.field.FieldType.Name.Contains("*"))
			{
				obj = fieldArgs.field.GetValue(fieldArgs.lineage.obj);
			}
		}
		catch
		{
			obj = null;
		}
		string field_name = fieldArgs.field.DeclaringType.ToString() + "." + fieldArgs.field.Name;
		this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(fieldArgs.field.FieldType, field_name, new MemorySnapshot.Lineage(obj, fieldArgs.lineage.parent3, fieldArgs.lineage.parent2, fieldArgs.lineage.parent1, fieldArgs.lineage.parent0, fieldArgs.field.DeclaringType)));
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x001B0F38 File Offset: 0x001AF138
	private static bool ShouldExclude(Type type)
	{
		return type.IsPrimitive || type.IsEnum || type == typeof(MemorySnapshot);
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x001B0F5C File Offset: 0x001AF15C
	private void CountAll()
	{
		while (this.refsToProcess.Count > 0 || this.fieldsToProcess.Count > 0)
		{
			while (this.fieldsToProcess.Count > 0)
			{
				MemorySnapshot.FieldArgs fieldArgs = this.fieldsToProcess[this.fieldsToProcess.Count - 1];
				this.fieldsToProcess.RemoveAt(this.fieldsToProcess.Count - 1);
				this.CountField(fieldArgs);
			}
			while (this.refsToProcess.Count > 0)
			{
				MemorySnapshot.ReferenceArgs refArgs = this.refsToProcess[this.refsToProcess.Count - 1];
				this.refsToProcess.RemoveAt(this.refsToProcess.Count - 1);
				this.CountReference(refArgs);
			}
		}
	}

	// Token: 0x06004ABE RID: 19134 RVA: 0x001B1018 File Offset: 0x001AF218
	public MemorySnapshot()
	{
		MemorySnapshot.Lineage lineage = new MemorySnapshot.Lineage(null, null, null, null, null, null);
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (fieldInfo.IsStatic)
				{
					this.statics.Add(fieldInfo);
					lineage.parent0 = fieldInfo.DeclaringType;
					this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, lineage));
				}
			}
		}
		this.CountAll();
		foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)))
		{
			lineage.obj = @object;
			lineage.parent0 = @object.GetType();
			this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(@object.GetType(), "Object." + @object.name, lineage));
		}
		this.CountAll();
	}

	// Token: 0x06004ABF RID: 19135 RVA: 0x001B1188 File Offset: 0x001AF388
	public void WriteTypeDetails(MemorySnapshot compare)
	{
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list = null;
		if (compare != null)
		{
			list = compare.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		}
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list2 = this.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		list2.Sort((KeyValuePair<string, MemorySnapshot.DetailInfo> x, KeyValuePair<string, MemorySnapshot.DetailInfo> y) => y.Value.count - x.Value.count);
		using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("type_details_" + MemorySnapshot.detailTypeStr)))
		{
			streamWriter.WriteLine("Delta,Count,NumArrayEntries,Type");
			foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair in list2)
			{
				int num = keyValuePair.Value.count;
				if (list != null)
				{
					foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair2 in list)
					{
						if (keyValuePair2.Key == keyValuePair.Key)
						{
							num -= keyValuePair2.Value.count;
							break;
						}
					}
				}
				TextWriter textWriter = streamWriter;
				string[] array = new string[7];
				array[0] = num.ToString();
				array[1] = ",";
				int num2 = 2;
				MemorySnapshot.DetailInfo value = keyValuePair.Value;
				array[num2] = value.count.ToString();
				array[3] = ",";
				int num3 = 4;
				value = keyValuePair.Value;
				array[num3] = value.numArrayEntries.ToString();
				array[5] = ",";
				array[6] = keyValuePair.Key;
				textWriter.Write(string.Concat(array));
			}
		}
	}

	// Token: 0x0400317E RID: 12670
	public Dictionary<int, MemorySnapshot.TypeData> types = new Dictionary<int, MemorySnapshot.TypeData>();

	// Token: 0x0400317F RID: 12671
	public Dictionary<int, MemorySnapshot.FieldCount> fieldCounts = new Dictionary<int, MemorySnapshot.FieldCount>();

	// Token: 0x04003180 RID: 12672
	public HashSet<object> walked = new HashSet<object>();

	// Token: 0x04003181 RID: 12673
	public List<FieldInfo> statics = new List<FieldInfo>();

	// Token: 0x04003182 RID: 12674
	public Dictionary<string, MemorySnapshot.DetailInfo> detailTypeCount = new Dictionary<string, MemorySnapshot.DetailInfo>();

	// Token: 0x04003183 RID: 12675
	private static readonly Type detailType = typeof(byte[]);

	// Token: 0x04003184 RID: 12676
	private static readonly string detailTypeStr = MemorySnapshot.detailType.ToString();

	// Token: 0x04003185 RID: 12677
	private List<MemorySnapshot.FieldArgs> fieldsToProcess = new List<MemorySnapshot.FieldArgs>();

	// Token: 0x04003186 RID: 12678
	private List<MemorySnapshot.ReferenceArgs> refsToProcess = new List<MemorySnapshot.ReferenceArgs>();

	// Token: 0x02001A62 RID: 6754
	public struct HierarchyNode
	{
		// Token: 0x0600A55B RID: 42331 RVA: 0x003B6844 File Offset: 0x003B4A44
		public HierarchyNode(Type parent_0, Type parent_1, Type parent_2, Type parent_3, Type parent_4)
		{
			this.parent0 = parent_0;
			this.parent1 = parent_1;
			this.parent2 = parent_2;
			this.parent3 = parent_3;
			this.parent4 = parent_4;
		}

		// Token: 0x0600A55C RID: 42332 RVA: 0x003B686C File Offset: 0x003B4A6C
		public bool Equals(MemorySnapshot.HierarchyNode a, MemorySnapshot.HierarchyNode b)
		{
			return a.parent0 == b.parent0 && a.parent1 == b.parent1 && a.parent2 == b.parent2 && a.parent3 == b.parent3 && a.parent4 == b.parent4;
		}

		// Token: 0x0600A55D RID: 42333 RVA: 0x003B68D8 File Offset: 0x003B4AD8
		public override int GetHashCode()
		{
			int num = 0;
			if (this.parent0 != null)
			{
				num += this.parent0.GetHashCode();
			}
			if (this.parent1 != null)
			{
				num += this.parent1.GetHashCode();
			}
			if (this.parent2 != null)
			{
				num += this.parent2.GetHashCode();
			}
			if (this.parent3 != null)
			{
				num += this.parent3.GetHashCode();
			}
			if (this.parent4 != null)
			{
				num += this.parent4.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600A55E RID: 42334 RVA: 0x003B6974 File Offset: 0x003B4B74
		public override string ToString()
		{
			if (this.parent4 != null)
			{
				return string.Concat(new string[]
				{
					this.parent4.ToString(),
					"--",
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent3 != null)
			{
				return string.Concat(new string[]
				{
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent2 != null)
			{
				return string.Concat(new string[]
				{
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent1 != null)
			{
				return this.parent1.ToString() + "--" + this.parent0.ToString();
			}
			return this.parent0.ToString();
		}

		// Token: 0x0400817D RID: 33149
		public Type parent0;

		// Token: 0x0400817E RID: 33150
		public Type parent1;

		// Token: 0x0400817F RID: 33151
		public Type parent2;

		// Token: 0x04008180 RID: 33152
		public Type parent3;

		// Token: 0x04008181 RID: 33153
		public Type parent4;
	}

	// Token: 0x02001A63 RID: 6755
	public class FieldCount
	{
		// Token: 0x04008182 RID: 33154
		public string name;

		// Token: 0x04008183 RID: 33155
		public int count;
	}

	// Token: 0x02001A64 RID: 6756
	public class TypeData
	{
		// Token: 0x0600A560 RID: 42336 RVA: 0x003B6B04 File Offset: 0x003B4D04
		public TypeData(Type type)
		{
			this.type = type;
			this.fields = new List<FieldInfo>();
			this.instanceCount = 0;
			this.refCount = 0;
			this.numArrayEntries = 0;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (!fieldInfo.IsStatic && !MemorySnapshot.ShouldExclude(fieldInfo.FieldType))
				{
					this.fields.Add(fieldInfo);
				}
			}
		}

		// Token: 0x04008184 RID: 33156
		public Dictionary<MemorySnapshot.HierarchyNode, int> hierarchies = new Dictionary<MemorySnapshot.HierarchyNode, int>();

		// Token: 0x04008185 RID: 33157
		public Type type;

		// Token: 0x04008186 RID: 33158
		public List<FieldInfo> fields;

		// Token: 0x04008187 RID: 33159
		public int instanceCount;

		// Token: 0x04008188 RID: 33160
		public int refCount;

		// Token: 0x04008189 RID: 33161
		public int numArrayEntries;
	}

	// Token: 0x02001A65 RID: 6757
	public struct DetailInfo
	{
		// Token: 0x0400818A RID: 33162
		public int count;

		// Token: 0x0400818B RID: 33163
		public int numArrayEntries;
	}

	// Token: 0x02001A66 RID: 6758
	private struct Lineage
	{
		// Token: 0x0600A561 RID: 42337 RVA: 0x003B6B85 File Offset: 0x003B4D85
		public Lineage(object obj, Type parent4, Type parent3, Type parent2, Type parent1, Type parent0)
		{
			this.obj = obj;
			this.parent0 = parent0;
			this.parent1 = parent1;
			this.parent2 = parent2;
			this.parent3 = parent3;
			this.parent4 = parent4;
		}

		// Token: 0x0400818C RID: 33164
		public object obj;

		// Token: 0x0400818D RID: 33165
		public Type parent0;

		// Token: 0x0400818E RID: 33166
		public Type parent1;

		// Token: 0x0400818F RID: 33167
		public Type parent2;

		// Token: 0x04008190 RID: 33168
		public Type parent3;

		// Token: 0x04008191 RID: 33169
		public Type parent4;
	}

	// Token: 0x02001A67 RID: 6759
	private struct ReferenceArgs
	{
		// Token: 0x0600A562 RID: 42338 RVA: 0x003B6BB4 File Offset: 0x003B4DB4
		public ReferenceArgs(Type reference_type, string field_name, MemorySnapshot.Lineage lineage)
		{
			this.reference_type = reference_type;
			this.lineage = lineage;
			this.field_name = field_name;
		}

		// Token: 0x04008192 RID: 33170
		public Type reference_type;

		// Token: 0x04008193 RID: 33171
		public string field_name;

		// Token: 0x04008194 RID: 33172
		public MemorySnapshot.Lineage lineage;
	}

	// Token: 0x02001A68 RID: 6760
	private struct FieldArgs
	{
		// Token: 0x0600A563 RID: 42339 RVA: 0x003B6BCB File Offset: 0x003B4DCB
		public FieldArgs(FieldInfo field, MemorySnapshot.Lineage lineage)
		{
			this.field = field;
			this.lineage = lineage;
		}

		// Token: 0x04008195 RID: 33173
		public FieldInfo field;

		// Token: 0x04008196 RID: 33174
		public MemorySnapshot.Lineage lineage;
	}
}
