using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImgMetadata
{

    public class ImageMetadata
    {
        public string Exif_Make { get; set; } = "";
        public string Exif_Model { get; set; } = "";
        public string Exif_Software { get; set; } = "";
        public string Exif_OriginalDateTime { get; set; } = "";
        public string GPS_TimeStamp { get; set; } = "";
        public string GPS_LatLon { get; set; } = "";
        public string GPS_LonLat { get; set; } = "";
        public string GPS_Latitude { get; set; } = "";
        public string GPS_Longitude { get; set; } = "";
        public string GPS_LatitudeD { get; set; } = "";
        public string GPS_LongitudeD { get; set; } = "";
        public string GPS_Altitude { get; set; } = "";
    }
    public class _ImgMetadata
    {
        public ImageMetadata GetLatLonString(Stream image)
        {

            var readers = new IJpegSegmentMetadataReader[] { new ExifReader(), new IptcReader() };
            var directories = JpegMetadataReader.ReadMetadata(image, readers);
            ImageMetadata imd = new ImageMetadata();
            Regex regex = new Regex(@"^([\-\d\.]+)\D+([\-\d\.]+)\D+([\-\d\.]+)");

            foreach (var dis in directories)
            {
                foreach (var diss in dis.Tags)
                {
                    if (diss.Name == "Make")
                    {
                        imd.Exif_Make = diss.Description;
                    }
                    if (diss.Name == "Model")
                    {
                        imd.Exif_Model = diss.Description;
                    }
                    if (diss.Name == "Software")
                    {
                        imd.Exif_Software = diss.Description;
                    }
                    if (diss.Name == "Date/Time Original")
                    {
                        imd.Exif_OriginalDateTime = diss.Description;
                    }

                    if (diss.Name == "GPS Time-Stamp")
                    {
                        imd.GPS_TimeStamp = diss.Description;
                    }
                    if (diss.Name == "GPS Latitude")
                    {
                        imd.GPS_Latitude = diss.Description;

                        Match match1 = regex.Match(imd.GPS_Latitude);
                        if (match1.Success & match1.Groups.Count == 4)
                        {
                            imd.GPS_LatitudeD = DMSToDD(double.Parse(match1.Groups[1].Value), double.Parse(match1.Groups[2].Value), double.Parse(match1.Groups[3].Value)).ToString();
                        }
                    }
                    if (diss.Name == "GPS Longitude")
                    {
                        imd.GPS_Longitude = diss.Description;

                        Match match1 = regex.Match(imd.GPS_Longitude);
                        if (match1.Success & match1.Groups.Count == 4)
                        {
                            imd.GPS_LongitudeD = DMSToDD(double.Parse(match1.Groups[1].Value), double.Parse(match1.Groups[2].Value), double.Parse(match1.Groups[3].Value)).ToString();
                        }
                    }
                    if (diss.Name == "GPS Altitude")
                    {
                        imd.GPS_Altitude = diss.Description;
                    }

                }
            }

            if (imd.GPS_Latitude != "" & imd.GPS_Longitude != "")
            {


                Match match1 = regex.Match(imd.GPS_Latitude);
                Match match2 = regex.Match(imd.GPS_Longitude);
                if (match1.Success & match2.Success & match1.Groups.Count == 4 & match2.Groups.Count == 4)
                {
                    imd.GPS_LatLon = DMSToDD(double.Parse(match1.Groups[1].Value), double.Parse(match1.Groups[2].Value), double.Parse(match1.Groups[3].Value)) + "," +
                        DMSToDD(double.Parse(match2.Groups[1].Value), double.Parse(match2.Groups[2].Value), double.Parse(match2.Groups[3].Value));

                    imd.GPS_LonLat = DMSToDD(double.Parse(match2.Groups[1].Value), double.Parse(match2.Groups[2].Value), double.Parse(match2.Groups[3].Value)) + "," +
                        DMSToDD(double.Parse(match1.Groups[1].Value), double.Parse(match1.Groups[2].Value), double.Parse(match1.Groups[3].Value));
                }
            }

            return imd;
        }

        double DMSToDD(double degrees, double minutes, double seconds)
        {
            if (degrees > 0)
            {
                return degrees + (minutes / 60) + (seconds / 3600);
            }
            else
            {
                return -((degrees * -1) + (minutes / 60) + (seconds / 3600));
            }
        }

    }
}
