using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace bioscoop_app.Service
{
    /// <summary>
    /// Service that has the initial availability layouts for each room hardcoded.
    /// </summary>
    public static class RoomLayoutService
    {
        /// <summary>
        /// Retrieve the availability layout for the specified room.
        /// </summary>
        /// <param name="roomName">auditorium1 || auditorium2 || auditorium3</param>
        /// <returns>the requested layout</returns>
        public static bool[,] GetInitialAvailability(string roomName)
        {
            bool[,]? value = (bool[,]?) typeof(RoomLayoutService).GetField(roomName).GetValue(null);
            return (value is object) ? value : throw new ArgumentException("Room not found");
        }

        private static readonly bool[,] auditorium1 = new bool[14, 12]
        {
            {false, false, true, true, true, true, true, true, true, true, false, false },
            {false, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, false },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true },
            {false, true, true, true, true, true, true, true, true, true, true, false },
            {false, false, true, true, true, true, true, true, true, true, false, false },
            {false, false, true, true, true, true, true, true, true, true, false, false }
        };

        private static readonly bool[,] auditorium2 = new bool[19, 18]
        {
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false },
            {false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false },
            {false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false }
        };

        private static readonly bool[,] auditorium3 = new bool[20, 30]
        {
            {false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
            {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            {false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false },
            {false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false },
            {false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false },
            {false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false },
        };
    }
}
