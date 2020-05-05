using bioscoop_app.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTests.Helper
{
    [TestClass]
    class RoomLayoutServiceTest
    {
        private readonly bool[,] expected = new bool[14, 12]
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

        [TestMethod]
        public void GetInitialAvailabilityTest_auditorium1()
        {
            Assert.AreEqual(expected, RoomLayoutService.GetInitialAvailability("auditorium1"));
        }

        [TestMethod]
        public void GetInitialAvailabilityTest_exception()
        {
            Assert.ThrowsException<ArgumentException>(delegate()
            {
                RoomLayoutService.GetInitialAvailability("this room does not exist");
            });
        }
    }
}
