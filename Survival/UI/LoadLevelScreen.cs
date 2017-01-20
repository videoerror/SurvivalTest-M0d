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
using System.IO;
using System.Windows.Forms;
using ClassicalSharp.Entities;
using ClassicalSharp.Gui.Screens;
using ClassicalSharp.Gui.Widgets;
using ClassicalSharp.Map;
using ClassicalSharp.Textures;
using OpenTK.Input;

namespace ClassicalSharp.Survival.UI {

	/// <summary>
	/// Holds all the elements for initializing, handling, and rendering the load level screen.
	/// </summary>
	internal sealed class LoadLevelScreen : MenuOptionsScreen {

		// Declares the wrapper value.
		private readonly Wrapper wrapper;

		public LoadLevelScreen(Game game) : base(game) {
			// Assigns the wrapper value to a new wrapper by using the needed game value.
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the load level screen's elements.
		/// </summary>
		public override void Init() {
			// Initializes the base extended and inherited class's elements.
			// This is required since it was overrided with this method.
			base.Init();

			// Declares and assigns the load level text to a new chat text widget by using the needed game value
			// and regular font value.
			ChatTextWidget LoadLevelText = new ChatTextWidget(game, regularFont);

			// Initializes the load level text value.
			LoadLevelText.Init();

			// Sets the text of the load level text's value by using the needed text value as "Load level."
			LoadLevelText.SetText("Load level");

			// Declares and assigns the html error text to a new chat text widget by using the needed game value
			// and regular font value.
			ChatTextWidget HtmlErrorText = new ChatTextWidget(game, regularFont);

			// Initializes the html error text value.
			HtmlErrorText.Init();

			// Sets the text of the load level text's value by using the needed text value as "<html>."
			HtmlErrorText.SetText("<html>");

			// Assigns the widgets array value to a new widgets array.
			widgets = new Widget[] {
				// Adds the load level text.
				LoadLevelText,
				// Adds the html error text.
				HtmlErrorText,

				// Adds the load file title by using the needed direction value as zero,
				// the Y value as one-hundred and seventy-five, the text as "Load file...", and the load level handler.
				MakeTitle(0, 175, "Load file...", LoadLevelHandler),
				// Adds the small title by using the needed direction value as zero,
				// the Y value as negative two-hundred and fifty, the text as "Cancel", and the cancel handler.
				MakeTitle(0, 250, "Cancel", CancelHandler),

				// Adds the last two values as null.
				null,
				null
			};
		}

		/// <summary>
		/// Responsible for handling the load level title button element.
		/// </summary>
		private void LoadLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Declares and assigns the open level file dialog to a new open file dialog
				OpenFileDialog openLevelFileDialog = new OpenFileDialog();

				// Assigns the open level file dialog title.
				openLevelFileDialog.Title = "Selecting file..";
				// Assigns the open level file dialog filter to accept .dat, .fcm, and .lvl file types.
				openLevelFileDialog.Filter = "Minecraft levels(*.dat, *.fcm, *.cw, *.lvl)|*.dat;*.fcm;*.cw;*.lvl";

				// Declares and assigns the dialog result to the open level file dialog's show dialog value.
				DialogResult dialogResult = openLevelFileDialog.ShowDialog();

				// Detects whether the dialog result equals OK.
				if(dialogResult == DialogResult.OK) {
					// Load level by using the needed filename as the open file dialog's filename value.
					LoadLevel(openLevelFileDialog.FileName);
				}
			}
		}

