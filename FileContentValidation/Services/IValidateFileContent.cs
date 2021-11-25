using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileContentValidation.Services
{
    public interface IValidateFileContent
    {
        string UploadAndValidateFile(IFormFile file);
    }
}