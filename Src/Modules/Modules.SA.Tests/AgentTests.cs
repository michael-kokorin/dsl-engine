namespace Modules.SA.Tests
{
	using System;
	using System.Diagnostics;
	using System.IO.Pipes;

	using NUnit.Framework;

	[TestFixture]
	[Ignore]
	public sealed class AgentTests
	{
		[Test]
		public void Test()
		{
			var id = "MyOwnPipe_" + Guid.NewGuid();
			var pipe = new NamedPipeServerStream(id);
			var info = new ProcessStartInfo
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = false,
				CreateNoWindow = false,
				FileName = @"g:\Src\SDL\Features\262067-update-ai-cores\Refs\ScanCores\java\jdk1.7.0\bin\java.exe",
				WorkingDirectory = @"g:\Src\SDL\Features\262067-update-ai-cores\Refs\ScanCores\java",
				Arguments = @" -server -jar poc.jar -cp g:\Src\SDL\Features\262067-update-ai-cores\Refs\ScanCores\java\poc.jar -t 810000 -u -appDirs G:\Garbage\java_14 G:\Garbage\java_14\Java\1 INPUT DATA VERIFICATION\10 Cross-site request forgery\java\MOPAS_1_CSRF_change_password.java -host localhost --site-root-url http://localhost/ --temp-dir C:\Users\mkokorin\AppData\Local\Application Inspector\temp\java\ffff -P " + id,
				UseShellExecute = false,
				WindowStyle = ProcessWindowStyle.Hidden
			};

			var process = new Process
			{
				StartInfo = info
			};

			process.Start();

			process.WaitForExit();
			Console.WriteLine(process.ExitCode);
			Console.WriteLine("Output:" + process.StandardOutput.ReadToEnd());
			Console.WriteLine("Error: " + process.StandardError.ReadToEnd());

			Console.ReadLine();

			/*Job = new PipedJob();
            Job.MessageRecived += TransportMessageRecived;
            Job.MessageTimeout = _timeout;
            Job.ModuleName = ModuleName;
            Job.PipeKey = "-p";
            Job.Start(info);
            OnStarted();

            Job?.Wait();*/
			/*     var info = new ProcessStartInfo()
			{
					UseShellExecute = true,
					RedirectStandardOutput = false,
					RedirectStandardError = false,
					RedirectStandardInput = false,
					CreateNoWindow = false,
					FileName = fileName,
					WorkingDirectory = workingPath,
					Arguments = arguments,
					WindowStyle = ProcessWindowStyle.Hidden
			};*/
		}
	}
}