using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000458 RID: 1112
public class CodeWriter
{
	// Token: 0x06001716 RID: 5910 RVA: 0x000838EF File Offset: 0x00081AEF
	public CodeWriter(string path)
	{
		this.Path = path;
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x00083909 File Offset: 0x00081B09
	public void Comment(string text)
	{
		this.Lines.Add("// " + text);
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x00083924 File Offset: 0x00081B24
	public void BeginPartialClass(string class_name, string parent_name = null)
	{
		string text = "public partial class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x00083970 File Offset: 0x00081B70
	public void BeginClass(string class_name, string parent_name = null)
	{
		string text = "public class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x000839B9 File Offset: 0x00081BB9
	public void EndClass()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x000839D4 File Offset: 0x00081BD4
	public void BeginNameSpace(string name)
	{
		this.Line("namespace " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x00083A00 File Offset: 0x00081C00
	public void EndNameSpace()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x00083A1B File Offset: 0x00081C1B
	public void BeginArrayStructureInitialization(string name)
	{
		this.Line("new " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x00083A47 File Offset: 0x00081C47
	public void EndArrayStructureInitialization(bool last_item)
	{
		this.Indent--;
		if (!last_item)
		{
			this.Line("},");
			return;
		}
		this.Line("}");
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x00083A71 File Offset: 0x00081C71
	public void BeginArraArrayInitialization(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x00083AA3 File Offset: 0x00081CA3
	public void EndArrayArrayInitialization(bool last_item)
	{
		this.Indent--;
		if (last_item)
		{
			this.Line("}");
			return;
		}
		this.Line("},");
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x00083ACD File Offset: 0x00081CCD
	public void BeginConstructor(string name)
	{
		this.Line("public " + name + "()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x00083AFE File Offset: 0x00081CFE
	public void EndConstructor()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x00083B19 File Offset: 0x00081D19
	public void BeginArrayAssignment(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x00083B4B File Offset: 0x00081D4B
	public void EndArrayAssignment()
	{
		this.Indent--;
		this.Line("};");
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x00083B66 File Offset: 0x00081D66
	public void FieldAssignment(string field_name, string value)
	{
		this.Line(field_name + " = " + value + ";");
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x00083B7F File Offset: 0x00081D7F
	public void BeginStructureDelegateFieldInitializer(string name)
	{
		this.Line(name + "=delegate()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x00083BAB File Offset: 0x00081DAB
	public void EndStructureDelegateFieldInitializer()
	{
		this.Indent--;
		this.Line("},");
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x00083BC6 File Offset: 0x00081DC6
	public void BeginIf(string condition)
	{
		this.Line("if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x00083BF8 File Offset: 0x00081DF8
	public void BeginElseIf(string condition)
	{
		this.Indent--;
		this.Line("}");
		this.Line("else if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x00083C4D File Offset: 0x00081E4D
	public void EndIf()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x00083C68 File Offset: 0x00081E68
	public void BeginFunctionDeclaration(string name, string parameter, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"(",
			parameter,
			")"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x00083CCC File Offset: 0x00081ECC
	public void BeginFunctionDeclaration(string name, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"()"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x00083D23 File Offset: 0x00081F23
	public void EndFunctionDeclaration()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x00083D40 File Offset: 0x00081F40
	private void InternalNamedParameter(string name, string value, bool last_parameter)
	{
		string str = "";
		if (!last_parameter)
		{
			str = ",";
		}
		this.Line(name + ":" + value + str);
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x00083D6F File Offset: 0x00081F6F
	public void NamedParameterBool(string name, bool value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString().ToLower(), last_parameter);
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x00083D85 File Offset: 0x00081F85
	public void NamedParameterInt(string name, int value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString(), last_parameter);
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x00083D96 File Offset: 0x00081F96
	public void NamedParameterFloat(string name, float value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString() + "f", last_parameter);
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x00083DB1 File Offset: 0x00081FB1
	public void NamedParameterString(string name, string value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value, last_parameter);
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x00083DBC File Offset: 0x00081FBC
	public void BeginFunctionCall(string name)
	{
		this.Line(name);
		this.Line("(");
		this.Indent++;
	}

	// Token: 0x06001734 RID: 5940 RVA: 0x00083DDE File Offset: 0x00081FDE
	public void EndFunctionCall()
	{
		this.Indent--;
		this.Line(");");
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x00083DFC File Offset: 0x00081FFC
	public void FunctionCall(string function_name, params string[] parameters)
	{
		string str = function_name + "(";
		for (int i = 0; i < parameters.Length; i++)
		{
			str += parameters[i];
			if (i != parameters.Length - 1)
			{
				str += ", ";
			}
		}
		this.Line(str + ");");
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x00083E52 File Offset: 0x00082052
	public void StructureFieldInitializer(string field, string value)
	{
		this.Line(field + " = " + value + ",");
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x00083E6C File Offset: 0x0008206C
	public void StructureArrayFieldInitializer(string field, string field_type, params string[] values)
	{
		string text = field + " = new " + field_type + "[]{ ";
		for (int i = 0; i < values.Length; i++)
		{
			text += values[i];
			if (i < values.Length - 1)
			{
				text += ", ";
			}
		}
		text += " },";
		this.Line(text);
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x00083ECC File Offset: 0x000820CC
	public void Line(string text = "")
	{
		for (int i = 0; i < this.Indent; i++)
		{
			text = "\t" + text;
		}
		this.Lines.Add(text);
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x00083F03 File Offset: 0x00082103
	public void Flush()
	{
		File.WriteAllLines(this.Path, this.Lines.ToArray());
	}

	// Token: 0x04000DA7 RID: 3495
	private List<string> Lines = new List<string>();

	// Token: 0x04000DA8 RID: 3496
	private string Path;

	// Token: 0x04000DA9 RID: 3497
	private int Indent;
}
