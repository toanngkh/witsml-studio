﻿using Caliburn.Micro;

namespace PDS.Witsml.Studio.Plugins.WitsmlBrowser.ViewModels.Result
{
    public class ResultViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public new MainViewModel Parent
        {
            get { return (MainViewModel)base.Parent; }
        }

        public Models.WitsmlSettings Model
        {
            get { return Parent.Model; }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ActivateItem(new ResponseViewModel());
            //Items.Add(new PropertyViewModel());
            //Items.Add(new RawViewModel());
        }
    }
}
