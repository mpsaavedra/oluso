using System.Collections;
using System.Collections.ObjectModel;

namespace Tests.Fakes;

public static class FakeTypes
{
    public class ValueTypes
    {
        public class SimpleTypes
        {
            #region Integer without sign

            public static sbyte? SByte_Null { get; } = null;
            public static sbyte SByte_Default { get; } = default;
            public static sbyte SByte_Min { get; } = sbyte.MinValue;
            public static sbyte SByte_Max { get; } = sbyte.MaxValue;

            public static sbyte SByte_New(int max = 3)
            {
                var bytes = new byte[max];
                new Random().NextBytes(bytes);
                return (sbyte)bytes[0];
            }

            public static short? Short_Null { get; } = null;
            public static short Short_Default { get; } = default;
            public static short Short_Min { get; } = short.MinValue;
            public static short Short_Max { get; } = short.MaxValue;
            public static short Short_New(int max = 3) => (short)new Random().Next(1 << max);

            public static int? Int_Null { get; } = null;
            public static int Int_Default { get; } = default;
            public static int Int_Min { get; } = int.MinValue;
            public static int Int_Max { get; } = int.MaxValue;
            public static int Int_New(int max = 3) => new Random().Next(max);

            public static long? Long_Null { get; } = null;
            public static long Long_Default { get; } = default;
            public static long Long_Min { get; } = long.MinValue;
            public static long Long_Max { get; } = long.MaxValue;
            public static long Long_New(int max = 3) => (long)new Random().Next(max);

            #endregion

            #region Integer with sign

            public static byte? Byte_Null { get; } = null;
            public static byte Byte_Default { get; } = default;
            public static byte Byte_Min { get; } = byte.MinValue;
            public static byte Byte_Max { get; } = byte.MaxValue;

            public static byte Byte_New(int max = 3)
            {
                var bytes = new byte[max];
                new Random().NextBytes(bytes);
                return bytes[0];
            }

            public static ushort? UShort_Null { get; } = null;
            public static ushort UShort_Default { get; } = default;
            public static ushort UShort_Min { get; } = ushort.MinValue;
            public static ushort UShort_Max { get; } = ushort.MaxValue;
            public static ushort UShort_New(int max = 3) => (ushort)new Random().Next(1 << max);

            public static uint? UInt_Null { get; } = null;
            public static uint UInt_Default { get; } = default;
            public static uint UInt_Min { get; } = uint.MinValue;
            public static uint UInt_Max { get; } = uint.MaxValue;
            public static uint UInt_New(int max = 3) => (uint)new Random().Next(max);

            public static ulong? ULong_Null { get; } = null;
            public static ulong ULong_Default { get; } = default;
            public static ulong ULong_Min { get; } = ulong.MinValue;
            public static ulong ULong_Max { get; } = ulong.MaxValue;
            public static ulong ULong_New(int max = 3) => (ulong)new Random().Next(max);

            #endregion

            #region Unicode chars

            public static char? Char_Null { get; } = null;
            public static char Char_Default { get; } = default;
            public static char Char_Min { get; } = char.MinValue;
            public static char Char_Max { get; } = char.MaxValue;
            public static char Char_New(int max = 3) => (char)new Random().Next(max);

            #endregion

            #region Binary Float IEEE

            public static float? Float_Null { get; } = null;
            public static float Float_Default { get; } = default;
            public static float Float_Min { get; } = float.MinValue;
            public static float Float_Max { get; } = float.MaxValue;
            public static float Float_New() => (float)new Random().NextDouble();

            public static double? Double_Null { get; } = null;
            public static double Double_Default { get; } = default;
            public static double Double_Min { get; } = double.MinValue;
            public static double Double_Max { get; } = double.MaxValue;
            public static double Double_New() => new Random().NextDouble();

            #endregion

            #region Decimal

            public static decimal? Decimal_Null { get; } = null;
            public static decimal Decimal_Default { get; } = default;
            public static decimal Decimal_Min { get; } = decimal.MinValue;
            public static decimal Decimal_Max { get; } = decimal.MaxValue;

            public static decimal Decimal_New()
            {
                var rng = new Random();
                byte scale = (byte)rng.Next(3);
                bool sign = rng.Next(2) == 1;
                return new decimal(rng.Next(), rng.Next(), rng.Next(), sign, scale);
            }

            #endregion

            #region Boolean

            public static bool? Bool_Null { get; } = null;
            public static bool Bool_Default { get; } = default; // false
            public static bool Bool_True { get; } = true;
            public static bool Bool_False { get; } = false;

            #endregion
        }

        public class EnumerationTypes
        {
            public enum ExampleEnumeration
            {
                a,
                e,
                i,
                o,
                u
            };

            public enum Gender
            {
                Male,
                Female,
                NotSpecified
            }
        }

        public class StructureTypes
        {
            public struct ExampleStructure
            {
                public Guid Id { get; set; }
                public string? Name { get; set; }
                public string? Password { get; set; }
                public string? ConfirmPassword { get; set; }
                public string? Email { get; set; }
                public string? Hostname { get; set; }
                public int? Point { get; set; }
                public List<string>? Roles { get; set; }
            }

