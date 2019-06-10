using System;
using System.Globalization;
using System.Numerics;

namespace Homomorphic_Encyption
{
    class Program
    {
        static void Main(string[] args)
        {

            int size = 1024;
            BigInteger p = getP(size);
            BigInteger g = getG(size);

            int votersCount = 5;
            BigInteger[] v = createSampleVotes(votersCount);
            BigInteger[] x = createSamplePrivateValues(votersCount);
            // Voting keys (broadcast by each voter)
            BigInteger[] y = createVotingKeys(g, p, x);
            // Voter values
            BigInteger[] V = createVotingKeys(g, p, v);
            // Each voter calculates Y[i]
            BigInteger[] Y = calculateY(g, p, y);
            BigInteger[] registeredVotes = calculateVotes(Y, x, V, p);
            BigInteger result = calculateResult(registeredVotes, p);

            Console.WriteLine("p={0}\n\ng={1}\n\n", p, g);
            Console.WriteLine("v=[{0}]\n\n", string.Join(", ", v));
            Console.WriteLine("x=[{0}]\n\n", string.Join(", ", x));
            Console.WriteLine("y=[{0}]\n\n", string.Join(", ", y));
            Console.WriteLine("V=[{0}]\n\n", string.Join(", ", V));
            Console.WriteLine("Y=[{0}]\n\n", string.Join(", ", Y));
            Console.WriteLine("registeredVotes=[{0}]\n\n", string.Join(", ", registeredVotes));
            bruteforce(result, g, p);
            Console.ReadLine();
        }

        static void bruteforce(BigInteger result, BigInteger g, BigInteger p)
        {
            for (int i = 0; i < 10000; i++)
            {
                BigInteger gm = BigInteger.ModPow(g, i, p);

                if (result.Equals(gm))
                {
                    Console.WriteLine("Total votes: {0}", i);
                    break;
                }
            }
        }

        static BigInteger calculateResult(BigInteger[] registeredVotes, BigInteger p)
        {
            BigInteger result = 1;

            for (int i = 0; i < registeredVotes.Length; i++)
            {
                result *= registeredVotes[i];
            }
            return result % p;
        }

        static BigInteger[] calculateVotes(BigInteger[] Y, BigInteger[] x, BigInteger[] V, BigInteger p)
        {
            BigInteger[] registeredVotes = new BigInteger[Y.Length];
            for (int i = 0; i < Y.Length; i++)
            {
                registeredVotes[i] = BigInteger.ModPow(Y[i], (int) x[i], p) * V[i];
            }
            return registeredVotes;
        }

        static BigInteger[] calculateY(BigInteger g, BigInteger p, BigInteger[] y)
        {
            BigInteger[] Y = new BigInteger[y.Length];

            for (int i = 0; i < y.Length; i++)
            {
                BigInteger res = 1;

                for (int j = 0; j <= i-1; j++)
                {
                    res *= y[j];
                    res = res % p;
                }

                for (int j = i+1; j < y.Length; j++)
                {
                    res *= modInverse(y[j], p);
                    res = res % p;
                }

                Y[i] = res % p;
            }

            return Y;
        }

        static BigInteger power(BigInteger x, BigInteger y, BigInteger m)
        {
            if (y == 0)
                return 1;

            BigInteger p = power(x, y / 2, m) % m;
            p = (p * p) % m;

            if (y % 2 == 0)
                return p;
            else
                return (x * p) % m;
        }

        static BigInteger modInverse(BigInteger num, BigInteger p)
        {
            return power(num, p - 2, p);
        }

        static BigInteger[] createVotingKeys(BigInteger g, BigInteger p, BigInteger[] x)
        {
            BigInteger[] y = new BigInteger[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                y[i] = makeG(g, x[i], p);
            }
            return y;
        }

        static BigInteger[] createSampleVotes(int size)
        {
            Random random = new Random();
            BigInteger[] votes = new BigInteger[size];
            for (int i = 0; i < size; i++) {
                votes[i] = (BigInteger) random.Next(1, 3);
            }
            return votes;
        }

        static BigInteger[] createSamplePrivateValues(int size)
        {
            Random random = new Random();
            BigInteger[] votes = new BigInteger[size];
            for (int i = 0; i < size; i++)
            {
                votes[i] = (BigInteger) random.Next(1, 10);
            }
            return votes;
        }

        static BigInteger makeG(BigInteger g, BigInteger v, BigInteger p)
        {
            return BigInteger.ModPow(g, v, p);
        }

        static BigInteger hexToInt(string hexValue)
        {
            return Convert.ToUInt64(hexValue, 16);
        }

        // TODO
        static BigInteger getG(int size)
        {
            if (size.Equals(512))
            {
                return BigInteger.Parse("0678471B27A9CF44EE91A49C5147DB1A9AAF244F05A434D6486931D2D14271B9E35030B71FD73DA179069B32E2935630E1C2062354D0DA20A6C416E50BE794CA4", NumberStyles.HexNumber);
            }
            else if (size.Equals(768)) {
                return BigInteger.Parse("030470AD5A005FB14CE2D9DCD87E38BC7D1B1C5FACBAECBE95F190AA7A31D23C4DBBCBE06174544401A5B2C020965D8C2BD2171D3668445771F74BA084D2029D83C1C158547F3A9F1A2715BE23D51AE4D3E5A1F6A7064F316933A346D3F529252", NumberStyles.HexNumber);
            }
            else if (size.Equals(1024)) {
                return BigInteger.Parse("0F7E1A085D69B3DDECBBCAB5C36B857B97994AFBBFA3AEA82F9574C0B3D0782675159578EBAD4594FE67107108180B449167123E84C281613B7CF09328CC8A6E13C167A8B547C8D28E0A3AE1E2BB3A675916EA37F0BFA213562F1FB627A01243BCCA4F1BEA8519089A883DFE15AE59F06928B665E807B552564014C3BFECF492A", NumberStyles.HexNumber);
            }
            return 0;
        }

        static BigInteger getP(int size)
        {
            if (size.Equals(512)) {
                return BigInteger.Parse("0FCA682CE8E12CABA26EFCCF7110E526DB078B05EDECBCD1EB4A208F3AE1617AE01F35B91A47E6DF63413C5E12ED0899BCD132ACD50D99151BDC43EE737592E17", NumberStyles.HexNumber);
            }
            else if (size.Equals(768)) {
                return BigInteger.Parse("0E9E642599D355F37C97FFD3567120B8E25C9CD43E927B3A9670FBEC5D890141922D2C3B3AD2480093799869D1E846AAB49FAB0AD26D2CE6A22219D470BCE7D777D4A21FBE9C270B57F607002F3CEF8393694CF45EE3688C11A8C56AB127A3DAF", NumberStyles.HexNumber);
            }
            else if (size.Equals(1024)) {
                return BigInteger.Parse("0FD7F53811D75122952DF4A9C2EECE4E7F611B7523CEF4400C31E3F80B6512669455D402251FB593D8D58FABFC5F5BA30F6CB9B556CD7813B801D346FF26660B76B9950A5A49F9FE8047B1022C24FBBA9D7FEB7C61BF83B57E7C6A8A6150F04FB83F6D3C51EC3023554135A169132F675F3AE2B61D72AEFF22203199DD14801C7", NumberStyles.HexNumber);
            }
            return 0;
        }
    }
}
