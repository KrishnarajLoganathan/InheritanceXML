using ConfigCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Client
namespace POCConfigReader
{
    public enum Levels
    {
        AllTemplates=1,
        BlockTypes=2,
        Templates=3,
        BlockInstances=4//,section=5,
    }
    class Program
    {
        static InstanceMain inst = new InstanceMain();
        static void Main(string[] args)
        {
            //Load data 
            //inst.LoadData(loadData());


            new XmlConfig();

            //new InstanceMain().PopulateNestedItems();
            /*Dictionary<String, ComplexType> keyvalues=   Program.getConfig(TemplateName: "105", BlockType: "Block1", BlockInstance: "Insight");
            Console.WriteLine(keyvalues);
            */

            /*
            ComplexType main = new ComplexType()
            {
                LeafKey = "Config",
                LeafValues = new List<ComplexType>()
            };

            ComplexType c = new ComplexType()
            {
                LeafKey = "BackgroundColor",
                LeafValue = "Black"
            };
            main.LeafValues.Add(c);

            ComplexType c1 = new ComplexType()
            {
                LeafKey = "Font",
                LeafValues = new List<ComplexType>()
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
            main.LeafValues.Add(c1);

            Console.WriteLine(main.LeafValues.
            Console.ReadKey();
            */
        }

        internal static Dictionary<String, ComplexType>  getConfig(string TemplateName, string BlockType, string BlockInstance)
        {
            Dictionary<int, string> searchPattern = new Dictionary<int, string>();
            searchPattern.Add((int)Levels.Templates, TemplateName);
            searchPattern.Add((int)Levels.BlockTypes, BlockType);
            searchPattern.Add((int)Levels.BlockInstances, BlockInstance);
            //return inst.GetConfig(searchPattern);
            return null;

        }
        /*public static <> loadData ()
        {
            NestedCollection coll = new NestedCollection()
            {
                ContainerName = "AllTemplates",
                Items = LoadItems("/root/AllTemplates"),
                Level = Levels.AllTemplates
            };
            coll.NestedItems = new List<NestedCollection>()
            {
                ContainerName = "Block1",
                Items = getItems(),
                Level = Levels.BlockType
            }

        }
        */
    }

    internal class ComplexType
    {
    }
}

