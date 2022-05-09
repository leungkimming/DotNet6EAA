using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
//using Telerik.Windows.Zip.Extensions;
using Telerik.Zip;

namespace DocumentProcessing {
    public class ZipProcessing: IZipProcessing {
        public ZipProcessing() { 
        }
        public void CreateZip(string zipFileName, string[] zipArchiveFiles) {
            using (Stream stream = File.Open(zipFileName, FileMode.Create)) {
                CreateZip(stream, zipFileName, zipArchiveFiles);
            }
        }
        public void CreateZip(Stream stream, string zipFileName,string[] zipArchiveFiles) {
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create, false, null)) {
                foreach (string file in zipArchiveFiles) {
                    string sourceFileName = file;
                    string fileName = file.Split(new string[] { @"\" }, StringSplitOptions.None).Last();
                    ZipArchiveEntry entry;
                    using (entry = archive.CreateEntry(fileName)) {
                        using (Stream fileStream = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                            // Setting the ExternalAttributes property 
                            entry.ExternalAttributes = (int)File.GetAttributes(sourceFileName);
                            DateTime lastWriteTime = File.GetLastWriteTime(sourceFileName);
                            // Setting the LastWriteTime property 
                            entry.LastWriteTime = lastWriteTime;
                            using (Stream entryStream = entry.Open()) {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
            }
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = zipFileName,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        public void ExtractZip(string zipFilePath,string zipFileName, string destinationFolder) {
            //string zipFile = Path.Combine(zipFilePath, zipFileName);

            //if (Directory.Exists(destinationFolder)) {
            //    Directory.Delete(destinationFolder, recursive: true);
            //}

            //ZipFile.ExtractToDirectory(zipFile, destinationFolder);

            //Console.WriteLine("Listing files in: " + destinationFolder);
            //foreach (string fileName in Directory.EnumerateFiles(destinationFolder)) {
            //    Console.WriteLine(Path.GetFileName(fileName));
            //}

            //ProcessStartInfo psi = new ProcessStartInfo()
            //{
            //    FileName = destinationFolder,
            //    UseShellExecute = true
            //};
            //Process.Start(psi);
        }
    }
}