            public static Guid? Guid_Null { get; } = null;
            public static Guid Guid_Default { get; } = default;
            public static Guid Guid_Empty { get; } = Guid.Empty;
            public static Guid Guid_New() => Guid.NewGuid();
        }
    }

    public class ReferenceTypes
    {
        public class ClassTypes
        {
            public static string? String_Null { get; } = null;
            public static string? String_Default { get; } = default; // null
            public static string String_Empty { get; } = string.Empty;

            public static string String_New(int count = 3, char value = '*')
                => new string(value, count);

            public static object? Object_Null { get; } = null;
            public static object? Object_Default { get; } = default; // null
            public static object Object_Empty { get; } = new object();
            public static object Object_New() => new object();

            public static object Object_New_AnonymousType()
                => new
                {
                    ConfirmPassword = String_New(),
                    Email = String_New(),
                    Hostname = String_New(),
                    Id = ValueTypes.StructureTypes.Guid_New(),
                    Name = String_New(),
                    Password = String_New(),
                    Point = ValueTypes.SimpleTypes.Int_New(),
                    Roles = new List<string>()
                };
        }

        public class InterfaceTypes
        {
            public interface IExampleInterface
            {
                public Guid Id { get; set; }
                public string? Name { get; set; }
                public string? Password { get; set; }
                public string? ConfirmPassword { get; set; }
                public string? Email { get; set; }
                public string? Hostname { get; set; }
                public int? Point { get; set; }
                public List<string>? Roles { get; set; }
            }
        }

        public class MatrixTypes
        {
            public static string[]? Array_Null { get; } = null;
            public static string[]? Array_Default { get; } = default;
            public static string[] Array_CountZero = new string[0];

            public static string[] Array_New(int count = 3)
                => List(count).ToArray();
        }

        public class DelegatesTypes
        {
            public delegate void ExampleDelegateVoid();

            public delegate int ExampleDelegate(int number);
        }
    }

    public class CollectionsTypes
    {
        public static IEnumerable<string>? Enumerable_Null { get; } = null;
        public static IEnumerable<string>? Enumerable_Default { get; } = default; // null
        public static IEnumerable<string> Enumerable_CountZero { get; } = new List<string>();

        public static IEnumerable<string> Enumerable_New(int count = 3)
            => List(count).AsEnumerable();

        public static List<string>? List_Null { get; } = null;
        public static List<string>? List_Default { get; } = default; // null
        public static List<string> List_CountZero { get; } = new List<string>();

        public static List<string> List_New(int count = 3)
            => List(count);

        public static HashSet<string>? HashSet_Null { get; } = null;
        public static HashSet<string>? HashSet_Default { get; } = default; // null
        public static HashSet<string> HashSet_CountZero { get; } = new HashSet<string>();

        public static HashSet<string> HashSet_New(int count = 3)
            => List(count).ToHashSet();

        public static ICollection<string>? Collection_Null { get; } = null;
        public static ICollection<string>? Collection_Default { get; } = default; // null
        public static ICollection<string> Collection_CountZero { get; } = new Collection<string>();

        public static ICollection<string> Collection_New(int count = 3)
            => new Collection<string>(List(count));

        public static Queue<string>? Queue_Null { get; } = null;
        public static Queue<string>? Queue_Default { get; } = default; // null
        public static Queue<string> Queue_CountZero { get; } = new Queue<string>();

        public static Queue<string> Queue_New(int count = 3)
            => new Queue<string>(List(count));

        public static SortedSet<string>? SortedSet_Null { get; } = null;
        public static SortedSet<string>? SortedSet_Default { get; } = default; // null
        public static SortedSet<string> SortedSet_CountZero { get; } = new SortedSet<string>();

        public static SortedSet<string> SortedSet_New(int count = 3)
            => new SortedSet<string>(List(count));

        public static Stack<string>? Stack_Null { get; } = null;
        public static Stack<string>? Stack_Default { get; } = default; // null
        public static Stack<string> Stack_CountZero { get; } = new Stack<string>();

        public static Stack<string> Stack_New(int count = 3)
            => new Stack<string>(List(count));

        public static Array? ArrayClass_Null { get; } = null;
        public static Array? ArrayClass_Default { get; } = default;
        public static Array ArrayClass_CountZero = Array.CreateInstance(typeof(string), 0);

        public static Array ArrayClass_New(int count = 3)
            => List(count).ToArray();

        public static ArrayList? ArrayList_Null { get; } = null;
        public static ArrayList? ArrayList_Default { get; } = default;
        public static ArrayList ArrayList_CountZero = new ArrayList(0);

        public static ArrayList ArrayList_New(int count = 3)
            => new ArrayList(List(count));
    }

    public class TestValues
    {
        public static string FakeUserName1 = "Fake User 1";
        
        public static string FakeUserName2 = "Fake User 2";
        
        public static string FakeUserName3 = "Fake User 3";
        
        public static string FakeUserName4 = "Fake User 4";

        public static string? String_Null = null;

        public static string String_WhiteSpace = " ";

    }
    
    private static List<string> List(int count)
    {
        var list = new List<string>(count);

        for (var i = 0; i < count; i++)
        {
            list.Add(Guid.NewGuid().ToString());
        }

        return list;
    }
}