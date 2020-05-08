using bioscoop_app.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CinemaTests.Helper
{
    [TestClass]
    public class RoomLayoutServiceTest
    {
        private static readonly bool[,] expected = new bool[14, 12]
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
            bool[,] actual = RoomLayoutService.GetInitialAvailability("auditorium1");
            var a = expected.GetEnumerator();
            var b = actual.GetEnumerator();
            while (a.MoveNext() && b.MoveNext())
            {
                Assert.AreEqual(a.Current, b.Current);
            };
            Assert.IsFalse(a.MoveNext() || b.MoveNext());
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
