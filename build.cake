#tool nuget:?package=Google.Protobuf.Tools&version=3.6.1
#addin nuget:?package=Cake.Warp&version=0.2.0

var target = Argument("target", "Default");
var configuration = "Release";
var publishDir = MakeAbsolute(Directory("./publish")).FullPath;


Task("Default")    
	.IsDependentOn("Proto")
	.IsDependentOn("Clean")
    .IsDependentOn("Restore")    
	.IsDependentOn("Build")
	.IsDependentOn("Test");
	

Task("Publish")    
	.IsDependentOn("Default")   
	.IsDependentOn("PublishNuget") 
	.IsDependentOn("PublishLegacy")
	.IsDependentOn("PublishClient")
	.IsDependentOn("PublishApi")	
	.IsDependentOn("PublishSpecs");
    

Task("Proto")
    .Does(() =>
    {
        var executable = GetFiles("./tools/**/windows_x64/protoc.exe").First().FullPath;
        var protoTools = System.IO.Directory.GetParent(executable).Parent.FullName;

        foreach(var file in GetFiles("specifications/**/*.proto"))
        {            
            var protoPaths = new List<string>();
            protoPaths.Add(protoTools);
            protoPaths.Add(System.IO.Path.GetDirectoryName(file.FullPath));
            
            var argsProtoPath = string.Join(" ", protoPaths.Distinct().Select(x => string.Format("--proto_path={0}", x)));
            var argsOut = string.Format("--csharp_out={0}", System.IO.Path.GetDirectoryName(file.FullPath));
            var argsOpt = " --csharp_opt=file_extension=.g.cs ";

            StartProcess(executable, new ProcessSettings {
                Arguments = new ProcessArgumentBuilder()
                    .Append(argsProtoPath)
                    .Append(argsOut)
                    .Append(argsOpt)
                    .Append(file.FullPath)
                });
        }
    });


Task("Clean")
  .Does(()=>{   
    var directoriesToClean = GetDirectories("./**/bin/Debug")
      .Union(GetDirectories("./**/bin/Release"))     
      .Union(GetDirectories(publishDir));
      
    CleanDirectories(directoriesToClean);
  });

Task("Restore")
    .Does(() => 
    {
        DotNetCoreRestore();
    });

Task("Build")
    .Does(() => 
    {	
		Func<IFileSystemInfo, bool> exclude_legacy =
			fileSystemInfo => !fileSystemInfo.Path.FullPath.EndsWith(".Legacy", StringComparison.OrdinalIgnoreCase) 
				&& !fileSystemInfo.Path.FullPath.EndsWith(".Specs", StringComparison.OrdinalIgnoreCase);


		var files = GetFiles("./**/*.csproj", exclude_legacy);
		foreach (var file in files)
		{
			Information(file);
			DotNetCoreBuild(file.FullPath, new DotNetCoreBuildSettings 
			{
				Configuration = configuration			
			});
		}

		var msBuildSettings = new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		ToolVersion = MSBuildToolVersion.VS2019,
    		Configuration = configuration
    	};

		MSBuild("./src/Vodamep.Legacy/Vodamep.Legacy.csproj",msBuildSettings);
		MSBuild("./tests/Vodamep.Specs/Vodamep.Specs.csproj",msBuildSettings);
		MSBuild("./tests/Vodamep.Hkpv.Specs/Vodamep.Hkpv.Specs.csproj",msBuildSettings);
		MSBuild("./tests/Vodamep.Agp.Specs/Vodamep.Agp.Specs.csproj",msBuildSettings);
		MSBuild("./tests/Vodamep.Mkkp.Specs/Vodamep.Mkkp.Specs.csproj",msBuildSettings);
		
    });


Task("Test")	
    .Does(() => 
    {
		var settings = new DotNetCoreTestSettings
		{
			Configuration = "Release",
			NoBuild = true			
		};
		
		// return;
		
        foreach(var file in GetFiles("./tests/**/*.csproj")) 
		{
			Information("{0}", file);
			
			DotNetCoreTest(file.FullPath, settings);
		}  
        
    });

Task("PublishLegacy")	
	.Does(() =>
	{
		EnsureDirectoryExists(publishDir);
		CleanDirectory(publishDir + "/dml");
		if (FileExists(publishDir + "dml.zip"))
		{
			DeleteFile(publishDir + "dml.zip");
		}

		var msBuildSettings = new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		ToolVersion = MSBuildToolVersion.VS2019,
    		Configuration = configuration
    	};

		MSBuild("./src/Vodamep.Legacy/Vodamep.Legacy.csproj",msBuildSettings.WithProperty("OutDir", publishDir + "/dml"));
	
		Zip(publishDir + "/dml", publishDir + "/dml.zip", publishDir + "/dml/dml.exe");
	});

Task("PublishClient")	
	.Does(() =>
	{		
		
		CleanDirectory(publishDir + "/dmc");
		if (FileExists(publishDir + "/dmc.zip"))
		{
			DeleteFile(publishDir + "/dmc.zip");
		}

		EnsureDirectoryExists(publishDir + "/dmc/dmc_warp/");

		var ms = new DotNetCoreMSBuildSettings();

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dmc",
			MSBuildSettings = ms,
			Runtime = "win-x64",
			SelfContained = true
		};			
		
		DotNetCorePublish("./src/Vodamep.Client/Vodamep.Client.csproj", settings); 

		Warp(publishDir+ "/dmc",
			 "dmc.exe", 
			publishDir+ "/dmc/dmc_warp/dmc.exe",
			WarpPlatforms.WindowsX64
			);

		Zip(publishDir + "/dmc/dmc_warp", publishDir + "/dmc.zip");
	});

Task("PublishApi")	
	.Does(() =>
	{	
		
		CleanDirectory(publishDir + "/dms");
		if (FileExists(publishDir + "/dms.zip"))
		{
			DeleteFile(publishDir + "/dms.zip");
		}

		EnsureDirectoryExists(publishDir + "/dms/dms_warp/");

		var ms = new DotNetCoreMSBuildSettings();

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dms",
			MSBuildSettings = ms,
			Runtime = "win-x64",
			SelfContained = true
		};	
		
		DotNetCorePublish("./src/Vodamep.Api/Vodamep.Api.csproj", settings); 
		
		Warp(publishDir+ "/dms",
			 "dms.exe", 
			publishDir+ "/dms/dms_warp/dms.exe",
			WarpPlatforms.WindowsX64
			);

		var files = new [] {						
			publishDir + "/dms/web.config",
			publishDir + "/dms/appsettings.json",
			publishDir + "/dms/nlog.config",
		};

		CopyFiles(files, publishDir + "/dms/dms_warp");

		Zip(publishDir + "/dms/dms_warp", publishDir + "/dms.zip");
	});

Task("PublishSpecs")	
	.Does(() =>
	{
		EnsureDirectoryExists(publishDir);
		
		if (FileExists(publishDir + "/specifications.zip"))
		{
			DeleteFile(publishDir + "/specifications.zip");
		}

		Zip("./specifications", publishDir + "/specifications.zip");
	});

Task("PublishNuget")
	.Does(() =>
	{
		var version = EnvironmentVariable("vodamepversion") ?? "0.0.1";

		var ms = new DotNetCoreMSBuildSettings();

		var settings = new DotNetCorePackSettings
		{
			Configuration = "Release",
			OutputDirectory = publishDir,
			MSBuildSettings = ms,
		};

		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("Version", version);

		DotNetCorePack("./src/Vodamep/Vodamep.csproj", settings);
	});

RunTarget(target);


