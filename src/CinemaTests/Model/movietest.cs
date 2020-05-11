using bioscoop_app.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;


namespace CinemaTests.Model

{
    [TestClass]
    class movietest

    {
        private static Movie a;
        private static Movie b;
        private static Movie c;
        [TestInitialize]
        public static void setup()
        {
            a = new Movie(1, "test", "action", 3.0, 105, "image"); 
            b = new Movie(1, "test", "action", 3.0, 105, "image");
            c = new Movie(2, "filmnaam", "avondtuur", 6.0, 85, "moviecover");
        }

        [TestMethod]
        public static void Equalself()
        {
            //arrange 
            bool expected = true;


            //act
            bool actual = a.Equals(a);
            //assert
            Assert.AreEqual(expected, actual);


        }
        [TestMethod]
        public static void Equaltrue()
        {
            //arrange 
            bool expected = true;


            //act
            bool actual = a.Equals(b);
            //assert
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public static void Equalfalse()
        {
            //arrange 
            bool expected = true;


            //act
            bool actual = a.Equals(c);
            //assert
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public static void Equalnull()
        {
            //arrange 
            bool expected = true;


            //act
            bool actual = a.Equals(null);
            //assert
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public static void Equaltypeerror()
        {
            //arrange 
            bool expected = true;
            bool d = true;

            //act
            bool actual = a.Equals(d);
            //assert
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public static void hashcodeself()
        {
            //arrange 
            bool expected = true;


            //act
            bool actual = a.Equals(a);
            //assert
            Assert.AreEqual(expected, actual);


        }
        [TestMethod]
        public static void Hashcodedifferent()
        {
            //arrange 
            bool expected = true;


            //act
            bool actual = a.Equals(b);
            //assert
            Assert.AreEqual(expected, actual);


        }
    }
}
