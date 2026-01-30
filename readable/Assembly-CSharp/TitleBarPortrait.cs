using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EB1 RID: 3761
[AddComponentMenu("KMonoBehaviour/scripts/TitleBarPortrait")]
public class TitleBarPortrait : KMonoBehaviour
{
	// Token: 0x0600788E RID: 30862 RVA: 0x002E5B47 File Offset: 0x002E3D47
	public void SetSaturation(bool saturated)
	{
		this.ImageObject.GetComponent<Image>().material = (saturated ? this.DefaultMaterial : this.DesatMaterial);
	}

	// Token: 0x0600788F RID: 30863 RVA: 0x002E5B6C File Offset: 0x002E3D6C
	public void SetPortrait(GameObject selectedTarget)
	{
		MinionIdentity component = selectedTarget.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.SetPortrait(component);
			return;
		}
		Building component2 = selectedTarget.GetComponent<Building>();
		if (component2 != null)
		{
			this.SetPortrait(component2.Def.GetUISprite("ui", false));
			return;
		}
		MeshRenderer componentInChildren = selectedTarget.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren)
		{
			this.SetPortrait(Sprite.Create((Texture2D)componentInChildren.material.mainTexture, new Rect(0f, 0f, (float)componentInChildren.material.mainTexture.width, (float)componentInChildren.material.mainTexture.height), new Vector2(0.5f, 0.5f)));
		}
	}

	// Token: 0x06007890 RID: 30864 RVA: 0x002E5C24 File Offset: 0x002E3E24
	public void SetPortrait(Sprite image)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(true);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
		if (image == null)
		{
			this.ClearPortrait();
			return;
		}
		this.ImageObject.GetComponent<Image>().sprite = image;
	}

	// Token: 0x06007891 RID: 30865 RVA: 0x002E5CB8 File Offset: 0x002E3EB8
	private void SetPortrait(MinionIdentity identity)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		CrewPortrait component = base.GetComponent<CrewPortrait>();
		if (component != null)
		{
			component.SetIdentityObject(identity, true);
			return;
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(true);
			CrewPortrait.SetPortraitData(identity, this.AnimControllerObject.GetComponent<KBatchedAnimController>(), true);
		}
	}

	// Token: 0x06007892 RID: 30866 RVA: 0x002E5D54 File Offset: 0x002E3F54
	public void ClearPortrait()
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(false);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
	}

	// Token: 0x04005407 RID: 21511
	public GameObject FaceObject;

	// Token: 0x04005408 RID: 21512
	public GameObject ImageObject;

	// Token: 0x04005409 RID: 21513
	public GameObject PortraitShadow;

	// Token: 0x0400540A RID: 21514
	public GameObject AnimControllerObject;

	// Token: 0x0400540B RID: 21515
	public Material DefaultMaterial;

	// Token: 0x0400540C RID: 21516
	public Material DesatMaterial;
}
