using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200090D RID: 2317
[SerializationConfig(MemberSerialization.OptIn)]
public class EightDirectionUtil
{
	// Token: 0x0600405A RID: 16474 RVA: 0x0016CD1C File Offset: 0x0016AF1C
	public static int GetDirectionIndex(EightDirection direction)
	{
		return (int)direction;
	}

	// Token: 0x0600405B RID: 16475 RVA: 0x0016CD1F File Offset: 0x0016AF1F
	public static EightDirection AngleToDirection(int angle)
	{
		return (EightDirection)Mathf.Floor((float)angle / 45f);
	}

	// Token: 0x0600405C RID: 16476 RVA: 0x0016CD2F File Offset: 0x0016AF2F
	public static Vector3 GetNormal(EightDirection direction)
	{
		return EightDirectionUtil.normals[EightDirectionUtil.GetDirectionIndex(direction)];
	}

	// Token: 0x0600405D RID: 16477 RVA: 0x0016CD41 File Offset: 0x0016AF41
	public static float GetAngle(EightDirection direction)
	{
		return (float)(45 * EightDirectionUtil.GetDirectionIndex(direction));
	}

	// Token: 0x04002802 RID: 10242
	public static readonly Vector3[] normals = new Vector3[]
	{
		Vector3.up,
		(Vector3.up + Vector3.left).normalized,
		Vector3.left,
		(Vector3.down + Vector3.left).normalized,
		Vector3.down,
		(Vector3.down + Vector3.right).normalized,
		Vector3.right,
		(Vector3.up + Vector3.right).normalized
	};
}
