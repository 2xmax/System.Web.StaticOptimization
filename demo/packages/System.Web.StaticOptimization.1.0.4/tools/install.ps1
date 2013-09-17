param($installPath, $toolsPath, $package, $project)

$project.ProjectItems.Item("bundles.config").Properties.Item("CopyToOutputDirectory").Value = 2
$project.Save()

#decided to use xml directly due not every dev can have the MSBuild lib
function Append-Property($doc, $namespace, $propertyGroup, $propertyName, $value)
{
    $property = $doc.CreateElement($propertyName, $namespace)
    $property.AppendChild($doc.CreateTextNode($value))
    $propertyGroup.AppendChild($property)
}

$doc = New-Object System.Xml.XmlDocument
$doc.Load($project.FullName)
$namespace = 'http://schemas.microsoft.com/developer/msbuild/2003'

#relative path calc
$dllAbsPath = Join-Path $toolsPath "System.Web.StaticOptimization.Build.dll"
$dllAbsUri = New-Object -typename System.Uri -argumentlist $dllAbsPath
$projectUri = New-Object -typename System.Uri -argumentlist $project.FullName
$dllRelUri = $projectUri.MakeRelativeUri($dllAbsUri)
$dllRelPath = [System.URI]::UnescapeDataString($dllRelUri.ToString()).Replace([System.IO.Path]::AltDirectorySeparatorChar, [System.IO.Path]::DirectorySeparatorChar)

#prop group
$propertyGroup = $doc.CreateElement('PropertyGroup', $namespace)
Append-Property $doc $namespace $propertyGroup 'StaticOptimizationLib' $dllRelPath
$doc.Project.AppendChild($propertyGroup)

#task import
$taskImport = $doc.CreateElement('UsingTask', $namespace)
$taskImport.SetAttribute('AssemblyFile', '$(StaticOptimizationLib)')
$taskImport.SetAttribute('TaskName', 'System.Web.StaticOptimization.BundleGeneratorTask')
$doc.Project.AppendChild($taskImport)

#target
$target = $doc.CreateElement('Target', $namespace)
$target.SetAttribute('Name', 'ProcessStaticContent')
$target.SetAttribute('AfterTargets', 'AfterBuild')

$taskInvoke = $doc.CreateElement('BundleGeneratorTask', $namespace)
$taskInvoke.SetAttribute('BundleConfig', "bundles.config")
$target.AppendChild($taskInvoke)
    
$doc.Project.AppendChild($target)

$doc.Save($project.FullName)