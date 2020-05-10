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

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            randomTicket = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
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
            Product other = new Product(1, 10, "stub", "ticket");

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
            Ticket one = new Ticket(1, 10, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(1, 9, "randtick", 2, 3, 26, 25);

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
            Ticket one = new Ticket(1, 1, "John", 2, 3, 26, 25);
            Ticket two = new Ticket(1, 1, "Doe", 2, 3, 26, 25);

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
            Ticket one = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(1, 1, "randtick", 2, 3, 26, 26);

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
            Ticket one = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(1, 1, "randtick", 2, 3, 27, 25);

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
            Ticket one = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(1, 1, "randtick", 2, 4, 26, 25);

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
            Ticket one = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(2, 1, "randtick", 2, 3, 26, 25);

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
            Ticket one = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(1, 1, "randtick", 2, 3, 26, 25);

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
            Ticket one = new Ticket(1, "randtick", 2, 3, 26, 25);
            Ticket two = new Ticket(1, "randtick", 2, 3, 26, 25);

            //act
            bool actual = one.Equals(two);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class GetHashCodeTests {
        [TestMethod]
        public void EqualInst()
        {
            // Arrange
            bool expected = true;
            DataType a = new Movie(1, "test", "adfa", 5.6, 120, "dasfjlk");
            DataType b = new Movie(1, "test", "adfa", 5.6, 120, "dasfjlk");

            // Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Instances that are equal must return the same hashcode");
        }

        [TestMethod]
        public void SameInst()
        {
            //Arrange
            bool expected = true;
            ScreenTime obj = new ScreenTime(5, new DateTime(), new DateTime(), "auditorium1");

            //Act
            bool actual = obj.GetHashCode() == obj.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Hashcode should be consistent.");
        }

        [TestMethod]
        public void DiffType()
        {
            //Arrange
            bool expected = false;
            ScreenTime a = new ScreenTime(3, new DateTime(), new DateTime(), "auditorium1");
            Movie b = new Movie(6, "saf;ljka", "sdlkfja;", 5.4, 100, "jkl;huoyp");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different object should not produce the same hashcode.");
        }

        [TestMethod]
        public void DiffInst_swap()
        {
            //Arrange
            bool expected = false;
            Movie a = new Movie(34, "jkl;", "asdf", 9.8, 124, "qwert");
            Movie b = new Movie(34, "asdf", "jkl;", 9.8, 124, "qwert");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different instances should not have the same hashcode.");
        }

        [TestMethod]
        public void DiffInst_newId()
        {
            //Arrange
            bool expected = false;
            Movie a = new Movie(34, "jkl;", "asdf", 9.8, 124, "qwert");
            Movie b = new Movie(65, "jkl", "asdf;", 9.8, 124, "qwert");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different instances should not have the same hashcode.");
        }

        [TestMethod]
        public void Polymorph()
        {
            //Arrange
            bool expected = false;
            Product a = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Product b = new Product(12, "podasiuf", "ticket");

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