		/// <summary>
		/// Responsible for loading the level.
		/// </summary>
		private void LoadLevel(string fileName) {
			// Declares and assigns the map format importer to null.
			IMapFormatImporter mapFormatImporter = null;

			// Detects whether the file name ends with .dat.
			if(fileName.EndsWith(".dat")) {
				// Assigns the map format importer to a new map MinerCPP importer.
				mapFormatImporter = new MapDatImporter();
			// Detects whether the file name ends with .fcm.
			} else if(fileName.EndsWith(".fcm")) {
				// Assigns the map format importer to a new map FCMv3 importer.
				mapFormatImporter = new MapFcm3Importer();
			// Detects whether the file name ends wth .cw.
			} else if(fileName.EndsWith(".cw")) {
				// Assigns the map format importer to a new map ClassicWorld importer.
				mapFormatImporter = new MapCwImporter();
			// Detects whether the file names ends with .lvl.
			} else if(fileName.EndsWith(".lvl")) {
				// Assigns the map format importer to a new map MCSharp importer.
				mapFormatImporter = new MapLvlImporter();
			}

			// Tries the following code.
			try {
				// Uses a declared and assigned file stream value to file's open read value by using the needed file name value.
				using(FileStream fileStream = File.OpenRead(fileName)) {
					// Declares the world's width, height and length values.
					int width, height, length;

					// Resets the world.
					wrapper.GetWorld.Reset();

					//Detects whether the world's texture URL is not equal to null.
					if(wrapper.GetWorld.TextureUrl != null) {
						// Extracts the default texture pack using the needed game value.
						TexturePack.ExtractDefault(game);

						// Assigns the world's texture URL to null.
						wrapper.GetWorld.TextureUrl = null;
					}

					// Resets block info using the needed game value.
					wrapper.GetBlockInfo.Reset(game);

					// Assigns blocks to the mp format importer's load value by using the needed file stream value,
					// the game value, the width value that will be assigned later, the height value that will be assigned later,
					// and the length value that will be assigned later.
					byte[] blocks = mapFormatImporter.Load(fileStream, game, out width, out height, out length);

					// Sets the new map by using the needed blocks value, the width value, the height value, and the length value.
					wrapper.GetWorld.SetNewMap(blocks, width, height, length);

					// Raises the on new map loaded event.
					wrapper.GetWorldEvents.RaiseOnNewMapLoaded();

					// Detects whether server textures are allowed and the texture URL value is not equal to null.
					if(game.AllowServerTextures && wrapper.GetWorld.TextureUrl != null) {
						// Retrives the texture pack using the needed texture URL value.
						wrapper.GetIServerConnection.RetrieveTexturePack(wrapper.GetWorld.TextureUrl);
					}

					// Declares and assigns the spawn update to location update's make position and orientation value by
					// using the needed player's spawn position, the player's spawn yaw, the players spawn pitch, and the relative value as false.
					LocationUpdate spawnUpdate = LocationUpdate.MakePosAndOri(wrapper.GetLocalPlayer.Spawn, wrapper.GetLocalPlayer.SpawnYaw, wrapper.GetLocalPlayer.SpawnPitch, false);

					// Sets the player's location to the spawn update and the interpolate value as false.
					wrapper.GetLocalPlayer.SetLocation(spawnUpdate, false);
				}
			// Catches any exception.
			} catch(Exception exception) {
				// Logs the error by using the location value as "loading map", and the exception value.
				ErrorHandler.LogError("loading map", exception);

				// Declares and assigns the file to the path's get file name value by using the file name value.
				string file = Path.GetFileName(fileName);

				// Adds the text as "&e/client loadmap: Failed to load map \"" + file + "\"" to the chat.
				wrapper.GetChat.Add("&e/client loadmap: Failed to load map \"" + file + "\"");
			}
		}

		/// <summary>
		/// Responsible for handling the cancel title button element.
		/// </summary>
		private void CancelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Sets the foremost screen to the death screen.
				wrapper.GetIGuiInterface.SetNewScreen(new DeathScreen(game));
			}
		}

		/// <summary>
		/// Responsible for rendering all the load level screen elements.
		/// </summary>
		public override void Render(double delta) {
			// Moves the load level text by using the needed new X value as the game width divided by two
			// minus the load level text's width divided by two and the new Y value as the html error text's Y value
			// minus the load file button title element's Y value minus the html error text's Y value
			// minus four times the load level text's height plus the load level text's height divided by two.
			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, widgets[1].Y - (widgets[2].Y - widgets[1].Y) - 4 * widgets[0].Height + widgets[0].Height / 2);
			// Moves the html error text by using the needed new X value as the game width value divided by two
			// minus the html error text's width value divided by two and the new Y value as the game height value divided by two
			// minus the score html error text's height value divided by two.
			widgets[1].MoveTo(game.Width / 2 - widgets[1].Width / 2, game.Height / 2 - widgets[1].Height / 2);

			// Renders the base extended and inherited class's elements.
			// This is required since it was overrided with this method.
			base.Render(delta);
		}

		/// <summary>
		/// Responsible for handling when a key is down.
		/// </summary>
		public override bool HandlesKeyDown(Key key) {
			// Disables the ESC key by using the needed GUI interface,
			// the key parameter, this instance, and a new instance of the generate new level screen with the needed game value.
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new LoadLevelScreen(game));
		}
	}
}
