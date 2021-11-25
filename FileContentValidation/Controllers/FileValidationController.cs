using FileContentValidation.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileContentValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileValidationController : ControllerBase
    {
        private readonly IValidateFileContent _validateFileContent;

        public FileValidationController(IValidateFileContent validateFileContent)
        {
            this._validateFileContent = validateFileContent;
        }
        [HttpPost]
        [Route("checkfilevalidation")]
        public string CheckFileValidation(IFormFile file)
        {
            if (file == null)
                return "Please select a file.";
            return _validateFileContent.UploadAndValidateFile(file);
        }
    }
}