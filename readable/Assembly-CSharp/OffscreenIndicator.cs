using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A77 RID: 2679
[AddComponentMenu("KMonoBehaviour/scripts/OffscreenIndicator")]
public class OffscreenIndicator : KMonoBehaviour
{
	// Token: 0x06004DF3 RID: 19955 RVA: 0x001C488B File Offset: 0x001C2A8B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OffscreenIndicator.Instance = this;
	}

	// Token: 0x06004DF4 RID: 19956 RVA: 0x001C4899 File Offset: 0x001C2A99
	protected override void OnForcedCleanUp()
	{
		OffscreenIndicator.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06004DF5 RID: 19957 RVA: 0x001C48A8 File Offset: 0x001C2AA8
	private void Update()
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in this.targets)
		{
			this.UpdateArrow(keyValuePair.Value, keyValuePair.Key);
		}
	}

	// Token: 0x06004DF6 RID: 19958 RVA: 0x001C4908 File Offset: 0x001C2B08
	public void ActivateIndicator(GameObject target)
	{
		if (!this.targets.ContainsKey(target))
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(target, "ui", false);
			if (uisprite != null)
			{
				this.ActivateIndicator(target, uisprite);
			}
		}
	}

	// Token: 0x06004DF7 RID: 19959 RVA: 0x001C493C File Offset: 0x001C2B3C
	public void ActivateIndicator(GameObject target, GameObject iconSource)
	{
		if (!this.targets.ContainsKey(target))
		{
			MinionIdentity component = iconSource.GetComponent<MinionIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon").gameObject.SetActive(false);
				CrewPortrait reference = gameObject.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait");
				reference.gameObject.SetActive(true);
				reference.SetIdentityObject(component, true);
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x06004DF8 RID: 19960 RVA: 0x001C49C8 File Offset: 0x001C2BC8
	public void ActivateIndicator(GameObject target, global::Tuple<Sprite, Color> icon)
	{
		if (!this.targets.ContainsKey(target))
		{
			GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
			Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon");
			if (icon != null)
			{
				reference.sprite = icon.first;
				reference.color = icon.second;
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x06004DF9 RID: 19961 RVA: 0x001C4A2F File Offset: 0x001C2C2F
	public void DeactivateIndicator(GameObject target)
	{
		if (this.targets.ContainsKey(target))
		{
			UnityEngine.Object.Destroy(this.targets[target]);
			this.targets.Remove(target);
		}
	}

	// Token: 0x06004DFA RID: 19962 RVA: 0x001C4A60 File Offset: 0x001C2C60
	private void UpdateArrow(GameObject arrow, GameObject target)
	{
		if (target == null)
		{
			UnityEngine.Object.Destroy(arrow);
			this.targets.Remove(target);
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(target.transform.position);
		if ((double)vector.x > 0.3 && (double)vector.x < 0.7 && (double)vector.y > 0.3 && (double)vector.y < 0.7)
		{
			arrow.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").SetIdentityObject(null, true);
			arrow.SetActive(false);
			return;
		}
		arrow.SetActive(true);
		arrow.rectTransform().SetLocalPosition(Vector3.zero);
		Vector3 b = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		b.z = target.transform.position.z;
		Vector3 normalized = (target.transform.position - b).normalized;
		arrow.transform.up = normalized;
		this.UpdateTargetIconPosition(target, arrow);
	}

	// Token: 0x06004DFB RID: 19963 RVA: 0x001C4B84 File Offset: 0x001C2D84
	private void UpdateTargetIconPosition(GameObject goTarget, GameObject indicator)
	{
		Vector3 vector = goTarget.transform.position;
		vector = Camera.main.WorldToViewportPoint(vector);
		if (vector.z < 0f)
		{
			vector.x = 1f - vector.x;
			vector.y = 1f - vector.y;
			vector.z = 0f;
			vector = this.Vector3Maxamize(vector);
		}
		vector = Camera.main.ViewportToScreenPoint(vector);
		vector.x = Mathf.Clamp(vector.x, this.edgeInset, (float)Screen.width - this.edgeInset);
		vector.y = Mathf.Clamp(vector.y, this.edgeInset, (float)Screen.height - this.edgeInset);
		indicator.transform.position = vector;
		indicator.GetComponent<HierarchyReferences>().GetReference<Image>("icon").rectTransform.up = Vector3.up;
		indicator.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").transform.up = Vector3.up;
	}

	// Token: 0x06004DFC RID: 19964 RVA: 0x001C4C90 File Offset: 0x001C2E90
	public Vector3 Vector3Maxamize(Vector3 vector)
	{
		float num = 0f;
		num = ((vector.x > num) ? vector.x : num);
		num = ((vector.y > num) ? vector.y : num);
		num = ((vector.z > num) ? vector.z : num);
		return vector / num;
	}

	// Token: 0x040033EA RID: 13290
	public GameObject IndicatorPrefab;

	// Token: 0x040033EB RID: 13291
	public GameObject IndicatorContainer;

	// Token: 0x040033EC RID: 13292
	private Dictionary<GameObject, GameObject> targets = new Dictionary<GameObject, GameObject>();

	// Token: 0x040033ED RID: 13293
	public static OffscreenIndicator Instance;

	// Token: 0x040033EE RID: 13294
	[SerializeField]
	private float edgeInset = 25f;
}
