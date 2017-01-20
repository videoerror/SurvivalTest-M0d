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
using ClassicalSharp.Generator;
using ClassicalSharp.Gui.Screens;
using ClassicalSharp.Gui.Widgets;
using ClassicalSharp.Singleplayer;
using OpenTK.Input;

namespace ClassicalSharp.Survival.UI {

	/// <summary>
	/// Holds all the elements for initializing, handling, and rendering the generate new level screen.
	/// </summary>
	internal sealed class GenerateNewLevelScreen : MenuOptionsScreen {

		// Declares the wrapper value.
		private readonly Wrapper wrapper;

		public GenerateNewLevelScreen(Game game) : base(game) {
			// Assigns the wrapper value to a new wrapper by using the needed game value.
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the generate new level screen's elements.
		/// </summary>
		public override void Init() {
			// Initializes the base extended and inherited class's elements.
			// This is required since it was overrided with this method.
			base.Init();

			// Declares and assigns the generate new level text to a new chat text widget by using the needed game value
			// and regular font value.
			ChatTextWidget GenerateNewLevelText = new ChatTextWidget(game, regularFont);

			// Initializes the generate level text value.
			GenerateNewLevelText.Init();

			// Sets the text of the game over text's value by using the needed text as "Generate new level."
			GenerateNewLevelText.SetText("Generate new level");

			// Assigns the widgets array value to a new widgets array.
			widgets = new Widget[] {
				// Adds the generate new level text.
				GenerateNewLevelText,

				// Adds the small title by using the needed direction value as zero,
				// the Y value as negative one-hundred and twenty-five, the text as "Small", and the small level handler.
				MakeTitle(0, -125, "Small", SmallLevelHandler),
				// Adds the large title by using the needed direction value as zero,
				// the Y value as negative seventy-five, the text as "Large", and the large level handler.
				MakeTitle(0, -75, "Large", LargeLevelHandler),
				// Adds the huge title by using the needed direction value as zero,
				// the Y value as negative twenty-five, the text as "Huge", and the huge level handler.
				MakeTitle(0, -25, "Huge", HugeLevelHandler),
				// Adds the canel title by using the needed direction value as zero,
				// the Y value as one-hundred and twenty-five, the text as "Cancel", and the cancel handler.
				MakeTitle(0, 125, "Cancel", CancelHandler),

				// Adds the last two values as null.
				null,
				null
			};
		}

		/// <summary>
		/// Responsible for handling the small level generation title button element.
		/// </summary>
		private void SmallLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Generates a new map by using the needed width value as one-hundred and twenty-eight,
				// the hieght value as sixty-four, the length value as one-hundred and twenty-eight,
				// the new random integer, and the new notchy generator.
				wrapper.GetSinglePlayerServer.GenMap(128, 64, 128, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the large level generation title button element.
		/// </summary>
		private void LargeLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Generates a new map by using the needed width value as two-hundred and fifty-six,
				// the hieght value as sixty-four, the length value as two-hundred and fifty-six,
				// the new random integer, and the new notchy generator.
				wrapper.GetSinglePlayerServer.GenMap(256, 64, 256, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the huge level generation title button element.
		/// </summary>
		private void HugeLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			// Detects whether the mouse button equals the left mouse button and whether the mouse is pressed.
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				// Generates a new map by using the needed width value as five-hundred and twelve,
				// the hieght value as sixty-four, the length value as five-hundred and twelve,
				// the new random integer, and the new notchy generator.
				wrapper.GetSinglePlayerServer.GenMap(512, 64, 512, new Random().Next(), new NotchyGenerator());
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
		/// Responsible for rendering all the new level generation screen elements.
		/// </summary>
		public override void Render(double delta) {
			// Moves the game over text by using the needed new X value as the game width divided by two
			// minus the generate new level text's width divided by two and the new Y value as the small title's Y value
			// minus the generate new level text's height value minus the small title's height divided by two.
			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, (widgets[1].Y - widgets[0].Height) - widgets[1].Height / 2);

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
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new GenerateNewLevelScreen(game));
		}
	}
}
