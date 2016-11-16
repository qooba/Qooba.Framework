﻿namespace Qooba.Framework.Cqrs.Abstractions
{
    public class CommandResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}