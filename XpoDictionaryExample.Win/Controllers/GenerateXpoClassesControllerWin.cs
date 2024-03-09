using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpoDictionaryExample.Module.BusinessObjects;

namespace XpoDictionaryExample.Win.Controllers
{
    public class GenerateXpoClassesControllerWin: GenerateXpoClassesController
    {
        GridView gridView;
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Obtain the List Editor: XAF's abstraction over the UI control.
            if (View.Editor is GridListEditor gridListEditor && gridListEditor.GridView != null)
            {
                
                // Access the GridView object (part of the DevExpress WinForms Grid Control architecture). 
                gridView = gridListEditor.GridView;
                
            }
        }
        protected override void RenderData(XPCollection xPCollection)
        {
            base.RenderData(xPCollection);
            gridView.GridControl.DataSource = xPCollection;
            gridView.Columns.Clear();
            gridView.PopulateColumns();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

        }
    }
}
