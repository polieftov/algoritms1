using System;
using System.Collections.Generic;
using System.Text;

namespace LabWork1
{
    class RSA
    {
       char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                '8', '9', '0', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                                                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
                                                'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        BigInt d;
        BigInt n;
        BigInt m;
        List<string> encodeRes; string input; string decodeRes;
        public RSA(BigInt p, BigInt q, string s)
        {
            input = s.ToUpper();
            if (p.IsNumSimple() && q.IsNumSimple())
            {
                n = p * q;
                m = ((p - BigInt.One) * (q - BigInt.One));
                var e = CalculateE(m);
                d = CalculateD(e, m);
                encodeRes = RSAEncode(input, e, n);

                decodeRes = RSADecode(encodeRes, d, n);
            }
            else
                Console.WriteLine("p или q не простые числа");
        }

        public string GetAnswer()
        {
            return decodeRes;
        }

        private string RSADecode(List<string> encodeRes, BigInt d, BigInt n)
        {
            var res = "";
            
            
            foreach(var e in encodeRes)
            {
                var b = new BigInt(e);
                b = b.Pow(d);

                b = b % n;
                
                var index = Convert.ToInt32(b.ToString());

                res += characters[index].ToString();
            }
            return res;
        }

        private List<string> RSAEncode(string s, BigInt e, BigInt n)
        {
            var res = new List<string>();
            var b = new BigInt();

            for (var i = 0; i < s.Length; i++)
            {
                var index = Array.IndexOf(characters, s[i]);

                b = new BigInt(index.ToString());
                b = b.Pow(e);

                b = b % n;

                res.Add(b.ToString());

            }
            return res;
        }

        private BigInt CalculateE(BigInt m)
        {

            var e = m - BigInt.One;

            for (var i = new BigInt("2"); i <= m; i += BigInt.One)
                if ((m % i == BigInt.Zero) && (e % i == BigInt.Zero)) //есть общий делитель
                {
                    e -= BigInt.One;
                    i = BigInt.One;
                }
            return e;
        }

        private BigInt CalculateD(BigInt e, BigInt m)
        {
            return BigInt.FindModInverse(e, m);
        }
    }
}
