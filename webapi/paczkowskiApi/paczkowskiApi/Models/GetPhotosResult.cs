﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbContract.Entities;

namespace paczkowskiApi.Models
{
    public class GetPhotosResult
    {
        public GetPhotosResult(Photo photo)
        {
            PhotoNum = photo.PhotoNum;
            Base64Image = Convert.ToBase64String(photo.Image);
            Category = photo.Category;
            DisplayName = photo.DisplayName;
            FileName = photo.FileName;
        }

        public string PhotoNum { get; set; }
        public string Base64Image { get; set; }
        public string Category { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
    }
}