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

        //[TestMethod]
        public static void equalself()
        {
            throw new NotImplementedException();

        }
        //[TestMethod]
        public static void equaltrue()
        {
            throw new NotImplementedException();

        }
        //[TestMethod]
        public static void equalfalse()
        {
            throw new NotImplementedException();

        }
        //[TestMethod]
        public static void equalnull()
        {
            throw new NotImplementedException();

        }
        //[TestMethod]
        public static void equaltypeerror()
        {
            throw new NotImplementedException();

        }
    }
}
