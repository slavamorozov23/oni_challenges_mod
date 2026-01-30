using System;
using System.Collections.Generic;
using System.Text;
using Database;
using STRINGS;

// Token: 0x02000E07 RID: 3591
public static class SearchUtil
{
	// Token: 0x060071CC RID: 29132 RVA: 0x002B77F0 File Offset: 0x002B59F0
	private static void CacheMeaninglessWords()
	{
		if (SearchUtil.MeaninglessWords.Count != 0)
		{
			return;
		}
		ListPool<string, SearchUtil.MatchCache>.PooledList pooledList = ListPool<string, SearchUtil.MatchCache>.Allocate();
		SearchUtil.AddCommaDelimitedSearchTerms(SEARCH_TERMS.SUPPRESSED, pooledList);
		foreach (string item in pooledList)
		{
			SearchUtil.MeaninglessWords.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x060071CD RID: 29133 RVA: 0x002B786C File Offset: 0x002B5A6C
	public static bool IsPassingScore(int score)
	{
		return score >= 79;
	}

	// Token: 0x060071CE RID: 29134 RVA: 0x002B7876 File Offset: 0x002B5A76
	public static string Canonicalize(string s)
	{
		return FuzzySearch.Canonicalize(s).ToUpper();
	}

	// Token: 0x060071CF RID: 29135 RVA: 0x002B7884 File Offset: 0x002B5A84
	public static string CanonicalizePhrase(string s)
	{
		string text = FuzzySearch.Canonicalize(s).ToUpper();
		FuzzySearch.Features features = FuzzySearch.GetFeatures();
		if ((features & (FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords)) == (FuzzySearch.Features)0)
		{
			return text;
		}
		string[] array = text.Split(FuzzySearch.TOKEN_SEPARATORS);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = (features & FuzzySearch.Features.Suppress1And2LetterWords) > (FuzzySearch.Features)0;
		bool flag2 = (features & FuzzySearch.Features.SuppressMeaninglessWords) > (FuzzySearch.Features)0;
		if (flag2)
		{
			SearchUtil.CacheMeaninglessWords();
		}
		foreach (string text2 in array)
		{
			if ((!flag || text2.Length > 2) && (!flag2 || !SearchUtil.MeaninglessWords.Contains(text2)))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendFormat(" {0}", text2);
				}
				else
				{
					stringBuilder.Append(text2);
				}
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060071D0 RID: 29136 RVA: 0x002B7944 File Offset: 0x002B5B44
	public static void AddCommaDelimitedSearchTerms(string commaDelimitedSearchTerms, List<string> searchTerms)
	{
		foreach (string item in commaDelimitedSearchTerms.ToUpper().Split(SearchUtil.COMMA_DELIMETERS, StringSplitOptions.RemoveEmptyEntries))
		{
			searchTerms.Add(item);
		}
	}

	// Token: 0x060071D1 RID: 29137 RVA: 0x002B797C File Offset: 0x002B5B7C
	public static Dictionary<string, SearchUtil.TechCache> CacheTechs()
	{
		Dictionary<string, SearchUtil.TechCache> dictionary = new Dictionary<string, SearchUtil.TechCache>();
		ListPool<ComplexRecipe, SearchUtil.TechCache>.PooledList pooledList = ListPool<ComplexRecipe, SearchUtil.TechCache>.Allocate();
		Techs techs = Db.Get().Techs;
		for (int num = 0; num != techs.Count; num++)
		{
			Tech tech = (Tech)techs.GetResource(num);
			Dictionary<string, SearchUtil.TechItemCache> dictionary2 = new Dictionary<string, SearchUtil.TechItemCache>();
			foreach (TechItem techItem in tech.unlockedItems)
			{
				pooledList.Clear();
				BuildingDef.CollectFabricationRecipes(techItem.Id, pooledList);
				List<SearchUtil.NameDescCache> list = new List<SearchUtil.NameDescCache>();
				foreach (ComplexRecipe complexRecipe in pooledList)
				{
					list.Add(new SearchUtil.NameDescCache
					{
						name = new SearchUtil.MatchCache
						{
							text = SearchUtil.Canonicalize(complexRecipe.GetUIName(false))
						},
						desc = new SearchUtil.MatchCache
						{
							text = SearchUtil.CanonicalizePhrase(complexRecipe.description)
						}
					});
				}
				TechItem techItem2 = Db.Get().TechItems.Get(techItem.Id);
				SearchUtil.TechItemCache value = new SearchUtil.TechItemCache
				{
					nameDescSearchTerms = new SearchUtil.NameDescSearchTermsCache
					{
						nameDesc = new SearchUtil.NameDescCache
						{
							name = new SearchUtil.MatchCache
							{
								text = SearchUtil.Canonicalize(techItem2.Name)
							},
							desc = new SearchUtil.MatchCache
							{
								text = SearchUtil.CanonicalizePhrase(techItem2.description)
							}
						},
						searchTerms = techItem2.searchTerms
					},
					recipes = list,
					tier = tech.tier
				};
				dictionary2[techItem.Id] = value;
			}
			SearchUtil.TechCache value2 = new SearchUtil.TechCache
			{
				tech = new SearchUtil.NameDescSearchTermsCache
				{
					nameDesc = new SearchUtil.NameDescCache
					{
						name = new SearchUtil.MatchCache
						{
							text = SearchUtil.Canonicalize(tech.Name)
						},
						desc = new SearchUtil.MatchCache
						{
							text = SearchUtil.CanonicalizePhrase(tech.desc)
						}
					},
					searchTerms = tech.searchTerms
				},
				techItems = dictionary2,
				tier = tech.tier
			};
			dictionary[tech.Id] = value2;
		}
		pooledList.Recycle();
		return dictionary;
	}

	// Token: 0x060071D2 RID: 29138 RVA: 0x002B7BFC File Offset: 0x002B5DFC
	public static SearchUtil.BuildingDefCache MakeBuildingDefCache(BuildingDef def)
	{
		SearchUtil.NameDescSearchTermsCache nameDescSearchTerms = new SearchUtil.NameDescSearchTermsCache
		{
			nameDesc = new SearchUtil.NameDescCache
			{
				name = new SearchUtil.MatchCache
				{
					text = SearchUtil.Canonicalize(def.Name)
				},
				desc = new SearchUtil.MatchCache
				{
					text = SearchUtil.CanonicalizePhrase(def.Desc)
				}
			},
			searchTerms = def.SearchTerms
		};
		SearchUtil.MatchCache effect = new SearchUtil.MatchCache
		{
			text = SearchUtil.CanonicalizePhrase(def.Effect)
		};
		List<SearchUtil.NameDescCache> list = new List<SearchUtil.NameDescCache>();
		ListPool<ComplexRecipe, PlanBuildingToggle>.PooledList pooledList = ListPool<ComplexRecipe, PlanBuildingToggle>.Allocate();
		BuildingDef.CollectFabricationRecipes(def.PrefabID, pooledList);
		foreach (ComplexRecipe complexRecipe in pooledList)
		{
			list.Add(new SearchUtil.NameDescCache
			{
				name = new SearchUtil.MatchCache
				{
					text = SearchUtil.Canonicalize(complexRecipe.GetUIName(false))
				},
				desc = new SearchUtil.MatchCache
				{
					text = SearchUtil.CanonicalizePhrase(complexRecipe.description)
				}
			});
		}
		pooledList.Recycle();
		return new SearchUtil.BuildingDefCache
		{
			nameDescSearchTerms = nameDescSearchTerms,
			effect = effect,
			recipes = list
		};
	}

	// Token: 0x04004E93 RID: 20115
	public const int MATCH_SCORE_MIN = 0;

	// Token: 0x04004E94 RID: 20116
	public const int MATCH_SCORE_MAX = 100;

	// Token: 0x04004E95 RID: 20117
	public const int MATCH_SCORE_THRESHOLD = 79;

	// Token: 0x04004E96 RID: 20118
	private static readonly HashSet<string> MeaninglessWords = new HashSet<string>();

	// Token: 0x04004E97 RID: 20119
	private static readonly char[] COMMA_DELIMETERS = new char[]
	{
		' ',
		','
	};

	// Token: 0x04004E98 RID: 20120
	private const int LHS_GT_RHS = -1;

	// Token: 0x04004E99 RID: 20121
	private const int RHS_GT_LHS = 1;

	// Token: 0x0200208C RID: 8332
	private interface IScore
	{
		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x0600B9A0 RID: 47520
		int Score { get; }
	}

	// Token: 0x0200208D RID: 8333
	private struct TieBreaker
	{
		// Token: 0x0600B9A1 RID: 47521 RVA: 0x003F8E57 File Offset: 0x003F7057
		public TieBreaker(int _globalMax)
		{
			this.globalMax = _globalMax;
			this.globalMaxCmp = 0;
			this.localMaxScore = -1;
			this.localMaxCmp = 0;
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x0600B9A2 RID: 47522 RVA: 0x003F8E75 File Offset: 0x003F7075
		public readonly bool IsTieBroken
		{
			get
			{
				return this.globalMaxCmp != 0;
			}
		}

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x003F8E80 File Offset: 0x003F7080
		private int CacheLocalScore(int score, int cmp)
		{
			if (this.localMaxScore == -1 || this.localMaxScore < score)
			{
				this.localMaxScore = score;
				this.localMaxCmp = cmp;
			}
			return this.localMaxCmp;
		}

		// Token: 0x0600B9A4 RID: 47524 RVA: 0x003F8EA8 File Offset: 0x003F70A8
		private int CacheScore(int score, int cmp)
		{
			if (score == this.globalMax)
			{
				this.globalMaxCmp = cmp;
				return this.globalMaxCmp;
			}
			return this.CacheLocalScore(score, cmp);
		}

		// Token: 0x0600B9A5 RID: 47525 RVA: 0x003F8ECC File Offset: 0x003F70CC
		public int Consider(int lhs, int rhs)
		{
			if (this.IsTieBroken)
			{
				return this.globalMaxCmp;
			}
			switch (-lhs.CompareTo(rhs))
			{
			case -1:
				return this.CacheScore(lhs, -1);
			case 0:
				if (this.localMaxScore != -1)
				{
					return this.localMaxCmp;
				}
				return 0;
			case 1:
				return this.CacheScore(rhs, 1);
			default:
				Debug.Assert(false);
				return 0;
			}
		}

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x003F8F34 File Offset: 0x003F7134
		public int Consider<T>(T lhs, T rhs) where T : IComparable, SearchUtil.IScore
		{
			if (this.IsTieBroken)
			{
				return this.globalMaxCmp;
			}
			if (lhs == null)
			{
				if (rhs != null)
				{
					return this.CacheScore(rhs.Score, 1);
				}
				if (this.localMaxScore != -1)
				{
					return this.localMaxCmp;
				}
				return 0;
			}
			else
			{
				if (rhs == null)
				{
					return this.CacheScore(lhs.Score, -1);
				}
				switch (lhs.CompareTo(rhs))
				{
				case -1:
					return this.CacheScore(lhs.Score, -1);
				case 0:
					if (this.localMaxScore != -1)
					{
						return this.localMaxCmp;
					}
					return 0;
				case 1:
					return this.CacheScore(rhs.Score, 1);
				default:
					Debug.Assert(false);
					return 0;
				}
			}
		}

		// Token: 0x04009682 RID: 38530
		private readonly int globalMax;

		// Token: 0x04009683 RID: 38531
		private int globalMaxCmp;

		// Token: 0x04009684 RID: 38532
		private int localMaxScore;

		// Token: 0x04009685 RID: 38533
		private int localMaxCmp;
	}

	// Token: 0x0200208E RID: 8334
	public class MatchCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x0600B9A7 RID: 47527 RVA: 0x003F9011 File Offset: 0x003F7211
		public int Score
		{
			get
			{
				return this.FuzzyMatch.score;
			}
		}

		// Token: 0x0600B9A8 RID: 47528 RVA: 0x003F901E File Offset: 0x003F721E
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x0600B9A9 RID: 47529 RVA: 0x003F902D File Offset: 0x003F722D
		// (set) Token: 0x0600B9AA RID: 47530 RVA: 0x003F9035 File Offset: 0x003F7235
		public FuzzySearch.Match FuzzyMatch { get; private set; }

		// Token: 0x0600B9AB RID: 47531 RVA: 0x003F9040 File Offset: 0x003F7240
		public void Bind(string searchStringUpper)
		{
			try
			{
				this.FuzzyMatch = FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, this.text, null);
			}
			catch (Exception innerException)
			{
				throw new Exception("searchStringUpper: " + searchStringUpper + ", text: " + this.text, innerException);
			}
		}

		// Token: 0x0600B9AC RID: 47532 RVA: 0x003F9090 File Offset: 0x003F7290
		public void Reset()
		{
			this.FuzzyMatch = FuzzySearch.Match.NONE;
		}

		// Token: 0x0600B9AD RID: 47533 RVA: 0x003F90A0 File Offset: 0x003F72A0
		public int CompareTo(object obj)
		{
			SearchUtil.MatchCache matchCache = (SearchUtil.MatchCache)obj;
			return -this.Score.CompareTo(matchCache.Score);
		}

		// Token: 0x04009686 RID: 38534
		public string text;
	}

	// Token: 0x0200208F RID: 8335
	public class NameDescCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x0600B9AF RID: 47535 RVA: 0x003F90D1 File Offset: 0x003F72D1
		public void Bind(string searchStringUpper)
		{
			this.name.Bind(searchStringUpper);
			this.desc.Bind(searchStringUpper);
		}

		// Token: 0x0600B9B0 RID: 47536 RVA: 0x003F90EB File Offset: 0x003F72EB
		public void Reset()
		{
			this.name.Reset();
			this.desc.Reset();
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x0600B9B1 RID: 47537 RVA: 0x003F9103 File Offset: 0x003F7303
		public int Score
		{
			get
			{
				return Math.Max(this.name.Score, this.desc.Score);
			}
		}

		// Token: 0x0600B9B2 RID: 47538 RVA: 0x003F9120 File Offset: 0x003F7320
		public int CompareTo(object obj)
		{
			SearchUtil.NameDescCache nameDescCache = (SearchUtil.NameDescCache)obj;
			int score = this.Score;
			int score2 = nameDescCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.name, nameDescCache.name);
			return tieBreaker.Consider<SearchUtil.MatchCache>(this.desc, nameDescCache.desc);
		}

		// Token: 0x04009688 RID: 38536
		public SearchUtil.MatchCache name;

		// Token: 0x04009689 RID: 38537
		public SearchUtil.MatchCache desc;
	}

	// Token: 0x02002090 RID: 8336
	public class NameDescSearchTermsCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x0600B9B4 RID: 47540 RVA: 0x003F9188 File Offset: 0x003F7388
		// (set) Token: 0x0600B9B5 RID: 47541 RVA: 0x003F9190 File Offset: 0x003F7390
		public FuzzySearch.Match SearchTermsScore { get; private set; }

		// Token: 0x0600B9B6 RID: 47542 RVA: 0x003F9199 File Offset: 0x003F7399
		public void Bind(string searchStringUpper)
		{
			this.nameDesc.Bind(searchStringUpper);
			this.SearchTermsScore = FuzzySearch.ScoreTokens(searchStringUpper, this.searchTerms);
		}

		// Token: 0x0600B9B7 RID: 47543 RVA: 0x003F91B9 File Offset: 0x003F73B9
		public void Reset()
		{
			this.nameDesc.Reset();
			this.SearchTermsScore = FuzzySearch.Match.NONE;
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x0600B9B8 RID: 47544 RVA: 0x003F91D1 File Offset: 0x003F73D1
		public int Score
		{
			get
			{
				return Math.Max(this.nameDesc.Score, this.SearchTermsScore.score);
			}
		}

		// Token: 0x0600B9B9 RID: 47545 RVA: 0x003F91EE File Offset: 0x003F73EE
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600B9BA RID: 47546 RVA: 0x003F9200 File Offset: 0x003F7400
		public int CompareTo(object obj)
		{
			SearchUtil.NameDescSearchTermsCache nameDescSearchTermsCache = (SearchUtil.NameDescSearchTermsCache)obj;
			int score = this.Score;
			int score2 = nameDescSearchTermsCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDesc.name, nameDescSearchTermsCache.nameDesc.name);
			tieBreaker.Consider(this.SearchTermsScore.score, nameDescSearchTermsCache.SearchTermsScore.score);
			return tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDesc.desc, nameDescSearchTermsCache.nameDesc.desc);
		}

