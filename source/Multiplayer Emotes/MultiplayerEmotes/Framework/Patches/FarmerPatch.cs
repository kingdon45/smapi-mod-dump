﻿
using System.Reflection;
using MultiplayerEmotes.Extensions;
using Harmony;
using StardewValley;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley.Network;

namespace MultiplayerEmotes.Framework.Patches {

	internal static class FarmerPatch {

		internal class DoEmotePatch : ClassPatch {

			public override MethodInfo Original => AccessTools.Method(typeof(Farmer), nameof(Farmer.doEmote), new Type[] { typeof(int) });
			public override MethodInfo Postfix => AccessTools.Method(this.GetType(), nameof(DoEmotePatch.DoEmote_Postfix));

			private static IReflectionHelper Reflection;

			public DoEmotePatch(IReflectionHelper reflection) {
				Reflection = reflection;
			}

			private static void DoEmote_Postfix(Farmer __instance, int whichEmote) {
				if(Context.IsMultiplayer && __instance is Farmer && __instance.IsLocalPlayer && __instance.IsEmoting) {
					// Traverse.Create(typeof(Game1)).Field("multiplayer").GetValue<Multiplayer>().BroadcastEmote(whichEmote);
					Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue().BroadcastEmote(whichEmote);
				}
			}

		}

	}

}
