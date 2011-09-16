using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE.PEFormat
{
    [TestClass]
    public class ImageTimestampTests
    {
        [TestMethod]
        public void Constructor_FromDateTime()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its = new ImageTimestamp(dt);
            Assert.AreEqual(dt.ToString(), its.ToString());
        }

        [TestMethod]
        public void Constructor_FromDateTime_ToDateTime()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its = new ImageTimestamp(dt);
            Assert.AreEqual(dt, its.ToDateTime());
        }

        [TestMethod]
        public void ToStringWithCulture()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its = new ImageTimestamp(dt);
            var ja = System.Globalization.CultureInfo.GetCultureInfo("ja-JP");
            Assert.AreEqual(dt.ToString(null, ja), its.ToString(null, ja), ja.EnglishName);
            var zh = System.Globalization.CultureInfo.GetCultureInfo("zh-CN");
            Assert.AreEqual(dt.ToString(null, zh), its.ToString(null, zh), zh.EnglishName);
        }

        [TestMethod]
        public void CompareTo()
        {
            var dtEarly = new DateTime(2011, 9, 15, 21, 21, 51);
            var itsEarly = new ImageTimestamp(dtEarly);

            var dtLate = new DateTime(2011, 9, 15, 21, 40, 16);
            var itsLate = new ImageTimestamp(dtLate);

            Assert.IsTrue(((IComparable)itsEarly).CompareTo(itsLate) < 0, "itsEarly.CompareTo(itsLate)");
            Assert.IsTrue(((IComparable)itsLate).CompareTo(itsEarly) > 0, "itsLate.CompareTo(itsEarly)");
        }

        [TestMethod]
        public void CompareToGeneric()
        {
            var dtEarly = new DateTime(2011, 9, 15, 21, 21, 51);
            var itsEarly = new ImageTimestamp(dtEarly);

            var dtLate = new DateTime(2011, 9, 15, 21, 40, 16);
            var itsLate = new ImageTimestamp(dtLate);

            Assert.IsTrue(((IComparable<ImageTimestamp>)itsEarly).CompareTo(itsLate) < 0, "itsEarly.CompareTo<>(itsLate)");
            Assert.IsTrue(((IComparable<ImageTimestamp>)itsLate).CompareTo(itsEarly) > 0, "itsLate.CompareTo<>(itsEarly)");
        }

        [TestMethod]
        public void Equals_True()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its1 = new ImageTimestamp(dt);
            var its2 = new ImageTimestamp(dt);

            Assert.IsTrue(its1.Equals(its2), "its1.Equals(its2)");
            Assert.IsTrue(its2.Equals(its1), "its2.Equals(its1)");
        }

        [TestMethod]
        public void Equals_IEquatable_True()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its1 = new ImageTimestamp(dt);
            var its2 = new ImageTimestamp(dt);

            Assert.IsTrue(((IEquatable<ImageTimestamp>)its1).Equals(its2), "its1.Equals<>(its2)");
            Assert.IsTrue(((IEquatable<ImageTimestamp>)its2).Equals(its1), "its2.Equals<>(its1)");
        }

        [TestMethod]
        public void Equals_False()
        {
            var dtEarly = new DateTime(2011, 9, 15, 21, 21, 51);
            var itsEarly = new ImageTimestamp(dtEarly);

            var dtLate = new DateTime(2011, 9, 15, 21, 40, 16);
            var itsLate = new ImageTimestamp(dtLate);

            Assert.IsFalse(itsEarly.Equals(itsLate), "itsEarly.Equals(itsLate)");
            Assert.IsFalse(itsLate.Equals(itsEarly), "itsLate.Equals(itsEarly)");
        }

        [TestMethod]
        public void Equals_IEquatable_False()
        {
            var dtEarly = new DateTime(2011, 9, 15, 21, 21, 51);
            var itsEarly = new ImageTimestamp(dtEarly);

            var dtLate = new DateTime(2011, 9, 15, 21, 40, 16);
            var itsLate = new ImageTimestamp(dtLate);

            Assert.IsFalse(((IEquatable<ImageTimestamp>)itsEarly).Equals(itsLate), "itsEarly.Equals<>(itsLate)");
            Assert.IsFalse(((IEquatable<ImageTimestamp>)itsLate).Equals(itsEarly), "itsLate.Equals<>(itsEarly)");
        }

        [TestMethod]
        public void Equals_WrongType_False()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its1 = new ImageTimestamp(dt);

            Assert.IsFalse(its1.Equals(214), "its1.Equals(214)");
        }

        [TestMethod]
        public void OperatorEquals_True()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its1 = new ImageTimestamp(dt);
            var its2 = new ImageTimestamp(dt);

            Assert.IsTrue(its1==its2, "its1==its2");
            Assert.IsTrue(its2==its1, "its2==its1");
        }

        [TestMethod]
        public void OperatorNotEquals_False()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its1 = new ImageTimestamp(dt);
            var its2 = new ImageTimestamp(dt);

            Assert.IsFalse(its1 != its2, "its1!=its2");
            Assert.IsFalse(its2 != its1, "its2!=its1");
        }

        [TestMethod]
        public void OperatorEquals_False()
        {
            var dtEarly = new DateTime(2011, 9, 15, 21, 21, 51);
            var itsEarly = new ImageTimestamp(dtEarly);

            var dtLate = new DateTime(2011, 9, 15, 21, 40, 16);
            var itsLate = new ImageTimestamp(dtLate);

            Assert.IsFalse(itsEarly==itsLate, "itsEarly==itsLate");
            Assert.IsFalse(itsLate==itsEarly, "itsLate==itsEarly");
        }

        [TestMethod]
        public void OperatorNotEquals_True()
        {
            var dtEarly = new DateTime(2011, 9, 15, 21, 21, 51);
            var itsEarly = new ImageTimestamp(dtEarly);

            var dtLate = new DateTime(2011, 9, 15, 21, 40, 16);
            var itsLate = new ImageTimestamp(dtLate);

            Assert.IsTrue(itsEarly != itsLate, "itsEarly!=itsLate");
            Assert.IsTrue(itsLate != itsEarly, "itsLate!=itsEarly");
        }

        [TestMethod]
        public void GetHashCode_Correct()
        {
            var dt = new DateTime(2011, 9, 15, 21, 21, 51);
            var its = new ImageTimestamp(dt);
            Assert.AreEqual(its.SecondsSinceEpochUTC.GetHashCode(), its.GetHashCode());
        }
    }
}