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
	/// A class responsible for managing all the elements of the generate new level screen.
	/// </summary>
	internal sealed class GenerateNewLevelScreen : MenuOptionsScreen {

		// Declares the wrapper value used for grabbing all elements used by the survival test mod.
		private readonly Wrapper wrapper;

		public GenerateNewLevelScreen(Game game) : base(game) {
			// Assigns the wrapper value by creating a new instance and inputing the needed game element.
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the generate new level screen.
		/// </summary>
		public override void Init() {
			// Initializes the base extended and inherited class.
			base.Init();

			// Creates a new instance of text widget for the generate new level text using the game value
			// and the regular font value.
			ChatTextWidget GenerateNewLevelText = new ChatTextWidget(game, regularFont);

			// Initializes the generate level text value.
			GenerateNewLevelText.Init();

			// Sets the text for the generate level text value
			GenerateNewLevelText.SetText("Generate new level");

			// Initializes all the widgets through an array.
			widgets = new Widget[] {
				// Adds the new level text to the widgets array.
				GenerateNewLevelText,

				// Creates and adds the small level generation title button widget.
				MakeTitle(0, -125, "Small", SmallLevelHandler),
				// Creates and adds the large level generation title button widget.
				MakeTitle(0, -75, "Large", LargeLevelHandler),
				// Creates and adds the huge level generation title button widget.
				MakeTitle(0, -25, "Huge", HugeLevelHandler),
				// Creates and adds the cancel title button widget.
				MakeTitle(0, 125, "Cancel", CancelHandler),

				null, null
			};
		}

		/// <summary>
		/// Responsible for handling the small level generation title button widget.
		/// </summary>
		private void SmallLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSinglePlayerServer.GenMap(128, 64, 128, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the large level generation title button widget.
		/// </summary>
		private void LargeLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSinglePlayerServer.GenMap(256, 64, 256, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the huge level generation title button widget.
		/// </summary>
		private void HugeLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSinglePlayerServer.GenMap(512, 64, 512, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the cancel title button widget.
		/// </summary>
		private void CancelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetIGuiInterface.SetNewScreen(new DeathScreen(game));
			}
		}

		/// <summary>
		/// Responsible for rendering the elements of the new level generation screen.
		/// </summary>
		public override void Render(double delta) {
			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, (widgets[1].Y - widgets[0].Height) - widgets[1].Height / 2);

			base.Render(delta);
		}

		/// <summary>
		/// Responsible for handling what happens when a key is down.
		/// </summary>
		public override bool HandlesKeyDown(Key key) {
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new GenerateNewLevelScreen(game));
		}
	}
}
