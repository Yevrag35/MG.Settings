using MG.Attributes;
using Microsoft.Win32;
using System;

namespace MG
{
    // This Enum translate incoming object 'types' into RegistryValueKinds
    public enum RegType : int
    {
        [MGName("String")]
        [Type(typeof(String))]
        String = 1
        ,
        [MGName("DWord")]
        [Type(typeof(int))]
        DWord = 2
        ,
        [MGName("QWord")]
        [Type(typeof(long))]
        QWord = 3
        ,
        [MGName("Binary")]
        [Type(typeof(Byte[]))]
        Binary = 4
        ,
        [MGName("MultiString")]
        [Type(typeof(string[]))]
        MultiString = 5
    }
}
