using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class SelectableController : MonoBehaviour
    {
        [SerializeField] ProgressBar progressBar = null;

        [SerializeField] float checkRadius = 1.0f;

        SelectableObject selectableObject = null;

        bool isActive = false;
        float currentDuration = 0.0f;

        CharacterController characterController = null;
        AnimationController animationController = null;

        void ActivateObject()
        {
            isActive = true;
            currentDuration = selectableObject.duration;

            progressBar.SetVisible(true);
            selectableObject.Activate();
            selectableObject.PlayAnim(animationController);
        }
        void DesactivateObject()
		{
            selectableObject.ApplyEffect();

            isActive = false;
            progressBar.SetVisible(false);
            selectableObject.StopAnim(animationController);
            selectableObject.Desactivate();
		}
        void CancelItem()
		{
            if (!isActive) return;

            isActive = false;
            progressBar.SetVisible(false);
            selectableObject.StopAnim(animationController);
            selectableObject.Desactivate();
        }

        void CheckActivableObject()
		{
            SelectableObject nearObject = null;

            var colliders = Physics.OverlapSphere(transform.position, checkRadius);
            foreach (var collider in colliders)
			{
                var tmp = collider.GetComponent<SelectableObject>();
                if (tmp != null && tmp.IsActivable())
                {
                    float distance = (tmp.transform.position - transform.position).sqrMagnitude;
                    if (distance < checkRadius * checkRadius)
                        if (nearObject == null || distance < (nearObject.transform.position - transform.position).sqrMagnitude)
                            nearObject = tmp;
                }
            }
            
            if (selectableObject != nearObject)
            {
                if (selectableObject != null)
                    selectableObject.Deselect();

                selectableObject = nearObject;

                if (selectableObject != null)
                    selectableObject.Select();
            }
		}
        void ObjectInput()
		{
            if (Input.GetButton("ActiveObject"))
                if (selectableObject != null && selectableObject.IsActivable())
                    ActivateObject();
        }
        void UpdateDuration()
		{
            if (!isActive || selectableObject == null) return;

            if (currentDuration <= 0.0f)
                DesactivateObject();

            currentDuration -= Time.deltaTime;
            progressBar.SetPercent(1 - (currentDuration / selectableObject.duration));
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
