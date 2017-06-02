using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JudyDou.Helper
{
    public static class UploadHelper
    {
        public static string ValidUploadFile(HttpPostedFileBase file)
        {
            string error = null;

            if (file == null || file.ContentLength == 0)
            {
                error = "Please select a file.";
            }
            else
            {
                string ext = Path.GetExtension(file.FileName).ToLower();

                if (!Constants.AcceptedImageTypes.Contains(ext))
                {
                    error = "File type not supported.";
                }
            }

            return error;
        }

        public static string UploadFile(string path, HttpPostedFileBase file, string name = null)
        {
            string filename = string.IsNullOrEmpty(name) ? Path.GetFileName(file.FileName) : name + Path.GetExtension(file.FileName);

            Directory.CreateDirectory(path);
            file.SaveAs(path + "/" + filename);

            return path + "/" + filename;
        }
    }
}
