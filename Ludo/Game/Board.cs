using Ludo.Enum;
using Ludo.interfaceX;

namespace Ludo.Game
{
    public class Board : IBoard
    {
        public int[,] Grid { get; } = new int[15, 15]; 
        private Dictionary<Position, ZoneType> _predefinedZones;
        public Board()
        {
            _predefinedZones = new Dictionary<Position, ZoneType>();
            InitializeBoardLayout();

        }
        private void InitializeBoardLayout()
        {
            for (int x = 0; x <= 5; x++) for (int y = 0; y <= 5; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;
            for (int x = 9; x <= 14; x++) for (int y = 0; y <= 5; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;
            for (int x = 9; x <= 14; x++) for (int y = 9; y <= 14; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;
            for (int x = 0; x <= 5; x++) for (int y = 9; y <= 14; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;

            _predefinedZones[new Position(7, 7)] = ZoneType.HomePoint;

            for (int y = 0; y < 15; y++)
            {
                if (!_predefinedZones.ContainsKey(new Position(6, y)) || _predefinedZones[new Position(6, y)] == ZoneType.Empty)
                    _predefinedZones[new Position(6, y)] = ZoneType.CommonPath;
                if (!_predefinedZones.ContainsKey(new Position(8, y)) || _predefinedZones[new Position(8, y)] == ZoneType.Empty)
                    _predefinedZones[new Position(8, y)] = ZoneType.CommonPath;
            }
            for (int x = 0; x < 15; x++)
            {
                if (!_predefinedZones.ContainsKey(new Position(x, 6)) || _predefinedZones[new Position(x, 6)] == ZoneType.Empty)
                    _predefinedZones[new Position(x, 6)] = ZoneType.CommonPath;
                if (!_predefinedZones.ContainsKey(new Position(x, 8)) || _predefinedZones[new Position(x, 8)] == ZoneType.Empty)
                    _predefinedZones[new Position(x, 8)] = ZoneType.CommonPath;
            }

            for (int y = 1; y <= 13; y++)
            {
                if (y != 7)
                {
                    if (!_predefinedZones.ContainsKey(new Position(7, y)) || _predefinedZones[new Position(7, y)] == ZoneType.Empty)
                        _predefinedZones[new Position(7, y)] = ZoneType.CommonPath;
                }
            }
            for (int x = 1; x <= 13; x++)
            {
                if (x != 7)
                {
                    if (!_predefinedZones.ContainsKey(new Position(x, 7)) || _predefinedZones[new Position(x, 7)] == ZoneType.Empty)
                        _predefinedZones[new Position(x, 7)] = ZoneType.CommonPath;
                }
            }
            for (int y = 1; y <= 6; y++) _predefinedZones[new Position(7, y)] = ZoneType.HomePath;
            for (int x = 8; x <= 13; x++) _predefinedZones[new Position(x, 7)] = ZoneType.HomePath;
            for (int y = 8; y <= 13; y++) _predefinedZones[new Position(7, y)] = ZoneType.HomePath;
            for (int x = 1; x <= 6; x++) _predefinedZones[new Position(x, 7)] = ZoneType.HomePath;

            _predefinedZones[new Position(6, 1)] = ZoneType.StartPoint;
            _predefinedZones[new Position(13, 6)] = ZoneType.StartPoint;
            _predefinedZones[new Position(8, 13)] = ZoneType.StartPoint;
            _predefinedZones[new Position(1, 8)] = ZoneType.StartPoint;

            _predefinedZones[new Position(2, 6)] = ZoneType.SafeZone;
            _predefinedZones[new Position(6, 12)] = ZoneType.SafeZone;
            _predefinedZones[new Position(12, 8)] = ZoneType.SafeZone;
            _predefinedZones[new Position(8, 2)] = ZoneType.SafeZone;

        }
        public ZoneType GetZoneType(int x, int y)
        {
            if (_predefinedZones.TryGetValue(new Position(x, y), out var zone))
                return zone;
            return ZoneType.Empty;
        }

    }
}

/*(0,0)  (0,1)  (0,2)  (0,3)  (0,4)  (0,5)  (0,6)  (0,7)  (0,8)  (0,9)  (0,10) (0,11) (0,12) (0,13) (0,14) 
(1,0)  (1,1)  (1,2)  (1,3)  (1,4)  (1,5)  (1,6)  (1,7)  (1,8)  (1,9)  (1,10) (1,11) (1,12) (1,13) (1,14)
(2,0)  (2,1)  (2,2)  (2,3)  (2,4)  (2,5)  (2,6)  (2,7)  (2,8)  (2,9)  (2,10) (2,11) (2,12) (2,13) (2,14)
(3,0)  (3,1)  (3,2)  (3,3)  (3,4)  (3,5)  (3,6)  (3,7)  (3,8)  (3,9)  (3,10) (3,11) (3,12) (3,13) (3,14)
(4,0)  (4,1)  (4,2)  (4,3)  (4,4)  (4,5)  (4,6)  (4,7)  (4,8)  (4,9)  (4,10) (4,11) (4,12) (4,13) (4,14)
(5,0)  (5,1)  (5,2)  (5,3)  (5,4)  (5,5)  (5,6)  (5,7)  (5,8)  (5,9)  (5,10) (5,11) (5,12) (5,13) (5,14)
(6,0)  (6,1)  (6,2)  (6,3)  (6,4)  (6,5)  (6,6)  (6,7)  (6,8)  (6,9)  (6,10) (6,11) (6,12) (6,13) (6,14)
(7,0)  (7,1)  (7,2)  (7,3)  (7,4)  (7,5)  (7,6)  (7,7)  (7,8)  (7,9)  (7,10) (7,11) (7,12) (7,13) (7,14)
(8,0)  (8,1)  (8,2)  (8,3)  (8,4)  (8,5)  (8,6)  (8,7)  (8,8)  (8,9)  (8,10) (8,11) (8,12) (8,13) (8,14)
(9,0)  (9,1)  (9,2)  (9,3)  (9,4)  (9,5)  (9,6)  (9,7)  (9,8)  (9,9)  (9,10) (9,11) (9,12) (9,13) (9,14)
(10,0) (10,1) (10,2) (10,3) (10,4) (10,5) (10,6) (10,7) (10,8) (10,9) (10,10)(10,11)(10,12)(10,13)(10,14)
(11,0) (11,1) (11,2) (11,3) (11,4) (11,5) (11,6) (11,7) (11,8) (11,9) (11,10)(11,11)(11,12)(11,13)(11,14)
(12,0) (12,1) (12,2) (12,3) (12,4) (12,5) (12,6) (12,7) (12,8) (12,9) (12,10)(12,11)(12,12)(12,13)(12,14)
(13,0) (13,1) (13,2) (13,3) (13,4) (13,5) (13,6) (13,7) (13,8) (13,9) (13,10)(13,11)(13,12)(13,13)(13,14)
(14,0) (14,1) (14,2) (14,3) (14,4) (14,5) (14,6) (14,7) (14,8) (14,9) (14,10)(14,11)(14,12)(14,13)(14,14)*/

