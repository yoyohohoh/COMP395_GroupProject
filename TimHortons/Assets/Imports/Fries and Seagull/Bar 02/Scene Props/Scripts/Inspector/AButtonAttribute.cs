﻿using System;

namespace Seagull.Bar_02.Inspector {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AButtonAttribute : Attribute {
        public string text { get; set; } = null;
        
        public AButtonAttribute(string buttonText) {
            text = buttonText;
        }
    }
}