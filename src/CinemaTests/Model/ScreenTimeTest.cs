using bioscoop_app.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTests.Model
{
    [TestClass]
    class ScreenTimeTest
    {
        //[TestMethod]
        public void ReserveSeatTest()
        {
            //Arrange
            var testroom = new bool[2, 2]{ { true, true }, { true, true } };
            var testobj = new ScreenTime(25, 64, new DateTime(), new DateTime(), "testroom", testroom, 4);
            //Act
            //testobj.ReserveSeat(new Ticket())
        }
    }
}
