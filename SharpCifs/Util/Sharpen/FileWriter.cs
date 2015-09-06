using System.IO;

namespace SharpCifs.Util.Sharpen
{
    internal class FileWriter : StreamWriter
    {
        public FileWriter(FilePath path) : base(new FileOutputStream(path))
        {
        }

        public FileWriter Append(string sequence)
        {
            Write(sequence);
            return this;
        }
    }
}
