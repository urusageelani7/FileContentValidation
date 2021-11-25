using FileContentValidation.Controllers;
using FileContentValidation.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Xunit.Sdk;

namespace FileContentValidationTest
{
    [TestClass]
    public class FileValidationTest
    {
        private static IWebHostEnvironment _environment;
        public FileValidationTest(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private IValidateFileContent _validateFileContent = new ValidateFileContent(_environment);
        [TestMethod]
        public void CheckFileValidation_EmptyFile()
        {
            var output = "";
            var expected = "The file is empty.";
            FileValidationController fileValidation = new FileValidationController(_validateFileContent);
            using (var fileStream = System.IO.File.OpenRead(_environment.WebRootPath + "\\Upload\\" + "Empty.txt"))
            {
                FormFile file = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(fileStream.Name));
                output = fileValidation.CheckFileValidation(file);
            }
            Assert.AreEqual(expected, output);
        }
        public void CheckFileValidation_NoFileSelected()
        {
            var output = "";
            var expected = "Please select a file.";
            FileValidationController fileValidation = new FileValidationController(_validateFileContent);
            output = fileValidation.CheckFileValidation(null);
            Assert.AreEqual(expected, output);
        }
        public void CheckFileValidation_WrongFileFormat()
        {
            var output = "";
            var expected = "Please select a valid file.";
            FileValidationController fileValidation = new FileValidationController(_validateFileContent);
            using (var fileStream = System.IO.File.OpenRead(_environment.WebRootPath + "\\Upload\\" + "Capture.jpg"))
            {
                FormFile file = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(fileStream.Name));
                output = fileValidation.CheckFileValidation(file);
            }
            Assert.AreEqual(expected, output);
        }
        public void CheckFileValidation_ValidFileContent()
        {
            var output = "";
            var expected = "{fileValid = true.}";
            FileValidationController fileValidation = new FileValidationController(_validateFileContent);
            using (var fileStream = System.IO.File.OpenRead(_environment.WebRootPath + "\\Upload\\" + "FileValidation.txt"))
            {
                FormFile file = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(fileStream.Name));
                output = fileValidation.CheckFileValidation(file);
            }
            Assert.AreEqual(expected, output);
        }
        public void CheckFileValidation_InvalidFileContent()
        {
            var output = "";
            var expected = "fileValid = false";
            FileValidationController fileValidation = new FileValidationController(_validateFileContent);
            using (var fileStream = System.IO.File.OpenRead(_environment.WebRootPath + "\\Upload\\" + "InvalidContent.txt"))
            {
                FormFile file = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(fileStream.Name));
                output = fileValidation.CheckFileValidation(file);
            }
            Assert.IsTrue(output.Contains(expected));
        }
    }
}