using DevExpress.Blazor;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Grid;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using XpoDictionaryExample.Module.BusinessObjects;

namespace XpoDictionaryExample.Blazor.Server.Controllers
{
    public class GenerateXpoClassesControllerBlazor : GenerateXpoClassesController
    {
        DxGridListEditor _gridListEditor;
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is DxGridListEditor gridListEditor)
            {

                _gridListEditor= gridListEditor; 
                gridListEditor.GridModel.ColumnResizeMode =
                    DevExpress.Blazor.GridColumnResizeMode.ColumnsContainer;
            }
        }
        protected override void RenderData(XPCollection xPCollection)
        {
          
            //TODO implement dynamic data for the grid
            base.RenderData(xPCollection);
            _gridListEditor.Columns.Clear();
            _gridListEditor.DataSource = xPCollection;
            DevExpress.Blazor.IGrid gridInstance = _gridListEditor.GetGridAdapter().GridInstance;
            var DxGridInstance = gridInstance as DxGrid;
           
            var GridType=gridInstance.GetType();
            DxGridInstance.BeginUpdate();

            Type elementType = xPCollection.ObjectClassInfo.ClassType;

            // Get the generic type definition for List<>
            Type listType = typeof(GridDevExtremeDataSource<>);

            // Use MakeGenericType to create the specific type List<string>
            Type constructedListType = listType.MakeGenericType(elementType);


            DxGridInstance.Data = Activator.CreateInstance(constructedListType, xPCollection as IQueryable);
         
            DxGridInstance.EndUpdate();
            //foreach (XPMemberInfo Property in xPCollection.ObjectClassInfo.PersistentProperties)
            //{


            //}
            //_gridListEditor.AddColumn()
            _gridListEditor.Refresh();
        }
    }
}
