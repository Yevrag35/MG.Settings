using MG.Attributes;
using System;

namespace MG
{
    // This Enum translate incoming object 'types' into RegistryValueKinds
    internal enum RegType : int
    {
        [MGName("string")]
        [Type(typeof(string))]
        String = 1
        ,
        [MGName("DWord")]
        [MultiType(new Type[] {
            typeof(int), typeof(bool) })]
        DWord = 2
        ,
        [MGName("QWord")]
        [Type(typeof(long))]
        QWord = 3
        ,
        [MGName("Binary")]
        [Type(typeof(byte[]))]
        Binary = 4
        ,
        [MGName("MultiString")]
        [Type(typeof(string[]))]
        MultiString = 5
    }
}
