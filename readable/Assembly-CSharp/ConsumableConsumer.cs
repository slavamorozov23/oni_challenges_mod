using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
	// Token: 0x06001AA0 RID: 6816 RVA: 0x00092DC0 File Offset: 0x00090FC0
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00092DFC File Offset: 0x00090FFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ConsumerManager.instance != null)
		{
			this.forbiddenTagSet = new HashSet<Tag>(ConsumerManager.instance.DefaultForbiddenTagsList);
			this.SetModelDietaryRestrictions();
			return;
		}
		this.forbiddenTagSet = new HashSet<Tag>();
		this.dietaryRestrictionTagSet = new HashSet<Tag>();
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x00092E4E File Offset: 0x0009104E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetModelDietaryRestrictions();
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x00092E5C File Offset: 0x0009105C
	private void SetModelDietaryRestrictions()
	{
		if (this.HasTag(GameTags.Minions.Models.Standard))
		{
			this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.StandardDuplicantDietaryRestrictions);
			return;
		}
		if (this.HasTag(GameTags.Minions.Models.Bionic))
		{
			this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.BionicDuplicantDietaryRestrictions);
		}
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x00092EB0 File Offset: 0x000910B0
	public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !this.forbiddenTagSet.Contains(item) && !this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x00092EE4 File Offset: 0x000910E4
	public bool IsDietRestricted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x00092F08 File Offset: 0x00091108
	public void SetPermitted(string consumable_id, bool is_allowed)
	{
		Tag item = new Tag(consumable_id);
		is_allowed = (is_allowed && !this.dietaryRestrictionTagSet.Contains(consumable_id));
		if (is_allowed)
		{
			this.forbiddenTagSet.Remove(item);
		}
		else
		{
			this.forbiddenTagSet.Add(item);
		}
		this.consumableRulesChanged.Signal();
	}

	// Token: 0x04000F56 RID: 3926
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	[HideInInspector]
	public Tag[] forbiddenTags;

	// Token: 0x04000F57 RID: 3927
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x04000F58 RID: 3928
	public HashSet<Tag> dietaryRestrictionTagSet;

	// Token: 0x04000F59 RID: 3929
	public System.Action consumableRulesChanged;
}
