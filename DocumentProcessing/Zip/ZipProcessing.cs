using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Zip;

namespace DocumentProcessing {
    public class ZipProcessing : IZipProcessing {
        public ZipProcessing() {
        }
        public void CreateZip(string zipFileName, string[] zipArchiveFiles) {
            using (Stream stream = File.Open(zipFileName, FileMode.Create)) {
                CreateZip(stream, zipFileName, zipArchiveFiles);
            }
        }
        public void CreateZip(Stream stream, string zipFileName, string[] zipArchiveFiles) {
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
        public void CreateZip(string zipFileName, Dictionary<string, Stream> zipArchiveFiles) {
            CreateZip(zipFileName, zipArchiveFiles, entryNameEncoding: null, compressionSettings: null, encryptionSettings: null);
        }
        public void CreateZip(string zipFileName, Dictionary<string, Stream> zipArchiveFiles, Encoding? entryNameEncoding, CompressionSettings? compressionSettings, EncryptionSettings? encryptionSettings) {
            using (Stream stream = File.Open(zipFileName, FileMode.Create)) {
                using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create, false, entryNameEncoding, compressionSettings, encryptionSettings)) {
                    foreach (var file in zipArchiveFiles) {
                        ZipArchiveEntry entry;
                        using (entry = archive.CreateEntry(file.Key)) {
                            using (Stream entryStream = entry.Open()) {
                                file.Value.CopyTo(entryStream);
                            }
                        }
                    }
                }
                ProcessStartInfo psi = new ProcessStartInfo(){
                    FileName = zipFileName,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }
    }
}
