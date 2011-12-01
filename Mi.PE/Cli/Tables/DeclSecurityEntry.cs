﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// All security custom attributes for a given security action on a method, type, or assembly shall be gathered together,
    /// and one System.Security.PermissionSet instance shall be created, stored in the Blob heap, and referenced from the <see cref="TableKind.DeclSecurity"/> table.
    /// [ECMA 22.11]
    /// </summary>
    /// <remarks>
    /// The general flow from a compiler‘s point of view is as follows.
    /// The user specifies a custom attribute through some language-specific syntax that encodes a call to the attribute‘s constructor.
    /// If the attribute‘s type is derived (directly or indirectly) from System.Security.Permissions.SecurityAttribute
    /// then it is a security custom attribute and requires special treatment, as follows
    /// (other custom attributes are handled by simply recording the constructor in the metadata as described in ECMA §22.10).
    /// The attribute object is constructed, and provides a method (CreatePermission)
    /// to convert it into a security permission object (an object derived from System.Security.Permission).
    /// All the permission objects attached to a given metadata item with the same security action are combined together into a System.Security.PermissionSet.
    /// This permission set is converted into a form that is ready to be stored in XML using its ToXML method
    /// to create a System.Security.SecurityElement.
    /// Finally, the XML that is required for the metadata is created using the ToString method on the security element.
    /// </remarks>
    public sealed class DeclSecurityEntry
    {
        public SecurityAction Action;

        /// <summary>
        /// An index into the <see cref="TabeKind.TypeDef"/>, <see cref="TableKind.MethodDef"/>, or <see cref="TableKind.Assembly"/> table;
        /// more precisely, a <see cref="HasDeclSecurity"/> (ECMA §24.2.6) coded index.
        /// </summary>
        public HasDeclSecurity Parent;

        public byte[] PermissionSet;
    }
}