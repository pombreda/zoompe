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
        public static implicit operator RelocationType(ArmRelocationType value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(HitachiSuperHRelocationType value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(PowerPCRelocationType value) { return new RelocationType((short)value); }
        public static implicit operator RelocationType(Amd64 value) { return new RelocationType((short)value); }


        public static implicit operator Amd64(RelocationType value) { return (Amd64)value.value; }
        public static implicit operator ArmRelocationType(RelocationType value) { return (ArmRelocationType)value.value; }
        public static implicit operator HitachiSuperHRelocationType(RelocationType value) { return (HitachiSuperHRelocationType)value.value; }
        public static implicit operator PowerPCRelocationType(RelocationType value) { return (PowerPCRelocationType)value.value; }
        public static implicit operator Amd64(RelocationType value) { return new (Amd64)value.value; }
    }
}