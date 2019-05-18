﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic
{
    class MenuPanelPresenter
    {
        #region Classes
        IMenuPanelView view;
        Model model;
        #endregion

        public MenuPanelPresenter(IMenuPanelView view, Model model)
        {
            this.view = view;
            this.model = model;
        }
    }
}
