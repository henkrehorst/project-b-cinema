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
            ScreenTime obj = new ScreenTime(5, new DateTime(), new DateTime(), "auditorium1");

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
            ScreenTime a = new ScreenTime(3, new DateTime(), new DateTime(), "auditorium1");
            Seat b = new Seat(6, 7, 8);

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different object should not produce the same hashcode.");
        }

        [TestMethod]
        public void diffInst_swap()
        {
            //Arrange
            bool expected = false;
            Movie a = new Movie(34, "jkl;", "asdf", 9.8, "Samenvatting", 124, "qwert",new []{1,1});
            Movie b = new Movie(65, "asdf", "jkl;", 9.8, "Samenvatting", 124, "qwert",new []{1,1});

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different instances should not have the same hashcode.");
        }

        [TestMethod]
        public void diffInst_newId()
        {
            //Arrange
            bool expected = false;
            Movie a = new Movie(34, "jkl;", "asdf", 9.8, "Samenvatting", 124, "qwert",new []{1,1});
            Movie b = new Movie(65, "jkl", "asdf;", 9.8, "Samenvatting", 124, "qwert",new []{1,1});

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
            Product a = new Ticket(1, 1, "randtick", 2, 3, 26, 25);
            Product b = new Product(12, "podasiuf", "ticket");

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
            DataType a = new Movie(1, "dasfda", "dlaskd;hfu", 3.7, "Samenvatting", 177, "dasdfioyywer",new []{1,1});
            DataType b = new Movie("dasfda", "dlaskd;hfu", 3.7, "Samenvatting", 177, "dasdfioyywer",new []{1,1});

            //Act
            //int hasha = a.GetHashCode();
            //int hashb = b.GetHashCode();
            bool actual = b.GetHashCode() == 0;//b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //KIJKWIJZER
        [TestMethod]
        public void diffInst_swap2()
        {
            //Arrange
            bool expected = false;
            Kijkwijzer a = new Kijkwijzer(34, "jkl;", "asdf", 9.8, "Samenvatting", 124, "qwert", new[] { 1, 1 });
            Kijkwijzer b = new Kijkwijzer(65, "asdf", "jkl;", 9.8, "Samenvatting", 124, "qwert", new[] { 1, 1 });

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different instances should not have the same hashcode.");
        }

        [TestMethod]
        public void diffInst_newId2()
        {
            //Arrange
            bool expected = false;
            Kijkwijzer a = new Kijkwijzer(34, "jkl;", "asdf", 9.8, "Samenvatting", 124, "qwert", new[] { 1, 1 });
            Kijkwijzer b = new Kijkwijzer(65, "jkl", "asdf;", 9.8, "Samenvatting", 124, "qwert", new[] { 1, 1 });

            //Act
            bool actual = a.GetHashCode() == b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual, "Different instances should not have the same hashcode.");
        }

        [TestMethod]
        public void polymorph2()
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
        [TestMethod]
        public void fieldEqZeroSignature2()
        {
            // A number field equal to zero should change the hashcode
            //Arrange
            bool expected = false;
            DataType a = new Kijkwijzer(1, "dasfda", "dlaskd;hfu", 3.7, "Samenvatting", 177, "dasdfioyywer", new[] { 1, 1 });
            DataType b = new Kijkwijzer("dasfda", "dlaskd;hfu", 3.7, "Samenvatting", 177, "dasdfioyywer", new[] { 1, 1 });

            //Act
            //int hasha = a.GetHashCode();
            //int hashb = b.GetHashCode();
            bool actual = b.GetHashCode() == 0;//b.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }



    }
}
