using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MyGameEngine
{
    public class XmlFormatter : IFileService
    {
        public void Save(string directory, string filename, IEnumerable<Army> armys)
        {
            var doc = new XDocument();
            var root = new XElement("Armys");
            foreach (var army in armys)
            {
                var army_node = new XElement("Army");
                army_node.Add(new XAttribute("name", army.Name));
                var stacks_node = new XElement("BattleUnitsStacks");
                foreach (var stack in army.Stacks)
                {
                    var stack_node = new XElement("BattleUnitsStack");
                    stack_node.Add(new XAttribute("name", stack.Name));
                    stack_node.Add(new XAttribute("unit", stack.Unit.Id));
                    stack_node.Add(new XAttribute("count", stack.Count));
                    stack_node.Add(new XAttribute("currentcount", stack.CurrentCount));
                    stack_node.Add(new XAttribute("currenthealth", stack.CurrentHealth));
                    var mods_node = new XElement("TempMods");
                    foreach (var mod in stack.Mods)
                    {
                        var mod_node = new XElement("TempMod");
                        mod_node.Add(new XAttribute("name", mod.Key));
                        mod_node.Add(new XAttribute("count", mod.Value));
                        mods_node.Add(mod_node);
                    }

                    stack_node.Add(mods_node);
                    stacks_node.Add(stack_node);
                }

                army_node.Add(stacks_node);
                root.Add(army_node);
            }

            doc.Add(root);
            doc.Save(directory + @"\" + filename + ".mygame");
        }

        public (Dictionary<string, Army>, Dictionary<string, BattleUnitsStack>) Open(string directory, string filename, Dictionary<string, Unit> units)
        {
            var doc = XDocument.Load(directory + @"\" + filename + ".mygame");
            var armys = new Dictionary<string, Army>();
            var stacks = new Dictionary<string, BattleUnitsStack>();
            foreach (var army_node in doc.Element("Armys").Elements("Army"))
            {
                var army_name = army_node.Attribute("name").Value;
                var army = new Army(army_name);
                foreach (var stack_node in army_node.Element("BattleUnitsStacks").Elements("BattleUnitsStack"))
                {
//                    Enum.TryParse<Units>(stack_node.Attribute("unit").Value, out var unit);
                    var unit = stack_node.Attribute("unit").Value;
                    var stack_name = stack_node.Attribute("name").Value;
                    var count = Convert.ToInt32(stack_node.Attribute("count").Value);
                    var currentcount = Convert.ToInt32(stack_node.Attribute("currentcount").Value);
                    var currenthealth = Convert.ToInt32(stack_node.Attribute("currenthealth").Value);
                    var mods = new SortedDictionary<TempMods, int>();
                    foreach (var mod_node in stack_node.Element("TempMods").Elements("TempMod"))
                    {
                        Enum.TryParse<TempMods>(mod_node.Attribute("name").Value, out var mod);
                        var mod_count = Convert.ToInt32(mod_node.Attribute("count").Value);
                        mods.Add(mod, mod_count);
                    }

                    var stack = new BattleUnitsStack(units[unit], count, stack_name, currentcount, currenthealth, mods);
                    army.Add(stack);
                    stacks.Add(stack_name, stack);
                }

                armys.Add(army_name, army);
            }

            return (armys, stacks);
        }
    }
}