using ASRS.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.Interfaces
{
    public interface IAsrsService
    {
        Task<(bool flag, string message)> ImportStoreReceiptData(ImportAsrsRequest importFileRequest);
        
    }
}
