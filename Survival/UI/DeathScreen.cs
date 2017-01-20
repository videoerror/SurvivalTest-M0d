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

		// Declares the wrapper value.
		private readonly Wrapper wrapper;

		public DeathScreen(Game game) : base(game) {
			// Assigns the wrapper value to a new wrapper by inputting the needed game value.
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the death screen's menu options screen elements.
		/// </summary>
		public override unsafe void Init() {
			// Initializes the base class's elements. This is required since it was overrided with this method.
			base.Init();

			// Assigns the death screen's back color to a new fast colour by inputting the needed red value as 128,
			// the green value as zero, the blue value as zero, and alpha value as 128.
			backCol = new FastColour(128, 0, 0, 128);

			// Declares and assigns the game over text's font to a new font by inputting the needed regular font's font family value,
			// font size value as thirty-two, and the regular font's style value.
			Font GameOverTextFont = new Font(regularFont.FontFamily, 32, regularFont.Style);

			// Declares and assigns the game over text to a new chat text widget by inputting the needed game value
			// and game over text font value.
			ChatTextWidget GameOverText = new ChatTextWidget(game, GameOverTextFont);

			// Initializes the game over text value.
			GameOverText.Init();

			// Sets the text of the game over text's value by inputting the needed text as "Game over!".
			GameOverText.SetText("Game over!");

			// Declares and assigns the score text to a new chat text widget by inputting the needed game value
			// and regular font value.
			ChatTextWidget ScoreText = new ChatTextWidget(game, regularFont);

			// Initializes the score text value.
			ScoreText.Init();

			// Sets the text of the score text's value by inputting the needed text as "Score: &e" plus
			// the player's score value.
			ScoreText.SetText("Score: &e" + wrapper.GetSurvivalTest.Score);

			// Detects whether the game is running in single player mode or not.
			if(wrapper.GetServerConnection.IsSinglePlayer) {
				// Assigns the widgets array value to a new widgets array.
				widgets = new Widget[] {
					// Adds the game over text.
					GameOverText,
					// Adds the score text.
					ScoreText,

					// Adds the generate new level title by inputting the needed direction value as zero,
					// the Y value as one-hundred, the text as "Generate new level...", and the generate new level handler.
					MakeTitle(0, 100, "Generate new level...", GenerateNewLevelHandler),
					// Adds the load level title by inputting the needed direction value as zero,
					// the Y value as one-hundred, the text as "Load level...", and the load level handler.
					MakeTitle(0, 150, "Load level...", LoadLevelHandler),

					// Adds the last two values as null.
					null,
					null
				};
			} else {
				// Assigns the widgets array value to a new widgets array.
				widgets = new Widget[] {
					// Adds the game over text.
					GameOverText,
					// Adds the score text.
					ScoreText,

					// Adds the load level title by inputting the needed direction value as zero,
					// the Y value as one-hundred, the text as "respawn", and the load level handler.
					MakeTitle(0, 100, "Respawn", RespawnHandler),

					// Adds the last two values as null.
					null,
					null
				};
			}
		}

		/// <summary>
		/// Responsible for handling the generate new level button.
		/// </summary>
		private void GenerateNewLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Sets the foremost screen to the generate new level screen.
				wrapper.GetIGuiInterface.SetNewScreen(new GenerateNewLevelScreen(game));
			}
		}

		/// <summary>
		/// Responsible for handling the respawn button.
		/// </summary>
		private void RespawnHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Respawns the player inputting the needed sender as null and the event arguments as null
				// because it is not being raised as an event.
				wrapper.GetSurvivalTest.Respawn(null, null);
			}
		}

		/// <summary>
		/// Responsible for handling the load level button.
		/// </summary>
		private void LoadLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Sets the foremost screen to the load level screen.
				wrapper.GetIGuiInterface.SetNewScreen(new LoadLevelScreen(game));
			}
		}

		/// <summary>
		/// Responsible for rendering all the death screen elements.
		/// </summary>
		public override void Render(double delta) {
			// Moves the game over text by inputting the needed X and Y value.
			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, widgets[1].Y - (widgets[2].Y - widgets[1].Y));
			// Moves the score text by inputting the needed X and Y value.
			widgets[1].MoveTo(game.Width / 2 - widgets[1].Width / 2, game.Height / 2 - widgets[1].Height / 2);

			// Renders the base class's elements. This is required since it was overrided with this method.
			base.Render(delta);
		}

		/// <summary>
		/// Responsible for handling when a key is down.
		/// </summary>
		public override bool HandlesKeyDown(Key key) {
			// Disables the ESC key by inputting the needed GUI interface,
			// the key parameter, this instance, and a new instance of the death screen with the needed gam parameter.
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new DeathScreen(game));
		}
	}
}
