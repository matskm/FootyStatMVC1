using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootyStatMVC1.Models.FootyStat.Init
{
    // Configuration class for xml input implementation
    class XmlConfig : BaseConfig
    {
        // Meta data xsd file
        public string xsdFilename { get; private set; }

        // xsd element/component names
        public string xsdFieldBlockName { get; private set; } //gameRowType
        public string xsdFieldName_displayStr { get; private set; } //displayStr
        public string xsdFieldName_descStr { get; private set; } //descStr

        // Actual data xml file
        public string xmlFilename { get; private set; }

        // xml element/component names:
        public string xmlRootName { get; private set; } //footyStat
        public string xmlRowName { get; private set; } //gameRow



        // Constructor
        public XmlConfig()
        {

            //string temp_str = AppDomain.CurrentDomain.BaseDirectory;
            //System.Diagnostics.Debug.WriteLine("BOBA FETT");

            //System.Diagnostics.Debug.WriteLine(temp_str);

            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location;

            string base_dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //string base_dir = AppDomain.CurrentDomain.BaseDirectory;

            // To work on local machine:
            string full_dir = base_dir + "\\App_Data";
            
            // To work on AppHarbor:
            //string full_dir = base_dir;

            xsdFilename = full_dir + "\\footyStat_xml_schema_v1.xsd";
            xsdFieldBlockName = "gameRowType";
            xsdFieldName_displayStr = "displayStr";
            xsdFieldName_descStr = "descStr";

            xmlFilename = full_dir + "\\sandbox_footyStat_v1.xml";
            xmlRootName = "footyStat";
            xmlRowName = "gameRow";


            

        }


    }
}
