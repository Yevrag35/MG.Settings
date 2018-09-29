using MG.Attributes;
using Microsoft.Win32;
using System;
using System.Numerics;

namespace MG
{
    // This Enum translate incoming object 'types' into RegistryValueKinds
    internal enum RegType : int
    {
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.String })]
        [Type(new Type[2] { typeof(string), typeof(object) })]
        String = 1
        ,
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.DWord })]
        [Type(new Type[4] {
            typeof(int), typeof(bool), typeof(Enum), typeof(uint) })]
        DWord = 2
        ,
        [Identifier(new RegistryValueKind[1] { RegistryValueKind.QWord })]
        [Type(new Type[4] {
            typeof(long), typeof(BigInteger), typeof(ulong), typeof(double) })]
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
