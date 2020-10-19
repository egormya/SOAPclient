using System;
using System.Xml;

namespace WpfApp1.Controller
{
    public class Curs
    {
        public static string ExtractCurrencyRate()
        {
            XmlTextReader reader = new XmlTextReader("http://www.cbr.ru/scripts/XML_daily.asp");
            string EuroXML = "";
            string USDXml = "";
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "Valute")
                        {
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "ID")
                                    {
                                        if (reader.Value == "R01235")
                                        {
                                            reader.MoveToElement();
                                            USDXml = reader.ReadOuterXml();
                                        }
                                    }
                                    if (reader.Name == "ID")
                                    {
                                        if (reader.Value == "R01239")
                                        {
                                            reader.MoveToElement();
                                            EuroXML = reader.ReadOuterXml();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            XmlDocument usdXmlDocument = new XmlDocument();
            usdXmlDocument.LoadXml(USDXml);
            XmlDocument euroXmlDocument = new XmlDocument();
            euroXmlDocument.LoadXml(EuroXML);
            XmlNode xmlNode = usdXmlDocument.SelectSingleNode("Valute/Value");
            string usdValue = xmlNode.InnerText;
            return usdValue;
            xmlNode = euroXmlDocument.SelectSingleNode("Valute/Value");
            decimal euroValue = Convert.ToDecimal(xmlNode.InnerText);
        }
    }
}
