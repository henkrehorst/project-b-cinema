using bioscoop_app.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTests.Model
{
    [TestClass]
    public class ScreenTimeTest
    {
        [TestMethod]
        public void ReserveSeatTest()
        {
            //Arrange
            var testroom = new bool[2, 2]{ { true, true }, { true, true } };
            var testobj = new ScreenTime(25, 1, new DateTime(), new DateTime(), "testroom", testroom, 4);
            //Act
            testobj.SetSeatAvailability(new Ticket(10, "", 0, 0, 25, 25), false);
            //Assert
            Assert.IsFalse(testobj.availability[0, 0]);
            Assert.AreEqual(testobj.availableTickets, 3);
        }
        
        [TestMethod]
        public void CancelSeatTest()
        {
            //throw new NotImplementedException();
            //Arrange
            var testroom = new bool[2, 2] { { false, true }, { true, true } };
            var testobj = new ScreenTime(25, 1, new DateTime(), new DateTime(), "testroom", testroom, 3);
            //Act
            testobj.SetSeatAvailability(new Ticket(10, "", 0, 0, 25, 25), true);
            //Assert
            Assert.IsTrue(testobj.availability[0, 0]);
            Assert.AreEqual(4, testobj.availableTickets);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "Unavailable seat was inappropriately allowed to be reserved.")]
        public void UnavailableSeatTest()
        {
            //Arrange
            var testroom = new bool[2, 2] { { false, true }, { true, true } };
            var testobj = new ScreenTime(25, 1, new DateTime(), new DateTime(), "testroom", testroom, 3);
            //Act
            testobj.SetSeatAvailability(new Ticket(10, "", 0, 0, 25, 25), false);
        }
    }
}
