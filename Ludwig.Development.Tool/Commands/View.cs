using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;
using Acidmanic.Utilities.Reflection.ObjectTree.FieldAddressing;
using CoreCommandLine;
using CoreCommandLine.Attributes;
using Ludwig.DataAccess.Models;
using Meadow;

namespace Ludwig.Development.Tool.Commands
{
    [CommandName("view")]
    public class View:CommandBase
    {
        public override bool Execute(Context context, string[] args)
        {
            if (AmIPresent(args))
            {

                Output("Select Model to print view names for.");

                var models = GetModels();

                foreach (var model in models)
                {
                    Output($"{model.Key} {model.Value.Name} ({model.Value.Namespace})");
                }

                var index = Console.ReadLine() ?? "";

                if (models.ContainsKey(index))
                {
                    var type = models[index];
                    
                    var mapper = new RelationalRelationalIdentifierToStandardFieldMapper();

                    var map = mapper.MapAddressesByIdentifier(type, true);

                    Output("------------------------------------------------");


                    var table = GetTable(map);

                    Output(table);

                }
                else
                {
                    Output("You should select one of the models");
                }
                return true;
            }

            return false;
        }

        private string GetTable<T>(Dictionary<string,T> map)
        {
            var maxLength = 0;

            foreach (var key in map.Keys)
            {
                if (key.Length > maxLength)
                {
                    maxLength = key.Length;
                }
            }

            string Padd(string value, int length)
            {
                if (value == null)
                {
                    value = "";
                }
                while (value.Length < length)
                {
                    value = " " + value;
                }

                return value;
            }

            var sb = new StringBuilder();

            foreach (var item in map)
            {
                var paddedKey = Padd(item.Key, maxLength);

                sb.Append(paddedKey).Append(" - ").Append(item.Value.ToString()).Append("\n");
            }

            return sb.ToString();
        }

        private Dictionary<string, Type> GetModels()
        {
            var allTypes = typeof(Card).Assembly.GetAvailableTypes();

            var models = allTypes.Where(TypeCheck.IsModel);
            
            int index = 1;

            var indexedModelTypes = new Dictionary<string, Type>();

            foreach (var model in models)
            {
                
                indexedModelTypes.Add(index.ToString(),model);

                index += 1;
            }

            return indexedModelTypes;
        }
    }
}