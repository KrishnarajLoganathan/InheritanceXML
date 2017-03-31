using POCConfigReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

/*
interface iComplexType
{
    iComplexType getValue();

}

class simpleType : iComplexType { String LeafValue;

    public iComplexType getValue()
    {
        throw new NotImplementedException();
    }
}
class ComplexTypeDef : iComplexType { public List<ComplexType> LeafValues { get; set; }

    public iComplexType getValue()
    {
        throw new NotImplementedException();
    }
}

public class ComplexType
{
    public String LeafKey { get; set; }


    public iComplexType LeafValue { get; set; }

    public ComplexType getValue (string key)
    {
        ComplexType value=null;

        return value;

    }

    public ComplexType ComplexValue { get; set; }

    public static void main (string[] args)
    {
        ComplexType c = new ComplexType()
        {
            LeafKey = "BackgroundColor",
            LeafValue = "Black"
        };

        ComplexType c1 = new ComplexType()
        {
            LeafKey = "Font"
        };
        c1.LeafValues.Add(new ComplexType()
        {
            LeafKey = "Color",
            LeafValue = "Black"
        });
        c1.LeafValues.Add(new ComplexType()
        {
            LeafKey = "size",
            LeafValue = "10"
        });

        Console.WriteLine(c.ToString());
    }
}
*/

namespace ConfigCollection
{

    public class XmlConfig
    {
        public XDocument ConfigDoc { get; protected internal set; }
        public List<NestedCollection> ConfigData { get; protected internal set; }

        public XDocument SetXmlDoc(string documentName)
        {
            documentName = @"D:\Sapient\POCAdobeForms\POCAdobeForms\dataStore.xml";
            ConfigDoc = XDocument.Load(documentName);

            return ConfigDoc;
        }

        public IEnumerable<XElement> GetChildElements()
        {
            return ConfigDoc != null ? (ConfigDoc.Root.HasElements ? ConfigDoc.Root.Elements() : null) : null;
        }

        public XmlConfig()
        {
            SetXmlDoc("");
            PopulateXml();
            var child = this.GetChildElements();

            Console.WriteLine(ConfigDoc);//ConfigDoc.Root.Elements().First().Name
        }

        /*public int GetEnumName(string elementname)
        {
            Levels enumValue = (Levels)Enum.Parse(typeof(Levels), elementname);
            return (int)enumValue;
            //return Enum.GetNames(typeof(Levels)).Where(w => w == elementname).FirstOrDefault();
            //foreach(Levels level in Enum.GetValues(typeof(Levels)).Cast<Levels>())
            //{}
        }*/

        public List<NestedCollection> PopulateXml()
        {
            /*this.ConfigData = this.GetChildElements().LoadNestedItems(i => i.HasElements == false, c => c.Name.LocalName.ToLower() == "config"
                , "Name");
                */
            if (ConfigDoc.Root.HasElements)
            {
                var result = new Dictionary<string, List<KeyValueItem>>();
                ConfigDoc.Root.GetFamily(c => c.Name.LocalName.ToLower() == "config", result);
            }

            Console.Write("");
            List<KeyValueLevel> searchList = new List<KeyValueLevel>()
            {
                new KeyValueLevel()
                {
                    Level =1,
                    MatchKey = new List<KeyValueItem>()
                    {
                       new KeyValueItem()
                       {
                            Key="ElementType",
                            Value="AllTemplates"
                       }
                    }
                },
                new KeyValueLevel()
                {
                    Level =2,
                    MatchKey = new List<KeyValueItem>()
                    {
                       new KeyValueItem()
                       {
                            Key="ElementType",
                            Value="Template"
                       },
                       new KeyValueItem()
                       {
                            Key="AttributeName",
                            Value="105"
                       }
                    }
                },
                new KeyValueLevel()
                {
                    Level =3,
                    MatchKey = new List<KeyValueItem>()
                    {
                       new KeyValueItem()
                       {
                            Key="ElementType",
                            Value="BlockType"
                       },
                       new KeyValueItem()
                       {
                            Key="AttributeName",
                            Value="abc"
                       }
                    }
                },
                new KeyValueLevel()
                {
                    Level =4,
                    MatchKey = new List<KeyValueItem>()
                    {
                       new KeyValueItem()
                       {
                            Key="ElementType",
                            Value="BlockInstance"
                       },
                       new KeyValueItem()
                       {
                            Key="AttributeName",
                            Value="2"
                       }
                    }
                }
            };

            /*TreeKeyValueItem<KeyValueItem> myTreeRoot = new TreeKeyValueItem<KeyValueItem>();// (new KeyValueItem(){Key="a", Value="1"});

            var first = myTreeRoot.AddChild(new KeyValueItem() { Key = "b", Value = "2" });
            var second = myTreeRoot.AddChild(new KeyValueItem() { Key = "c", Value = "3" });
            var grandChild = first.AddChild(new KeyValueItem() { Key = "d", Value = "4" })
                .AddChild(new KeyValueItem(){ Key = "a", Value = "1"});
                */

            List<KeyValueItem> finalList = new List<KeyValueItem>();
            //finalList.AddRange(
                ConfigData.ForEach(c=> c.CheckConfigKeyValue(searchList, finalList));

            //var data = this.ConfigData.FindAll(w => w.CheckConfigKeyValue(searchList));
            return this.ConfigData;
        }
    }



