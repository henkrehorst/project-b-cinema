using Microsoft.VisualStudio.TestTools.UnitTesting;
using bioscoop_app;
using System;
using bioscoop_app.Model;
using System.Security.Cryptography;
using Moq;

namespace CinemaTests
{
    [TestClass]
    public class EqualsTicketCase
    {
        static Ticket randomTicket;
        Mock<Seat> MockedSeatA;
        Mock<Seat> MockedSeatB;
        Mock<ScreenTime> MockedScreenTimeA;
        Mock<ScreenTime> MockedScreenTimeB;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            randomTicket = new Ticket(0, 1, "randtick", null, null, 25);
        }

        [TestInitialize]
        public void Setup()
        {
            MockedSeatA = new Mock<Seat>(-1, -1, -1);
            MockedSeatB = new Mock<Seat>(-1, -1, -1);
            MockedScreenTimeA = new Mock<ScreenTime>(-1, new DateTime(), new DateTime());
            MockedScreenTimeB = new Mock<ScreenTime>(-1, new DateTime(), new DateTime());
            MockedSeatA.Setup(x => x.Equals(MockedSeatB)).Returns(false);
            MockedSeatB.Setup(x => x.Equals(MockedSeatA)).Returns(false);
            MockedScreenTimeA.Setup(x => x.Equals(MockedScreenTimeB)).Returns(false);
            MockedScreenTimeB.Setup(x => x.Equals(MockedScreenTimeA)).Returns(false);
        }

        [TestMethod]
        public void Other_is_null()
        {
            //arrange
            bool expected = false;
            Ticket other = null;

            //act
            bool actual = randomTicket.Equals(other);

            //assert
            Assert.AreEqual(expected, actual, "Equals invoked with null should return false.");
        }

        [TestMethod]
        public void Other_refeq_this()
        {
            //arrange
            bool expected = true;

            //act
            bool actual = randomTicket.Equals(randomTicket);

            //assert
            Assert.AreEqual(expected, actual, "An instance is equal to itself.");
        }

        [TestMethod]
        public void Other_type_eq_false()
        {
            //arrange
            bool expected = false;
            Product other = new Product(0, 10, "stub");

            //act
            bool actual = randomTicket.Equals(other);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Price_not_eq()
        {
            //arrange
            bool expected = false;
            Ticket one = new Ticket(0, 1, "", null, null, 10);
            Ticket two = new Ticket(0, 0.9, "", null, null, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Name_not_eq()
        {
            //arrange
            bool expected = false;
            Ticket one = new Ticket(0, 1, "John", null, null, 10);
            Ticket two = new Ticket(0, 1, "Doe", null, null, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void VistorAge_not_eq()
        {
            //arrange
            bool expected = false;
            Ticket one = new Ticket(0, 1, "", null, null, 10);
            Ticket two = new Ticket(0, 1, "", null, null, 11);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ScreenTime_not_eq()
        {
            //arrange
            bool expected = false;
            Ticket one = new Ticket(0, 1, "", null, MockedScreenTimeA.Object, 10);
            Ticket two = new Ticket(0, 1, "", null, MockedScreenTimeB.Object, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Seat_not_eq()
        {
            //arrange
            bool expected = false;
            Ticket one = new Ticket(0, 1, "", MockedSeatA.Object, null, 10);
            Ticket two = new Ticket(0, 1, "", MockedSeatB.Object, null, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Id_not_eq()
        {
            //arrange
            bool expected = false;
            Ticket one = new Ticket(0, 1, "", null, null, 10);
            Ticket two = new Ticket(1, 1, "", null, null, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EqualsTrue()
        {
            //arrange
            bool expected = true;
            Ticket one = new Ticket(0, 1, "", null, null, 10);
            Ticket two = new Ticket(0, 1, "", null, null, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EqualsTrueNoId()
        {
            //arrange
            bool expected = true;
            Ticket one = new Ticket(1, "", null, null, 10);
            Ticket two = new Ticket(1, "", null, null, 10);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
