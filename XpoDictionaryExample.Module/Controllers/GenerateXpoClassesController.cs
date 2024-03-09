using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraRichEdit.Model.History;
using System.ServiceModel.Channels;

namespace XpoDictionaryExample.Module.BusinessObjects
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GenerateXpoClassesController : ObjectViewController<ListView, DummyClass>
    {
        SingleChoiceAction SelectTableAction;
        protected ConnectionProviderSql sqlProvider;
        protected IDataStore xpoProvider;
        protected ReflectionDictionary reflectionDictionary;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public GenerateXpoClassesController()
        {
            InitializeComponent();
            SelectTableAction = new SingleChoiceAction(this, "SelectTableAction", "View");
            SelectTableAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            SelectTableAction.Execute += SelectTableAction_Execute;
            // Create some items
            //SelectTableAction.Items.Add(new ChoiceActionItem("MyItem1", "My Item 1", 1));
            
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void SelectTableAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var itemData = e.SelectedChoiceActionItem.Data;
            SimpleDataLayer simpleDataLayer = new SimpleDataLayer(reflectionDictionary, this.xpoProvider);
            UnitOfWork unitOfWork = new UnitOfWork(simpleDataLayer);
            var ClassInfo= unitOfWork.GetClassInfo(string.Empty, e.SelectedChoiceActionItem.Id);
            XPCollection xPCollection = new XPCollection(unitOfWork, ClassInfo);
            RenderData(xPCollection);
            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112738/).
        }
        protected virtual void RenderData(XPCollection xPCollection)
        {
            
        }
        protected override void OnActivated()
        {
            base.OnActivated();


            xpoProvider = XpoDefault.GetConnectionProvider("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=DemoBNext", AutoCreateOption.SchemaAlreadyExists);
            reflectionDictionary = new ReflectionDictionary();
            sqlProvider = xpoProvider as ConnectionProviderSql;
            var Tables = sqlProvider.GetStorageTablesList(false);

            var DbTables = sqlProvider.GetStorageTables(Tables);
            foreach (var item in DbTables)
            {
                SelectTableAction.Items.Add(new ChoiceActionItem(item.Name, item.Name, item));
                AddClass(reflectionDictionary, item);
            }



            // Perform various tasks depending on the target View.
        }
        public static XPClassInfo AddClass(XPDictionary dict, DBTable table)
        {
            if (table.PrimaryKey.Columns.Count > 1)
                throw new NotSupportedException("Compound primary keys are not supported");
            XPClassInfo classInfo = dict.CreateClass(dict.GetClassInfo(typeof(BasePersistentClass)), table.Name.Replace('.', '_'));
            classInfo.AddAttribute(new PersistentAttribute(table.Name));
            DBColumnType primaryColumnType = table.GetColumn(table.PrimaryKey.Columns[0]).ColumnType;
            classInfo.CreateMember(table.PrimaryKey.Columns[0], DBColumn.GetType(primaryColumnType),
                new KeyAttribute(IsAutoGenerationSupported(primaryColumnType)));
            foreach (DBColumn col in table.Columns)
                if (!col.IsKey)
                    classInfo.CreateMember(col.Name, DBColumn.GetType(col.ColumnType));
            return classInfo;
        }
        static bool IsAutoGenerationSupported(DBColumnType type)
        {
            return type == DBColumnType.Guid || type == DBColumnType.Int16 || type == DBColumnType.Int32;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
