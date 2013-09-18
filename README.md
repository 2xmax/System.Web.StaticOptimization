System.Web.StaticOptimization
=============================

The MSBuild tasks package contains utilities for JS, CSS and HTML minification without server-side participation in release, HTML preprocessing. It uses [WebGreace config]((https://www.nuget.org/packages/System.Web.StaticOptimization/) for bundles definition.

Quick start for ASP.NET MVC developers
======================================

Manual for developers who use cshtml, i.e. not static HTML. 
If you have no plans to use static HTML you can look for [alternative](http://yuicompressor.codeplex.com/), but IMHO the definition of the package is less convienment that [WebGrease] (http://webgrease.codeplex.com/wikipage?title=WebGrease%20configuration%20file)

1. Install [System.Web.StaticOptimization package](https://www.nuget.org/packages/System.Web.StaticOptimization/) from Nuget (if you have probs, here is the [help](https://nuget.org/packages/NLapack/1.0.14/Download)).
2. Add reference to System.Web.StaticOptimization.dll, System.Web.StaticOptimization.Mvc.dll (will be automated later in special NuGet package for ASP.NET MVC developers)
3. Add following initialization code in Global.asax.cs:


```csharp
        protected void Application_Start()
        {
            ...
#if DEBUG
            BundleTable.Init(System.Web.HttpContext.Current.Server.MapPath("~/bundles.config"));
#endif
        }
```
4. Edit bundles.config file by describing your JS and CSS bundles

How to use HTML preprocessing
=============================

For example, we have {filename}.template.html for template files and we want to see {filename}.html preprocessed:

index.temlate.html:

```html
<!DOCTYPE html>
<html ng-app="homepage">
<head>
    <!--style:~/Content/css/common.css-->
</head>
<body>
    <!--template:Views/Shared/_Header.html-->
    <div class="container-fluid">
        <error></error>
        <busyindicator></busyindicator>
        <div ng-view jq-show-effect="!(isLoading || isError)"></div>
    </div>
    <!--template:Views/Shared/_Settings.html-->
    <!--script:~/bundles/common.js-->
</body>
</html>
```  

We expect to generate index.html with substitution of JS, CSS bundles and html templates.

Let we write in csproj or your external MSBuild file:

```xml
  <!--Task import-->
  <UsingTask AssemblyFile="$(StaticOptimizationLib)" TaskName="System.Web.StaticOptimization.HtmlMinifierTask" />
  <Target Name="AfterBuild">
    <ItemGroup>
      <!--output HTML replace rules goes here. This example means that it will create
      the output file with the same name as input, but with replaced '.template.html'
      occurrence to '.html'. E.g.: index.template.html -> index.html -->
      <OutputFileNameReplaceRule Include=".template.html">
        <ReplaceBy>.html</ReplaceBy>
      </OutputFileNameReplaceRule>
    </ItemGroup>
    <PropertyGroup>
      <BundleConfig>$(MSBuildProjectDirectory)\bundles.config</BundleConfig>
    </PropertyGroup>
    <!-- Process all *template.html files with concrete bundles config and using release mode for rendering of bundles-->
    <HtmlPreprocessingTask OutputFileNameReplaceRules="@(OutputFileNameReplaceRule)" InputFile="*template.html" BundleConfig="$(BundleConfig)" IsRelease="True" />
    <!--Here you can place html minificator (template.html is ignored by default -->
    <!--<HtmlMinifierTask InputDir="$(MSBuildProjectDirectory)" />-->
  </Target>
```  

Press F6;)


Plans
=====
1. Prepare demo for SPA (using HTML preprocessing engine)
2. Automate Global.asax generation
3. Detailed configuration for each task
