using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    public partial struct RelocationType
    {
        readonly short value;

        private RelocationType(short value)
        {
            this.value = value;
        }

        public static implicit operator RelocationType(short value)
        {
            return new RelocationType(value);
        }

        public static implicit operator RelocationType(Amd64 value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(Arm value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(HitachiSuperH value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(PowerPC value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(I386 value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(Itanium value) { return new RelocationType((short)value); }


        public static implicit operator Amd64(RelocationType value) { return (Amd64)value.value; }
        public static implicit operator Arm(RelocationType value) { return (Arm)value.value; }
        public static implicit operator HitachiSuperH(RelocationType value) { return (HitachiSuperH)value.value; }
        public static implicit operator PowerPC(RelocationType value) { return (PowerPC)value.value; }
        public static implicit operator I386(RelocationType value) { return (I386)value.value; }
        public static implicit operator Itanium(RelocationType value) { return (Itanium)value.value; }
    }
}