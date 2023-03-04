using Newtonsoft.Json;
using Sitecore.Data.Fields;
using Sitecore.Shell.Framework.Commands;
using SXAModule.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace SXAModule.Services
{
    public class FillData : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(CommandContext context)
        {
            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
            ApiUtility apiUtility = new ApiUtility();
            string question = context.Items[0].Name;
            string output = apiUtility.GetChatMessage(question);
            foreach (Field field in context.Items[0].Fields)
            {
                if (field.TypeKey == "rich text")
                {
                    updateTextField(context, output, field);
                    break;
                }
            }
        }

        private static void updateTextField(CommandContext context, string output, Field field)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                context.Items[0].Editing.BeginEdit();
                try
                {
                    context.Items[0].Fields[field.Name].Value = output;
                    context.Items[0].Editing.EndEdit();
                }
                catch (Exception ex)
                {
                    context.Items[0].Editing.EndEdit();
                }
            }
        }
    }
}