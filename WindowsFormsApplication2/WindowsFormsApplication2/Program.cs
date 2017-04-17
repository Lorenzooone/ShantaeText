using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace WindowsFormsApplication2
{
    public class Free
    {
        public List<int> Length = new List<int>(), Position = new List<int>();
    }
    public class Shantae
    {
        public int Pointer, Length, Position;
    }
    public class TextByte
    {
        public List<List<byte>> BytesChar = new List<List<byte>>();
        public List<int> Flag = new List<int>();
        public List<int> FinalPosition = new List<int>();
        public List<int> Pointers = new List<int>();
    }
    public class TextHandler
    {
        public static bool CheckROM(string Path) {
        byte[] memblock = File.ReadAllBytes(Path);
            if ((memblock.Length == 0x400000) && ((memblock[0x134] == 'S') && (memblock[0x135] == 'H') && (memblock[0x136] == 'A') && (memblock[0x137] == 'N') && (memblock[0x138] == 'T') && (memblock[0x139] == 'A') && (memblock[0x13A] == 'E')))
                return true;
            else
                return false;
            }
        static int SearchClosest(int a, List<int> b)
        {
            int o = 0, u = 0xFFFFFF;
            for (int g = 0; g < b.Count; g++)
            {
                if ((a <= b[g]) && (b[g] < u))
                {
                    u = b[g];
                    o = g;
                }
            }
            return o;
        }
        static int ConvertCharToHex(char a)
        {
            int e = 0;
            if (a == '0')
                e = 0x0;
            else if (a == '1')
                e = 0x1;
            else if (a == '2')
                e = 0x2;
            else if (a == '3')
                e = 0x3;
            else if (a == '4')
                e = 0x4;
            else if (a == '5')
                e = 0x5;
            else if (a == '6')
                e = 0x6;
            else if (a == '7')
                e = 0x7;
            else if (a == '8')
                e = 0x8;
            else if (a == '9')
                e = 0x9;
            else if (a == 'A')
                e = 0xA;
            else if (a == 'B')
                e = 0xB;
            else if (a == 'C')
                e = 0xC;
            else if (a == 'D')
                e = 0xD;
            else if (a == 'E')
                e = 0xE;
            else if (a == 'F')
                e = 0xF;
            return e;
        }
        public static int Inject(string Path2)
        {
            string Path = "textBank.txt";
            string[] text = File.ReadAllLines(Path);
            char[] phrase;
            byte[] memblock = File.ReadAllBytes(Path2);
            List<Shantae> TextRel = new List<Shantae>();
            List<Free> FreeSpace = new List<Free>();
            for (int u = 0; u < 3; u++)
                FreeSpace.Add(new Free());
            for (int u = 0; u < text.Count(); u++)
            {
                Shantae Temp = new Shantae();
                phrase = text[u].ToArray();
                int Pointer = 0;
                if (phrase[0] == '{')
                {
                    Pointer = (ConvertCharToHex(phrase[1]) << 12) + (ConvertCharToHex(phrase[2]) << 8) + (ConvertCharToHex(phrase[3]) << 4) + ConvertCharToHex(phrase[4])+0xA0000;
                    Temp.Pointer = Pointer;
                }
                else
                {
                    return 0xFE;
                }
                Temp.Position = (memblock[Pointer] + (memblock[Pointer + 1] << 8)) + ((((Pointer - 0xA0000) / 0x4000)-1) * 0x4000)+0xA0000;
                int a = 0;
                while ((memblock[Temp.Position + a] != 0xFF) && (memblock[Temp.Position + a] != 0x80))
                {
                    a++;
                }
                if (memblock[Temp.Position + a] == 0xFF)
                    if (memblock[Temp.Position + a + 1] == 0x00)
                        a += 2;
                    else a++;
                else
                    a++;
                Temp.Length = a;
                TextRel.Add(Temp);
            }
            int Begin = 0x4000;
            int o = 0;
            while (o < TextRel.Count)
            {
                Begin += 0x4000;
                while (TextRel[o].Pointer < Begin)
                {
                    int u = 0;
                    if (FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Position != null)
                        for (int g = 0; g < FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Position.Count; g++)
                            if (TextRel[o].Position == (FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Position[g] + FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Length[g]))
                            {
                                u = 1;
                                FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Length[g] += TextRel[o].Length;
                                g = FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Position.Count;
                            }
                    if (u == 0)
                    {
                        if (TextRel[o].Length > 1)
                        {
                            FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Position.Add(TextRel[o].Position);
                            FreeSpace[((Begin - 0xA0000) / 0x4000) - 1].Length.Add(TextRel[o].Length);
                        }
                    }
                    if (o < TextRel.Count - 1)
                        o++;
                    else
                        Begin = 0;
                }
                if (Begin == 0)
                    o++;
            }
            for (Begin = 0xABFFF; Begin > 0x9FFFF; Begin -= 0x4000)
            {
                int Begun = Begin;
                int a = 0;
                while (memblock[Begun--] == 0xFF)
                    a++;
                FreeSpace[((Begin-0xA0000) / 0x4000)].Position.Add(Begun + 2);
                FreeSpace[((Begin - 0xA0000) / 0x4000)].Length.Add(a);
            }
            o = 0;
            TextByte[] TextFromChar = new TextByte[3];
            for (int u = 0; u < 3; u++)
                TextFromChar[u] = new TextByte();
            for (Begin = 0xA3FFF; Begin < 0xAC000; Begin += 0x4000)
            {
                while (TextRel[o].Pointer < Begin)
                {
                    List<byte> Temp = new List<byte>();
                    phrase = text[o].ToArray();
                    for (int g = 7; g < phrase.Length; g++)
                    {
                        if (phrase[g] == '[')
                        {
                            if (phrase[g + 1] == 'N')
                            {
                                Temp.Add(0x9C);
                                Temp.Add((byte)ConvertCharToHex(phrase[g + 5]));
                                Temp.Add(0xC0);
                                g += 6;
                            }
                            else if (phrase[g + 2] == 'F')
                            {
                                Temp.Add(0x5B);
                                Temp.Add((byte)phrase[g + 5]);
                                g += 6;
                            }
                            else if (phrase[g + 5] == 'F')
                            {
                                Temp.Add(0xFF);
                                Temp.Add(0x00);
                                g += 6;
                            }
                            else
                            {
                                Temp.Add(0x80);
                                g += 6;
                            }
                        }
                        else
                            Temp.Add((Byte)phrase[g]);
                    }
                    int Flag = 0;
                    for (int u = 0; u < TextFromChar[((Begin - 0xA0000) / 0x4000)].BytesChar.Count; u++)
                    {
                        if (TextFromChar[((Begin - 0xA0000) / 0x4000)].BytesChar[u].Count == Temp.Count)
                        {
                            int t = 0;
                            for (int g = 0; g < Temp.Count; g++)
                                if (TextFromChar[((Begin - 0xA0000) / 0x4000)].BytesChar[u][g] != Temp[g])
                                {
                                    t = 1;
                                    g = Temp.Count;
                                }
                            if (t == 0)
                            {
                                Flag = u + 1;
                                u = TextFromChar[((Begin - 0xA0000) / 0x4000)].BytesChar.Count;
                            }
                        }
                    }
                    TextFromChar[((Begin-0xA0000) / 0x4000)].BytesChar.Add(Temp);
                    TextFromChar[((Begin - 0xA0000) / 0x4000)].Flag.Add(Flag);
                    TextFromChar[((Begin - 0xA0000) / 0x4000)].Pointers.Add(TextRel[o].Pointer);
                    o++;
                    if (o == TextRel.Count)
                    {
                        Begin = 0;
                        o--;
                    }
                }
                if ((o == TextRel.Count - 1) && (Begin == 0))
                {
                    Begin = 0xABFFF;
                }
            }
            List<int>[] Exceeding = new List<int>[3];
            for (int u = 0; u < 3; u++)
                Exceeding[u] = new List<int>();
            for (int u = 0; u < 3; u++)
            {
                for (o = 0; o < TextFromChar[u].BytesChar.Count; o++)
                {
                    int k = 0;
                    int h = -1;
                    for (int g = 0; g < FreeSpace[u].Position.Count; g++)
                    {
                        if (TextFromChar[u].BytesChar[o].Count == FreeSpace[u].Length[g])
                        {
                            if (TextFromChar[u].Flag[o] == 0)
                            {
                                FreeSpace[u].Length[g] = 0;
                                k = 1;
                                for (int i = 0; i < TextFromChar[u].BytesChar[o].Count; i++)
                                    memblock[FreeSpace[u].Position[g] + i] = TextFromChar[u].BytesChar[o][i];
                                memblock[TextFromChar[u].Pointers[o]] = (byte)((FreeSpace[u].Position[g] - (0x4000 * (u-1))) & 0xFF);
                                memblock[TextFromChar[u].Pointers[o] + 1] = (byte)(((FreeSpace[u].Position[g] - (0x4000 * (u-1))) >> 8) & 0xFF);
                                h = (FreeSpace[u].Position[g]);
                                g = FreeSpace[u].Position.Count;
                            }
                        }
                    }
                    if (k == 0)
                        Exceeding[u].Add(o);
                    TextFromChar[u].FinalPosition.Add(h);
                }
            }
            for (int u = 0; u < 3; u++)
            {
                for (int i = 0; i < Exceeding[u].Count; i++)
                {
                    if (TextFromChar[u].Flag[Exceeding[u][i]] == 0)
                    {
                        int l = SearchClosest(TextFromChar[u].BytesChar[Exceeding[u][i]].Count, FreeSpace[u].Length);
                        for (int j = 0; j < TextFromChar[u].BytesChar[Exceeding[u][i]].Count; j++)
                            memblock[FreeSpace[u].Position[l] + j] = TextFromChar[u].BytesChar[Exceeding[u][i]][j];
                        memblock[TextFromChar[u].Pointers[Exceeding[u][i]]] = (byte)((FreeSpace[u].Position[l] - (0x4000 * (u-1))) & 0xFF);
                        memblock[TextFromChar[u].Pointers[Exceeding[u][i]] + 1] = (byte)(((FreeSpace[u].Position[l] - (0x4000 * (u-1))) >> 8) & 0xFF);
                        TextFromChar[u].FinalPosition[Exceeding[u][i]] = FreeSpace[u].Position[l];
                        FreeSpace[u].Position[l] += TextFromChar[u].BytesChar[Exceeding[u][i]].Count;
                        FreeSpace[u].Length[l] -= TextFromChar[u].BytesChar[Exceeding[u][i]].Count;
                    }
                }
            }
            for (int u = 0; u < 3; u++)
            {
                for (int i = 0; i < TextFromChar[u].BytesChar.Count; i++)
                {
                    if (TextFromChar[u].Flag[i] != 0)
                    {
                        memblock[TextFromChar[u].Pointers[i]] = (byte)((TextFromChar[u].FinalPosition[TextFromChar[u].Flag[i] - 1] - (0x4000 * (u-1))) & 0xFF);
                        memblock[TextFromChar[u].Pointers[i] + 1] = (byte)(((TextFromChar[u].FinalPosition[TextFromChar[u].Flag[i] - 1] - (0x4000 * (u-1))) >> 8) & 0xFF);
                        TextFromChar[u].FinalPosition[i] = TextFromChar[u].FinalPosition[TextFromChar[u].Flag[i] - 1];
                    }
                }
            }
            String Path3 = Path2.Remove(Path2.Length-4);
                Path3 += "New.gbc";
            File.WriteAllBytes(Path3, memblock);
            return 0;
        }
        static char ConvertHexToChar(int a)
        {
            char e = '0';
            if (a == 0x0)
                e = '0';
            else if (a == 0x1)
                e = '1';
            else if (a == 0x2)
                e = '2';
            else if (a == 0x3)
                e = '3';
            else if (a == 0x4)
                e = '4';
            else if (a == 0x5)
                e = '5';
            else if (a == 0x6)
                e = '6';
            else if (a == 0x7)
                e = '7';
            else if (a == 0x8)
                e = '8';
            else if (a == 0x9)
                e = '9';
            else if (a == 0xA)
                e = 'A';
            else if (a == 0xB)
                e = 'B';
            else if (a == 0xC)
                e = 'C';
            else if (a == 0xD)
                e = 'D';
            else if (a == 0xE)
                e = 'E';
            else if (a == 0xF)
                e = 'F';
            return e;
        }
        public static int Extract(string Path)
        {
            int Begin = 0xA0000;
            byte[] memblock = File.ReadAllBytes(Path);
            int Address = memblock[Begin] + (memblock[Begin + 1] << 8) + 0xA0000;
            List<string> Phrases = new List<string>();
            List<char> Phrase = new List<char>();
            int a = 0;
            while (Begin < 0xABFFF)
            {
                List<int> Save = new List<int>();
                while (((memblock[Begin] != 0xFF) && (memblock[Begin + 1] != 0xFF)) || ((memblock[Begin] == 0xFF) && (memblock[Begin - 1] == 0x00) && (memblock[Begin + 1] != 0xFF)))
                {
                    Phrase = new List<char>();
                    if ((memblock[Begin] == 0x5A) && (memblock[Begin + 1] == 0x63))
                        Begin += 0x34;
                    if (Begin == 0xA8ED5)
                        Begin = 0xA8F22;
                    if (Begin == 0xA772A)
                        Begin = 0xA8034;
                    Address = memblock[Begin] + 0xA0000 + (memblock[Begin + 1] << 8) + (0x4000 * (((Begin-0xA0000) / 0x4000)-1));
                    a = 0;
                    if ((Address < 0xABFFF) && (Address > Begin))
                    {
                        char e = ConvertHexToChar((Begin >> 12) & 0xF);
                        Phrase.Add('{');
                        Phrase.Add(e);
                        e = ConvertHexToChar((Begin >> 8) & 0xF);
                        Phrase.Add(e);
                        e = ConvertHexToChar((Begin >> 4) & 0xF);
                        Phrase.Add(e);
                        e = ConvertHexToChar(Begin & 0xF);
                        Phrase.Add(e);
                        Phrase.Add('}');
                        Phrase.Add(':');
                        while ((memblock[Address + a] != 0xFF) && (memblock[Address + a] != 0x80))
                        {
                            if ((memblock[Address + a] != 0x5B) && (memblock[Address + a] != 0x9C))
                            {
                                Phrase.Add((char)memblock[Address + a]);
                                a++;
                            }
                            else if (memblock[Address + a] == 0x5B)
                            {
                                Phrase.Add((char)memblock[Address + a]);
                                Phrase.Add('E');
                                Phrase.Add('F');
                                Phrase.Add('F');
                                Phrase.Add('0');
                                Phrase.Add((char)memblock[Address + a + 1]);
                                Phrase.Add(']');
                                a += 2;
                            }
                            else
                            {
                                Phrase.Add('[');
                                Phrase.Add('N');
                                Phrase.Add('U');
                                Phrase.Add('M');
                                Phrase.Add('0');
                                Phrase.Add(ConvertHexToChar(memblock[Address + a + 1]));
                                Phrase.Add(']');
                                a += 3;
                            }
                        }
                        if (memblock[Address + a] == 0xFF)
                        {
                            Phrase.Add('[');
                            Phrase.Add('E');
                            Phrase.Add('N');
                            Phrase.Add('D');
                            Phrase.Add('F');
                            Phrase.Add('F');
                            Phrase.Add(']');
                        }
                        else
                        {
                            Phrase.Add('[');
                            Phrase.Add('E');
                            Phrase.Add('N');
                            Phrase.Add('D');
                            Phrase.Add('8');
                            Phrase.Add('0');
                            Phrase.Add(']');
                        }
                        Phrases.Add(string.Concat(Phrase));
                    }
                    if (Address == Begin + 2)
                    {
                        if (memblock[Address + a] == 0xFF)
                            Begin = Address + a + 2;
                        else
                            Begin = Address + a + 1;
                    }
                    else
                    {
                        Begin += 2;
                        if (((memblock[Begin] == 0x29) && ((memblock[Begin - 3] == 0x2A) || (memblock[Begin - 3] == 0x29) || ((memblock[Begin + 3] == 0x2A) || (memblock[Begin + 3] == 0x29)))) || ((memblock[Begin] == 0x2A) && ((memblock[Begin - 3] == 0x2A) || (memblock[Begin - 3] == 0x29) || ((memblock[Begin + 3] == 0x2A) || (memblock[Begin + 3] == 0x29)))))
                            Begin++;
                        if (Address >= Begin + 2)
                            Save.Add(Address);
                        int maxSave = 0;
                        for (int u = 0; u < Save.Count(); u++)
                            if ((maxSave < Save[u]) && ((memblock[Save[u]] != 0xFF) && (memblock[Save[u]] != 0x80)))
                                maxSave = Save[u];
                        for (int u = 0; u < Save.Count(); u++)
                            if (Save[u] == Begin)
                            {
                                a = 0;
                                while ((memblock[maxSave + a] != 0xFF) && (memblock[maxSave + a] != 0x80))
                                {
                                    a++;
                                }
                                if (memblock[maxSave + a] == 0x80)
                                    Begin = maxSave + a + 1;
                                else
                                    Begin = maxSave + a + 2;
                                u = Save.Count;
                            }
                    }
                }
                Begin++;
            }
            File.WriteAllLines("textBank.txt", Phrases);
            return 0;
        }
    }
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}