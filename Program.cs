//Written for games by JOYCITY
//FreeStyleFootball https://steamcommunity.com/app/568810/
//FreestyleFootball R https://store.steampowered.com/app/1826980
//Freestyle 2 Street Basketball https://store.steampowered.com/app/339610/
namespace JOYCITY_Extractor
{
    internal class Program
    {
        public static BinaryReader br;
        static void Main(string[] args)
        {
            br = new(File.OpenRead(args[0]));

            if (new string(br.ReadChars(4)) != "PACK")
                throw new Exception("This is not a JOYCITY pak file.");

            int tableStart = br.ReadInt32();
            int tableSize = br.ReadInt32();

            br.BaseStream.Position = tableStart;//We're already there, but just in case.
            List<Subfile> subfiles = new();
            while (br.BaseStream.Position < tableStart + tableSize)
            {
                subfiles.Add(new());
            }

            string path = Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]) + "//";
            foreach (Subfile file in subfiles)
            {
                Directory.CreateDirectory(path + Path.GetDirectoryName(file.name));
                br.BaseStream.Position = file.start;
                BinaryWriter bw = new(File.Create(path + file.name));
                bw.Write(br.ReadBytes(file.size));
                bw.Close();
            }
        }

        class Subfile
        {
            public string name = new string(br.ReadChars(256)).TrimEnd('\0');
            public int start = br.ReadInt32();
            public int size = br.ReadInt32();
        }
    }
}
