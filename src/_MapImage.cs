using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Steganography.Robot.Code;

namespace MapImage
{

    public class baidu_coord
    {
        public int error { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }

    public class _MapImage
    {

    }


    public class amap_coordinate
    {
        public string status { get; set; }
        public string info { get; set; }
        public string infocode { get; set; }
        public string locations { get; set; }
    }

    public class gmapRootobject
    {
        public Result[] results { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public Address_Components[] address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Northeast
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Southwest
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Viewport
    {
        public Northeast1 northeast { get; set; }
        public Southwest1 southwest { get; set; }
    }

    public class Northeast1
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Southwest1
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Address_Components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }


    public class amapRootobject
    {
        public string status { get; set; }
        public string info { get; set; }
        public string infocode { get; set; }
        public Regeocode regeocode { get; set; }
    }

    public class Regeocode
    {
        public string formatted_address { get; set; }
        public Addresscomponent addressComponent { get; set; }
    }

    public class Addresscomponent
    {
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string citycode { get; set; }
        public string district { get; set; }
        public string adcode { get; set; }
        public string township { get; set; }
        public string towncode { get; set; }
        public Neighborhood neighborhood { get; set; }
        public Building building { get; set; }
        public Streetnumber streetNumber { get; set; }
        public List<object> businessAreas { get; set; }
    }

    public class Neighborhood
    {
        public object name { get; set; }
        public object type { get; set; }
    }

    public class Building
    {
        public object name { get; set; }
        public object type { get; set; }
    }

    public class Streetnumber
    {
        public string street { get; set; }
        public string number { get; set; }
        public string location { get; set; }
        public string direction { get; set; }
        public string distance { get; set; }
    }

}
