#addin nuget:?package=Cake.Warp&version=0.4.0

var target = Argument("target", "Default");
var configuration = "Release";
var publishDir = MakeAbsolute(Directory("./publish")).FullPath;


Task("Default")
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


Task("Clean")
  .Does(()=>{   

  // nur zum aufräumen, kann dann wieder weg
    var oldProtoFiles = GetFiles("./specifications/**/*.g.cs");
	DeleteFiles(oldProtoFiles);


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
		var files = GetFiles("./**/*.csproj");
		foreach (var file in files)
		{
			Information(file);
			DotNetCoreBuild(file.FullPath, new DotNetCoreBuildSettings 
			{
				Configuration = configuration			
			});
		}
    });


Task("Test")	
    .Does(() => 
    {
		var settings = new DotNetCoreTestSettings
		{
			Configuration = "Release",
			NoBuild = true			
		};
		
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
		
		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dml",			
			Runtime = "win-x64",
			SelfContained = true
		};			
		
		DotNetCorePublish("./src/Vodamep.Legacy/Vodamep.Legacy.csproj", settings); 

		
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

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dmc",			
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

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dms",			
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


