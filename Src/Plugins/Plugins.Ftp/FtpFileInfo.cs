namespace Plugins.Ftp
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Xml.Serialization;

	internal sealed class FtpFileInfo
	{
		[XmlAttribute]
		public string Flags { get; set; }

		[XmlAttribute]
		public string Owner { get; set; }

		[XmlAttribute]
		public string Group { get; set; }

		[XmlAttribute]
		public bool IsDirectory { get; set; }

		[XmlAttribute]
		public DateTime Created { get; set; }

		[XmlAttribute]
		public string FileName { get; set; }

		public static IEnumerable<FtpFileInfo> Parse(string ftpResponse)
		{
			var splitterString = ftpResponse.Contains("\r\n") ? "\r\n" : "\n"; // win or *nix string separator

			var responseLines = ftpResponse.Split(new[] { splitterString }, StringSplitOptions.RemoveEmptyEntries);

			return Parse(responseLines);
		}

		private static IEnumerable<FtpFileInfo> Parse(IEnumerable<string> responseLines)
		{
			var myListArray = new List<FtpFileInfo>();

			responseLines = responseLines.ToArray();

			var directoryListStyle = GuessFileListStyle(responseLines);

			foreach (var s in responseLines)
			{
				if (directoryListStyle == FtpServerType.Unknown || s == "") continue;

				var f = new FtpFileInfo {FileName = ".."};

				switch (directoryListStyle)
				{
					case FtpServerType.Unix:
						f = ParseFileStructFromUnixStyleRecord(s);
						break;
					case FtpServerType.Windows:
						f = ParseFileStructFromWindowsStyleRecord(s);
						break;
				}

				if (!(f.FileName == "." || f.FileName == ".."))
				{
					myListArray.Add(f);
				}
			}
			return myListArray;
		}

		const string UnixFileInfoRegexp = "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)";

		const string WindowsFileInfoRegexp = "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]";

		private static FtpServerType GuessFileListStyle(IEnumerable<string> recordList)
		{
			foreach (var s in recordList)
			{
				if (s.Length > 10 && Regex.IsMatch(s.Substring(0, 10), UnixFileInfoRegexp))
				{
					return FtpServerType.Unix;
				}

				if (s.Length > 8 && Regex.IsMatch(s.Substring(0, 8), WindowsFileInfoRegexp))
				{
					return FtpServerType.Windows;
				}
			}

			return FtpServerType.Unknown;
		}

		private static FtpFileInfo ParseFileStructFromUnixStyleRecord(string record)
		{
			var f = new FtpFileInfo();

			var processstr = record.Trim();

			f.Flags = processstr.Substring(0, 9);

			f.IsDirectory = (f.Flags[0] == 'd');

			processstr = (processstr.Substring(11)).Trim();

			CutSubstringFromStringWithTrim(ref processstr, ' ', 0);

			f.Owner = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
			f.Group = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
			CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
			f.Created = DateTime.Parse(CutSubstringFromStringWithTrim(ref processstr, ' ', 8));
			f.FileName = processstr;

			return f;
		}

		private static string CutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
		{
			var pos1 = s.IndexOf(c, startIndex);

			var retString = s.Substring(0, pos1);

			s = (s.Substring(pos1)).Trim();

			return retString;
		}

		private static FtpFileInfo ParseFileStructFromWindowsStyleRecord(string record)
		{
			var f = new FtpFileInfo();

			var processstr = record.Trim();
			var dateStr = processstr.Substring(0, 8);
			processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
			var timeStr = processstr.Substring(0, 7);
			processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();

			f.Created = DateTime.Parse(dateStr + " " + timeStr, CultureInfo.InvariantCulture);

			if (processstr.Substring(0, 5) == "<DIR>")
			{
				f.IsDirectory = true;
				processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
			}
			else
			{
				processstr = processstr.Remove(0, processstr.IndexOf(' ') + 1);

				f.IsDirectory = false;
			}

			f.FileName = processstr;

			return f;
		}
	}
}