using Microsoft.VisualStudio.TestTools.UnitTesting;
using bioscoop_app;
using System;
using bioscoop_app.Model;
using System.Security.Cryptography;
using Moq;
using System.Text;

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

    [TestClass]
    public class GetHashCodeTests {
        [TestMethod]
        public void equalInst()
        {
            // Arrange
            bool expected = true;
            DataType a = new Seat(3, 6, 9, 12);
            DataType b = new Seat(3, 6, 9, 12);

            // Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Instances that are equal must return the same hashcode");
        }

        [TestMethod]
        public void sameInst()
        {
            //Arrange
            bool expected = true;
            ScreenTime obj = new ScreenTime(5, new DateTime(), new DateTime());

            //Act
            bool actual = obj.GetHashCode() == obj.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Hashcode should be consistent.");
        }

        [TestMethod]
        public void diffType()
        {
            //Arrange
            bool expected = false;
            ScreenTime a = new ScreenTime(3, 4, new DateTime(), new DateTime());
            Seat b = new Seat(6, 7, 8);

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different object should not produce the same hashcode.");
        }

        [TestMethod]
        public void diffInst()
        {
            //Arrange
            bool expected = false;
            Movie a = new Movie(34, "jkl;", "asdf", 9.8, 124, "qwert");
            Movie b = new Movie(65, "asdf", "jkl;", 9.8, 124, "qwert");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different instances should not have the same hashcode.");
        }

        [TestMethod]
        public void polymorph()
        {
            //Arrange
            bool expected = false;
            Product a = new Ticket(54, 1.4, "kalskdjfi", null, null, 37);
            Product b = new Product(12, "podasiuf");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void fieldEqZeroSignature()
        {
            // A number field equal to zero should change the hashcode
            //Arrange
            bool expected = false;
            DataType a = new Movie(0, "dasfda", "dlaskd;hfu", 3.7, 177, "dasdfioyywer");
            DataType b = new Movie("dasfda", "dlaskd;hfu", 3.7, 177, "dasdfioyywer");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
