using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileContentValidation.Services
{
    public class ValidateFileContent : IValidateFileContent
    {
        public static IWebHostEnvironment _environment;
        public ValidateFileContent(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string UploadAndValidateFile(IFormFile file)
        {
            try
            {
                int lineNumber = 0;
                List<string> invalidLineList = new List<string>();
                string jsonResponse = "";
                string uploadFolderPath = _environment.WebRootPath + "\\Upload\\";

                //check if user has selected a file
                if (file?.Length > 0)
                {
                    //check if the file is in the text format
                    if (Path.GetExtension(file.FileName).Equals(".txt"))
                    {
                        if (!Directory.Exists(uploadFolderPath))
                        {
                            Directory.CreateDirectory(uploadFolderPath);
                        }
                        //upload a file
                        using (FileStream fileStream = System.IO.File.Create(uploadFolderPath + file.FileName))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                            fileStream.Close();
                        }

                        //read the file line by line
                        foreach (string lineContent in System.IO.File.ReadLines(uploadFolderPath + file.FileName))
                        {
                            lineNumber++;
                            bool invalidAccName = false;
                            bool invalidAccNumber = false;
                            string firstName = lineContent?.Split(' ')?[0];
                            string accNumber = lineContent?.Split(' ')?[1];
                            Regex regexAccName = new Regex("^[A-Z][a-zA-Z]*$"); // First name should only consist of alphabetic characters, first letter should always be in uppercase
                            Regex regexAccNumber = new Regex("^[34][0-9]{6}[p]?$"); // Account number should be 7 digit number or 7 digit number + 'p' at the end and  must start with a digit 3 or 4
                            if (!regexAccName.IsMatch(firstName))
                                invalidAccName = true;
                            if (!regexAccNumber.IsMatch(accNumber))
                                invalidAccNumber = true;
                            if (invalidAccName && invalidAccNumber)
                                invalidLineList.Add("Account name, account number - not valid for " + lineNumber + " line '" + lineContent + "'");
                            else if (invalidAccName)
                                invalidLineList.Add("Account name - not valid for " + lineNumber + " line '" + lineContent + "'");
                            else if (invalidAccNumber)
                                invalidLineList.Add("Account number - not valid for " + lineNumber + " line '" + lineContent + "'");
                        }
                        if (invalidLineList.Count > 0)
                            jsonResponse = JsonConvert.SerializeObject(new { fileValid = false, invalidLines = invalidLineList }); //In case at least single line is not valid
                        else
                            jsonResponse = JsonConvert.SerializeObject(new { fileValid = true }); //In case of valid file
                        return jsonResponse;
                    }
                    else
                    {
                        return "Please select a valid file.";
                    }
                }
                else
                {
                    return "The file is empty.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}