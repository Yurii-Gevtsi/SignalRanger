using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>
    /// Wire format for the authoritative turn boundary. Same explicit style as
    /// <see cref="BattleCommandCodec"/>: fixed layout, versioned, refused rather
    /// than guessed at when it does not match.
    /// </summary>
    public static class BattleTurnSnapshotCodec
    {
        public const byte Version = 1;

        public static byte[] Encode(BattleTurnSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return Array.Empty<byte>();
            }

            var activeBytes = Encoding.UTF8.GetBytes(snapshot.ActiveUnitId);
            var unitIdBytes = new byte[snapshot.Units.Count][];
            var size = 1 + 4 + 1 + activeBytes.Length + 1 + 1;
            for (var i = 0; i < snapshot.Units.Count; i++)
            {
                unitIdBytes[i] = Encoding.UTF8.GetBytes(snapshot.Units[i].UnitId);
                size += 1 + unitIdBytes[i].Length + 4 + 4 + 4 + 1;
            }

            size += snapshot.Craters.Count * (4 + 4 + 4);

            var buffer = new byte[size];
            var offset = 0;
            buffer[offset++] = Version;
            BattleCommandCodec.WriteInt(buffer, ref offset, snapshot.TurnNumber);
            buffer[offset++] = (byte)activeBytes.Length;
            Buffer.BlockCopy(activeBytes, 0, buffer, offset, activeBytes.Length);
            offset += activeBytes.Length;

            buffer[offset++] = (byte)snapshot.Units.Count;
            for (var i = 0; i < snapshot.Units.Count; i++)
            {
                var unit = snapshot.Units[i];
                buffer[offset++] = (byte)unitIdBytes[i].Length;
                Buffer.BlockCopy(unitIdBytes[i], 0, buffer, offset, unitIdBytes[i].Length);
                offset += unitIdBytes[i].Length;
                BattleCommandCodec.WriteFloat(buffer, ref offset, unit.Position.x);
                BattleCommandCodec.WriteFloat(buffer, ref offset, unit.Position.y);
                BattleCommandCodec.WriteInt(buffer, ref offset, unit.Health);
                buffer[offset++] = (byte)(unit.FacingDirectionX >= 0 ? 1 : 0);
            }

            buffer[offset++] = (byte)snapshot.Craters.Count;
            for (var i = 0; i < snapshot.Craters.Count; i++)
            {
                var crater = snapshot.Craters[i];
                BattleCommandCodec.WriteFloat(buffer, ref offset, crater.Center.x);
                BattleCommandCodec.WriteFloat(buffer, ref offset, crater.Center.y);
                BattleCommandCodec.WriteFloat(buffer, ref offset, crater.Radius);
            }

            return buffer;
        }

        public static bool TryDecode(byte[] payload, out BattleTurnSnapshot snapshot)
        {
            snapshot = null;
            if (payload == null || payload.Length < 7 || payload[0] != Version)
            {
                return false;
            }

            try
            {
                var offset = 1;
                var turnNumber = BattleCommandCodec.ReadInt(payload, ref offset);
                var activeLength = payload[offset++];
                var activeUnitId = Encoding.UTF8.GetString(payload, offset, activeLength);
                offset += activeLength;

                var unitCount = payload[offset++];
                var units = new List<BattleUnitSnapshot>(unitCount);
                for (var i = 0; i < unitCount; i++)
                {
                    var idLength = payload[offset++];
                    var unitId = Encoding.UTF8.GetString(payload, offset, idLength);
                    offset += idLength;
                    var x = BattleCommandCodec.ReadFloat(payload, ref offset);
                    var y = BattleCommandCodec.ReadFloat(payload, ref offset);
                    var health = BattleCommandCodec.ReadInt(payload, ref offset);
                    var facing = payload[offset++] == 1 ? 1 : -1;
                    units.Add(new BattleUnitSnapshot(unitId, new Vector2(x, y), health, facing));
                }

                var craterCount = payload[offset++];
                var craters = new List<BattleCraterSnapshot>(craterCount);
                for (var i = 0; i < craterCount; i++)
                {
                    var x = BattleCommandCodec.ReadFloat(payload, ref offset);
                    var y = BattleCommandCodec.ReadFloat(payload, ref offset);
                    var radius = BattleCommandCodec.ReadFloat(payload, ref offset);
                    craters.Add(new BattleCraterSnapshot(new Vector2(x, y), radius));
                }

                snapshot = new BattleTurnSnapshot(turnNumber, activeUnitId, units, craters);
                return true;
            }
            catch (Exception)
            {
                // A truncated or malformed payload is a transport problem, not a
                // reason to take the battle down with it.
                snapshot = null;
                return false;
            }
        }
    }
}
