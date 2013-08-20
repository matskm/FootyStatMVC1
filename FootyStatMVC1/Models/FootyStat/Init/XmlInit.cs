using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using System.Reflection;
using System.IO;

namespace FootyStatMVC1.Models.FootyStat.Init
{
    // Initialisation class based on xml data format
    // 
    class XmlInit : BaseInit
    {

        // Configuration class
        XmlConfig config;


        // NOTE: improvement for transparency is to pass these around the functions not have them as data members
        // Xmlschema containing the xsd info
        XmlSchema fsSchema;

        // Xdoc for xml file
        XDocument xdoc;

        public XmlInit()
        {
            config = new XmlConfig();
        }

        public void loadAndValidateXml()
        {
            xdoc = XDocument.Load(config.xmlFilename);

            // Validate the xml file against schema
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("http://www.w3.org/2001/XMLSchema",config.xsdFilename);

            // NOTE: no exception handling coded as yet in case xml file doesn't validate against schema.
            string msg = "";
            
            xdoc.Validate(schemas, (o, e) =>
            {
                msg = e.Message;
            },true); // last bool here turns on the saving of the SchemaInfo
            Console.WriteLine(msg == "" ? "xml input Document is valid" : "xml inpute Document invalid: " + msg);

            // Get the schema by itself
            fsSchema = null;
            foreach (XmlSchema schema in schemas.Schemas())
            {
                fsSchema = schema;
            }// foreach

            // Probably want a return value here.. (exceptions??)

        }

        public FieldDictionary fillFieldDictionary(XmlSchema schema, SnapView sv)
        {

            // Create a new FieldDictionary to fill:
            FieldDictionary return_dict = new FieldDictionary(10); // this is growable.

            // Iterate over the schema and process appinfo
            // NOTE: this code is totally unproteced from unhandled exceptions (To fix: 30/7/13)
            // The structure of this code totally depends on the form of the xsd file.
            // WARNING: If you change the xsd file - this code will become invalid (code weakness)
            foreach (XmlSchemaObject xso in schema.Items)
            {
                XmlSchemaComplexType xsa = xso as XmlSchemaComplexType;

                if (xsa != null)
                {
                    string comment =
                        //    ((XmlSchemaDocumentation)xsa.Items[0]).Markup[0].InnerText;
                    xsa.Name; // THIS is where we got to. Look at MSDN article "Traversing XML Schemas"
                    Console.WriteLine(comment);

                    // Only consider gameRowType block
                    if (xsa.Name == config.xsdFieldBlockName)
                    {
                        Console.WriteLine("Inside gameRowType if");


                        // Get the sequence particle of the complex type.
                        XmlSchemaSequence sequence = xsa.ContentTypeParticle as XmlSchemaSequence;


                        // Initial value of address for the fields (0)
                        int current_field_address = 0;

                        // Iterate over each XmlSchemaElement in the Items collection.
                        foreach (XmlSchemaElement childElement in sequence.Items)
                        {
                            Console.WriteLine("Element: {0}", childElement.Name);

                            string field_name = childElement.Name;

                            // Iterate over appinfo 
                            XmlSchemaAnnotation annotation = childElement.Annotation;

                            // Iterate over Items to find appinfo
                            foreach (XmlSchemaObject xso2 in annotation.Items)
                            {
                                XmlSchemaAppInfo ai = xso2 as XmlSchemaAppInfo;
                                Console.WriteLine("     Markup[0] displayStr : " + (ai.Markup[0]).Attributes[config.xsdFieldName_displayStr].InnerText);
                                Console.WriteLine("     Markup[0] descStr : " + (ai.Markup[0]).Attributes[config.xsdFieldName_descStr].InnerText);

                                string field_displayStr = (ai.Markup[0]).Attributes[config.xsdFieldName_displayStr].InnerText;
                                string field_descStr = (ai.Markup[0]).Attributes[config.xsdFieldName_descStr].InnerText;


                                // NOTE: we may want to construct the field outside this foreach
                                // Now we can construct the Field object and add it to the FieldDictionary
                                Field f = new Field(field_name, field_displayStr, field_descStr, current_field_address, sv);

                                // Increment field address 
                                current_field_address++;

                                // Add field to the dictionary
                                // NOTE: one of the few accesses to a SnapView data member.
                                return_dict.dict.Add(f);


                            }//foreach




                        }


                        // NEED some kind of protection for not finding anything (i.e., fails to find any entries at all)





                    }//if

                }//if
            }//foreach (loop over xsd top level elements)


            // Test the field dictionary by printing it out:
            Console.WriteLine("Test of the FieldDictionary");
            return_dict.print_me();

            return return_dict;

        }

        public void fillSnapViewTable(SnapView sv)
        {
            // LINQ query to return a list of rows
            var rowList = from row in xdoc.Descendants(config.xmlRootName).Descendants(config.xmlRowName)
                          select new
                          {
                              fieldList = row.Descendants()
                          };

            // Loop over the rows
            foreach (var row in rowList)
            {

                // Have to iterate over row twice: once to find its size, so 
                // the array can be created; second to fill the array.
                // This is because IEnumerable<T> doesn't provide a way of knowing
                // the number of elements in the collection without iterating over them all.

                int arr_size = 0;

                foreach (var field in row.fieldList)
                {
                    arr_size++;
                }//foreach

                // Create the string array
                string[] row_array = new string[arr_size];

                int arr_idx = 0;

                Console.WriteLine("Another row");
                // Loop over the fields in the row
                foreach (var field in row.fieldList)
                {


                    Console.WriteLine("  Another field: ");
                    Console.WriteLine(field.Value.ToString());

                    // Add string to array

                    row_array[arr_idx] = field.Value.ToString();
                    arr_idx++;


                }//foreach loop over fields in a row

                // Add row to the SnapView datastructure
                SVRow svr = new SVRow();
                svr.row = row_array;
                sv.addRow(svr);


            }//foreach loop over rows

            // Test snapview constructor by printing table

            sv.print_table();
        }

        // Main parent method which calls the other methods
        public void initSnapView(SnapView sv){
            // First load the xml from disk, and validate against the xsd
            loadAndValidateXml();

            // Then given the schema read in from the xsd, pull out the meta-data
            FieldDictionary dict = fillFieldDictionary(fsSchema,sv);

            // Set the meta-data for the SnapView
            sv.setDict(dict);

            // Finally, pull out the table from the xml file
            fillSnapViewTable(sv);
        }//method

    }//class
}//namespace
