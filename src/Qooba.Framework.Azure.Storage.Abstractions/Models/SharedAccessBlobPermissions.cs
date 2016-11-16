﻿namespace Qooba.Framework.Azure.Storage.Abstractions.Models
{
    public enum SharedAccessBlobPermissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        List = 8,
        Add = 16,
        Create = 32
    }
}
