﻿using System;
using System.Collections.Generic;
using System.Text;

namespace bioscoop_app.Model
{
    public abstract class DataType
    {
        public int id;

        public abstract override bool Equals(object other);
    }
}
