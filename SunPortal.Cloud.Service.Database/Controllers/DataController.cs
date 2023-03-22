using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Service.Database.Services;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DataController:Controller
{
    private readonly DatabaseService _databaseService;

    public DataController(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }



}