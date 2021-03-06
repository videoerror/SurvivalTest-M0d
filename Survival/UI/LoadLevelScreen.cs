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

		private readonly Wrapper wrapper;

		/// <summary>
		/// Responsible for class constructing and used for initialization.
		/// </summary>
		public LoadLevelScreen(Game game) : base(game) {
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the load level screen's GUI elements.
		/// </summary>
		public override void Init() {
			base.Init();

			ChatTextWidget LoadLevelText = new ChatTextWidget(game, regularFont);

			LoadLevelText.Init();

			LoadLevelText.SetText("Load level");

			ChatTextWidget HtmlErrorText = new ChatTextWidget(game, regularFont);

			HtmlErrorText.Init();

			HtmlErrorText.SetText("<html>");

			widgets = new Widget[] {
				LoadLevelText,
				HtmlErrorText,

				MakeTitle(0, 175, "Load file...", LoadLevelHandler),
				MakeTitle(0, 250, "Cancel", CancelHandler),

				null,
				null
			};
		}

		/// <summary>
		/// Responsible for handling the load level title button.
		/// </summary>
		private void LoadLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				OpenFileDialog openLevelFileDialog = new OpenFileDialog();

				openLevelFileDialog.Title = "Selecting file..";
				openLevelFileDialog.Filter = "Minecraft levels(*.dat, *.fcm, *.cw, *.lvl)|*.dat;*.fcm;*.cw;*.lvl";

				DialogResult dialogResult = openLevelFileDialog.ShowDialog();

				if(dialogResult == DialogResult.OK) {
					LoadLevel(openLevelFileDialog.FileName);
				}
			}
		}

		/// <summary>
		/// Responsible for loading the level.
		/// </summary>
		private void LoadLevel(string fileName) {
			IMapFormatImporter mapFormatImporter = null;

			if(fileName.EndsWith(".dat")) {
				mapFormatImporter = new MapDatImporter();
			} else if(fileName.EndsWith(".fcm")) {
				mapFormatImporter = new MapFcm3Importer();
			} else if(fileName.EndsWith(".cw")) {
				mapFormatImporter = new MapCwImporter();
			} else if(fileName.EndsWith(".lvl")) {
				mapFormatImporter = new MapLvlImporter();
			}

			try {
				using(FileStream fileStream = File.OpenRead(fileName)) {
					int width, height, length;

					wrapper.GetWorld.Reset();

					if(wrapper.GetWorld.TextureUrl != null) {
						TexturePack.ExtractDefault(game);

						wrapper.GetWorld.TextureUrl = null;
					}

					wrapper.GetBlockInfo.Reset(game);

					byte[] blocks = mapFormatImporter.Load(fileStream, game, out width, out height, out length);

					wrapper.GetWorld.SetNewMap(blocks, width, height, length);

					wrapper.GetWorldEvents.RaiseOnNewMapLoaded();

					if(game.AllowServerTextures && wrapper.GetWorld.TextureUrl != null) {
						wrapper.GetIServerConnection.RetrieveTexturePack(wrapper.GetWorld.TextureUrl);
					}

					LocationUpdate spawnUpdate = LocationUpdate.MakePosAndOri(wrapper.GetLocalPlayer.Spawn, wrapper.GetLocalPlayer.SpawnYaw, wrapper.GetLocalPlayer.SpawnPitch, false);

					wrapper.GetLocalPlayer.SetLocation(spawnUpdate, false);
				}
			} catch(Exception exception) {
				ErrorHandler.LogError("loading map", exception);

				string file = Path.GetFileName(fileName);

				wrapper.GetChat.Add("&e/client loadmap: Failed to load map \"" + file + "\"");
			}
		}

		/// <summary>
		/// Responsible for handling the cancel title button.
		/// </summary>
		private void CancelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Sets the foremost screen to the death screen.
				wrapper.GetIGuiInterface.SetNewScreen(new DeathScreen(game));
			}
		}

		/// <summary>
		/// Responsible for rendering all the load level screen GUI elements.
		/// </summary>
		public override void Render(double delta) {
			base.Render(delta);

			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, widgets[1].Y - (widgets[2].Y - widgets[1].Y) - 4 * widgets[0].Height + widgets[0].Height / 2);
			widgets[1].MoveTo(game.Width / 2 - widgets[1].Width / 2, game.Height / 2 - widgets[1].Height / 2);
		}

		/// <summary>
		/// Responsible for handling when a key is down.
		/// </summary>
		public override bool HandlesKeyDown(Key key) {
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new LoadLevelScreen(game));
		}
	}
}
