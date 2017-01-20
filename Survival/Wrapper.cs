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
using ClassicalSharp.Entities;
using ClassicalSharp.Events;
using ClassicalSharp.GraphicsAPI;
using ClassicalSharp.Map;
using ClassicalSharp.Singleplayer;

namespace ClassicalSharp.Survival {

	/// <summary>
	/// A helper class used for grabbing all the elements used by the survival test mod.
	/// </summary>
	internal sealed class Wrapper {

		// Declares the game value.
		private readonly Game game;

		public Wrapper(Game game) {
			// Assigns the game value to the game paramter.
			this.game = game;
		}

		/// <summary>
		/// Responsible for the grabbing of the block info element.
		/// </summary>
		public BlockInfo GetBlockInfo {
			get {
				// Returns the block info element.
				return game.BlockInfo;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the chat element.
		/// </summary>
		public Chat GetChat {
			get {
				// Returns the chat element.
				return game.Chat;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the graphics API interface element.
		/// </summary>
		public IGraphicsApi GetIGraphicsAPI {
			get {
				// Returns the graphics API interface element.
				return game.Graphics;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the GUI interface element.
		/// </summary>
		public GuiInterface GetIGuiInterface {
			get {
				// Returns the GUI interface element.
				return game.Gui;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the input handler element.
		/// </summary>
		public InputHandler GetInputHandler {
			get {
				// Returns the input handler element.
				return game.InputHandler;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the inventory element.
		/// </summary>
		public Inventory GetInventory {
			get {
				// Returns the inventory element.
				return game.Inventory;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the server element.
		/// </summary>
		public IServerConnection GetServerConnection {
			get {
				// Returns the server element.
				return game.Server;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the local player element.
		/// </summary>
		public LocalPlayer GetLocalPlayer {
			get {
				// Returns the local player element.
				return game.LocalPlayer;
			}
		}


		/// <summary>
		/// Responsible for the grabbing of other events element.
		/// </summary>
		public OtherEvents GetOtherEvents {
			get {
				// Returns the other events element.
				return game.Events;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the single player server elementt.
		/// </summary>
		public SinglePlayerServer GetSinglePlayerServer {
			get {
				// Returns the casted single player server to the server element.
				return (SinglePlayerServer)game.Server;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the survival test element.
		/// </summary>
		public SurvivalTest GetSurvivalTest {
			get {
				// Returns the survival test element.
				return game.LocalPlayer.Survival;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the world element.
		/// </summary>
		public World GetWorld {
			get {
				// Returns the world element.
				return game.World;
			}
		}

		/// <summary>
		/// Responsible for the grabbing of the world events element.
		/// </summary>
		public WorldEvents GetWorldEvents {
			get {
				// Returns the world events element.
				return game.WorldEvents;
			}
		}
	}
}