    public static class Ex
    {
        public static bool CheckConfigKeyValue(this NestedCollection source
                , List<KeyValueLevel> searchList, List<KeyValueItem> finalList)
        {
            int cnt = 0;
            foreach (var kv in searchList.OrderByDescending(o => o.Level))
            {
                cnt = source.ElementOrAttributeName.Count();
                foreach (var item in source.ElementOrAttributeName)
                {
                    cnt-= kv.MatchKey.Any(a=> a.Key.Equals(item.Key) && a.Value.Equals(item.Value))? 1:0;
                }
                if(cnt==0)
                {
                    finalList.AddRange(item.Items);
                }
            }
            if(source.NestedItems.Any())
            {
                source.NestedItems.ForEach(c => c.CheckConfigKeyValue(searchList, finalList));
            }
            return true;
        }

        public static bool CheckKeyValue(this KeyValueItem source
                , string keyName, string valueName)
        {
            if (source.Key == keyName && source.Value== valueName)
                return true;
            else
                return false;
        }
        public static IEnumerable<KeyValueItem> GetNestedKeyValueItem(this IEnumerable<XElement> source)
        {
            List<KeyValueItem> items = new List<KeyValueItem>();
            foreach (XElement element in source)
            {
                items.Add(new KeyValueItem()
                {
                    Key = element.Name.LocalName,
                    Value = element.HasElements? element.Elements(): null // element.Value ?? element.Value
                });
            }
            return items;
        }
        public static IEnumerable<KeyValueItem> GetKeyValueItem(this IEnumerable<XElement> source)
        {
            List<KeyValueItem> items = new List<KeyValueItem>();
            foreach (XElement element in source)
            {
                items.Add(new KeyValueItem()
                {
                    Key = element.Name.LocalName,
                    Value = element.Value ?? element.Value
                });
            }
            return items;
        }
        public static int GetEnumValue(this string elementname)
        {
            if (Enum.GetNames(typeof(Levels)).Any(w => w == elementname))
            {
                Levels enumValue = (Levels)Enum.Parse(typeof(Levels), elementname);
                return (int)enumValue;
            }
            else
            {
                return 0;
            }

        }

        /*public static IEnumerable<KeyValueItem> PopulateConfigItems(this IEnumerable<XElement> source
                , Func<XElement, bool> configPredicate, Func<XElement, bool> itemPredicate)
        {
            //if (source.Any())
            //return source.TakeWhile(itemPredicate).GetKeyValueItem().Concat(
            //source.SkipWhile(itemPredicate).GetNestedKeyValueItem().PopulateConfigItems(configPredicate, itemPredicate));

            return default(IEnumerable<KeyValueItem>);
        }*/
        public static List<KeyValueItem> PopulateConfigTree(this IEnumerable<XElement> elements, Func<XElement, bool> configPredicate)
        {
            var result = new List<KeyValueItem>();
            foreach (var element in elements)
            {
                var children = element.HasElements? PopulateConfigTree(element.Elements(), configPredicate) : null;
                if (children != null && children.Count() != 0)
                {
                    result.Add(new KeyValueItem()
                    {
                        Key = element.Name.LocalName,
                        Value = children
                    });
                }
                else
                {
                    result.Add(new KeyValueItem()
                    {
                        Key = element.Name.LocalName,
                        Value = element.Value
                    });
                }
            }
            return result;
        }

