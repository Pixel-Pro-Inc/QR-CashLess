using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ExcelService: _BaseService, IExcelService
    {

        private readonly string _rootPath;
        private readonly IFirebaseServices _IFirebaseServices;

        public ExcelService(IFirebaseServices firebaseServices)
        {
            _IFirebaseServices = firebaseServices;
        }


    }
}
