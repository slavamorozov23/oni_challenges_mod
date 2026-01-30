using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A5B RID: 2651
public class Personality : Resource
{
	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06004D09 RID: 19721 RVA: 0x001C0723 File Offset: 0x001BE923
	public string description
	{
		get
		{
			return this.GetDescription();
		}
	}

	// Token: 0x06004D0A RID: 19722 RVA: 0x001C072C File Offset: 0x001BE92C
	[Obsolete("Modders: Use constructor with isStartingMinion parameter")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, true, "", GameTags.Minions.Models.Standard, 0)
	{
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x001C0770 File Offset: 0x001BE970
	[Obsolete("Modders: Added additional body part customization to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, true, "", GameTags.Minions.Models.Standard, 0)
	{
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x001C07B4 File Offset: 0x001BE9B4
	[Obsolete("Modders: Added a custom gravestone image to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, isStartingMinion, "", GameTags.Minions.Models.Standard, 0)
	{
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x001C07FC File Offset: 0x001BE9FC
	[Obsolete("Modders: Added 'model', 'arm_skin' and 'leg skin' to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, isStartingMinion, "", GameTags.Minions.Models.Standard, 0)
	{
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x001C0844 File Offset: 0x001BEA44
	[Obsolete("Modders: Added override_speech_mouth to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, int arm_skin, int leg_skin, string description, bool isStartingMinion, string graveStone, Tag model) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, belt, cuff, foot, hand, pelvis, leg, arm_skin, leg_skin, description, isStartingMinion, graveStone, model, 0)
	{
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x001C088C File Offset: 0x001BEA8C
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, int arm_skin, int leg_skin, string description, bool isStartingMinion, string graveStone, Tag model, int SpeechMouth) : base(name_string_key, name)
	{
		this.nameStringKey = name_string_key;
		this.genderStringKey = Gender;
		this.personalityType = PersonalityType;
		this.stresstrait = StressTrait;
		this.joyTrait = JoyTrait;
		this.stickerType = StickerType;
		this.congenitaltrait = CongenitalTrait;
		this.unformattedDescription = description;
		this.headShape = headShape;
		this.mouth = mouth;
		this.neck = neck;
		this.eyes = eyes;
		this.hair = hair;
		this.body = body;
		this.belt = belt;
		this.cuff = cuff;
		this.foot = foot;
		this.hand = hand;
		this.pelvis = pelvis;
		this.leg = leg;
		this.arm_skin = arm_skin;
		this.leg_skin = leg_skin;
		this.startingMinion = isStartingMinion;
		this.graveStone = graveStone;
		this.model = model;
		this.speech_mouth = SpeechMouth;
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x001C0985 File Offset: 0x001BEB85
	public string GetDescription()
	{
		this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
		return this.unformattedDescription;
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x001C09AC File Offset: 0x001BEBAC
	public void SetAttribute(Klei.AI.Attribute attribute, int value)
	{
		Personality.StartingAttribute item = new Personality.StartingAttribute(attribute, value);
		this.attributes.Add(item);
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x001C09CD File Offset: 0x001BEBCD
	public void AddTrait(Trait trait)
	{
		this.traits.Add(trait);
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x001C09DB File Offset: 0x001BEBDB
	public void SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(outfitType, this.Id, outfit);
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x001C09F0 File Offset: 0x001BEBF0
	public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
	{
		string result;
		if (CustomClothingOutfits.Instance.Internal_TryGetDuplicantPersonalityOutfit(outfitType, this.Id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x001C0A18 File Offset: 0x001BEC18
	public Sprite GetMiniIcon()
	{
		if (string.IsNullOrWhiteSpace(this.nameStringKey))
		{
			return Assets.GetSprite("unknown");
		}
		string str;
		if (this.nameStringKey == "MIMA")
		{
			str = "Mi-Ma";
		}
		else
		{
			str = this.nameStringKey[0].ToString() + this.nameStringKey.Substring(1).ToLower();
		}
		return Assets.GetSprite("dreamIcon_" + str);
	}

	// Token: 0x04003359 RID: 13145
	public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();

	// Token: 0x0400335A RID: 13146
	public List<Trait> traits = new List<Trait>();

	// Token: 0x0400335B RID: 13147
	public int headShape;

	// Token: 0x0400335C RID: 13148
	public int mouth;

	// Token: 0x0400335D RID: 13149
	public int neck;

	// Token: 0x0400335E RID: 13150
	public int eyes;

	// Token: 0x0400335F RID: 13151
	public int hair;

	// Token: 0x04003360 RID: 13152
	public int body;

	// Token: 0x04003361 RID: 13153
	public int belt;

	// Token: 0x04003362 RID: 13154
	public int cuff;

	// Token: 0x04003363 RID: 13155
	public int foot;

	// Token: 0x04003364 RID: 13156
	public int hand;

	// Token: 0x04003365 RID: 13157
	public int pelvis;

	// Token: 0x04003366 RID: 13158
	public int leg;

	// Token: 0x04003367 RID: 13159
	public int leg_skin;

	// Token: 0x04003368 RID: 13160
	public int arm_skin;

	// Token: 0x04003369 RID: 13161
	public int speech_mouth;

	// Token: 0x0400336A RID: 13162
	public string nameStringKey;

	// Token: 0x0400336B RID: 13163
	public string genderStringKey;

	// Token: 0x0400336C RID: 13164
	public string personalityType;

	// Token: 0x0400336D RID: 13165
	public Tag model;

	// Token: 0x0400336E RID: 13166
	public string stresstrait;

	// Token: 0x0400336F RID: 13167
	public string joyTrait;

	// Token: 0x04003370 RID: 13168
	public string stickerType;

	// Token: 0x04003371 RID: 13169
	public string congenitaltrait;

	// Token: 0x04003372 RID: 13170
	public string unformattedDescription;

	// Token: 0x04003373 RID: 13171
	public string graveStone;

	// Token: 0x04003374 RID: 13172
	public bool startingMinion;

	// Token: 0x04003375 RID: 13173
	public string requiredDlcId;

	// Token: 0x02001B6F RID: 7023
	public class StartingAttribute
	{
		// Token: 0x0600AA05 RID: 43525 RVA: 0x003C30E4 File Offset: 0x003C12E4
		public StartingAttribute(Klei.AI.Attribute attribute, int value)
		{
			this.attribute = attribute;
			this.value = value;
		}

		// Token: 0x040084FC RID: 34044
		public Klei.AI.Attribute attribute;

		// Token: 0x040084FD RID: 34045
		public int value;
	}
}
