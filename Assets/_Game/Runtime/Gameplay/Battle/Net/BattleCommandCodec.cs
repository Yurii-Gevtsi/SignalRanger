using System;
using System.Text;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>
    /// Binary wire format for <see cref="BattleCommand"/>. Explicit rather than
    /// reflective so the layout is visible and stable: a message that changes shape
    /// silently between builds is the hardest kind of network bug to find.
    /// <para>
    /// A command is a few dozen bytes, which is the whole point of sending commands
    /// instead of state - a turn costs a handful of packets.
    /// </para>
    /// </summary>
    public static class BattleCommandCodec
    {
        /// <summary>Bumped whenever the layout changes; a mismatch is refused, not guessed at.</summary>
        public const byte Version = 1;

        private const int HeaderLength = 1 + 1 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 1;

        public static byte[] Encode(in BattleCommand command)
        {
            var unitBytes = Encoding.UTF8.GetBytes(command.ActorUnitId ?? string.Empty);
            if (unitBytes.Length > byte.MaxValue)
            {
                throw new ArgumentException("Unit id is too long for the wire format.", nameof(command));
            }

            var buffer = new byte[HeaderLength + unitBytes.Length];
            var offset = 0;
            buffer[offset++] = Version;
            buffer[offset++] = (byte)command.Kind;
            WriteInt(buffer, ref offset, command.TurnNumber);
            WriteInt(buffer, ref offset, command.Sequence);
            WriteInt(buffer, ref offset, command.Value);
            WriteFloat(buffer, ref offset, command.Origin.x);
            WriteFloat(buffer, ref offset, command.Origin.y);
            WriteFloat(buffer, ref offset, command.Direction.x);
            WriteFloat(buffer, ref offset, command.Direction.y);
            WriteFloat(buffer, ref offset, command.Power);
            buffer[offset++] = (byte)unitBytes.Length;
            Buffer.BlockCopy(unitBytes, 0, buffer, offset, unitBytes.Length);
            return buffer;
        }

        public static bool TryDecode(byte[] payload, out BattleCommand command)
        {
            command = default;
            if (payload == null || payload.Length < HeaderLength || payload[0] != Version)
            {
                return false;
            }

            var offset = 1;
            var kind = (BattleCommandKind)payload[offset++];
            var turnNumber = ReadInt(payload, ref offset);
            var sequence = ReadInt(payload, ref offset);
            var value = ReadInt(payload, ref offset);
            var originX = ReadFloat(payload, ref offset);
            var originY = ReadFloat(payload, ref offset);
            var directionX = ReadFloat(payload, ref offset);
            var directionY = ReadFloat(payload, ref offset);
            var power = ReadFloat(payload, ref offset);
            var unitLength = payload[offset++];
            if (offset + unitLength > payload.Length)
            {
                return false;
            }

            command = new BattleCommand(
                turnNumber,
                sequence,
                Encoding.UTF8.GetString(payload, offset, unitLength),
                kind,
                value,
                new Vector2(originX, originY),
                new Vector2(directionX, directionY),
                power);
            return true;
        }

        internal static void WriteInt(byte[] buffer, ref int offset, int value)
        {
            buffer[offset++] = (byte)value;
            buffer[offset++] = (byte)(value >> 8);
            buffer[offset++] = (byte)(value >> 16);
            buffer[offset++] = (byte)(value >> 24);
        }

        internal static int ReadInt(byte[] buffer, ref int offset)
        {
            var value = buffer[offset]
                        | (buffer[offset + 1] << 8)
                        | (buffer[offset + 2] << 16)
                        | (buffer[offset + 3] << 24);
            offset += 4;
            return value;
        }

        /// <summary>
        /// Floats travel as their exact bit pattern, little-endian, so a value that
        /// leaves one client arrives byte-identical at the other. Every target we ship
        /// to is little-endian; writing the bytes ourselves keeps that explicit rather
        /// than depending on the host's order.
        /// </summary>
        internal static void WriteFloat(byte[] buffer, ref int offset, float value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            Buffer.BlockCopy(bytes, 0, buffer, offset, 4);
            offset += 4;
        }

        internal static float ReadFloat(byte[] buffer, ref int offset)
        {
            var bytes = new byte[4];
            Buffer.BlockCopy(buffer, offset, bytes, 0, 4);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            offset += 4;
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
