using System.IO;
using System.Text;

namespace SharpCifs.Util.Sharpen
{
    public class InputStreamReader : StreamReader
	{
		public InputStreamReader (InputStream s) : base(s.GetWrappedStream ())
		{
		}

		public InputStreamReader (InputStream s, string encoding) : base(s.GetWrappedStream (), Encoding.GetEncoding (encoding))
		{
		}

		public InputStreamReader (InputStream s, Encoding e) : base(s.GetWrappedStream (), e)
		{
		}
	}
}
