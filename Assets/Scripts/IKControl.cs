using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct IKGoalInfo
{
	public AvatarIKGoal type;
	public Transform transform;

	[Space]

	public bool position;
	[Range(0.0f, 1.0f)]
	public float positionWeight;

	[Space]

	public bool rotation;
	[Range(0.0f, 1.0f)]
	public float rotationWeight;

	public void UpdateIK(Animator animator)
	{
		if (position)
		{
			animator.SetIKPosition(type, transform.position);
			animator.SetIKPositionWeight(type, positionWeight);
		}
		if (rotation)
		{
			animator.SetIKRotation(type, transform.rotation);
			animator.SetIKRotationWeight(type, rotationWeight);
		}	
	}
}
[System.Serializable]
struct IKHintInfo
{
	public AvatarIKHint type;
	public Transform transform;

	[Space]

	public bool position;
	[Range(0.0f, 1.0f)]
	public float positionWeight;

	public void UpdateIK(Animator animator)
	{
		if (position)
		{
			animator.SetIKHintPosition(type, transform.position);
			animator.SetIKHintPositionWeight(type, positionWeight);
		}
	}
}

public class IKControl : MonoBehaviour
{
	Animator animator = null;

	[SerializeField] IKGoalInfo[] IKGoal = null;

	[SerializeField] IKHintInfo[] IKHint = null;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	private void OnAnimatorIK(int layerIndex)
	{
		foreach (var goal in IKGoal)
			goal.UpdateIK(animator);

		foreach (var hint in IKHint)
			hint.UpdateIK(animator);
	}
}