		// Token: 0x0400968A RID: 38538
		public SearchUtil.NameDescCache nameDesc;

		// Token: 0x0400968B RID: 38539
		public IReadOnlyList<string> searchTerms;
	}

	// Token: 0x02002091 RID: 8337
	public class BuildingDefCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x0600B9BC RID: 47548 RVA: 0x003F929A File Offset: 0x003F749A
		// (set) Token: 0x0600B9BD RID: 47549 RVA: 0x003F92A2 File Offset: 0x003F74A2
		public SearchUtil.NameDescCache BestRecipe { get; private set; }

		// Token: 0x0600B9BE RID: 47550 RVA: 0x003F92AC File Offset: 0x003F74AC
		public void Bind(string searchStringUpper)
		{
			this.nameDescSearchTerms.Bind(searchStringUpper);
			this.effect.Bind(searchStringUpper);
			this.BestRecipe = null;
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Bind(searchStringUpper);
				if (this.BestRecipe == null || nameDescCache.CompareTo(this.BestRecipe) == -1)
				{
					this.BestRecipe = nameDescCache;
				}
			}
		}

		// Token: 0x0600B9BF RID: 47551 RVA: 0x003F933C File Offset: 0x003F753C
		public void Reset()
		{
			this.nameDescSearchTerms.Reset();
			this.effect.Reset();
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Reset();
			}
			this.BestRecipe = null;
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x0600B9C0 RID: 47552 RVA: 0x003F93AC File Offset: 0x003F75AC
		public int Score
		{
			get
			{
				return Math.Max(this.nameDescSearchTerms.Score, Math.Max(this.effect.Score, (this.BestRecipe == null) ? 0 : this.BestRecipe.Score));
			}
		}

		// Token: 0x0600B9C1 RID: 47553 RVA: 0x003F93E4 File Offset: 0x003F75E4
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600B9C2 RID: 47554 RVA: 0x003F93F4 File Offset: 0x003F75F4
		public int CompareTo(object obj)
		{
			SearchUtil.BuildingDefCache buildingDefCache = (SearchUtil.BuildingDefCache)obj;
			int score = this.Score;
			int score2 = buildingDefCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.name, buildingDefCache.nameDescSearchTerms.nameDesc.name);
			tieBreaker.Consider(this.nameDescSearchTerms.SearchTermsScore.score, buildingDefCache.nameDescSearchTerms.SearchTermsScore.score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.effect, buildingDefCache.effect);
			return tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.desc, buildingDefCache.nameDescSearchTerms.nameDesc.desc);
		}

		// Token: 0x0400968D RID: 38541
		public SearchUtil.NameDescSearchTermsCache nameDescSearchTerms;

		// Token: 0x0400968E RID: 38542
		public SearchUtil.MatchCache effect;

		// Token: 0x0400968F RID: 38543
		public List<SearchUtil.NameDescCache> recipes;
	}

	// Token: 0x02002092 RID: 8338
	public class TechItemCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x0600B9C4 RID: 47556 RVA: 0x003F94C0 File Offset: 0x003F76C0
		// (set) Token: 0x0600B9C5 RID: 47557 RVA: 0x003F94C8 File Offset: 0x003F76C8
		public SearchUtil.NameDescCache BestRecipe { get; private set; }

		// Token: 0x0600B9C6 RID: 47558 RVA: 0x003F94D4 File Offset: 0x003F76D4
		public void Bind(string searchStringUpper)
		{
			this.nameDescSearchTerms.Bind(searchStringUpper);
			this.BestRecipe = null;
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Bind(searchStringUpper);
				if (this.BestRecipe == null || nameDescCache.CompareTo(this.BestRecipe) == -1)
				{
					this.BestRecipe = nameDescCache;
				}
			}
		}

		// Token: 0x0600B9C7 RID: 47559 RVA: 0x003F9558 File Offset: 0x003F7758
		public void Reset()
		{
			this.nameDescSearchTerms.Reset();
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Reset();
			}
			this.BestRecipe = null;
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x0600B9C8 RID: 47560 RVA: 0x003F95BC File Offset: 0x003F77BC
		public int Score
		{
			get
			{
				return Math.Max(this.nameDescSearchTerms.Score, (this.BestRecipe == null) ? 0 : this.BestRecipe.Score);
			}
		}

		// Token: 0x0600B9C9 RID: 47561 RVA: 0x003F95E4 File Offset: 0x003F77E4
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600B9CA RID: 47562 RVA: 0x003F95F4 File Offset: 0x003F77F4
		public int CompareTo(object obj)
		{
			SearchUtil.TechItemCache techItemCache = (SearchUtil.TechItemCache)obj;
			int score = this.Score;
			int score2 = techItemCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.name, techItemCache.nameDescSearchTerms.nameDesc.name);
			tieBreaker.Consider(this.nameDescSearchTerms.SearchTermsScore.score, techItemCache.nameDescSearchTerms.SearchTermsScore.score);
			if (!tieBreaker.IsTieBroken)
			{
				int num2 = this.tier.CompareTo(techItemCache.tier);
				if (num2 != 0)
				{
					return num2;
				}
			}
			tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.desc, techItemCache.nameDescSearchTerms.nameDesc.desc);
			return tieBreaker.Consider<SearchUtil.NameDescCache>(this.BestRecipe, techItemCache.BestRecipe);
		}

		// Token: 0x04009691 RID: 38545
		public SearchUtil.NameDescSearchTermsCache nameDescSearchTerms;

		// Token: 0x04009692 RID: 38546
		public List<SearchUtil.NameDescCache> recipes;

		// Token: 0x04009694 RID: 38548
		public int tier;
	}

	// Token: 0x02002093 RID: 8339
	public class TechCache : IComparable
	{
		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x0600B9CC RID: 47564 RVA: 0x003F96E3 File Offset: 0x003F78E3
		// (set) Token: 0x0600B9CD RID: 47565 RVA: 0x003F96EB File Offset: 0x003F78EB
		public SearchUtil.TechItemCache BestItem { get; private set; }

		// Token: 0x0600B9CE RID: 47566 RVA: 0x003F96F4 File Offset: 0x003F78F4
		public void Bind(string searchStringUpper)
		{
			this.tech.Bind(searchStringUpper);
			this.BestItem = null;
			foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair in this.techItems)
			{
				keyValuePair.Value.Bind(searchStringUpper);
				if (this.BestItem == null || keyValuePair.Value.CompareTo(this.BestItem) == -1)
				{
					this.BestItem = keyValuePair.Value;
				}
			}
		}

		// Token: 0x0600B9CF RID: 47567 RVA: 0x003F978C File Offset: 0x003F798C
		public void Reset()
		{
			this.tech.Reset();
			foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair in this.techItems)
			{
				keyValuePair.Value.Reset();
			}
			this.BestItem = null;
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x0600B9D0 RID: 47568 RVA: 0x003F97F8 File Offset: 0x003F79F8
		public int Score
		{
			get
			{
				return Math.Max(this.tech.Score, (this.BestItem == null) ? 0 : this.BestItem.Score);
			}
		}

		// Token: 0x0600B9D1 RID: 47569 RVA: 0x003F9820 File Offset: 0x003F7A20
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600B9D2 RID: 47570 RVA: 0x003F9830 File Offset: 0x003F7A30
		public int CompareTo(object obj)
		{
			SearchUtil.TechCache techCache = (SearchUtil.TechCache)obj;
			int score = this.Score;
			int score2 = techCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.tech.nameDesc.name, techCache.tech.nameDesc.name);
			tieBreaker.Consider(this.tech.SearchTermsScore.score, techCache.tech.SearchTermsScore.score);
			if (!tieBreaker.IsTieBroken)
			{
				int num2 = this.tier.CompareTo(techCache.tier);
				if (num2 != 0)
				{
					return num2;
				}
			}
			tieBreaker.Consider<SearchUtil.MatchCache>(this.tech.nameDesc.desc, techCache.tech.nameDesc.desc);
			return tieBreaker.Consider<SearchUtil.TechItemCache>(this.BestItem, techCache.BestItem);
		}

		// Token: 0x04009695 RID: 38549
		public SearchUtil.NameDescSearchTermsCache tech;

		// Token: 0x04009696 RID: 38550
		public Dictionary<string, SearchUtil.TechItemCache> techItems;

		// Token: 0x04009698 RID: 38552
		public int tier;
	}

	// Token: 0x02002094 RID: 8340
	public class SubcategoryCache : IComparable
	{
		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x0600B9D4 RID: 47572 RVA: 0x003F991F File Offset: 0x003F7B1F
		// (set) Token: 0x0600B9D5 RID: 47573 RVA: 0x003F9927 File Offset: 0x003F7B27
		public SearchUtil.BuildingDefCache BestBuildingDef { get; private set; }

		// Token: 0x0600B9D6 RID: 47574 RVA: 0x003F9930 File Offset: 0x003F7B30
		public void Bind(string searchStringUpper)
		{
			this.subcategory.Bind(searchStringUpper);
			this.BestBuildingDef = null;
			foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
			{
				buildingDefCache.Bind(searchStringUpper);
				if (this.BestBuildingDef == null || buildingDefCache.CompareTo(this.BestBuildingDef) == -1)
				{
					this.BestBuildingDef = buildingDefCache;
				}
			}
		}

		// Token: 0x0600B9D7 RID: 47575 RVA: 0x003F99B4 File Offset: 0x003F7BB4
		public void Reset()
		{
			this.subcategory.Reset();
			foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
			{
				buildingDefCache.Reset();
			}
			this.BestBuildingDef = null;
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x0600B9D8 RID: 47576 RVA: 0x003F9A18 File Offset: 0x003F7C18
		public int Score
		{
			get
			{
				return Math.Max(this.subcategory.Score, (this.BestBuildingDef == null) ? 0 : this.BestBuildingDef.Score);
			}
		}

		// Token: 0x0600B9D9 RID: 47577 RVA: 0x003F9A40 File Offset: 0x003F7C40
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600B9DA RID: 47578 RVA: 0x003F9A50 File Offset: 0x003F7C50
		public int CompareTo(object obj)
		{
			SearchUtil.SubcategoryCache subcategoryCache = (SearchUtil.SubcategoryCache)obj;
			int score = this.Score;
			int score2 = subcategoryCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.subcategory, subcategoryCache.subcategory);
			return tieBreaker.Consider<SearchUtil.BuildingDefCache>(this.BestBuildingDef, subcategoryCache.BestBuildingDef);
		}

		// Token: 0x04009699 RID: 38553
		public SearchUtil.MatchCache subcategory;

		// Token: 0x0400969A RID: 38554
		public HashSet<SearchUtil.BuildingDefCache> buildingDefs;
	}
}
