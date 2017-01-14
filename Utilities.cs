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
using ClassicalSharp.Gui.Screens;
using OpenTK.Input;

namespace ClassicalSharp.Survival {

	/// <summary>
	/// A static helper class containing miscellaneous helpful elements used for the survival test mod.
	/// </summary>
	internal sealed class Utilities {

		public delegate void Handler();

		/// <summary>
		/// Responsible for disabling the escape key.
		/// </summary>
		public static bool DisableEscapeKey(GuiInterface guiInterface,
		                                    Key key,
											MenuOptionsScreen oldMenuOptionsScreen,
		                                    MenuOptionsScreen newMenuOptionsScreen) {
			if(key == Key.Escape) {
				guiInterface.SetNewScreen(newMenuOptionsScreen);

				oldMenuOptionsScreen.Dispose();

				return true;
			}

			return true;
		}
	}
}
