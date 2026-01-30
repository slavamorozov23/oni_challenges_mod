using System;
using System.Collections.Generic;
using Klei;
using ProcGen;

// Token: 0x020008D1 RID: 2257
public static class TemplateCache
{
	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06003E90 RID: 16016 RVA: 0x0015E4EF File Offset: 0x0015C6EF
	// (set) Token: 0x06003E91 RID: 16017 RVA: 0x0015E4F6 File Offset: 0x0015C6F6
	public static bool Initted { get; private set; }

	// Token: 0x06003E92 RID: 16018 RVA: 0x0015E4FE File Offset: 0x0015C6FE
	public static void Init()
	{
		if (TemplateCache.Initted)
		{
			return;
		}
		TemplateCache.templates = new Dictionary<string, TemplateContainer>();
		TemplateCache.Initted = true;
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x0015E518 File Offset: 0x0015C718
	public static void Clear()
	{
		TemplateCache.templates = null;
		TemplateCache.Initted = false;
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0015E528 File Offset: 0x0015C728
	public static string RewriteTemplatePath(string scopePath)
	{
		string dlcId;
		string str;
		SettingsCache.GetDlcIdAndPath(scopePath, out dlcId, out str);
		return SettingsCache.GetAbsoluteContentPath(dlcId, "templates/" + str);
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0015E550 File Offset: 0x0015C750
	public static string RewriteTemplateYaml(string scopePath)
	{
		return TemplateCache.RewriteTemplatePath(scopePath) + ".yaml";
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x0015E564 File Offset: 0x0015C764
	public static TemplateContainer GetTemplate(string templatePath)
	{
		if (!TemplateCache.templates.ContainsKey(templatePath))
		{
			TemplateCache.templates.Add(templatePath, null);
		}
		if (TemplateCache.templates[templatePath] == null)
		{
			string text = TemplateCache.RewriteTemplateYaml(templatePath);
			TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(text, null, null);
			if (templateContainer == null)
			{
				Debug.LogWarning("Missing template [" + text + "]");
			}
			templateContainer.name = templatePath;
			TemplateCache.templates[templatePath] = templateContainer;
		}
		return TemplateCache.templates[templatePath];
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x0015E5DD File Offset: 0x0015C7DD
	public static bool TemplateExists(string templatePath)
	{
		return FileSystem.FileExists(TemplateCache.RewriteTemplateYaml(templatePath));
	}

	// Token: 0x0400269F RID: 9887
	private const string defaultAssetFolder = "bases";

	// Token: 0x040026A0 RID: 9888
	private static Dictionary<string, TemplateContainer> templates;
}
