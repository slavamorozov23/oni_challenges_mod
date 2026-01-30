using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200069E RID: 1694
public class DevToolFuzzy : DevTool
{
	// Token: 0x060029B2 RID: 10674 RVA: 0x000EF8DF File Offset: 0x000EDADF
	public DevToolFuzzy()
	{
		this.mostRecentEditTime = Time.unscaledTime;
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x000EF91C File Offset: 0x000EDB1C
	private void RecipesUi(StringBuilder sb, string id, List<SearchUtil.NameDescCache> recipes)
	{
		int num = 0;
		foreach (SearchUtil.NameDescCache nameDescCache in recipes)
		{
			if (nameDescCache.Score > num)
			{
				num = nameDescCache.Score;
			}
		}
		if (!this.IsPassingScore(num))
		{
			return;
		}
		sb.Clear();
		sb.AppendFormat("[{0}] Recipes##{1}", num, id);
		if (ImGui.CollapsingHeader(sb.ToString()))
		{
			ImGui.Indent();
			foreach (SearchUtil.NameDescCache nameDescCache2 in recipes)
			{
				if (this.IsPassingScore(nameDescCache2.Score))
				{
					sb.Clear();
					sb.AppendFormat("{0}##{1}", DevToolFuzzy.FormatScoreDisplay(nameDescCache2.Score, nameDescCache2.name.text), id);
					if (ImGui.CollapsingHeader(sb.ToString()))
					{
						this.DisplayIfScorePasses(nameDescCache2);
					}
				}
			}
			ImGui.Unindent();
		}
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000EFA38 File Offset: 0x000EDC38
	private void TechItemUi(StringBuilder sb, string id, SearchUtil.TechItemCache techItem, SearchUtil.TechCache parentTech = null)
	{
		if (!this.IsPassingScore(techItem.Score))
		{
			return;
		}
		sb.Clear();
		sb.AppendFormat("{0}##TechItem{1}", DevToolFuzzy.FormatScoreDisplay(techItem.Score, techItem.nameDescSearchTerms.nameDesc.name.text), id);
		string text = sb.ToString();
		if (ImGui.CollapsingHeader(text))
		{
			ImGui.Indent();
			if (parentTech != null)
			{
				ImGui.LabelText("Parent Tech", parentTech.tech.nameDesc.name.text);
			}
			this.DisplayIfScorePasses(techItem.nameDescSearchTerms);
			this.RecipesUi(sb, text, techItem.recipes);
			ImGui.Unindent();
		}
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x000EFAE0 File Offset: 0x000EDCE0
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.InputText("Search Text", ref this.searchText, 30U))
		{
			this.refresh = true;
			this.mostRecentEditTime = Time.unscaledTime;
		}
		if (this.refresh && Time.unscaledTime - this.mostRecentEditTime > 0.4f)
		{
			this.Refresh();
			this.refresh = false;
		}
		ImGui.DragInt("Score Threshold", ref this.scoreThreshold, 0.5f, 0, 100);
		StringBuilder stringBuilder = new StringBuilder();
		if (ImGui.CollapsingHeader("Techs"))
		{
			ImGui.Indent();
			foreach (SearchUtil.TechCache techCache in this.techs)
			{
				if (this.IsPassingScore(techCache.Score))
				{
					stringBuilder.Clear();
					stringBuilder.AppendFormat("{0}##Tech", DevToolFuzzy.FormatScoreDisplay(techCache.Score, techCache.tech.nameDesc.name.text));
					string text = stringBuilder.ToString();
					if (ImGui.CollapsingHeader(text))
					{
						ImGui.Indent();
						this.DisplayIfScorePasses(techCache.tech);
						foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair in techCache.techItems)
						{
							this.TechItemUi(stringBuilder, text, keyValuePair.Value, null);
						}
						ImGui.Unindent();
					}
				}
			}
			ImGui.Unindent();
		}
		if (ImGui.CollapsingHeader("TechItems"))
		{
			ImGui.Indent();
			foreach (SearchUtil.TechCache techCache2 in this.techs)
			{
				foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair2 in techCache2.techItems)
				{
					this.TechItemUi(stringBuilder, "TechItem", keyValuePair2.Value, techCache2);
				}
			}
			ImGui.Unindent();
		}
		if (ImGui.CollapsingHeader("BuildingDefs"))
		{
			ImGui.Indent();
			foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
			{
				if (this.IsPassingScore(buildingDefCache.Score))
				{
					stringBuilder.Clear();
					stringBuilder.AppendFormat("{0}##BuildingDef", DevToolFuzzy.FormatScoreDisplay(buildingDefCache.Score, buildingDefCache.nameDescSearchTerms.nameDesc.name.text));
					string text2 = stringBuilder.ToString();
					if (ImGui.CollapsingHeader(text2))
					{
						ImGui.Indent();
						this.DisplayIfScorePasses(buildingDefCache.nameDescSearchTerms);
						this.DisplayIfScorePasses("Effect", buildingDefCache.effect);
						this.RecipesUi(stringBuilder, text2, buildingDefCache.recipes);
						ImGui.Unindent();
					}
				}
			}
			ImGui.Unindent();
		}
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000EFE00 File Offset: 0x000EE000
	private void Refresh()
	{
		string text = this.searchText.ToUpper().Trim();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.techs.Count == 0)
		{
			foreach (KeyValuePair<string, SearchUtil.TechCache> keyValuePair in SearchUtil.CacheTechs())
			{
				this.techs.Add(keyValuePair.Value);
			}
		}
		foreach (SearchUtil.TechCache techCache in this.techs)
		{
			techCache.Bind(text);
		}
		this.techs.Sort();
		if (this.buildingDefs.Count == 0)
		{
			foreach (BuildingDef def in Assets.BuildingDefs)
			{
				this.buildingDefs.Add(SearchUtil.MakeBuildingDefCache(def));
			}
		}
		foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
		{
			buildingDefCache.Bind(text);
		}
		this.buildingDefs.Sort();
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x000EFF78 File Offset: 0x000EE178
	private bool IsPassingScore(int score)
	{
		return score >= this.scoreThreshold;
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x000EFF86 File Offset: 0x000EE186
	private static string FormatScoreDisplay(int score, string text)
	{
		return string.Format("[{0}] {1}", score, FuzzySearch.Canonicalize(text));
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x000EFFA0 File Offset: 0x000EE1A0
	private static void DisplayScore(int score, string label, string token, string text)
	{
		ImGui.Text(string.Format("[{0}]", score));
		ImGui.SameLine();
		ImGui.Text(label);
		ImGui.SameLine();
		ImGui.Text(string.Format("({0})", token));
		ImGui.SameLine();
		ImGui.TextWrapped(text);
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000EFFED File Offset: 0x000EE1ED
	private static void DisplayScore(string label, SearchUtil.MatchCache match)
	{
		DevToolFuzzy.DisplayScore(match.Score, label, match.FuzzyMatch.token, match.text);
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x000F000C File Offset: 0x000EE20C
	private void DisplayIfScorePasses(string label, SearchUtil.MatchCache match)
	{
		if (this.IsPassingScore(match.Score))
		{
			DevToolFuzzy.DisplayScore(label, match);
		}
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x000F0023 File Offset: 0x000EE223
	private void DisplayIfScorePasses(SearchUtil.NameDescCache nameDesc)
	{
		this.DisplayIfScorePasses("Name", nameDesc.name);
		this.DisplayIfScorePasses("Desc", nameDesc.desc);
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000F0048 File Offset: 0x000EE248
	private void DisplayIfScorePasses(SearchUtil.NameDescSearchTermsCache nameDescSearchTerms)
	{
		this.DisplayIfScorePasses(nameDescSearchTerms.nameDesc);
		if (this.IsPassingScore(nameDescSearchTerms.SearchTermsScore.score))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendJoin<string>(", ", nameDescSearchTerms.searchTerms);
			DevToolFuzzy.DisplayScore(nameDescSearchTerms.SearchTermsScore.score, "SearchTerms", nameDescSearchTerms.SearchTermsScore.token, stringBuilder.ToString());
		}
	}

	// Token: 0x0400189A RID: 6298
	private string searchText = "";

	// Token: 0x0400189B RID: 6299
	private float mostRecentEditTime;

	// Token: 0x0400189C RID: 6300
	private bool refresh;

	// Token: 0x0400189D RID: 6301
	private const float REFRESH_DELAY = 0.4f;

	// Token: 0x0400189E RID: 6302
	private int scoreThreshold = 79;

	// Token: 0x0400189F RID: 6303
	private readonly List<SearchUtil.TechCache> techs = new List<SearchUtil.TechCache>();

	// Token: 0x040018A0 RID: 6304
	private readonly List<SearchUtil.BuildingDefCache> buildingDefs = new List<SearchUtil.BuildingDefCache>();
}
