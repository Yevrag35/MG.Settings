using MG.Attributes;
using Microsoft.Win32;
using System;

namespace MG
{
    // This Enum translate incoming object 'types' into RegistryValueKinds
    internal enum RegType : int
    {
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.String })]
        [Type(new Type[1] { typeof(string) })]
        String = 1
        ,
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.DWord })]
        [Type(new Type[3] {
            typeof(int), typeof(bool), typeof(Enum) })]
        DWord = 2
        ,
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.QWord })]
        [Type(new Type[1] { typeof(long) })]
        QWord = 3
        ,
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.Binary })]
        [Type(new Type[1] { typeof(byte[]) })]
        Binary = 4
        ,
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.MultiString })]
        [Type(new Type[1] { typeof(string[]) })]
        MultiString = 5
    }
}
