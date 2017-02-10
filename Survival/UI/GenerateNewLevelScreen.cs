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

		private readonly Wrapper wrapper;

		/// <summary>
		/// Responsible for class constructing and used for initialization.
		/// </summary>
		public GenerateNewLevelScreen(Game game) : base(game) {
			// Assigns the wrapper value to a new wrapper by using the needed game value.
			wrapper = new Wrapper(game);
		}

		/// <summary>
		/// Responsible for initializing the generate new level screen's GUI elements.
		/// </summary>
		public override void Init() {
			base.Init();

			ChatTextWidget GenerateNewLevelText = new ChatTextWidget(game, regularFont);

			GenerateNewLevelText.Init();

			GenerateNewLevelText.SetText("Generate new level");

			widgets = new Widget[] {
				GenerateNewLevelText,

				MakeTitle(0, -125, "Small", SmallLevelHandler),
				MakeTitle(0, -75, "Large", LargeLevelHandler),
				MakeTitle(0, -25, "Huge", HugeLevelHandler),
				MakeTitle(0, 125, "Cancel", CancelHandler),

				null,
				null
			};
		}

		/// <summary>
		/// Responsible for handling the small level generation title button.
		/// </summary>
		private void SmallLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSinglePlayerServer.GenMap(128, 64, 128, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the large level generation title button.
		/// </summary>
		private void LargeLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSinglePlayerServer.GenMap(256, 64, 256, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the huge level generation title button.
		/// </summary>
		private void HugeLevelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetSinglePlayerServer.GenMap(512, 64, 512, new Random().Next(), new NotchyGenerator());
			}
		}

		/// <summary>
		/// Responsible for handling the cancel title button.
		/// </summary>
		private void CancelHandler(Game game, Widget widget, MouseButton mouseButton) {
			if(mouseButton == MouseButton.Left && game.IsMousePressed(mouseButton)) {
				wrapper.GetIGuiInterface.SetNewScreen(new DeathScreen(game));
			}
		}

		/// <summary>
		/// Responsible for rendering all the new level generation screen GUI elements.
		/// </summary>
		public override void Render(double delta) {
			base.Render(delta);

			widgets[0].MoveTo(game.Width / 2 - widgets[0].Width / 2, (widgets[1].Y - widgets[0].Height) - widgets[1].Height / 2);
		}

		/// <summary>
		/// Responsible for handling when a key is down.
		/// </summary>
		public override bool HandlesKeyDown(Key key) {
			return Utilities.DisableEscapeKey(wrapper.GetIGuiInterface, key, this, new GenerateNewLevelScreen(game));
		}
	}
}
