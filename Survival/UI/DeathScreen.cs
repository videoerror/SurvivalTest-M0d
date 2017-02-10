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
using System.Drawing;

using ClassicalSharp.Gui.Screens;
using ClassicalSharp.Gui.Widgets;

using OpenTK.Input;

namespace ClassicalSharp.Survival.UI {

	/// <summary>
	/// Holds all the elements for initializing, handling, and rendering the death screen.
	/// </summary>
	internal sealed class DeathScreen : MenuOptionsScreen {

		private readonly Wrapper wrapper;

		/// <summary>
		/// Responsible for class constructing and used for initialization.
		/// </summary>
		public DeathScreen(Game game) : base(game) {
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the death screen's GUI elements.
		/// </summary>
		public override unsafe void Init() {
			base.Init();

			backCol = new FastColour(128, 0, 0, 128);

			Font GameOverTextFont = new Font(regularFont.FontFamily, 32, regularFont.Style);

			ChatTextWidget GameOverText = new ChatTextWidget(game, GameOverTextFont);

			GameOverText.Init();

			GameOverText.SetText("Game over!");

			ChatTextWidget ScoreText = new ChatTextWidget(game, regularFont);

			ScoreText.Init();

			ScoreText.SetText("Score: &e" + wrapper.GetSurvivalTest.Score);

			if(wrapper.GetIServerConnection.IsSinglePlayer) {
				widgets = new Widget[] {
					GameOverText,
					ScoreText,

					MakeTitle(0, 100, "Generate new level...", GenerateNewLevelHandler),
					MakeTitle(0, 150, "Load level...", LoadLevelHandler),

					null,
					null
				};
			} else {
				widgets = new Widget[] {
					GameOverText,
					ScoreText,

					MakeTitle(0, 100, "Respawn", RespawnHandler),

					null,
					null
				};
			}
		}

		/// <summary>
		/// Responsible for handling the generate new level title button.
		/// </summary>
		private void GenerateNewLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetIGuiInterface.SetNewScreen(new GenerateNewLevelScreen(game));
			}
		}

		/// <summary>
		/// Responsible for handling the respawn title button.
		/// </summary>
		private void RespawnHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSurvivalTest.Respawn(null, null);
			}
		}

		/// <summary>
		/// Responsible for handling the load level title button.
		/// </summary>
		private void LoadLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetIGuiInterface.SetNewScreen(new LoadLevelScreen(game));
			}
		}

		/// <summary>
		/// Responsible for rendering all the death screen GUI elements.
		/// </summary>
		public override void Render(double delta) {
			base.Render(delta);

			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, widgets[1].Y - (widgets[2].Y - widgets[1].Y));
			widgets[1].MoveTo(game.Width / 2 - widgets[1].Width / 2, game.Height / 2 - widgets[1].Height / 2);
		}

		/// <summary>
		/// Responsible for handling when a key is down.
		/// </summary>
		public override bool HandlesKeyDown(Key key) {
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new DeathScreen(game));
		}
	}
}
