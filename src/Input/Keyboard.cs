#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2024 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
#endregion

namespace Microsoft.Xna.Framework.Input
{
	/// <summary>
	/// Allows getting keystrokes from keyboard.
	/// </summary>
	public static class Keyboard
	{
		#region Public Static Methods

		[DllImport("user32", CallingConvention=CallingConvention.StdCall)]
		private static extern bool GetKeyboardState([MarshalAs(UnmanagedType.LPArray)] byte[] keystate);

		private static Keys[] key_ranges = {
			// First, Last for each range of vkeys that have a defined value in Keys
			Keys.Back, Keys.Tab,
			Keys.Enter, Keys.Enter,
			Keys.Pause, Keys.Kana,
			Keys.Kanji, Keys.Kanji,
			Keys.Escape, Keys.ImeNoConvert,
			Keys.Space, Keys.Apps,
			Keys.Sleep, Keys.F24,
			Keys.NumLock, Keys.Scroll,
			Keys.LeftShift, Keys.LaunchApplication2,
			Keys.OemSemicolon, Keys.OemTilde,
			Keys.ChatPadGreen, Keys.ChatPadOrange,
			Keys.OemOpenBrackets, Keys.Oem8,
			Keys.OemBackslash, Keys.OemBackslash,
			Keys.ProcessKey, Keys.ProcessKey,
			Keys.OemCopy, Keys.OemEnlW,
			Keys.Attn, Keys.Zoom,
			Keys.Pa1, Keys.OemClear,
		};


		/// <summary>
		/// Returns the current keyboard state.
		/// </summary>
		/// <returns>Current keyboard state.</returns>
		public static KeyboardState GetState()
		{
			/* Wine change: We need to be able to get keyboard events without
			   seeing the keyboard messages, so we don't go through SDL at all */
			// return new KeyboardState(keys);
			byte[] win32_state = new byte[256];
			List<Keys> result = new List<Keys> (0);

			GetKeyboardState(win32_state);

			for (int i = 0; i + 1 < key_ranges.Length; i += 2)
			{
				int first = (int)key_ranges[i];
				int last = (int)key_ranges[i+1];
				for (int j = first; j <= last; j++)
				{
					if ((win32_state[j] & 0x80) == 0x80) {
						result.Add((Keys)j);
					}
				}
			}

			return new KeyboardState(result);
		}

		/// <summary>
		/// Returns the current keyboard state for a given player.
		/// </summary>
		/// <param name="playerIndex">Player index of the keyboard.</param>
		/// <returns>Current keyboard state.</returns>
		public static KeyboardState GetState(PlayerIndex playerIndex)
		{
			return keys;
		}

		#endregion

		#region Public Static FNA Extensions

		public static Keys GetKeyFromScancodeEXT(Keys scancode)
		{
			return FNAPlatform.GetKeyFromScancode(scancode);
		}

		#endregion

		#region Internal Static Variables

		internal static KeyboardState keys = default(KeyboardState);

		#endregion
	}
}
