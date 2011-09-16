﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.PEFormat
{
    public struct ImageTimestamp : IComparable<ImageTimestamp>, IComparable, IEquatable<ImageTimestamp>, IFormattable
    {
        public static readonly DateTime EpochUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); 

        public readonly uint SecondsSinceEpochUTC;

        public ImageTimestamp(uint secondsSinceEpochUTC)
        {
            this.SecondsSinceEpochUTC = secondsSinceEpochUTC;
        }

        public ImageTimestamp(DateTime dateTime)
        {
            long ticksFromEpoch = (dateTime - EpochUTC).Ticks;

            // rounding half-second and more up
            long secondsFromEpoch = (ticksFromEpoch + TimeSpan.TicksPerSecond / 2) / TimeSpan.TicksPerSecond;

            this.SecondsSinceEpochUTC = checked((uint)secondsFromEpoch);
        }

        public DateTime ToDateTime()
        {
            return EpochUTC.AddSeconds(SecondsSinceEpochUTC);
        }

        public override string ToString()
        {
            return ToDateTime().ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToDateTime().ToString(format, formatProvider);
        }

        #region IComparable, IEquatable, [in]equality operators

        int IComparable.CompareTo(object obj)
        {
            return this.SecondsSinceEpochUTC.CompareTo(((ImageTimestamp)obj).SecondsSinceEpochUTC);
        }

        int IComparable<ImageTimestamp>.CompareTo(ImageTimestamp other)
        {
            return this.SecondsSinceEpochUTC.CompareTo(other.SecondsSinceEpochUTC);
        }

        bool IEquatable<ImageTimestamp>.Equals(ImageTimestamp other)
        {
            return this.SecondsSinceEpochUTC == other.SecondsSinceEpochUTC;
        }

        public override bool Equals(object obj)
        {
            return
                obj is ImageTimestamp
                && this.SecondsSinceEpochUTC == ((ImageTimestamp)obj).SecondsSinceEpochUTC;
        }

        public override int GetHashCode()
        {
            return
                this.SecondsSinceEpochUTC.GetHashCode();
        }

        public static bool operator==(ImageTimestamp timestamp1, ImageTimestamp timestamp2)
        {
            return timestamp1.SecondsSinceEpochUTC == timestamp2.SecondsSinceEpochUTC;
        }

        public static bool operator !=(ImageTimestamp timestamp1, ImageTimestamp timestamp2)
        {
            return !(timestamp1 == timestamp2);
        }

        #endregion
    }
}