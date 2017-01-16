#region LICENCE
/*
Copyright 2017 video_error

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

		// Declares and assigns the mod name.
		public const string ModName = "SurvivalTest M0d";
		// Declares and assigns the mod version.
		public const string ModVersion = "PR0T0TYPE[0]";
		// Declares and assigns the mod author.
		public const string ModAuthor = "video_error";
		// Declares and assigns the mod name with ampersand color codes.
		public const string ModNameAmpersand = "&cSurvival&eTest &cM0d";
		// Declares and assigns the mod name with percent color codes.
		public const string ModNamePercent = "%cSurvival%eTest %cM0d";
		// Declares and assigns the mod version with ampersand color codes.
		public const string ModVersionAmpersand = "&5PR0T0TYPE&8[&d0&8]";
		// Declares and assigns the mod version with percent color codes.
		public const string ModVersionPercent = "%5PR0T0TYPE%8[%d0%8]";
		// Declares and assigns the mod author with ampersand color codes.
		public const string ModAuthorAmpersand = "&fv&ei&bd&ae&do&8_&cerror";
		// Declares and assigns the mod author with percent color codes.
		public const string ModAuthorPercent = "%fv%ei%bd%ae%do%8_%cerror";
		// Declares and assigns the mod name with version.
		public const string ModNameWithVersion = ModName + " - " + ModVersion;
		// Declares and assigns the mod name with version and ampersand color codes.
		public const string ModNameWithVersionAmpersand = ModNameAmpersand + " &8- " + ModVersionAmpersand;
		// Declares and assigns the mod name with version and percent color codes.
		public const string ModNameWithVersionPercent = ModNamePercent + " %8- " + ModVersionPercent;
		// Declares and assigns the full mode name including: mod name, mod version, and mod author.
		public const string FullModName = ModName + " - " + ModVersion + " By: " + ModAuthor;
		// Declares and assigns the full mode name including: mod name, mod version, and mod author with apersand color codes.
		public const string FullModNameAmpersand = ModNameAmpersand + " &8- " + ModVersionAmpersand + " &8By: " + ModAuthorAmpersand;
		// Declares and assigns the full mode name including: mod name, mod version, and mod author with percent color codes.
		public const string FullModNamePercent = ModNamePercent + " %8- " + ModVersionPercent + " %8By: " + ModAuthorPercent;

		// Declares the main game value responsible for all other game elements.
		private readonly Game game;

		// Declares the wrapper value used for grabbing all elements used by the survival test mod.
		private readonly Wrapper wrapper;

		/// <summary>
		/// Called upon class instantiation and used for initialization.
		/// </summary>
		public SurvivalTest(Game game) {
			// Assigns the main game value to the inputed game paramter.
			this.game = game;

			// Assigns the wrapper value by creating a new instance and inputing the needed game element value.
			wrapper = new Wrapper(game);

			// Assigns the old inventory keybinding value to the current inventory keybinding.
			OldInventoryKeybind = wrapper.GetInputHandler.Keys[KeyBind.Inventory];

			// Attaches the method for loading the player's inventory to the on new map loaded event for initialization.
			wrapper.GetWorldEvents.OnNewMapLoaded += LoadInventory;
			wrapper.GetWorldEvents.OnNewMap += Respawn;

			// Attaches the method for rebinding the inventory keybind on game window closing.
			((INativeWindow)game.window).Closed += RebindInventoryKeybind;
		}

		// Declares and assigns the under heart texture for the health bar.
		private TextureRec UnderHeartTexRect = new TextureRec(16/256F, 0/256F, 9/256F, 9/256F);
		// Declares and assigns the heart texture for the health bar.
		private TextureRec HeartTexRect = new TextureRec(52/256F, 0/256F, 9/256F, 9/256F);
		// Declares and assigns the half heart texture for the health bar.
		private TextureRec HalfHeartTexRect = new TextureRec(61/256F, 0/256F, 9/256F, 9/256F);

		/// <summary>
		/// Responsible for rendering the player's health bar ingame.
		/// </summary>
		public void RenderHealthBar(BlockHotbarWidget hotbar) {
			// Declares and assigns the evaluation of the selected block's size.
			int selectedBlockSize = (int)Math.Ceiling(22.5F * 2 * game.GuiHotbarScale);
			// Declares and assigns the evaluation of the hotbar's scale which is used for the scale of the heart textures.
			int scale = (int)(18 * game.GuiHotbarScale);

			// Declares and assigns the offset of the hotbar's scale.
			float offset = 16 * game.GuiHotbarScale;

			// Declares the X value used for the heart textures.
			int xValue = 0;
			// Declares and assigns the Y value's delta used for the heart textures, evaluated using the selected block's size and the scale.
			int yValue = game.Height - selectedBlockSize - scale - 2;
			// Declares and assigns the icon texture used for grabbing the heart textures.
			int iconTex = wrapper.GetIGuiInterface.IconsTex;
			// Using the graphics API, the texture is binded.
			wrapper.GetIGraphicsAPI.BindTexture(iconTex);

			// Responsible for rendering the under heart textures.
			for(int underHeart = 0; underHeart < MaxHealth / 2; underHeart++) {
				// Assigns the X value for the under heart texture using the hotbar's X value,
				// the for loop iteration value, and the offset.
				xValue = hotbar.X + (int)(underHeart * offset);

				// Declares and assigns the under heart texture using the binded icon texture,
				// X value, Y value, scale, and the under heart texture rectangle.
				Texture underHeartTex = new Texture(iconTex, xValue, yValue, scale, scale, UnderHeartTexRect);

				// Tells the graphics API to apply texturing when rasterizing primitives.
				wrapper.GetIGraphicsAPI.Texturing = true;
				// Renders the under heart texture using the graphics API.
				underHeartTex.Render(wrapper.GetIGraphicsAPI);
				// Tells the graphics API we no longer need to apply texturing when rasterizing primitives.
				wrapper.GetIGraphicsAPI.Texturing = false;
			}

			// Declares and assigns the half heart offset used for deciding whether to render an extra half heart or not.
			int halfHeartOffset = 1;

			// Responsible for rendering the half heart textures.
			for(int halfHeart = 0; halfHeart < Health / 2 - halfHeartOffset; halfHeart++) {
				// Assigns the X value for the half heart texture using the hotbar's X value,
				// the for loop iteration value, and the offset.
				xValue = hotbar.X + (int)(halfHeart * offset);

				// Declares and assigns the half heart texture using the binded icon texture,
				// X value, Y value, scale, and the half heart texture rectangle.
				Texture halfHeartTex = new Texture(iconTex, xValue, yValue, scale, scale, HalfHeartTexRect);

				// Tells the graphics API to apply texturing when rasterizing primitives.
				wrapper.GetIGraphicsAPI.Texturing = true;
				// Renders the half heart texture using the graphics API.
				halfHeartTex.Render(wrapper.GetIGraphicsAPI);
				// Tells the graphics API we no longer need to apply texturing when rasterizing primitives.
				wrapper.GetIGraphicsAPI.Texturing = false;

				// Assigns the half heart offset to one if the player's health is at the minimum possible health.
				// Otherwise if the player's health is at the maximum possible health.
				// Otherwise the half heart offset is assigned to negative one.
				// This procedure ensures that an extra half heart isn't rendered.
				if(Health == MinimumHealth) {
					halfHeartOffset = 1;
				} else if(Health == MaxHealth) {
					halfHeartOffset = 0;
				} else {
					halfHeartOffset = -1;
				}
			}

			// Responsible for rendering the heart textures.
			for(int heart = 0; heart < Health / 2; heart++) {
				// Assigns the X value for the heart texture using the hotbar's X value,
				// the for loop iteration value, and the offset.
				xValue = hotbar.X + (int)(heart * offset);

				// Declares and assigns the heart texture using the binded icon texture,
				// X value, Y value, scale, and the half heart texture rectangle.
				Texture heartTex = new Texture(iconTex, xValue, yValue, scale, scale, HeartTexRect);

				// Tells the graphics API to apply texturing when rasterizing primitives.
				wrapper.GetIGraphicsAPI.Texturing = true;
				// Renders the heart texture using the graphics API.
				heartTex.Render(wrapper.GetIGraphicsAPI);
				// Tells the graphics API we no longer need to apply texturing when rasterizing primitives.
				wrapper.GetIGraphicsAPI.Texturing = false;
			}
		}

		// <summary>
		// Responsible for all other main survival test loops and methods.
		// </summary>
		public void SurvivalTestTick(double delta) {
			// Calls the method for managing the UI.
			UiManager();
			// Calls the method for managing the player's inventory.
			InventoryManager();
			// Calls the method for managing the player's health.
			HealthManager();
			// Calls the method for fall damage evaluation.
			CalculateFallDamage();
			// Calls the method for managing the player's score.
			ScoreManager();
		}

		/// <summary>
		/// Responsible for managing the ingame UI.
		/// </summary>
		private void UiManager() {
			// Renders the first line of the HUD with the mod name and version using the CPE status one message.
			game.Chat.Add(ModNameWithVersionAmpersand, MessageType.Status1);
			// Renders the second line of the HUD with the mod author using the CPE status two message.
			game.Chat.Add("&8By: " + ModAuthorAmpersand, MessageType.Status2);
		}

		/// <summary>
		/// Responsible for loading the player's inventory.
		/// </summary>
		private void LoadInventory(object sender, EventArgs eventArgs) {
			// Assigns all inventory slots to air for starting out with nothing, resetting the player's inventory.
			for(int inventorySlot = 0; inventorySlot < wrapper.GetInventory.Hotbar.Length; inventorySlot++) {
				wrapper.GetInventory.Hotbar[inventorySlot] = Block.Air;
			}
		}

		// Declares the old inventory keybinding.
		private Key OldInventoryKeybind;

		/// <summary>
		/// Responsible for managing the player's inventory.
		/// </summary>
		private void InventoryManager() {
			// Assigns the inventory keybinding to an unknown key so it can't be opened, essentially disabling the GUI.
			wrapper.GetInputHandler.Keys[KeyBind.Inventory] = Key.Unknown;
		}

		/// <summary>
		/// Responsible for rebinding the inventory keybind.
		/// </summary>
		private void RebindInventoryKeybind(object sender, EventArgs eventArgs) {
			// Assigns the inventory keybinding to the old inventory keybinding.
			wrapper.GetInputHandler.Keys[KeyBind.Inventory] = OldInventoryKeybind;
		}

		// Declares the player's old fall height.
		private int OldFallHeight;
		// Declares the player's fall height.
		private int FallHeight;
		// Declares the player's fall damage.
		private int FallDamage;

		/// <summary>
		/// Responsible for evaluating the player's fall damage.
		/// </summary>
		// TODO Finish commenting the "CalculateFallDamage" method.
		private void CalculateFallDamage() {
			// Checks if the player is in the air so it can start calculating the fall damage otherwise,
			// reset the old fall height, fall height, and fall damage values.
			if(!wrapper.GetLocalPlayer.onGround) {
				// Asigns the fall height value to zero resetting it for re-evaluation.
				FallHeight = 0;

				// Declares and assigns the fall position to the player's current position.
				Vector3I fallPosition = new Vector3I((int)wrapper.GetLocalPlayer.Position.X, (int)wrapper.GetLocalPlayer.Position.Y, (int)wrapper.GetLocalPlayer.Position.Z);

				// Responsbile for getting the blocks under the player with a range of twenty-five blocks.
				for(int height = 0; height < 25; height++) {
					// Checks if the fall position is an air block so it can subtract one from the fall position value otherwise,
					// the fall height, the old fall height, and the fall damage values are all evaluated.
					if(wrapper.GetWorld.SafeGetBlock(fallPosition) == Block.Air) {
						fallPosition.Y--;
					} else {
						FallHeight = (int)wrapper.GetLocalPlayer.Position.Y - fallPosition.Y - 1;

						if(OldFallHeight < FallHeight) {
							OldFallHeight = FallHeight;
						}

						if(OldFallHeight > 3) {
							FallDamage = Math.Max(0, OldFallHeight - 3);
						}

						if(wrapper.GetWorld.SafeGetBlock(fallPosition) == Block.StillWater ||
						   wrapper.GetWorld.SafeGetBlock(fallPosition) == Block.Water ||
						   wrapper.GetWorld.SafeGetBlock(fallPosition) == Block.StillLava ||
						   wrapper.GetWorld.SafeGetBlock(fallPosition) == Block.Lava) {
							FallDamage = 0;
						}
					}
				}
			} else {
				// Assigns the old fall height to zero resetting it for re-evaluation.
				OldFallHeight = 0;
				// Assigns the fall height to zero resetting it for re-evaluation.
				FallHeight = 0;
				// Assigns the fall damage to zero resetting it for re-evaluation.
				FallDamage = 0;
			}
		}

		// Declares and assigns the player's health.
		public int Health = 20;
		// Declares and assigns the player's maximum possible health.
		private const int MinimumHealth = 0;
		// Declares and assigns the player's maximum possible health.
		private const int MaxHealth = 20;
		// Declares a state of whether the player is dead or alive.
		private bool IsDead;

		/// <summary>
		/// Responsible for resetting the player after death.
		/// </summary>
		public void Respawn(object sender, EventArgs eventArgs) {
			// Assigns the state of whether the player is dead to false because the player is now alive again.
			IsDead = false;

			// Assigns the player's health to the maximum possible health value.
			Health = MaxHealth;

			// Sets the formost screen to the ingame screen.
			wrapper.GetIGuiInterface.SetNewScreen(null);

			// Sets and updates the player's position and orientation to the player's spawn position,
			// the player's spawn yaw, and the player's spawn pitch without a relative or interpolated update.
			wrapper.GetLocalPlayer.SetLocation(LocationUpdate.MakePosAndOri(wrapper.GetLocalPlayer.Spawn, wrapper.GetLocalPlayer.SpawnYaw, wrapper.GetLocalPlayer.SpawnPitch, false), false);
		}

		/// <summary>
		/// Responsible for evaluating and managing the player's health.
		/// </summary>
		private void HealthManager() {
			// Checks if the player is on the ground so it can apply the fall damage value.
			if(wrapper.GetLocalPlayer.onGround) {
				// Applies the fall damage, that was previously calculated, to the player's health by subracting itself minus the fall damage value.
				Health -= FallDamage;
			}

			// Prevents the player's health from becoming too low or too high in the event of a possible stack underflow or overflow and displays the death screen.
			if(Health <= MinimumHealth) {
				// Checks if the player is dead so it can display the death screen.
				if(IsDead) {
					// Sets the formost screen to the death screen.
					wrapper.GetIGuiInterface.SetNewScreen(new DeathScreen(game));

					// Assigns whether the player is dead to true so that the death screen is not set to the formost screen twice or more.
					IsDead = false;
				}

				// Assigns the player's health to the minimum health.
				Health = MinimumHealth;
			} else if(Health >= MaxHealth) {
				// Assigns whether the player is dead to false so when the player's health becomes too low again the death screen can be displayed.
				IsDead = true;

				// Assigns the player's health to the maximum health.
				Health = MaxHealth;
			}
		}

		// Declares the player's score.
		public int Score;

		/// <summary>
		/// Responsible for evaluating and managing the player's score.
		/// </summary>
		private void ScoreManager() {
		}
	}
}
