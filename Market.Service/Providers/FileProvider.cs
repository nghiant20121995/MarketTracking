using Market.Domain.Exceptions;
using Market.Domain.Interfaces.Provider;
using Market.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Market.Service.Providers
{
    public class FileImportProvider : IFileImportProvider
    {
        private readonly Dictionary<string, Type> _providers = new Dictionary<string, Type>()
        {
            ["csv"] = typeof(CSVFileService),
            ["xlsx"] = typeof(ExcelFileService),
            ["xls"] = typeof(ExcelFileService)
        };

        private readonly IServiceProvider _serviceProvider;
        public FileImportProvider(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IFileService GetService(string key)
        {
            if (_providers.TryGetValue(key, out var serviceType))
            {
                return _serviceProvider.GetServices<IFileService>().First(e => e.GetType().Equals(serviceType));
            }
            throw new ValidateException("The type of the imported file is not valid.");
        }
    }
}
