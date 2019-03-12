namespace Common.FileSystem
{
	using JetBrains.Annotations;

	public interface IFileWriter
	{
		void Write(string folderPath, string fileName, byte[] content);

		[UsedImplicitly]
		void Write(string filePath, byte[] content);
	}
}