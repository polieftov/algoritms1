using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace LabWork1
{
    class Program
    {
        static void Test()
        {
            //Init test
            var a = new BigInt();
            var b = new BigInt("+10");
            var b1 = new BigInt("010");
            var c = new BigInt('-', new List<int>() { 1, 2, 3, 4, 5 });

            if (a.sign != '+' || !Enumerable.SequenceEqual(a.number, new List<int> { 0 }))
                Console.WriteLine("Init 1 failed");
            if (b.sign != '+' || !Enumerable.SequenceEqual(b.number, new List<int> { 1, 0 }))
                Console.WriteLine("Init 2.1 failed");
            if (b1.sign != '+' || !Enumerable.SequenceEqual(b1.number, new List<int> { 1, 0 }))
                Console.WriteLine("Init 2.2 failed");
            if (c.sign != '-' || !Enumerable.SequenceEqual(c.number, new List<int> { 1, 2, 3, 4, 5 }))
                Console.WriteLine("Init 3 failed");

            //+ - Test
            var d = new BigInt("+12345678");
            var e = new BigInt("+8765432");
            var f = new BigInt("-12345");

            var t1 = d + e;//21111110
            var t2 = d - e;//3580246
            var t3 = d + f;//12333333
            var t4 = d - f;//12358023
            var t5 = e - d;//-3580246
            var t6 = e - f;//8777777
            var t7 = f - e;//-8777777


            if (t1.sign != '+' || !Enumerable.SequenceEqual(t1.number, new List<int> { 2, 1, 1, 1, 1, 1, 1, 0 }))
                Console.WriteLine("+ - 1 failed");
            if (t2.sign != '+' || !Enumerable.SequenceEqual(t2.number, new List<int> { 3, 5, 8, 0, 2, 4, 6 }))
                Console.WriteLine("+ - 2 failed");
            if (t3.sign != '+' || !Enumerable.SequenceEqual(t3.number, new List<int> { 1, 2, 3, 3, 3, 3, 3, 3 }))
                Console.WriteLine("+ - 3 failed");
            if (t4.sign != '+' || !Enumerable.SequenceEqual(t4.number, new List<int> { 1, 2, 3, 5, 8, 0, 2, 3 }))
                Console.WriteLine("+ - 4 failed");
            if (t5.sign != '-' || !Enumerable.SequenceEqual(t5.number, new List<int> { 3, 5, 8, 0, 2, 4, 6 }))
                Console.WriteLine("+ - 5 failed");
            if (t6.sign != '+' || !Enumerable.SequenceEqual(t6.number, new List<int> { 8, 7, 7, 7, 7, 7, 7 }))
                Console.WriteLine("+ - 6 failed");
            if (t7.sign != '-' || !Enumerable.SequenceEqual(t7.number, new List<int> { 8, 7, 7, 7, 7, 7, 7 }))
                Console.WriteLine("+ - 7 failed");

            // * / % Test
            var g = new BigInt("+987654321");
            var h = new BigInt("+123456");
            var i = new BigInt("-987654");
            var j = new BigInt("100000");
            var g1 = new BigInt("28");
            var j1 = new BigInt("4");

            var t8 = g * h;//121931851853376
            var t9 = g * i;//-975460740752934
            var t10 = g * j;//98765432100000
            var t11 = g / h;//8000
            var t12 = g % h;//6321
            var t13 = h % g;//123456
            var t14 = g / j;//9876
            var t15 = g % j;//54321
            var t16 = g1 % j1;//0

            if (t8.sign != '+' || !Enumerable.SequenceEqual(t8.number, new List<int> { 1, 2, 1, 9, 3, 1, 8, 5, 1, 8, 5, 3, 3, 7, 6 }))
                Console.WriteLine("* / % 1 failed");
            if (t9.sign != '-' || !Enumerable.SequenceEqual(t9.number, new List<int> { 9, 7, 5, 4, 6, 0, 7, 4, 0, 7, 5, 2, 9, 3, 4 }))
                Console.WriteLine("* / % 2 failed");
            if (t10.sign != '+' || !Enumerable.SequenceEqual(t10.number, new List<int> { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0 }))
                Console.WriteLine("* / % 3 failed");
            if (t11.sign != '+' || !Enumerable.SequenceEqual(t11.number, new List<int> { 8, 0, 0, 0 }))
                Console.WriteLine("* / % 4 failed");
            if (t12.sign != '+' || !Enumerable.SequenceEqual(t12.number, new List<int> { 6, 3, 2, 1 }))
                Console.WriteLine("* / % 5 failed");
            if (t13.sign != '+' || !Enumerable.SequenceEqual(t13.number, new List<int> { 1, 2, 3, 4, 5, 6 }))
                Console.WriteLine("* / % 6 failed");
            if (t14.sign != '+' || !Enumerable.SequenceEqual(t14.number, new List<int> { 9, 8, 7, 6 }))
                Console.WriteLine("* / % 7 failed");
            if (t15.sign != '+' || !Enumerable.SequenceEqual(t15.number, new List<int> { 5, 4, 3, 2, 1 }))
                Console.WriteLine("* / % 8 failed");
            if (t16.sign != '+' || !Enumerable.SequenceEqual(t16.number, new List<int> { 0 }))
                Console.WriteLine("* / % 9 failed");

            //Compare Test
            if (!(g > h))
                Console.WriteLine("Compare 1 failed");
            if (!(h < g))
                Console.WriteLine("Compare 2 failed");
            if (!(g != h))
                Console.WriteLine("Compare 3 failed");
            if (!(g == new BigInt("+987654321")))
                Console.WriteLine("Compare 4 failed");
            if (!(g >= i))
                Console.WriteLine("Compare 5 failed");
            if (!(i <= g))
                Console.WriteLine("Compare 6 failed");

            //Mod Inverse Test
            var k = new BigInt("3");
            var l = new BigInt("26");
            var m = new BigInt("5");
            var n = new BigInt("12");

            var t17 = BigInt.FindModInverse(k, l);//9
            var t18 = BigInt.FindModInverse(m, n);//5
            
            if (t17 != new BigInt("9"))
                Console.WriteLine("Mod Inverse 1 failed");
            if (t18 != new BigInt("5"))
                Console.WriteLine("Mod Inverse 2 failed");

            
            //RSA TEST
            var o = new BigInt("13");
            var p = new BigInt("7");

            var t19 = new RSA(o, p, "text");

            if (t19.GetAnswer() != "TEXT")
                Console.WriteLine("RSA TEST 1 failed");

            //IsSimple TEST
            var q = new BigInt("26");
            var r = new BigInt("59");
            var s = new BigInt("103");
            var t = new BigInt("200");

            if (q.IsNumSimple())
                Console.WriteLine("IsSimple TEST 1 failed");
            if (!r.IsNumSimple())
                Console.WriteLine("IsSimple TEST 1 failed");
            if (!s.IsNumSimple())
                Console.WriteLine("IsSimple TEST 1 failed");
            if (t.IsNumSimple())
                Console.WriteLine("IsSimple TEST 1 failed");
        }

        static void Main()
        {
            var p = ""; var q = "";
            var text = "";
            Test();
            var xDoc = new XmlDocument();
            xDoc.Load("lab.xml");
            var xRoot = xDoc.DocumentElement;
            foreach (XmlNode xNode in xRoot)
            {

                if (xNode.Name == "p")
                    p = xNode.InnerText;
                if (xNode.Name == "q")
                    q = xNode.InnerText;
                if (xNode.Name == "text")
                    text = xNode.InnerText;

            }
            var rsa = new RSA(new BigInt(p), new BigInt(q), text);
            Console.WriteLine(rsa.GetAnswer());
            Console.ReadKey();
        }
    }
}
