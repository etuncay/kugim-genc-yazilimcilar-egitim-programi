﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haber.Data
{
    public class CacheEntity : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public long ExpirationTime { get; set; }
    }
}
