// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git
open Fake.ReleaseNotesHelper
open Fake.UserInputHelper
open Fake.DocFxHelper
open System
open System.IO

// --------------------------------------------------------------------------------------
// START TODO: Provide project-specific details below
// --------------------------------------------------------------------------------------

// Information about the project are used
//  - for version and project name in generated AssemblyInfo file
//  - by the generated NuGet package
//  - to run tests and to publish documentation on GitHub gh-pages
//  - for documentation, you also need to edit info in "docs/tools/generate.fsx"

// The name of the project
// (used by attributes in AssemblyInfo, name of a NuGet package and directory in 'src')
let project = "TheVoid"

// Pattern specifying assemblies to be tested using NUnit
let testProject = sprintf "%s.Tests" project

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "Represents a type for the void keyword."

// Longer description of the project
// (used as a description for NuGet package; line breaks are automatically cleaned up)
let description = "Represents a type for the void keyword. Licensed under the MIT License (http://opensource.org/licenses/MIT)."

// List of author names (for NuGet package)
let authors = [ "Stefan Reichel" ]

// Tags for your project (for NuGet package)
let tags = "unit void nothing return"


// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitOwner = "bomret"
let gitHome = "https://github.com/" + gitOwner

// The name of the project on GitHub
let gitName = "Unit"

// The url for the raw files hosted
let gitRaw = environVarOrDefault "gitRaw" "https://raw.github.com/bomret"

// --------------------------------------------------------------------------------------
// END TODO: The rest of the file includes standard build steps
// --------------------------------------------------------------------------------------

// Read additional information from the release notes document
let release = LoadReleaseNotes "RELEASE_NOTES.md"

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" <| fun _ ->
  CleanDirs
    [ "bin"
      "temp" ]

Target "CleanDocs" <| fun _ ->
  CleanDirs
    [ "docs" @@ "_site"
      "docs" @@ "_api" ]

// --------------------------------------------------------------------------------------
// Build library & test project

Target "Restore" <| fun _ ->
  DotNetCli.Restore id

// --------------------------------------------------------------------------------------
// Build library & test project

Target "Build" <| fun _ ->
  !! ("**" @@ "project.json")
  |> Seq.iter (fun proj ->
       DotNetCli.Build <| fun p ->
         { p with
             Configuration = "Release"
             Project = proj })

// --------------------------------------------------------------------------------------
// Run the unit tests using test runner

Target "RunTests" <| fun _ ->
  !! ("tests" @@ "**" @@ "project.json")
  |> Seq.iter (fun proj ->
       DotNetCli.Test <| fun p ->
         { p with
             Configuration = "Release"
             Project = proj })

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "Pack" <| fun _ ->
  DotNetCli.Pack <| fun p ->
    { p with
        Project = "src" @@ project
        OutputPath = "bin" }

Target "Publish" <| fun _ ->
  let apiKey =
    match getBuildParam "nugetkey" with
    | s when not (String.IsNullOrWhiteSpace s) -> s
    | _ -> getUserInput "NuGet ApiKey: "

  Paket.Push <| fun p ->
    { p with
        ApiKey = apiKey
        WorkingDir = "bin" }

// --------------------------------------------------------------------------------------
// Generate the documentation

Target "GenerateDocs" <| fun _ ->
  DocFx id

Target "ServeDocs" <| fun _ ->
  DocFx <| fun p ->
    { p with
        Serve = true }

// --------------------------------------------------------------------------------------
// Release Scripts

Target "ReleaseDocs" <| fun _ ->
  let tempDocsDir = "temp/gh-pages"
  CleanDir tempDocsDir
  Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

  CopyRecursive ("docs" @@ "_site") tempDocsDir true |> tracefn "%A"
  StageAll tempDocsDir
  Git.Commit.Commit tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
  Branches.push tempDocsDir

#load "paket-files/fsharp/FAKE/modules/Octokit/Octokit.fsx"
open Octokit

Target "Release" <| fun _ ->
  let user =
    match getBuildParam "github-user" with
    | s when not (String.IsNullOrWhiteSpace s) -> s
    | _ -> getUserInput "Username: "
  let pw =
    match getBuildParam "github-pw" with
    | s when not (String.IsNullOrWhiteSpace s) -> s
    | _ -> getUserPassword "Password: "
  let remote =
    Git.CommandHelper.getGitResult "" "remote -v"
    |> Seq.filter (fun (s: string) -> s.EndsWith("(push)"))
    |> Seq.tryFind (fun (s: string) -> s.Contains(gitOwner + "/" + gitName))
    |> function None -> gitHome + "/" + gitName | Some (s: string) -> s.Split().[0]

  StageAll ""
  Git.Commit.Commit "" (sprintf "Bump version to %s" release.NugetVersion)
  Branches.pushBranch "" remote (Information.getBranchName "")

  Branches.tag "" release.NugetVersion
  Branches.pushTag "" remote release.NugetVersion

  // release on github
  createClient user pw
  |> createDraft gitOwner gitName release.NugetVersion (release.SemVer.PreRelease <> None) release.Notes
  |> releaseDraft
  |> Async.RunSynchronously

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean"
  ==> "Restore"
  ==> "Build"
  ==> "RunTests"
  ==> "Pack"
  ==> "All"

"CleanDocs"
  ==> "GenerateDocs"

"CleanDocs"
  ==> "ServeDocs"

"Pack"
  ==> "Publish"

"Pack"
  ==> "Release"

RunTargetOrDefault "All"
