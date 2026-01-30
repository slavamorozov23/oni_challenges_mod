using System;
using UnityEngine;

// Token: 0x020009D3 RID: 2515
public readonly struct JoyResponseOutfitTarget
{
	// Token: 0x06004905 RID: 18693 RVA: 0x001A6A6A File Offset: 0x001A4C6A
	public JoyResponseOutfitTarget(JoyResponseOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001A6A73 File Offset: 0x001A4C73
	public Option<string> ReadFacadeId()
	{
		return this.impl.ReadFacadeId();
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x001A6A80 File Offset: 0x001A4C80
	public void WriteFacadeId(Option<string> facadeId)
	{
		this.impl.WriteFacadeId(facadeId);
	}

	// Token: 0x06004908 RID: 18696 RVA: 0x001A6A8E File Offset: 0x001A4C8E
	public string GetMinionName()
	{
		return this.impl.GetMinionName();
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x001A6A9B File Offset: 0x001A4C9B
	public Personality GetPersonality()
	{
		return this.impl.GetPersonality();
	}

	// Token: 0x0600490A RID: 18698 RVA: 0x001A6AA8 File Offset: 0x001A4CA8
	public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.MinionInstanceTarget(minionInstance));
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x001A6ABA File Offset: 0x001A4CBA
	public static JoyResponseOutfitTarget FromPersonality(Personality personality)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.PersonalityTarget(personality));
	}

	// Token: 0x0400308D RID: 12429
	private readonly JoyResponseOutfitTarget.Implementation impl;

	// Token: 0x02001A2D RID: 6701
	public interface Implementation
	{
		// Token: 0x0600A470 RID: 42096
		Option<string> ReadFacadeId();

		// Token: 0x0600A471 RID: 42097
		void WriteFacadeId(Option<string> permitId);

		// Token: 0x0600A472 RID: 42098
		string GetMinionName();

		// Token: 0x0600A473 RID: 42099
		Personality GetPersonality();
	}

	// Token: 0x02001A2E RID: 6702
	public readonly struct MinionInstanceTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x0600A474 RID: 42100 RVA: 0x003B46E9 File Offset: 0x003B28E9
		public MinionInstanceTarget(GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x0600A475 RID: 42101 RVA: 0x003B46FE File Offset: 0x003B28FE
		public string GetMinionName()
		{
			return this.minionInstance.GetProperName();
		}

		// Token: 0x0600A476 RID: 42102 RVA: 0x003B470B File Offset: 0x003B290B
		public Personality GetPersonality()
		{
			return Db.Get().Personalities.Get(this.minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		}

		// Token: 0x0600A477 RID: 42103 RVA: 0x003B472C File Offset: 0x003B292C
		public Option<string> ReadFacadeId()
		{
			return this.wearableAccessorizer.GetJoyResponseId();
		}

		// Token: 0x0600A478 RID: 42104 RVA: 0x003B4739 File Offset: 0x003B2939
		public void WriteFacadeId(Option<string> permitId)
		{
			this.wearableAccessorizer.SetJoyResponseId(permitId);
		}

		// Token: 0x040080AE RID: 32942
		public readonly GameObject minionInstance;

		// Token: 0x040080AF RID: 32943
		public readonly WearableAccessorizer wearableAccessorizer;
	}

	// Token: 0x02001A2F RID: 6703
	public readonly struct PersonalityTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x0600A479 RID: 42105 RVA: 0x003B4747 File Offset: 0x003B2947
		public PersonalityTarget(Personality personality)
		{
			this.personality = personality;
		}

		// Token: 0x0600A47A RID: 42106 RVA: 0x003B4750 File Offset: 0x003B2950
		public string GetMinionName()
		{
			return this.personality.Name;
		}

		// Token: 0x0600A47B RID: 42107 RVA: 0x003B475D File Offset: 0x003B295D
		public Personality GetPersonality()
		{
			return this.personality;
		}

		// Token: 0x0600A47C RID: 42108 RVA: 0x003B4765 File Offset: 0x003B2965
		public Option<string> ReadFacadeId()
		{
			return this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
		}

		// Token: 0x0600A47D RID: 42109 RVA: 0x003B4778 File Offset: 0x003B2978
		public void WriteFacadeId(Option<string> facadeId)
		{
			this.personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
		}

		// Token: 0x040080B0 RID: 32944
		public readonly Personality personality;
	}
}
