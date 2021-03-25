using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LabWork1
{
    class BigInt : ICloneable
    {
        public char sign; // +, -
        public List<int> number;
        public static BigInt Zero => new BigInt();
        public static BigInt One => new BigInt("1");

        public BigInt(char _sign, List<int> _number)
        {
            sign = _sign;
            number = _number;
            if (number.Count > 1)
                this.DeleteNuls();
        }

        public BigInt()
        {
            sign = '+';
            number = new List<int>() { 0 };
        }

        public BigInt(string str)
        {
            number = new List<int>();
            if (str[0] == '+')
            {
                sign = '+';
                str = str.Remove(0, 1);
            }
            else if (str[0] == '-')
            {
                sign = '-';
                str = str.Remove(0, 1);
            }
            else if (char.IsDigit(str[0]))
            {
                sign = '+';
            }
            else
                throw new ArgumentException();
            for (int i = 0; i < str.Length; i++)
                number.Add(int.Parse(str[i].ToString()));
            this.DeleteNuls();
        }

        public override string ToString()
        {
            var n = "";
            foreach(var e in number)
                n += e.ToString();
            return sign + n;
        }

        private void DeleteNuls()
        {
            if (number.Count > 1)
            {
                while (number[0] == 0)
                    number.RemoveAt(0);
            }
        }

        private void SetDigit(int index, int val)
        {
            while (number.Count <= index)
            {
                number.Add(0);
            }

            number[index] = val;
        }

        private void EqualizeTheDigits(int a)
        {
            if (a >= number.Count)
                number.InsertRange(0, new List<int>(new int[a - number.Count]));
        }

        private static BigInt Addition(BigInt a, BigInt b)//суммирует два бигинта
        {
            var res = new BigInt();
            var maxLen = Math.Max(a.number.Count, b.number.Count) - 1;
            var enlarge = 0;
            var num1 = a.number; var num2 = b.number; var resNum = res.number;
            if (num1.Count < num2.Count) 
            { 
                a.EqualizeTheDigits(b.number.Count);
                res.EqualizeTheDigits(b.number.Count);
            }
            else 
            { 
                b.EqualizeTheDigits(a.number.Count); 
                res.EqualizeTheDigits(a.number.Count);
            }
                for (var i = maxLen; i >= 0; i--)
            {
                var s = num1[i] + num2[i] + enlarge;
                if (s >= 10)
                {
                    enlarge = 1;
                    resNum[i] = s - 10;
                }
                else
                {
                    enlarge = 0;
                    resNum[i] = s;
                }
            }
            if (enlarge == 1) resNum.Insert(0, 1);
            //res.number = resNum;
            res.DeleteNuls();
            return res;
        }

        private static BigInt Reduce(BigInt a, BigInt b)//вычитает из большего меньшее два бигинта
        {
            if (b.sign == '-') return Addition(a, b);
            a.DeleteNuls(); b.DeleteNuls(); 
            b.EqualizeTheDigits(a.number.Count);
            var res = new BigInt();
            var num1 = new List<int>(a.number); var num2 = new List<int>(b.number);
            for (var i = a.number.Count - 1; i >= 0; i--)
            {
                var r = num1[i] - num2[i];
                if (r < 0)
                {
                    num1[i - 1]--;
                    r = 10 + r;
                }
                res.SetDigit(i, r);
            }
            res.DeleteNuls();
            return res;

        }

        public object Clone()
        {
            return new BigInt { number = new List<int>(this.number), sign = this.sign };
        }

        public static BigInt Mod(BigInt a1, BigInt b) // ИСПРАВЛЕНО деление по модулю, дает всегда неотрицательный остаток
        {
            var a = new BigInt('+', new List<int>(a1.number));
            a.DeleteNuls(); b.DeleteNuls();
            if (a < b) return a;
            else if (a == b) return new BigInt('+', new List<int> { 0 });
            else
            {

                while (a > b)
                {
                    
                    var bSDopisNuls = new BigInt(b.sign, new List<int>(b.number));

                    BigInt newraz;
                    var razn = new BigInt((a.number.Count - b.number.Count).ToString());//1
                    bSDopisNuls *= new BigInt("10").Pow(razn);
                    if (bSDopisNuls > a)
                        newraz = razn - One;//2
                    else
                        newraz = razn;
                    var rabRazr = new BigInt("10").Pow(newraz);//3
                    var newrazRes = b * rabRazr.MultOnDigit(1);
                    for (var i = 1; b * rabRazr.MultOnDigit(i) < a; i++)
                    {
                        var r = rabRazr.MultOnDigit(i);
                        newrazRes = r;
                    }//4
                    if (a > newrazRes * b)
                    {
                        if (a - newrazRes * b < b)
                        {

                            a -= newrazRes * b;
                            break;
                        }
                    }
                    else break;
                    a -= newrazRes * b;
                }
                a.DeleteNuls();
                return a;
            }
        }

        public static BigInt Div(BigInt a1, BigInt b) // ИСПРАВЛЕНО деление нацело, округляет в левую сторону(-11)/10 = -2  (-10)/10 = -1
        {
            var a = new BigInt('+', new List<int>(a1.number));
            a.DeleteNuls();b.DeleteNuls();
            if (a < b) return Zero;
            else if (a == b) return One;
            else
            {
                var res = new List<BigInt>();
                while (a > b)
                {
                    var r = new BigInt(b.sign, new List<int>(b.number));
                    var bSDopisNuls = new BigInt(b.sign, new List<int>(b.number));
                    var newrazRes = Zero;
                    var newraz = 0;
                    var razn = a.number.Count - b.number.Count;//1
                    bSDopisNuls = bSDopisNuls.MultOn10(razn);
                    if (bSDopisNuls > a)
                        newraz = razn - 1;//2
                    else
                        newraz = razn;
                    var rabRazr = new BigInt('+' + (Math.Pow(10, newraz)).ToString());//3
                    
                    for (var i = 1; b * rabRazr.MultOnDigit(i) < a; i++)
                    {
                        r = rabRazr.MultOnDigit(i);
                        newrazRes = r;
                    }//4
                    if (a > newrazRes * b)
                    {
                        if (a - newrazRes * b < b)
                        {
                            res.Add(r);
                            break;
                        }
                    }
                    else break;
                    a -= newrazRes * b;
                    res.Add(r);

                }
                var result = Zero;
                foreach (var e in res)
                    result += e;
                result.sign = a.sign == b.sign ? '+' : '-';
                return result;
            }
        }
        public BigInt MultOnDigit(int n) // умножение на цифру 0, ...,9
        {
            var sign = n;
            n = Math.Abs(n);
            var result = new BigInt('+', new List<int>());
            var t = 0; int r;
            for (var i = this.number.Count - 1; i >= 0; i--)
            {
                r = this.number[i] * n + t;
                {
                    t = r / 10;
                    r %= 10;
                }
                result.SetDigit(i, r);
            }
            if (t != 0) result.number.Insert(0, t);
            result.DeleteNuls();
            if ((this.sign == '+' && sign > 0) || (this.sign == '-' && sign < 0))
                return result;
            else
                return -result;

        }
        public BigInt MultOn10(int n) // умножение на 10^n
        {
            this.number.AddRange(new int[n]);
            return this;
        }

        //-------------------------------
        public static BigInt operator +(BigInt V1, BigInt V2)  // *this + V
        {
            if (V1.sign == '+' && V2.sign == '+')
                return Addition(V1, V2);
            else if (V1.sign == '+' && V2.sign == '-')
                return V1 - -V2;
            else if (V2.sign == '+' && V1.sign == '-')
                return V2 - -V1;
            else
            {
                var r = Addition(V1, V2);
                r.sign = '-';
                return r;
            }  
        }
        public static BigInt operator -(BigInt V1, BigInt V2)  // *this - V
        {
            if (V1 >= V2)
            {
                return Reduce(V1, V2);
            }
            else
            {
                return -Reduce(V2, V1);
            }
        }
        public static BigInt operator *(BigInt V1, BigInt V2)  // *this * V
        {
            V1.DeleteNuls(); V2.DeleteNuls();
            var result = new BigInt();
            result.EqualizeTheDigits(V1.number.Count + V2.number.Count);
            V1.number.Reverse(); V2.number.Reverse();
            for (var i = 0; i < V1.number.Count; ++i)
                for (int j = 0, carry = 0; j < V2.number.Count || carry > 0; ++j)
                {
                    var cur = result.number[i + j] + V1.number[i] * (j < V2.number.Count ? V2.number[j] : 0) + carry;
                    result.number[i + j] = cur % 10;
                    carry = cur / 10;
                }
            while (result.number.Count > 1 && result.number.Last() == 0)
                result.number.RemoveAt(result.number.Count - 1);
            result.sign = V1.sign == V2.sign ? '+' : '-';
            V1.number.Reverse(); V2.number.Reverse();
            result.number.Reverse();
            result.DeleteNuls();
            return result;
        }

        public BigInt Pow(BigInt a)
        {
            this.DeleteNuls();a.DeleteNuls();
            var res = new BigInt(sign, new List<int>(number));
            var a1 = new BigInt(a.sign, new List<int>(a.number));
            if (a1 == Zero) return One;
            for (;a1 > One; a1 -= One)
                res *= this;
            return res;
        }
                                             //---------------------------------
        public static BigInt operator -(BigInt obj)   // унарный минус  -V
        {
            return obj.sign == '+'? new BigInt('-', obj.number) : new BigInt('+', obj.number);
        }

        //Операторы сравнения

        private static int Compare(BigInt V1, BigInt V2)
        {
            if (V1.sign == '+' && V2.sign == '-')
                return 1;
            else if (V2.sign == '+' && V1.sign == '-')
                return -1;
            else if (V1.sign == '+' && V2.sign == '+')
            {
                return CompareWithoutSign(V1, V2);
            }
            else
                return -1 * CompareWithoutSign(V1, V2);
        }

        private static int CompareWithoutSign(BigInt v1, BigInt v2)
        {
            v1.DeleteNuls();v2.DeleteNuls();
            if (v1.number.Count > v2.number.Count)
                return 1;
            else if (v1.number.Count < v2.number.Count)
                return -1;
            else
            {
                for (var i = 0; i <= v1.number.Count-1; i++)
                {
                    if (v1.number[i] > v2.number[i])
                        return 1;
                    else if (v1.number[i] < v2.number[i])
                        return -1;
                }
                return 0;
            }
        }

        public static BigInt FindModInverse(BigInt a, BigInt n)//computes the inverse of a modulo n
        {
            var x = new BigInt(); var y = new BigInt();
            ExtendedEuclid(a, n, out x, out y);
            return (x % n + n) % n;
        }

        private static BigInt ExtendedEuclid(BigInt a, BigInt b, out BigInt x, out BigInt y)
        {
            
            if (a == new BigInt())
            {
                x = new BigInt();
                y = new BigInt("1");
                return b;
            }
            var newY = new BigInt();
            var newX = new BigInt();
            var gcd = ExtendedEuclid(b % a, a, out newX,out newY);
            
            x = newY - (b / a) * newX;
            y = newX;

            return gcd;
        }

        public static BigInt operator /(BigInt a, BigInt b) => Div(a, b);
        public static BigInt operator %(BigInt a, BigInt b) => Mod(a, b);
        public static bool operator <(BigInt a, BigInt b) => Compare(a, b) < 0;

        public static bool operator >(BigInt a, BigInt b) => Compare(a, b) > 0;

        public static bool operator <=(BigInt a, BigInt b) => Compare(a, b) <= 0;

        public static bool operator >=(BigInt a, BigInt b) => Compare(a, b) >= 0;

        public static bool operator ==(BigInt a, BigInt b) => Compare(a, b) == 0;

        public static bool operator !=(BigInt a, BigInt b) => Compare(a, b) != 0;
    }
}
