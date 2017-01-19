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

		// Declares the main game variable responsible for all other game elements.
		private readonly Game game;

		public Wrapper(Game game) {
			this.game = game;
		}

		/// <summary>
		/// Grabs the block info element which is responsible for most other block related elements.
		/// </summary>
		public BlockInfo GetBlockInfo {
			get {
				return game.BlockInfo;
			}
		}

		/// <summary>
		/// Grabs the chat element which is responsible for most other chat related elements.
		/// </summary>
		public Chat GetChat {
			get {
				return game.Chat;
			}
		}

		/// <summary>
		/// Grabs the graphics API interface which is responsible for all graphics related elements.
		/// </summary>
		public IGraphicsApi GetIGraphicsAPI {
			get {
				return game.Graphics;
			}
		}

		/// <summary>
		/// Grabs the gui interface which is responsible for most other GUI related elements.
		/// </summary>
		public GuiInterface GetIGuiInterface {
			get {
				return game.Gui;
			}
		}

		/// <summary>
		/// Grabs the inventory which is responsible for all inventory related elements.
		/// </summary>
		public Inventory GetInventory {
			get {
				return game.Inventory;
			}
		}

		/// <summary>
		/// Grabs the input handler which is responsible for most other input related elements.
		/// </summary>
		public InputHandler GetInputHandler {
			get {
				return game.InputHandler;
			}
		}

		/// <summary>
		/// Grabs the local player which is responsible for most client sided player related elements.
		/// </summary>
		public LocalPlayer GetLocalPlayer {
			get {
				return game.LocalPlayer;
			}
		}


		/// <summary>
		/// Grabs the all other events which is responsible for all miscellaneous event related elements.
		/// </summary>
		public OtherEvents GetOtherEvents {
			get {
				return game.Events;
			}
		}

		public IServerConnection GetServerConnection {
			get {
				return game.Server;
			}
		}

		/// <summary>
		/// Grabs the single player server which is responsible for most other single player related elements.
		/// </summary>
		public SinglePlayerServer GetSinglePlayerServer {
			get {
				return (SinglePlayerServer)game.Server;
			}
		}

		/// <summary>
		/// Grabs the survival test element which is responsible for most other survival related elements.
		/// </summary>
		public SurvivalTest GetSurvivalTest {
			get {
				return game.LocalPlayer.Survival;
			}
		}

		/// <summary>
		/// Grabs the world element which is responsible for managing most other world related elements.
		/// </summary>
		public World GetWorld {
			get {
				return game.World;
			}
		}

		/// <summary>
		/// Grabs the world events element which is responsible for managing all world event related elements.
		/// </summary>
		public WorldEvents GetWorldEvents {
			get {
				return game.WorldEvents;
			}
		}
	}
}