        public static void GetFamily(this XElement source, Func<XElement, bool> configPredicate
            , Dictionary<string, List<KeyValueItem>> configList)
        {
            string key = "";
            foreach(var item in source.Elements())
            {
                if(item.Elements().Any(configPredicate))
                {
                    key = "Element:"+item.Name.LocalName 
                        + ((item.Attributes().Any(a=> a.Name.LocalName.ToLower() == "name")==true) ? ";Attribute:" +item.Attributes().Where(w => w.Name.LocalName.ToLower() == "name").FirstOrDefault().Value.ToString():"");
                    configList.Add(key, item.Elements().Where(configPredicate).Elements()
                                    .PopulateConfigTree(configPredicate));
                }
                else
                {
                    item.GetFamily(configPredicate, configList);
                }
            }

            /*
                .SelectMany(p => GetFamily(p, configPredicate))
                      .Concat(new XElement[] { source });
                      */
        }

        public static List<NestedCollection> LoadNestedItems(this IEnumerable<XElement> source, Func<XElement, bool> itemPredicate
            , Func<XElement, bool> configPredicate, string attributeName)
        {

            
            return source.Select(x => new NestedCollection()
            {
                ElementOrAttributeName = x.GetElementAndAttributeName(attributeName),
                Items = x.Elements().Where(configPredicate).Elements().PopulateConfigTree(configPredicate), // x.Elements().SkipConfigAndGetChildern(configPredicate).PopulateConfigTree()
                Level = x.Name.LocalName.GetEnumValue(),// x.Level
                NestedItems = LoadNestedItems(x.Elements().SkipConfigAndGetChildern(configPredicate).SkipWhile(itemPredicate), itemPredicate, configPredicate, attributeName)
            }).ToList();
            

            /*return source.Select(x => new NestedCollection()
            {
                ElementOrAttributeName = x.GetElementAndAttributeName(attributeName),
                Items = x.Elements().Where(configPredicate).Elements().PopulateConfigTree(configPredicate), // x.Elements().SkipConfigAndGetChildern(configPredicate).PopulateConfigTree()
                Level = x.Name.LocalName.GetEnumValue(),// x.Level
                NestedItems = LoadNestedItems(x.Elements().SkipConfigAndGetChildern(configPredicate).SkipWhile(itemPredicate), itemPredicate, configPredicate, attributeName)
            }).ToList();
            */
        }

        public static IEnumerable<XElement> SkipConfigAndGetChildern(this IEnumerable<XElement> source
                , Func<XElement, bool> configPredicate)
        {
            if (source.Any(configPredicate))
                return source.Elements();
            else
                return source;// source.Where(w=> w.Name.LocalName =="");//default(IEnumerable<XElement>);
        }
        public static IEnumerable<KeyValueItem> PopulateConfigItems(this IEnumerable<XElement> source
                , Func<XElement, bool> configPredicate, Func<XElement, bool> itemPredicate)
        {
            //if (source.Any())
                //return source.TakeWhile(itemPredicate).GetKeyValueItem().Concat(
                //source.SkipWhile(itemPredicate).GetNestedKeyValueItem().PopulateConfigItems(configPredicate, itemPredicate));

            return default(IEnumerable<KeyValueItem>);
        }
        public static List<KeyValueItem> GetElementAndAttributeName(this XElement source, string attributeName)
        {
            List<KeyValueItem> elemntAndAttributename = new List<KeyValueItem>();
            elemntAndAttributename.Add(new KeyValueItem
            {
                Key = "ElementType",
                Value = source.Name.LocalName
            }
            );

            if (source.HasAttributes && source.Attributes().Where(w => w.Name.ToString().ToLower() == attributeName.ToLower()).Count() > 0)
                elemntAndAttributename.Add(new KeyValueItem
                {
                    Key = "AttributeName",
                    Value = source.Attributes().Where(w => w.Name.ToString().ToLower() == attributeName.ToLower()).ToList().FirstOrDefault().Value.ToString()
                }
                );

            return elemntAndAttributename;//default(List<KeyValueItem>);
        }
        /*
         public static List<NestedCollection> LoadNestedItems(this IEnumerable<XElement> source, Func<XElement, bool> itemPredicate
            , Func<XElement, bool> configPredicate)
        {
            return source.Select(x => new NestedCollection()
            {
                ContainerName = x.Name.LocalName,
                Items = x.Elements().Elements().TakeWhile(itemPredicate).GetKeyValueItem(), // .ToList<KeyValueItem>(),
                Level = x.Name.LocalName.GetEnumValue(),// x.Level
                NestedItems = LoadNestedItems(x.Elements().Elements().SkipWhile(itemPredicate), itemPredicate, configPredicate)
            }).ToList();
        }
             */

