#region LICENCE
/*
Copyright 2016-2017 video_error

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
#endregion

using System;

using ClassicalSharp.Entities;
using ClassicalSharp.Gui.Widgets;
using ClassicalSharp.Survival.UI;

using OpenTK;
using OpenTK.Input;

namespace ClassicalSharp.Survival {

	/// <summary>
	/// A mod for adding the features of Minecraft Classic Survival Test version 0.24_07(http://minecraft.gamepedia.com/Version_history/Classic#0.24_07).
	/// </summary>
	internal sealed class SurvivalTest {

		public const string ModName = "SurvivalTest M0d";
		public const string ModVersion = "PR0T0TYPE[1]";
		public const string ModAuthor = "video_error";
		public const string ModNameAmpersand = "&cSurvival&eTest &cM0d";
		public const string ModNamePercent = "%cSurvival%eTest %cM0d";
		public const string ModVersionAmpersand = "&5PR0T0TYPE&8[&d1&8]";
		public const string ModVersionPercent = "%5PR0T0TYPE%8[%d1%8]";
		public const string ModAuthorAmpersand = "&fv&ei&bd&ae&do&8_&cerror";
		public const string ModAuthorPercent = "%fv%ei%bd%ae%do%8_%cerror";
		public const string ModNameWithVersion = ModName + " - " + ModVersion;
		public const string ModNameWithVersionAmpersand = ModNameAmpersand + " &8- " + ModVersionAmpersand;
		public const string ModNameWithVersionPercent = ModNamePercent + " %8- " + ModVersionPercent;
		public const string FullModName = ModName + " - " + ModVersion + " By: " + ModAuthor;
		public const string FullModNameAmpersand = ModNameAmpersand + " &8- " + ModVersionAmpersand + " &8By: " + ModAuthorAmpersand;
		public const string FullModNamePercent = ModNamePercent + " %8- " + ModVersionPercent + " %8By: " + ModAuthorPercent;

		private readonly Game game;

		private readonly Wrapper wrapper;

		/// <summary>
		/// Responsible for class constructing and used for initialization.
		/// </summary>
		public SurvivalTest(Game game) {
			this.game = game;

			wrapper = new Wrapper(this.game);

			OldInventoryKeybind = wrapper.GetInputHandler.Keys[KeyBind.Inventory];

			wrapper.GetWorldEvents.OnNewMapLoaded += LoadInventory;
			wrapper.GetWorldEvents.OnNewMap += Respawn;

			wrapper.GetINativeWindow.Closed += RebindInventoryKeybind;
			wrapper.GetINativeWindow.Closed += Deconstructor;
		}

		// <summary>
		// Responsible for calling other main rendering functions in a loop.
		// </summary>
		public void RenderTick(BlockHotbarWidget hotbar) {
			RenderHealthBar(hotbar);
			RenderBubbleBar(hotbar);
		}

		private TextureRec UnderHeartTexRect = new TextureRec(16/256F, 0/256F, 9/256F, 9/256F);
		private TextureRec HeartTexRect = new TextureRec(52/256F, 0/256F, 9/256F, 9/256F);
		private TextureRec HalfHeartTexRect = new TextureRec(61/256F, 0/256F, 9/256F, 9/256F);

		/// <summary>
		/// Responsible for rendering the player's health bar ingame.
		/// </summary>
		private void RenderHealthBar(BlockHotbarWidget hotbar) {
			int selectedBlockSize = (int)Math.Ceiling(22.5F * 2 * game.GuiHotbarScale);
			int scale = (int)(18 * game.GuiHotbarScale);

			float offset = 16 * game.GuiHotbarScale;

			int x = 0;
			int y = game.Height - selectedBlockSize - scale - 2;
			int iconTex = wrapper.GetIGuiInterface.IconsTex;

			wrapper.GetIGraphicsAPI.BindTexture(iconTex);

			for(int underHeart = 0; underHeart < MaxHealth / 2; underHeart++) {
				x = hotbar.X + (int)(underHeart * offset);

				Texture underHeartTex = new Texture(iconTex, x, y, scale, scale, UnderHeartTexRect);

				wrapper.GetIGraphicsAPI.Texturing = true;

				underHeartTex.Render(wrapper.GetIGraphicsAPI);

				wrapper.GetIGraphicsAPI.Texturing = false;
			}

			int halfHeartOffset = 1;

			if(Health % 2 == 0) {
				halfHeartOffset = 1;
			} else {
				halfHeartOffset = -1;
			}

			for(int halfHeart = 0; halfHeart < Health / 2 - halfHeartOffset; halfHeart++) {
				x = hotbar.X + (int)(halfHeart * offset);

				Texture halfHeartTex = new Texture(iconTex, x, y, scale, scale, HalfHeartTexRect);

				wrapper.GetIGraphicsAPI.Texturing = true;

				halfHeartTex.Render(wrapper.GetIGraphicsAPI);

				wrapper.GetIGraphicsAPI.Texturing = false;
			}

			for(int heart = 0; heart < Health / 2; heart++) {
				x = hotbar.X + (int)(heart * offset);

				Texture heartTex = new Texture(iconTex, x, y, scale, scale, HeartTexRect);

				wrapper.GetIGraphicsAPI.Texturing = true;

				heartTex.Render(wrapper.GetIGraphicsAPI);

				wrapper.GetIGraphicsAPI.Texturing = false;
			}
		}

		bool CanRenderBubbleBar;

		private TextureRec BubbleTexRec = new TextureRec(16/256F, 18/256F, 9/256F, 9/256F);

		private void RenderBubbleBar(BlockHotbarWidget hotbar) {
			if(CanRenderBubbleBar) {
				int selectedBlockSize = (int)Math.Ceiling(22.5F * 2 * game.GuiHotbarScale);
				int scale = (int)(18 * game.GuiHotbarScale);

				float offset = 16 * game.GuiHotbarScale;

				int x = 0;
				int y = game.Height - selectedBlockSize - scale * 2 - 2;
				int iconTex = wrapper.GetIGuiInterface.IconsTex;

				wrapper.GetIGraphicsAPI.BindTexture(iconTex);

				for(int bubble = 0; bubble < LungCapacity; bubble++) {
					x = hotbar.X + (int)(bubble * offset);

					Texture bubbleTex = new Texture(iconTex, x, y, scale, scale, BubbleTexRec);

					wrapper.GetIGraphicsAPI.Texturing = true;

					bubbleTex.Render(wrapper.GetIGraphicsAPI);

					wrapper.GetIGraphicsAPI.Texturing = false;
				}
			}
		}

		// <summary>
		// Responsible for calling other main functions in a loop.
		// </summary>
		public void Tick() {
			UiManager();
			InventoryManager();
			HealthManager();
			LungCapacityManager();
			CalculateFallDamage();
			CalculateLavaDamage();
			ScoreManager();
		}

		/// <summary>
		/// Responsible for managing the ingame UI.
		/// </summary>
		private void UiManager() {
			game.Chat.Add(ModNameWithVersionAmpersand, MessageType.Status1);
			game.Chat.Add("&8By: " + ModAuthorAmpersand, MessageType.Status2);
		}

		/// <summary>
		/// Responsible for loading the player's inventory.
		/// </summary>
		private void LoadInventory(object sender, EventArgs eventArgs) {
			for(int inventorySlot = 0; inventorySlot < wrapper.GetInventory.Hotbar.Length; inventorySlot++) {
				wrapper.GetInventory.Hotbar[inventorySlot] = Block.Air;
			}
		}

		private Key OldInventoryKeybind;

		/// <summary>
		/// Responsible for managing the player's inventory.
		/// </summary>
		private void InventoryManager() {
			wrapper.GetInputHandler.Keys[KeyBind.Inventory] = Key.Unknown;
		}

		/// <summary>
		/// Responsible for rebinding the inventory keybind.
		/// </summary>
		private void RebindInventoryKeybind(object sender, EventArgs eventArgs) {
			wrapper.GetInputHandler.Keys[KeyBind.Inventory] = OldInventoryKeybind;
		}

		Vector3I FallPosition;
		Vector3I GroundPosition;

		private int FallHeight;
		private int FallDamage;

		/// <summary>
		/// Responsible for evaluating the player's fall damage.
		/// </summary>
		private void CalculateFallDamage() {
			if(!wrapper.GetLocalPlayer.onGround) {
				if(wrapper.GetLocalPlayer.Position.Y > FallPosition.Y) {
					FallPosition = (Vector3I)wrapper.GetLocalPlayer.Position;
				}
			} else {
				GroundPosition = (Vector3I)wrapper.GetLocalPlayer.Position;

				FallHeight = FallPosition.Y - GroundPosition.Y;

				FallDamage = Math.Max(0, FallHeight - 3);

				FallPosition = Vector3I.Zero;

				FallHeight = 0;

				if(wrapper.GetWorld.SafeGetBlock(GroundPosition) == Block.Water ||
				   wrapper.GetWorld.SafeGetBlock(GroundPosition) == Block.StillWater ||
				   wrapper.GetWorld.SafeGetBlock(GroundPosition) == Block.Lava ||
				   wrapper.GetWorld.SafeGetBlock(GroundPosition) == Block.StillLava) {
					FallDamage = 0;
				}

				GroundPosition = Vector3I.Zero;
			}
		}

		public int LungCapacity = 10;
		private const int MinLungCapacity = 0;
		private const int MaxLungCapacity = 10;

		private DateTime LastLungCapacityTime = DateTime.UtcNow;
		private DateTime LastDrownDamageTime = DateTime.UtcNow;

		/// <summary>
		/// Responsible for evaluating the player's lung capacity.
		/// </summary>
		private void LungCapacityManager() {
			if(wrapper.GetLocalPlayer.BlockAtHead == Block.Water ||
			   wrapper.GetLocalPlayer.BlockAtHead == Block.StillWater) {
				CanRenderBubbleBar = true;

				if((DateTime.UtcNow - LastLungCapacityTime).TotalSeconds >= 1.5D) {
					if(LungCapacity > MinLungCapacity) {
						LungCapacity -= 1;

						LastLungCapacityTime = DateTime.UtcNow;
					} else {
						if((DateTime.UtcNow - LastDrownDamageTime).TotalSeconds >= 0.5D) {
							Health -= 2;

							LastDrownDamageTime = DateTime.UtcNow;
						}
					}
				}
			} else {
				CanRenderBubbleBar = false;

				LungCapacity = MaxLungCapacity;

				LastLungCapacityTime = DateTime.UtcNow;
				LastDrownDamageTime = DateTime.UtcNow;
			}
		}

		private DateTime LastLavaDamageTime = DateTime.UtcNow;

		/// <summary>
		/// Responsible for evaluating the player's lava damage.
		/// </summary>
		private void CalculateLavaDamage() {
			Vector3I GroundPosition = (Vector3I)wrapper.GetLocalPlayer.Position;

			if(wrapper.GetLocalPlayer.BlockAtHead == Block.Lava ||
			   wrapper.GetLocalPlayer.BlockAtHead == Block.StillLava ||
			   wrapper.GetWorld.SafeGetBlock(GroundPosition) == Block.Lava ||
			   wrapper.GetWorld.SafeGetBlock(GroundPosition) == Block.StillLava) {
				if((DateTime.UtcNow - LastLavaDamageTime).TotalSeconds >= 0.5D) {
					Health -= 10;

					LastLavaDamageTime = DateTime.UtcNow;
				}
			} else {
				LastLavaDamageTime = DateTime.UtcNow;
			}
		}

		public int Health = 20;
		private const int MinHealth = 0;
		private const int MaxHealth = 20;

		private bool IsDead;

		/// <summary>
		/// Responsible for resetting the player after death.
		/// </summary>
		public void Respawn(object sender, EventArgs eventArgs) {
			IsDead = false;

			Health = MaxHealth;

			wrapper.GetIGuiInterface.SetNewScreen(null);

			wrapper.GetLocalPlayer.SetLocation(LocationUpdate.MakePosAndOri(wrapper.GetLocalPlayer.Spawn,
			                                                                wrapper.GetLocalPlayer.SpawnYaw,
			                                                                wrapper.GetLocalPlayer.SpawnPitch,
			                                                                false), false);
		}

		/// <summary>
		/// Responsible for evaluating and managing the player's health.
		/// </summary>
		private void HealthManager() {
			if(wrapper.GetLocalPlayer.onGround) {
				Health -= FallDamage;
			}

			if(Health <= MinHealth) {
				if(IsDead) {
					wrapper.GetIGuiInterface.SetNewScreen(new DeathScreen(game));

					IsDead = false;
				}

				Health = MinHealth;
			} else if(Health >= MaxHealth) {
				IsDead = true;

				Health = MaxHealth;
			}
		}

		public int Score;

		/// <summary>
		/// Responsible for evaluating and managing the player's score.
		/// </summary>
		private void ScoreManager() {
		}

		/// <summary>
		/// Responsible for class destructing and used for deinitialization.
		/// </summary>
		private void Deconstructor(object sender, EventArgs eventArgs) {
			wrapper.GetWorldEvents.OnNewMapLoaded -= LoadInventory;
			wrapper.GetWorldEvents.OnNewMap -= Respawn;

			wrapper.GetINativeWindow.Closed -= RebindInventoryKeybind;
			wrapper.GetINativeWindow.Closed -= Deconstructor;
		}
	}
}
