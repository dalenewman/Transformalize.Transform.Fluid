nuget pack Transformalize.Transform.Fluid.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Transform.Fluid.Autofac.nuspec -OutputDirectory "c:\temp\modules"

nuget push "c:\temp\modules\Transformalize.Transform.Fluid.0.10.2-beta.nupkg" -source https://www.myget.org/F/transformalize/api/v3/index.json
nuget push "c:\temp\modules\Transformalize.Transform.Fluid.Autofac.0.10.2-beta.nupkg" -source https://www.myget.org/F/transformalize/api/v3/index.json
