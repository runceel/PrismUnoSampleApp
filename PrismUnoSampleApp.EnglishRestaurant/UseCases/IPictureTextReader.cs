﻿using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public interface IPictureTextReader
    {
        Task<(bool ok, DetectedText[] result)> ReadTextsAsync(byte[] value);
    }
}
