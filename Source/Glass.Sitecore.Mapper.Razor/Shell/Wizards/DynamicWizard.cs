﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using Sitecore;
using System.IO;
using Glass.Sitecore.Mapper.Razor.Model;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework;

namespace Glass.Sitecore.Mapper.Razor.Shell.Wizards
{
    public class DynamicWizard : AbtractFileCreateWizard<GlassDynamicRazor>
    {
        protected const string TemplateCsHtml = "/sitecore modules/shell/glass/mapper/razor/templates/DynamicRazorTemplate.cshtml.temp";

        protected override bool ActivePageChanging(string page, ref string newpage)
        {
            this.BackButton.Enabled = true;
            this.BackButton.Disabled = false;
           
            if (page == "fileNamePage" && newpage == "fileLocationPage")
            {
                if (FileName.Value.IsNullOrEmpty())
                {
                    SheerResponse.Alert("You must enter a file name");
                    return false;
                }
            }

            if (page == "fileLocationPage" && newpage == "itemLocationPage")
            {
                if (FileExists()) return false;
            }

            if (page == "itemLocationPage" && newpage == "finalPage")
            {
                if (!ItemCanWrite()) return false;
            }

            return base.ActivePageChanging(page, ref newpage);
        }

        protected override void ActivePageChanged(string page, string oldPage)
        {
            if (page == "finalPage")
            {
                this.BackButton.Disabled = true;

                CreateCsHtmlFile(new System.Collections.Specialized.NameValueCollection(), TemplateCsHtml);

                GlassDynamicRazor item = new GlassDynamicRazor();
                item.Name = FileName.Value;
                item.View = GetRelativeFilePath();

                CreateItem(item);

                global::Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren");
            }

            base.ActivePageChanged(page, oldPage);

        }
    }
}
