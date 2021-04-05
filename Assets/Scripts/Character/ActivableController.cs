using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ActivableController : MonoBehaviour
    {
        [SerializeField] ProgressBar progressBar = null;

        [SerializeField] float checkRadius = 1.0f;

        ActivableObject activableObject = null;

        bool isActive = false;
        float currentDuration = 0.0f;

        CharacterController characterController = null;
        AnimationController animationController = null;

        void ActiveObject()
        {
            isActive = true;
            currentDuration = activableObject.duration;

            progressBar.SetVisible(true);
            activableObject.Activate();
            activableObject.PlayAnim(animationController);
        }
        void DesactivateObject()
		{
            activableObject.ApplyEffect();

            isActive = false;
            progressBar.SetVisible(false);
            activableObject.StopAnim(animationController);
            activableObject.Desactivate();
		}
        void CancelItem()
		{
            if (!isActive) return;

            isActive = false;
            progressBar.SetVisible(false);
            activableObject.StopAnim(animationController);
            activableObject.Desactivate();
        }

        void CheckActivableObject()
		{
            ActivableObject nearObject = null;

            var colliders = Physics.OverlapSphere(transform.position, checkRadius);
            foreach (var collider in colliders)
			{
                var tmp = collider.GetComponent<ActivableObject>();
                if (tmp != null)
				{
                    float distance = (tmp.transform.position - transform.position).sqrMagnitude;
                    if (distance < checkRadius * checkRadius)
                        if (nearObject == null || distance < (nearObject.transform.position - transform.position).sqrMagnitude)
                            nearObject = tmp;
				}
            }
            
            if (activableObject != nearObject)
            {
                if (activableObject != null)
                    activableObject.Deselect();

                activableObject = nearObject;

                if (activableObject != null)
                    activableObject.Select();
            }
		}
        void ObjectInput()
		{
            if (Input.GetButton("ActiveObject"))
                if (activableObject != null && activableObject.IsActivable())
                    ActiveObject();
        }
        void UpdateDuration()
		{
            if (!isActive || activableObject == null) return;

            if (currentDuration <= 0.0f)
                DesactivateObject();

            currentDuration -= Time.deltaTime;
            progressBar.SetPercent(1 - (currentDuration / activableObject.duration));
        }

		private void Awake()
		{
            animationController = GetComponent<AnimationController>();
            characterController = GetComponent<CharacterController>();

            characterController.onMoveEnter += CancelItem;
		}
		private void Update()
		{
            CheckActivableObject();

            ObjectInput();

            UpdateDuration();
        }
		private void OnDestroy()
		{
            characterController.onMoveEnter -= CancelItem;
		}
	}
}
