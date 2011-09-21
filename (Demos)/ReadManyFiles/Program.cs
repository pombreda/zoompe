﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi.PE;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            var reader = new PEFile.Reader();

            var dllFiles = 
                (from f in EnumerateClrCoreDllFiles()
                where IsDotNet(f)
                select f).ToArray();

            MeasureManyFilesLoad(reader, dllFiles);
        }
    }

    private static void MeasureManyFilesLoad(PEFile.Reader reader, string[] dllFiles)
    {
        var start = DateTime.UtcNow;
        reader.PopulateSectionContent = false;
        foreach (var dll in dllFiles)
        {
            using (var dllStream = File.OpenRead(dll))
            {
                reader.ReadMetadata(dllStream);
            }
        }

        TimeSpan headersOnly = DateTime.UtcNow - start;

        start = DateTime.UtcNow;
        reader.PopulateSectionContent = true;
        foreach (var dll in dllFiles)
        {
            using (var dllStream = File.OpenRead(dll))
            {
                reader.ReadMetadata(dllStream);
            }
        }

        TimeSpan headersAndContent = DateTime.UtcNow - start;

        start = DateTime.UtcNow;
        foreach (var dll in dllFiles)
        {
            System.Reflection.Assembly.Load(File.ReadAllBytes(dll));
        }

        TimeSpan reflectionLoad = DateTime.UtcNow - start;

        Console.WriteLine(
            dllFiles.Length + " dlls\t" +
            "Headers only: " + headersOnly.TotalSeconds.ToString("#0.000") + " sec." +
            "  " +
            "Headers and content: " + headersAndContent.TotalSeconds.ToString("#0.000") + " sec." +
            "  " +
            "Reflection: " + reflectionLoad.TotalSeconds.ToString("#0.000") + " sec." +
            "");
    }

    static bool IsDotNet(string file)
    {
        try
        {
            System.Reflection.AssemblyName.GetAssemblyName(file);
            return true;
        }
        catch (BadImageFormatException)
        {
            return false;
        }
    }

    static IEnumerable<string> EnumerateClrCoreDllFiles()
    {
        return
            Directory.EnumerateFiles(
                Path.GetDirectoryName(typeof(int).Assembly.Location),
                "*.dll");
    }

    static IEnumerable<string> EnumerateWindowsDllExeSysFiles()
    {
        return
            from f in Directory.EnumerateFiles(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "*.*",
                SearchOption.AllDirectories)
            let ext = Path.GetExtension(f)
            where string.Equals(ext, ".dll")
            select f;
    }
}