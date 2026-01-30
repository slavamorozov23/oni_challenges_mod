using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020009E3 RID: 2531
[Serializable]
public class LocString
{
	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x060049AB RID: 18859 RVA: 0x001AB684 File Offset: 0x001A9884
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x060049AC RID: 18860 RVA: 0x001AB68C File Offset: 0x001A988C
	public StringKey key
	{
		get
		{
			return this._key;
		}
	}

	// Token: 0x060049AD RID: 18861 RVA: 0x001AB694 File Offset: 0x001A9894
	public LocString(string text)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x060049AE RID: 18862 RVA: 0x001AB6AF File Offset: 0x001A98AF
	public LocString(string text, string keystring)
	{
		this._text = text;
		this._key = new StringKey(keystring);
	}

	// Token: 0x060049AF RID: 18863 RVA: 0x001AB6CA File Offset: 0x001A98CA
	public LocString(string text, bool isLocalized)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x001AB6E5 File Offset: 0x001A98E5
	public static implicit operator LocString(string text)
	{
		return new LocString(text);
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x001AB6ED File Offset: 0x001A98ED
	public static implicit operator string(LocString loc_string)
	{
		return loc_string.text;
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x001AB6F5 File Offset: 0x001A98F5
	public override string ToString()
	{
		return Strings.Get(this.key).String;
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x001AB707 File Offset: 0x001A9907
	public void SetKey(string key_name)
	{
		this._key = new StringKey(key_name);
	}

	// Token: 0x060049B4 RID: 18868 RVA: 0x001AB715 File Offset: 0x001A9915
	public void SetKey(StringKey key)
	{
		this._key = key;
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x001AB71E File Offset: 0x001A991E
	public string Replace(string search, string replacement)
	{
		return this.ToString().Replace(search, replacement);
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x001AB730 File Offset: 0x001A9930
	public static void CreateLocStringKeys(Type type, string parent_path = "STRINGS.")
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		string text = parent_path;
		if (text == null)
		{
			text = "";
		}
		text = text + type.Name + ".";
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + parent_path);
				}
				else
				{
					string text2 = text + fieldInfo.Name;
					LocString locString = (LocString)fieldInfo.GetValue(null);
					locString.SetKey(text2);
					string text3 = locString.text;
					Strings.Add(new string[]
					{
						text2,
						text3
					});
					fieldInfo.SetValue(null, locString);
				}
			}
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < nestedTypes.Length; i++)
		{
			LocString.CreateLocStringKeys(nestedTypes[i], text);
		}
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x001AB81C File Offset: 0x001A9A1C
	public static string[] GetStrings(Type type)
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < fields.Length; i++)
		{
			LocString locString = (LocString)fields[i].GetValue(null);
			list.Add(locString.text);
		}
		return list.ToArray();
	}

	// Token: 0x04003103 RID: 12547
	[SerializeField]
	private string _text;

	// Token: 0x04003104 RID: 12548
	[SerializeField]
	private StringKey _key;

	// Token: 0x04003105 RID: 12549
	public const BindingFlags data_member_fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
}
