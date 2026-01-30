using System;
using System.Collections.Generic;
using FuzzySharp;
using STRINGS;

// Token: 0x02000476 RID: 1142
public class FuzzySearch
{
	// Token: 0x060017E3 RID: 6115 RVA: 0x000873E4 File Offset: 0x000855E4
	public static FuzzySearch.Features GetFeatures()
	{
		FuzzySearch.Features features = FuzzySearch.Features.Initialism;
		if (Localization.GetLocale() == null)
		{
			features |= FuzzySearch.Features.Suppress1And2LetterWords;
			features |= FuzzySearch.Features.SuppressMeaninglessWords;
		}
		return features;
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00087403 File Offset: 0x00085603
	public static string Canonicalize(string s)
	{
		return UI.StripLinkFormatting(UI.StripStyleFormatting(s));
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00087410 File Offset: 0x00085610
	private static int ScoreImpl_Unchecked(string searchString, string candidate)
	{
		return Fuzz.Ratio(searchString, candidate);
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x00087419 File Offset: 0x00085619
	private static int ScoreImpl(string searchString, string candidate)
	{
		return FuzzySearch.ScoreImpl_Unchecked(searchString, candidate);
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x00087424 File Offset: 0x00085624
	private static bool IsUpper(string s)
	{
		foreach (char c in s)
		{
			if (char.IsLetter(c) && !char.IsUpper(c))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x00087460 File Offset: 0x00085660
	private static FuzzySearch.Match ScoreTokens_Unchecked(string searchStringUpper, string[] tokens)
	{
		if (tokens.Length == 0)
		{
			return FuzzySearch.Match.NONE;
		}
		int? num = null;
		string token = null;
		int i = 0;
		while (i < tokens.Length)
		{
			string text = tokens[i];
			int num2 = FuzzySearch.ScoreImpl_Unchecked(searchStringUpper, text);
			if (num == null)
			{
				goto IL_4A;
			}
			int num3 = num2;
			int? num4 = num;
			if (num3 > num4.GetValueOrDefault() & num4 != null)
			{
				goto IL_4A;
			}
			IL_56:
			i++;
			continue;
			IL_4A:
			num = new int?(num2);
			token = text;
			goto IL_56;
		}
		return new FuzzySearch.Match
		{
			score = num.Value,
			token = token
		};
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000874F0 File Offset: 0x000856F0
	private static FuzzySearch.Match ScoreTokens_Unchecked(string searchStringUpper, IReadOnlyList<string> tokens)
	{
		if (tokens.Count == 0)
		{
			return FuzzySearch.Match.NONE;
		}
		int? num = null;
		string token = null;
		foreach (string text in tokens)
		{
			int num2 = FuzzySearch.ScoreImpl_Unchecked(searchStringUpper, text);
			if (num != null)
			{
				int num3 = num2;
				int? num4 = num;
				if (!(num3 > num4.GetValueOrDefault() & num4 != null))
				{
					continue;
				}
			}
			num = new int?(num2);
			token = text;
		}
		return new FuzzySearch.Match
		{
			score = num.Value,
			token = token
		};
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x000875A0 File Offset: 0x000857A0
	public static FuzzySearch.Match ScoreTokens(string searchStringUpper, string[] tokens)
	{
		return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000875A9 File Offset: 0x000857A9
	public static FuzzySearch.Match ScoreTokens(string searchStringUpper, IReadOnlyList<string> tokens)
	{
		return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000875B4 File Offset: 0x000857B4
	public static FuzzySearch.Match ScoreCanonicalCandidate(string searchStringUpper, string canonicalCandidate, string candidate = null)
	{
		FuzzySearch.Match match = new FuzzySearch.Match
		{
			score = Fuzz.WeightedRatio(searchStringUpper, canonicalCandidate),
			token = (candidate ?? canonicalCandidate)
		};
		if ((FuzzySearch.GetFeatures() & FuzzySearch.Features.Initialism) != (FuzzySearch.Features)0)
		{
			int num = Fuzz.TokenInitialismRatio(searchStringUpper, canonicalCandidate);
			if (num > match.score)
			{
				match.score = num;
			}
		}
		string[] tokens = canonicalCandidate.Split(FuzzySearch.TOKEN_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
		FuzzySearch.Match match2 = FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
		if (match2.score <= match.score)
		{
			return match;
		}
		return match2;
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x00087631 File Offset: 0x00085831
	public static FuzzySearch.Match CanonicalizeAndScore(string searchStringUpper, string candidate)
	{
		return FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, FuzzySearch.Canonicalize(candidate).ToUpper(), candidate);
	}

	// Token: 0x04000E1E RID: 3614
	public const FuzzySearch.Features PHRASE_MUTATION_FEATURES = FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords;

	// Token: 0x04000E1F RID: 3615
	public static readonly char[] TOKEN_SEPARATORS = new char[]
	{
		' ',
		'.',
		'\n',
		',',
		';',
		':',
		'?',
		'!',
		'-',
		'(',
		')',
		'[',
		']',
		'{',
		'}'
	};

	// Token: 0x0200128A RID: 4746
	[Flags]
	public enum Features
	{
		// Token: 0x04006837 RID: 26679
		Suppress1And2LetterWords = 1,
		// Token: 0x04006838 RID: 26680
		SuppressMeaninglessWords = 2,
		// Token: 0x04006839 RID: 26681
		Initialism = 4
	}

	// Token: 0x0200128B RID: 4747
	public struct Match
	{
		// Token: 0x0400683A RID: 26682
		public int score;

		// Token: 0x0400683B RID: 26683
		public string token;

		// Token: 0x0400683C RID: 26684
		public static readonly FuzzySearch.Match NONE = new FuzzySearch.Match
		{
			score = 0,
			token = string.Empty
		};
	}
}
