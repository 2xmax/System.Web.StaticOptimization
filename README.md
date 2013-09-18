System.Web.StaticOptimization
=============================

The MSBuild tasks package contains utilities for JS, CSS and HTML minification without server-side participation in release, HTML preprocessing. It uses [WebGreace config](https://www.nuget.org/packages/System.Web.StaticOptimization/) for bundles definition.

Dependencies:

1. [Microsoft Ajax](http://ajaxmin.codeplex.com/) for JS and CSS minification.
2. [Zeta HTML Compressor](http://blog.magerquark.de/c-port-of-googles-htmlcompressor-library/) - a .NET Port of [Google's HTML compresor](https://code.google.com/p/htmlcompressor/)

Quick start for ASP.NET MVC developers
======================================

Manual for developers who use cshtml, i.e. not static HTML. 
If you have no plans to use static HTML you can look for [alternative](http://yuicompressor.codeplex.com/), but IMHO the definition of the package is less convienment that [WebGrease] (http://webgrease.codeplex.com/wikipage?title=WebGrease%20configuration%20file).

1. Install [System.Web.StaticOptimization package](https://www.nuget.org/packages/System.Web.StaticOptimization/) from Nuget (if you have probs, here is the [help](https://www.nuget.org/packages/System.Web.StaticOptimization/Download)).
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
4. Edit bundles.config file by describing your JS and CSS bundles.
5. Fasten the belts and press F6 (rebuild) ;).

How it works? The package adds MSBuild post build tasks in your csproj. Simple and Easy.
Have problems? Check out my demos.

How to use HTML preprocessing
=============================

For example, we have {filename}.template.html for template files and we want to get {filename}.html preprocessed:

index.template.html:

```html
<!DOCTYPE html>
<html ng-app="homepage">
<head>
    <!--style:~/Content/css/common.css-->
</head>
<body>
    <div class="container-fluid">
        <error></error>
        <busyindicator></busyindicator>
        <div ng-view jq-show-effect="!(isLoading || isError)"></div>
    </div>
    <!--template:Views/Shared/templates.html-->
    <!--script:~/bundles/common.js-->
</body>
</html>
```  

bundles.config:
```xml

<?xml version="1.0" encoding="utf-8"?>
<WebGrease>
  <CssFileSet name="~/Content/css/common.css">
    <Input>~/Content/css/bootstrap.css</Input>
    <!--+ over 9000 styles
    ...
    -->
  </CssFileSet>
  <JsFileSet name="~/bundles/common.js">
    <Input>~/Scripts/libs/jquery-1.9.1.js</Input>
    <!-- + 100500 scripts
    ...
    -->
  </JsFileSet>
</WebGrease>

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
3. Detailed documentation for extra input params of each task
