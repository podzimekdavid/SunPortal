using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Service.Database.Services;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DataController:Controller
{
    private readonly DevicesService _devicesService;

    public DataController(DevicesService devicesService)
    {
        _devicesService = devicesService;
    }




}