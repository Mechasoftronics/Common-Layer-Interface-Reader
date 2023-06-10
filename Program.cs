// See https://aka.ms/new-console-template for more information


using System.Text;

Console.WriteLine("Reading .CLI file");
Console.WriteLine();

string fileToRead = "F:\\Learning C#\\ReadAFile\\TestBinaryMini.cli";
string fileToWrite = "F:\\Learning C#\\ReadAFile\\TextFileOut.txt";

StreamReader sr = new(fileToRead);
ReadingFile.ReadHeader(sr, fileToRead);
//ReadingFile.ReadFileData(sr);


public class ReadingFile
{
    //int headerLength = 0;

    public static void ReadHeader(StreamReader sr, string fileToRead)
    {
        String? line = sr.ReadLine();
        int headerLength = 0;

        //Continue to read and parse the text until you reach the 'HEADEREND'.
        while (line != null)
        {
            String[] words = line.Split('\r', '\n');

            ParsingHeader.ParsingText(words, ref headerLength);

            // USERDATA is a section of data in HTML format.
            if (line.Contains("USERDATA"))
            {
                HTMLParsing.HTMLParse(words, ref headerLength);
            }

            // HEADEREND signifies the end of the ASCII/HTML section of the file
            // and the start of the binary data section.
            else if (line.Contains("HEADEREND"))
            {
                BinaryParser.BinaryParsing(fileToRead, ref headerLength);
            }
            line = sr.ReadLine();
        }

        sr.Close();
        Console.WriteLine();
    }
}

