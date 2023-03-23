using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Service.Database.Services;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DataController:Controller
{
    private readonly DevicesesService _devicesesService;

    public DataController(DevicesesService devicesesService)
    {
        _devicesesService = devicesesService;
    }




}