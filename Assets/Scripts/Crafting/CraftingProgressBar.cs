using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class CraftingProgressBar : MonoBehaviour
    {
        [SerializeField] Image gaugeBar = null;
		[SerializeField] Text text = null;

		public void SetVisible(bool value)
		{
			gameObject.SetActive(value);
		}
		public void SetPercent(float percent)
		{
			gaugeBar.fillAmount = Mathf.Clamp01(percent);
		}
		public void SetText(string name)
		{
			text.text = name;
		}

		void OnStartCraft(Recipe recipe)
		{
			SetText(recipe.name);
			SetVisible(true);
			SetPercent(0);
		}
		void OnCancelCraft()
		{
			SetVisible(false);
		}
		void OnEndCraft()
		{
			SetVisible(false);
		}
		void OnCraft(float percent)
		{
			SetPercent(percent);
		}

		public void Init()
		{
			Player.OnStartCraft += OnStartCraft;
			Player.OnCancelCraft += OnCancelCraft;
			Player.OnEndCraft += OnEndCraft;
			Player.OnCraft += OnCraft;
		}
		public void Destroy()
		{
			Player.OnStartCraft -= OnStartCraft;
			Player.OnCancelCraft -= OnCancelCraft;
			Player.OnEndCraft -= OnEndCraft;
			Player.OnCraft -= OnCraft;
		}
	}
}