public class ParsingHeader
{
    public static void ParsingText(string[] line, ref int headerLength)
    {
        if (line[0].StartsWith("//") & line[0].EndsWith("//"))
        {
            string tmp = line[0];
            headerLength += line[0].Length;

            line[0] = line[0].Replace("//", "");
            Console.WriteLine(line[0]);
        }

        if (line[0].Contains("HEADERSTART"))
        {
            headerLength += line[0].Length;
        }

        if (line[0].Contains("BINARY"))
        {
            Console.WriteLine("File is: Binary");
            headerLength += line[0].Length;
        }

        if (line[0].Contains("UNITS"))
        {
            Console.WriteLine($"Units are: ({line[0].Substring(line[0].IndexOf("/") + 1)} * mm) in mm.");
            headerLength += line[0].Length;
        }
        if (line[0].Contains("VERSION"))
        {
            Console.WriteLine($"Version is: {line[0].Substring(line[0].IndexOf("/") + 1)}");
            headerLength += line[0].Length;
        }
        if (line[0].Contains("LABEL"))
        {
            Console.WriteLine($"3D file name: {line[0].Substring(line[0].IndexOf("/") + 1)}");
            headerLength += line[0].Length;
        }
        if (line[0].Contains("DATE"))
        {
            Console.WriteLine($"Date file created: {line[0].Substring(line[0].IndexOf("/") + 1)}");
            headerLength += line[0].Length;
        }
        if (line[0].Contains("LAYERS"))
        {
            Console.WriteLine($"Number of layers: {line[0].Substring(line[0].IndexOf("/") + 1)}");
            Console.WriteLine();
            headerLength += line[0].Length;
        }
        if (line[0].Contains("DIMENSION"))
        {
            Console.WriteLine($"Object print box dimensions: {line[0].Substring(line[0].IndexOf("/") + 1)}");
            headerLength += line[0].Length;
        }
        if (line[0].Contains("USERDATA"))
        {
            headerLength += line[0].Length;

            Console.WriteLine($"Build Data:");

            // Finds all words after '$$USERDATA/'.
            string[] buildData = { line[0].Substring(line[0].IndexOf("/") + 1) };

            headerLength += line[0].IndexOf("/") + 1;

            // Converts the buildData array of words into a continous string.
            string text = buildData[0];

            // Set a delimiter (array) to extract comma delimited words from the string.
            char[] delimiterChars = { ',' };

            // Places each extracted word(s) into an array element.
            string[] parsedData = text.Split(delimiterChars);


            for (int i = 0; i < parsedData.Length; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine($"Slicer Application: {parsedData[i]}");
                }
                else if (i == 1)
                {
                    Console.WriteLine($"Length of HTML section: {parsedData[i]}");
                }
                else if (i > 2)
                {
                    Console.WriteLine($"More data is available in the header. Review .CLI file in a text editer");
                    break;
                }
            }
        }
    }

}
public class HTMLParsing
{
    // this method parses numberics from the HTML strings, and builds the numbers for display. 
    public static void HTMLParse(String[] words, ref int headerLength)
    {
        string word = words[0];
        string line;
        string cNum;
        int foundFirst;
        int foundLast;
        int charCount;
        int length;

        // Used to build a complete number from the HTML data.
        // Then display that number.
        StringBuilder HTMLNumber = new StringBuilder();
        do
        {
            if (word.Contains("<LI"))
            {
                foundFirst = word.IndexOf("<LI>");

                foundLast = word.IndexOf("</LI>");

                charCount = foundLast - foundFirst;

                line = word.Substring(foundFirst, charCount);
                word = word.Remove(0, foundLast + 5);
                length = word.Length;

                foreach (char value in line)
                {
                    cNum = Convert.ToString(value);

                    switch (cNum)
                    {
                        case "0":
                            HTMLNumber.Append(cNum);
                            break;
                        case "1":
                            HTMLNumber.Append(cNum);
                            break;
                        case "2":
                            HTMLNumber.Append(cNum);
                            break;
                        case "3":
                            HTMLNumber.Append(cNum);
                            break;
                        case "4":
                            HTMLNumber.Append(cNum);
                            break;
                        case "5":
                            HTMLNumber.Append(cNum);
                            break;
                        case "6":
                            HTMLNumber.Append(cNum);
                            break;
                        case "7":
                            HTMLNumber.Append(cNum);
                            break;
                        case "8":
                            HTMLNumber.Append(cNum);
                            break;
                        case "9":
                            HTMLNumber.Append(cNum);
                            break;
                        case ".":
                            HTMLNumber.Append(cNum);
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine(HTMLNumber);
                HTMLNumber.Clear();
            }
        } while (word.Length > 5);
        Console.WriteLine();
    }

}


public class BinaryParser
{
    public static void BinaryParsing(String fileToRead, ref int headerLength)
    {
        int count = 0;
        int startOfHex = headerLength + 20;

        try
        {
            using (FileStream fsSource = new FileStream(fileToRead, FileMode.Open, FileAccess.Read))
            {

                // Read the source file into a byte array.
                byte[] bytes = new byte[fsSource.Length - headerLength];
                int numBytesToRead = (int)fsSource.Length - headerLength;

                Console.WriteLine($"File seek status: {fsSource.CanSeek}");


                fsSource.Seek(startOfHex, 0);

                Console.WriteLine(fsSource.Position);

                int n = fsSource.Read(bytes, 0, numBytesToRead);

                //while (numBytesToRead > 0)
                //{
                //    // Read may return anything from 0 to numBytesToRead.
                //    fsSource.Position = fileStreamPosition;
                //    int n = fsSource.Read(bytes, 0, numBytesToRead);


                //    Console.WriteLine($"Total number of binary bytes read: {n}");


                //    // Break when the end of the file is reached.
                //    if (n == 0)
                //        break;

                //    startIndex += n;
                //    numBytesToRead -= n;
                //}
                //numBytesToRead = bytes.Length;


                using (MemoryStream memStream = new MemoryStream(bytes.Length))
                {
                    memStream.Seek(0, SeekOrigin.Begin);
                    while (count < memStream.Length)
                    {
                        bytes[count++] = (byte)memStream.ReadByte();
                    }
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Binary read finished.");
        }


        //public static void BinaryParsing(string[] binaryValues)
        //{
        //    string binaries = binaryValues[0];

        //    if (binaries.Contains("$$HEADEREND"))
        //    {
        //        binaries = binaries.Remove(0, "$$HEADEREND".Length);
        //    }

        //    byte[] bytes = new byte[binaries.Length];

        //    bytes = Encoding.ASCII.GetBytes(binaries);

        //    int index = 0;
        //    int arrayLocation = 0;

        //    foreach (byte singleByte in bytes)
        //    {
        //        //Convert the number expressed in base 16 to an integer.
        //        int value = Convert.ToInt32(singleByte);

        //        // Get the character corresponding to the integral value. 
        //        Console.WriteLine(String.Format("{0,10:X}", singleByte));

        //        if (singleByte == 127)
        //        {
        //            Console.WriteLine($"Command: Start Layer LONG ( {singleByte} )");

        //            var layerData = new LayerData();
        //            ref int returnIndex = ref layerData.CI127(bytes, ref index);
        //            index = returnIndex;
        //        }
        //        else if (value == 128)
        //        {
        //            Console.WriteLine($"Command: Start Layer SHORT ( {singleByte} )");
        //        }
        //        else if (value == 129)
        //        {
        //            Console.WriteLine($"Command: Start PolyLine SHORT ( {singleByte} )");
        //        }
        //        else if (value == 130)
        //        {
        //            Console.WriteLine($"Command: Start PolyLine LONG ( {singleByte} )");
        //        }
        //        else if (value == 131)
        //        {
        //            Console.WriteLine($"Command: Start Hatches SHORT ( {singleByte} )");
        //        }
        //        else if (value == 132)
        //        {
        //            Console.WriteLine($"Command: Start Hatches LONG ( {singleByte} )");
        //        }
        //        arrayLocation++;

        //    }
        //}
    }

    public class LayerData
    {
        public ref int CI127(byte[] bytes, ref int index)
        {
            byte[] floatHexValues = new byte[4];
            index += 2;
            const string formatter = "{0,5}{1,17}{2,18:G10}{3,18:G10}";

            float fValue = BitConverter.ToSingle(bytes, index);
            Console.WriteLine(formatter, index, BitConverter.ToString(bytes, index, 4), fValue, fValue * 0.005);
            index += 4;
            return ref index;
        }

        public static void CI128(byte[] bytes)
        {
            byte[] arrayByte = new byte[4];
            int byteIndex = 0;

            for (int arrayIndex = 2; arrayIndex < 6; arrayIndex++)
            {
                byte singleByte = Convert.ToByte(bytes[arrayIndex]);

                arrayByte[byteIndex] = singleByte;
                byteIndex++;

            }
            const string formatter = "{0,5}{1,17}{2,18:E10}";
            float value = BitConverter.ToSingle(arrayByte, 0);
            Console.WriteLine(formatter, 0, BitConverter.ToString(bytes, 0, 4), value);

        }

    }
    public class FileWriter
    {
        [STAThread]
        static void WriteToFile()
        {
            Int64 x;
            try
            {
                //Open the File
                StreamWriter sw = new StreamWriter("C:\\Test1.txt", true, Encoding.ASCII);
                //Writeout the numbers 1 to 10 on the same line.
                for (x = 0; x < 10; x++)
                {
                    sw.Write(x);
                }
                //close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
    }
}


