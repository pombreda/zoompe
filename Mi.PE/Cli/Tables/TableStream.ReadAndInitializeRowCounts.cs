using System;
using System.Collections.Generic;
using System.Linq;
using Mi.PE.Internal;

namespace Mi.PE.Cli.Tables
{
    partial class TableStream
    {
		ModuleEntry[] ModuleEntries;
		TypeRefEntry[] TypeRefEntries;
		TypeDefEntry[] TypeDefEntries;
		// 3 (0x03) not allocated
		FieldEntry[] FieldEntries;
		// 5 (0x05) not allocated
		MethodDefEntry[] MethodDefEntries;
		// 7 (0x07) not allocated
		ParamEntry[] ParamEntries;
		InterfaceImplEntry[] InterfaceImplEntries;
		MemberRefEntry[] MemberRefEntries;
		ConstantEntry[] ConstantEntries;
		CustomAttributeEntry[] CustomAttributeEntries;
		FieldMarshalEntry[] FieldMarshalEntries;
		DeclSecurityEntry[] DeclSecurityEntries;
		ClassLayoutEntry[] ClassLayoutEntries;
		FieldLayoutEntry[] FieldLayoutEntries;
		StandAloneSigEntry[] StandAloneSigEntries;
		EventMapEntry[] EventMapEntries;
		// 19 (0x13) not allocated
		EventEntry[] EventEntries;
		PropertyMapEntry[] PropertyMapEntries;
		// 22 (0x16) not allocated
		PropertyEntry[] PropertyEntries;
		MethodSemanticsEntry[] MethodSemanticsEntries;
		MethodImplEntry[] MethodImplEntries;
		ModuleRefEntry[] ModuleRefEntries;
		TypeSpecEntry[] TypeSpecEntries;
		ImplMapEntry[] ImplMapEntries;
		FieldRVAEntry[] FieldRVAEntries;
		// 30 (0x1E) not allocated
		// 31 (0x1F) not allocated
		// 32 (0x20) not allocated
		AssemblyProcessorEntry[] AssemblyProcessorEntries;
		AssemblyOSEntry[] AssemblyOSEntries;
		AssemblyRefEntry[] AssemblyRefEntries;
		AssemblyRefProcessorEntry[] AssemblyRefProcessorEntries;
		AssemblyRefOSEntry[] AssemblyRefOSEntries;
		FileEntry[] FileEntries;
		ExportedTypeEntry[] ExportedTypeEntries;
		ManifestResourceEntry[] ManifestResourceEntries;
		NestedClassEntry[] NestedClassEntries;
		GenericParamEntry[] GenericParamEntries;
		MethodSpecEntry[] MethodSpecEntries;
		GenericParamConstraintEntry[] GenericParamConstraintEntries;

		private void ReadAndInitializeRowCounts(BinaryStreamReader reader, ulong validMask)
        {
			if ((validMask & (1 << (int)TableKind.Module)) != 0)
				this.ModuleEntries = new ModuleEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.TypeRef)) != 0)
				this.TypeRefEntries = new TypeRefEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.TypeDef)) != 0)
				this.TypeDefEntries = new TypeDefEntry[reader.ReadUInt32()];
			if ((validMask & (1 << 0x03)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << (int)TableKind.Field)) != 0)
				this.FieldEntries = new FieldEntry[reader.ReadUInt32()];
			if ((validMask & (1 << 0x05)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << (int)TableKind.MethodDef)) != 0)
				this.MethodDefEntries = new MethodDefEntry[reader.ReadUInt32()];
			if ((validMask & (1 << 0x07)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << (int)TableKind.Param)) != 0)
				this.ParamEntries = new ParamEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.InterfaceImpl)) != 0)
				this.InterfaceImplEntries = new InterfaceImplEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.MemberRef)) != 0)
				this.MemberRefEntries = new MemberRefEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.Constant)) != 0)
				this.ConstantEntries = new ConstantEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.CustomAttribute)) != 0)
				this.CustomAttributeEntries = new CustomAttributeEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.FieldMarshal)) != 0)
				this.FieldMarshalEntries = new FieldMarshalEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.DeclSecurity)) != 0)
				this.DeclSecurityEntries = new DeclSecurityEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.ClassLayout)) != 0)
				this.ClassLayoutEntries = new ClassLayoutEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.FieldLayout)) != 0)
				this.FieldLayoutEntries = new FieldLayoutEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.StandAloneSig)) != 0)
				this.StandAloneSigEntries = new StandAloneSigEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.EventMap)) != 0)
				this.EventMapEntries = new EventMapEntry[reader.ReadUInt32()];
			if ((validMask & (1 << 0x13)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << (int)TableKind.Event)) != 0)
				this.EventEntries = new EventEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.PropertyMap)) != 0)
				this.PropertyMapEntries = new PropertyMapEntry[reader.ReadUInt32()];
			if ((validMask & (1 << 0x16)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << (int)TableKind.Property)) != 0)
				this.PropertyEntries = new PropertyEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.MethodSemantics)) != 0)
				this.MethodSemanticsEntries = new MethodSemanticsEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.MethodImpl)) != 0)
				this.MethodImplEntries = new MethodImplEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.ModuleRef)) != 0)
				this.ModuleRefEntries = new ModuleRefEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.TypeSpec)) != 0)
				this.TypeSpecEntries = new TypeSpecEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.ImplMap)) != 0)
				this.ImplMapEntries = new ImplMapEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.FieldRVA)) != 0)
				this.FieldRVAEntries = new FieldRVAEntry[reader.ReadUInt32()];
			if ((validMask & (1 << 0x1E)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << 0x1F)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << 0x20)) != 0)
				throw new BadImageFormatException("Non-standard metadata table 0x"+i.ToString("X2")+".");
			if ((validMask & (1 << (int)TableKind.AssemblyProcessor)) != 0)
				this.AssemblyProcessorEntries = new AssemblyProcessorEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.AssemblyOS)) != 0)
				this.AssemblyOSEntries = new AssemblyOSEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.AssemblyRef)) != 0)
				this.AssemblyRefEntries = new AssemblyRefEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.AssemblyRefProcessor)) != 0)
				this.AssemblyRefProcessorEntries = new AssemblyRefProcessorEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.AssemblyRefOS)) != 0)
				this.AssemblyRefOSEntries = new AssemblyRefOSEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.File)) != 0)
				this.FileEntries = new FileEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.ExportedType)) != 0)
				this.ExportedTypeEntries = new ExportedTypeEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.ManifestResource)) != 0)
				this.ManifestResourceEntries = new ManifestResourceEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.NestedClass)) != 0)
				this.NestedClassEntries = new NestedClassEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.GenericParam)) != 0)
				this.GenericParamEntries = new GenericParamEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.MethodSpec)) != 0)
				this.MethodSpecEntries = new MethodSpecEntry[reader.ReadUInt32()];
			if ((validMask & (1 << (int)TableKind.GenericParamConstraint)) != 0)
				this.GenericParamConstraintEntries = new GenericParamConstraintEntry[reader.ReadUInt32()];

			ulong trailingZeroesMask = ulong.MaxValue << 45;
			if ((validMask & trailingZeroesMask) != 0)
				throw new BadImageFormatException("Non-standard metadata table bits.");
		}
	}
}