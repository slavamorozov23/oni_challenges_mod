using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BD4 RID: 3028
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005AB8 RID: 23224 RVA: 0x0020DD68 File Offset: 0x0020BF68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.loadCharmed && DlcManager.IsExpansion1Active())
		{
			base.gameObject.AddTag(GameTags.CharmedArtifact);
			this.SetEntombedDecor();
		}
		else
		{
			this.loadCharmed = false;
			this.SetAnalyzedDecor();
		}
		this.UpdateStatusItem();
		Components.SpaceArtifacts.Add(this);
		this.UpdateAnim();
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x0020DDC6 File Offset: 0x0020BFC6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SpaceArtifacts.Remove(this);
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x0020DDD9 File Offset: 0x0020BFD9
	public void RemoveCharm()
	{
		base.gameObject.RemoveTag(GameTags.CharmedArtifact);
		this.UpdateStatusItem();
		this.loadCharmed = false;
		this.UpdateAnim();
		this.SetAnalyzedDecor();
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x0020DE04 File Offset: 0x0020C004
	private void SetEntombedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(DECOR.BONUS.TIER0);
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x0020DE16 File Offset: 0x0020C016
	private void SetAnalyzedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(this.artifactTier.decorValues);
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x0020DE30 File Offset: 0x0020C030
	public void UpdateStatusItem()
	{
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, null);
			return;
		}
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, false);
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x0020DE92 File Offset: 0x0020C092
	public void SetArtifactTier(ArtifactTier tier)
	{
		this.artifactTier = tier;
	}

	// Token: 0x06005ABF RID: 23231 RVA: 0x0020DE9B File Offset: 0x0020C09B
	public ArtifactTier GetArtifactTier()
	{
		return this.artifactTier;
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x0020DEA3 File Offset: 0x0020C0A3
	public void SetUIAnim(string anim)
	{
		this.ui_anim = anim;
	}

	// Token: 0x06005AC1 RID: 23233 RVA: 0x0020DEAC File Offset: 0x0020C0AC
	public string GetUIAnim()
	{
		return this.ui_anim;
	}

	// Token: 0x06005AC2 RID: 23234 RVA: 0x0020DEB4 File Offset: 0x0020C0B4
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			Descriptor item = new Descriptor(STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE_TOOLTIP.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		Descriptor item2 = new Descriptor(string.Format("This is an artifact from space", Array.Empty<object>()), string.Format("This is the tooltip string", Array.Empty<object>()), Descriptor.DescriptorType.Information, false);
		list.Add(item2);
		return list;
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x0020DF64 File Offset: 0x0020C164
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x0020DF6C File Offset: 0x0020C16C
	private void UpdateAnim()
	{
		string s;
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			s = "entombed_" + this.uniqueAnimNameFragment.Replace("idle_", "");
		}
		else
		{
			s = this.uniqueAnimNameFragment;
		}
		base.GetComponent<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005AC5 RID: 23237 RVA: 0x0020DFD0 File Offset: 0x0020C1D0
	[OnDeserialized]
	public void OnDeserialize()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null)
		{
			component.deleteOffGrid = false;
		}
	}

	// Token: 0x04003C7F RID: 15487
	public const string ID = "SpaceArtifact";

	// Token: 0x04003C80 RID: 15488
	private const string charmedPrefix = "entombed_";

	// Token: 0x04003C81 RID: 15489
	private const string idlePrefix = "idle_";

	// Token: 0x04003C82 RID: 15490
	[SerializeField]
	private string ui_anim;

	// Token: 0x04003C83 RID: 15491
	[Serialize]
	private bool loadCharmed = true;

	// Token: 0x04003C84 RID: 15492
	public ArtifactTier artifactTier;

	// Token: 0x04003C85 RID: 15493
	public ArtifactType artifactType;

	// Token: 0x04003C86 RID: 15494
	public string uniqueAnimNameFragment;
}
