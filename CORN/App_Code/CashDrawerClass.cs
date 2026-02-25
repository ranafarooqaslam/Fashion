using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.PointOfService;

    /// <summary>
    /// Summary description for CashDrawerClass
    /// </summary>
    public class CashDrawerClass
    {
        private readonly CashDrawer _myCashDrawer;

        public CashDrawerClass()
        {
        var explorer = new PosExplorer();
        DeviceInfo device = explorer.GetDevice(System.Configuration.ConfigurationManager.AppSettings["CashDrawer"]);

        _myCashDrawer = (CashDrawer)explorer.CreateInstance(device);
    }

    public void OpenCashDrawer()
        {

        _myCashDrawer.Open();
        _myCashDrawer.Claim(1000);
        _myCashDrawer.DeviceEnabled = true;
        _myCashDrawer.OpenDrawer();
        _myCashDrawer.DeviceEnabled = false;
        _myCashDrawer.Release();
        _myCashDrawer.Close();
    }
    }
