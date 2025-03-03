using Market.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Interfaces.Provider
{
    public interface IFileImportProvider
    {
        IFileService? GetService(string key);
    }
}
