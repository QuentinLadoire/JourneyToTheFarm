using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.DataBase;
using JTTF.Behaviour;
using JTTF.Management;

namespace JTTF.Character
{
    public class CraftingController : CustomNetworkBehaviour
    {
        private bool isOpen = false;
        private bool inCrafting = false;
        private RecipeAsset currentRecipe = null;
        private float currentDuration = 0.0f;
        private CraftingProgressBar craftingProgressBar = null;

        public RecipeAsset CurrentRecipe => currentRecipe;

        public Player OwnerPlayer { get; private set; } = null;

        private bool CanCraft(RecipeAsset recipe)
        {
            foreach (var requirement in recipe.requirements)
                if (OwnerPlayer.GetAmountOf(requirement.item) < requirement.amount)
                    return false;

            return true;
        }

        private float GetPercentDuration()
        {
            return 1 - (currentDuration / currentRecipe.craftDuration);
        }

        private void CancelCraft()
        {
            inCrafting = false;
            currentRecipe = null;
            currentDuration = 0.0f;

            craftingProgressBar.SetActive(false);
        }
        private void EndCraft()
        {
            OwnerPlayer.AddItem(currentRecipe.item, currentRecipe.amount);
            foreach (var requirement in currentRecipe.requirements)
                OwnerPlayer.RemoveItem(requirement.item, requirement.amount);

            CanvasManager.GamePanel.RefreshCraftingPanel();

            CancelCraft();
        }

        private void OpenCrafting()
		{
            CanvasManager.GamePanel.OpenCraftingPanel();
            OwnerPlayer.MovementController.DeactiveControl();
            GameManager.cameraController.DeactiveControl();
            GameManager.ActiveCursor();
        }
        private void CloseCrafting()
		{
            CanvasManager.GamePanel.CloseCraftingPanel();
            OwnerPlayer.MovementController.ActiveControl();
            GameManager.cameraController.ActiveControl();
            GameManager.DeactiveCursor();
        }

        private void OpeningInput()
        {
            if (Input.GetButtonDown("Crafting"))
            {
                isOpen = !isOpen;
                if (isOpen)
                    OpenCrafting();
                else
                    CloseCrafting();
            }
        }
        private void UpdateDuration()
        {
            if (!inCrafting) return;

            if (currentDuration <= 0.0f)
                EndCraft();
            else
            {
                currentDuration -= Time.deltaTime;

                craftingProgressBar.SetFillAmount(GetPercentDuration());
            }
        }

        protected override void Awake()
        {
            base.Awake();

            OwnerPlayer = GetComponent<Player>();
        }
        public override void NetworkStart()
        {
            base.NetworkStart();

            if (!(this.IsClient && this.IsLocalPlayer))
            {
                this.enabled = false;
                return;
            }
        }
        protected override void Start()
		{
			base.Start();

            CanvasManager.GamePanel.InitCraftingPanel(this);
            craftingProgressBar = CanvasManager.GamePanel.CraftingProgressBar;
		}
		protected override void Update()
        {
            base.Update();

            OpeningInput();
            UpdateDuration();
        }

        public void StartCraft(RecipeAsset recipe)
        {
            if (recipe == null) return;
            if (inCrafting) CancelCraft();

            if (!CanCraft(recipe)) return;

            inCrafting = true;
            currentRecipe = recipe;
            currentDuration = recipe.craftDuration;

            craftingProgressBar.SetActive(true);
            craftingProgressBar.Init(recipe.name);
        }
    }
}
