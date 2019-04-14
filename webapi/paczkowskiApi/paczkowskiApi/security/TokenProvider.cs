﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paczkowskiApi.security
{
    public static class TokenProvider
    {
        private static Random _random = new Random((int)(DateTime.Now.Ticks * 0.5));

        public static string NewAuthToken =>
            Guid.NewGuid()
            .ToString()
            .Replace("-", _random.Next(10).ToString())
            .Replace(_random.Next(10).ToString(), "-")
            .Replace(_random.Next(10).ToString(), Convert.ToChar(_random.Next(128)).ToString());
    }
}