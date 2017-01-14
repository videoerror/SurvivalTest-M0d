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
using System.Drawing;
using ClassicalSharp.Gui.Screens;
using ClassicalSharp.Gui.Widgets;
using OpenTK.Input;

namespace ClassicalSharp.Survival.UI {

	internal sealed class DeathScreen : MenuOptionsScreen {

		// Declares the wrapper variable used for grabbing all elements used by the survival test mod.
		private readonly Wrapper wrapper;

		public DeathScreen(Game game) : base(game) {
			// Assigns the wrapper variable by creating a new instance and inputing the needed game element variable.
			wrapper = new Wrapper(game);
		}

		public override unsafe void Init() {
			base.Init();

			backCol = new FastColour(128, 0, 0, 128);

			TextWidget GameOverText = new TextWidget(game, new Font(regularFont.FontFamily, 32, regularFont.Style));

			GameOverText.Init();

			GameOverText.SetText("Game over!");

			TextWidget ScoreText = new TextWidget(game, regularFont);

			ScoreText.Init();

			ScoreText.SetText("Score: &e" + wrapper.GetSurvivalTest.Score);

			if(wrapper.GetServerConnection.IsSinglePlayer) {
				widgets = new Widget[] {
					GameOverText,
					ScoreText,

					MakeTitle(0, 100, "Generate new level...", GenerateNewLevelHandler),
					MakeTitle(0, 150, "Load level...", LoadLevelHandler),

					null, null
				};
			} else {
				widgets = new Widget[] {
					GameOverText,
					ScoreText,

					MakeTitle(0, 100, "Respawn", RespawnHandler),

					null, null
				};
			}
		}

		private void GenerateNewLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetIGuiInterface.SetNewScreen(new GenerateNewLevelScreen(game));
			}
		}

		private void RespawnHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSurvivalTest.Respawn(null, null);
			}
		}

		private void LoadLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetIGuiInterface.SetNewScreen(new LoadLevelScreen(game));
			}
		}

		public override void Render(double delta) {
			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, widgets[1].Y - (widgets[2].Y - widgets[1].Y));
			widgets[1].MoveTo(game.Width / 2 - widgets[1].Width / 2, game.Height / 2 - widgets[1].Height / 2);

			base.Render(delta);
		}

		public override bool HandlesKeyDown(Key key) {
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new DeathScreen(game));
		}

		public override void Dispose() {
			base.Dispose();
		}
	}
}