        public static IEnumerable<NestedCollection> RecursiveSelector<NestedCollection>(this IEnumerable<NestedCollection> nodes
                , Func<NestedCollection, IEnumerable<NestedCollection>> selector)
        {
            if (nodes.Any())
                return nodes.Concat(nodes.SelectMany(selector).RecursiveSelector(selector));

            return nodes;
        }
        /*
          public static List<NestedCollection> CloneWhere(this IEnumerable<XElement> source, Func<XElement, bool> nestedPredicate
          , Func<XElement, bool> itemPredicate)
         {
             return
                 source
                     .Where(nestedPredicate)
                     .Select(x => new NestedCollection()
                     {
                         ContainerName = x.Name.LocalName,
                         //Items = x.Elements().Where(itemPredicate).ToList<KeyValueItem>(),
                         Level = x.Name.LocalName.GetEnumValue(),// x.Level
                         NestedItems = x.Elements().CloneWhere(nestedPredicate, itemPredicate)
                     })
                     //.Where(predicate)
                     //.Where(w=> w.Items.Count()>0)
                     .ToList();
         }
         */
    }
    /*
    class TreeKeyValueItem//<KeyValueItem>
    {
        //public List<TreeKeyValueItem<KeyValueItem>> Children { get; protected set; }
        public List<KeyValueItem> Children { get; protected set; }

        //public List<KeyValueItem> Item { get; protected set; }

        public TreeKeyValueItem()//KeyValueItem item)
        {
            //Item = Item ?? new List<KeyValueItem>();
            //Item.Add(item);
        }
        public List<KeyValueItem> AddItem(KeyValueItem item)
        {
            Item = Item ?? new List<KeyValueItem>();
            Item.Add(item);
            return Item;
        }

        public KeyValueItem AddChild(KeyValueItem item) //TreeKeyValueItem<KeyValueItem> AddChild(KeyValueItem item)
        {

            Children = Children ?? new List<TreeKeyValueItem<KeyValueItem>>();
            TreeKeyValueItem<KeyValueItem> nodeItem = new TreeKeyValueItem<KeyValueItem>();// (item);
            Children.Add(nodeItem);
            return nodeItem;
        }
    }
*/
    public class KeyValueItem
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
    }
    public class KeyValueLevel
    {
        public int Level { get; set; }
        public List<KeyValueItem> MatchKey { get; set; }//AllTemplates
    }
    public class NestedCollection
    {
        public List<KeyValueItem> ElementOrAttributeName { get; set; }
        public int Level { get; set; }//1
        public List<KeyValueItem> Items { get; set; }//{ {Size: 10}, {Color: Red} }
        public List<NestedCollection> NestedItems { get; set; }//AllBlocks//Name: Font1; Attributes: { {Size: 10}, {Color: Red} }
    }
    public class InstanceMain
    {

        //public Dictionary<string, List<NestedCollection>> nestedItems = null;
        public Dictionary<string, List<NestedCollection>> dataStore = null;

        /*
        internal static Dictionary<string, ComplexType> GetConfig(Dictionary<int, string> value)
        {
            Dictionary<String, ComplexType> retValue = new Dictionary<string, ComplexType>();
            //Loop the levels, override the configuration
            //<>
            return retValue;
        }
        */

        public void PopulateNestedItems()
        {
            /*
            NestedCollection item = new NestedCollection()
            {
                ContainerName = "AllTemplates",
                Level = 1,
                Items = new List<KeyValues>()
                {
                        new KeyValues()
                        {
                            KeyValueName = "Font4",
                            KeyValueItems = new Dictionary<string, string>()
                            {   { "Size", "10" },
                                { "Type", "Courier" }
                            }
                        }
                },
                NestedItems = new List<NestedCollection>()
                {
                    {   new NestedCollection()
                        {
                                  ContainerName = "AllBlocks",
                                  Level = 2,
                                  Items= new List<KeyValues>()
                                    {
                                            new KeyValues() {
                                                KeyValueName = "Font4",
                                                KeyValueItems = new Dictionary<string, string>()
                                                {   { "Size", "11" },
                                                    { "Type", "Arial" }
                                                }
                                    } },
                                  NestedItems = new List<NestedCollection>()
                        }
                    },
                        new NestedCollection()
                        {         ContainerName = "Template1",
                                    Level = 3,
                                  Items= new List<KeyValues>()
                                    {
                                            new KeyValues() {
                                                KeyValueName = "Font3",
                                                KeyValueItems = new Dictionary<string, string>()
                                                {   { "Size", "12" },
                                                    { "Type", "Courier New" }
                                                }
                                    } },
                                    NestedItems = new List<NestedCollection>()
                                      {
                                          {     new NestedCollection()
                                                {
                                                    ContainerName = "Block1",
                                                    Level = 4,
                                                    Items= new List<KeyValues>()
                                                    {
                                                            new KeyValues()
                                                            {
                                                                KeyValueName = "Font4",
                                                                KeyValueItems = new Dictionary<string, string>()
                                                                {   { "Size", "14" },
                                                                    { "Type", "MS Sans Sarif" }
                                                                }
                                                            }
                                                    },
                                                    NestedItems = new List<NestedCollection>()
                                                }
                                          },
                                          {     new NestedCollection()
                                                {
                                                    ContainerName = "Block2",
                                                    Level = 4,
                                                    Items= new List<KeyValues>()
                                                    {
                                                            new KeyValues()
                                                            {
                                                                KeyValueName = "Font4",
                                                                KeyValueItems = new Dictionary<string, string>()
                                                                {   { "Size", "15" },
                                                                    { "Type", "Sans Sarif" }
                                                                }
                                                            }
                                                    },
                                                    NestedItems = new List<NestedCollection>()
                                                }
                                        }
                                    }
                        },
                }
            };
            */

            /*nestedItems = new Dictionary<string, List<NestedCollection>>()
            {
                { "Font",  new List<NestedCollection> { item } }
                //,{ "Colors",  colorsitem}
            };

            var result = nestedItems["Font"].CloneWhere(c => c.NestedItems.Any()
                                        || c.ContainerName == "Template1" || c.ContainerName == "Block2"
                                        || c.ContainerName == "AllBlocks" || c.ContainerName == "AllTemplates"
                                    , i => i.KeyValueName == "Font4"
                );*///.RecursiveSelector(x => x.NestedItems)//.Where(w=> w.Items.Count()>0).OrderByDescending(o=>o.Level).FirstOrDefault();
                  //.OrderByDescending(o => o.Level);
        }
    }
}



namespace FormRenderLogic
{
    public interface IElement
    {
        void draw();
    }
    public class Template : IElement
    {
        public void draw()
        { throw new NotImplementedException(); }
    }

    public interface IBlock : IElement
    {
        IElement[] ElementsToDraw { get; }

    }

    public interface BlockECT : IBlock
    {
        void AddElement(IElement element);
        void RemoveElement(IElement element);
    }

    public interface IColor
    {
        int Color();
    }

    public interface IForeColor : IColor
    {
    }
    public interface IBackColor : IColor
    {
    }

    public interface IFont : IForeColor
    {
        int Width();
        int Height();
        string FontType();

    }
    public interface IRectangle : IBackColor
    {
        int Width();
        int Height();
    }
}